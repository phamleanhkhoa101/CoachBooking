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
			SetupButtonEffects();
			SetupNumericUpDownValidation();
		}

		private void SetupButtonEffects()
		{
			// Hiệu ứng cho nút Primary (Lưu/Thêm)
			btnPrimary.MouseEnter += (s, e) => {
				btnPrimary.BackColor = Color.FromArgb(96, 195, 100);
			};
			btnPrimary.MouseLeave += (s, e) => {
				btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
			};
			btnPrimary.MouseDown += (s, e) => {
				btnPrimary.BackColor = Color.FromArgb(56, 155, 60);
			};
			btnPrimary.MouseUp += (s, e) => {
				btnPrimary.BackColor = Color.FromArgb(96, 195, 100);
			};

			// Hiệu ứng cho nút Cancel (Hủy)
			btnCancel.MouseEnter += (s, e) => {
				btnCancel.BackColor = Color.FromArgb(255, 87, 74);
			};
			btnCancel.MouseLeave += (s, e) => {
				btnCancel.BackColor = Color.FromArgb(244, 67, 54);
			};
			btnCancel.MouseDown += (s, e) => {
				btnCancel.BackColor = Color.FromArgb(200, 50, 40);
			};
			btnCancel.MouseUp += (s, e) => {
				btnCancel.BackColor = Color.FromArgb(255, 87, 74);
			};
		}

		private void SetupNumericUpDownValidation()
		{
			// Đảm bảo giá vé không thể là số âm
			numGiaVe.Minimum = 0;
			numGiaVe.Maximum = 100000000; // 100 triệu VNĐ
			numGiaVe.DecimalPlaces = 0;
			numGiaVe.Increment = 10000; // Tăng mỗi lần 10,000 VNĐ

			// Xử lý sự kiện khi giá trị thay đổi
			numGiaVe.ValueChanged += (s, e) => {
				// Đảm bảo giá trị không âm
				if (numGiaVe.Value < 0)
				{
					numGiaVe.Value = 0;
				}
			};

			// Xử lý khi người dùng nhập thủ công
			numGiaVe.KeyPress += (s, e) => {
				// Chỉ cho phép số và các phím điều khiển
				if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				{
					e.Handled = true;
				}
			};

			// Xử lý khi mất focus
			numGiaVe.Leave += (s, e) => {
				// Nếu giá trị nhỏ hơn 0, reset về 0
				if (numGiaVe.Value < 0)
				{
					numGiaVe.Value = 0;
					MessageBox.Show(
						"Giá vé không thể là số âm. Giá trị đã được đặt lại về 0.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
				}
			};
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

		public void LockCombination()
		{
			cbbNhaXe.Enabled = false;
			cbbLoaiXe.Enabled = false;
			cbbTuyen.Enabled = false;
			
			cbbNhaXe.BackColor = Color.WhiteSmoke;
			cbbLoaiXe.BackColor = Color.WhiteSmoke;
			cbbTuyen.BackColor = Color.WhiteSmoke;
			
			cbbNhaXe.TabStop = false;
			cbbLoaiXe.TabStop = false;
			cbbTuyen.TabStop = false;
		}

		public void SetMode(GiaVeDialogMode mode)
		{
			Mode = mode;
			switch (mode)
			{
				case GiaVeDialogMode.Add:
					Text = "Thêm giá vé";
					lblHeader.Text = "💰 THÊM GIÁ VÉ MỚI";
					btnPrimary.Text = "Thêm";
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
					txtMaGV.Enabled = false;
					SetReadOnly(false);
					break;
				case GiaVeDialogMode.Edit:
					Text = "Sửa giá vé";
					lblHeader.Text = "💰 CẬP NHẬT GIÁ VÉ";
					btnPrimary.Text = "Lưu";
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
					txtMaGV.Enabled = false;
					SetReadOnly(false);
					// Không cho phép sửa nhà xe, loại xe, tuyến vì đây là khóa chính
					cbbNhaXe.Enabled = false;
					cbbLoaiXe.Enabled = false;
					cbbTuyen.Enabled = false;
					break;
				case GiaVeDialogMode.View:
					Text = "Xem giá vé";
					lblHeader.Text = "💰 THÔNG TIN GIÁ VÉ";
					btnPrimary.Visible = false;
					btnCancel.Text = "Đóng";
					btnCancel.BackColor = Color.FromArgb(158, 158, 158);
					btnCancel.Location = new Point(460, 385);
					SetReadOnly(false);
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

			// Thay đổi màu nền khi readonly
			if (ro)
			{
				txtMaGV.BackColor = Color.WhiteSmoke;
				cbbNhaXe.BackColor = Color.WhiteSmoke;
				cbbLoaiXe.BackColor = Color.WhiteSmoke;
				cbbTuyen.BackColor = Color.WhiteSmoke;
				numGiaVe.BackColor = Color.WhiteSmoke;
			}
			else
			{
				txtMaGV.BackColor = Color.WhiteSmoke; // Luôn readonly
				cbbNhaXe.BackColor = Color.White;
				cbbLoaiXe.BackColor = Color.White;
				cbbTuyen.BackColor = Color.White;
				numGiaVe.BackColor = Color.White;
			}
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

			// Kiểm tra nhà xe
			if (SelectedMaNX == null)
			{
				MessageBox.Show(
					"Vui lòng chọn nhà xe.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				cbbNhaXe.Focus();
				return false;
			}

			// Kiểm tra loại xe
			if (SelectedMaLX == null)
			{
				MessageBox.Show(
					"Vui lòng chọn loại xe.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				cbbLoaiXe.Focus();
				return false;
			}

			// Kiểm tra tuyến đường
			if (SelectedMaTuyen == null)
			{
				MessageBox.Show(
					"Vui lòng chọn tuyến đường.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				cbbTuyen.Focus();
				return false;
			}

			// Kiểm tra giá vé phải là số dương
			if (numGiaVe.Value < 0)
			{
				MessageBox.Show(
					"Giá vé không thể là số âm.\n\nVui lòng nhập giá vé hợp lệ.",
					"Lỗi nhập liệu",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				numGiaVe.Value = 0;
				numGiaVe.Focus();
				return false;
			}

			// Kiểm tra giá vé phải lớn hơn 0
			if (numGiaVe.Value == 0)
			{
				MessageBox.Show(
					"Giá vé phải lớn hơn 0.\n\nVui lòng nhập giá vé hợp lệ.",
					"Lỗi nhập liệu",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				numGiaVe.Focus();
				return false;
			}

			// Kiểm tra giá vé trong khoảng hợp lý (10,000 - 100,000,000 VNĐ)
			if (numGiaVe.Value < 10000)
			{
				var result = MessageBox.Show(
					$"Giá vé quá thấp ({numGiaVe.Value:#,##0} VNĐ).\n\n" +
					"Giá vé thông thường từ 10,000 VNĐ trở lên.\n" +
					"Bạn có chắc chắn muốn tiếp tục?",
					"Xác nhận giá vé",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					numGiaVe.Focus();
					return false;
				}
			}

			if (numGiaVe.Value > 50000000)
			{
				var result = MessageBox.Show(
					$"Giá vé rất cao ({numGiaVe.Value:#,##0} VNĐ).\n\n" +
					"Bạn có chắc chắn giá vé này đúng?",
					"Xác nhận giá vé",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					numGiaVe.Focus();
					return false;
				}
			}

			return true;
		}
	}
}
