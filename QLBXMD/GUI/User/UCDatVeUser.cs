using QLBXMD.BUS;
using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json; // ? Thêm using

namespace QLBXMD.GUI.User
{
	public partial class UCDatVeUser : UserControl
	{
		#region Fields
		private int? selectedMaChuyen;
		private int? selectedMaXe;
		private int? selectedMaLoaiXe;
		private decimal? selectedGiaVe;
		private List<int> selectedSeats = new List<int>();
		private List<int> bookedSeats = new List<int>();
		private string selectedDiemDi;
		private string selectedDiemDen;
		private DateTime selectedNgayDi;
		private TaiKhoan currentUser;
		private bool isLoadingData = false; // Flag ð? tránh recursive calls
		#endregion

		#region Constructor
		public UCDatVeUser()
		{
			InitializeComponent();
			this.Load += UCDatVeUser_Load;
		}

		public UCDatVeUser(TaiKhoan user) : this()
		{
			this.currentUser = user;
		}
		#endregion

		#region Load Events
		private void UCDatVeUser_Load(object sender, EventArgs e)
		{
			isLoadingData = true; // B?t ð?u load
			
			LoadDiemDiDiemDen();
			dtpNgayDi.Value = DateTime.Now;
			cboGioDi.SelectedIndex = 0; // Ch?n "T?t c?"
			
			// Thêm event handlers ð? t? ð?ng t?m chuy?n
			cboDiemDi.SelectedIndexChanged += CboDiemDi_SelectedIndexChanged;
			cboDiemDen.SelectedIndexChanged += CboDiemDen_SelectedIndexChanged;
			dtpNgayDi.ValueChanged += DtpNgayDi_ValueChanged;
			cboGioDi.SelectedIndexChanged += CboGioDi_SelectedIndexChanged;
			
			isLoadingData = false; // K?t thúc load
			
			// T?i chuy?n xe ngay khi vào
			LoadChuyenXeAuto();
		}

