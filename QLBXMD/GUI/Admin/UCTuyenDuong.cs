using QLBXMD.BUS;
using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public partial class UCTuyenDuong : UserControl
	{
		private readonly TuyenDuongBUS _tuyenDuongBUS = new TuyenDuongBUS();
		private List<dynamic> _data;

		public UCTuyenDuong()
		{
			InitializeComponent();
		}

		private void UCTuyenDuong_Load(object sender, EventArgs e)
		{
			// load data and wire events similar to UCLoaiXe
			_data = (_tuyenDuongBUS.GetAll() ?? new List<TuyenDuong>())
				.Select(td => new
				{
					MaTuyen = td.MaTuyen.ToString(),
					DiemDi = td.DiemDi,
					DiemDen = td.DiemDen,
					QuangDuong = td.QuangDuong?.ToString(),
					ThoiGianDuKien = td.ThoiGianDuKien.HasValue
						? ((int)td.ThoiGianDuKien.Value.TotalMinutes).ToString()
						: null,
					TrangThai = td.TrangThai == true ? "Hoạt động" : "Ngừng hoạt động"
				})
				.ToList<dynamic>();

			LoadData(_data);
			EnableBtn(false);
			initCbb();
		}

		private void LoadData(List<dynamic> list)
		{
			// switch to unbound mode (UCLoaiXe style)
			dgv.DataSource = null;
			dgv.Rows.Clear();

			foreach (var item in list)
			{
				dgv.Rows.Add(
					item.MaTuyen,
					item.DiemDi,
					item.DiemDen,
					item.QuangDuong,
					item.ThoiGianDuKien,
					item.TrangThai
				);
			}

			dgv.ClearSelection();
		}
		private void initCbb()
		{
			cbbLoai.Items.Add("Tất cả");
			cbbLoai.Items.Add("Mã tuyến");
			cbbLoai.Items.Add("Điểm đi");
			cbbLoai.Items.Add("Điểm đến");
			cbbLoai.Items.Add("Quãng đường");
			cbbLoai.Items.Add("Thời gian dự kiến");
			cbbLoai.Items.Add("Trạng thái");
			cbbLoai.SelectedIndex = 0;
		}
		private void EnableBtn(bool enabled)
		{
			btnXem.Enabled = enabled;
			btnSua.Enabled = enabled;
			btnXoa.Enabled = enabled;
		}

		private void dgvTuyenDuong_SelectionChanged(object sender, EventArgs e)
		{
			bool hasSelectedRow = dgv.SelectedRows.Count > 0;
			EnableBtn(hasSelectedRow);
		}

		private void dgvTuyenDuong_CellClick(object sender, DataGridViewCellEventArgs e)
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

		private void dgvTuyenDuong_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0) return;
			if (dgv.Rows[e.RowIndex].Cells[0].Value == null) return;
			btnSua_Click(sender, e);
		}

		private void btnLamMoi_Click(object sender, EventArgs e)
		{
			UCTuyenDuong_Load(null, null);
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			using (var dialog = new TuyenDuongDialog())
			{
				dialog.SetMode(TuyenDuongDialogMode.Add);

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var td = new TuyenDuong
					{
						DiemDi = dialog.DiemDi,
						DiemDen = dialog.DiemDen,
						QuangDuong = (int?)dialog.QuangDuongValue,
						ThoiGianDuKien = TimeSpan.FromMinutes(dialog.ThoiGianDuKienValue),
						TrangThai = dialog.TrangThaiValue
					};

					string result = _tuyenDuongBUS.Add(td);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

					if (result.Contains("thành công"))
					{
						UCTuyenDuong_Load(null, null);
					}
				}
			}
		}

		private void btnSua_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0) return;
			var selectedRow = dgv.SelectedRows[0];
			if (selectedRow.Cells[0].Value == null) return;

			int maTuyen = int.Parse(selectedRow.Cells[0].Value.ToString());
			var existing = _tuyenDuongBUS.GetById(maTuyen);
			if (existing == null)
			{
				MessageBox.Show("Không tìm thấy tuyến đường.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			using (var dialog = new TuyenDuongDialog())
			{
				dialog.SetMode(TuyenDuongDialogMode.Edit);
				dialog.MaTuyen = existing.MaTuyen.ToString();
				dialog.DiemDi = existing.DiemDi;
				dialog.DiemDen = existing.DiemDen;
				dialog.QuangDuongValue = existing.QuangDuong ?? 0;
				dialog.ThoiGianDuKienValue = existing.ThoiGianDuKien.HasValue 
					? (int)existing.ThoiGianDuKien.Value.TotalMinutes 
					: 60;
				dialog.TrangThaiValue = existing.TrangThai;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var update = new TuyenDuong
					{
						MaTuyen = existing.MaTuyen,
						DiemDi = dialog.DiemDi,
						DiemDen = dialog.DiemDen,
						QuangDuong = (int?)dialog.QuangDuongValue,
						ThoiGianDuKien = TimeSpan.FromMinutes(dialog.ThoiGianDuKienValue),
						TrangThai = dialog.TrangThaiValue
					};

					string result = _tuyenDuongBUS.Update(update);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

					if (result.Contains("thành công"))
					{
						UCTuyenDuong_Load(null, null);
					}
				}
			}
		}

		private void btnXoa_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0)
			{
				MessageBox.Show("Vui lòng chọn tuyến đường cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var selectedRow = dgv.SelectedRows[0];

			if (selectedRow.Cells[0].Value == null)
			{
				MessageBox.Show("Vui lòng chọn tuyến đường cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			string maTuyen = selectedRow.Cells[0].Value.ToString();
			string diemDi = selectedRow.Cells[1].Value?.ToString();
			string diemDen = selectedRow.Cells[2].Value?.ToString();

			var confirm = MessageBox.Show(
				$"Bạn có chắc chắn muốn xóa tuyến đường '{diemDi} -> {diemDen}'?",
				"Xác nhận xóa",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question);

			if (confirm == DialogResult.Yes)
			{
				string result = _tuyenDuongBUS.Delete(int.Parse(maTuyen));
				MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
					result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

				if (result.Contains("thành công"))
				{
					UCTuyenDuong_Load(null, null);
				}
			}
		}

		private void txtSearch_TextChanged(object sender, EventArgs e)
		{
			Filter();
		}

		private void cbbLoai_SelectedIndexChanged(object sender, EventArgs e)
		{
			// reset view like UCLoaiXe
			LoadData(_data);
			txtSearch.Clear();
		}

		private void btnXem_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0) return;
			var selectedRow = dgv.SelectedRows[0];
			if (selectedRow.Cells[0].Value == null) return;

			int maTuyen = int.Parse(selectedRow.Cells[0].Value.ToString());
			var existing = _tuyenDuongBUS.GetById(maTuyen);
			if (existing == null) return;

			using (var dialog = new TuyenDuongDialog())
			{
				dialog.SetMode(TuyenDuongDialogMode.View);
				dialog.MaTuyen = existing.MaTuyen.ToString();
				dialog.DiemDi = existing.DiemDi;
				dialog.DiemDen = existing.DiemDen;
				dialog.QuangDuongValue = existing.QuangDuong ?? 0;
				dialog.ThoiGianDuKienValue = existing.ThoiGianDuKien.HasValue 
					? (int)existing.ThoiGianDuKien.Value.TotalMinutes 
					: 60;
				dialog.TrangThaiValue = existing.TrangThai;
				dialog.ShowDialog();
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
				case 1:
					filtered = _data.Where(x => (x.MaTuyen?.ToString() ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;

				case 2:
					filtered = _data.Where(x => (x.DiemDi?.ToString() ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;

				case 3:
					filtered = _data.Where(x => (x.DiemDen?.ToString() ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;

				case 4:
					filtered = _data.Where(x => (x.QuangDuong?.ToString() ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;

				case 5:
					filtered = _data.Where(x => (x.ThoiGianDuKien?.ToString() ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;

				case 6:
					filtered = _data.Where(x => (x.TrangThai?.ToString() ?? "").ToLowerInvariant().Contains(keyword)).ToList();
					break;

				case 0:
				default:
					filtered = _data.Where(x =>
						new[] { x.MaTuyen, x.DiemDi, x.DiemDen, x.QuangDuong, x.ThoiGianDuKien, x.TrangThai }
							.Any(v => (v?.ToString() ?? "").ToLowerInvariant().Contains(keyword))
					).ToList();
					break;
			}

			LoadData(filtered);
		}
	}
}
