using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.EF;

namespace QLBXMD.GUI.User
{
	public partial class UCLichSuMuaVe : UserControl
	{
		private TaiKhoan currentUser;

		public UCLichSuMuaVe()
		{
			InitializeComponent();
		}

		public UCLichSuMuaVe(TaiKhoan user) : this()
		{
			this.currentUser = user;
			this.Load += UCLichSuMuaVe_Load;
		}

		private void UCLichSuMuaVe_Load(object sender, EventArgs e)
		{
			LoadLichSuMuaVe();
		}

		private void LoadLichSuMuaVe()
		{
			if (currentUser == null)
			{
				MessageBox.Show("Vui lòng đăng nhập để xem lịch sử.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
						lblThongBao.Visible = true;
						lblThongBao.Text = "Không tìm thấy thông tin khách hàng.";
						return;
					}

					// Lấy danh sách hóa đơn
					var hoaDons = (from hd in db.HoaDons
								   where hd.MaKH == khachHang.MaKH
								   orderby hd.NgayLap descending
								   select hd).ToList();

					if (!hoaDons.Any())
					{
						lblThongBao.Visible = true;
						lblThongBao.Text = "Bạn chưa có lịch sử mua vé nào.";
						return;
					}

					lblThongBao.Visible = false;

					// Clear và thêm hóa đơn cards
					flpHoaDon.Controls.Clear();
					
					foreach (var hoaDon in hoaDons)
					{
						var card = CreateHoaDonCard(hoaDon);
						flpHoaDon.Controls.Add(card);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi tải lịch sử: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private Panel CreateHoaDonCard(HoaDon hoaDon)
		{
			int cardWidth = flpHoaDon.Width - 40;
			if (cardWidth < 1000) cardWidth = 1000;

			Panel card = new Panel
			{
				Width = cardWidth,
				Height = 220,
				BorderStyle = BorderStyle.FixedSingle,
				BackColor = Color.White,
				Margin = new Padding(5, 5, 5, 10),
				Padding = new Padding(20)
			};

			// Lấy thông tin chi tiết
			using (var db = new BXMDEntities())
			{
				var veXes = db.VeXes.Where(v => v.MaHD == hoaDon.MaHD).ToList();
				if (!veXes.Any())
				{
					Label lblEmpty = new Label
					{
						Text = "Không có thông tin vé",
						Location = new Point(20, 20),
						AutoSize = true,
						Font = new Font("Segoe UI", 10F),
						ForeColor = Color.Gray
					};
					card.Controls.Add(lblEmpty);
					return card;
				}

				var veXeFirst = veXes.First();
				var chuyenXe = db.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == veXeFirst.MaChuyen);
				if (chuyenXe == null)
				{
					Label lblEmpty = new Label
					{
						Text = "Không tìm thấy thông tin chuyến xe",
						Location = new Point(20, 20),
						AutoSize = true,
						Font = new Font("Segoe UI", 10F),
						ForeColor = Color.Gray
					};
					card.Controls.Add(lblEmpty);
					return card;
				}

				var tuyenDuong = db.TuyenDuongs.FirstOrDefault(td => td.MaTuyen == chuyenXe.MaTuyen);
				var xe = db.Xes.FirstOrDefault(x => x.MaXe == chuyenXe.MaXe);
				var nhaXe = xe != null ? db.NhaXes.FirstOrDefault(nx => nx.MaNX == xe.MaNX) : null;

				// Mã hóa đơn
				Label lblMaHD = new Label
				{
					Text = $"Mã HĐ: #{hoaDon.MaHD}",
					Location = new Point(20, 15),
					AutoSize = true,
					Font = new Font("Segoe UI", 11F, FontStyle.Bold),
					ForeColor = Color.FromArgb(0, 150, 136)
				};

				// Ngày lập
				Label lblNgayLap = new Label
				{
					Text = $"Ngày đặt: {hoaDon.NgayLap:dd/MM/yyyy HH:mm}",
					Location = new Point(20, 45),
					AutoSize = true,
					Font = new Font("Segoe UI", 9F),
					ForeColor = Color.Gray
				};

				// Trạng thái
				Label lblTrangThai = new Label
				{
					Text = hoaDon.TrangThai == true ? "✓ Đã thanh toán" : "⏳ Chưa thanh toán",
					Location = new Point(cardWidth - 200, 15),
					AutoSize = true,
					Font = new Font("Segoe UI", 10F, FontStyle.Bold),
					ForeColor = hoaDon.TrangThai == true ? Color.Green : Color.Orange
				};

				// Tuyến đường
				Label lblTuyen = new Label
				{
					Text = tuyenDuong != null ? $"🚌 {tuyenDuong.DiemDi} → {tuyenDuong.DiemDen}" : "Không rõ tuyến",
					Location = new Point(20, 75),
					AutoSize = true,
					Font = new Font("Segoe UI", 12F, FontStyle.Bold),
					ForeColor = Color.FromArgb(51, 51, 51)
				};

				// Nhà xe
				Label lblNhaXe = new Label
				{
					Text = nhaXe != null ? $"Nhà xe: {nhaXe.TenNX}" : "Không rõ nhà xe",
					Location = new Point(20, 105),
					AutoSize = true,
					Font = new Font("Segoe UI", 9F),
					ForeColor = Color.Gray
				};

				// Ngày khởi hành
				Label lblKhoiHanh = new Label
				{
					Text = $"Khởi hành: {chuyenXe.NgayGioKhoiHanh:dd/MM/yyyy HH:mm}",
					Location = new Point(20, 130),
					AutoSize = true,
					Font = new Font("Segoe UI", 10F, FontStyle.Bold),
					ForeColor = Color.FromArgb(244, 67, 54)
				};

				// Ghế đã đặt
				var gheNumbers = veXes.Select(v => v.ChoNgoi).OrderBy(g => g).ToList();
				Label lblGhe = new Label
				{
					Text = $"Ghế: {string.Join(", ", gheNumbers)}",
					Location = new Point(20, 160),
					AutoSize = true,
					Font = new Font("Segoe UI", 9F),
					ForeColor = Color.Gray
				};

				// Tổng tiền
				Label lblTongTien = new Label
				{
					Text = $"{hoaDon.TongTien:#,##0} VNĐ",
					Location = new Point(cardWidth - 200, 150),
					AutoSize = true,
					Font = new Font("Segoe UI", 16F, FontStyle.Bold),
					ForeColor = Color.FromArgb(244, 67, 54)
				};

				// Button xem chi tiết
				Button btnChiTiet = new Button
				{
					Text = "Xem chi tiết",
					Location = new Point(cardWidth - 150, 50),
					Size = new Size(120, 35),
					BackColor = Color.FromArgb(0, 150, 136),
					ForeColor = Color.White,
					FlatStyle = FlatStyle.Flat,
					Font = new Font("Segoe UI", 9F, FontStyle.Bold),
					Cursor = Cursors.Hand,
					Tag = hoaDon.MaHD
				};
				btnChiTiet.FlatAppearance.BorderSize = 0;
				btnChiTiet.Click += (s, e) => XemChiTiet(hoaDon.MaHD);

				// Add controls
				card.Controls.Add(lblMaHD);
				card.Controls.Add(lblNgayLap);
				card.Controls.Add(lblTrangThai);
				card.Controls.Add(lblTuyen);
				card.Controls.Add(lblNhaXe);
				card.Controls.Add(lblKhoiHanh);
				card.Controls.Add(lblGhe);
				card.Controls.Add(lblTongTien);
				card.Controls.Add(btnChiTiet);

				// Hover effect
				card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(250, 250, 250);
				card.MouseLeave += (s, e) => card.BackColor = Color.White;
			}

			return card;
		}

		private void XemChiTiet(int maHD)
		{
			try
			{
				// Mở dialog xem chi tiết với tính năng thanh toán và hủy vé
				using (var dialog = new XemChiTietDialog(maHD, currentUser))
				{
					var result = dialog.ShowDialog();
					
					// Nếu user hủy vé hoặc thanh toán thành công, reload lại danh sách
					if (result == DialogResult.OK)
					{
						LoadLichSuMuaVe();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi xem chi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnLamMoi_Click(object sender, EventArgs e)
		{
			LoadLichSuMuaVe();
		}
	}
}
