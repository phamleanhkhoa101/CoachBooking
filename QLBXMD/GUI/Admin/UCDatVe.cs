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
		private NhanVien currentNhanVien; // ✅ Thêm field nhân viên
		#endregion

		#region Constructor
		public UCDatVe()
		{
			InitializeComponent();
			this.Load += UCDatVe_Load;
		}

		/// <summary>
		/// Constructor nhận nhân viên để lưu vào hóa đơn
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
			cboGioDi.SelectedIndex = 0; // Chọn "Tất cả"
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
				MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		#endregion

		#region Bước 1: Tìm chuyến xe
		private void btnTimChuyen_Click(object sender, EventArgs e)
		{
			if (cboDiemDi.SelectedItem == null || cboDiemDen.SelectedItem == null)
			{
				MessageBox.Show("Vui lòng chọn điểm đi và điểm đến.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			selectedDiemDi = cboDiemDi.SelectedItem.ToString();
			selectedDiemDen = cboDiemDen.SelectedItem.ToString();
			selectedNgayDi = dtpNgayDi.Value.Date;

			if (selectedDiemDi == selectedDiemDen)
			{
				MessageBox.Show("Điểm đi và điểm đến không được trùng nhau.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
					// ✅ Query lấy giá vé thực tế từ ChuyenXe
					var query = (from cx in db.ChuyenXes
								join x in db.Xes on cx.MaXe equals x.MaXe
								join nx in db.NhaXes on x.MaNX equals nx.MaNX
								join lx in db.LoaiXes on x.MaLX equals lx.MaLX
								join td in db.TuyenDuongs on cx.MaTuyen equals td.MaTuyen
								where td.DiemDi == selectedDiemDi
									  && td.DiemDen == selectedDiemDen
									  && System.Data.Entity.DbFunctions.TruncateTime(cx.NgayGioKhoiHanh) == selectedNgayDi
									  //&& cx.NgayGioKhoiHanh > DateTime.Now // ✅ Chỉ hiển thị chuyến chưa khởi hành
									  && cx.GiaVeThucTe.HasValue && cx.GiaVeThucTe > 0 // ✅ Chỉ lấy chuyến có giá vé
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
									GiaVe = cx.GiaVeThucTe.Value // ✅ Lấy giá vé thực tế từ ChuyenXe
								}).ToList();

					var results = query;
					int gioIndex;
					// Lọc theo giờ nếu không chọn "Tất cả"
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
						MessageBox.Show("Không tìm thấy chuyến xe phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
						pnlResults.Visible = false;
						return;
					}

					// Setup DataGridView
					dgvChuyenXe.Columns.Clear();
					dgvChuyenXe.AutoGenerateColumns = false;

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "MaChuyen",
						HeaderText = "Mã chuyến",
						Name = "MaChuyen",
						Visible = false
					});

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "MaXe",
						HeaderText = "Mã xe",
						Name = "MaXe",
						Visible = false
					});

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "MaLoaiXe",
						HeaderText = "Mã loại xe",
						Name = "MaLoaiXe",
						Visible = false
					});

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "KhoiHanh",
						HeaderText = "Khởi hành",
						Name = "KhoiHanh",
						DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
					});

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "DenDuKien",
						HeaderText = "Đến dự kiến",
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
						HeaderText = "Loại xe",
						Name = "TenLX"
					});

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "SoCho",
						HeaderText = "Số chỗ",
						Name = "SoCho"
					});

					dgvChuyenXe.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "GiaVe",
						HeaderText = "Giá vé (VNĐ)",
						Name = "GiaVe",
						DefaultCellStyle = new DataGridViewCellStyle { Format = "#,##0" }
					});

					var btnColumn = new DataGridViewButtonColumn
					{
						HeaderText = "Hành động",
						Name = "btnChon",
						Text = "Chọn",
						UseColumnTextForButtonValue = true
					};
					dgvChuyenXe.Columns.Add(btnColumn);

					dgvChuyenXe.DataSource = results;
					pnlResults.Visible = true;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi tải danh sách chuyến xe: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		#endregion

		#region Bước 2: Chọn chuyến xe
		private void dgvChuyenXe_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0) return;

			if (dgvChuyenXe.Columns[e.ColumnIndex].Name == "btnChon")
			{
				var row = dgvChuyenXe.Rows[e.RowIndex];
				selectedMaChuyen = Convert.ToInt32(row.Cells["MaChuyen"].Value);
				selectedMaXe = Convert.ToInt32(row.Cells["MaXe"].Value);
				selectedMaLoaiXe = Convert.ToInt32(row.Cells["MaLoaiXe"].Value);
				
				// ✅ FIX: Kiểm tra cell GiaVe trước khi convert
				var giaVeCell = row.Cells["GiaVe"].Value;
				if (giaVeCell != null && giaVeCell != DBNull.Value)
				{
					selectedGiaVe = Convert.ToDecimal(giaVeCell);
				}
				else
				{
					selectedGiaVe = 0;
				}

				// ✅ Debug log
				Console.WriteLine($"=== CHỌN CHUYẾN XE ===");
				Console.WriteLine($"MaChuyen: {selectedMaChuyen}");
				Console.WriteLine($"MaLoaiXe: {selectedMaLoaiXe}");
				Console.WriteLine($"GiaVe: {selectedGiaVe}");

				// ✅ Validate giá vé
				if (selectedGiaVe == 0)
				{
					MessageBox.Show(
						"Chuyến xe này chưa có giá vé hoặc giá vé = 0.\n" +
						"Vui lòng liên hệ quản trị viên để cập nhật giá vé.",
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

		#region Bước 3: Hiển thị sơ đồ ghế
		private void LoadSeatMap()
		{
			try
			{
				using (var db = new BXMDEntities())
				{
					// Lấy số chỗ và loại xe
					var loaiXe = db.LoaiXes.FirstOrDefault(lx => lx.MaLX == selectedMaLoaiXe);
					if (loaiXe == null) return;

					int soCho = loaiXe.SoCho;
					if (soCho == 0) return;
					
					bool isGiuongNam = loaiXe.TenLX.ToLower().Contains("giường");

					// Lấy danh sách ghế đã đặt
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
						// Xe giường nằm: 2 tầng
						gbTangDuoi.Visible = true;
						gbTangTren.Visible = true;

						int choMoiTang = soCho / 2;
						CreateSeatButtons(flpTangDuoi, 1, choMoiTang);
						CreateSeatButtons(flpTangTren, choMoiTang + 1, soCho);
					}
					else
					{
						// Xe ghế ngồi: 1 tầng
						gbTangDuoi.Visible = true;
						gbTangTren.Visible = false;
						gbTangDuoi.Text = "Sơ đồ ghế";

						CreateSeatButtons(flpTangDuoi, 1, soCho);
					}

					pnlSeatMap.Visible = true;
					UpdateThongTinChuyen();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi tải sơ đồ ghế: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
					// Ghế đã đặt
					btnSeat.BackColor = Color.FromArgb(217, 83, 79);
					btnSeat.ForeColor = Color.White;
					btnSeat.Enabled = false;
				}
				else
				{
					// Ghế trống
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
				// Bỏ chọn
				selectedSeats.Remove(seatNumber);
				btn.BackColor = Color.FromArgb(0, 199, 174); // Xanh - trống
			}
			else
			{
				// Chọn ghế
				selectedSeats.Add(seatNumber);
				btn.BackColor = Color.FromArgb(245, 128, 37); // Cam - đang chọn
			}

			// Debug: Hiển thị số ghế đã chọn
			Console.WriteLine($"Ghế đã chọn: {string.Join(", ", selectedSeats)}");
			Console.WriteLine($"Tổng số ghế: {selectedSeats.Count}");
			Console.WriteLine($"Giá vé: {selectedGiaVe}");

			UpdateThongTinChuyen();
			
			// Hiển thị panel thông tin khách hàng nếu đã chọn ghế
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
					: "Chưa chọn";

				// ✅ Tạo text với line breaks rõ ràng
				lblThongTinChuyen.Text = string.Format(
					"Tuyến: {0} → {1}{2}" +
					"Ngày: {3}{4}" +
					"Ghế đã chọn: {5}",
					selectedDiemDi, 
					selectedDiemDen,
					Environment.NewLine,
					selectedNgayDi.ToString("dd/MM/yyyy"),
					Environment.NewLine,
					gheDaChon
				);

				decimal tongTien = selectedSeats.Count * selectedGiaVe.Value;
				lblTongTien.Text = $"Tổng: {tongTien:#,##0} VNĐ";

				// ✅ Auto-size label để hiển thị đầy đủ
				lblThongTinChuyen.AutoSize = true;
				lblThongTinChuyen.MaximumSize = new Size(1200, 0); // Giới hạn rộng, không giới hạn cao

				// Debug
				Console.WriteLine($"UpdateThongTinChuyen - Số ghế: {selectedSeats.Count}, Giá: {selectedGiaVe}, Tổng: {tongTien}");
			}
			else
			{
				// Debug: Kiểm tra giá trị null
				Console.WriteLine($"selectedMaChuyen: {selectedMaChuyen}, selectedGiaVe: {selectedGiaVe}");
				
				lblThongTinChuyen.Text = "Chưa chọn chuyến";
				lblTongTien.Text = "Tổng: 0 VNĐ";
			}
		}
		#endregion

		#region Bước 4: Đặt vé
		private void btnTimKH_Click(object sender, EventArgs e)
		{
			// Validate SDT
			if (string.IsNullOrWhiteSpace(txtSDT.Text))
			{
				MessageBox.Show("Vui lòng nhập số điện thoại để tìm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
						// Tìm thấy - Đổ dữ liệu vào textbox
						txtTenKH.Text = khachHang.TenKH;
						dtpNamSinh.Value = khachHang.NamSinh; // ✅ FIX: NamSinh is DateTime, not DateTime?
						cboGioiTinh.SelectedIndex = (khachHang.GioiTinh ?? false) ? 0 : 1; // true = Nam, false = Nữ

						// Disable các field
						txtTenKH.ReadOnly = true;
						dtpNamSinh.Enabled = false;
						cboGioiTinh.Enabled = false;

						txtTenKH.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);

						MessageBox.Show(
							$"Đã tìm thấy khách hàng:\n{khachHang.TenKH}",
							"Thông báo",
							MessageBoxButtons.OK,
							MessageBoxIcon.Information
						);
					}
					else
					{
						// Không tìm thấy - Enable nhập mới
						txtTenKH.Clear();
						dtpNamSinh.Value = DateTime.Now.AddYears(-20);
						cboGioiTinh.SelectedIndex = 0;

						txtTenKH.ReadOnly = false;
						dtpNamSinh.Enabled = true;
						cboGioiTinh.Enabled = true;

						txtTenKH.BackColor = System.Drawing.Color.White;

						MessageBox.Show(
							"Không tìm thấy khách hàng với số điện thoại này.\n" +
							"Vui lòng nhập thông tin khách hàng mới.",
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
				MessageBox.Show("Lỗi tìm khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnDatVe_Click(object sender, EventArgs e)
		{
			// Debug log đầu tiên
			Console.WriteLine("=== BẮT ĐẦU ĐẶT VÉ ===");
			Console.WriteLine($"selectedMaChuyen: {selectedMaChuyen}");
			Console.WriteLine($"selectedGiaVe: {selectedGiaVe}");
			Console.WriteLine($"selectedSeats.Count: {selectedSeats.Count}");
			Console.WriteLine($"Ghế đã chọn: {string.Join(", ", selectedSeats)}");

			// Validate
			if (!selectedMaChuyen.HasValue)
			{
				MessageBox.Show("Vui lòng chọn chuyến xe.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (selectedSeats.Count == 0)
			{
				MessageBox.Show("Vui lòng chọn ít nhất một ghế.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (!selectedGiaVe.HasValue || selectedGiaVe.Value == 0)
			{
				MessageBox.Show("Giá vé không hợp lệ. Vui lòng chọn lại chuyến xe.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (string.IsNullOrWhiteSpace(txtTenKH.Text))
			{
				MessageBox.Show("Vui lòng nhập tên khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtTenKH.Focus();
				return;
			}

			if (string.IsNullOrWhiteSpace(txtSDT.Text))
			{
				MessageBox.Show("Vui lòng nhập số điện thoại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtSDT.Focus();
				return;
			}

			if (txtSDT.Text.Length < 10 || txtSDT.Text.Length > 11)
			{
				MessageBox.Show("Số điện thoại phải có 10-11 chữ số.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtSDT.Focus();
				return;
			}

			if (cboGioiTinh.SelectedItem == null)
			{
				MessageBox.Show("Vui lòng chọn giới tính.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// Tính tổng tiền trước khi confirm
			decimal tienTam = selectedSeats.Count * selectedGiaVe.Value;

			// ✅ Hiển thị dialog xác nhận với phương thức thanh toán
			DateTime ngayKhoiHanh = DateTime.Now;
			using (var db = new BXMDEntities())
			{
				var chuyenXe = db.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == selectedMaChuyen);
				if (chuyenXe != null)
				{
					ngayKhoiHanh = chuyenXe.NgayGioKhoiHanh;
				}
			}

			// ✅ Lấy phương thức thanh toán từ dialog
			string phuongThucThanhToan = "Tiền mặt";
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

				// ✅ FIX: Lưu phương thức thanh toán từ dialog
				phuongThucThanhToan = confirmDialog.PhuongThucThanhToan;
				Console.WriteLine($"✅ Phương thức thanh toán đã chọn: {phuongThucThanhToan}");
			}

			// ✅ Nếu chọn MoMo → Xử lý thanh toán MoMo
			if (phuongThucThanhToan == "MoMo")
			{
				ProcessMoMoPayment(tienTam);
				return;
			}

			// ✅ Tiếp tục với thanh toán tiền mặt
			try
			{
				int maHD = 0;
				int soLuongVe = 0;
				decimal tongTien = 0;
				string tenNguoiTaoVe = "Khách hàng"; // ✅ Khai báo ở ngoài using block

				using (var db = new BXMDEntities())
				{
					// ✅ 1. Kiểm tra chuyến xe còn tồn tại không
					var chuyenXe = db.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == selectedMaChuyen);
					if (chuyenXe == null)
					{
						MessageBox.Show("Chuyến xe không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					// ✅ 1.5. Kiểm tra ngày khởi hành phải lớn hơn hiện tại
					if (chuyenXe.NgayGioKhoiHanh <= DateTime.Now)
					{
						MessageBox.Show(
							$"Không thể đặt vé cho chuyến xe đã khởi hành hoặc sắp khởi hành.\n\n" +
							$"Ngày khởi hành: {chuyenXe.NgayGioKhoiHanh:dd/MM/yyyy HH:mm}\n" +
							$"Hiện tại: {DateTime.Now:dd/MM/yyyy HH:mm}",
							"Lỗi",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error
						);
						return;
					}

					// ✅ 1.6. Kiểm tra giá vé thực tế
					if (!chuyenXe.GiaVeThucTe.HasValue || chuyenXe.GiaVeThucTe.Value <= 0)
					{
						MessageBox.Show(
							$"Chuyến xe này chưa có giá vé hoặc giá vé = 0.\n\n" +
							$"Mã chuyến: {chuyenXe.MaChuyen}\n" +
							$"Vui lòng liên hệ quản trị viên để cập nhật giá vé.",
							"Lỗi",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error
						);
						return;
					}

					// ✅ 2. Kiểm tra ghế có bị đặt trùng không
					var daDat = db.VeXes
						.Where(v => v.MaChuyen == selectedMaChuyen && selectedSeats.Contains(v.ChoNgoi))
						.Select(v => v.ChoNgoi)
						.ToList();

					if (daDat.Any())
					{
						MessageBox.Show(
							$"Ghế số {string.Join(", ", daDat)} đã được đặt bởi người khác.\nVui lòng chọn ghế khác.",
							"Thông báo",
							MessageBoxButtons.OK,
							MessageBoxIcon.Warning
						);
						LoadSeatMap(); // Reload lại sơ đồ ghế
						return;
					}

					// ✅ 3. Tìm hoặc tạo khách hàng
					var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.SDT == txtSDT.Text.Trim());
					
					if (khachHang == null)
					{
						// ✅ Validate dữ liệu trước khi tạo
						if (string.IsNullOrWhiteSpace(txtTenKH.Text))
						{
							MessageBox.Show("Tên khách hàng không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}

						khachHang = new KhachHang
						{
							TenKH = txtTenKH.Text.Trim(),
							NamSinh = dtpNamSinh.Value.Date, // ✅ Chỉ lấy ngày, bỏ giờ
							SDT = txtSDT.Text.Trim(),
							GioiTinh = cboGioiTinh.SelectedItem.ToString() == "Nam",
							MaTK = null
						};

						db.KhachHangs.Add(khachHang);
						
						try
						{
							db.SaveChanges(); // ✅ Save riêng để catch lỗi KH
							Console.WriteLine($"✅ Đã tạo KH mới: MaKH = {khachHang.MaKH}");
						}
						catch (Exception exKH)
						{
							MessageBox.Show(
								$"Lỗi tạo khách hàng:\n{exKH.Message}\n" +
								(exKH.InnerException != null ? exKH.InnerException.Message : ""),
								"Lỗi",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error
							);
							return;
						}
					}
					else
					{
						Console.WriteLine($"✅ Sử dụng KH cũ: MaKH = {khachHang.MaKH}");
					}

					// ✅ 4. Tạo hóa đơn
					var hoaDon = new HoaDon
					{
						MaKH = khachHang.MaKH,
						MANV = currentNhanVien?.MaNV, // ✅ Lưu nhân viên thực hiện (nếu có)
						NgayLap = DateTime.Now,
						TongTien = 0, // ✅ Để trigger tự động tính
						TrangThai = false // ✅ Chưa thanh toán
					};

					db.HoaDons.Add(hoaDon);
					
					try
					{
						db.SaveChanges(); // ✅ Save riêng để catch lỗi HoaDon
						Console.WriteLine($"✅ Đã tạo hóa đơn: MaHD = {hoaDon.MaHD}");
					}
					catch (Exception exHD)
					{
						var innerMsg = exHD.InnerException?.InnerException?.Message ?? exHD.InnerException?.Message ?? "";
						MessageBox.Show(
							$"Lỗi tạo hóa đơn:\n{exHD.Message}\n\nChi tiết:\n{innerMsg}",
							"Lỗi",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error
						);
						Console.WriteLine($"❌ Lỗi HoaDon: {exHD.ToString()}");
						return;
					}

					// ✅ 5. Tạo vé xe
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
						db.SaveChanges(); // ✅ Save vé xe
						Console.WriteLine($"✅ Đã tạo {selectedSeats.Count} vé xe");
					}
					catch (Exception exVe)
					{
						var innerMsg = exVe.InnerException?.InnerException?.Message ?? exVe.InnerException?.Message ?? "";
						MessageBox.Show(
							$"Lỗi tạo vé xe:\n{exVe.Message}\n\nChi tiết:\n{innerMsg}",
							"Lỗi",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error
						);
						Console.WriteLine($"❌ Lỗi VeXe: {exVe.ToString()}");
						return;
					}

					// ✅ Refresh để lấy TongTien đã được trigger tính
					db.Entry(hoaDon).Reload();

					// ✅ 6. Tạo thanh toán và cập nhật trạng thái hóa đơn
					var thanhToan = new ThanhToan
					{
						MaHD = hoaDon.MaHD,
						TongTien = hoaDon.TongTien ?? 0,
						NgayTT = DateTime.Now,
						PhuongThuc = phuongThucThanhToan
					};
					db.ThanhToans.Add(thanhToan);

					// ✅ Đánh dấu hóa đơn đã thanh toán
					hoaDon.TrangThai = true;

					try
					{
						db.SaveChanges(); // ✅ Save thanh toán và cập nhật trạng thái
						Console.WriteLine($"✅ Đã tạo thanh toán: MaTT = {thanhToan.MaTT}");
					}
					catch (Exception exTT)
					{
						var innerMsg = exTT.InnerException?.InnerException?.Message ?? exTT.InnerException?.Message ?? "";
						MessageBox.Show(
							$"Lỗi tạo thanh toán:\n{exTT.Message}\n\nChi tiết:\n{innerMsg}",
							"Lỗi",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error
						);
						Console.WriteLine($"❌ Lỗi ThanhToan: {exTT.ToString()}");
						return;
					}

					// ✅ Lưu thông tin để hiển thị
					maHD = hoaDon.MaHD;
					soLuongVe = selectedSeats.Count;
					tongTien = hoaDon.TongTien ?? 0;
					
					// ✅ Lấy tên nhân viên để hiển thị
					tenNguoiTaoVe = currentNhanVien != null ? currentNhanVien.TenNV : "Khách hàng";
				}

				MessageBox.Show(
					$"Đặt vé thành công!\n\n" +
					$"Mã hóa đơn: {maHD}\n" +
					$"Số lượng vé: {soLuongVe}\n" +
					$"Tổng tiền: {tongTien:#,##0} VNĐ\n" +
					$"Phương thức thanh toán: {phuongThucThanhToan}\n" +
					$"Người tạo vé: {tenNguoiTaoVe}\n" +
					$"Trạng thái: Đã thanh toán",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);

				// Reset form
				ResetForm();
			}
			catch (Exception ex)
			{
				// ✅ Hiển thị chi tiết lỗi
				string errorMsg = $"Lỗi đặt vé:\n{ex.Message}";
				
				if (ex.InnerException != null)
				{
					errorMsg += $"\n\nChi tiết:\n{ex.InnerException.Message}";
				}

				// ✅ Log full stack trace
				Console.WriteLine("=== FULL ERROR ===");
				Console.WriteLine(ex.ToString());

				MessageBox.Show(errorMsg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Xử lý thanh toán MoMo
		/// </summary>
		private async void ProcessMoMoPayment(decimal amount)
		{
			try
			{
				// ✅ 1. TẠO HÓA ĐƠN VÀ VÉ TẠM THỜI TRƯỚC (TrangThai = false)
				int maHD = 0;
				string orderId = $"HD{DateTime.Now:yyyyMMddHHmmss}";
				
				using (var db = new BXMDEntities())
				{
					// Kiểm tra chuyến xe
					var chuyenXe = db.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == selectedMaChuyen);
					if (chuyenXe == null)
					{
						MessageBox.Show("Chuyến xe không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					// Kiểm tra ghế trùng
					var daDat = db.VeXes
						.Where(v => v.MaChuyen == selectedMaChuyen && selectedSeats.Contains(v.ChoNgoi))
						.Select(v => v.ChoNgoi)
						.ToList();

					if (daDat.Any())
					{
						MessageBox.Show(
							$"Ghế số {string.Join(", ", daDat)} đã được đặt.\nVui lòng chọn ghế khác.",
							"Thông báo",
							MessageBoxButtons.OK,
							MessageBoxIcon.Warning
						);
						LoadSeatMap();
						return;
					}

					// Tạo/lấy khách hàng
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

					// ✅ Tạo hóa đơn TẠM THỜI (TrangThai = false = chờ thanh toán)
					var hoaDon = new HoaDon
					{
						MaKH = khachHang.MaKH,
						MANV = currentNhanVien?.MaNV,
						NgayLap = DateTime.Now,
						TongTien = 0,
						TrangThai = false // ❌ CHỜ THANH TOÁN
					};
					db.HoaDons.Add(hoaDon);
					db.SaveChanges();

					// ✅ Tạo vé xe
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

					// Refresh để lấy TongTien
					db.Entry(hoaDon).Reload();

					// ✅ Lưu MaHD để tracking
					maHD = hoaDon.MaHD;
					
					Console.WriteLine($"✅ Đã tạo hóa đơn tạm: MaHD = {maHD}, OrderId = {orderId}");
				}

				// ✅ 2. GỌI MOMO PAYMENT
				this.Cursor = Cursors.WaitCursor;
				
				string orderInfo = $"Thanh toan ve xe - HD{maHD}";
				
				var momoService = new Services.MoMoPaymentService();
				
				// ✅ Thêm maHD vào extraData để tracking
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
					
					// Thành công - Mở trình duyệt
					MessageBox.Show(
						$"✅ Đã tạo đơn hàng tạm thời!\n\n" +
						$"Mã hóa đơn: {maHD}\n" +
						$"Mã đơn MoMo: {orderId}\n\n" +
						$"Đang chuyển đến trang thanh toán MoMo...\n" +
						$"Vui lòng hoàn tất thanh toán trong trình duyệt.",
						"Thanh toán MoMo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);


System.Diagnostics.Process.Start(response.PayUrl);

					// ✅ Sau khi mở MoMo, bắt đầu Polling kiểm tra trạng thái
					StartPollingPaymentStatus(orderId, maHD, amount);

					// ✅ 3. CLEAR FORM NGAY SAU KHI TẠO ĐƠN
					ResetForm();

					MessageBox.Show(
						"📌 Hướng dẫn:\n\n" +
						"1. Hoàn tất thanh toán trên MoMo\n" +
						"2. Sau khi thanh toán thành công, hệ thống sẽ tự động cập nhật trạng thái\n" +
						"3. Kiểm tra lại trong 'Quản lý vé' hoặc 'Lịch sử mua vé'\n\n" +
						$"Mã hóa đơn: {maHD}\n" +
						$"Trạng thái hiện tại: Chờ thanh toán",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);
				}
				else
				{
					// Lỗi - XÓA HÓA ĐƠN TẠM
					using (var db = new BXMDEntities())
					{
						var hoaDonCancel = db.HoaDons.FirstOrDefault(h => h.MaHD == maHD);
						if (hoaDonCancel != null)
						{
							// Xóa vé trước
							var veXeCancel = db.VeXes.Where(v => v.MaHD == maHD).ToList();
							db.VeXes.RemoveRange(veXeCancel);
							
							// Xóa hóa đơn
							db.HoaDons.Remove(hoaDonCancel);
							db.SaveChanges();
							
							Console.WriteLine($"❌ Đã hủy hóa đơn tạm: MaHD = {maHD}");
						}
					}

					MessageBox.Show(
						$"❌ Lỗi tạo yêu cầu thanh toán MoMo:\n\n" +
						$"Mã lỗi: {response.ResultCode}\n" +
						$"Thông báo: {response.Message}\n\n" +
						$"Vui lòng thử lại hoặc chọn phương thức thanh toán khác.",
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
			const int maxAttempts = 12; // 12 lần * 5s = 1 phút
			const int delayMs = 5000;

			MessageBox.Show("⏳ Đang chờ người dùng hoàn tất thanh toán MoMo...", "MoMo", MessageBoxButtons.OK, MessageBoxIcon.Information);

			while (attempt < maxAttempts)
			{
				attempt++;
				string status = await QueryMomoOrder(orderId);

				if (status == "Success")
				{
					MessageBox.Show("✅ Thanh toán MoMo thành công!", "MoMo", MessageBoxButtons.OK, MessageBoxIcon.Information);
					UpdateHoaDonTrangThai(maHD, true, "MoMo");
					return;
				}
				else if (status == "Failed")
				{
					MessageBox.Show("❌ Thanh toán MoMo thất bại hoặc bị hủy!", "MoMo", MessageBoxButtons.OK, MessageBoxIcon.Error);
					UpdateHoaDonTrangThai(maHD, false, "MoMo");
					return;
				}

				await Task.Delay(delayMs);
			}

			MessageBox.Show("⏰ Hết thời gian chờ thanh toán MoMo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
				Console.WriteLine("❌ QueryMoMoOrder Error: " + ex.Message);
				return "Pending";
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
						$"💰 Hóa đơn #{maHD} đã được cập nhật trạng thái: ĐÃ THANH TOÁN ({phuongThuc}).",
						"Thành công",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);
				}
				else
				{
					MessageBox.Show(
						$"⚠️ Hóa đơn #{maHD} đã được cập nhật: THANH TOÁN THẤT BẠI.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi cập nhật hóa đơn: " + ex.Message);
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

			// ✅ Enable lại các field
			txtTenKH.ReadOnly = false;
			dtpNamSinh.Enabled = true;
			cboGioiTinh.Enabled = true;

			txtTenKH.BackColor = System.Drawing.Color.White;
			dtpNamSinh.BackColor = System.Drawing.Color.White;
			cboGioiTinh.BackColor = System.Drawing.Color.White;

			pnlResults.Visible = false;
			pnlSeatMap.Visible = false;
			pnlThongTinKH.Visible = false;

			lblThongTinChuyen.Text = "Chưa chọn chuyến";
			lblTongTien.Text = "Tổng: 0 VNĐ";
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
