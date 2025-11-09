using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.BUS;
using QLBXMD.EF;

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
			LoadDiemDiDiemDen();
			dtpNgayDi.Value = DateTime.Now;
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

					cboDiemDi.DataSource = new List<string>(diemDi);
					cboDiemDen.DataSource = new List<string>(diemDen);

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
					var query = (from cx in db.ChuyenXes
								join x in db.Xes on cx.MaXe equals x.MaXe
								join nx in db.NhaXes on x.MaNX equals nx.MaNX
								join lx in db.LoaiXes on x.MaLX equals lx.MaLX
								join td in db.TuyenDuongs on cx.MaTuyen equals td.MaTuyen
								where td.DiemDi == selectedDiemDi
									  && td.DiemDen == selectedDiemDen
									  && System.Data.Entity.DbFunctions.TruncateTime(cx.NgayGioKhoiHanh) == selectedNgayDi
									  && cx.NgayGioKhoiHanh > DateTime.Now
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
									SoChoTrong = lx.SoCho - db.VeXes.Count(v => v.MaChuyen == cx.MaChuyen)
								}).ToList();

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
					}

					if (!query.Any())
					{
						MessageBox.Show("Không tìm thấy chuyến xe phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
						pnlResults.Visible = false;
						return;
					}

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
							item.TenNX, item.TenLX, item.SoCho, item.SoChoTrong, item.GiaVe);
						flpChuyenXe.Controls.Add(card);
					}
					
					flpChuyenXe.ResumeLayout(); // Resume layout updates
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi tải danh sách chuyến xe: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private Panel CreateChuyenXeCard(int maChuyen, int maXe, int maLoaiXe, 
			DateTime khoiHanh, DateTime denDuKien, string tenNX, string tenLX, 
			int soCho, int soChoTrong, decimal giaVe)
		{
			// Dynamic width based on container - leave room for scrollbar
			int cardWidth = flpChuyenXe.Width - 40;
			if (cardWidth < 800) cardWidth = 800; // Minimum width
			
			Panel card = new Panel
			{
				Width = cardWidth,
				Height = 120,
				BorderStyle = BorderStyle.FixedSingle,
				BackColor = Color.White,
				Margin = new Padding(5, 5, 5, 10)
			};

			// Logo xe
			Label lblBusIcon = new Label
			{
				Text = "🚌",
				Location = new Point(15, 15),
				Size = new Size(50, 50),
				Font = new Font("Segoe UI", 24F),
				TextAlign = ContentAlignment.MiddleCenter,
				BackColor = Color.FromArgb(240, 240, 240)
			};

			// Tên nhà xe
			Label lblTenNX = new Label
			{
				Text = tenNX,
				Location = new Point(75, 15),
				AutoSize = false,
				Width = 300,
				Height = 25,
				Font = new Font("Segoe UI", 11F, FontStyle.Bold),
				ForeColor = Color.FromArgb(51, 51, 51)
			};

			// Loại xe
			Label lblLoaiXe = new Label
			{
				Text = $"{tenLX} - {soCho} chỗ",
				Location = new Point(75, 42),
				AutoSize = false,
				Width = 250,
				Height = 18,
				Font = new Font("Segoe UI", 8.5F),
				ForeColor = Color.Gray
			};

			// Giờ đi
			Label lblGioDi = new Label
			{
				Text = khoiHanh.ToString("HH:mm"),
				Location = new Point(75, 70),
				AutoSize = false,
				Width = 60,
				Height = 25,
				Font = new Font("Segoe UI", 13F, FontStyle.Bold),
				ForeColor = Color.FromArgb(0, 150, 136)
			};

			Label lblMuiTen = new Label
			{
				Text = "→",
				Location = new Point(140, 70),
				AutoSize = false,
				Width = 25,
				Height = 25,
				Font = new Font("Segoe UI", 12F),
				ForeColor = Color.Gray,
				TextAlign = ContentAlignment.MiddleCenter
			};

			// Giờ đến
			Label lblGioDen = new Label
			{
				Text = denDuKien.ToString("HH:mm"),
				Location = new Point(170, 70),
				AutoSize = false,
				Width = 60,
				Height = 25,
				Font = new Font("Segoe UI", 13F, FontStyle.Bold),
				ForeColor = Color.FromArgb(0, 150, 136)
			};

			// Ngày đi
			Label lblNgayDi = new Label
			{
				Text = khoiHanh.ToString("dd/MM/yyyy"),
				Location = new Point(240, 75),
				AutoSize = false,
				Width = 100,
				Height = 18,
				Font = new Font("Segoe UI", 8.5F),
				ForeColor = Color.Gray
			};

			// Số chỗ trống
			Label lblChoTrong = new Label
			{
				Text = soChoTrong > 0 ? $"Còn {soChoTrong} chỗ" : "Hết chỗ",
				Location = new Point(360, 75),
				AutoSize = false,
				Width = 120,
				Height = 20,
				Font = new Font("Segoe UI", 8.5F),
				ForeColor = soChoTrong > 0 ? Color.Green : Color.Red
			};

			// Giá vé (position from right)
			Label lblGiaVe = new Label
			{
				Text = $"{giaVe:#,##0} VNĐ",
				Location = new Point(cardWidth - 280, 30),
				AutoSize = false,
				Width = 150,
				Height = 30,
				Font = new Font("Segoe UI", 13F, FontStyle.Bold),
				ForeColor = Color.FromArgb(244, 67, 54),
				TextAlign = ContentAlignment.MiddleRight
			};

			// Button chọn (position from right)
			Button btnChon = new Button
			{
				Text = "Chọn chuyến",
				Location = new Point(cardWidth - 155, 70),
				Size = new Size(135, 35),
				BackColor = Color.FromArgb(0, 150, 136),
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
				Font = new Font("Segoe UI", 10F, FontStyle.Bold),
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
			
			btnChon.Click += (s, e) => {
				try
				{
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
				using (var db = new BXMDEntities())
				{
					// Lấy thông tin khách hàng
					var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.MaTK == currentUser.MaTK);
					if (khachHang == null)
					{
						MessageBox.Show("Không tìm thấy thông tin khách hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					// Kiểm tra chuyến xe
					var chuyenXe = db.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == selectedMaChuyen);
					if (chuyenXe == null)
					{
						MessageBox.Show("Chuyến xe không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					if (chuyenXe.NgayGioKhoiHanh <= DateTime.Now)
					{
						MessageBox.Show("Không thể đặt vé cho chuyến xe đã khởi hành.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					// Kiểm tra ghế đã đặt
					var daDat = db.VeXes
						.Where(v => v.MaChuyen == selectedMaChuyen && selectedSeats.Contains(v.ChoNgoi))
						.Select(v => v.ChoNgoi)
						.ToList();

					if (daDat.Any())
					{
						MessageBox.Show($"Ghế số {string.Join(", ", daDat)} đã được đặt.\nVui lòng chọn ghế khác.", 
							"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						LoadSeatMap();
						return;
					}

					// Hiển thị dialog xác nhận
					decimal tongTien = selectedSeats.Count * (selectedGiaVe ?? 0);
					var result = MessageBox.Show(
						$"Xác nhận đặt vé?\n\n" +
						$"Khách hàng: {khachHang.TenKH}\n" +
						$"Tuyến: {selectedDiemDi} → {selectedDiemDen}\n" +
						$"Ngày đi: {chuyenXe.NgayGioKhoiHanh:dd/MM/yyyy HH:mm}\n" +
						$"Ghế: {string.Join(", ", selectedSeats.OrderBy(s => s))}\n" +
						$"Tổng tiền: {tongTien:#,##0} VNĐ",
						"Xác nhận đặt vé",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Question
					);

					if (result != DialogResult.Yes) return;

					// Tạo hóa đơn
					var hoaDon = new HoaDon
					{
						MaKH = khachHang.MaKH,
						MANV = null,
						NgayLap = DateTime.Now,
						TongTien = 0,
						TrangThai = false
					};
					db.HoaDons.Add(hoaDon);
					db.SaveChanges();

					// Tạo vé xe
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

					// Refresh hóa đơn
					db.Entry(hoaDon).Reload();

					MessageBox.Show(
						$"Đặt vé thành công!\n" +
						$"Mã hóa đơn: {hoaDon.MaHD}\n" +
						$"Số lượng vé: {selectedSeats.Count}\n" +
						$"Tổng tiền: {hoaDon.TongTien:#,##0} VNĐ",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);

					ResetForm();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi đặt vé: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

			pnlResults.Visible = false;
			pnlSeatMap.Visible = false;

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
				var temp = cboDiemDi.SelectedItem;
				cboDiemDi.SelectedItem = cboDiemDen.SelectedItem;
				cboDiemDen.SelectedItem = temp;
			}
		}
		#endregion
	}
}
