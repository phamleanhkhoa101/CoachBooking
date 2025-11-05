using System;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public partial class TuyenDuongDialog : Form
	{
		public string MaTuyen
		{
			get => txtMaTuyen.Text.Trim();
			set => txtMaTuyen.Text = value;
		}

		public string DiemDi
		{
			get => txtDiemDi.Text.Trim();
			set => txtDiemDi.Text = value;
		}

		public string DiemDen
		{
			get => txtDiemDen.Text.Trim();
			set => txtDiemDen.Text = value;
		}

		public string QuangDuong
		{
			get => txtQuangDuong.Text.Trim();
			set => txtQuangDuong.Text = value;
		}

		public int? QuangDuongValue
		{
			get
			{
				if (int.TryParse(QuangDuong, out var v)) return v;
				return null;
			}
		}

		public string ThoiGianDuKien
		{
			get => txtThoiGianDuKien.Text.Trim();
			set => txtThoiGianDuKien.Text = value;
		}

		public int? ThoiGianDuKienValue
		{
			get
			{
				if (int.TryParse(ThoiGianDuKien, out var v)) return v;
				return null;
			}
		}

		public string TrangThai
		{
			get => cbbTrangThai.SelectedItem?.ToString();
			set => cbbTrangThai.SelectedItem = value;
		}

		public TuyenDuongDialog()
		{
			InitializeComponent();
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(DiemDi))
			{
				MessageBox.Show("Vui lòng nhập điểm đi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtDiemDi.Focus();
				return;
			}
			if (string.IsNullOrWhiteSpace(DiemDen))
			{
				MessageBox.Show("Vui lòng nhập điểm đến.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtDiemDen.Focus();
				return;
			}
			if (!QuangDuongValue.HasValue || QuangDuongValue.Value <= 0)
			{
				MessageBox.Show("Quãng đường phải là số dương.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtQuangDuong.Focus();
				return;
			}
			if (!ThoiGianDuKienValue.HasValue || ThoiGianDuKienValue.Value <= 0)
			{
				MessageBox.Show("Thời gian dự kiến phải là số dương (phút).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtThoiGianDuKien.Focus();
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

		private void txtNumber_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
			{
				e.Handled = true;
			}
		}
	}
}