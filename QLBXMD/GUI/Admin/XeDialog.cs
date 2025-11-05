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
using System.Windows.Forms.VisualStyles;

namespace QLBXMD.GUI.Admin
{
	public partial class XeDialog : Form
	{
		private BXMDEntities db = new BXMDEntities();

		public string MaXe
		{
			get => txtMaXe.Text.Trim();
			set => txtMaXe.Text = value;
		}

		public string BienSoXe
		{
			get => txtBienSoXe.Text.Trim();
			set => txtBienSoXe.Text = value;
		}

		public int? MaNhaXe
		{
			get => cbbNhaXe.SelectedValue as int?;
			set => cbbNhaXe.SelectedValue = value;
		}

		public int? MaLoaiXe
		{
			get => cbbLoaiXe.SelectedValue as int?;
			set => cbbLoaiXe.SelectedValue = value;
		}

		public string SoCho
		{
			get => txtSoCho.Text.Trim();
			set => txtSoCho.Text = value;
		}
		public string TrangThai
		{
			get => cbbTrangThai.SelectedItem?.ToString();
			set => cbbTrangThai.SelectedItem = value;
		}
		public XeDialog()
		{
			InitializeComponent();
		}

		private void XeDialog_Load(object sender, EventArgs e)
		{
			//load cbb nha xe
			var nhaXe = db.NhaXes.Where(nx => nx.TrangThai == true)
						.Select(nx => new { nx.MaNX, nx.TenNX })
						.ToList();
			cbbNhaXe.DataSource = nhaXe;
			cbbNhaXe.DisplayMember = "TenNX";
			cbbNhaXe.ValueMember = "MaNX";
			cbbNhaXe.SelectedIndex = -1;

			//load cbb loai xe
			var loaiXe = db.LoaiXes.Where(lx => lx.TrangThai == true)
				.Select(lx => new { lx.MaLX, lx.TenLX })
						.ToList();

			cbbLoaiXe.DataSource = loaiXe;
			cbbLoaiXe.DisplayMember = "TenLX";
			cbbLoaiXe.ValueMember = "MaLX";
			cbbLoaiXe.SelectedIndex = -1;

			txtSoCho.Enabled =false;
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			// Validate dữ liệu trước khi đóng dialog
			if (string.IsNullOrWhiteSpace(BienSoXe))
			{
				MessageBox.Show("Vui lòng nhập biển số xe.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtBienSoXe.Focus();
				return;
			}

			if (MaNhaXe == null)
			{
				MessageBox.Show("Vui lòng chọn nhà xe.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cbbNhaXe.Focus();
				return;
			}

			if (MaLoaiXe == null)
			{
				MessageBox.Show("Vui lòng chọn loại xe.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cbbLoaiXe.Focus();
				return;
			}

			if (cbbTrangThai.SelectedItem == null)
			{
				MessageBox.Show("Vui lòng chọn trạng thái.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cbbTrangThai.Focus();
				return;
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnHuy_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void cbbLoaiXe_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbbLoaiXe.SelectedValue != null && cbbLoaiXe.SelectedValue is int)
			{
				int maLoaiXe = (int)cbbLoaiXe.SelectedValue;
				var loaiXe = db.LoaiXes.FirstOrDefault(lx => lx.MaLX == maLoaiXe);
				
				if (loaiXe != null)
				{
					txtSoCho.Text = loaiXe.SoCho.ToString();
				}
			}
			else
			{
				txtSoCho.Text = string.Empty;
			}
		}
	}
}
