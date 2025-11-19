using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.BUS;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	public partial class UCGiaVe : UserControl
	{
		private readonly GiaVeBUS _bus = new GiaVeBUS();
		private List<dynamic> _dataDaCo;
		private List<dynamic> _dataChuaCo;
		private List<dynamic> _allDataChuaCo; // Lưu tất cả dữ liệu chưa lọc theo nhà xe
		private int? _selectedMaNX = null; // Nhà xe được chọn

		public UCGiaVe()
		{
			InitializeComponent();
			this.Load += UCGiaVe_Load;
			this.dgvDaCo.SelectionChanged += DgvDaCo_SelectionChanged;
			this.dgvChuaCo.SelectionChanged += DgvChuaCo_SelectionChanged;
		}

		private void DgvDaCo_SelectionChanged(object sender, EventArgs e)
		{
			bool has = dgvDaCo.SelectedRows.Count > 0 && dgvDaCo.SelectedRows[0].Cells[0].Value != null;
			btnXem.Enabled = has; 
			btnSua.Enabled = has; 
			btnXoa.Enabled = has;
		}

		private void DgvChuaCo_SelectionChanged(object sender, EventArgs e)
		{
			bool has = dgvChuaCo.SelectedRows.Count > 0;
			btnThemChuaCo.Enabled = has;
		}

		private void UCGiaVe_Load(object sender, EventArgs e)
		{
			try
			{
				SetupGridHeaders();

				// Sử dụng GetAllCombinations để lấy tất cả tổ hợp
				var allCombos = _bus.GetAllCombinations() ?? new List<dynamic>();
				
				// Tách dữ liệu thành 2 danh sách
				_dataDaCo = allCombos
					.Where(combo => combo.HasPrice)
					.Select(combo => new
					{
						MaGV = combo.MaGV,
						NhaXeName = combo.TenNX ?? "",
						LoaiXeName = combo.TenLX ?? "",
						TuyenName = $"{combo.DiemDi} -> {combo.DiemDen}",
						GiaVe = combo.GiaVe != null ? ((decimal)combo.GiaVe).ToString("N0") : "0",
						MaNX = combo.MaNX,
						MaLX = combo.MaLX,
						MaTuyen = combo.MaTuyen,
						GiaVeValue = combo.GiaVe,
						HasPrice = combo.HasPrice
					})
					.ToList<dynamic>();

				// Lưu tất cả dữ liệu chưa có giá vé
				_allDataChuaCo = allCombos
					.Where(combo => !combo.HasPrice)
					.Select(combo => new
					{
						MaGV = combo.MaGV,
						NhaXeName = combo.TenNX ?? "",
						LoaiXeName = combo.TenLX ?? "",
						TuyenName = $"{combo.DiemDi} -> {combo.DiemDen}",
						GiaVe = "Chưa có giá",
						MaNX = combo.MaNX,
						MaLX = combo.MaLX,
						MaTuyen = combo.MaTuyen,
						DiemDi = combo.DiemDi,
						DiemDen = combo.DiemDen,
						GiaVeValue = combo.GiaVe,
						HasPrice = combo.HasPrice
					})
					.ToList<dynamic>();

				LoadDataDaCo(_dataDaCo);
				InitCbb();
				InitNhaXeComboBox();
				btnXem.Enabled = btnSua.Enabled = btnXoa.Enabled = false;
				btnThemChuaCo.Enabled = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Tải dữ liệu thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void SetupGridHeaders()
		{
			// Setup cho dgvDaCo
			dgvDaCo.Columns.Clear();
			dgvDaCo.Columns.Add("MaGV", "Mã giá vé");
			dgvDaCo.Columns.Add("NhaXe", "Nhà xe");
			dgvDaCo.Columns.Add("LoaiXe", "Loại xe");
			dgvDaCo.Columns.Add("Tuyen", "Tuyến đường");
			dgvDaCo.Columns.Add("GiaVe", "Giá vé (₫)");

			// Setup cho dgvChuaCo - hiển thị Nhà xe, Loại xe và Tuyến đường
			dgvChuaCo.Columns.Clear();
			dgvChuaCo.Columns.Add("NhaXe", "Nhà xe");
			dgvChuaCo.Columns.Add("LoaiXe", "Loại xe");
			dgvChuaCo.Columns.Add("Tuyen", "Tuyến đường");
		}

		private void InitNhaXeComboBox()
		{
			cbbNhaXeChuaCo.Items.Clear();
			
			// ✅ Thêm tùy chọn "Tất cả" (thay vì "-- Chọn nhà xe --")
			cbbNhaXeChuaCo.Items.Add(new { MaNX = (int?)null, TenNX = "Tất cả" });

			// Lấy danh sách nhà xe
			using (var db = new BXMDEntities())
			{
				var nhaXes = db.NhaXes.AsNoTracking()
					.OrderBy(nx => nx.TenNX)
					.Select(nx => new { MaNX = (int?)nx.MaNX, TenNX = nx.TenNX })
					.ToList();

				foreach (var nx in nhaXes)
				{
					cbbNhaXeChuaCo.Items.Add(nx);
				}
			}

			cbbNhaXeChuaCo.DisplayMember = "TenNX";
			cbbNhaXeChuaCo.ValueMember = "MaNX";
			cbbNhaXeChuaCo.SelectedIndex = 0;
			cbbNhaXeChuaCo.SelectedIndexChanged += CbbNhaXeChuaCo_SelectedIndexChanged;
		}

		private void CbbNhaXeChuaCo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbbNhaXeChuaCo.SelectedItem == null) return;

			dynamic selectedItem = cbbNhaXeChuaCo.SelectedItem;
			_selectedMaNX = selectedItem.MaNX;

			// ✅ Lọc dữ liệu theo nhà xe được chọn
			if (_selectedMaNX.HasValue)
			{
				// Nếu chọn một nhà xe cụ thể
				_dataChuaCo = _allDataChuaCo
					.Where(x => x.MaNX == _selectedMaNX.Value)
					.ToList<dynamic>();
			}
			else
			{
				// ✅ Nếu chọn "Tất cả" -> Hiển thị tất cả
				_dataChuaCo = _allDataChuaCo.ToList<dynamic>();
			}

			LoadDataChuaCo(_dataChuaCo);
			txtSearchChuaCo.Clear();
		}

		private void InitCbb()
		{
			// ComboBox cho tab Đã có giá vé
			cbbLoai.Items.Clear();
			cbbLoai.Items.Add("Tất cả");
			cbbLoai.Items.Add("Mã giá vé");
			cbbLoai.Items.Add("Nhà xe");
			cbbLoai.Items.Add("Loại xe");
			cbbLoai.Items.Add("Tuyến đường");
			cbbLoai.Items.Add("Giá vé");
			cbbLoai.SelectedIndex = 0;
			cbbLoai.SelectedIndexChanged += cbbLoai_SelectedIndexChanged;

			// ComboBox cho tab Chưa có giá vé - Nhà xe, Loại xe và Tuyến đường
			cbbLoaiChuaCo.Items.Clear();
			cbbLoaiChuaCo.Items.Add("Tất cả");
			cbbLoaiChuaCo.Items.Add("Nhà xe");
			cbbLoaiChuaCo.Items.Add("Loại xe");
			cbbLoaiChuaCo.Items.Add("Tuyến đường");
			cbbLoaiChuaCo.SelectedIndex = 0;
			cbbLoaiChuaCo.SelectedIndexChanged += cbbLoaiChuaCo_SelectedIndexChanged;
		}

		private void LoadDataDaCo(List<dynamic> list)
		{
			dgvDaCo.Rows.Clear();

			foreach (var item in list)
			{
				dgvDaCo.Rows.Add(item.MaGV, item.NhaXeName, item.LoaiXeName, item.TuyenName, item.GiaVe);
			}

			dgvDaCo.ClearSelection();
		}

		private void LoadDataChuaCo(List<dynamic> list)
		{
			dgvChuaCo.Rows.Clear();

			foreach (var item in list)
			{
				// ✅ Hiển thị Nhà xe, Loại xe và Tuyến đường
				int rowIndex = dgvChuaCo.Rows.Add(item.NhaXeName, item.LoaiXeName, item.TuyenName);
				// Đánh dấu màu xám nhạt
				dgvChuaCo.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);
				dgvChuaCo.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.DarkOrange;
			}

			dgvChuaCo.ClearSelection();
		}

		private void txtSearch_TextChanged(object sender, EventArgs e)
		{
			FilterDaCo();
		}

		private void txtSearchChuaCo_TextChanged(object sender, EventArgs e)
		{
			FilterChuaCo();
		}

		private void cbbLoai_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadDataDaCo(_dataDaCo);
			txtSearch.Clear();
		}

		private void cbbLoaiChuaCo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_dataChuaCo != null)
				LoadDataChuaCo(_dataChuaCo);
			txtSearchChuaCo.Clear();
		}

		private IEnumerable<KeyValuePair<int, string>> GetNhaXeItems()
		{
			using (var db = new BXMDEntities())
			{
				var data = db.NhaXes.AsNoTracking()
								 .Select(nx => new { nx.MaNX, nx.TenNX })
								 .ToList();
				return data.Select(nx => new KeyValuePair<int, string>(nx.MaNX, nx.TenNX))
						   .ToList();
			}
		}

		private IEnumerable<KeyValuePair<int, string>> GetLoaiXeItems()
		{
			using (var db = new BXMDEntities())
			{
				var data = db.LoaiXes.AsNoTracking()
					 .Select(lx => new { lx.MaLX, lx.TenLX })
					 .ToList();
				return data.Select(lx => new KeyValuePair<int, string>(lx.MaLX, lx.TenLX))
				   .ToList();
			}
		}

		private IEnumerable<KeyValuePair<int, string>> GetTuyenItems()
		{
			using (var db = new BXMDEntities())
			{
				var data = db.TuyenDuongs.AsNoTracking()
					 .Select(t => new { t.MaTuyen, Text = t.DiemDi + " -> " + t.DiemDen })
					 .ToList();
				return data.Select(t => new KeyValuePair<int, string>(t.MaTuyen, t.Text))
				   .ToList();
			}
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			using (var dialog = new GiaVeDialog())
			{
				dialog.BindNhaXe(GetNhaXeItems());
				dialog.BindLoaiXe(GetLoaiXeItems());
				dialog.BindTuyen(GetTuyenItems());
				dialog.SetMode(GiaVeDialogMode.Add);
				
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var gv = new GiaVe
					{
						MaNX = dialog.SelectedMaNX.GetValueOrDefault(),
						MaLX = dialog.SelectedMaLX.GetValueOrDefault(),
						MaTuyen = dialog.SelectedMaTuyen.GetValueOrDefault(),
						GiaVe1 = dialog.GiaVeValue
					};
					
					string result = _bus.Add(gv);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
					
					if (result.Contains("thành công")) 
						UCGiaVe_Load(null, null);
				}
			}
		}

		private void btnThemChuaCo_Click(object sender, EventArgs e)
		{
			if (dgvChuaCo.SelectedRows.Count == 0)
			{
				MessageBox.Show("Vui lòng chọn tổ hợp cần thêm giá vé.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			var r = dgvChuaCo.SelectedRows[0];
			int rowIndex = r.Index;
			
			if (rowIndex < 0 || _dataChuaCo == null || rowIndex >= _dataChuaCo.Count)
			{
				MessageBox.Show("Không thể lấy thông tin dòng được chọn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Lấy dữ liệu từ list theo index
			var selectedItem = _dataChuaCo[rowIndex];

			using (var dialog = new GiaVeDialog())
			{
				dialog.BindNhaXe(GetNhaXeItems());
				dialog.BindLoaiXe(GetLoaiXeItems());
				dialog.BindTuyen(GetTuyenItems());
				dialog.SetMode(GiaVeDialogMode.Add);
				
				// Tự động điền thông tin từ dòng được chọn
				dialog.SelectedMaNX = selectedItem.MaNX;
				dialog.SelectedMaLX = selectedItem.MaLX;
				dialog.SelectedMaTuyen = selectedItem.MaTuyen;
				
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var gv = new GiaVe
					{
						MaNX = dialog.SelectedMaNX.GetValueOrDefault(),
						MaLX = dialog.SelectedMaLX.GetValueOrDefault(),
						MaTuyen = dialog.SelectedMaTuyen.GetValueOrDefault(),
						GiaVe1 = dialog.GiaVeValue
					};
					
					string result = _bus.Add(gv);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
					
					if (result.Contains("thành công")) 
						UCGiaVe_Load(null, null);
				}
			}
		}

		private void btnXem_Click(object sender, EventArgs e)
		{
			if (dgvDaCo.SelectedRows.Count == 0) return;
			var r = dgvDaCo.SelectedRows[0];
			if (r.Cells[0].Value == null)
			{
				MessageBox.Show("Dòng này chưa có giá vé. Vui lòng chọn 'Thêm' để thiết lập giá vé.", 
					"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			
			int maGV = int.Parse(r.Cells[0].Value.ToString());
			var existing = _bus.GetById(maGV);
			if (existing == null) return;
			
			using (var dialog = new GiaVeDialog())
			{
				dialog.BindNhaXe(GetNhaXeItems());
				dialog.BindLoaiXe(GetLoaiXeItems());
				dialog.BindTuyen(GetTuyenItems());
				dialog.SetMode(GiaVeDialogMode.View);
				dialog.MaGV = existing.MaGV.ToString();
				dialog.SelectedMaNX = existing.MaNX;
				dialog.SelectedMaLX = existing.MaLX;
				dialog.SelectedMaTuyen = existing.MaTuyen;
				dialog.GiaVeValue = existing.GiaVe1;
				dialog.ShowDialog();
			}
		}

		private void btnSua_Click(object sender, EventArgs e)
		{
			if (dgvDaCo.SelectedRows.Count == 0) return;
			var r = dgvDaCo.SelectedRows[0];
			if (r.Cells[0].Value == null)
			{
				MessageBox.Show("Dòng này chưa có giá vé. Vui lòng chọn 'Thêm' để thiết lập giá vé.", 
					"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			
			int maGV = int.Parse(r.Cells[0].Value.ToString());
			var existing = _bus.GetById(maGV);
			if (existing == null)
			{
				MessageBox.Show("Không tìm thấy giá vé.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			using (var dialog = new GiaVeDialog())
			{
				dialog.BindNhaXe(GetNhaXeItems());
				dialog.BindLoaiXe(GetLoaiXeItems());
				dialog.BindTuyen(GetTuyenItems());
				dialog.SetMode(GiaVeDialogMode.Edit);
				dialog.MaGV = existing.MaGV.ToString();
				dialog.SelectedMaNX = existing.MaNX;
				dialog.SelectedMaLX = existing.MaLX;
				dialog.SelectedMaTuyen = existing.MaTuyen;
				dialog.GiaVeValue = existing.GiaVe1;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var update = new GiaVe
					{
						MaGV = existing.MaGV,
						MaNX = dialog.SelectedMaNX.GetValueOrDefault(),
						MaLX = dialog.SelectedMaLX.GetValueOrDefault(),
						MaTuyen = dialog.SelectedMaTuyen.GetValueOrDefault(),
						GiaVe1 = dialog.GiaVeValue
					};

					string result = _bus.Update(update);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
					
					if (result.Contains("thành công")) 
						UCGiaVe_Load(null, null);
				}
			}
		}

		private void btnXoa_Click(object sender, EventArgs e)
		{
			if (dgvDaCo.SelectedRows.Count == 0)
			{
				MessageBox.Show("Vui lòng chọn giá vé cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			
			var r = dgvDaCo.SelectedRows[0];
			if (r.Cells[0].Value == null)
			{
				MessageBox.Show("Dòng này chưa có giá vé nên không thể xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			
			int maGV = int.Parse(r.Cells[0].Value.ToString());
			
			// Lấy thông tin để hiển thị trong câu hỏi xác nhận
			string nhaXeName = r.Cells[1].Value?.ToString() ?? "N/A";
			string loaiXeName = r.Cells[2].Value?.ToString() ?? "N/A";
			string tuyenName = r.Cells[3].Value?.ToString() ?? "N/A";
			string giaVe = r.Cells[4].Value?.ToString() ?? "N/A";
			
			// Hiển thị dialog xác nhận xóa
			var confirmResult = MessageBox.Show(
				$"Bạn có chắc chắn muốn xóa giá vé?\n\n" +
				$"Nhà xe: {nhaXeName}\n" +
				$"Loại xe: {loaiXeName}\n" +
				$"Tuyến: {tuyenName}\n" +
				$"Giá vé: {giaVe} ₫",
				"Xác nhận xóa",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);
			
			if (confirmResult == DialogResult.Yes)
			{
				string result = _bus.Delete(maGV);
				MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
					result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
				
				if (result.Contains("thành công")) 
				{
					UCGiaVe_Load(null, null);
				}
			}
		}

		private void FilterDaCo()
		{
			if (_dataDaCo == null) return;
			
			string keyword = (txtSearch.Text ?? string.Empty).Trim().ToLowerInvariant();
			if (keyword.Length == 0) 
			{ 
				LoadDataDaCo(_dataDaCo); 
				return; 
			}
			
			int selected = cbbLoai.SelectedIndex;
			List<dynamic> filtered = _dataDaCo;
			
			switch (selected)
			{
				case 1: // Mã giá vé
					filtered = _dataDaCo.Where(x => x.MaGV != null && x.MaGV.ToString().ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 2: // Nhà xe
					filtered = _dataDaCo.Where(x => (x.NhaXeName ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 3: // Loại xe
					filtered = _dataDaCo.Where(x => (x.LoaiXeName ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 4: // Tuyến đường
					filtered = _dataDaCo.Where(x => (x.TuyenName ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 5: // Giá vé
					filtered = _dataDaCo.Where(x => (x.GiaVe ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				default: // Tất cả
					filtered = _dataDaCo.Where(x => new[] 
					{ 
						x.MaGV?.ToString() ?? "", 
						x.NhaXeName, 
						x.LoaiXeName, 
						x.TuyenName, 
						x.GiaVe 
					}.Any(v => (v ?? string.Empty).ToLowerInvariant().Contains(keyword))).ToList();
					break;
			}
			
			LoadDataDaCo(filtered);
		}

		private void FilterChuaCo()
		{
			if (_dataChuaCo == null) return;
			
			string keyword = (txtSearchChuaCo.Text ?? string.Empty).Trim().ToLowerInvariant();
			if (keyword.Length == 0) 
			{ 
				LoadDataChuaCo(_dataChuaCo); 
				return; 
			}
			
			int selected = cbbLoaiChuaCo.SelectedIndex;
			List<dynamic> filtered = _dataChuaCo;
			
			switch (selected)
			{
				case 1: // Nhà xe
					filtered = _dataChuaCo.Where(x => (x.NhaXeName ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 2: // Loại xe
					filtered = _dataChuaCo.Where(x => (x.LoaiXeName ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 3: // Tuyến đường
					filtered = _dataChuaCo.Where(x => (x.TuyenName ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				default: // Tất cả
					filtered = _dataChuaCo.Where(x => new[] 
					{ 
						x.NhaXeName,
						x.LoaiXeName, 
						x.TuyenName
					}.Any(v => (v ?? string.Empty).ToLowerInvariant().Contains(keyword))).ToList();
					break;
			}
			
			LoadDataChuaCo(filtered);
		}

		private void dgvDaCo_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				EnableBtnDaCo(false);
				return;
			}
			
			if (dgvDaCo.Rows[e.RowIndex].Cells[0].Value == null)
			{
				EnableBtnDaCo(false);
				return;
			}
			
			dgvDaCo.Rows[e.RowIndex].Selected = true;
			
			if (dgvDaCo.Rows[e.RowIndex].Selected && btnSua.Enabled == false)
			{
				EnableBtnDaCo(true);
			}
		}

		private void dgvChuaCo_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				btnThemChuaCo.Enabled = false;
				return;
			}
			
			dgvChuaCo.Rows[e.RowIndex].Selected = true;
			btnThemChuaCo.Enabled = true;
		}

		private void EnableBtnDaCo(bool enabled)
		{
			btnXem.Enabled = enabled;
			btnSua.Enabled = enabled;
			btnXoa.Enabled = enabled;
		}
	}
}
