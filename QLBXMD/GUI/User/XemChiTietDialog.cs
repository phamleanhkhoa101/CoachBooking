using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBXMD.EF;

namespace QLBXMD.GUI.User
{
	public partial class XemChiTietDialog : Form
	{
		private int maHD;
		private HoaDon hoaDon;
		private TaiKhoan currentUser;

		// Màu gốc của các nút
		private Color btnThanhToanColor = Color.FromArgb(0, 150, 136);
		private Color btnHuyVeColor = Color.FromArgb(244, 67, 54);

		public XemChiTietDialog()
		{
			InitializeComponent();
			SetupButtonEffects();
		}

		public XemChiTietDialog(int maHD, TaiKhoan user) : this()
		{
			this.maHD = maHD;
			this.currentUser = user;
			this.Load += XemChiTietDialog_Load;
		}

		private void SetupButtonEffects()
		{
			// Hiệu ứng cho nút Thanh toán
			btnThanhToan.MouseEnter += (s, e) => {
				btnThanhToan.BackColor = Color.FromArgb(0, 180, 164); // Màu sáng hơn
			};
			btnThanhToan.MouseLeave += (s, e) => {
				btnThanhToan.BackColor = btnThanhToanColor;
			};
			btnThanhToan.MouseDown += (s, e) => {
				btnThanhToan.BackColor = Color.FromArgb(0, 120, 110); // Màu tối hơn khi nhấn
			};
			btnThanhToan.MouseUp += (s, e) => {
				btnThanhToan.BackColor = Color.FromArgb(0, 180, 164);
			};

			// Hiệu ứng cho nút Hủy vé
			btnHuyVe.MouseEnter += (s, e) => {
				btnHuyVe.BackColor = Color.FromArgb(255, 87, 74); // Màu sáng hơn
			};
			btnHuyVe.MouseLeave += (s, e) => {
				btnHuyVe.BackColor = btnHuyVeColor;
			};
			btnHuyVe.MouseDown += (s, e) => {
				btnHuyVe.BackColor = Color.FromArgb(200, 50, 40); // Màu tối hơn khi nhấn
			};
			btnHuyVe.MouseUp += (s, e) => {
				btnHuyVe.BackColor = Color.FromArgb(255, 87, 74);
			};

			// Hiệu ứng cho nút Đóng
			btnDong.MouseEnter += (s, e) => {
				btnDong.BackColor = Color.FromArgb(180, 180, 180);
			};
			btnDong.MouseLeave += (s, e) => {
				btnDong.BackColor = Color.FromArgb(158, 158, 158);
			};
			btnDong.MouseDown += (s, e) => {
				btnDong.BackColor = Color.FromArgb(120, 120, 120);
			};
			btnDong.MouseUp += (s, e) => {
				btnDong.BackColor = Color.FromArgb(180, 180, 180);
			};

			// Hiệu ứng cho nút Close (X)
			btnClose.MouseEnter += (s, e) => {
				btnClose.BackColor = Color.FromArgb(244, 67, 54);
			};
			btnClose.MouseLeave += (s, e) => {
				btnClose.BackColor = Color.Transparent;
			};
		}

		private void XemChiTietDialog_Load(object sender, EventArgs e)
		{
			LoadChiTietHoaDon();
		}

