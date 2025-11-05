using QLBXMD.BUS;
using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public partial class UCLoaiXe : UserControl
	{

		private BXMDEntities db =  new BXMDEntities();
		private LoaiXeBUS _loaiXeBUS = new LoaiXeBUS();
		private List<dynamic> data;
		public UCLoaiXe()
		{
			InitializeComponent();
		}
		private void UCLoaiXe_Load(object sender, EventArgs e)
		{
			data = (from lx in db.LoaiXes
					select new
					{
						MaLX = lx.MaLX.ToString(),
						TenLX = lx.TenLX,
						SoCho = lx.SoCho.ToString(),
						TrangThai = lx.TrangThai == true ? "Hoạt động" : "Ngừng hoạt động"
					}).ToList<dynamic>();
			LoadData(data);
			initCbb();

			EnableBtn(false);
		}

		private void initCbb()
		{
			cbbLoai.Items.Add("Tất cả");
			cbbLoai.Items.Add("Mã loại xe");
			cbbLoai.Items.Add("Tên loại xe");
			cbbLoai.Items.Add("Số chỗ");
			cbbLoai.Items.Add("Trạng thái");
			cbbLoai.SelectedIndex = 0;
		}
		private void LoadData(List<dynamic> list)
		{
			dgv.Rows.Clear();
			foreach (var item in list)
			{
				dgv.Rows.Add(item.MaLX, item.TenLX, item.SoCho, item.TrangThai);
			}
		}

		private void Filter()
		{
			if (data == null) return;
			int selectedLoai = cbbLoai.SelectedIndex;
			if (string.IsNullOrEmpty(txtSearch.Text))
			{
				LoadData(data);
				return;
			}

			List<dynamic> filteredData = data;
			switch (selectedLoai)
			{
				case 0:
					string keyword = txtSearch.Text.Trim().ToLower();
					filteredData = data.Where(lx =>
						new string[] { lx.MaLX?.ToString(), lx.TenLX?.ToString(), lx.TrangThai?.ToString() }
						.Any(field => field != null && field.ToLower().Contains(keyword))
					).ToList();
					LoadData(filteredData);
					break;
				case 1:
					filteredData = data.Where(lx =>
						lx.MaLX?.ToString().ToLower().Contains(txtSearch.Text.ToLower()) ?? false
					).ToList<dynamic>();
					LoadData(filteredData);
					break;
				case 2:
					filteredData = data.Where(lx =>
						lx.TenLX?.ToString().ToLower().Contains(txtSearch.Text.ToLower()) ?? false
					).ToList<dynamic>();
					LoadData(filteredData);
					break;
				case 3:
					filteredData = data.Where(lx =>
						lx.SoCho?.ToString().ToLower().Contains(txtSearch.Text.ToLower()) ?? false
					).ToList<dynamic>();
					LoadData(filteredData);
					break;
				case 4:
					filteredData = data.Where(lx =>
						lx.TrangThai?.ToString().ToLower().Contains(txtSearch.Text.ToLower()) ?? false
					).ToList<dynamic>();
					LoadData(filteredData);
					break;
				default: return;
			}
		}

		private void txtSearch_TextChanged(object sender, EventArgs e)
		{
			Filter();
		}

		private void cbbLoai_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadData(data);
			txtSearch.Clear();
		}

		private void EnableBtn(bool b)
		{
			btnXem.Enabled = b;
			btnSua.Enabled = b;
			btnXoa.Enabled = b;
		}

		private void dgvLoaiXe_SelectionChanged(object sender, EventArgs e)
		{
			bool hasSelectedRow = dgv.SelectedRows.Count > 0;
			EnableBtn(hasSelectedRow);
		}

		private void dgvLoaiXe_CellClick(object sender, DataGridViewCellEventArgs e)
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
			if (dgv.Rows[e.RowIndex].Selected == true && btnSua.Enabled == false)
			{
				EnableBtn(true);
			}
		}

		private void dgvLoaiXe_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0) return;
			if (dgv.Rows[e.RowIndex].Cells[0].Value == null) return;
			btnXem_Click(sender, e);
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			using (var dialog = new LoaiXeDialog())
			{
				dialog.Text = "Thêm Loại Xe Mới";
				dialog.Controls["txtMaLoaiXe"].Enabled = false;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					LoaiXe lx = new LoaiXe
					{
						TenLX = dialog.TenLoaiXe,
						SoCho = int.Parse(dialog.SoCho),
						TrangThai = dialog.TrangThai == "Hoạt động" ? true : false
					};
					if (lx == null)
					{
						MessageBox.Show("Dữ liệu không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
					string result = _loaiXeBUS.Add(lx);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

					if (result.Contains("thành công"))
					{
						UCLoaiXe_Load(null, null);
					}
				}
			}
		}

		private void btnXem_Click(object sender, EventArgs e)
		{
			using (var dialog = new LoaiXeDialog())
			{
				if (dgv.SelectedRows.Count == 0) return;
				var selectedRow = dgv.SelectedRows[0];

				if (selectedRow.Cells[0].Value == null) return;

				dialog.MaLoaiXe = selectedRow.Cells[0].Value.ToString();
				dialog.TenLoaiXe = selectedRow.Cells[1].Value.ToString();
				dialog.SoCho = selectedRow.Cells[2].Value.ToString();
				dialog.TrangThai = selectedRow.Cells[3].Value.ToString();

				dialog.Controls["btnThem"].Enabled = false;
				dialog.ShowDialog();
			}
		}

		private void btnSua_Click(object sender, EventArgs e)
		{
			using (var dialog = new LoaiXeDialog())
			{
				if (dgv.SelectedRows.Count == 0) return;
				var selectedRow = dgv.SelectedRows[0];

				if (selectedRow.Cells[0].Value == null) return;

				dialog.MaLoaiXe = selectedRow.Cells[0].Value.ToString();
				dialog.TenLoaiXe = selectedRow.Cells[1].Value.ToString();
				dialog.SoCho = selectedRow.Cells[2].Value.ToString();
				dialog.TrangThai = selectedRow.Cells[3].Value.ToString();
				dialog.Controls["txtMaLoaiXe"].Enabled = false;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					LoaiXe lx = new LoaiXe
					{
						MaLX = int.Parse(dialog.MaLoaiXe),
						TenLX = dialog.TenLoaiXe,
						SoCho = int.Parse(dialog.SoCho),
						TrangThai = dialog.TrangThai == "Hoạt động" ? true : false
					};
					if (lx == null)
					{
						MessageBox.Show("Dữ liệu không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
					string result = _loaiXeBUS.Update(lx);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

					if (result.Contains("thành công"))
					{
						UCLoaiXe_Load(null, null);
					}
				}
			}
		}

		private void btnXoa_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0)
			{
				MessageBox.Show("Vui lòng chọn loại xe cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var selectedRow = dgv.SelectedRows[0];

			if (selectedRow.Cells[0].Value == null)
			{
				MessageBox.Show("Vui lòng chọn loại xe cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			string maLX = selectedRow.Cells[0].Value.ToString();
			string tenLX = selectedRow.Cells[1].Value.ToString();

			DialogResult confirmResult = MessageBox.Show(
				$"Bạn có chắc chắn muốn xóa loại xe '{tenLX}' không?",
				"Xác nhận xóa",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);

			if (confirmResult == DialogResult.Yes)
			{
				string result = _loaiXeBUS.Delete(int.Parse(maLX));
				MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
					result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

				if (result.Contains("thành công"))
				{
					UCLoaiXe_Load(null, null);
				}
			}
		}
	}
}

