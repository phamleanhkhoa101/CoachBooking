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
				MessageBox.Show("Vui lòng ??ng nh?p ?? xem l?ch s?.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			try
			{
				using (var db = new BXMDEntities())
				{
					// L?y thông tin khách hàng
					var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.MaTK == currentUser.MaTK);
					if (khachHang == null)
					{
						lblThongBao.Visible = true;
						lblThongBao.Text = "Không tìm th?y thông tin khách hàng.";
						return;
					}

					// L?y danh sách hóa ??n
					var hoaDons = (from hd in db.HoaDons
								   where hd.MaKH == khachHang.MaKH
								   orderby hd.NgayLap descending
								   select hd).ToList();

					if (!hoaDons.Any())
					{
						lblThongBao.Visible = true;
						lblThongBao.Text = "B?n ch?a có l?ch s? mua vé nào.";
						return;
					}

					lblThongBao.Visible = false;

					// Clear và thêm hóa ??n cards
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
				MessageBox.Show($"L?i t?i l?ch s?: {ex.Message}", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

			// L?y thông tin chi ti?t
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
						Text = "Không tìm th?y thông tin chuy?n xe",
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

				// Mã hóa ??n
				Label lblMaHD = new Label
				{
					Text = $"Mã H?: #{hoaDon.MaHD}",
					Location = new Point(20, 15),
					AutoSize = true,
					Font = new Font("Segoe UI", 11F, FontStyle.Bold),
					ForeColor = Color.FromArgb(0, 150, 136)
				};

				// Ngày l?p
				Label lblNgayLap = new Label
				{
					Text = $"Ngày ??t: {hoaDon.NgayLap:dd/MM/yyyy HH:mm}",
					Location = new Point(20, 45),
					AutoSize = true,
					Font = new Font("Segoe UI", 9F),
					ForeColor = Color.Gray
				};

				// Tr?ng thái
				Label lblTrangThai = new Label
				{
					Text = hoaDon.TrangThai == true ? "? ?ã thanh toán" : "? Ch?a thanh toán",
					Location = new Point(cardWidth - 200, 15),
					AutoSize = true,
					Font = new Font("Segoe UI", 10F, FontStyle.Bold),
					ForeColor = hoaDon.TrangThai == true ? Color.Green : Color.Orange
				};

				// Tuy?n ???ng
				Label lblTuyen = new Label
				{
					Text = tuyenDuong != null ? $"?? {tuyenDuong.DiemDi} ? {tuyenDuong.DiemDen}" : "Không rõ tuy?n",
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

				// Ngày kh?i hành
				Label lblKhoiHanh = new Label
				{
					Text = $"Kh?i hành: {chuyenXe.NgayGioKhoiHanh:dd/MM/yyyy HH:mm}",
					Location = new Point(20, 130),
					AutoSize = true,
					Font = new Font("Segoe UI", 10F, FontStyle.Bold),
					ForeColor = Color.FromArgb(244, 67, 54)
				};

				// Gh? ?ã ??t
				var gheNumbers = veXes.Select(v => v.ChoNgoi).OrderBy(g => g).ToList();
				Label lblGhe = new Label
				{
					Text = $"Gh?: {string.Join(", ", gheNumbers)}",
					Location = new Point(20, 160),
					AutoSize = true,
					Font = new Font("Segoe UI", 9F),
					ForeColor = Color.Gray
				};

				// T?ng ti?n
				Label lblTongTien = new Label
				{
					Text = $"{hoaDon.TongTien:#,##0} VN?",
					Location = new Point(cardWidth - 200, 150),
					AutoSize = true,
					Font = new Font("Segoe UI", 16F, FontStyle.Bold),
					ForeColor = Color.FromArgb(244, 67, 54)
				};

				// Button xem chi ti?t
				Button btnChiTiet = new Button
				{
					Text = "Xem chi ti?t",
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
				using (var db = new BXMDEntities())
				{
					var hoaDon = db.HoaDons.FirstOrDefault(hd => hd.MaHD == maHD);
					if (hoaDon == null)
					{
						MessageBox.Show("Không tìm th?y hóa ??n.", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					var veXes = db.VeXes.Where(v => v.MaHD == maHD).ToList();
					if (!veXes.Any())
					{
						MessageBox.Show("Không có vé nào trong hóa ??n này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
						return;
					}

					var veXeFirst = veXes.First();
					var chuyenXe = db.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == veXeFirst.MaChuyen);
					var tuyenDuong = chuyenXe != null ? db.TuyenDuongs.FirstOrDefault(td => td.MaTuyen == chuyenXe.MaTuyen) : null;
					var xe = chuyenXe != null ? db.Xes.FirstOrDefault(x => x.MaXe == chuyenXe.MaXe) : null;
					var nhaXe = xe != null ? db.NhaXes.FirstOrDefault(nx => nx.MaNX == xe.MaNX) : null;
					var loaiXe = xe != null ? db.LoaiXes.FirstOrDefault(lx => lx.MaLX == xe.MaLX) : null;

					// T?o thông tin chi ti?t
					string chiTiet = "????????????????????????????????????????????????????????????\n";
					chiTiet += $"                    HÓA ??N #{hoaDon.MaHD}\n";
					chiTiet += "????????????????????????????????????????????????????????????\n\n";
					
					chiTiet += $"?? Ngày ??t: {hoaDon.NgayLap:dd/MM/yyyy HH:mm}\n";
					chiTiet += $"?? Tr?ng thái: {(hoaDon.TrangThai == true ? "?ã thanh toán" : "Ch?a thanh toán")}\n\n";
					
					chiTiet += "??????????????????????????????????????????????????????????\n";
					chiTiet += "  THÔNG TIN CHUY?N XE\n";
					chiTiet += "??????????????????????????????????????????????????????????\n\n";
					
					if (tuyenDuong != null)
					{
						chiTiet += $"?? Tuy?n: {tuyenDuong.DiemDi} ? {tuyenDuong.DiemDen}\n";
					}
					
					if (nhaXe != null)
					{
						chiTiet += $"?? Nhà xe: {nhaXe.TenNX}\n";
					}
					
					if (loaiXe != null)
					{
						chiTiet += $"?? Lo?i xe: {loaiXe.TenLX}\n";
					}
					
					if (chuyenXe != null)
					{
						chiTiet += $"? Kh?i hành: {chuyenXe.NgayGioKhoiHanh:dd/MM/yyyy HH:mm}\n";
						if (chuyenXe.NgayGioDenDuKien.HasValue)
						{
							chiTiet += $"?? ??n d? ki?n: {chuyenXe.NgayGioDenDuKien:dd/MM/yyyy HH:mm}\n";
						}
					}
					
					chiTiet += "\n??????????????????????????????????????????????????????????\n";
					chiTiet += "  THÔNG TIN VÉ\n";
					chiTiet += "??????????????????????????????????????????????????????????\n\n";
					
					chiTiet += $"?? S? l??ng vé: {veXes.Count}\n";
					chiTiet += $"?? Gh?: {string.Join(", ", veXes.Select(v => v.ChoNgoi).OrderBy(g => g))}\n";
					chiTiet += $"?? Giá m?i vé: {veXes.First().GiaVe:#,##0} VN?\n\n";
					
					chiTiet += "??????????????????????????????????????????????????????????\n";
					chiTiet += $"?? T?NG TI?N: {hoaDon.TongTien:#,##0} VN?\n";
					chiTiet += "??????????????????????????????????????????????????????????\n";

					MessageBox.Show(chiTiet, "Chi ti?t hóa ??n", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"L?i xem chi ti?t: {ex.Message}", "L?i", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnLamMoi_Click(object sender, EventArgs e)
		{
			LoadLichSuMuaVe();
		}
	}
}
