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
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public partial class UCNhaXe : UserControl
	{
		private BXMDEntities db = new BXMDEntities();
		private NhaXeBUS _nhaXeBUS = new NhaXeBUS();
		private List<dynamic> data;
		public UCNhaXe()
		{
			InitializeComponent();
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			Filter();
		}

		private void UCNhaXe_Load(object sender, EventArgs e)
		{
			data = (from nx in db.NhaXes
					select new
					{
						MaNX = nx.MaNX.ToString(),
						TenNX = nx.TenNX,
						SDT = nx.SDT,
						Email = nx.Email,
						TrangThai = nx.TrangThai == true ? "Hoạt động" : "Ngừng hoạt động"
					}).ToList<dynamic>();
			LoadData(data);
			cbbLoai.SelectedIndex = 0;

			EnableBtn(false);
		}
		private void LoadData(List<dynamic> list)
		{
			dgvNhaXe.Rows.Clear();
			foreach (var item in list)
			{
				dgvNhaXe.Rows.Add(item.MaNX, item.TenNX, item.SDT, item.Email, item.TrangThai);
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
					filteredData = data.Where(nx =>
						new string[] { nx.MaNX?.ToString(), nx.TenNX?.ToString(), nx.SDT?.ToString(), nx.Email?.ToString(), nx.TrangThai?.ToString() }
						.Any(field => field != null && field.ToLower().Contains(keyword))
					).ToList();
					LoadData(filteredData);
					break;
				case 1:
					filteredData = data.Where(nx =>
						nx.MaNX?.ToString().ToLower().Contains(txtSearch.Text.ToLower()) ?? false
					).ToList<dynamic>();
					LoadData(filteredData);
					break;
				case 2:
					filteredData = data.Where(nx =>
						nx.TenNX?.ToString().ToLower().Contains(txtSearch.Text.ToLower()) ?? false
					).ToList<dynamic>();
					LoadData(filteredData);
					break;
				case 3:
					filteredData = data.Where(nx =>
						nx.SDT?.ToString().Contains(txtSearch.Text) ?? false
					).ToList<dynamic>();
					LoadData(filteredData);
					break;
				case 4:
					filteredData = data.Where(nx =>
						nx.Email?.ToString().ToLower().Contains(txtSearch.Text.ToLower()) ?? false
					).ToList<dynamic>();
					LoadData(filteredData);
					break;
				case 5:
					filteredData = data.Where(nx =>
						nx.TrangThai?.ToString().ToLower().Contains(txtSearch.Text.ToLower()) ?? false
					).ToList<dynamic>();
					LoadData(filteredData);
					break;
				default: return;
			}

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
		private void btnXem_Click(object sender, EventArgs e)
		{
			using (var dialog = new NhaXeDialog())
			{
				if (dgvNhaXe.SelectedRows.Count == 0) return;
				var selectedRow = dgvNhaXe.SelectedRows[0];
				
				// Kiểm tra dòng có dữ liệu không
				if (selectedRow.Cells[0].Value == null) return;
				
				dialog.MaNhaXe = selectedRow.Cells[0].Value.ToString();
				dialog.TenNhaXe = selectedRow.Cells[1].Value.ToString();
				dialog.SDT = selectedRow.Cells[2].Value.ToString();
				dialog.Email = selectedRow.Cells[3].Value.ToString();
				dialog.TrangThai = selectedRow.Cells[4].Value.ToString();

				dialog.Controls["btnThem"].Enabled = false;
				dialog.ShowDialog();
			}
		}

		private void btnSua_Click(object sender, EventArgs e)
		{
			using (var dialog = new NhaXeDialog())
			{
				if (dgvNhaXe.SelectedRows.Count == 0) return;
				var selectedRow = dgvNhaXe.SelectedRows[0];
				
				// Kiểm tra dòng có dữ liệu không
				if (selectedRow.Cells[0].Value == null) return;
				
				dialog.MaNhaXe = selectedRow.Cells[0].Value.ToString();
				dialog.TenNhaXe = selectedRow.Cells[1].Value.ToString();
				dialog.SDT = selectedRow.Cells[2].Value.ToString();
				dialog.Email = selectedRow.Cells[3].Value.ToString();
				dialog.TrangThai = selectedRow.Cells[4].Value.ToString();

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					NhaXe nx = new NhaXe
					{
						MaNX = int.Parse(dialog.MaNhaXe),
						TenNX = dialog.TenNhaXe,
						SDT = dialog.SDT,
						Email = dialog.Email,
						TrangThai = dialog.TrangThai == "Hoạt động" ? true : false
					};
					string result = _nhaXeBUS.Update(nx);
					MessageBox.Show(result);
					if (result.Contains("thành công"))
					{
						// Refresh data
						UCNhaXe_Load(null, null);
					}
				}
				;
			}
		}
		private void dgvNhaXe_SelectionChanged(object sender, EventArgs e)
		{
			bool hasSelectedRow = dgvNhaXe.SelectedRows.Count > 0;
			EnableBtn(hasSelectedRow);
		}

		private void btnXoa_Click(object sender, EventArgs e)
		{
			if (dgvNhaXe.SelectedRows.Count == 0)
			{
				MessageBox.Show("Vui lòng chọn nhà xe cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var selectedRow = dgvNhaXe.SelectedRows[0];
			
			// Kiểm tra dòng có dữ liệu không
			if (selectedRow.Cells[0].Value == null)
			{
				MessageBox.Show("Vui lòng chọn nhà xe cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			
			string maNX = selectedRow.Cells[0].Value.ToString();
			string tenNX = selectedRow.Cells[1].Value.ToString();

			DialogResult confirmResult = MessageBox.Show(
				$"Bạn có chắc chắn muốn xóa nhà xe '{tenNX}' không?",
				"Xác nhận xóa",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);

			if (confirmResult == DialogResult.Yes)
			{
				string result = _nhaXeBUS.Delete(int.Parse(maNX));
				MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
					result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

				if (result.Contains("thành công"))
				{
					// Refresh data
					UCNhaXe_Load(null, null);
				}
			}
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			using (var dialog = new NhaXeDialog())
			{
				dialog.Text = "Thêm Nhà Xe Mới";
				dialog.Controls["txtMaNhaXe"].Enabled = false;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					NhaXe nx = new NhaXe
					{
						TenNX = dialog.TenNhaXe,
						SDT = dialog.SDT,
						Email = dialog.Email,
						TrangThai = dialog.TrangThai == "Hoạt động" ? true : false
					};
					
					string result = _nhaXeBUS.Add(nx);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

					if (result.Contains("thành công"))
					{
						UCNhaXe_Load(null, null);
					}
				}
			}
		}

		private void dgvNhaXe_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			// Đảm bảo không click vào header row
			if (e.RowIndex < 0) {
				EnableBtn(false);
				return; }
			if (dgvNhaXe.Rows[e.RowIndex].Cells[0].Value == null)
			{
								EnableBtn(false);
				return;
			}
			// Chọn row khi click vào cell
			dgvNhaXe.Rows[e.RowIndex].Selected = true;
			if (dgvNhaXe.Rows[e.RowIndex].Selected ==true && btnSua.Enabled == false)
			{
				EnableBtn(true);
			}
		}

		private void dgvNhaXe_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			// Đảm bảo không click vào header row
			if (e.RowIndex < 0) return;

			// Kiểm tra xem dòng có dữ liệu không (không phải dòng trống)
			if (dgvNhaXe.Rows[e.RowIndex].Cells[0].Value == null) return;

			// Mở dialog xem khi double click
			btnXem_Click(sender, e);
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{

		}
	}
}
