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

			// Xử lý thanh toán MoMo trực tiếp, bỏ lựa chọn tiền mặt
			await ProcessMoMoPayment();
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
					System.Diagnostics.Process.Start(response.PayUrl);

					var confirmResult = MessageBox.Show(
						"Đang chuyển đến trang thanh toán MoMo...\n\n" +
						"Sau khi thanh toán thành công trên trình duyệt, vui lòng nhấn 'OK' để hệ thống kiểm tra và cập nhật trạng thái đơn hàng.\n" +
						$"Mã đơn hàng: {orderId}",
						"Xác nhận thanh toán MoMo",
						MessageBoxButtons.OKCancel,
						MessageBoxIcon.Information
					);

					if (confirmResult == DialogResult.OK)
					{
						this.Cursor = Cursors.WaitCursor;
						var queryResponse = await momoService.QueryPaymentStatus(orderId);
						this.Cursor = Cursors.Default;

						if (queryResponse.ResultCode == 0) // Thành công
						{
							using (var db = new BXMDEntities())
							{
								var hoaDonDb = db.HoaDons.FirstOrDefault(hd => hd.MaHD == maHD);
								if (hoaDonDb != null && hoaDonDb.TrangThai != true)
								{
									// Tạo bản ghi thanh toán
									var thanhToan = new ThanhToan
									{
										MaHD = hoaDonDb.MaHD,
										TongTien = hoaDonDb.TongTien ?? 0,
										NgayTT = DateTime.Now,
										PhuongThuc = "MoMo"
									};
									db.ThanhToans.Add(thanhToan);

									// Cập nhật trạng thái hóa đơn
									hoaDonDb.TrangThai = true;
									db.SaveChanges();

									MessageBox.Show("Thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
									LoadChiTietHoaDon();
								}
							}
						}
						else
						{
							MessageBox.Show(
								$"Giao dịch chưa hoàn tất hoặc thất bại.\n\nMã lỗi: {queryResponse.ResultCode}\nThông báo: {queryResponse.Message}\n\nVui lòng thử lại sau hoặc liên hệ hỗ trợ.",
								"Kiểm tra thanh toán",
								MessageBoxButtons.OK,
								MessageBoxIcon.Warning
							);
						}
					}
				}
				else
				{
					MessageBox.Show(
						$"Lỗi tạo yêu cầu thanh toán MoMo:\n\n" +
						$"Mã lỗi: {response.ResultCode}\n" +
						$"Thông báo: {response.Message}\n\n" +
						$"Vui lòng thử lại sau.",
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
