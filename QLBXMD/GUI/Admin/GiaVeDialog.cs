using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public enum GiaVeDialogMode { Add, View, Edit }

	public partial class GiaVeDialog : Form
	{
		public GiaVeDialogMode Mode { get; private set; } = GiaVeDialogMode.Add;

		public string MaGV { get => txtMaGV.Text; set => txtMaGV.Text = value; }
		public decimal GiaVeValue { get => numGiaVe.Value; set => numGiaVe.Value = value; }

		public int? SelectedMaNX 
		{ 
			get { return cbbNhaXe.SelectedValue is int v ? (int?)v : (cbbNhaXe.SelectedValue as int?); } 
			set { if (value.HasValue) cbbNhaXe.SelectedValue = value.Value; else cbbNhaXe.SelectedIndex = -1; } 
		}
		
		public int? SelectedMaLX 
		{ 
			get { return cbbLoaiXe.SelectedValue is int v ? (int?)v : (cbbLoaiXe.SelectedValue as int?); } 
			set { if (value.HasValue) cbbLoaiXe.SelectedValue = value.Value; else cbbLoaiXe.SelectedIndex = -1; } 
		}
		
		public int? SelectedMaTuyen 
		{ 
			get { return cbbTuyen.SelectedValue is int v ? (int?)v : (cbbTuyen.SelectedValue as int?); } 
			set { if (value.HasValue) cbbTuyen.SelectedValue = value.Value; else cbbTuyen.SelectedIndex = -1; } 
		}

		public GiaVeDialog()
		{
			InitializeComponent();
		}

		public void BindNhaXe(IEnumerable<KeyValuePair<int, string>> items)
		{
			cbbNhaXe.DataSource = items?.ToList();
			cbbNhaXe.DisplayMember = "Value";
			cbbNhaXe.ValueMember = "Key";
		}

		public void BindLoaiXe(IEnumerable<KeyValuePair<int, string>> items)
		{
			cbbLoaiXe.DataSource = items?.ToList();
			cbbLoaiXe.DisplayMember = "Value";
			cbbLoaiXe.ValueMember = "Key";
		}

		public void BindTuyen(IEnumerable<KeyValuePair<int, string>> items)
		{
			cbbTuyen.DataSource = items?.ToList();
			cbbTuyen.DisplayMember = "Value";
			cbbTuyen.ValueMember = "Key";
		}

		public void SetMode(GiaVeDialogMode mode)
		{
			Mode = mode;
			switch (mode)
			{
				case GiaVeDialogMode.Add:
					Text = "Thêm giá vé";
					btnPrimary.Text = "Thêm";
					txtMaGV.Enabled = false;
					SetReadOnly(false);
					break;
				case GiaVeDialogMode.Edit:
					Text = "S?a giá vé";
					btnPrimary.Text = "L?u";
					txtMaGV.Enabled = false;
					SetReadOnly(false);
					// Không cho phép s?a nhà xe, lo?i xe, tuy?n vì ?ây là khóa chính
					cbbNhaXe.Enabled = false;
					cbbLoaiXe.Enabled = false;
					cbbTuyen.Enabled = false;
					break;
				case GiaVeDialogMode.View:
					Text = "Xem giá vé";
					btnPrimary.Visible = false;
					btnCancel.Text = "?óng";
					SetReadOnly(true);
					break;
			}
		}

		private void SetReadOnly(bool ro)
		{
			txtMaGV.Enabled = !ro;
			cbbNhaXe.Enabled = !ro;
			cbbLoaiXe.Enabled = !ro;
			cbbTuyen.Enabled = !ro;
			numGiaVe.Enabled = !ro;

			txtMaGV.TabStop = !ro;
			cbbNhaXe.TabStop = !ro;
			cbbLoaiXe.TabStop = !ro;
			cbbTuyen.TabStop = !ro;
			numGiaVe.TabStop = !ro;
		}

		private void btnPrimary_Click(object sender, EventArgs e)
		{
			if (ValidateInput())
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private bool ValidateInput()
		{
			if (Mode == GiaVeDialogMode.View)
				return true;

			if (SelectedMaNX == null)
			{
				MessageBox.Show("Vui lòng ch?n nhà xe.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cbbNhaXe.Focus();
				return false;
			}

			if (SelectedMaLX == null)
			{
				MessageBox.Show("Vui lòng ch?n lo?i xe.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cbbLoaiXe.Focus();
				return false;
			}

			if (SelectedMaTuyen == null)
			{
				MessageBox.Show("Vui lòng ch?n tuy?n ???ng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cbbTuyen.Focus();
				return false;
			}

			if (numGiaVe.Value <= 0)
			{
				MessageBox.Show("Giá vé ph?i l?n h?n 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				numGiaVe.Focus();
				return false;
			}

			return true;
		}
	}
}
