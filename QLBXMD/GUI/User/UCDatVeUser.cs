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
using Newtonsoft.Json;

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
		private bool isLoadingData = false;
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
			isLoadingData = true;

			LoadDiemDiDiemDen();
			dtpNgayDi.Value = DateTime.Now;
			cboGioDi.SelectedIndex = 0;

			cboDiemDi.SelectedIndexChanged += CboDiemDi_SelectedIndexChanged;
			cboDiemDen.SelectedIndexChanged += CboDiemDen_SelectedIndexChanged;
			dtpNgayDi.ValueChanged += DtpNgayDi_ValueChanged;
			cboGioDi.SelectedIndexChanged += CboGioDi_SelectedIndexChanged;

			isLoadingData = false;

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

					var diemDiList = new List<string>();
					diemDiList.Add("-- Chọn điểm đi --");
					diemDiList.AddRange(diemDi);

					var diemDenList = new List<string>();
					diemDenList.Add("-- Chọn điểm đến --");
					diemDenList.AddRange(diemDen);

					cboDiemDi.DataSource = diemDiList;
					cboDiemDen.DataSource = diemDenList;

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
			string diemDi = cboDiemDi.SelectedItem?.ToString();
			string diemDen = cboDiemDen.SelectedItem?.ToString();

			if (diemDi != null && diemDi.StartsWith("--")) diemDi = null;
			if (diemDen != null && diemDen.StartsWith("--")) diemDen = null;

			if (string.IsNullOrEmpty(diemDi) && string.IsNullOrEmpty(diemDen))
			{
				LoadChuyenXe(null, null);
				return;
			}

			LoadChuyenXe(diemDi, diemDen);
		}

		private void btnTimChuyen_Click(object sender, EventArgs e)
		{
			LoadChuyenXeAuto();
		}

		private void LoadChuyenXe(string diemDi, string diemDen)
		{
			try
			{
				selectedNgayDi = dtpNgayDi.Value.Date;

				using (var db = new BXMDEntities())
				{
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

					System.Diagnostics.Debug.WriteLine($"Tìm thấy {query.Count} chuyến xe ngày {selectedNgayDi:dd/MM/yyyy}");

					if (!string.IsNullOrEmpty(diemDi))
					{
						query = query.Where(r => r.DiemDi == diemDi).ToList();
						System.Diagnostics.Debug.WriteLine($"Sau khi lọc điểm đi '{diemDi}': {query.Count} chuyến");
					}

					if (!string.IsNullOrEmpty(diemDen))
					{
						query = query.Where(r => r.DiemDen == diemDen).ToList();
						System.Diagnostics.Debug.WriteLine($"Sau khi lọc điểm đến '{diemDen}': {query.Count} chuyến");
					}

					if (cboGioDi.SelectedIndex > 0)
					{
						int gioIndex = cboGioDi.SelectedIndex;
						query = query.Where(r =>
						{
							int gio = r.KhoiHanh.Hour;
							switch (gioIndex)
							{
								case 1: return gio >= 0 && gio < 6;
								case 2: return gio >= 6 && gio < 12;
								case 3: return gio >= 12 && gio < 18;
								case 4: return gio >= 18 && gio < 24;
								default: return true;
							}
						}).ToList();
						System.Diagnostics.Debug.WriteLine($"Sau khi lọc giờ: {query.Count} chuyến");
					}

					if (!query.Any())
					{
						System.Diagnostics.Debug.WriteLine("Không có chuyến xe phù hợp");
						pnlResults.Visible = false;
						return;
					}

					if (!string.IsNullOrEmpty(diemDi)) selectedDiemDi = diemDi;
					if (!string.IsNullOrEmpty(diemDen)) selectedDiemDen = diemDen;

					pnlResults.Visible = true;

					pnlResults.PerformLayout();
					flpChuyenXe.PerformLayout();
					Application.DoEvents();

					flpChuyenXe.Controls.Clear();
					flpChuyenXe.SuspendLayout();

					foreach (var item in query)
					{
						var card = CreateChuyenXeCard(
							item.MaChuyen,
							item.MaXe,
							item.MaLoaiXe,
							item.KhoiHanh,
							item.DenDuKien ?? item.KhoiHanh.AddHours(6),
							item.TenNX,
							item.TenLX,
							item.SoCho,
							item.SoChoTrong,
							item.GiaVe,
							item.DiemDi,
							item.DiemDen);
						flpChuyenXe.Controls.Add(card);
					}

					flpChuyenXe.ResumeLayout();
					System.Diagnostics.Debug.WriteLine($"Đã hiển thị {query.Count} chuyến xe");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi tải danh sách chuyến xe: " + ex.Message + "\n\nStack trace: " + ex.StackTrace, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				System.Diagnostics.Debug.WriteLine($"Lỗi: {ex.Message}\n{ex.StackTrace}");
			}
		}

		private Panel CreateChuyenXeCard(
			int maChuyen,
			int maXe,
			int maLoaiXe,
			DateTime khoiHanh,
			DateTime denDuKien,
			string tenNX,
			string tenLX,
			int soCho,
			int soChoTrong,
			decimal giaVe,
			string diemDi,
			string diemDen)
		{
			int cardWidth = flpChuyenXe.Width - 40;
			if (cardWidth < 800) cardWidth = 800;

			Panel card = new Panel
			{
				Width = cardWidth,
				Height = 100,
				BorderStyle = BorderStyle.FixedSingle,
				BackColor = Color.White,
				Margin = new Padding(5, 5, 5, 10)
			};

			Label lblBusIcon = new Label
			{
				Text = "XE",
				Location = new Point(10, 10),
				Size = new Size(40, 40),
				Font = new Font("Segoe UI", 12F, FontStyle.Bold),
				TextAlign = ContentAlignment.MiddleCenter,
				BackColor = Color.FromArgb(240, 240, 240)
			};

			Label lblTenNX = new Label
			{
				Text = tenNX,
				Location = new Point(60, 10),
				AutoSize = false,
				Width = 250,
				Height = 22,
				Font = new Font("Segoe UI", 10F, FontStyle.Bold),
				ForeColor = Color.FromArgb(51, 51, 51)
			};

			Label lblLoaiXe = new Label
			{
				Text = $"{tenLX} - {soCho} chỗ",
				Location = new Point(60, 32),
				AutoSize = false,
				Width = 200,
				Height = 16,
				Font = new Font("Segoe UI", 8F),
				ForeColor = Color.Gray
			};

			Label lblTuyen = new Label
			{
				Text = $"{diemDi} → {diemDen}",
				Location = new Point(60, 50),
				AutoSize = false,
				Width = 350,
				Height = 18,
				Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
				ForeColor = Color.FromArgb(0, 150, 136)
			};

			Label lblGioDi = new Label
			{
				Text = khoiHanh.ToString("HH:mm"),
				Location = new Point(60, 70),
				AutoSize = false,
				Width = 55,
				Height = 22,
				Font = new Font("Segoe UI", 12F, FontStyle.Bold),
				ForeColor = Color.FromArgb(0, 150, 136)
			};

			Label lblMuiTen = new Label
			{
				Text = "→",
				Location = new Point(120, 70),
				AutoSize = false,
				Width = 20,
				Height = 22,
				Font = new Font("Segoe UI", 11F),
				ForeColor = Color.Gray,
				TextAlign = ContentAlignment.MiddleCenter
			};

			Label lblGioDen = new Label
			{
				Text = denDuKien.ToString("HH:mm"),
				Location = new Point(145, 70),
				AutoSize = false,
				Width = 55,
				Height = 22,
				Font = new Font("Segoe UI", 12F, FontStyle.Bold),
				ForeColor = Color.FromArgb(0, 150, 136)
			};

			Label lblNgayDi = new Label
			{
				Text = khoiHanh.ToString("dd/MM/yyyy"),
				Location = new Point(210, 73),
				AutoSize = false,
				Width = 85,
				Height = 16,
				Font = new Font("Segoe UI", 8F),
				ForeColor = Color.Gray
			};

			Label lblChoTrong = new Label
			{
				Text = soChoTrong > 0 ? $"Còn {soChoTrong} chỗ" : "Hết chỗ",
				Location = new Point(310, 73),
				AutoSize = false,
				Width = 100,
				Height = 18,
				Font = new Font("Segoe UI", 8F),
				ForeColor = soChoTrong > 0 ? Color.Green : Color.Red
			};

			Label lblGiaVe = new Label
			{
				Text = $"{giaVe:#,##0} VNĐ",
				Location = new Point(cardWidth - 260, 25),
				AutoSize = false,
				Width = 140,
				Height = 28,
				Font = new Font("Segoe UI", 12F, FontStyle.Bold),
				ForeColor = Color.FromArgb(244, 67, 54),
				TextAlign = ContentAlignment.MiddleRight
			};

			Button btnChon = new Button
			{
				Text = "Chọn chuyến",
				Location = new Point(cardWidth - 145, 60),
				Size = new Size(125, 32),
				BackColor = Color.FromArgb(0, 150, 136),
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
				Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
				Cursor = Cursors.Hand,
				Enabled = soChoTrong > 0,
				TabStop = true,
				UseVisualStyleBackColor = false
			};
			btnChon.FlatAppearance.BorderSize = 0;

			int capturedMaChuyen = maChuyen;
			int capturedMaXe = maXe;
			int capturedMaLoaiXe = maLoaiXe;
			decimal capturedGiaVe = giaVe;
			string capturedDiemDi = diemDi;
			string capturedDiemDen = diemDen;

			btnChon.Click += (s, e) =>
			{
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

			card.MouseEnter += (s, e) =>
			{
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

					bookedSeats = db.VeXes
						.Where(v => v.MaChuyen == selectedMaChuyen)
						.Select(v => v.ChoNgoi)
						.ToList();

					selectedSeats.Clear();

					flpTangDuoi.Controls.Clear();
					flpTangTren.Controls.Clear();

					if (isGiuongNam)
					{
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
					btnSeat.BackColor = Color.FromArgb(220, 220, 220);
					btnSeat.ForeColor = Color.Gray;
					btnSeat.Enabled = false;
					btnSeat.Cursor = Cursors.No;
				}
				else
				{
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
				selectedSeats.Remove(seatNumber);
				btn.BackColor = Color.FromArgb(0, 199, 174);
				btn.ForeColor = Color.White;
			}
			else
			{
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

			if (currentUser == null)
			{
				MessageBox.Show("Vui lòng đăng nhập để đặt vé.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			try
			{
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

					var khachHang = db.KhachHangs.FirstOrDefault(kh => kh.MaTK == currentUser.MaTK);
					if (khachHang == null)
					{
						MessageBox.Show("Không tìm thấy thông tin khách hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

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

					var hoaDon = new HoaDon
					{
						MaKH = khachHang.MaKH,
						MaNV = null,
						NgayLap = DateTime.Now,
						TongTien = 0,
						TrangThai = false
					};
					db.HoaDons.Add(hoaDon);
					db.SaveChanges();

					maHD = hoaDon.MaHD;
					Console.WriteLine($"Tạo hóa đơn: MaHD = {maHD}, TongTien ban đầu = {hoaDon.TongTien}");

					foreach (int soGhe in selectedSeats)
					{
						var ve = new VeXe
						{
							MaChuyen = selectedMaChuyen.Value,
							ChoNgoi = soGhe,
							MaHD = maHD,
							GiaVe = selectedGiaVe.Value
						};
						db.VeXes.Add(ve);
					}
					db.SaveChanges();
					Console.WriteLine($"Đã tạo {selectedSeats.Count} vé xe");

					db.Entry(hoaDon).Reload();
					Console.WriteLine($"Sau khi refresh: TongTien = {hoaDon.TongTien}");

					ProcessMoMoPayment(hoaDon.TongTien ?? tienTam, khachHang.MaKH, maHD);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi đặt vé: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Console.WriteLine($"Error: {ex}");
			}
		}

		/// <summary>
		/// Xử lý thanh toán MoMo
		/// </summary>
		private async void ProcessMoMoPayment(decimal amount, int maKH, int maHD)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;

				string orderId = $"HD{maHD}_{DateTime.Now:yyyyMMddHHmmss}";
				string orderInfo = $"Thanh toan ve xe - Ma HD: {maHD}";

				var momoService = new Services.MoMoPaymentService();
				var response = await momoService.CreatePaymentRequest(
					orderId,
					(long)amount,
					orderInfo
				);

				this.Cursor = Cursors.Default;

				if (response.ResultCode == 0)
				{
					MessageBox.Show(
						"Đang chuyển đến trang thanh toán MoMo...\n\n" +
						"Vui lòng hoàn tất thanh toán trong trình duyệt.\n" +
						$"Mã đơn hàng: {orderId}",
						"Thanh toán MoMo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);

					System.Diagnostics.Process.Start(response.PayUrl);

					StartPollingPaymentStatus(orderId, maHD, amount);
				}
				else
				{
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
				Console.WriteLine($"MoMo Payment Error: {ex}");
			}
		}

		/// <summary>
		/// Polling MoMo để kiểm tra trạng thái thanh toán
		/// </summary>
		private async void StartPollingPaymentStatus(string orderId, int maHD, decimal amount)
		{
			int attempt = 0;
			const int maxAttempts = 24;
			const int delayMs = 5000;

			while (attempt < maxAttempts)
			{
				attempt++;
				await Task.Delay(delayMs);

				string status = await QueryMomoOrder(orderId);

				if (status == "Success")
				{
					MessageBox.Show(
						$"Thanh toán MoMo thành công!\n\n" +
						$"Mã hóa đơn: {maHD}\n" +
						$"Số tiền: {amount:#,##0} VNĐ",
						"Thành công",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
					);

					UpdateHoaDonTrangThai(maHD, true, "MoMo");
					ResetFormAfterPayment();
					return;
				}
				else if (status == "Failed")
				{
					MessageBox.Show(
						"Thanh toán MoMo thất bại hoặc bị hủy!\n\n" +
						"Hóa đơn và vé đã được hủy.",
						"Thất bại",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error
					);

					RollbackHoaDon(maHD);
					return;
				}

				Console.WriteLine($"[MoMo Polling] Attempt {attempt}/{maxAttempts} - Status: {status}");
			}

			MessageBox.Show(
				"Hết thời gian chờ thanh toán MoMo.\n\n" +
				"Vui lòng kiểm tra lại trạng thái đơn hàng trong Lịch sử mua vé.",
				"Timeout",
				MessageBoxButtons.OK,
				MessageBoxIcon.Warning
			);
		}

		/// <summary>
		/// Gọi API Query của MoMo để xác minh đơn hàng
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

					// 0 = Success
					// 1000 = Transaction initiated
					// 1006 = Transaction not found
					// 1007 = Transaction pending
					// 9000 = Transaction failed/cancelled
					if (resultCode == 0)
					{
						return "Success";
					}
					else if (resultCode == 1000 || resultCode == 1006 || resultCode == 1007)
					{
						return "Pending";
					}
					else if (resultCode == 9000 || resultCode == 1001)
					{
						return "Failed";
					}
					else
					{
						Console.WriteLine($"Unknown MoMo code: {resultCode}");
						return "Pending";
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("QueryMoMoOrder Error: " + ex.Message);
				return "Pending";
			}
		}

		/// <summary>
		/// Cập nhật trạng thái hóa đơn khi có kết quả thanh toán
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
						Console.WriteLine($"Không tìm thấy hóa đơn #{maHD}");
						return;
					}

					hoaDon.TrangThai = daThanhToan;

					if (daThanhToan)
					{
						var thanhToan = new ThanhToan
						{
							MaHD = maHD,
							TongTien = hoaDon.TongTien ?? 0,
							NgayTT = DateTime.Now,
							PhuongThuc = phuongThuc
						};
						db.ThanhToans.Add(thanhToan);

						Console.WriteLine($"Hóa đơn #{maHD} đã thanh toán qua {phuongThuc}");
					}

					db.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi cập nhật hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Console.WriteLine($"UpdateHoaDonTrangThai Error: {ex.Message}");
			}
		}

		/// <summary>
		/// Rollback hóa đơn khi thanh toán thất bại
		/// </summary>
		private void RollbackHoaDon(int maHD)
		{
			try
			{
				using (var db = new BXMDEntities())
				{
					var veXes = db.VeXes.Where(v => v.MaHD == maHD).ToList();
					db.VeXes.RemoveRange(veXes);

					var hoaDon = db.HoaDons.FirstOrDefault(h => h.MaHD == maHD);
					if (hoaDon != null)
					{
						db.HoaDons.Remove(hoaDon);
					}

					db.SaveChanges();
					Console.WriteLine($"Đã rollback hóa đơn #{maHD}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"RollbackHoaDon Error: {ex.Message}");
			}
		}

		/// <summary>
		/// Reset form sau khi thanh toán thành công
		/// </summary>
		private void ResetFormAfterPayment()
		{
			try
			{
				ResetForm();

				LoadChuyenXeAuto();

				MessageBox.Show(
					"Đặt vé thành công!\n\n" +
					"Bạn có thể xem chi tiết trong mục 'Lịch sử mua vé'.",
					"Thành công",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"ResetFormAfterPayment Error: {ex.Message}");
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
				var temp = cboDiemDi.SelectedItem;
				cboDiemDi.SelectedItem = cboDiemDen.SelectedItem;
				cboDiemDen.SelectedItem = temp;
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
