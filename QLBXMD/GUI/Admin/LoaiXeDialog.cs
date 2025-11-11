using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public enum LoaiXeDialogMode { Add, View, Edit }

	public partial class LoaiXeDialog : Form
	{
		private Button btnCancel;
		private Button btnPrimary;
		private ComboBox cbbTrangThai;
		private Label label5;
		private Label label2;
		private TextBox txtTenLoaiXe;
		private Label label1;
		private Label label3;
		private NumericUpDown numSoCho;
		private TextBox txtMaLoaiXe;
		private Panel pnlHeader;
		private Label lblHeader;

		public LoaiXeDialogMode Mode { get; private set; } = LoaiXeDialogMode.Add;

		public string MaLoaiXe
		{
			get => txtMaLoaiXe.Text.Trim();
			set => txtMaLoaiXe.Text = value;
		}
		
		public string TenLoaiXe
		{
			get => txtTenLoaiXe.Text.Trim();
			set => txtTenLoaiXe.Text = value;
		}
		
		public int SoCho
		{
			get => (int)numSoCho.Value;
			set => numSoCho.Value = value;
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
		
		public LoaiXeDialog()
		{
			InitializeComponent();
			SetupButtonEffects();
		}

		private void SetupButtonEffects()
		{
			// Hiệu ứng cho nút Primary (Lưu/Thêm)
			btnPrimary.MouseEnter += (s, e) => {
				if (Mode == LoaiXeDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(53, 170, 255);
				else
					btnPrimary.BackColor = Color.FromArgb(96, 195, 100);
			};
			btnPrimary.MouseLeave += (s, e) => {
				if (Mode == LoaiXeDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
				else
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
			};
			btnPrimary.MouseDown += (s, e) => {
				if (Mode == LoaiXeDialogMode.Edit)
					btnPrimary.BackColor = Color.FromArgb(13, 130, 223);
				else
					btnPrimary.BackColor = Color.FromArgb(56, 155, 60);
			};
			btnPrimary.MouseUp += (s, e) => {
				if (Mode == LoaiXeDialogMode.Edit)
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

		public void SetMode(LoaiXeDialogMode mode)
		{
			Mode = mode;
			switch (mode)
			{
				case LoaiXeDialogMode.Add:
					Text = "Thêm loại xe";
					lblHeader.Text = "🚌 THÊM LOẠI XE MỚI";
					btnPrimary.Text = "Thêm";
					btnPrimary.BackColor = Color.FromArgb(76, 175, 80);
					pnlHeader.BackColor = Color.FromArgb(76, 175, 80);
					label1.Visible = false;
					txtMaLoaiXe.Visible = false;
					SetReadOnly(false);
					break;
				case LoaiXeDialogMode.Edit:
					Text = "Sửa loại xe";
					lblHeader.Text = "🚌 CẬP NHẬT LOẠI XE";
					btnPrimary.Text = "Lưu";
					btnPrimary.BackColor = Color.FromArgb(33, 150, 243);
					pnlHeader.BackColor = Color.FromArgb(33, 150, 243);
					label1.Visible = true;
					txtMaLoaiXe.Visible = true;
					txtMaLoaiXe.Enabled = false;
					SetReadOnly(false);
					break;
				case LoaiXeDialogMode.View:
					Text = "Xem loại xe";
					lblHeader.Text = "🚌 THÔNG TIN LOẠI XE";
					btnPrimary.Visible = false;
					btnCancel.Text = "Đóng";
					btnCancel.BackColor = Color.FromArgb(158, 158, 158);
					btnCancel.Location = new Point(460, 340);
					pnlHeader.BackColor = Color.FromArgb(158, 158, 158);
					label1.Visible = true;
					txtMaLoaiXe.Visible = true;
					SetReadOnly(true);
					break;
			}
		}

		private void SetReadOnly(bool ro)
		{
			txtMaLoaiXe.Enabled = !ro;
			txtTenLoaiXe.Enabled = !ro;
			numSoCho.Enabled = !ro;
			cbbTrangThai.Enabled = !ro;

			txtTenLoaiXe.TabStop = !ro;
			numSoCho.TabStop = !ro;
			cbbTrangThai.TabStop = !ro;

			// Thay đổi màu nền khi readonly
			if (ro)
			{
				txtMaLoaiXe.BackColor = Color.WhiteSmoke;
				txtTenLoaiXe.BackColor = Color.WhiteSmoke;
				numSoCho.BackColor = Color.WhiteSmoke;
				cbbTrangThai.BackColor = Color.WhiteSmoke;
			}
			else
			{
				txtMaLoaiXe.BackColor = Color.WhiteSmoke; // Luôn readonly
				txtTenLoaiXe.BackColor = Color.White;
				numSoCho.BackColor = Color.White;
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
			if (Mode == LoaiXeDialogMode.View)
				return true;

			// Kiểm tra tên loại xe
			if (string.IsNullOrWhiteSpace(txtTenLoaiXe.Text))
			{
				MessageBox.Show(
					"Vui lòng nhập tên loại xe.\n\nTên loại xe là thông tin bắt buộc.",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtTenLoaiXe.Focus();
				return false;
			}

			// Kiểm tra độ dài tên
			if (txtTenLoaiXe.Text.Trim().Length < 2)
			{
				MessageBox.Show(
					"Tên loại xe quá ngắn.\n\nVui lòng nhập tên đầy đủ (ít nhất 2 ký tự).",
					"Thông báo",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				txtTenLoaiXe.Focus();
				return false;
			}

			// Kiểm tra số chỗ
			if (numSoCho.Value <= 0)
			{
				MessageBox.Show(
					"Số chỗ phải lớn hơn 0.\n\nVui lòng nhập số chỗ hợp lệ.",
					"Lỗi số chỗ",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
				);
				numSoCho.Focus();
				return false;
			}

			// Cảnh báo nếu số chỗ quá ít
			if (numSoCho.Value < 4)
			{
				var result = MessageBox.Show(
					$"Số chỗ rất ít ({numSoCho.Value} chỗ).\n\n" +
					"Xe khách thông thường có từ 4 chỗ trở lên.\n" +
					"Bạn có chắc chắn muốn tiếp tục?",
					"Xác nhận số chỗ",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					numSoCho.Focus();
					return false;
				}
			}

			// Cảnh báo nếu số chỗ quá nhiều
			if (numSoCho.Value > 60)
			{
				var result = MessageBox.Show(
					$"Số chỗ rất lớn ({numSoCho.Value} chỗ).\n\n" +
					"Xe khách thông thường có từ 4-60 chỗ.\n" +
					"Bạn có chắc chắn số chỗ này đúng?",
					"Xác nhận số chỗ",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);
				
				if (result != DialogResult.Yes)
				{
					numSoCho.Focus();
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

		private void LoaiXeDialog_Load(object sender, EventArgs e)
		{
			// Khởi tạo combobox trạng thái nếu chưa có
			if (cbbTrangThai.Items.Count == 0)
			{
				cbbTrangThai.Items.Add("Hoạt động");
				cbbTrangThai.Items.Add("Ngừng hoạt động");
				cbbTrangThai.SelectedIndex = 0;
			}
		}

		private void InitializeComponent()
		{
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnPrimary = new System.Windows.Forms.Button();
			this.cbbTrangThai = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTenLoaiXe = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtMaLoaiXe = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.numSoCho = new System.Windows.Forms.NumericUpDown();
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.lblHeader = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numSoCho)).BeginInit();
			this.pnlHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
			this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnCancel.FlatAppearance.BorderSize = 0;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnCancel.ForeColor = System.Drawing.Color.White;
			this.btnCancel.Location = new System.Drawing.Point(469, 340);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(100, 45);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Hủy";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnPrimary
			// 
			this.btnPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
			this.btnPrimary.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnPrimary.FlatAppearance.BorderSize = 0;
			this.btnPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPrimary.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnPrimary.ForeColor = System.Drawing.Color.White;
			this.btnPrimary.Location = new System.Drawing.Point(350, 340);
			this.btnPrimary.Name = "btnPrimary";
			this.btnPrimary.Size = new System.Drawing.Size(100, 45);
			this.btnPrimary.TabIndex = 4;
			this.btnPrimary.Text = "Thêm";
			this.btnPrimary.UseVisualStyleBackColor = false;
			this.btnPrimary.Click += new System.EventHandler(this.btnPrimary_Click);
			// 
			// cbbTrangThai
			// 
			this.cbbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbTrangThai.FormattingEnabled = true;
			this.cbbTrangThai.Items.AddRange(new object[] {
            "Hoạt động",
            "Ngừng hoạt động"});
			this.cbbTrangThai.Location = new System.Drawing.Point(220, 262);
			this.cbbTrangThai.Name = "cbbTrangThai";
			this.cbbTrangThai.Size = new System.Drawing.Size(250, 36);
			this.cbbTrangThai.TabIndex = 3;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label5.Location = new System.Drawing.Point(60, 265);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(102, 28);
			this.label5.TabIndex = 19;
			this.label5.Text = "Trạng thái:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label2.Location = new System.Drawing.Point(60, 155);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(106, 28);
			this.label2.TabIndex = 14;
			this.label2.Text = "Tên loại xe:";
			// 
			// txtTenLoaiXe
			// 
			this.txtTenLoaiXe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtTenLoaiXe.Location = new System.Drawing.Point(220, 152);
			this.txtTenLoaiXe.Name = "txtTenLoaiXe";
			this.txtTenLoaiXe.Size = new System.Drawing.Size(340, 34);
			this.txtTenLoaiXe.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label1.Location = new System.Drawing.Point(60, 100);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(105, 28);
			this.label1.TabIndex = 12;
			this.label1.Text = "Mã loại xe:";
			// 
			// txtMaLoaiXe
			// 
			this.txtMaLoaiXe.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtMaLoaiXe.Enabled = false;
			this.txtMaLoaiXe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtMaLoaiXe.Location = new System.Drawing.Point(220, 97);
			this.txtMaLoaiXe.Name = "txtMaLoaiXe";
			this.txtMaLoaiXe.ReadOnly = true;
			this.txtMaLoaiXe.Size = new System.Drawing.Size(340, 34);
			this.txtMaLoaiXe.TabIndex = 0;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label3.Location = new System.Drawing.Point(60, 210);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(76, 28);
			this.label3.TabIndex = 24;
			this.label3.Text = "Số chỗ:";
			// 
			// numSoCho
			// 
			this.numSoCho.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.numSoCho.Location = new System.Drawing.Point(220, 207);
			this.numSoCho.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numSoCho.Name = "numSoCho";
			this.numSoCho.Size = new System.Drawing.Size(150, 34);
			this.numSoCho.TabIndex = 2;
			this.numSoCho.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
			// 
			// pnlHeader
			// 
			this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.pnlHeader.Controls.Add(this.lblHeader);
			this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlHeader.Location = new System.Drawing.Point(0, 0);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(630, 70);
			this.pnlHeader.TabIndex = 25;
			// 
			// lblHeader
			// 
			this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.lblHeader.ForeColor = System.Drawing.Color.White;
			this.lblHeader.Location = new System.Drawing.Point(0, 0);
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.Size = new System.Drawing.Size(630, 70);
			this.lblHeader.TabIndex = 0;
			this.lblHeader.Text = "THÔNG TIN LOẠI XE";
			this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LoaiXeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(630, 415);
			this.Controls.Add(this.pnlHeader);
			this.Controls.Add(this.numSoCho);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnPrimary);
			this.Controls.Add(this.cbbTrangThai);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtTenLoaiXe);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtMaLoaiXe);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoaiXeDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Quản lý loại xe";
			this.Load += new System.EventHandler(this.LoaiXeDialog_Load);
			((System.ComponentModel.ISupportInitialize)(this.numSoCho)).EndInit();
			this.pnlHeader.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}