		private void LoadChiTietHoaDon()
		{
			try
			{
				using (var db = new BXMDEntities())
				{
					hoaDon = db.HoaDons.FirstOrDefault(hd => hd.MaHD == maHD);
					if (hoaDon == null)
					{
						MessageBox.Show("Không tìm thấy hóa đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						this.Close();
						return;
					}

					var veXes = db.VeXes.Where(v => v.MaHD == maHD).ToList();
					if (!veXes.Any())
					{
						MessageBox.Show("Không có vé nào trong hóa đơn này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
						this.Close();
						return;
					}

					var veXeFirst = veXes.First();
					var chuyenXe = db.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == veXeFirst.MaChuyen);
					var tuyenDuong = chuyenXe != null ? db.TuyenDuongs.FirstOrDefault(td => td.MaTuyen == chuyenXe.MaTuyen) : null;
					var xe = chuyenXe != null ? db.Xes.FirstOrDefault(x => x.MaXe == chuyenXe.MaXe) : null;
					var nhaXe = xe != null ? db.NhaXes.FirstOrDefault(nx => nx.MaNX == xe.MaNX) : null;
					var loaiXe = xe != null ? db.LoaiXes.FirstOrDefault(lx => lx.MaLX == xe.MaLX) : null;

					// Thông tin hóa đơn
					lblMaHDValue.Text = $"#{hoaDon.MaHD}";
					lblNgayLapValue.Text = hoaDon.NgayLap.ToString("dd/MM/yyyy HH:mm");
					
					// Trạng thái thanh toán
					if (hoaDon.TrangThai == true)
					{
						lblTrangThaiValue.Text = "✓ Đã thanh toán";
						lblTrangThaiValue.ForeColor = Color.Green;
						
						// Ẩn nút thanh toán và hủy vé
						btnThanhToan.Visible = false;
						btnHuyVe.Visible = false;
						btnDong.Location = new Point(268, 24);
					}
					else
					{
						lblTrangThaiValue.Text = "⏳ Chưa thanh toán";
						lblTrangThaiValue.ForeColor = Color.Orange;
						
						// Hiển thị nút thanh toán và hủy vé
						btnThanhToan.Visible = true;
						btnHuyVe.Visible = true;
						btnDong.Visible = false;
					}

					// Thông tin chuyến xe
					if (tuyenDuong != null)
					{
						lblTuyenValue.Text = $"{tuyenDuong.DiemDi} → {tuyenDuong.DiemDen}";
					}
					else
					{
						lblTuyenValue.Text = "Không rõ";
					}

					if (nhaXe != null)
					{
						lblNhaXeValue.Text = nhaXe.TenNX;
					}
					else
					{
						lblNhaXeValue.Text = "Không rõ";
					}

					if (loaiXe != null)
					{
						lblLoaiXeValue.Text = loaiXe.TenLX;
					}
					else
					{
						lblLoaiXeValue.Text = "Không rõ";
					}

					if (chuyenXe != null)
					{
						lblKhoiHanhValue.Text = chuyenXe.NgayGioKhoiHanh.ToString("dd/MM/yyyy HH:mm");
						
						if (chuyenXe.NgayGioDenDuKien.HasValue)
						{
							lblDenDuKienValue.Text = chuyenXe.NgayGioDenDuKien.Value.ToString("dd/MM/yyyy HH:mm");
						}
						else
						{
							lblDenDuKienValue.Text = "Chưa xác định";
						}
					}

					// Thông tin vé
					lblGiaVeValue.Text = $"{veXes.First().GiaVe:#,##0} VNĐ";
					lblSoLuongValue.Text = veXes.Count.ToString();
					lblGheValue.Text = string.Join(", ", veXes.Select(v => v.ChoNgoi).OrderBy(g => g));

					// Tổng tiền
					lblTongTien.Text = $"💰 {hoaDon.TongTien:#,##0} VNĐ";
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi tải chi tiết hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				this.Close();
			}
		}

		private async void btnThanhToan_Click(object sender, EventArgs e)
		{
			if (hoaDon == null || hoaDon.TrangThai == true)
			{
				MessageBox.Show("Hóa đơn đã được thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			// Hiển thị dialog chọn phương thức thanh toán
			using (var dialog = new Form())
			{
				dialog.Text = "Chọn phương thức thanh toán";
				dialog.Size = new Size(400, 200);
				dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
				dialog.StartPosition = FormStartPosition.CenterParent;
				dialog.MaximizeBox = false;
				dialog.MinimizeBox = false;

				var lblPrompt = new Label
				{
					Text = "Vui lòng chọn phương thức thanh toán:",
					Location = new Point(20, 20),
					Size = new Size(350, 30),
					Font = new Font("Segoe UI", 10F)
				};

				var btnTienMat = new Button
				{
					Text = "💵 Tiền mặt",
					Location = new Point(20, 60),
					Size = new Size(150, 50),
					BackColor = Color.FromArgb(76, 175, 80),
					ForeColor = Color.White,
					FlatStyle = FlatStyle.Flat,
					Font = new Font("Segoe UI", 10F, FontStyle.Bold),
					Cursor = Cursors.Hand
				};
				btnTienMat.FlatAppearance.BorderSize = 0;
				btnTienMat.MouseEnter += (s, ev) => btnTienMat.BackColor = Color.FromArgb(96, 195, 100);
				btnTienMat.MouseLeave += (s, ev) => btnTienMat.BackColor = Color.FromArgb(76, 175, 80);
				btnTienMat.MouseDown += (s, ev) => btnTienMat.BackColor = Color.FromArgb(56, 155, 60);
				btnTienMat.MouseUp += (s, ev) => btnTienMat.BackColor = Color.FromArgb(96, 195, 100);
				btnTienMat.Click += (s, ev) => { dialog.Tag = "Tiền mặt"; dialog.DialogResult = DialogResult.OK; };

				var btnMoMo = new Button
				{
					Text = "📱 MoMo",
					Location = new Point(210, 60),
					Size = new Size(150, 50),
					BackColor = Color.FromArgb(163, 26, 92),
					ForeColor = Color.White,
					FlatStyle = FlatStyle.Flat,
					Font = new Font("Segoe UI", 10F, FontStyle.Bold),
					Cursor = Cursors.Hand
				};
				btnMoMo.FlatAppearance.BorderSize = 0;
				btnMoMo.MouseEnter += (s, ev) => btnMoMo.BackColor = Color.FromArgb(183, 46, 112);
				btnMoMo.MouseLeave += (s, ev) => btnMoMo.BackColor = Color.FromArgb(163, 26, 92);
				btnMoMo.MouseDown += (s, ev) => btnMoMo.BackColor = Color.FromArgb(143, 6, 72);
				btnMoMo.MouseUp += (s, ev) => btnMoMo.BackColor = Color.FromArgb(183, 46, 112);
				btnMoMo.Click += (s, ev) => { dialog.Tag = "MoMo"; dialog.DialogResult = DialogResult.OK; };

				var btnHuy = new Button
				{
					Text = "Hủy",
					Location = new Point(140, 120),
					Size = new Size(100, 35),
					BackColor = Color.Gray,
					ForeColor = Color.White,
					FlatStyle = FlatStyle.Flat,
					Font = new Font("Segoe UI", 9F),
					Cursor = Cursors.Hand
				};
				btnHuy.FlatAppearance.BorderSize = 0;
				btnHuy.MouseEnter += (s, ev) => btnHuy.BackColor = Color.FromArgb(120, 120, 120);
				btnHuy.MouseLeave += (s, ev) => btnHuy.BackColor = Color.Gray;
				btnHuy.MouseDown += (s, ev) => btnHuy.BackColor = Color.FromArgb(80, 80, 80);
				btnHuy.MouseUp += (s, ev) => btnHuy.BackColor = Color.FromArgb(120, 120, 120);
				btnHuy.Click += (s, ev) => { dialog.DialogResult = DialogResult.Cancel; };

				dialog.Controls.Add(lblPrompt);
				dialog.Controls.Add(btnTienMat);
				dialog.Controls.Add(btnMoMo);
				dialog.Controls.Add(btnHuy);

				if (dialog.ShowDialog() != DialogResult.OK)
					return;

				string phuongThuc = dialog.Tag?.ToString();
				if (string.IsNullOrEmpty(phuongThuc))
					return;

				// Xử lý thanh toán theo phương thức
				if (phuongThuc == "MoMo")
				{
					await ProcessMoMoPayment();
				}
				else // Tiền mặt
				{
					ProcessCashPayment();
				}
			}
		}

		private async Task ProcessMoMoPayment()
		{
			try
			{
				if (hoaDon.TongTien == null || hoaDon.TongTien <= 0)
				{
					MessageBox.Show("Số tiền thanh toán không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				this.Cursor = Cursors.WaitCursor;

				// Tạo orderId unique
				string orderId = $"HD{hoaDon.MaHD}_{DateTime.Now:yyyyMMddHHmmss}";
				string orderInfo = $"Thanh toan hoa don #{hoaDon.MaHD}";

				// Tạo request thanh toán MoMo
				var momoService = new Services.MoMoPaymentService();
				var response = await momoService.CreatePaymentRequest(
					orderId,
					(long)hoaDon.TongTien.Value,
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

					MessageBox.Show(
						"⚠️ Lưu ý:\n" +
						"- Sau khi thanh toán thành công trên MoMo, vui lòng quay lại ứng dụng.\n" +
						"- Hệ thống sẽ tự động xác nhận và cập nhật trạng thái đơn hàng.\n" +
						"- Nếu có vấn đề, vui lòng liên hệ quản trị viên.\n\n" +
						$"Mã đơn hàng: {orderId}",
						"Hướng dẫn",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);
				}
				else
				{
					MessageBox.Show(
						$"Lỗi tạo yêu cầu thanh toán MoMo:\n\n" +
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

		private void ProcessCashPayment()
		{
			try
			{
				var confirmResult = MessageBox.Show(
					$"Xác nhận thanh toán bằng tiền mặt?\n\n" +
					$"Mã hóa đơn: #{hoaDon.MaHD}\n" +
					$"Số tiền: {hoaDon.TongTien:#,##0} VNĐ",
					"Xác nhận thanh toán",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);

				if (confirmResult != DialogResult.Yes)
					return;

				using (var db = new BXMDEntities())
				{
					var hoaDonDb = db.HoaDons.FirstOrDefault(hd => hd.MaHD == maHD);
					if (hoaDonDb == null)
					{
						MessageBox.Show("Không tìm thấy hóa đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					if (hoaDonDb.TrangThai == true)
					{
						MessageBox.Show("Hóa đơn đã được thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
						return;
					}

					// Tạo bản ghi thanh toán
					var thanhToan = new ThanhToan
					{
						MaHD = hoaDonDb.MaHD,
						TongTien = hoaDonDb.TongTien ?? 0,
						NgayTT = DateTime.Now,
						PhuongThuc = "Tiền mặt"
					};
					db.ThanhToans.Add(thanhToan);

					// Cập nhật trạng thái hóa đơn
					hoaDonDb.TrangThai = true;

					db.SaveChanges();

					MessageBox.Show(
						$"Thanh toán thành công!\n\n" +
						$"Mã hóa đơn: #{hoaDonDb.MaHD}\n" +
						$"Số tiền: {hoaDonDb.TongTien:#,##0} VNĐ\n" +
						$"Phương thức: Tiền mặt\n" +
						$"Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm}",
						"Thanh toán thành công",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);

					// Reload lại thông tin
					LoadChiTietHoaDon();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"Lỗi thanh toán:\n{ex.Message}",
					"Lỗi",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				Console.WriteLine($"❌ Cash Payment Error: {ex.ToString()}");
			}
		}

		private void btnHuyVe_Click(object sender, EventArgs e)
		{
			if (hoaDon == null)
				return;

			if (hoaDon.TrangThai == true)
			{
				MessageBox.Show(
					"Không thể hủy vé đã thanh toán.\n\n" +
					"Vui lòng liên hệ quản trị viên để được hỗ trợ.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				return;
			}

			var confirmResult = MessageBox.Show(
				$"Bạn có chắc chắn muốn hủy vé?\n\n" +
				$"Mã hóa đơn: #{hoaDon.MaHD}\n" +
				$"Số tiền: {hoaDon.TongTien:#,##0} VNĐ\n\n" +
				$"Hành động này không thể hoàn tác!",
				"Xác nhận hủy vé",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning
			);

			if (confirmResult != DialogResult.Yes)
				return;

			try
			{
				using (var db = new BXMDEntities())
				{
					// Lấy hóa đơn và các vé
					var hoaDonDb = db.HoaDons.FirstOrDefault(hd => hd.MaHD == maHD);
					if (hoaDonDb == null)
					{
						MessageBox.Show("Không tìm thấy hóa đơn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					if (hoaDonDb.TrangThai == true)
					{
						MessageBox.Show("Không thể hủy vé đã thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return;
					}

					var veXes = db.VeXes.Where(v => v.MaHD == maHD).ToList();

					// Xóa các vé
					foreach (var ve in veXes)
					{
						db.VeXes.Remove(ve);
					}

					// Xóa hóa đơn
					db.HoaDons.Remove(hoaDonDb);

					db.SaveChanges();

					MessageBox.Show(
						"Hủy vé thành công!\n\n" +
						$"Mã hóa đơn: #{hoaDonDb.MaHD}\n" +
						$"Số vé đã hủy: {veXes.Count}",
						"Thành công",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);

					// Đóng dialog
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					$"Lỗi hủy vé:\n{ex.Message}",
					"Lỗi",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				Console.WriteLine($"❌ Cancel Ticket Error: {ex.ToString()}");
			}
		}

		private void btnDong_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
