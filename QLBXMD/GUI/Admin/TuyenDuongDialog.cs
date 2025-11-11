using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public enum TuyenDuongDialogMode { Add, View, Edit }

	public partial class TuyenDuongDialog : Form
	{
		public TuyenDuongDialogMode Mode { get; private set; } = TuyenDuongDialogMode.Add;

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

		public decimal QuangDuongValue
		{
			get => numQuangDuong.Value;
			set => numQuangDuong.Value = value;
		}

		public int ThoiGianDuKienValue
		{
			get => (int)numThoiGian.Value;
			set => numThoiGian.Value = value;
		}

		public bool? TrangThaiValue
		{
			get
			{
				if (cbbTrangThai.SelectedIndex == 0) return true; // Hoạt động
				if (cbbTrangThai.SelectedIndex == 1) return false; // Ngừng hoạt động
				return null;
			}
			set
			{
				if (value == true) cbbTrangThai.SelectedIndex = 0;
				else if (value == false) cbbTrangThai.SelectedIndex = 1;
				else cbbTrangThai.SelectedIndex = -1;
			}
		}

		public TuyenDuongDialog()
		{
			InitializeComponent();
			cbbTrangThai.SelectedIndex = 0;
			SetupButtonEffects();
		}

		private void SetupButtonEffects()
		{
			// Hiệu ứng cho nút Primary (Lưu/Thêm)
			btnPrimary.MouseEnter += (s, e) => {
				if (Mode == TuyenDuongDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(53, 170, 255);
				else
					btnPrimary.BackColor = Color.FromArgb(96, 195, 100);
			};
			btnPrimary.MouseLeave += (s, e) => {
				if (Mode == TuyenDuongDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
				else
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
			};
			btnPrimary.MouseDown += (s, e) => {
				if (Mode == TuyenDuongDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(13, 130, 223);
				else
					btnPrimary.BackColor = Color.FromArgb(56, 155, 60);
			};
			btnPrimary.MouseUp += (s, e) => {
				if (Mode == TuyenDuongDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(53, 170, 255);
				else
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

		public void SetMode(TuyenDuongDialogMode mode)
		{
			Mode = mode;
			switch (mode)
			{
				case TuyenDuongDialogMode.Add:
					Text = "Thêm tuyến đường";
					lblHeader.Text = "🛣️ THÊM TUYẾN ĐƯỜNG MỚI";
					btnPrimary.Text = "Thêm";
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
					pnlHeader.BackColor = Color.FromArgb(76, 175, 80);
					lblMaTuyen.Visible = false;
					txtMaTuyen.Visible = false;
					SetReadOnly(false);
					break;
				case TuyenDuongDialogMode.Edit:
					Text = "Sửa tuyến đường";
					lblHeader.Text = "🛣️ CẬP NHẬT TUYẾN ĐƯỜNG";
					btnPrimary.Text = "Lưu";
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
					pnlHeader.BackColor = Color.FromArgb(33, 150, 243);
					lblMaTuyen.Visible = true;
					txtMaTuyen.Visible = true;
					txtMaTuyen.Enabled = false;
					SetReadOnly(false);
					break;
				case TuyenDuongDialogMode.View:
					Text = "Xem tuyến đường";
					lblHeader.Text = "🛣️ THÔNG TIN TUYẾN ĐƯỜNG";
					btnPrimary.Visible = false;
					btnCancel.Text = "Đóng";
					btnCancel.BackColor = Color.FromArgb(158, 158, 158);
					btnCancel.Location = new Point(460, 440);
					pnlHeader.BackColor = Color.FromArgb(158, 158, 158);
					lblMaTuyen.Visible = true;
					txtMaTuyen.Visible = true;
					SetReadOnly(true);
					break;
			}
		}

		private void SetReadOnly(bool ro)
		{
			txtMaTuyen.Enabled = !ro;
			txtDiemDi.Enabled = !ro;
			txtDiemDen.Enabled = !ro;
			numQuangDuong.Enabled = !ro;
			numThoiGian.Enabled = !ro;
			cbbTrangThai.Enabled = !ro;

			txtDiemDi.TabStop = !ro;
			txtDiemDen.TabStop = !ro;
			numQuangDuong.TabStop = !ro;
			numThoiGian.TabStop = !ro;
			cbbTrangThai.TabStop = !ro;

			// Thay đổi màu nền khi readonly
			if (ro)
			{
				txtMaTuyen.BackColor = Color.WhiteSmoke;
				txtDiemDi.BackColor = Color.WhiteSmoke;
				txtDiemDen.BackColor = Color.WhiteSmoke;
				numQuangDuong.BackColor = Color.WhiteSmoke;
				numThoiGian.BackColor = Color.WhiteSmoke;
				cbbTrangThai.BackColor = Color.WhiteSmoke;
			}
			else
			{
				txtMaTuyen.BackColor = Color.WhiteSmoke; // Luôn readonly
				txtDiemDi.BackColor = Color.White;
				txtDiemDen.BackColor = Color.White;
				numQuangDuong.BackColor = Color.White;
				numThoiGian.BackColor = Color.White;
				cbbTrangThai.BackColor = Color.White;
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
			if (Mode == TuyenDuongDialogMode.View)
				return true;

			// Kiểm tra điểm đi
			if (string.IsNullOrWhiteSpace(txtDiemDi.Text))
			{
				MessageBox.Show(
					"Vui lòng nhập điểm đi.\n\nĐiểm đi là thông tin bắt buộc.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtDiemDi.Focus();
				return false;
			}

			// Kiểm tra độ dài điểm đi
			if (txtDiemDi.Text.Trim().Length < 2)
			{
				MessageBox.Show(
					"Điểm đi quá ngắn.\n\nVui lòng nhập tên đầy đủ (ít nhất 2 ký tự).",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtDiemDi.Focus();
				return false;
			}

			// Kiểm tra điểm đến
			if (string.IsNullOrWhiteSpace(txtDiemDen.Text))
			{
				MessageBox.Show(
					"Vui lòng nhập điểm đến.\n\nĐiểm đến là thông tin bắt buộc.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtDiemDen.Focus();
				return false;
			}

			// Kiểm tra độ dài điểm đến
			if (txtDiemDen.Text.Trim().Length < 2)
			{
				MessageBox.Show(
					"Điểm đến quá ngắn.\n\nVui lòng nhập tên đầy đủ (ít nhất 2 ký tự).",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtDiemDen.Focus();
				return false;
			}

			// Kiểm tra điểm đi và điểm đến không được giống nhau
			if (txtDiemDi.Text.Trim().Equals(txtDiemDen.Text.Trim(), StringComparison.OrdinalIgnoreCase))
			{
				MessageBox.Show(
					"Điểm đi và điểm đến không được giống nhau.\n\n" +
					$"Điểm đi: {txtDiemDi.Text}\n" +
					$"Điểm đến: {txtDiemDen.Text}",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtDiemDen.Focus();
				return false;
			}

			// Kiểm tra quãng đường
			if (numQuangDuong.Value <= 0)
			{
				MessageBox.Show(
					"Quãng đường phải lớn hơn 0.\n\nVui lòng nhập quãng đường hợp lệ.",
					"Lỗi quãng đường",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				numQuangDuong.Focus();
				return false;
			}

			// Cảnh báo nếu quãng đường quá ngắn
			if (numQuangDuong.Value < 5)
			{
				var result = MessageBox.Show(
					$"Quãng đường rất ngắn ({numQuangDuong.Value} km).\n\n" +
					"Tuyến đường xe khách thông thường từ 5 km trở lên.\n" +
					"Bạn có chắc chắn muốn tiếp tục?",
					"Xác nhận quãng đường",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					numQuangDuong.Focus();
					return false;
				}
			}

			// Cảnh báo nếu quãng đường quá dài
			if (numQuangDuong.Value > 2000)
			{
				var result = MessageBox.Show(
					$"Quãng đường rất dài ({numQuangDuong.Value:#,##0.0} km).\n\n" +
					"Quãng đường thông thường dưới 2,000 km.\n" +
					"Bạn có chắc chắn quãng đường này đúng?",
					"Xác nhận quãng đường",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					numQuangDuong.Focus();
					return false;
				}
			}

			// Kiểm tra thời gian dự kiến
			if (numThoiGian.Value <= 0)
			{
				MessageBox.Show(
					"Thời gian dự kiến phải lớn hơn 0.\n\nVui lòng nhập thời gian hợp lệ.",
					"Lỗi thời gian",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				numThoiGian.Focus();
				return false;
			}

			// Cảnh báo nếu thời gian quá ngắn
			if (numThoiGian.Value < 10)
			{
				var result = MessageBox.Show(
					$"Thời gian dự kiến rất ngắn ({numThoiGian.Value} phút).\n\n" +
					"Chuyến xe thông thường ít nhất 10 phút.\n" +
					"Bạn có chắc chắn muốn tiếp tục?",
					"Xác nhận thời gian",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					numThoiGian.Focus();
					return false;
				}
			}

			// Kiểm tra tỷ lệ quãng đường/thời gian có hợp lý không
			// Tốc độ trung bình = quãng đường (km) / (thời gian (phút) / 60)
			decimal tocDoTrungBinh = numQuangDuong.Value / (numThoiGian.Value / 60m);
			
			if (tocDoTrungBinh < 20)
			{
				var result = MessageBox.Show(
					$"Tốc độ trung bình rất thấp ({tocDoTrungBinh:#,##0.0} km/h).\n\n" +
					$"Quãng đường: {numQuangDuong.Value:#,##0.0} km\n" +
					$"Thời gian: {numThoiGian.Value} phút ({(decimal)numThoiGian.Value / 60m:#,##0.00} giờ)\n" +
					$"Tốc độ: {tocDoTrungBinh:#,##0.0} km/h\n\n" +
					"Xe khách thông thường đi khoảng 40-80 km/h.\n" +
					"Bạn có muốn điều chỉnh thời gian?",
					"Cảnh báo tốc độ",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					numThoiGian.Focus();
					return false;
				}
			}

			if (tocDoTrungBinh > 120)
			{
				var result = MessageBox.Show(
					$"Tốc độ trung bình rất cao ({tocDoTrungBinh:#,##0.0} km/h).\n\n" +
					$"Quãng đường: {numQuangDuong.Value:#,##0.0} km\n" +
					$"Thời gian: {numThoiGian.Value} phút ({(decimal)numThoiGian.Value / 60m:#,##0.00} giờ)\n" +
					$"Tốc độ: {tocDoTrungBinh:#,##0.0} km/h\n\n" +
					"Xe khách thông thường đi khoảng 40-80 km/h.\n" +
					"Bạn có muốn điều chỉnh thời gian?",
					"Cảnh báo tốc độ",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					numThoiGian.Focus();
					return false;
				}
			}

			// Cảnh báo nếu thời gian quá dài
			if (numThoiGian.Value > 1440) // > 24 giờ
			{
				var result = MessageBox.Show(
					$"Thời gian dự kiến rất dài ({numThoiGian.Value} phút = {(decimal)numThoiGian.Value / 60m:#,##0.0} giờ).\n\n" +
					"Chuyến xe thông thường dưới 24 giờ.\n" +
					"Bạn có chắc chắn thời gian này đúng?",
					"Xác nhận thời gian",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					numThoiGian.Focus();
					return false;
				}
			}

			// Kiểm tra trạng thái
			if (TrangThaiValue == null)
			{
				MessageBox.Show(
					"Vui lòng chọn trạng thái.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				cbbTrangThai.Focus();
				return false;
			}

			return true;
		}
	}
}