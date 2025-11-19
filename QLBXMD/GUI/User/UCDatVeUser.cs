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
using Newtonsoft.Json; // ✅ Thêm using

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
		private bool isLoadingData = false; // Flag để tránh recursive calls
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
			isLoadingData = true; // Bắt đầu load
			
			LoadDiemDiDiemDen();
			dtpNgayDi.Value = DateTime.Now;
			cboGioDi.SelectedIndex = 0; // Chọn "Tất cả"
			
			// Thêm event handlers để tự động tìm chuyến
			cboDiemDi.SelectedIndexChanged += CboDiemDi_SelectedIndexChanged;
			cboDiemDen.SelectedIndexChanged += CboDiemDen_SelectedIndexChanged;
			dtpNgayDi.ValueChanged += DtpNgayDi_ValueChanged;
			cboGioDi.SelectedIndexChanged += CboGioDi_SelectedIndexChanged;
			
			isLoadingData = false; // Kết thúc load
			
			// Tải chuyến xe ngay khi vào
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

					// Thêm placeholder vào đầu danh sách
					var diemDiList = new List<string>();
					diemDiList.Add("-- Chọn điểm đi --");
					diemDiList.AddRange(diemDi);

					var diemDenList = new List<string>();
					diemDenList.Add("-- Chọn điểm đến --");
					diemDenList.AddRange(diemDen);

					cboDiemDi.DataSource = diemDiList;
					cboDiemDen.DataSource = diemDenList;

					// Chọn placeholder (index 0)
					cboDiemDi.SelectedIndex = 0;
					cboDiemDen.SelectedIndex = 0;
					
					System.Diagnostics.Debug.WriteLine($"Đã tải {diemDi.Count} điểm đi và {diemDen.Count} điểm đến");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		#endregion

		#region Bước 1: Tìm chuyến xe
		
		private void LoadChuyenXeAuto()
		{
			// Lấy giá trị hiện tại
			string diemDi = cboDiemDi.SelectedItem?.ToString();
			string diemDen = cboDiemDen.SelectedItem?.ToString();
			
			// Bỏ qua placeholder
			if (diemDi != null && diemDi.StartsWith("--")) diemDi = null;
			if (diemDen != null && diemDen.StartsWith("--")) diemDen = null;
			
			// Nếu cả hai đều trống, hiển thị tất cả chuyến xe
			if (string.IsNullOrEmpty(diemDi) && string.IsNullOrEmpty(diemDen))
			{
				LoadChuyenXe(null, null);
				return;
			}
			
			// Nếu chỉ có một trong hai
			LoadChuyenXe(diemDi, diemDen);
		}
		
		private void btnTimChuyen_Click(object sender, EventArgs e)
		{
			// Giữ lại để tương thích, nhưng giờ chỉ gọi LoadChuyenXeAuto
			LoadChuyenXeAuto();
		}

		private void LoadChuyenXe(string diemDi, string diemDen)
		{
			try
			{
				selectedNgayDi = dtpNgayDi.Value.Date;
				
				using (var db = new BXMDEntities())
				{
					// Lấy tất cả chuyến xe trước
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

					// Debug: Kiểm tra số lượng chuyến ban đầu
					System.Diagnostics.Debug.WriteLine($"Tìm thấy {query.Count} chuyến xe ngày {selectedNgayDi:dd/MM/yyyy}");

					// Lọc theo điểm đi nếu có
					if (!string.IsNullOrEmpty(diemDi))
					{
						query = query.Where(r => r.DiemDi == diemDi).ToList();
						System.Diagnostics.Debug.WriteLine($"Sau khi lọc điểm đi '{diemDi}': {query.Count} chuyến");
					}
					
					// Lọc theo điểm đến nếu có
					if (!string.IsNullOrEmpty(diemDen))
					{
						query = query.Where(r => r.DiemDen == diemDen).ToList();
						System.Diagnostics.Debug.WriteLine($"Sau khi lọc điểm đến '{diemDen}': {query.Count} chuyến");
					}

					// Lọc theo giờ nếu không chọn "Tất cả"
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
						System.Diagnostics.Debug.WriteLine($"Sau khi lọc giờ: {query.Count} chuyến");
					}

					if (!query.Any())
					{
						// Không hiện thông báo nữa, chỉ ẩn panel
						System.Diagnostics.Debug.WriteLine("Không có chuyến xe phù hợp");
						pnlResults.Visible = false;
						return;
					}

					// Lưu thông tin để dùng sau
					if (!string.IsNullOrEmpty(diemDi)) selectedDiemDi = diemDi;
					if (!string.IsNullOrEmpty(diemDen)) selectedDiemDen = diemDen;

					// Hiển thị panel trước để tính toán layout đúng
					pnlResults.Visible = true;
					
					// Đảm bảo layout được update
					pnlResults.PerformLayout();
					flpChuyenXe.PerformLayout();
					Application.DoEvents();

					// Clear và add cards
					flpChuyenXe.Controls.Clear();
					flpChuyenXe.SuspendLayout(); // Tạm dừng layout updates
					
					foreach (var item in query)
					{
						var card = CreateChuyenXeCard(item.MaChuyen, item.MaXe, item.MaLoaiXe, 
							item.KhoiHanh, item.DenDuKien ?? item.KhoiHanh.AddHours(6), 
							item.TenNX, item.TenLX, item.SoCho, item.SoChoTrong, item.GiaVe,
							item.DiemDi, item.DiemDen);
						flpChuyenXe.Controls.Add(card);
					}
					
					flpChuyenXe.ResumeLayout(); // Resume layout updates
					System.Diagnostics.Debug.WriteLine($"Đã hiển thị {query.Count} chuyến xe");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi tải danh sách chuyến xe: " + ex.Message + "\n\nStack trace: " + ex.StackTrace, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				System.Diagnostics.Debug.WriteLine($"Lỗi: {ex.Message}\n{ex.StackTrace}");
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
				Height = 100, // Giảm từ 140 xuống 100
				BorderStyle = BorderStyle.FixedSingle,
				BackColor = Color.White,
				Margin = new Padding(5, 5, 5, 10)
			};

			// Logo xe - nhỏ hơn
			Label lblBusIcon = new Label
			{
				Text = "🚌",
				Location = new Point(10, 10),
				Size = new Size(40, 40), // Giảm từ 50x50 xuống 40x40
				Font = new Font("Segoe UI", 20F), // Giảm từ 24F xuống 20F
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
				Font = new Font("Segoe UI", 10F, FontStyle.Bold), // Giảm từ 11F xuống 10F
				ForeColor = Color.FromArgb(51, 51, 51)
			};

			// Loại xe
			Label lblLoaiXe = new Label
			{
				Text = $"{tenLX} - {soCho} chỗ",
				Location = new Point(60, 32),
				AutoSize = false,
				Width = 200,
				Height = 16,
				Font = new Font("Segoe UI", 8F), // Giảm từ 8.5F xuống 8F
				ForeColor = Color.Gray
			};

			// Tuyến đường: Điểm đi → Điểm đến
			Label lblTuyen = new Label
			{
				Text = $"{diemDi} → {diemDen}",
				Location = new Point(60, 50),
				AutoSize = false,
				Width = 350,
				Height = 18,
				Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), // Giảm từ 9F xuống 8.5F
				ForeColor = Color.FromArgb(0, 150, 136)
			};

			// Giờ đi
			Label lblGioDi = new Label
			{
				Text = khoiHanh.ToString("HH:mm"),
				Location = new Point(60, 70),
				AutoSize = false,
				Width = 55,
				Height = 22,
				Font = new Font("Segoe UI", 12F, FontStyle.Bold), // Giảm từ 13F xuống 12F
				ForeColor = Color.FromArgb(0, 150, 136)
			};

			Label lblMuiTen = new Label
			{
				Text = "→",
				Location = new Point(120, 70),
				AutoSize = false,
				Width = 20,
				Height = 22,
				Font = new Font("Segoe UI", 11F), // Giảm từ 12F xuống 11F
				ForeColor = Color.Gray,
				TextAlign = ContentAlignment.MiddleCenter
			};

			// Giờ đến
			Label lblGioDen = new Label
			{
				Text = denDuKien.ToString("HH:mm"),
				Location = new Point(145, 70),
				AutoSize = false,
				Width = 55,
				Height = 22,
				Font = new Font("Segoe UI", 12F, FontStyle.Bold), // Giảm từ 13F xuống 12F
				ForeColor = Color.FromArgb(0, 150, 136)
			};

			// Ngày đi
			Label lblNgayDi = new Label
			{
				Text = khoiHanh.ToString("dd/MM/yyyy"),
				Location = new Point(210, 73),
				AutoSize = false,
				Width = 85,
				Height = 16,
				Font = new Font("Segoe UI", 8F), // Giảm từ 8.5F xuống 8F
				ForeColor = Color.Gray
			};

			// Số chỗ trống
			Label lblChoTrong = new Label
			{
				Text = soChoTrong > 0 ? $"Còn {soChoTrong} chỗ" : "Hết chỗ",
				Location = new Point(310, 73),
				AutoSize = false,
				Width = 100,
				Height = 18,
				Font = new Font("Segoe UI", 8F), // Giảm từ 8.5F xuống 8F
				ForeColor = soChoTrong > 0 ? Color.Green : Color.Red
			};

			// Giá vé (position from right)
			Label lblGiaVe = new Label
			{
				Text = $"{giaVe:#,##0} VNĐ",
				Location = new Point(cardWidth - 260, 25),
				AutoSize = false,
				Width = 140,
				Height = 28,
				Font = new Font("Segoe UI", 12F, FontStyle.Bold), // Giảm từ 13F xuống 12F
				ForeColor = Color.FromArgb(244, 67, 54),
				TextAlign = ContentAlignment.MiddleRight
			};

			// Button chọn (position from right)
			Button btnChon = new Button
			{
				Text = "Chọn chuyến",
				Location = new Point(cardWidth - 145, 60),
				Size = new Size(125, 32), // Giảm từ 135x35 xuống 125x32
				BackColor = Color.FromArgb(0, 150, 136),
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
				Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), // Giảm từ 10F xuống 9.5F
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
					MessageBox.Show($"Lỗi chọn chuyến: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

		#region Bước 2: Chọn chuyến xe và hiển thị sơ đồ ghế
		private void ChonChuyenXe(int maChuyen, int maXe, int maLoaiXe, decimal giaVe)
		{
			selectedMaChuyen = maChuyen;
			selectedMaXe = maXe;
			selectedMaLoaiXe = maLoaiXe;
			selectedGiaVe = giaVe;

			LoadSeatMap();
			
			// Không hiện pnlSeatInfo ngay, chỉ hiện khi chọn ghế
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
						MessageBox.Show($"Không tìm thấy loại xe với MaLX = {selectedMaLoaiXe}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					int soCho = loaiXe.SoCho;
					if (soCho == 0)
					{
						MessageBox.Show("Loại xe có số chỗ = 0", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					bool isGiuongNam = loaiXe.TenLX.ToLower().Contains("giường");

					// Lấy danh sách ghế đã đặt
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
						// Xe giường nằm: 2 tầng
						lblLoaiGhe.Text = "Sơ đồ giường nằm";
						gbTangDuoi.Visible = true;
						gbTangTren.Visible = true;
						gbTangDuoi.Text = "Tầng dưới";
						gbTangTren.Text = "Tầng trên";

						int choMoiTang = soCho / 2;
						CreateSeatButtons(flpTangDuoi, 1, choMoiTang);
						CreateSeatButtons(flpTangTren, choMoiTang + 1, soCho);
					}
					else
					{
						// Xe ghế ngồi: 1 tầng
						lblLoaiGhe.Text = "Sơ đồ ghế ngồi";
						gbTangDuoi.Visible = true;
						gbTangTren.Visible = false;
						gbTangDuoi.Text = "Sơ đồ ghế";

						CreateSeatButtons(flpTangDuoi, 1, soCho);
					}

					pnlSeatMap.Visible = true;
					UpdateThongTinGhe();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi tải sơ đồ ghế:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
					// Ghế đã đặt
					btnSeat.BackColor = Color.FromArgb(220, 220, 220);
					btnSeat.ForeColor = Color.Gray;
					btnSeat.Enabled = false;
					btnSeat.Cursor = Cursors.No;
				}
				else
				{
					// Ghế trống
					btnSeat.BackColor = Color.FromArgb(0, 199, 174); // Xanh - trống
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
				// Bỏ chọn - về màu xanh lá (trống)
				selectedSeats.Remove(seatNumber);
				btn.BackColor = Color.FromArgb(0, 199, 174);
				btn.ForeColor = Color.White;
			}
			else
			{
				// Chọn ghế - màu cam (đang chọn)
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
				lblGheDaChon.Text = $"Ghế đã chọn: {string.Join(", ", selectedSeats.OrderBy(s => s))}";
				decimal tongTien = selectedSeats.Count * (selectedGiaVe ?? 0);
				lblTongTien.Text = $"Tổng tiền: {tongTien:#,##0} VNĐ";
				btnDatVe.Enabled = true;
			}
			else
			{
				lblGheDaChon.Text = "Ghế đã chọn: Chưa chọn";
				lblTongTien.Text = "Tổng tiền: 0 VNĐ";
				btnDatVe.Enabled = false;
			}
		}
		#endregion

		#region Bước 3: Đặt vé
		private void btnDatVe_Click(object sender, EventArgs e)
		{
			if (!selectedMaChuyen.HasValue || selectedSeats.Count == 0)
			{
				MessageBox.Show("Vui lòng chọn chuyến xe và ghế.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// Lấy thông tin khách hàng từ tài khoản
			if (currentUser == null)
			{
				MessageBox.Show("Vui lòng đăng nhập để đặt vé.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			try
			{
				// Tính tổng tiền trước
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

					// Lấy thông tin khách hàng
					var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.MaTK == currentUser.MaTK);
					if (khachHang == null)
					{
						MessageBox.Show("Không tìm thấy thông tin khách hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					// ✅ Hiển thị dialog xác nhận
					using (var confirmDialog = new XacNhanDatVeUserDialog())
					{
						confirmDialog.TenKhachHang = khachHang.TenKH;
						confirmDialog.SoDienThoai = khachHang.SDT;
						confirmDialog.GioiTinh = khachHang.GioiTinh.HasValue && khachHang.GioiTinh.Value ? "Nam" : "Nữ";
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

					// ✅ TẠO HÓA ĐƠN (TongTien = 0, để trigger tính)
					var hoaDon = new HoaDon
					{
						MaKH = khachHang.MaKH,
						MANV = null,
						NgayLap = DateTime.Now,
						TongTien = 0, // ✅ Để trigger tự tính
						TrangThai = false // Chưa thanh toán
					};
					db.HoaDons.Add(hoaDon);
					db.SaveChanges();

					maHD = hoaDon.MaHD;
					Console.WriteLine($"✅ Tạo hóa đơn: MaHD = {maHD}, TongTien ban đầu = {hoaDon.TongTien}");

					// ✅ TẠO VÉ XE (với GiaVe)
					foreach (int soGhe in selectedSeats)
					{
						var ve = new VeXe
						{
							MaChuyen = selectedMaChuyen.Value,
							ChoNgoi = soGhe,
							MaHD = maHD,
							GiaVe = selectedGiaVe.Value // ✅ Lưu giá vé
						};
						db.VeXes.Add(ve);
					}
					db.SaveChanges();
					Console.WriteLine($"✅ Đã tạo {selectedSeats.Count} vé xe");

					// ✅ REFRESH để lấy TongTien đã được trigger tính
					db.Entry(hoaDon).Reload();
					Console.WriteLine($"✅ Sau khi refresh: TongTien = {hoaDon.TongTien}");

					// ✅ Xử lý thanh toán MoMo (với TongTien đã đúng)
					ProcessMoMoPayment(hoaDon.TongTien ?? tienTam, khachHang.MaKH, maHD);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi đặt vé: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Console.WriteLine($"❌ Error: {ex.ToString()}");
			}
		}

		/// <summary>
		/// Xử lý thanh toán MoMo
		/// </summary>
		private async void ProcessMoMoPayment(decimal amount, int maKH, int maHD)
		{
			try
			{
				// Hiển thị loading
				this.Cursor = Cursors.WaitCursor;

				// Tạo orderId unique
				string orderId = $"HD{maHD}_{DateTime.Now:yyyyMMddHHmmss}";
				string orderInfo = $"Thanh toan ve xe - Ma HD: {maHD}";

				// Tạo request thanh toán MoMo
				var momoService = new Services.MoMoPaymentService();
				var response = await momoService.CreatePaymentRequest(
					orderId,
					(long)amount,
					orderInfo
				);

				this.Cursor = Cursors.Default;

				if (response.ResultCode == 0)
				{
					// Thành công - Mở trình duyệt với PayUrl
					MessageBox.Show(
						"Đang chuyển đến trang thanh toán MoMo...\n\n" +
						"Vui lòng hoàn tất thanh toán trong trình duyệt.\n" +
						$"Mã đơn hàng: {orderId}",
						"Thanh toán MoMo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);

					System.Diagnostics.Process.Start(response.PayUrl);
					
					// ✅ Bắt đầu polling để check trạng thái
					StartPollingPaymentStatus(orderId, maHD, amount);
				}
				else
				{
					// Lỗi - Xóa hóa đơn và vé đã tạo
					RollbackHoaDon(maHD);
					
					MessageBox.Show(
						$"Lỗi tạo yêu cầu thanh toán MoMo:\n\n" +
						$"Mã lỗi: {response.ResultCode}\n" +
						$"Thông báo: {response.Message}\n\n" +
						$"Vui lòng thử lại hoặc liên hệ quản trị viên.",
						"Lỗi thanh toán",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
					);
				}
			}
			catch (Exception ex)
			{
				this.Cursor = Cursors.Default;
				MessageBox.Show(
					$"Lỗi kết nối với MoMo:\n{ex.Message}\n\n" +
					$"Vui lòng kiểm tra kết nối internet và thử lại.",
					"Lỗi",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				Console.WriteLine($"❌ MoMo Payment Error: {ex.ToString()}");
			}
		}
		/// <summary>
		/// ✅ Polling MoMo để kiểm tra trạng thái thanh toán (không cần IPN)
		/// </summary>
		private async void StartPollingPaymentStatus(string orderId, int maHD, decimal amount)
		{
			int attempt = 0;
			const int maxAttempts = 24; // 24 lần * 5s = 2 phút
			const int delayMs = 5000;

			while (attempt < maxAttempts)
			{
				attempt++;
				await Task.Delay(delayMs);
				
				string status = await QueryMomoOrder(orderId);

				if (status == "Success")
				{
					MessageBox.Show(
						"✅ Thanh toán MoMo thành công!\n\n" +
						$"Mã hóa đơn: {maHD}\n" +
						$"Số tiền: {amount:#,##0} VNĐ",
						"Thành công",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);
					
					UpdateHoaDonTrangThai(maHD, true, "MoMo");
					
					// ✅ Reset form sau khi thành công
					ResetFormAfterPayment();
					return;
				}
				else if (status == "Failed")
				{
					MessageBox.Show(
						"❌ Thanh toán MoMo thất bại hoặc bị hủy!\n\n" +
						"Hóa đơn và vé đã được hủy.",
						"Thất bại",
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
				"⏰ Hết thời gian chờ thanh toán MoMo.\n\n" +
				"Vui lòng kiểm tra lại trạng thái đơn hàng trong Lịch sử mua vé.",
				"Timeout",
				MessageBoxButtons.OK,
				MessageBoxIcon.Warning
			);
		}

		/// <summary>
		/// ✅ Gọi API Query của MoMo để xác minh đơn hàng
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

					// ✅ MoMo result codes:
					// 0 = Success (Giao dịch thành công)
					// 1000 = Transaction initiated (Đang chờ user xác nhận)
					// 1006 = Transaction not found (Chưa có trong hệ thống)
					// 1007 = Transaction pending (Đang xử lý)
					// 9000 = Transaction failed/cancelled (Thất bại/hủy)
					// Other = Error
					
					if (resultCode == 0)
					{
						return "Success";
					}
					else if (resultCode == 1000 || resultCode == 1006 || resultCode == 1007)
					{
						// ✅ 1000 = Đang chờ user thanh toán → Pending
						return "Pending";
					}
					else if (resultCode == 9000 || resultCode == 1001)
					{
						// 9000 = User hủy giao dịch
						// 1001 = Giao dịch thất bại
						return "Failed";
					}
					else
					{
						// Unknown code → Retry
						Console.WriteLine($"⚠️ Unknown MoMo code: {resultCode}");
						return "Pending";
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("❌ QueryMoMoOrder Error: " + ex.Message);
				return "Pending"; // Retry on error
			}
		}

		/// <summary>
		/// ✅ Cập nhật trạng thái hóa đơn khi có kết quả thanh toán
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
						Console.WriteLine($"❌ Không tìm thấy hóa đơn #{maHD}");
						return;
					}

					hoaDon.TrangThai = daThanhToan;

					if (daThanhToan)
					{
						// Tạo bản ghi thanh toán
						var thanhToan = new ThanhToan
						{
							MaHD = maHD,
							TongTien = hoaDon.TongTien ?? 0,
							NgayTT = DateTime.Now,
							PhuongThuc = phuongThuc
						};
						db.ThanhToans.Add(thanhToan);
						
						Console.WriteLine($"✅ Hóa đơn #{maHD} đã thanh toán qua {phuongThuc}");
					}

					db.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi cập nhật hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Console.WriteLine($"❌ UpdateHoaDonTrangThai Error: {ex.Message}");
			}
		}

		/// <summary>
		/// ✅ Rollback hóa đơn khi thanh toán thất bại
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

					// Xóa hóa đơn
					var hoaDon = db.HoaDons.FirstOrDefault(h => h.MaHD == maHD);
					if (hoaDon != null)
					{
						db.HoaDons.Remove(hoaDon);
					}

					db.SaveChanges();
					Console.WriteLine($"✅ Đã rollback hóa đơn #{maHD}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"❌ RollbackHoaDon Error: {ex.Message}");
			}
		}

		/// <summary>
		/// ✅ Reset form sau khi thanh toán thành công
		/// </summary>
		private void ResetFormAfterPayment()
		{
			try
			{
				ResetForm();
				
				// Reload lại danh sách chuyến xe
				LoadChuyenXeAuto();
				
				MessageBox.Show(
					"🎉 Đặt vé thành công!\n\n" +
					"Bạn có thể xem chi tiết trong mục 'Lịch sử mua vé'.",
					"Thành công",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"❌ ResetFormAfterPayment Error: {ex.Message}");
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

			lblGheDaChon.Text = "Ghế đã chọn: Chưa chọn";
			lblTongTien.Text = "Tổng tiền: 0 VNĐ";
			btnDatVe.Enabled = false;
		}
		#endregion

		#region Swap địa điểm
		private void btnSwap_Click(object sender, EventArgs e)
		{
			if (cboDiemDi.SelectedItem != null && cboDiemDen.SelectedItem != null)
			{
				string diemDi = cboDiemDi.SelectedItem.ToString();
				string diemDen = cboDiemDen.SelectedItem.ToString();

				// Swap cả khi có placeholder (không cần kiểm tra)
				var temp = cboDiemDi.SelectedItem;
				cboDiemDi.SelectedItem = cboDiemDen.SelectedItem;
				cboDiemDen.SelectedItem = temp;
				
				// LoadChuyenXeAuto sẽ được gọi tự động qua SelectedIndexChanged event
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
