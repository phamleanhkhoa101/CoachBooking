using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.BUS;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	public partial class UCGiaVe : UserControl
	{
		private readonly GiaVeBUS _bus = new GiaVeBUS();
		private List<dynamic> _data;

		public UCGiaVe()
		{
			InitializeComponent();
			this.Load += UCGiaVe_Load;
			this.dgv.SelectionChanged += Dgv_SelectionChanged;
		}

		private void Dgv_SelectionChanged(object sender, EventArgs e)
		{
			bool has = dgv.SelectedRows.Count > 0 && dgv.SelectedRows[0].Cells[0].Value != null;
			btnXem.Enabled = has; 
			btnSua.Enabled = has; 
			btnXoa.Enabled = has;
		}

		private void UCGiaVe_Load(object sender, EventArgs e)
		{
			try
			{
				SetupGridHeaders();

				var list = _bus.GetAll() ?? new List<GiaVe>();
				_data = list
					.Select(gv => new
					{
						MaGV = gv.MaGV,
						NhaXeName = gv.NhaXe != null ? gv.NhaXe.TenNX : (gv.MaNX != 0 ? $"NX {gv.MaNX}" : ""),
						LoaiXeName = gv.LoaiXe != null ? gv.LoaiXe.TenLX : (gv.MaLX != 0 ? $"LX {gv.MaLX}" : ""),
						TuyenName = gv.TuyenDuong != null ? ($"{gv.TuyenDuong.DiemDi} -> {gv.TuyenDuong.DiemDen}") : (gv.MaTuyen != 0 ? $"Tuyến {gv.MaTuyen}" : ""),
						GiaVe = gv.GiaVe1.ToString("N0"),
						MaNX = gv.MaNX,
						MaLX = gv.MaLX,
						MaTuyen = gv.MaTuyen,
						GiaVeValue = gv.GiaVe1
					})
					.ToList<dynamic>();

				LoadData(_data);
				InitCbb();
				btnXem.Enabled = btnSua.Enabled = btnXoa.Enabled = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Tải dữ liệu thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void SetupGridHeaders()
		{
			dgv.Columns.Clear();
			dgv.Columns.Add("MaGV", "Mã giá vé");
			dgv.Columns.Add("NhaXe", "Nhà xe");
			dgv.Columns.Add("LoaiXe", "Loại xe");
			dgv.Columns.Add("Tuyen", "Tuyến đường");
			dgv.Columns.Add("GiaVe", "Giá vé (₫)");
		}

		private void InitCbb()
		{
			cbbLoai.Items.Clear();
			cbbLoai.Items.Add("Tất cả");
			cbbLoai.Items.Add("Mã giá vé");
			cbbLoai.Items.Add("Nhà xe");
			cbbLoai.Items.Add("Loại xe");
			cbbLoai.Items.Add("Tuyến đường");
			cbbLoai.Items.Add("Giá vé");
			cbbLoai.SelectedIndex = 0;
			cbbLoai.SelectedIndexChanged += cbbLoai_SelectedIndexChanged;
		}

		private void LoadData(List<dynamic> list)
		{
			dgv.Rows.Clear();

			foreach (var item in list)
			{
				dgv.Rows.Add(item.MaGV, item.NhaXeName, item.LoaiXeName, item.TuyenName, item.GiaVe);
			}

			dgv.ClearSelection();
		}

		private void txtSearch_TextChanged(object sender, EventArgs e)
		{
			Filter();
		}

		private void cbbLoai_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadData(_data);
			txtSearch.Clear();
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

		private void btnXem_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0) return;
			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null) return;
			
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
			if (dgv.SelectedRows.Count == 0) return;
			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null) return;
			
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
			if (dgv.SelectedRows.Count == 0)
			{
				MessageBox.Show("Vui lòng chọn giá vé cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			
			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null)
			{
				MessageBox.Show("Vui lòng chọn giá vé cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

		private void Filter()
		{
			if (_data == null) return;
			
			string keyword = (txtSearch.Text ?? string.Empty).Trim().ToLowerInvariant();
			if (keyword.Length == 0) 
			{ 
				LoadData(_data); 
				return; 
			}
			
			int selected = cbbLoai.SelectedIndex;
			List<dynamic> filtered = _data;
			
			switch (selected)
			{
				case 1: // Mã giá vé
					filtered = _data.Where(x => x.MaGV.ToString().ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 2: // Nhà xe
					filtered = _data.Where(x => (x.NhaXeName ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 3: // Loại xe
					filtered = _data.Where(x => (x.LoaiXeName ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 4: // Tuyến đường
					filtered = _data.Where(x => (x.TuyenName ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 5: // Giá vé
					filtered = _data.Where(x => (x.GiaVe ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				default: // Tất cả
					filtered = _data.Where(x => new[] 
					{ 
						x.MaGV.ToString(), 
						x.NhaXeName, 
						x.LoaiXeName, 
						x.TuyenName, 
						x.GiaVe 
					}
					.Any(v => (v ?? string.Empty).ToLowerInvariant().Contains(keyword))).ToList();
					break;
			}
			
			LoadData(filtered);
		}

		private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				EnableBtn(false);
				return;
			}
			
			if (dgv.Rows[e.RowIndex].Cells[0].Value == null)
			{
				EnableBtn(false);
				return;
			}
			
			dgv.Rows[e.RowIndex].Selected = true;
			
			if (dgv.Rows[e.RowIndex].Selected && btnSua.Enabled == false)
			{
				EnableBtn(true);
			}
		}

		private void EnableBtn(bool enabled)
		{
			btnXem.Enabled = enabled;
			btnSua.Enabled = enabled;
			btnXoa.Enabled = enabled;
		}
	}
}
