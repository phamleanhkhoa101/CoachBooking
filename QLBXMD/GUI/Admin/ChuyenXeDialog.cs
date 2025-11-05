using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	public enum ChuyenXeDialogMode { Add, View, Edit, Delete }

	public class ChuyenXeDialog : Form
	{
		private TextBox txtMaChuyen;
		private ComboBox cbbXe, cbbTuyen, cbbTaiXe;
		private DateTimePicker dtpKhoiHanh, dtpDenDuKien;
		private NumericUpDown numGiaVe;
		private Button btnPrimary, btnCancel;
		private Label lblGiaVeInfo; // Label để hiển thị thông báo giá vé

		public ChuyenXeDialogMode Mode { get; private set; } = ChuyenXeDialogMode.Add;

		public string MaChuyen { get => txtMaChuyen.Text; set => txtMaChuyen.Text = value; }
		public DateTime KhoiHanhValue { get => dtpKhoiHanh.Value; set => dtpKhoiHanh.Value = value; }
		public DateTime? DenDuKienValue
		{
			get => dtpDenDuKien.Checked ? dtpDenDuKien.Value : (DateTime?)null;
			set
			{
				if (value.HasValue)
				{
					dtpDenDuKien.Checked = true;
					dtpDenDuKien.Value = value.Value;
				}
				else
				{
					dtpDenDuKien.Checked = false;
				}
			}
		}
		public decimal? GiaVeValue { get => numGiaVe.Value > 0 ? numGiaVe.Value : (decimal?)null; set => numGiaVe.Value = value ?? 0; }

		public int? SelectedMaXe { get { return cbbXe.SelectedValue is int v ? (int?)v : (cbbXe.SelectedValue as int?); } set { if (value.HasValue) cbbXe.SelectedValue = value.Value; else cbbXe.SelectedIndex = -1; } }
		public int? SelectedMaTuyen { get { return cbbTuyen.SelectedValue is int v ? (int?)v : (cbbTuyen.SelectedValue as int?); } set { if (value.HasValue) cbbTuyen.SelectedValue = value.Value; else cbbTuyen.SelectedIndex = -1; } }
		public int? SelectedMaTX { get { return cbbTaiXe.SelectedValue is int v ? (int?)v : (cbbTaiXe.SelectedValue as int?); } set { if (value.HasValue) cbbTaiXe.SelectedValue = value.Value; else cbbTaiXe.SelectedIndex = -1; } }

		public ChuyenXeDialog()
		{
			InitializeComponent();
			
			// Thêm sự kiện khi chọn xe hoặc tuyến
			cbbXe.SelectedIndexChanged += CbbXeOrTuyen_SelectedIndexChanged;
			cbbTuyen.SelectedIndexChanged += CbbXeOrTuyen_SelectedIndexChanged;
		}

		private void CbbXeOrTuyen_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Chỉ tự động lấy giá vé khi đang ở chế độ Add hoặc Edit
			if (Mode != ChuyenXeDialogMode.Add && Mode != ChuyenXeDialogMode.Edit)
				return;

			AutoLoadGiaVe();
		}

		private void AutoLoadGiaVe()
		{
			if (SelectedMaXe == null || SelectedMaTuyen == null)
			{
				lblGiaVeInfo.Text = "";
				return;
			}

			try
			{
				using (var db = new BXMDEntities())
				{
					// Lấy thông tin xe để biết MaNX và MaLX
					var xe = db.Xes.AsNoTracking()
						.FirstOrDefault(x => x.MaXe == SelectedMaXe.Value);
					
					if (xe == null || xe.MaNX == null)
					{
						lblGiaVeInfo.Text = "⚠ Không tìm thấy thông tin xe";
						lblGiaVeInfo.ForeColor = Color.Red;
						return;
					}

					// Tìm giá vé trong bảng GiaVe
					var giaVe = db.GiaVes.AsNoTracking()
						.FirstOrDefault(gv => 
							gv.MaNX == xe.MaNX && 
							gv.MaLX == xe.MaLX && 
							gv.MaTuyen == SelectedMaTuyen.Value);

					if (giaVe != null)
					{
						numGiaVe.Value = giaVe.GiaVe1;
						lblGiaVeInfo.Text = "✓ Giá vé được lấy từ bảng giá";
						lblGiaVeInfo.ForeColor = Color.Green;
					}
					else
					{
						lblGiaVeInfo.Text = "⚠ Chưa có giá vé cho tuyến này";
						lblGiaVeInfo.ForeColor = Color.Orange;
					}
				}
			}
			catch (Exception ex)
			{
				lblGiaVeInfo.Text = "✗ Lỗi: " + ex.Message;
				lblGiaVeInfo.ForeColor = Color.Red;
			}
		}

		public void BindXe(IEnumerable<KeyValuePair<int, string>> items)
		{
			cbbXe.DataSource = items?.ToList();
			cbbXe.DisplayMember = "Value";
			cbbXe.ValueMember = "Key";
		}

		public void BindTuyen(IEnumerable<KeyValuePair<int, string>> items)
		{
			cbbTuyen.DataSource = items?.ToList();
			cbbTuyen.DisplayMember = "Value";
			cbbTuyen.ValueMember = "Key";
		}

		public void BindTaiXe(IEnumerable<KeyValuePair<int, string>> items)
		{
			cbbTaiXe.DataSource = items?.ToList();
			cbbTaiXe.DisplayMember = "Value";
			cbbTaiXe.ValueMember = "Key";
		}

		public void SetMode(ChuyenXeDialogMode mode)
		{
			Mode = mode;
			switch (mode)
			{
				case ChuyenXeDialogMode.Add:
					Text = "Thêm chuyến xe";
					btnPrimary.Text = "Thêm";
					txtMaChuyen.Enabled = false;
					SetReadOnly(false);
					lblGiaVeInfo.Visible = true;
					break;
				case ChuyenXeDialogMode.Edit:
					Text = "Sửa chuyến xe";
					btnPrimary.Text = "Lưu";
					txtMaChuyen.Enabled = false;
					SetReadOnly(false);
					lblGiaVeInfo.Visible = true;
					break;
				case ChuyenXeDialogMode.View:
					Text = "Xem chuyến xe";
					btnPrimary.Visible = false;
					btnCancel.Text = "Đóng";
					SetReadOnly(true);
					lblGiaVeInfo.Visible = false;
					break;
				case ChuyenXeDialogMode.Delete:
					Text = "Xóa chuyến xe";
					btnPrimary.Text = "Xóa";
					txtMaChuyen.Enabled = false;
					SetReadOnly(true);
					btnPrimary.Visible = true;
					lblGiaVeInfo.Visible = false;
					break;
			}
		}

		private void SetReadOnly(bool ro)
		{
			// Disable/enable all inputs consistently for view/edit modes
			txtMaChuyen.Enabled = !ro;
			cbbXe.Enabled = !ro;
			cbbTuyen.Enabled = !ro;
			cbbTaiXe.Enabled = !ro;
			dtpKhoiHanh.Enabled = !ro;
			dtpDenDuKien.Enabled = !ro;
			numGiaVe.Enabled = !ro;

			// Also remove from tab navigation when read-only
			txtMaChuyen.TabStop = !ro;
			cbbXe.TabStop = !ro;
			cbbTuyen.TabStop = !ro;
			cbbTaiXe.TabStop = !ro;
			dtpKhoiHanh.TabStop = !ro;
			dtpDenDuKien.TabStop = !ro;
			numGiaVe.TabStop = !ro;
		}

		private void InitializeComponent()
		{
			this.Text = "Chuyến xe";
			this.StartPosition = FormStartPosition.CenterParent;
			this.Size = new Size(560, 420); // Tăng chiều cao để chứa label mới
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;

			var lblMaChuyen = new Label { Text = "Mã chuyến", Left = 20, Top = 20, AutoSize = true };
			txtMaChuyen = new TextBox { Left = 160, Top = 16, Width = 360, ReadOnly = true };
			
			var lblXe = new Label { Text = "Xe", Left = 20, Top = 55, AutoSize = true };
			cbbXe = new ComboBox { Left = 160, Top = 51, Width = 360, DropDownStyle = ComboBoxStyle.DropDownList };
			
			var lblTuyen = new Label { Text = "Tuyến", Left = 20, Top = 90, AutoSize = true };
			cbbTuyen = new ComboBox { Left = 160, Top = 86, Width = 360, DropDownStyle = ComboBoxStyle.DropDownList };
			
			var lblTaiXe = new Label { Text = "Tài xế", Left = 20, Top = 125, AutoSize = true };
			cbbTaiXe = new ComboBox { Left = 160, Top = 121, Width = 360, DropDownStyle = ComboBoxStyle.DropDownList };
			
			var lblKhoiHanh = new Label { Text = "Khởi hành", Left = 20, Top = 160, AutoSize = true };
			dtpKhoiHanh = new DateTimePicker
			{
				Left = 160,
				Top = 156,
				Width = 220,
				Format = DateTimePickerFormat.Custom,
				CustomFormat = "dd/MM/yyyy HH:mm",
				ShowUpDown = false
			};
			
			var lblDenDuKien = new Label { Text = "Đến dự kiến", Left = 20, Top = 195, AutoSize = true };
			dtpDenDuKien = new DateTimePicker
			{
				Left = 160,
				Top = 191,
				Width = 220,
				Format = DateTimePickerFormat.Custom,
				CustomFormat = "dd/MM/yyyy HH:mm",
				ShowCheckBox = true,
				ShowUpDown = false
			};
			
			var lblGiaVe = new Label { Text = "Giá vé (₫)", Left = 20, Top = 230, AutoSize = true };
			numGiaVe = new NumericUpDown
			{
				Left = 160,
				Top = 226,
				Width = 220,
				Maximum = 1000000000,
				Minimum = 0,
				Increment = 1000,
				ThousandsSeparator = true
			};

			// Label thông báo giá vé
			lblGiaVeInfo = new Label
			{
				Left = 160,
				Top = 254,
				Width = 360,
				Height = 20,
				ForeColor = Color.Green,
				Text = "",
				AutoSize = false
			};

			btnPrimary = new Button { Left = 330, Top = 320, Width = 90, Text = "Lưu" };
			btnCancel = new Button { Left = 430, Top = 320, Width = 90, Text = "Hủy" };

			btnPrimary.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };
			btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

			Controls.AddRange(new Control[]
			{
				lblMaChuyen, txtMaChuyen,
				lblXe, cbbXe,
				lblTuyen, cbbTuyen,
				lblTaiXe, cbbTaiXe,
				lblKhoiHanh, dtpKhoiHanh,
				lblDenDuKien, dtpDenDuKien,
				lblGiaVe, numGiaVe,
				lblGiaVeInfo, // Thêm label thông báo
				btnPrimary, btnCancel
			});
		}
	}
}
