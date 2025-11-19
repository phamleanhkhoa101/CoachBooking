using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	public partial class UCThanhToan : UserControl
	{
		private List<dynamic> _allData;
		private List<dynamic> _filteredData;

		public UCThanhToan()
		{
			InitializeComponent();
			this.Load += UCThanhToan_Load;
		}

		private void UCThanhToan_Load(object sender, EventArgs e)
		{
			try
			{
				LoadData();
				InitializeFilters();
				UpdateStatistics();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LoadData()
		{
			using (var db = new BXMDEntities())
			{
				_allData = (from tt in db.ThanhToans
							join hd in db.HoaDons on tt.MaHD equals hd.MaHD
							join kh in db.KhachHangs on hd.MaKH equals kh.MaKH
							select new
							{
								MaTT = tt.MaTT,
								MaHD = tt.MaHD,
								TenKH = kh.TenKH,
								SDT = kh.SDT,
								TongTien = tt.TongTien,
								NgayTT = tt.NgayTT,
								PhuongThuc = tt.PhuongThuc,
								SoVe = hd.VeXes.Count
							})
							.OrderByDescending(x => x.NgayTT)
							.ToList<dynamic>();

				_filteredData = _allData;
				LoadDataToGrid(_filteredData);
			}
		}

		private void LoadDataToGrid(List<dynamic> data)
		{
			dgvThanhToan.Rows.Clear();

			foreach (var item in data)
			{
				int rowIndex = dgvThanhToan.Rows.Add(
					item.MaTT,
					item.MaHD,
					item.TenKH,
					item.SDT,
					((decimal)item.TongTien).ToString("N0"),
					((DateTime)item.NgayTT).ToString("dd/MM/yyyy HH:mm"),
					item.PhuongThuc,
					item.SoVe
				);
			}

			dgvThanhToan.ClearSelection();
			lblTongSo.Text = $"Tổng số: {data.Count} thanh toán";
		}

		private void InitializeFilters()
		{
			// Phương thức thanh toán
			cbbPhuongThuc.Items.Clear();
			cbbPhuongThuc.Items.Add("Tất cả");
			cbbPhuongThuc.Items.Add("Tiền mặt");
			cbbPhuongThuc.Items.Add("MoMo");
			cbbPhuongThuc.Items.Add("Chuyển khoản");
			cbbPhuongThuc.SelectedIndex = 0;

			// Khoảng thời gian
			dtpTuNgay.Value = DateTime.Now.AddMonths(-1);
			dtpDenNgay.Value = DateTime.Now;

			// Loại tìm kiếm
			cbbLoaiTimKiem.Items.Clear();
			cbbLoaiTimKiem.Items.Add("Tất cả");
			cbbLoaiTimKiem.Items.Add("Mã thanh toán");
			cbbLoaiTimKiem.Items.Add("Mã hóa đơn");
			cbbLoaiTimKiem.Items.Add("Tên khách hàng");
			cbbLoaiTimKiem.Items.Add("Số điện thoại");
			cbbLoaiTimKiem.SelectedIndex = 0;
		}

		private void UpdateStatistics()
		{
			if (_filteredData == null || !_filteredData.Any())
			{
				lblTongDoanhThu.Text = "0 ₫";
				lblTongGiaoDich.Text = "0";
				lblTrungBinh.Text = "0 ₫";
				return;
			}

			decimal tongDoanhThu = _filteredData.Sum(x => (decimal)x.TongTien);
			int tongGiaoDich = _filteredData.Count;
			decimal trungBinh = tongGiaoDich > 0 ? tongDoanhThu / tongGiaoDich : 0;

			lblTongDoanhThu.Text = $"{tongDoanhThu:N0} ₫";
			lblTongGiaoDich.Text = tongGiaoDich.ToString();
			lblTrungBinh.Text = $"{trungBinh:N0} ₫";
		}

		private void btnTimKiem_Click(object sender, EventArgs e)
		{
			ApplyFilters();
		}

		private void ApplyFilters()
		{
			if (_allData == null) return;

			_filteredData = _allData;

			// Lọc theo thời gian
			DateTime tuNgay = dtpTuNgay.Value.Date;
			DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

			_filteredData = _filteredData
				.Where(x => ((DateTime)x.NgayTT) >= tuNgay && ((DateTime)x.NgayTT) <= denNgay)
				.ToList<dynamic>();

			// Lọc theo phương thức
			if (cbbPhuongThuc.SelectedIndex > 0)
			{
				string phuongThuc = cbbPhuongThuc.SelectedItem.ToString();
				_filteredData = _filteredData
					.Where(x => x.PhuongThuc == phuongThuc)
					.ToList<dynamic>();
			}

			// Lọc theo từ khóa tìm kiếm
			string keyword = txtTimKiem.Text.Trim().ToLower();
			if (!string.IsNullOrEmpty(keyword))
			{
				int loaiTimKiem = cbbLoaiTimKiem.SelectedIndex;
				switch (loaiTimKiem)
				{
					case 1: // Mã thanh toán
						_filteredData = _filteredData.Where(x => x.MaTT.ToString().Contains(keyword)).ToList<dynamic>();
						break;
					case 2: // Mã hóa đơn
						_filteredData = _filteredData.Where(x => x.MaHD.ToString().Contains(keyword)).ToList<dynamic>();
						break;
					case 3: // Tên khách hàng
						_filteredData = _filteredData.Where(x => ((string)x.TenKH ?? "").ToLower().Contains(keyword)).ToList<dynamic>();
						break;
					case 4: // Số điện thoại
						_filteredData = _filteredData.Where(x => ((string)x.SDT ?? "").Contains(keyword)).ToList<dynamic>();
						break;
					default: // Tất cả
						_filteredData = _filteredData.Where(x =>
							x.MaTT.ToString().Contains(keyword) ||
							x.MaHD.ToString().Contains(keyword) ||
							((string)x.TenKH ?? "").ToLower().Contains(keyword) ||
							((string)x.SDT ?? "").Contains(keyword)
						).ToList<dynamic>();
						break;
				}
			}

			LoadDataToGrid(_filteredData);
			UpdateStatistics();
		}

		private void btnLamMoi_Click(object sender, EventArgs e)
		{
			txtTimKiem.Clear();
			cbbPhuongThuc.SelectedIndex = 0;
			cbbLoaiTimKiem.SelectedIndex = 0;
			dtpTuNgay.Value = DateTime.Now.AddMonths(-1);
			dtpDenNgay.Value = DateTime.Now;
			LoadData();
			UpdateStatistics();
		}

		private void btnXemChiTiet_Click(object sender, EventArgs e)
		{
			if (dgvThanhToan.SelectedRows.Count == 0)
			{
				MessageBox.Show("Vui lòng chọn một thanh toán để xem chi tiết.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			int maHD = int.Parse(dgvThanhToan.SelectedRows[0].Cells["MaHD"].Value.ToString());
			
			try
			{
				using (var db = new BXMDEntities())
				{
					var hoaDon = db.HoaDons.FirstOrDefault(h => h.MaHD == maHD);
					if (hoaDon != null)
					{
						using (var dialog = new HoaDonDialog())
						{
							dialog.BindKhachHang(GetKhachHangItems());
							dialog.BindNhanVien(GetNhanVienItems());
							dialog.SetMode(HoaDonDialogMode.View);
							dialog.MaHD = hoaDon.MaHD.ToString();
							dialog.SelectedMaKH = hoaDon.MaKH;
							dialog.SelectedMANV = hoaDon.MANV;
							dialog.NgayLapValue = hoaDon.NgayLap;
							dialog.TongTienValue = hoaDon.TongTien ?? 0;
							dialog.TrangThaiValue = hoaDon.TrangThai;
							dialog.ShowDialog();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi khi xem chi tiết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private IEnumerable<KeyValuePair<int, string>> GetKhachHangItems()
		{
			using (var db = new BXMDEntities())
			{
				var data = db.KhachHangs.AsNoTracking()
								 .Select(kh => new { kh.MaKH, kh.TenKH })
								 .ToList();
				return data.Select(kh => new KeyValuePair<int, string>(kh.MaKH, kh.TenKH))
						   .ToList();
			}
		}

		private IEnumerable<KeyValuePair<int, string>> GetNhanVienItems()
		{
			using (var db = new BXMDEntities())
			{
				var data = db.NhanViens.AsNoTracking()
								 .Select(nv => new { nv.MaNV, nv.TenNV })
								 .ToList();
				return data.Select(nv => new KeyValuePair<int, string>(nv.MaNV, nv.TenNV))
						   .ToList();
			}
		}

		private void btnXuatBaoCao_Click(object sender, EventArgs e)
		{
			if (_filteredData == null || !_filteredData.Any())
			{
				MessageBox.Show("Không có dữ liệu để xuất báo cáo.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			try
			{
				SaveFileDialog saveDialog = new SaveFileDialog
				{
					Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
					FileName = $"BaoCaoThanhToan_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
					Title = "Xuất báo cáo thanh toán"
				};

				if (saveDialog.ShowDialog() == DialogResult.OK)
				{
					using (var writer = new System.IO.StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8))
					{
						// Header
						writer.WriteLine("Mã TT,Mã HĐ,Tên khách hàng,Số điện thoại,Tổng tiền,Ngày thanh toán,Phương thức,Số vé");

						// Data
						foreach (var item in _filteredData)
						{
							writer.WriteLine($"{item.MaTT},{item.MaHD},{item.TenKH},{item.SDT},{item.TongTien},{item.NgayTT:dd/MM/yyyy HH:mm},{item.PhuongThuc},{item.SoVe}");
						}
					}

					MessageBox.Show($"Xuất báo cáo thành công!\nĐường dẫn: {saveDialog.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi khi xuất báo cáo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void dgvThanhToan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				btnXemChiTiet_Click(sender, e);
			}
		}

		private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				btnTimKiem_Click(sender, e);
				e.SuppressKeyPress = true;
			}
		}
	}
}
