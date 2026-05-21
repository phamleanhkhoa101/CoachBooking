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

        // Backing fields to allow pre-setting values before controls are data-bound
        private int? _initialMaNhaXe;
        private int? _initialMaLoaiXe;

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
			set
			{
				// store initial value; if combo already has data, apply immediately
				_initialMaNhaXe = value;
				if (cbbNhaXe.DataSource != null)
				{
					cbbNhaXe.SelectedValue = value;
				}
			}
		}

		public int? MaLoaiXe
		{
			get => cbbLoaiXe.SelectedValue as int?;
			set
			{
				_initialMaLoaiXe = value;
				if (cbbLoaiXe.DataSource != null)
				{
					cbbLoaiXe.SelectedValue = value;
				}
			}
		}

		public string SoCho
		{
			get => txtSoCho.Text.Trim();
			set => txtSoCho.Text = value;
		}
		public string TrangThai
		{
			get => cbbTrangThai.SelectedItem?.ToString();
			set => cbbTrangThai.SelectedIndex = cbbTrangThai.FindStringExact(value);
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

			//apply initial value if any
			if (_initialMaNhaXe != null)
			{
				cbbNhaXe.SelectedValue = _initialMaNhaXe;
			}

			//load cbb loai xe
			var loaiXe = db.LoaiXes.Where(lx => lx.TrangThai == true)
				.Select(lx => new { lx.MaLX, lx.TenLX })
						.ToList();

			cbbLoaiXe.DataSource = loaiXe;
			cbbLoaiXe.DisplayMember = "TenLX";
			cbbLoaiXe.ValueMember = "MaLX";
			cbbLoaiXe.SelectedIndex = -1;

			if (_initialMaLoaiXe != null)
			{
				cbbLoaiXe.SelectedValue = _initialMaLoaiXe;
			}

			txtSoCho.Enabled = false;
		}

        // Call this to make the dialog read-only / view-only and hide the primary action button
        public void SetReadOnlyMode()
        {
			this.Text = "Xem Chi Tiết Xe";
			Controls["pnlHeader"].Controls["lblHeader"].Text = "CHI TIẾT XE";
            // Hide primary (Thêm) button
            btnPrimary.Visible = false;

            // Make inputs read-only / disabled
            txtBienSoXe.ReadOnly = false;
            txtMaXe.ReadOnly = false;
            txtSoCho.ReadOnly = false;
            cbbNhaXe.Enabled = false;
            cbbLoaiXe.Enabled = false;
            cbbTrangThai.Enabled = false;
		}

		public void SetEditMode()
		{
			this.Text = "Cập Nhật Xe";
			Controls["pnlHeader"].Controls["lblHeader"].Text = "CẬP NHẬT XE";
			btnPrimary.Text = "Lưu";
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
