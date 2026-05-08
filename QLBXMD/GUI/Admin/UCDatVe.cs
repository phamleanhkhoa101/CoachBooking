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

namespace QLBXMD.GUI.Admin
{
	public partial class UCDatVe : UserControl
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
		private NhanVien currentNhanVien; // ? Thêm field nhân viên
		#endregion

		#region Constructor
		public UCDatVe()
		{
			InitializeComponent();
			this.Load += UCDatVe_Load;
		}

		/// <summary>
		/// Constructor nh?n nhân viên ð? lýu vào hóa ðõn
		/// </summary>
		public void SetNhanVien(NhanVien nhanVien)
		{
			this.currentNhanVien = nhanVien;
		}
		#endregion

		#region Load Events
		private void UCDatVe_Load(object sender, EventArgs e)
		{
			LoadDiemDiDiemDen();
			dtpNgayDi.Value = DateTime.Now;
			dtpNamSinh.Value = DateTime.Now.AddYears(-20);
			cboGioiTinh.SelectedIndex = 0;
			cboGioDi.SelectedIndex = 0; // Ch?n "T?t c?"
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

					cboDiemDi.DataSource = diemDi;
					cboDiemDen.DataSource = diemDen;

					if (cboDiemDi.Items.Count > 0) cboDiemDi.SelectedIndex = 0;
					if (cboDiemDen.Items.Count > 1) cboDiemDen.SelectedIndex = 1;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("L?i t?i d? li?u: " + ex.Message, "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		#endregion

		#region Bý?c 1: T?m chuy?n xe
		private void btnTimChuyen_Click(object sender, EventArgs e)
		{
			if (cboDiemDi.SelectedItem == null || cboDiemDen.SelectedItem == null)
			{
				MessageBox.Show("Vui l?ng ch?n ði?m ði và ði?m ð?n.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			selectedDiemDi = cboDiemDi.SelectedItem.ToString();
			selectedDiemDen = cboDiemDen.SelectedItem.ToString();
			selectedNgayDi = dtpNgayDi.Value.Date;

			if (selectedDiemDi == selectedDiemDen)
			{
				MessageBox.Show("Ði?m ði và ði?m ð?n không ðý?c trùng nhau.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			LoadChuyenXe();
		}

		private void LoadChuyenXe()
		{
			try
			{
				using (var db = new BXMDEntities())
				{
					// ? Query l?y giá vé th?c t? t? ChuyenXe
					var query = (from cx in db.ChuyenXes
								join x in db.Xes on cx.MaXe equals x.MaXe
								join nx in db.NhaXes on x.MaNX equals nx.MaNX
								join lx in db.LoaiXes on x.MaLX equals lx.MaLX
								join td in db.TuyenDuongs on cx.MaTuyen equals td.MaTuyen
								where td.DiemDi == selectedDiemDi
									  && td.DiemDen == selectedDiemDen
									  && System.Data.Entity.DbFunctions.TruncateTime(cx.NgayGioKhoiHanh) == selectedNgayDi
									  //&& cx.NgayGioKhoiHanh > DateTime.Now // ? Ch? hi?n th? chuy?n chýa kh?i hành
									  && cx.GiaVeThucTe.HasValue && cx.GiaVeThucTe > 0 // ? Ch? l?y chuy?n có giá vé
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
									GiaVe = cx.GiaVeThucTe.Value // ? L?y giá vé th?c t? t? ChuyenXe
								}).ToList();

					var results = query;
					int gioIndex;
					// L?c theo gi? n?u không ch?n "T?t c?"
					if (cboGioDi.SelectedIndex > 0)
					{
						gioIndex = cboGioDi.SelectedIndex;
						results = results.Where(r =>
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
					}

					if (!results.Any())
					{
						MessageBox.Show("Không t?m th?y chuy?n xe phù h?p.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
						pnlResults.Visible = false;
						return;
					}

					// Setup DataGridView
					dgvChuyenXe.Columns.Clear();
					dgvChuyenXe.AutoGenerateColumns = false;

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "MaChuyen",
						HeaderText = "M? chuy?n",
						Name = "MaChuyen",
						Visible = false
					});

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "MaXe",
						HeaderText = "M? xe",
						Name = "MaXe",
						Visible = false
					});

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "MaLoaiXe",
						HeaderText = "M? lo?i xe",
						Name = "MaLoaiXe",
						Visible = false
					});

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "KhoiHanh",
						HeaderText = "Kh?i hành",
						Name = "KhoiHanh",
						DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
					});

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "DenDuKien",
						HeaderText = "Ð?n d? ki?n",
						Name = "DenDuKien",
						DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
					});

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "TenNX",
						HeaderText = "Nhà xe",
						Name = "TenNX"
					});

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "TenLX",
						HeaderText = "Lo?i xe",
						Name = "TenLX"
					});

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "SoCho",
						HeaderText = "S? ch?",
						Name = "SoCho"
					});

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "GiaVe",
						HeaderText = "Giá vé (VNÐ)",
						Name = "GiaVe",
						DefaultCellStyle = new DataGridViewCellStyle { Format = "#,##0" }
					});

					var btnColumn = new DataGridViewButtonColumn
					{
						HeaderText = "Hành ð?ng",
						Name = "btnChon",
						Text = "Ch?n",
						UseColumnTextForButtonValue = true
					};
					dgvChuyenXe.Columns.Add(btnColumn);

					dgvChuyenXe.DataSource = results;
					pnlResults.Visible = true;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("L?i t?i danh sách chuy?n xe: " + ex.Message, "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		#endregion

		#region Bý?c 2: Ch?n chuy?n xe
		private void dgvChuyenXe_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0) return;

			if (dgvChuyenXe.Columns[e.ColumnIndex].Name == "btnChon")
			{
				var row = dgvChuyenXe.Rows[e.RowIndex];
				selectedMaChuyen = Convert.ToInt32(row.Cells["MaChuyen"].Value);
				selectedMaXe = Convert.ToInt32(row.Cells["MaXe"].Value);
				selectedMaLoaiXe = Convert.ToInt32(row.Cells["MaLoaiXe"].Value);
				
				// ? FIX: Ki?m tra cell GiaVe trý?c khi convert
				var giaVeCell = row.Cells["GiaVe"].Value;
				if (giaVeCell != null && giaVeCell != DBNull.Value)
				{
					selectedGiaVe = Convert.ToDecimal(giaVeCell);
				}
				else
				{
					selectedGiaVe = 0;
				}

				// ? Debug log
				Console.WriteLine($"=== CH?N CHUY?N XE ===");
				Console.WriteLine($"MaChuyen: {selectedMaChuyen}");
				Console.WriteLine($"MaLoaiXe: {selectedMaLoaiXe}");
				Console.WriteLine($"GiaVe: {selectedGiaVe}");

				// ? Validate giá vé
				if (selectedGiaVe == 0)
				{
					MessageBox.Show(
						"Chuy?n xe này chýa có giá vé ho?c giá vé = 0.\n" +
						"Vui l?ng liên h? qu?n tr? viên ð? c?p nh?t giá vé.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
					return;
				}

				LoadSeatMap();
			}
		}
		#endregion

		#region Bý?c 3: Hi?n th? sõ ð? gh?
		private void LoadSeatMap()
		{
			try
			{
				using (var db = new BXMDEntities())
				{
					// L?y s? ch? và lo?i xe
					var loaiXe = db.LoaiXes.FirstOrDefault(lx => lx.MaLX == selectedMaLoaiXe);
					if (loaiXe == null) return;

					int soCho = loaiXe.SoCho;
					if (soCho == 0) return;
					
					bool isGiuongNam = loaiXe.TenLX.ToLower().Contains("giý?ng");

					// L?y danh sách gh? ð? ð?t
					bookedSeats = db.VeXes
						.Where(v => v.MaChuyen == selectedMaChuyen)
						.Select(v => v.ChoNgoi)
						.ToList();

					// Clear selected seats
					selectedSeats.Clear();

					// Clear panels
					flpTangDuoi.Controls.Clear();
					flpTangTren.Controls.Clear();

					if (isGiuongNam)
					{
						// Xe giý?ng n?m: 2 t?ng
						gbTangDuoi.Visible = true;
						gbTangTren.Visible = true;

						int choMoiTang = soCho / 2;
						CreateSeatButtons(flpTangDuoi, 1, choMoiTang);
						CreateSeatButtons(flpTangTren, choMoiTang + 1, soCho);
					}
					else
					{
						// Xe gh? ng?i: 1 t?ng
						gbTangDuoi.Visible = true;
						gbTangTren.Visible = false;
						gbTangDuoi.Text = "Sõ ð? gh?";

						CreateSeatButtons(flpTangDuoi, 1, soCho);
					}

					pnlSeatMap.Visible = true;
					UpdateThongTinChuyen();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("L?i t?i sõ ð? gh?: " + ex.Message, "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
					Tag = i
				};

				btnSeat.FlatAppearance.BorderSize = 1;
				btnSeat.FlatAppearance.BorderColor = Color.Gray;

				if (bookedSeats.Contains(i))
				{
					// Gh? ð? ð?t
					btnSeat.BackColor = Color.FromArgb(217, 83, 79);
					btnSeat.ForeColor = Color.White;
					btnSeat.Enabled = false;
				}
				else
				{
					// Gh? tr?ng
					btnSeat.BackColor = Color.FromArgb(0, 199, 174);
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
				// B? ch?n
				selectedSeats.Remove(seatNumber);
				btn.BackColor = Color.FromArgb(0, 199, 174); // Xanh - tr?ng
			}
			else
			{
				// Ch?n gh?
				selectedSeats.Add(seatNumber);
				btn.BackColor = Color.FromArgb(245, 128, 37); // Cam - ðang ch?n
			}

			// Debug: Hi?n th? s? gh? ð? ch?n
			Console.WriteLine($"Gh? ð? ch?n: {string.Join(", ", selectedSeats)}");
			Console.WriteLine($"T?ng s? gh?: {selectedSeats.Count}");
			Console.WriteLine($"Giá vé: {selectedGiaVe}");

			UpdateThongTinChuyen();
			
			// Hi?n th? panel thông tin khách hàng n?u ð? ch?n gh?
			if (selectedSeats.Count > 0)
			{
				pnlThongTinKH.Visible = true;
			}
			else
			{
				pnlThongTinKH.Visible = false;
			}
		}

		private void UpdateThongTinChuyen()
		{
			if (selectedMaChuyen.HasValue && selectedGiaVe.HasValue)
			{
				string gheDaChon = selectedSeats.Count > 0 
					? string.Join(", ", selectedSeats.OrderBy(s => s)) 
					: "Chýa ch?n";

				// ? T?o text v?i line breaks r? ràng
				lblThongTinChuyen.Text = string.Format(
					"Tuy?n: {0} ? {1}{2}" +
					"Ngày: {3}{4}" +
					"Gh? ð? ch?n: {5}",
					selectedDiemDi, 
					selectedDiemDen,
					Environment.NewLine,
					selectedNgayDi.ToString("dd/MM/yyyy"),
					Environment.NewLine,
					gheDaChon
				);

				decimal tongTien = selectedSeats.Count * selectedGiaVe.Value;
				lblTongTien.Text = $"T?ng: {tongTien:#,##0} VNÐ";

				// ? Auto-size label ð? hi?n th? ð?y ð?
				lblThongTinChuyen.AutoSize = true;
				lblThongTinChuyen.MaximumSize = new Size(1200, 0); // Gi?i h?n r?ng, không gi?i h?n cao

				// Debug
				Console.WriteLine($"UpdateThongTinChuyen - S? gh?: {selectedSeats.Count}, Giá: {selectedGiaVe}, T?ng: {tongTien}");
			}
			else
			{
				// Debug: Ki?m tra giá tr? null
				Console.WriteLine($"selectedMaChuyen: {selectedMaChuyen}, selectedGiaVe: {selectedGiaVe}");
				
				lblThongTinChuyen.Text = "Chýa ch?n chuy?n";
				lblTongTien.Text = "T?ng: 0 VNÐ";
			}
		}
		#endregion

		#region Bý?c 4: Ð?t vé
		private void btnTimKH_Click(object sender, EventArgs e)
		{
			// Validate SDT
			if (string.IsNullOrWhiteSpace(txtSDT.Text))
			{
				MessageBox.Show("Vui l?ng nh?p s? ði?n tho?i ð? t?m.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtSDT.Focus();
				return;
			}

			string sdt = txtSDT.Text.Trim();

			try
			{
				using (var db = new BXMDEntities())
				{
					var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.SDT == sdt);

					if (khachHang != null)
					{
						// T?m th?y - Ð? d? li?u vào textbox
						txtTenKH.Text = khachHang.TenKH;
						dtpNamSinh.Value = khachHang.NamSinh; // ? FIX: NamSinh is DateTime, not DateTime?
						cboGioiTinh.SelectedIndex = (khachHang.GioiTinh ?? false) ? 0 : 1; // true = Nam, false = N?

						// Disable các field
						txtTenKH.ReadOnly = true;
						dtpNamSinh.Enabled = false;
						cboGioiTinh.Enabled = false;

						txtTenKH.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);

						MessageBox.Show(
							$"Ð? t?m th?y khách hàng:\n{khachHang.TenKH}",
							"Thông báo",
							MessageBoxButtons.OK,
							MessageBoxIcon.Information
						);
					}
					else
					{
						// Không t?m th?y - Enable nh?p m?i
						txtTenKH.Clear();
						dtpNamSinh.Value = DateTime.Now.AddYears(-20);
						cboGioiTinh.SelectedIndex = 0;

						txtTenKH.ReadOnly = false;
						dtpNamSinh.Enabled = true;
						cboGioiTinh.Enabled = true;

						txtTenKH.BackColor = System.Drawing.Color.White;

						MessageBox.Show(
							"Không t?m th?y khách hàng v?i s? ði?n tho?i này.\n" +
							"Vui l?ng nh?p thông tin khách hàng m?i.",
							"Thông báo",
							MessageBoxButtons.OK,
							MessageBoxIcon.Information
						);

						txtTenKH.Focus();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("L?i t?m khách hàng: " + ex.Message, "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnDatVe_Click(object sender, EventArgs e)
		{
			// Debug log ð?u tiên
			Console.WriteLine("=== B?T Ð?U Ð?T VÉ ===");
			Console.WriteLine($"selectedMaChuyen: {selectedMaChuyen}");
			Console.WriteLine($"selectedGiaVe: {selectedGiaVe}");
			Console.WriteLine($"selectedSeats.Count: {selectedSeats.Count}");
			Console.WriteLine($"Gh? ð? ch?n: {string.Join(", ", selectedSeats)}");

			// Validate
			if (!selectedMaChuyen.HasValue)
			{
				MessageBox.Show("Vui l?ng ch?n chuy?n xe.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (selectedSeats.Count == 0)
			{
				MessageBox.Show("Vui l?ng ch?n ít nh?t m?t gh?.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (!selectedGiaVe.HasValue || selectedGiaVe.Value == 0)
			{
				MessageBox.Show("Giá vé không h?p l?. Vui l?ng ch?n l?i chuy?n xe.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (string.IsNullOrWhiteSpace(txtTenKH.Text))
			{
				MessageBox.Show("Vui l?ng nh?p tên khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtTenKH.Focus();
				return;
			}

			if (string.IsNullOrWhiteSpace(txtSDT.Text))
			{
				MessageBox.Show("Vui l?ng nh?p s? ði?n tho?i.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtSDT.Focus();
				return;
			}

			if (txtSDT.Text.Length < 10 || txtSDT.Text.Length > 11)
			{
				MessageBox.Show("S? ði?n tho?i ph?i có 10-11 ch? s?.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtSDT.Focus();
				return;
			}

			if (cboGioiTinh.SelectedItem == null)
			{
				MessageBox.Show("Vui l?ng ch?n gi?i tính.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// Tính t?ng ti?n trý?c khi confirm
			decimal tienTam = selectedSeats.Count * selectedGiaVe.Value;

			// ? Hi?n th? dialog xác nh?n v?i phýõng th?c thanh toán
			DateTime ngayKhoiHanh = DateTime.Now;
			using (var db = new BXMDEntities())
			{
				var chuyenXe = db.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == selectedMaChuyen);
				if (chuyenXe != null)
				{
					ngayKhoiHanh = chuyenXe.NgayGioKhoiHanh;
				}
			}

			// ? L?y phýõng th?c thanh toán t? dialog
			string phuongThucThanhToan = "Ti?n m?t";
			using (var confirmDialog = new XacNhanDatVeDialog())
			{
				confirmDialog.TenKhachHang = txtTenKH.Text.Trim();
				confirmDialog.SoDienThoai = txtSDT.Text.Trim();
				confirmDialog.GioiTinh = cboGioiTinh.SelectedItem.ToString();
				confirmDialog.NamSinh = dtpNamSinh.Value.Date;
				confirmDialog.DiemDi = selectedDiemDi;
				confirmDialog.DiemDen = selectedDiemDen;
				confirmDialog.NgayKhoiHanh = ngayKhoiHanh;
				confirmDialog.GheDaChon = string.Join(", ", selectedSeats.OrderBy(s => s));
				confirmDialog.GiaVe = selectedGiaVe.Value;
				confirmDialog.SoLuongGhe = selectedSeats.Count;
				confirmDialog.TongTien = tienTam;

				if (confirmDialog.ShowDialog() != DialogResult.OK)
					return;

				// ? FIX: Lýu phýõng th?c thanh toán t? dialog
				phuongThucThanhToan = confirmDialog.PhuongThucThanhToan;
				Console.WriteLine($"? Phýõng th?c thanh toán ð? ch?n: {phuongThucThanhToan}");
			}

			// ? N?u ch?n MoMo ? X? l? thanh toán MoMo
			if (phuongThucThanhToan == "MoMo")
			{
				ProcessMoMoPayment(tienTam);
				return;
			}

			// ? Ti?p t?c v?i thanh toán ti?n m?t
			try
			{
				int maHD = 0;
				int soLuongVe = 0;
				decimal tongTien = 0;
				string tenNguoiTaoVe = "Khách hàng"; // ? Khai báo ? ngoài using block

				using (var db = new BXMDEntities())
				{
					// ? 1. Ki?m tra chuy?n xe c?n t?n t?i không
					var chuyenXe = db.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == selectedMaChuyen);
					if (chuyenXe == null)
					{
						MessageBox.Show("Chuy?n xe không t?n t?i.", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					// ? 1.5. Ki?m tra ngày kh?i hành ph?i l?n hõn hi?n t?i
					if (chuyenXe.NgayGioKhoiHanh <= DateTime.Now)
					{
						MessageBox.Show(
							$"Không th? ð?t vé cho chuy?n xe ð? kh?i hành ho?c s?p kh?i hành.\n\n" +
							$"Ngày kh?i hành: {chuyenXe.NgayGioKhoiHanh:dd/MM/yyyy HH:mm}\n" +
							$"Hi?n t?i: {DateTime.Now:dd/MM/yyyy HH:mm}",
							"L?i",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error
						);
						return;
					}

					// ? 1.6. Ki?m tra giá vé th?c t?
					if (!chuyenXe.GiaVeThucTe.HasValue || chuyenXe.GiaVeThucTe.Value <= 0)
					{
						MessageBox.Show(
							$"Chuy?n xe này chýa có giá vé ho?c giá vé = 0.\n\n" +
							$"M? chuy?n: {chuyenXe.MaChuyen}\n" +
							$"Vui l?ng liên h? qu?n tr? viên ð? c?p nh?t giá vé.",
							"L?i",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error
						);
						return;
					}

					// ? 2. Ki?m tra gh? có b? ð?t trùng không
					var daDat = db.VeXes
						.Where(v => v.MaChuyen == selectedMaChuyen && selectedSeats.Contains(v.ChoNgoi))
						.Select(v => v.ChoNgoi)
						.ToList();

					if (daDat.Any())
					{
						MessageBox.Show(
							$"Gh? s? {string.Join(", ", daDat)} ð? ðý?c ð?t b?i ngý?i khác.\nVui l?ng ch?n gh? khác.",
							"Thông báo",
							MessageBoxButtons.OK,
							MessageBoxIcon.Warning
						);
						LoadSeatMap(); // Reload l?i sõ ð? gh?
						return;
					}

					// ? 3. T?m ho?c t?o khách hàng
					var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.SDT == txtSDT.Text.Trim());
					
					if (khachHang == null)
					{
						// ? Validate d? li?u trý?c khi t?o
						if (string.IsNullOrWhiteSpace(txtTenKH.Text))
						{
							MessageBox.Show("Tên khách hàng không ðý?c ð? tr?ng.", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}

						khachHang = new KhachHang
						{
							TenKH = txtTenKH.Text.Trim(),
							NamSinh = dtpNamSinh.Value.Date, // ? Ch? l?y ngày, b? gi?
							SDT = txtSDT.Text.Trim(),
							GioiTinh = cboGioiTinh.SelectedItem.ToString() == "Nam",
							MaTK = null
						};

						db.KhachHangs.Add(khachHang);
						
						try
						{
							db.SaveChanges(); // ? Save riêng ð? catch l?i KH
							Console.WriteLine($"? Ð? t?o KH m?i: MaKH = {khachHang.MaKH}");
						}
						catch (Exception exKH)
						{
							MessageBox.Show(
								$"L?i t?o khách hàng:\n{exKH.Message}\n" +
								(exKH.InnerException != null ? exKH.InnerException.Message : ""),
								"L?i",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error
							);
							return;
						}
					}
					else
					{
						Console.WriteLine($"? S? d?ng KH c?: MaKH = {khachHang.MaKH}");
					}

					// ? 4. T?o hóa ðõn
					var hoaDon = new HoaDon
					{
						MaKH = khachHang.MaKH,
						MaNV = currentNhanVien?.MaNV, // ? Lýu nhân viên th?c hi?n (n?u có)
						NgayLap = DateTime.Now,
						TongTien = 0, // ? Ð? trigger t? ð?ng tính
						TrangThai = false // ? Chýa thanh toán
					};

					db.HoaDons.Add(hoaDon);
					
					try
					{
						db.SaveChanges(); // ? Save riêng ð? catch l?i HoaDon
						Console.WriteLine($"? Ð? t?o hóa ðõn: MaHD = {hoaDon.MaHD}");
					}
					catch (Exception exHD)
					{
						var innerMsg = exHD.InnerException?.InnerException?.Message ?? exHD.InnerException?.Message ?? "";
						MessageBox.Show(
							$"L?i t?o hóa ðõn:\n{exHD.Message}\n\nChi ti?t:\n{innerMsg}",
							"L?i",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error
						);
						Console.WriteLine($"? L?i HoaDon: {exHD.ToString()}");
						return;
					}

					// ? 5. T?o vé xe
					foreach (var seat in selectedSeats)
					{
						var veXe = new VeXe
						{
							MaChuyen = selectedMaChuyen.Value,
							MaHD = hoaDon.MaHD,
							ChoNgoi = seat,
							GiaVe = selectedGiaVe.Value
						};
						db.VeXes.Add(veXe);
					}

					try
					{
						db.SaveChanges(); // ? Save vé xe
						Console.WriteLine($"? Ð? t?o {selectedSeats.Count} vé xe");
					}
					catch (Exception exVe)
					{
						var innerMsg = exVe.InnerException?.InnerException?.Message ?? exVe.InnerException?.Message ?? "";
						MessageBox.Show(
							$"L?i t?o vé xe:\n{exVe.Message}\n\nChi ti?t:\n{innerMsg}",
							"L?i",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error
						);
						Console.WriteLine($"? L?i VeXe: {exVe.ToString()}");
						return;
					}

					// ? Refresh ð? l?y TongTien ð? ðý?c trigger tính
					db.Entry(hoaDon).Reload();

					// ? 6. T?o thanh toán và c?p nh?t tr?ng thái hóa ðõn
					var thanhToan = new ThanhToan
					{
						MaHD = hoaDon.MaHD,
						TongTien = hoaDon.TongTien ?? 0,
						NgayTT = DateTime.Now,
						PhuongThuc = phuongThucThanhToan
					};
					db.ThanhToans.Add(thanhToan);

					// ? Ðánh d?u hóa ðõn ð? thanh toán
					hoaDon.TrangThai = true;

					try
					{
						db.SaveChanges(); // ? Save thanh toán và c?p nh?t tr?ng thái
						Console.WriteLine($"? Ð? t?o thanh toán: MaTT = {thanhToan.MaTT}");
					}
					catch (Exception exTT)
					{
						var innerMsg = exTT.InnerException?.InnerException?.Message ?? exTT.InnerException?.Message ?? "";
						MessageBox.Show(
							$"L?i t?o thanh toán:\n{exTT.Message}\n\nChi ti?t:\n{innerMsg}",
							"L?i",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error
						);
						Console.WriteLine($"? L?i ThanhToan: {exTT.ToString()}");
						return;
					}

					// ? Lýu thông tin ð? hi?n th?
					maHD = hoaDon.MaHD;
					soLuongVe = selectedSeats.Count;
					tongTien = hoaDon.TongTien ?? 0;
					
					// ? L?y tên nhân viên ð? hi?n th?
					tenNguoiTaoVe = currentNhanVien != null ? currentNhanVien.TenNV : "Khách hàng";
				}

				MessageBox.Show(
					$"Ð?t vé thành công!\n\n" +
					$"M? hóa ðõn: {maHD}\n" +
					$"S? lý?ng vé: {soLuongVe}\n" +
					$"T?ng ti?n: {tongTien:#,##0} VNÐ\n" +
					$"Phýõng th?c thanh toán: {phuongThucThanhToan}\n" +
					$"Ngý?i t?o vé: {tenNguoiTaoVe}\n" +
					$"Tr?ng thái: Ð? thanh toán",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);

				// Reset form
				ResetForm();
			}
			catch (Exception ex)
			{
				// ? Hi?n th? chi ti?t l?i
				string errorMsg = $"L?i ð?t vé:\n{ex.Message}";
				
				if (ex.InnerException != null)
				{
					errorMsg += $"\n\nChi ti?t:\n{ex.InnerException.Message}";
				}

				// ? Log full stack trace
				Console.WriteLine("=== FULL ERROR ===");
				Console.WriteLine(ex.ToString());

				MessageBox.Show(errorMsg, "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// X? l? thanh toán MoMo
		/// </summary>
		private async void ProcessMoMoPayment(decimal amount)
		{
			try
			{
				// ? 1. T?O HÓA ÐÕN VÀ VÉ T?M TH?I TRÝ?C (TrangThai = false)
				int maHD = 0;
				string orderId = $"HD{DateTime.Now:yyyyMMddHHmmss}";
				
				using (var db = new BXMDEntities())
				{
					// Ki?m tra chuy?n xe
					var chuyenXe = db.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == selectedMaChuyen);
					if (chuyenXe == null)
					{
						MessageBox.Show("Chuy?n xe không t?n t?i.", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					// Ki?m tra gh? trùng
					var daDat = db.VeXes
						.Where(v => v.MaChuyen == selectedMaChuyen && selectedSeats.Contains(v.ChoNgoi))
						.Select(v => v.ChoNgoi)
						.ToList();

					if (daDat.Any())
					{
						MessageBox.Show(
							$"Gh? s? {string.Join(", ", daDat)} ð? ðý?c ð?t.\nVui l?ng ch?n gh? khác.",
							"Thông báo",
							MessageBoxButtons.OK,
							MessageBoxIcon.Warning
						);
						LoadSeatMap();
						return;
					}

					// T?o/l?y khách hàng
					var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.SDT == txtSDT.Text.Trim());
					if (khachHang == null)
					{
						khachHang = new KhachHang
						{
							TenKH = txtTenKH.Text.Trim(),
							NamSinh = dtpNamSinh.Value.Date,
							SDT = txtSDT.Text.Trim(),
							GioiTinh = cboGioiTinh.SelectedItem.ToString() == "Nam",
							MaTK = null
						};
						db.KhachHangs.Add(khachHang);
						db.SaveChanges();
					}

					// ? T?o hóa ðõn T?M TH?I (TrangThai = false = ch? thanh toán)
					var hoaDon = new HoaDon
					{
						MaKH = khachHang.MaKH,
						MaNV = currentNhanVien?.MaNV,
						NgayLap = DateTime.Now,
						TongTien = 0,
						TrangThai = false // ? CH? THANH TOÁN
					};
					db.HoaDons.Add(hoaDon);
					db.SaveChanges();

					// ? T?o vé xe
					foreach (var seat in selectedSeats)
					{
						var veXe = new VeXe
						{
							MaChuyen = selectedMaChuyen.Value,
							MaHD = hoaDon.MaHD,
							ChoNgoi = seat,
							GiaVe = selectedGiaVe.Value
						};
						db.VeXes.Add(veXe);
					}
					db.SaveChanges();

					// Refresh ð? l?y TongTien
					db.Entry(hoaDon).Reload();

					// ? Lýu MaHD ð? tracking
					maHD = hoaDon.MaHD;
					
					Console.WriteLine($"? Ð? t?o hóa ðõn t?m: MaHD = {maHD}, OrderId = {orderId}");
				}

				// ? 2. G?I MOMO PAYMENT
				this.Cursor = Cursors.WaitCursor;
				
				string orderInfo = $"Thanh toan ve xe - HD{maHD}";
				
				var momoService = new Services.MoMoPaymentService();
				
				// ? Thêm maHD vào extraData ð? tracking
				string extraData = $"{{\"MaHD\":{maHD}}}";
				
				var response = await momoService.CreatePaymentRequest(
					orderId,
					(long)amount,
					orderInfo,
					extraData
				);

				this.Cursor = Cursors.Default;

				if (response.ResultCode == 0)
				{
					
					// Thành công - M? tr?nh duy?t
					MessageBox.Show(
						$"? Ð? t?o ðõn hàng t?m th?i!\n\n" +
						$"M? hóa ðõn: {maHD}\n" +
						$"M? ðõn MoMo: {orderId}\n\n" +
						$"Ðang chuy?n ð?n trang thanh toán MoMo...\n" +
						$"Vui l?ng hoàn t?t thanh toán trong tr?nh duy?t.",
						"Thanh toán MoMo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);


System.Diagnostics.Process.Start(response.PayUrl);

					// ? Sau khi m? MoMo, b?t ð?u Polling ki?m tra tr?ng thái
					StartPollingPaymentStatus(orderId, maHD, amount);

					// ? 3. CLEAR FORM NGAY SAU KHI T?O ÐÕN
					ResetForm();

					MessageBox.Show(
						"?? Hý?ng d?n:\n\n" +
						"1. Hoàn t?t thanh toán trên MoMo\n" +
						"2. Sau khi thanh toán thành công, h? th?ng s? t? ð?ng c?p nh?t tr?ng thái\n" +
						"3. Ki?m tra l?i trong 'Qu?n l? vé' ho?c 'L?ch s? mua vé'\n\n" +
						$"M? hóa ðõn: {maHD}\n" +
						$"Tr?ng thái hi?n t?i: Ch? thanh toán",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);
				}
				else
				{
					// L?i - XÓA HÓA ÐÕN T?M
					using (var db = new BXMDEntities())
					{
						var hoaDonCancel = db.HoaDons.FirstOrDefault(h => h.MaHD == maHD);
						if (hoaDonCancel != null)
						{
							// Xóa vé trý?c
							var veXeCancel = db.VeXes.Where(v => v.MaHD == maHD).ToList();
							db.VeXes.RemoveRange(veXeCancel);
							
							// Xóa hóa ðõn
							db.HoaDons.Remove(hoaDonCancel);
							db.SaveChanges();
							
							Console.WriteLine($"? Ð? h?y hóa ðõn t?m: MaHD = {maHD}");
						}
					}

					MessageBox.Show(
						$"? L?i t?o yêu c?u thanh toán MoMo:\n\n" +
						$"M? l?i: {response.ResultCode}\n" +
						$"Thông báo: {response.Message}\n\n" +
						$"Vui l?ng th? l?i ho?c ch?n phýõng th?c thanh toán khác.",
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
			const int maxAttempts = 12; // 12 l?n * 5s = 1 phút
			const int delayMs = 5000;

			MessageBox.Show("? Ðang ch? ngý?i dùng hoàn t?t thanh toán MoMo...", "MoMo", MessageBoxButtons.OK, MessageBoxIcon.Information);

			while (attempt < maxAttempts)
			{
				attempt++;
				string status = await QueryMomoOrder(orderId);

				if (status == "Success")
				{
					MessageBox.Show("? Thanh toán MoMo thành công!", "MoMo", MessageBoxButtons.OK, MessageBoxIcon.Information);
					UpdateHoaDonTrangThai(maHD, true, "MoMo");
					return;
				}
				else if (status == "Failed")
				{
					MessageBox.Show("? Thanh toán MoMo th?t b?i ho?c b? h?y!", "MoMo", MessageBoxButtons.OK, MessageBoxIcon.Error);
					UpdateHoaDonTrangThai(maHD, false, "MoMo");
					return;
				}

				await Task.Delay(delayMs);
			}

			MessageBox.Show("? H?t th?i gian ch? thanh toán MoMo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
					var json = Newtonsoft.Json.JsonConvert.SerializeObject(body);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					var response = await httpClient.PostAsync(ApiQuery, content);
					string responseJson = await response.Content.ReadAsStringAsync();

					dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseJson);
					int resultCode = data.resultCode;
					string message = data.message;

					Console.WriteLine($"[MoMo Polling] {orderId} => Code: {resultCode}, Message: {message}");

					if (resultCode == 0)
						return "Success";
					else if (resultCode == 1006 || resultCode == 1007)
						return "Pending";
					else
						return "Failed";
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine("? QueryMoMoOrder Error: " + ex.Message);
				return "Pending";
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
					if (hoaDon == null) return;

					hoaDon.TrangThai = daThanhToan;
					hoaDon.TongTien = hoaDon.TongTien ?? 0;

					if (daThanhToan)
					{
						var thanhToan = new ThanhToan
						{
							MaHD = maHD,
							TongTien = hoaDon.TongTien.Value,
							NgayTT = DateTime.Now,
							PhuongThuc = phuongThuc
						};
						db.ThanhToans.Add(thanhToan);
					}

					db.SaveChanges();
				}

				if (daThanhToan)
				{
					MessageBox.Show(
						$"?? Hóa ðõn #{maHD} ð? ðý?c c?p nh?t tr?ng thái: Ð? THANH TOÁN ({phuongThuc}).",
						"Thành công",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);
				}
				else
				{
					MessageBox.Show(
						$"?? Hóa ðõn #{maHD} ð? ðý?c c?p nh?t: THANH TOÁN TH?T B?I.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("L?i c?p nh?t hóa ðõn: " + ex.Message);
			}
		}

		private void ResetForm()
		{
			selectedMaChuyen = null;
			selectedMaXe = null;
			selectedMaLoaiXe = null;
			selectedGiaVe = null;
			selectedSeats.Clear();
			bookedSeats.Clear();

			txtTenKH.Clear();
			txtSDT.Clear();
			dtpNamSinh.Value = DateTime.Now.AddYears(-20);
			cboGioiTinh.SelectedIndex = 0;

			// ? Enable l?i các field
			txtTenKH.ReadOnly = false;
			dtpNamSinh.Enabled = true;
			cboGioiTinh.Enabled = true;

			txtTenKH.BackColor = System.Drawing.Color.White;
			dtpNamSinh.BackColor = System.Drawing.Color.White;
			cboGioiTinh.BackColor = System.Drawing.Color.White;

			pnlResults.Visible = false;
			pnlSeatMap.Visible = false;
			pnlThongTinKH.Visible = false;

			lblThongTinChuyen.Text = "Chýa ch?n chuy?n";
			lblTongTien.Text = "T?ng: 0 VNÐ";
		}
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

		#endregion
	}
}
