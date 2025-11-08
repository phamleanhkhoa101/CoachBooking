using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	/// <summary>
	/// Dialog ?? ch?n tài kho?n khi thêm/s?a Nhân viên ho?c Khách hàng
	/// </summary>
	public partial class GanTaiKhoanDialog : Form
	{
		public int? SelectedMaTK
		{
			get 
			{ 
				if (cbbTaiKhoan.SelectedValue == null || (int)cbbTaiKhoan.SelectedValue == 0)
					return null;
				return cbbTaiKhoan.SelectedValue is int v ? (int?)v : null;
			}
			set 
			{ 
				if (value.HasValue && value.Value > 0)
					cbbTaiKhoan.SelectedValue = value.Value;
				else
					cbbTaiKhoan.SelectedIndex = 0;
			}
		}

		public GanTaiKhoanDialog()
		{
			InitializeComponent();
		}

		public void BindTaiKhoan(IEnumerable<KeyValuePair<int, string>> items, int? currentMaTK = null)
		{
			var list = items?.ToList() ?? new List<KeyValuePair<int, string>>();
			list.Insert(0, new KeyValuePair<int, string>(0, "-- Không ch?n --"));
			cbbTaiKhoan.DataSource = list;
			cbbTaiKhoan.DisplayMember = "Value";
			cbbTaiKhoan.ValueMember = "Key";
			
			if (currentMaTK.HasValue && currentMaTK.Value > 0)
				cbbTaiKhoan.SelectedValue = currentMaTK.Value;
			else
				cbbTaiKhoan.SelectedIndex = 0;
		}

		private void btnChon_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnHuy_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
