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
	public partial class UCXe : UserControl
	{
		private BXMDEntities db = new BXMDEntities();
		private XeBUS _xeBUS = new XeBUS();
		private List<dynamic> data;

		public UCXe()
		{
			InitializeComponent();
		}

		private void UCXe_Load(object sender, EventArgs e)
		{
			data = (from xe in db.Xes
					select new
					{
						MaXe = xe.MaXe.ToString(),
						BienSoXe = xe.BienSoXe,
						NhaXe = xe.NhaXe.TenNX,
						LoaiXe = xe.LoaiXe.TenLX,
						SoCho = xe.LoaiXe.SoCho.ToString(),
						TrangThai = xe.TrangThai == true ? "Ho?t ð?ng" : "Ng?ng ho?t ð?ng"
					}).ToList<dynamic>();
			LoadData(data);
			initCbb();
			EnableBtn(false);
		}

		private void initCbb()
		{
			cbbLoai.Items.Add("T?t c?");
			cbbLoai.Items.Add("M? xe");
			cbbLoai.Items.Add("Bi?n s? xe");
			cbbLoai.Items.Add("Nhà xe");
			cbbLoai.Items.Add("Lo?i xe");
			cbbLoai.Items.Add("Tr?ng thái");
			cbbLoai.SelectedIndex = 0;
		}

		private void LoadData(List<dynamic> list)
		{
			dgv.Rows.Clear();
			foreach (var item in list)
			{
				dgv.Rows.Add(item.MaXe, item.BienSoXe, item.NhaXe, item.LoaiXe, item.SoCho, item.TrangThai);
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
					filteredData = data.Where(xe =>
						new string[] { 
							xe.MaXe?.ToString(), 
							xe.BienSoXe?.ToString(), 
							xe.NhaXe?.ToString(), 
							xe.LoaiXe?.ToString(), 
							xe.TrangThai?.ToString() 
						}
						.Any(field => field != null && field.ToLower().Contains(keyword))
					).ToList();
					break;
				case 1:
					filteredData = data.Where(xe =>
						xe.MaXe?.ToString().ToLower().Contains(txtSearch.Text.ToLower()) ?? false
					).ToList<dynamic>();
					break;
				case 2:
					filteredData = data.Where(xe =>
						xe.BienSoXe?.ToString().ToLower().Contains(txtSearch.Text.ToLower()) ?? false
					).ToList<dynamic>();
					break;
				case 3:
					filteredData = data.Where(xe =>
						xe.NhaXe?.ToString().ToLower().Contains(txtSearch.Text.ToLower()) ?? false
					).ToList<dynamic>();
					break;
				case 4:
					filteredData = data.Where(xe =>
						xe.LoaiXe?.ToString().ToLower().Contains(txtSearch.Text.ToLower()) ?? false
					).ToList<dynamic>();
					break;
				case 5:
					filteredData = data.Where(xe =>
						xe.TrangThai?.ToString().ToLower().Contains(txtSearch.Text.ToLower()) ?? false
					).ToList<dynamic>();
					break;
				default:
					return;
			}
			LoadData(filteredData);
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

		private void dgv_SelectionChanged(object sender, EventArgs e)
		{
			bool hasSelectedRow = dgv.SelectedRows.Count > 0;
			EnableBtn(hasSelectedRow);
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
			if (dgv.Rows[e.RowIndex].Selected == true && btnSua.Enabled == false)
			{
				EnableBtn(true);
			}
		}

		private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0) return;
			if (dgv.Rows[e.RowIndex].Cells[0].Value == null) return;
			btnXem_Click(sender, e);
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			using (var dialog = new XeDialog())
			{
				dialog.Text = "Thêm Xe M?i";
				dialog.Controls["txtMaXe"].Enabled = false;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					Xe xe = new Xe
					{
						BienSoXe = dialog.BienSoXe,
						MaNX = dialog.MaNhaXe ?? 0,
						MaLX = dialog.MaLoaiXe.Value,
						TrangThai = dialog.TrangThai == "Ho?t ð?ng" ? true : false
					};

					string result = _xeBUS.Add(xe);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

					if (result.Contains("thành công"))
					{
						UCXe_Load(null, null);
					}
				}
			}
		}

		private void btnXem_Click(object sender, EventArgs e)
		{
			using (var dialog = new XeDialog())
			{
				if (dgv.SelectedRows.Count == 0) return;
				var selectedRow = dgv.SelectedRows[0];

				if (selectedRow.Cells[0].Value == null) return;

				int maXe = int.Parse(selectedRow.Cells[0].Value.ToString());
				var xe = db.Xes.Find(maXe);

				if (xe == null) return;

				dialog.MaXe = xe.MaXe.ToString();
				dialog.BienSoXe = xe.BienSoXe;
				dialog.MaNhaXe = xe.MaNX;
				dialog.MaLoaiXe = xe.MaLX;
				dialog.TrangThai = xe.TrangThai == true ? "Ho?t ð?ng" : "Ng?ng ho?t ð?ng";

				dialog.Controls["btnPrimary"].Enabled = false;
				dialog.Controls["txtMaXe"].Enabled = false;
				dialog.Controls["txtBienSoXe"].Enabled = false;
				dialog.Controls["cbbNhaXe"].Enabled = false;
				dialog.Controls["cbbLoaiXe"].Enabled = false;
				dialog.Controls["cbbTrangThai"].Enabled = false;
				dialog.SetReadOnlyMode();
				dialog.ShowDialog();
			}
		}

		private void btnSua_Click(object sender, EventArgs e)
		{
			using (var dialog = new XeDialog())
			{
				if (dgv.SelectedRows.Count == 0) return;
				var selectedRow = dgv.SelectedRows[0];

				if (selectedRow.Cells[0].Value == null) return;

				int maXe = int.Parse(selectedRow.Cells[0].Value.ToString());
				var xe = db.Xes.Find(maXe);

				if (xe == null) return;

				dialog.MaXe = xe.MaXe.ToString();
				dialog.BienSoXe = xe.BienSoXe;
				dialog.MaNhaXe = xe.MaNX;
				dialog.MaLoaiXe = xe.MaLX;
				dialog.TrangThai = xe.TrangThai == true ? "Ho?t ð?ng" : "Ng?ng ho?t ð?ng";
				dialog.Controls["txtMaXe"].Enabled = false;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					Xe xeUpdate = new Xe
					{
						MaXe = int.Parse(dialog.MaXe),
						BienSoXe = dialog.BienSoXe,
						MaNX = dialog.MaNhaXe ?? 0,
						MaLX = dialog.MaLoaiXe.Value,
						TrangThai = dialog.TrangThai == "Ho?t ð?ng" ? true : false
					};

					string result = _xeBUS.Update(xeUpdate);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

					if (result.Contains("thành công"))
					{
						UCXe_Load(null, null);
					}
				}
			}
		}

		private void btnXoa_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0)
			{
				MessageBox.Show("Vui l?ng ch?n xe c?n xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var selectedRow = dgv.SelectedRows[0];

			if (selectedRow.Cells[0].Value == null)
			{
				MessageBox.Show("Vui l?ng ch?n xe c?n xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			string maXe = selectedRow.Cells[0].Value.ToString();
			string bienSoXe = selectedRow.Cells[1].Value.ToString();

			DialogResult confirmResult = MessageBox.Show(
				$"B?n có ch?c ch?n mu?n xóa xe '{bienSoXe}' không?",
				"Xác nh?n xóa",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);

			if (confirmResult == DialogResult.Yes)
			{
				string result = _xeBUS.Delete(int.Parse(maXe));
				MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
					result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

				if (result.Contains("thành công"))
				{
					UCXe_Load(null, null);
				}
			}
		}

		private void label1_Click(object sender, EventArgs e)
		{
			// Optional: implement if needed
		}
	}
}
