using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public enum HoaDonDialogMode { Add, View, Edit }

	public partial class HoaDonDialog : Form
	{
		public HoaDonDialogMode Mode { get; private set; } = HoaDonDialogMode.Add;

		public string MaHD { get => txtMaHD.Text; set => txtMaHD.Text = value; }
		public DateTime NgayLapValue { get => dtpNgayLap.Value; set => dtpNgayLap.Value = value; }
		public decimal TongTienValue { get => numTongTien.Value; set => numTongTien.Value = value; }

		public int? SelectedMaKH 
		{ 
			get { return cbbKhachHang.SelectedValue is int v ? (int?)v : (cbbKhachHang.SelectedValue as int?); } 
			set { if (value.HasValue) cbbKhachHang.SelectedValue = value.Value; else cbbKhachHang.SelectedIndex = -1; } 
		}
		
		public int? SelectedMANV 
		{ 
			get { return cbbNhanVien.SelectedValue is int v ? (int?)v : (cbbNhanVien.SelectedValue as int?); } 
			set { if (value.HasValue) cbbNhanVien.SelectedValue = value.Value; else cbbNhanVien.SelectedIndex = -1; } 
		}

		public bool? TrangThaiValue
		{
			get
			{
				if (cbbTrangThai.SelectedIndex == 0) return true; // Đã thanh toán
				if (cbbTrangThai.SelectedIndex == 1) return false; // Chưa thanh toán
				return null;
			}
			set
			{
				if (value == true) cbbTrangThai.SelectedIndex = 0;
				else if (value == false) cbbTrangThai.SelectedIndex = 1;
				else cbbTrangThai.SelectedIndex = -1;
			}
		}

		public HoaDonDialog()
		{
			InitializeComponent();
			cbbTrangThai.SelectedIndex = 1; // Mặc định chưa thanh toán
			SetupButtonEffects();
			SetupNumericUpDownValidation();
		}

		private void SetupButtonEffects()
		{
			// Hiệu ứng cho nút Primary (Lưu/Thêm)
			btnPrimary.MouseEnter += (s, e) => {
				if (Mode == HoaDonDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(53, 170, 255);
				else
					btnPrimary.BackColor = Color.FromArgb(96, 195, 100);
			};
			btnPrimary.MouseLeave += (s, e) => {
				if (Mode == HoaDonDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
				else
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
			};
			btnPrimary.MouseDown += (s, e) => {
				if (Mode == HoaDonDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(13, 130, 223);
				else
					btnPrimary.BackColor = Color.FromArgb(56, 155, 60);
			};
			btnPrimary.MouseUp += (s, e) => {
				if (Mode == HoaDonDialogMode.Edit)
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

		private void SetupNumericUpDownValidation()
		{
			// Đảm bảo tổng tiền không thể là số âm
			numTongTien.Minimum = 0;
			numTongTien.Maximum = 1000000000; // 1 tỷ VNĐ
			numTongTien.DecimalPlaces = 0;
			numTongTien.Increment = 10000; // Tăng mỗi lần 10,000 VNĐ

			// Xử lý sự kiện khi giá trị thay đổi
			numTongTien.ValueChanged += (s, e) => {
				// Đảm bảo giá trị không âm
				if (numTongTien.Value < 0)
				{
					numTongTien.Value = 0;
				}
			};

			// Xử lý khi người dùng nhập thủ công
			numTongTien.KeyPress += (s, e) => {
				// Chỉ cho phép số và các phím điều khiển
				if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				{
					e.Handled = true;
				}
			};

			// Xử lý khi mất focus
			numTongTien.Leave += (s, e) => {
				// Nếu giá trị nhỏ hơn 0, reset về 0
				if (numTongTien.Value < 0)
				{
					numTongTien.Value = 0;
					MessageBox.Show(
						"Tổng tiền không thể là số âm. Giá trị đã được đặt lại về 0.",
						"Thông báo",
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning
					);
				}
			};
		}

		public void BindKhachHang(IEnumerable<KeyValuePair<int, string>> items)
		{
			var list = items?.ToList() ?? new List<KeyValuePair<int, string>>();
			list.Insert(0, new KeyValuePair<int, string>(0, "-- Không chọn --"));
			cbbKhachHang.DataSource = list;
			cbbKhachHang.DisplayMember = "Value";
			cbbKhachHang.ValueMember = "Key";
		}

		public void BindNhanVien(IEnumerable<KeyValuePair<int, string>> items)
		{
			var list = items?.ToList() ?? new List<KeyValuePair<int, string>>();
			list.Insert(0, new KeyValuePair<int, string>(0, "-- Không chọn --"));
			cbbNhanVien.DataSource = list;
			cbbNhanVien.DisplayMember = "Value";
			cbbNhanVien.ValueMember = "Key";
		}

		public void SetMode(HoaDonDialogMode mode)
		{
			Mode = mode;
			switch (mode)
			{
				case HoaDonDialogMode.Add:
					Text = "Thêm hóa đơn";
					lblHeader.Text = "📄 THÊM HÓA ĐƠN MỚI";
					btnPrimary.Text = "Thêm";
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
					pnlHeader.BackColor = Color.FromArgb(76, 175, 80);
					txtMaHD.Enabled = false;
					SetReadOnly(false);
					break;
				case HoaDonDialogMode.Edit:
					Text = "Sửa hóa đơn";
					lblHeader.Text = "📄 CẬP NHẬT HÓA ĐƠN";
					btnPrimary.Text = "Lưu";
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
					pnlHeader.BackColor = Color.FromArgb(33, 150, 243);
					txtMaHD.Enabled = false;
					SetReadOnly(false);
					break;
				case HoaDonDialogMode.View:
					Text = "Xem hóa đơn";
					lblHeader.Text = "📄 THÔNG TIN HÓA ĐƠN";
					btnPrimary.Visible = false;
					btnCancel.Text = "Đóng";
					btnCancel.BackColor = Color.FromArgb(158, 158, 158);
					btnCancel.Location = new Point(460, 440);
					pnlHeader.BackColor = Color.FromArgb(158, 158, 158);
					SetReadOnly(true);
					break;
			}
		}

		private void SetReadOnly(bool ro)
		{
			txtMaHD.Enabled = !ro;
			cbbKhachHang.Enabled = !ro;
			cbbNhanVien.Enabled = !ro;
			dtpNgayLap.Enabled = !ro;
			numTongTien.Enabled = !ro;
			cbbTrangThai.Enabled = !ro;

			cbbKhachHang.TabStop = !ro;
			cbbNhanVien.TabStop = !ro;
			dtpNgayLap.TabStop = !ro;
			numTongTien.TabStop = !ro;
			cbbTrangThai.TabStop = !ro;

			// Thay đổi màu nền khi readonly
			if (ro)
			{
				txtMaHD.BackColor = Color.WhiteSmoke;
				cbbKhachHang.BackColor = Color.WhiteSmoke;
				cbbNhanVien.BackColor = Color.WhiteSmoke;
				dtpNgayLap.BackColor = Color.WhiteSmoke;
				numTongTien.BackColor = Color.WhiteSmoke;
				cbbTrangThai.BackColor = Color.WhiteSmoke;
			}
			else
			{
				txtMaHD.BackColor = Color.WhiteSmoke; // Luôn readonly
				cbbKhachHang.BackColor = Color.White;
				cbbNhanVien.BackColor = Color.White;
				dtpNgayLap.BackColor = Color.White;
				numTongTien.BackColor = Color.White;
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
			if (Mode == HoaDonDialogMode.View)
				return true;

			// Phải chọn ít nhất khách hàng hoặc nhân viên
			int? maKH = SelectedMaKH;
			int? maNV = SelectedMANV;
			
			if ((maKH == null || maKH == 0) && (maNV == null || maNV == 0))
			{
				MessageBox.Show(
					"Vui lòng chọn ít nhất khách hàng hoặc nhân viên.\n\n" +
					"Hóa đơn phải liên kết với ít nhất một khách hàng hoặc nhân viên.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				cbbKhachHang.Focus();
				return false;
			}

			// Kiểm tra ngày lập
			if (dtpNgayLap.Value > DateTime.Now)
			{
				MessageBox.Show(
					"Ngày lập không được lớn hơn ngày hiện tại.\n\n" +
					$"Ngày hiện tại: {DateTime.Now:dd/MM/yyyy HH:mm}\n" +
					$"Ngày đã chọn: {dtpNgayLap.Value:dd/MM/yyyy HH:mm}",
					"Lỗi ngày lập",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				dtpNgayLap.Focus();
				return false;
			}

			// Kiểm tra ngày lập không quá xa trong quá khứ (hơn 1 năm)
			if (dtpNgayLap.Value < DateTime.Now.AddYears(-1))
			{
				var result = MessageBox.Show(
					$"Ngày lập đã chọn là {DateTime.Now.Subtract(dtpNgayLap.Value).Days} ngày trước.\n\n" +
					"Bạn có chắc chắn muốn tiếp tục?",
					"Xác nhận ngày lập",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					dtpNgayLap.Focus();
					return false;
				}
			}

			// Kiểm tra tổng tiền không thể là số âm
			if (numTongTien.Value < 0)
			{
				MessageBox.Show(
					"Tổng tiền không thể là số âm.\n\nVui lòng nhập tổng tiền hợp lệ.",
					"Lỗi nhập liệu",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
				numTongTien.Value = 0;
				numTongTien.Focus();
				return false;
			}

			// Kiểm tra tổng tiền phải lớn hơn 0
			if (numTongTien.Value == 0)
			{
				MessageBox.Show(
					"Tổng tiền phải lớn hơn 0.\n\nVui lòng nhập tổng tiền hợp lệ.",
					"Lỗi nhập liệu",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				numTongTien.Focus();
				return false;
			}

			// Kiểm tra tổng tiền trong khoảng hợp lý
			if (numTongTien.Value < 10000)
			{
				var result = MessageBox.Show(
					$"Tổng tiền quá thấp ({numTongTien.Value:#,##0} VNĐ).\n\n" +
					"Tổng tiền thông thường từ 10,000 VNĐ trở lên.\n" +
					"Bạn có chắc chắn muốn tiếp tục?",
					"Xác nhận tổng tiền",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					numTongTien.Focus();
					return false;
				}
			}

			if (numTongTien.Value > 100000000)
			{
				var result = MessageBox.Show(
					$"Tổng tiền rất cao ({numTongTien.Value:#,##0} VNĐ).\n\n" +
					"Bạn có chắc chắn tổng tiền này đúng?",
					"Xác nhận tổng tiền",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					numTongTien.Focus();
					return false;
				}
			}

			// Kiểm tra trạng thái thanh toán
			if (TrangThaiValue == null)
			{
				MessageBox.Show(
					"Vui lòng chọn trạng thái thanh toán.",
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