		private void CboDiemDi_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!isLoadingData) LoadChuyenXeAuto();
		}

		private void CboDiemDen_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!isLoadingData) LoadChuyenXeAuto();
		}

		private void DtpNgayDi_ValueChanged(object sender, EventArgs e)
		{
			if (!isLoadingData) LoadChuyenXeAuto();
		}

		private void CboGioDi_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!isLoadingData) LoadChuyenXeAuto();
		}

		private void LoadDiemDiDiemDen()
		{
			try
			{
				using (var db = new BXMDEntities())
				{
					var diemDi = db.TuyenDuongs
						.Select(t => t.DiemDi)
						.Distinct()
						.OrderBy(d => d)
						.ToList();

					var diemDen = db.TuyenDuongs
						.Select(t => t.DiemDen)
						.Distinct()
						.OrderBy(d => d)
						.ToList();

					// Thêm placeholder vào ð?u danh sách
					var diemDiList = new List<string>();
					diemDiList.Add("-- Ch?n ði?m ði --");
					diemDiList.AddRange(diemDi);

					var diemDenList = new List<string>();
					diemDenList.Add("-- Ch?n ði?m ð?n --");
					diemDenList.AddRange(diemDen);

					cboDiemDi.DataSource = diemDiList;
					cboDiemDen.DataSource = diemDenList;

					// Ch?n placeholder (index 0)
					cboDiemDi.SelectedIndex = 0;
					cboDiemDen.SelectedIndex = 0;
					
					System.Diagnostics.Debug.WriteLine($"Ð? t?i {diemDi.Count} ði?m ði và {diemDen.Count} ði?m ð?n");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("L?i t?i d? li?u: " + ex.Message, "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		#endregion

		#region Bý?c 1: T?m chuy?n xe
		
		private void LoadChuyenXeAuto()
		{
			// L?y giá tr? hi?n t?i
			string diemDi = cboDiemDi.SelectedItem?.ToString();
			string diemDen = cboDiemDen.SelectedItem?.ToString();
			
			// B? qua placeholder
			if (diemDi != null && diemDi.StartsWith("--")) diemDi = null;
			if (diemDen != null && diemDen.StartsWith("--")) diemDen = null;
			
			// N?u c? hai ð?u tr?ng, hi?n th? t?t c? chuy?n xe
			if (string.IsNullOrEmpty(diemDi) && string.IsNullOrEmpty(diemDen))
			{
				LoadChuyenXe(null, null);
				return;
			}
			
			// N?u ch? có m?t trong hai
			LoadChuyenXe(diemDi, diemDen);
		}
		
		private void btnTimChuyen_Click(object sender, EventArgs e)
		{
			// Gi? l?i ð? týõng thích, nhýng gi? ch? g?i LoadChuyenXeAuto
			LoadChuyenXeAuto();
		}

		private void LoadChuyenXe(string diemDi, string diemDen)
		{
			try
			{
				selectedNgayDi = dtpNgayDi.Value.Date;
				
				using (var db = new BXMDEntities())
				{
					// L?y t?t c? chuy?n xe trý?c
					var query = (from cx in db.ChuyenXes
								join x in db.Xes on cx.MaXe equals x.MaXe
								join nx in db.NhaXes on x.MaNX equals nx.MaNX
								join lx in db.LoaiXes on x.MaLX equals lx.MaLX
								join td in db.TuyenDuongs on cx.MaTuyen equals td.MaTuyen
								where System.Data.Entity.DbFunctions.TruncateTime(cx.NgayGioKhoiHanh) == selectedNgayDi
									  && cx.GiaVeThucTe.HasValue && cx.GiaVeThucTe > 0
								select new
								{
									MaChuyen = cx.MaChuyen,
									MaXe = x.MaXe,
									MaLoaiXe = lx.MaLX,
									KhoiHanh = cx.NgayGioKhoiHanh,
									DenDuKien = cx.NgayGioDenDuKien,
									TenNX = nx.TenNX,
									TenLX = lx.TenLX,
									SoCho = lx.SoCho,
									GiaVe = cx.GiaVeThucTe.Value,
									SoChoTrong = lx.SoCho - db.VeXes.Count(v => v.MaChuyen == cx.MaChuyen),
									DiemDi = td.DiemDi,
									DiemDen = td.DiemDen
								}).ToList();

					// Debug: Ki?m tra s? lý?ng chuy?n ban ð?u
					System.Diagnostics.Debug.WriteLine($"T?m th?y {query.Count} chuy?n xe ngày {selectedNgayDi:dd/MM/yyyy}");

					// L?c theo ði?m ði n?u có
					if (!string.IsNullOrEmpty(diemDi))
					{
						query = query.Where(r => r.DiemDi == diemDi).ToList();
						System.Diagnostics.Debug.WriteLine($"Sau khi l?c ði?m ði '{diemDi}': {query.Count} chuy?n");
					}
					
					// L?c theo ði?m ð?n n?u có
					if (!string.IsNullOrEmpty(diemDen))
					{
						query = query.Where(r => r.DiemDen == diemDen).ToList();
						System.Diagnostics.Debug.WriteLine($"Sau khi l?c ði?m ð?n '{diemDen}': {query.Count} chuy?n");
					}

					// L?c theo gi? n?u không ch?n "T?t c?"
					if (cboGioDi.SelectedIndex > 0)
					{
						int gioIndex = cboGioDi.SelectedIndex;
						query = query.Where(r =>
						{
							int gio = r.KhoiHanh.Hour;
							switch (gioIndex)
							{
								case 1: return gio >= 0 && gio < 6;   // 00:00 - 06:00
								case 2: return gio >= 6 && gio < 12;  // 06:00 - 12:00
								case 3: return gio >= 12 && gio < 18; // 12:00 - 18:00
								case 4: return gio >= 18 && gio < 24; // 18:00 - 24:00
								default: return true;
							}
						}).ToList();
						System.Diagnostics.Debug.WriteLine($"Sau khi l?c gi?: {query.Count} chuy?n");
					}

					if (!query.Any())
					{
						// Không hi?n thông báo n?a, ch? ?n panel
						System.Diagnostics.Debug.WriteLine("Không có chuy?n xe phù h?p");
						pnlResults.Visible = false;
						return;
					}

					// Lýu thông tin ð? dùng sau
					if (!string.IsNullOrEmpty(diemDi)) selectedDiemDi = diemDi;
					if (!string.IsNullOrEmpty(diemDen)) selectedDiemDen = diemDen;

					// Hi?n th? panel trý?c ð? tính toán layout ðúng
					pnlResults.Visible = true;
					
					// Ð?m b?o layout ðý?c update
					pnlResults.PerformLayout();
					flpChuyenXe.PerformLayout();
					Application.DoEvents();

					// Clear và add cards
					flpChuyenXe.Controls.Clear();
					flpChuyenXe.SuspendLayout(); // T?m d?ng layout updates
					
					foreach (var item in query)
					{
						var card = CreateChuyenXeCard(item.MaChuyen, item.MaXe, item.MaLoaiXe, 
							item.KhoiHanh, item.DenDuKien ?? item.KhoiHanh.AddHours(6), 
							item.TenNX, item.TenLX, item.SoCho, item.SoChoTrong, item.GiaVe,
							item.DiemDi, item.DiemDen);
						flpChuyenXe.Controls.Add(card);
					}
					
					flpChuyenXe.ResumeLayout(); // Resume layout updates
					System.Diagnostics.Debug.WriteLine($"Ð? hi?n th? {query.Count} chuy?n xe");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("L?i t?i danh sách chuy?n xe: " + ex.Message + "\n\nStack trace: " + ex.StackTrace, "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
				System.Diagnostics.Debug.WriteLine($"L?i: {ex.Message}\n{ex.StackTrace}");
			}
		}

		private Panel CreateChuyenXeCard(int maChuyen, int maXe, int maLoaiXe, 
			DateTime khoiHanh, DateTime denDuKien, string tenNX, string tenLX, 
			int soCho, int soChoTrong, decimal giaVe, string diemDi, string diemDen)
		{
			// Dynamic width based on container - leave room for scrollbar
			int cardWidth = flpChuyenXe.Width - 40;
			if (cardWidth < 800) cardWidth = 800; // Minimum width
			
			Panel card = new Panel
			{
				Width = cardWidth,
				Height = 100, // Gi?m t? 140 xu?ng 100
				BorderStyle = BorderStyle.FixedSingle,
				BackColor = Color.White,
				Margin = new Padding(5, 5, 5, 10)
			};

			// Logo xe - nh? hõn
			Label lblBusIcon = new Label
			{
				Text = "??",
				Location = new Point(10, 10),
				Size = new Size(40, 40), // Gi?m t? 50x50 xu?ng 40x40
				Font = new Font("Segoe UI", 20F), // Gi?m t? 24F xu?ng 20F
				TextAlign = ContentAlignment.MiddleCenter,
				BackColor = Color.FromArgb(240, 240, 240)
			};

			// Tên nhà xe
			Label lblTenNX = new Label
			{
				Text = tenNX,
				Location = new Point(60, 10),
				AutoSize = false,
				Width = 250,
				Height = 22,
				Font = new Font("Segoe UI", 10F, FontStyle.Bold), // Gi?m t? 11F xu?ng 10F
				ForeColor = Color.FromArgb(51, 51, 51)
			};

			// Lo?i xe
			Label lblLoaiXe = new Label
			{
				Text = $"{tenLX} - {soCho} ch?",
				Location = new Point(60, 32),
				AutoSize = false,
				Width = 200,
				Height = 16,
				Font = new Font("Segoe UI", 8F), // Gi?m t? 8.5F xu?ng 8F
				ForeColor = Color.Gray
			};

			// Tuy?n ðý?ng: Ði?m ði ? Ði?m ð?n
			Label lblTuyen = new Label
			{
				Text = $"{diemDi} ? {diemDen}",
				Location = new Point(60, 50),
				AutoSize = false,
				Width = 350,
				Height = 18,
				Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), // Gi?m t? 9F xu?ng 8.5F
				ForeColor = Color.FromArgb(0, 150, 136)
			};

			// Gi? ði
			Label lblGioDi = new Label
			{
				Text = khoiHanh.ToString("HH:mm"),
				Location = new Point(60, 70),
				AutoSize = false,
				Width = 55,
				Height = 22,
				Font = new Font("Segoe UI", 12F, FontStyle.Bold), // Gi?m t? 13F xu?ng 12F
				ForeColor = Color.FromArgb(0, 150, 136)
			};

			Label lblMuiTen = new Label
			{
				Text = "?",
				Location = new Point(120, 70),
				AutoSize = false,
				Width = 20,
				Height = 22,
				Font = new Font("Segoe UI", 11F), // Gi?m t? 12F xu?ng 11F
				ForeColor = Color.Gray,
				TextAlign = ContentAlignment.MiddleCenter
			};

			// Gi? ð?n
			Label lblGioDen = new Label
			{
				Text = denDuKien.ToString("HH:mm"),
				Location = new Point(145, 70),
				AutoSize = false,
				Width = 55,
				Height = 22,
				Font = new Font("Segoe UI", 12F, FontStyle.Bold), // Gi?m t? 13F xu?ng 12F
				ForeColor = Color.FromArgb(0, 150, 136)
			};

			// Ngày ði
			Label lblNgayDi = new Label
			{
				Text = khoiHanh.ToString("dd/MM/yyyy"),
				Location = new Point(210, 73),
				AutoSize = false,
				Width = 85,
				Height = 16,
				Font = new Font("Segoe UI", 8F), // Gi?m t? 8.5F xu?ng 8F
				ForeColor = Color.Gray
			};

			// S? ch? tr?ng
			Label lblChoTrong = new Label
			{
				Text = soChoTrong > 0 ? $"C?n {soChoTrong} ch?" : "H?t ch?",
				Location = new Point(310, 73),
				AutoSize = false,
				Width = 100,
				Height = 18,
				Font = new Font("Segoe UI", 8F), // Gi?m t? 8.5F xu?ng 8F
				ForeColor = soChoTrong > 0 ? Color.Green : Color.Red
			};

			// Giá vé (position from right)
			Label lblGiaVe = new Label
			{
				Text = $"{giaVe:#,##0} VNÐ",
				Location = new Point(cardWidth - 260, 25),
				AutoSize = false,
				Width = 140,
				Height = 28,
				Font = new Font("Segoe UI", 12F, FontStyle.Bold), // Gi?m t? 13F xu?ng 12F
				ForeColor = Color.FromArgb(244, 67, 54),
				TextAlign = ContentAlignment.MiddleRight
			};

			// Button ch?n (position from right)
			Button btnChon = new Button
			{
				Text = "Ch?n chuy?n",
				Location = new Point(cardWidth - 145, 60),
				Size = new Size(125, 32), // Gi?m t? 135x35 xu?ng 125x32
				BackColor = Color.FromArgb(0, 150, 136),
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
				Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), // Gi?m t? 10F xu?ng 9.5F
				Cursor = Cursors.Hand,
				Enabled = soChoTrong > 0,
				TabStop = true,
				UseVisualStyleBackColor = false
			};
			btnChon.FlatAppearance.BorderSize = 0;
			
			// Capture variables
			int capturedMaChuyen = maChuyen;
			int capturedMaXe = maXe;
			int capturedMaLoaiXe = maLoaiXe;
			decimal capturedGiaVe = giaVe;
			string capturedDiemDi = diemDi;
			string capturedDiemDen = diemDen;
			
			btnChon.Click += (s, e) => {
				try
				{
					selectedDiemDi = capturedDiemDi;
					selectedDiemDen = capturedDiemDen;
					ChonChuyenXe(capturedMaChuyen, capturedMaXe, capturedMaLoaiXe, capturedGiaVe);
				}
				catch (Exception ex)
				{
					MessageBox.Show($"L?i ch?n chuy?n: {ex.Message}", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			};

			// Add controls
			card.Controls.Add(lblBusIcon);
			card.Controls.Add(lblTenNX);
			card.Controls.Add(lblLoaiXe);
			card.Controls.Add(lblTuyen);
			card.Controls.Add(lblGioDi);
			card.Controls.Add(lblMuiTen);
			card.Controls.Add(lblGioDen);
			card.Controls.Add(lblNgayDi);
			card.Controls.Add(lblChoTrong);
			card.Controls.Add(lblGiaVe);
			card.Controls.Add(btnChon);

			btnChon.BringToFront();

			// Hover effect
			card.MouseEnter += (s, e) => {
				if (card.ClientRectangle.Contains(card.PointToClient(Cursor.Position)))
				{
					Point mousePos = card.PointToClient(Cursor.Position);
					if (!btnChon.Bounds.Contains(mousePos))
					{
						card.BackColor = Color.FromArgb(250, 250, 250);
					}
				}
			};
			card.MouseLeave += (s, e) => card.BackColor = Color.White;

			return card;
		}
		#endregion

		#region Bý?c 2: Ch?n chuy?n xe và hi?n th? sõ ð? gh?
		private void ChonChuyenXe(int maChuyen, int maXe, int maLoaiXe, decimal giaVe)
		{
			selectedMaChuyen = maChuyen;
			selectedMaXe = maXe;
			selectedMaLoaiXe = maLoaiXe;
			selectedGiaVe = giaVe;

			LoadSeatMap();
			
			// Không hi?n pnlSeatInfo ngay, ch? hi?n khi ch?n gh?
			pnlSeatInfo.Visible = true;
			UpdateThongTinGhe();
		}

		private void LoadSeatMap()
		{
			try
			{
				using (var db = new BXMDEntities())
				{
					var loaiXe = db.LoaiXes.FirstOrDefault(lx => lx.MaLX == selectedMaLoaiXe);
					if (loaiXe == null)
					{
						MessageBox.Show($"Không t?m th?y lo?i xe v?i MaLX = {selectedMaLoaiXe}", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					int soCho = loaiXe.SoCho;
					if (soCho == 0)
					{
						MessageBox.Show("Lo?i xe có s? ch? = 0", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					bool isGiuongNam = loaiXe.TenLX.ToLower().Contains("giý?ng");

					// L?y danh sách gh? ð? ð?t
					bookedSeats = db.VeXes
						.Where(v => v.MaChuyen == selectedMaChuyen)
						.Select(v => v.ChoNgoi)
						.ToList();

					selectedSeats.Clear();

					// Clear panels
					flpTangDuoi.Controls.Clear();
					flpTangTren.Controls.Clear();

					if (isGiuongNam)
					{
						// Xe giý?ng n?m: 2 t?ng
						lblLoaiGhe.Text = "Sõ ð? giý?ng n?m";
						gbTangDuoi.Visible = true;
						gbTangTren.Visible = true;
						gbTangDuoi.Text = "T?ng dý?i";
						gbTangTren.Text = "T?ng trên";

						int choMoiTang = soCho / 2;
						CreateSeatButtons(flpTangDuoi, 1, choMoiTang);
						CreateSeatButtons(flpTangTren, choMoiTang + 1, soCho);
					}
					else
					{
						// Xe gh? ng?i: 1 t?ng
						lblLoaiGhe.Text = "Sõ ð? gh? ng?i";
						gbTangDuoi.Visible = true;
						gbTangTren.Visible = false;
						gbTangDuoi.Text = "Sõ ð? gh?";

						CreateSeatButtons(flpTangDuoi, 1, soCho);
					}

					pnlSeatMap.Visible = true;
					UpdateThongTinGhe();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"L?i t?i sõ ð? gh?:\n{ex.Message}", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void CreateSeatButtons(FlowLayoutPanel panel, int startSeat, int endSeat)
		{
			panel.SuspendLayout();

			for (int i = startSeat; i <= endSeat; i++)
			{
				Button btnSeat = new Button
				{
					Name = $"btnGhe_{i}",
					Text = i.ToString(),
					Width = 50,
					Height = 50,
					Margin = new Padding(5),
					FlatStyle = FlatStyle.Flat,
					Font = new Font("Segoe UI", 9F, FontStyle.Bold),
					Tag = i,
					Cursor = Cursors.Hand
				};

				btnSeat.FlatAppearance.BorderSize = 1;
				btnSeat.FlatAppearance.BorderColor = Color.Gray;

				if (bookedSeats.Contains(i))
				{
					// Gh? ð? ð?t
					btnSeat.BackColor = Color.FromArgb(220, 220, 220);
					btnSeat.ForeColor = Color.Gray;
					btnSeat.Enabled = false;
					btnSeat.Cursor = Cursors.No;
				}
				else
				{
					// Gh? tr?ng
					btnSeat.BackColor = Color.FromArgb(0, 199, 174); // Xanh - tr?ng
					btnSeat.ForeColor = Color.White;
					btnSeat.Click += BtnSeat_Click;
				}

				panel.Controls.Add(btnSeat);
			}

			panel.ResumeLayout();
			panel.PerformLayout();
		}

		private void BtnSeat_Click(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			int seatNumber = (int)btn.Tag;

			if (selectedSeats.Contains(seatNumber))
			{
				// B? ch?n - v? màu xanh lá (tr?ng)
				selectedSeats.Remove(seatNumber);
				btn.BackColor = Color.FromArgb(0, 199, 174);
				btn.ForeColor = Color.White;
			}
			else
			{
				// Ch?n gh? - màu cam (ðang ch?n)
				selectedSeats.Add(seatNumber);
				btn.BackColor = Color.FromArgb(245, 128, 37);
				btn.ForeColor = Color.White;
			}

			UpdateThongTinGhe();
			

		}

		private void UpdateThongTinGhe()
		{
			if (selectedSeats.Count > 0)
			{
				lblGheDaChon.Text = $"Gh? ð? ch?n: {string.Join(", ", selectedSeats.OrderBy(s => s))}";
				decimal tongTien = selectedSeats.Count * (selectedGiaVe ?? 0);
				lblTongTien.Text = $"T?ng ti?n: {tongTien:#,##0} VNÐ";
				btnDatVe.Enabled = true;
			}
			else
			{
				lblGheDaChon.Text = "Gh? ð? ch?n: Chýa ch?n";
				lblTongTien.Text = "T?ng ti?n: 0 VNÐ";
				btnDatVe.Enabled = false;
			}
		}
		#endregion

		#region Bý?c 3: Ð?t vé
		private void btnDatVe_Click(object sender, EventArgs e)
		{
			if (!selectedMaChuyen.HasValue || selectedSeats.Count == 0)
			{
				MessageBox.Show("Vui l?ng ch?n chuy?n xe và gh?.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// L?y thông tin khách hàng t? tài kho?n
			if (currentUser == null)
			{
				MessageBox.Show("Vui l?ng ðãng nh?p ð? ð?t vé.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			try
			{
				// Tính t?ng ti?n trý?c
				decimal tienTam = selectedSeats.Count * (selectedGiaVe ?? 0);
				DateTime ngayKhoiHanh = DateTime.Now;
				int maHD = 0;

				using (var db = new BXMDEntities())
				{
					var chuyenXe = db.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == selectedMaChuyen);
					if (chuyenXe != null)
					{
						ngayKhoiHanh = chuyenXe.NgayGioKhoiHanh;
					}

					// L?y thông tin khách hàng
					var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.MaTK == currentUser.MaTK);
					if (khachHang == null)
					{
						MessageBox.Show("Không t?m th?y thông tin khách hàng.", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					// ? Hi?n th? dialog xác nh?n
					using (var confirmDialog = new XacNhanDatVeUserDialog())
					{
						confirmDialog.TenKhachHang = khachHang.TenKH;
						confirmDialog.SoDienThoai = khachHang.SDT;
						confirmDialog.GioiTinh = khachHang.GioiTinh.HasValue && khachHang.GioiTinh.Value ? "Nam" : "N?";
						confirmDialog.NamSinh = khachHang.NamSinh;
						confirmDialog.DiemDi = selectedDiemDi;
						confirmDialog.DiemDen = selectedDiemDen;
						confirmDialog.NgayKhoiHanh = ngayKhoiHanh;
						confirmDialog.GheDaChon = string.Join(", ", selectedSeats.OrderBy(s => s));
						confirmDialog.GiaVe = selectedGiaVe.Value;
						confirmDialog.SoLuongGhe = selectedSeats.Count;
						confirmDialog.TongTien = tienTam;

						if (confirmDialog.ShowDialog() != DialogResult.OK)
							return;
					}

					// ? T?O HÓA ÐÕN (TongTien = 0, ð? trigger tính)
					var hoaDon = new HoaDon
					{
						MaKH = khachHang.MaKH,
						MaNV = null,
						NgayLap = DateTime.Now,
						TongTien = 0, // ? Ð? trigger t? tính
						TrangThai = false // Chýa thanh toán
					};
					db.HoaDons.Add(hoaDon);
					db.SaveChanges();

					maHD = hoaDon.MaHD;
					Console.WriteLine($"? T?o hóa ðõn: MaHD = {maHD}, TongTien ban ð?u = {hoaDon.TongTien}");

					// ? T?O VÉ XE (v?i GiaVe)
					foreach (int soGhe in selectedSeats)
					{
						var ve = new VeXe
						{
							MaChuyen = selectedMaChuyen.Value,
							ChoNgoi = soGhe,
							MaHD = maHD,
							GiaVe = selectedGiaVe.Value // ? Lýu giá vé
						};
						db.VeXes.Add(ve);
					}
					db.SaveChanges();
					Console.WriteLine($"? Ð? t?o {selectedSeats.Count} vé xe");

					// ? REFRESH ð? l?y TongTien ð? ðý?c trigger tính
					db.Entry(hoaDon).Reload();
					Console.WriteLine($"? Sau khi refresh: TongTien = {hoaDon.TongTien}");

					// ? X? l? thanh toán MoMo (v?i TongTien ð? ðúng)
					ProcessMoMoPayment(hoaDon.TongTien ?? tienTam, khachHang.MaKH, maHD);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"L?i ð?t vé: {ex.Message}", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Console.WriteLine($"? Error: {ex.ToString()}");
			}
		}

		/// <summary>
		/// X? l? thanh toán MoMo
		/// </summary>
		private async void ProcessMoMoPayment(decimal amount, int maKH, int maHD)
		{
			try
			{
				// Hi?n th? loading
				this.Cursor = Cursors.WaitCursor;

				// T?o orderId unique
				string orderId = $"HD{maHD}_{DateTime.Now:yyyyMMddHHmmss}";
				string orderInfo = $"Thanh toan ve xe - Ma HD: {maHD}";

				// T?o request thanh toán MoMo
				var momoService = new Services.MoMoPaymentService();
				var response = await momoService.CreatePaymentRequest(
					orderId,
					(long)amount,
					orderInfo
				);

				this.Cursor = Cursors.Default;

				if (response.ResultCode == 0)
				{
					// Thành công - M? tr?nh duy?t v?i PayUrl
					MessageBox.Show(
						"Ðang chuy?n ð?n trang thanh toán MoMo...\n\n" +
						"Vui l?ng hoàn t?t thanh toán trong tr?nh duy?t.\n" +
						$"M? ðõn hàng: {orderId}",
						"Thanh toán MoMo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);

					System.Diagnostics.Process.Start(response.PayUrl);
					
					// ? B?t ð?u polling ð? check tr?ng thái
					StartPollingPaymentStatus(orderId, maHD, amount);
				}
				else
				{
					// L?i - Xóa hóa ðõn và vé ð? t?o
					RollbackHoaDon(maHD);
					
					MessageBox.Show(
						$"L?i t?o yêu c?u thanh toán MoMo:\n\n" +
						$"M? l?i: {response.ResultCode}\n" +
						$"Thông báo: {response.Message}\n\n" +
						$"Vui l?ng th? l?i ho?c liên h? qu?n tr? viên.",
						"L?i thanh toán",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
					);
				}
			}
			catch (Exception ex)
			{
				this.Cursor = Cursors.Default;
				MessageBox.Show(
					$"L?i k?t n?i v?i MoMo:\n{ex.Message}\n\n" +
					$"Vui l?ng ki?m tra k?t n?i internet và th? l?i.",
					"L?i",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				Console.WriteLine($"? MoMo Payment Error: {ex.ToString()}");
			}
		}
		/// <summary>
		/// ? Polling MoMo ð? ki?m tra tr?ng thái thanh toán (không c?n IPN)
		/// </summary>
		private async void StartPollingPaymentStatus(string orderId, int maHD, decimal amount)
		{
			int attempt = 0;
			const int maxAttempts = 24; // 24 l?n * 5s = 2 phút
			const int delayMs = 5000;

			while (attempt < maxAttempts)
			{
				attempt++;
				await Task.Delay(delayMs);
				
				string status = await QueryMomoOrder(orderId);

				if (status == "Success")
				{
					MessageBox.Show(
						"? Thanh toán MoMo thành công!\n\n" +
						$"M? hóa ðõn: {maHD}\n" +
						$"S? ti?n: {amount:#,##0} VNÐ",
						"Thành công",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);
					
					UpdateHoaDonTrangThai(maHD, true, "MoMo");
					
					// ? Reset form sau khi thành công
					ResetFormAfterPayment();
					return;
				}
				else if (status == "Failed")
				{
					MessageBox.Show(
						"? Thanh toán MoMo th?t b?i ho?c b? h?y!\n\n" +
						"Hóa ðõn và vé ð? ðý?c h?y.",
						"Th?t b?i",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
					);
					
					RollbackHoaDon(maHD);
					return;
				}

				// Log progress
				Console.WriteLine($"[MoMo Polling] Attempt {attempt}/{maxAttempts} - Status: {status}");
			}

			// Timeout
			MessageBox.Show(
				"? H?t th?i gian ch? thanh toán MoMo.\n\n" +
				"Vui l?ng ki?m tra l?i tr?ng thái ðõn hàng trong L?ch s? mua vé.",
				"Timeout",
				MessageBoxButtons.OK,
				MessageBoxIcon.Warning
			);
		}

		/// <summary>
		/// ? G?i API Query c?a MoMo ð? xác minh ðõn hàng
		/// </summary>
		private async Task<string> QueryMomoOrder(string orderId)
		{
			const string ApiQuery = "https://test-payment.momo.vn/v2/gateway/api/query";
			const string PartnerCode = "MOMO4LG620250601_TEST";
			const string AccessKey = "e6g7W44vmy1rgT7K";
			const string SecretKey = "XAAT9fHRPeJD8zhjKsZnTTUSnYEHFISa";

			try
			{
				string requestId = Guid.NewGuid().ToString();
				string rawSignature =
					$"accessKey={AccessKey}" +
					$"&orderId={orderId}" +
					$"&partnerCode={PartnerCode}" +
					$"&requestId={requestId}";

				string signature = ComputeHmacSha256(rawSignature, SecretKey);

				var body = new
				{
					partnerCode = PartnerCode,
					requestId,
					orderId,
					signature,
					lang = "vi"
				};

				using (var httpClient = new HttpClient())
				{
					var json = JsonConvert.SerializeObject(body);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					var response = await httpClient.PostAsync(ApiQuery, content);
					string responseJson = await response.Content.ReadAsStringAsync();

					dynamic data = JsonConvert.DeserializeObject(responseJson);
					int resultCode = data.resultCode;
					string message = data.message ?? "";

					Console.WriteLine($"[MoMo Query] {orderId} => Code: {resultCode}, Message: {message}");

					// ? MoMo result codes:
					// 0 = Success (Giao d?ch thành công)
					// 1000 = Transaction initiated (Ðang ch? user xác nh?n)
					// 1006 = Transaction not found (Chýa có trong h? th?ng)
					// 1007 = Transaction pending (Ðang x? l?)
					// 9000 = Transaction failed/cancelled (Th?t b?i/h?y)
					// Other = Error
					
					if (resultCode == 0)
					{
						return "Success";
					}
					else if (resultCode == 1000 || resultCode == 1006 || resultCode == 1007)
					{
						// ? 1000 = Ðang ch? user thanh toán ? Pending
						return "Pending";
					}
					else if (resultCode == 9000 || resultCode == 1001)
					{
						// 9000 = User h?y giao d?ch
						// 1001 = Giao d?ch th?t b?i
						return "Failed";
					}
					else
					{
						// Unknown code ? Retry
						Console.WriteLine($"?? Unknown MoMo code: {resultCode}");
						return "Pending";
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("? QueryMoMoOrder Error: " + ex.Message);
				return "Pending"; // Retry on error
			}
		}

		/// <summary>
		/// ? C?p nh?t tr?ng thái hóa ðõn khi có k?t qu? thanh toán
		/// </summary>
		private void UpdateHoaDonTrangThai(int maHD, bool daThanhToan, string phuongThuc)
		{
			try
			{
				using (var db = new BXMDEntities())
				{
					var hoaDon = db.HoaDons.FirstOrDefault(h => h.MaHD == maHD);
					if (hoaDon == null)
					{
						Console.WriteLine($"? Không t?m th?y hóa ðõn #{maHD}");
						return;
					}

					hoaDon.TrangThai = daThanhToan;

					if (daThanhToan)
					{
						// T?o b?n ghi thanh toán
						var thanhToan = new ThanhToan
						{
							MaHD = maHD,
							TongTien = hoaDon.TongTien ?? 0,
							NgayTT = DateTime.Now,
							PhuongThuc = phuongThuc
						};
						db.ThanhToans.Add(thanhToan);
						
						Console.WriteLine($"? Hóa ðõn #{maHD} ð? thanh toán qua {phuongThuc}");
					}

					db.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"L?i c?p nh?t hóa ðõn: {ex.Message}", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Console.WriteLine($"? UpdateHoaDonTrangThai Error: {ex.Message}");
			}
		}

		/// <summary>
		/// ? Rollback hóa ðõn khi thanh toán th?t b?i
		/// </summary>
		private void RollbackHoaDon(int maHD)
		{
			try
			{
				using (var db = new BXMDEntities())
				{
					// Xóa vé
					var veXes = db.VeXes.Where(v => v.MaHD == maHD).ToList();
					db.VeXes.RemoveRange(veXes);

					// Xóa hóa ðõn
					var hoaDon = db.HoaDons.FirstOrDefault(h => h.MaHD == maHD);
					if (hoaDon != null)
					{
						db.HoaDons.Remove(hoaDon);
					}

					db.SaveChanges();
					Console.WriteLine($"? Ð? rollback hóa ðõn #{maHD}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"? RollbackHoaDon Error: {ex.Message}");
			}
		}

		/// <summary>
		/// ? Reset form sau khi thanh toán thành công
		/// </summary>
		private void ResetFormAfterPayment()
		{
			try
			{
				ResetForm();
				
				// Reload l?i danh sách chuy?n xe
				LoadChuyenXeAuto();
				
				MessageBox.Show(
					"?? Ð?t vé thành công!\n\n" +
					"B?n có th? xem chi ti?t trong m?c 'L?ch s? mua vé'.",
					"Thành công",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"? ResetFormAfterPayment Error: {ex.Message}");
			}
		}
		#endregion

		#region Reset form
		private void ResetForm()
		{
			selectedMaChuyen = null;
			selectedMaXe = null;
			selectedMaLoaiXe = null;
			selectedGiaVe = null;
			selectedSeats.Clear();
			bookedSeats.Clear();

			pnlResults.Visible = false;
			pnlSeatMap.Visible = false;
			pnlSeatInfo.Visible = false;

			lblGheDaChon.Text = "Gh? ð? ch?n: Chýa ch?n";
			lblTongTien.Text = "T?ng ti?n: 0 VNÐ";
			btnDatVe.Enabled = false;
		}
		#endregion

		#region Swap ð?a ði?m
		private void btnSwap_Click(object sender, EventArgs e)
		{
			if (cboDiemDi.SelectedItem != null && cboDiemDen.SelectedItem != null)
			{
				string diemDi = cboDiemDi.SelectedItem.ToString();
				string diemDen = cboDiemDen.SelectedItem.ToString();

				// Swap c? khi có placeholder (không c?n ki?m tra)
				var temp = cboDiemDi.SelectedItem;
				cboDiemDi.SelectedItem = cboDiemDen.SelectedItem;
				cboDiemDen.SelectedItem = temp;
				
				// LoadChuyenXeAuto s? ðý?c g?i t? ð?ng qua SelectedIndexChanged event
			}
		}
		#endregion
		private string ComputeHmacSha256(string message, string secretKey)
		{
			var keyBytes = Encoding.UTF8.GetBytes(secretKey);
			var messageBytes = Encoding.UTF8.GetBytes(message);

			using (var hmac = new HMACSHA256(keyBytes))
			{
				var hashBytes = hmac.ComputeHash(messageBytes);
				return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
			}
		}
	}
}
