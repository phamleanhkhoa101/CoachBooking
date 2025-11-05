using System.Windows.Forms;
using System.Drawing;

namespace QLBXMD.GUI.Admin
{
	partial class UCThongTin
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtMaNV = new System.Windows.Forms.TextBox();
			this.txtTenNV = new System.Windows.Forms.TextBox();
			this.dtpNamSinh = new System.Windows.Forms.DateTimePicker();
			this.txtDiaChi = new System.Windows.Forms.TextBox();
			this.txtSDT = new System.Windows.Forms.TextBox();
			this.cbbGioiTinh = new System.Windows.Forms.ComboBox();
			this.cbbTrangThai = new System.Windows.Forms.ComboBox();
			this.txtMaTK = new System.Windows.Forms.TextBox();
			this.txtTenDangNhap = new System.Windows.Forms.TextBox();
			this.txtEmail = new System.Windows.Forms.TextBox();
			this.txtQuyen = new System.Windows.Forms.TextBox();
			this.btnChinhSua = new System.Windows.Forms.Button();
			this.btnLuu = new System.Windows.Forms.Button();
			this.btnHuy = new System.Windows.Forms.Button();
			this.btnDoiMatKhau = new System.Windows.Forms.Button();
			this.btnDoiEmail = new System.Windows.Forms.Button();
			System.Windows.Forms.Label lblMaNV = new System.Windows.Forms.Label();
			System.Windows.Forms.Label lblTenNV = new System.Windows.Forms.Label();
			System.Windows.Forms.Label lblNamSinh = new System.Windows.Forms.Label();
			System.Windows.Forms.Label lblDiaChi = new System.Windows.Forms.Label();
			System.Windows.Forms.Label lblSDT = new System.Windows.Forms.Label();
			System.Windows.Forms.Label lblGioiTinh = new System.Windows.Forms.Label();
			System.Windows.Forms.Label lblTrangThai = new System.Windows.Forms.Label();
			System.Windows.Forms.Label lblMaTK = new System.Windows.Forms.Label();
			System.Windows.Forms.Label lblTenDangNhap = new System.Windows.Forms.Label();
			System.Windows.Forms.Label lblEmail = new System.Windows.Forms.Label();
			System.Windows.Forms.Label lblQuyen = new System.Windows.Forms.Label();
			System.Windows.Forms.Label lblThongTinNV = new System.Windows.Forms.Label();
			System.Windows.Forms.Label lblThongTinTK = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblThongTinNV
			// 
			lblThongTinNV.AutoSize = true;
			lblThongTinNV.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			lblThongTinNV.Location = new System.Drawing.Point(30, 20);
			lblThongTinNV.Name = "lblThongTinNV";
			lblThongTinNV.Size = new System.Drawing.Size(200, 29);
			lblThongTinNV.TabIndex = 15;
			lblThongTinNV.Text = "Thông tin nhân viên";
			// 
			// lblMaNV
			// 
			lblMaNV.AutoSize = true;
			lblMaNV.Location = new System.Drawing.Point(30, 73);
			lblMaNV.Name = "lblMaNV";
			lblMaNV.Size = new System.Drawing.Size(113, 20);
			lblMaNV.TabIndex = 16;
			lblMaNV.Text = "Mã nhân viên:";
			// 
			// txtMaNV
			// 
			this.txtMaNV.Location = new System.Drawing.Point(200, 70);
			this.txtMaNV.Name = "txtMaNV";
			this.txtMaNV.ReadOnly = true;
			this.txtMaNV.Size = new System.Drawing.Size(300, 26);
			this.txtMaNV.TabIndex = 0;
			// 
			// lblTenNV
			// 
			lblTenNV.AutoSize = true;
			lblTenNV.Location = new System.Drawing.Point(30, 123);
			lblTenNV.Name = "lblTenNV";
			lblTenNV.Size = new System.Drawing.Size(118, 20);
			lblTenNV.TabIndex = 17;
			lblTenNV.Text = "Tên nhân viên:";
			// 
			// txtTenNV
			// 
			this.txtTenNV.Location = new System.Drawing.Point(200, 120);
			this.txtTenNV.Name = "txtTenNV";
			this.txtTenNV.Size = new System.Drawing.Size(300, 26);
			this.txtTenNV.TabIndex = 1;
			// 
			// lblNamSinh
			// 
			lblNamSinh.AutoSize = true;
			lblNamSinh.Location = new System.Drawing.Point(30, 173);
			lblNamSinh.Name = "lblNamSinh";
			lblNamSinh.Size = new System.Drawing.Size(82, 20);
			lblNamSinh.TabIndex = 18;
			lblNamSinh.Text = "Năm sinh:";
			// 
			// dtpNamSinh
			// 
			this.dtpNamSinh.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpNamSinh.Location = new System.Drawing.Point(200, 170);
			this.dtpNamSinh.Name = "dtpNamSinh";
			this.dtpNamSinh.Size = new System.Drawing.Size(300, 26);
			this.dtpNamSinh.TabIndex = 2;
			// 
			// lblDiaChi
			// 
			lblDiaChi.AutoSize = true;
			lblDiaChi.Location = new System.Drawing.Point(30, 223);
			lblDiaChi.Name = "lblDiaChi";
			lblDiaChi.Size = new System.Drawing.Size(61, 20);
			lblDiaChi.TabIndex = 19;
			lblDiaChi.Text = "Địa chỉ:";
			// 
			// txtDiaChi
			// 
			this.txtDiaChi.Location = new System.Drawing.Point(200, 220);
			this.txtDiaChi.Name = "txtDiaChi";
			this.txtDiaChi.Size = new System.Drawing.Size(300, 26);
			this.txtDiaChi.TabIndex = 3;
			// 
			// lblSDT
			// 
			lblSDT.AutoSize = true;
			lblSDT.Location = new System.Drawing.Point(30, 273);
			lblSDT.Name = "lblSDT";
			lblSDT.Size = new System.Drawing.Size(106, 20);
			lblSDT.TabIndex = 20;
			lblSDT.Text = "Số điện thoại:";
			// 
			// txtSDT
			// 
			this.txtSDT.Location = new System.Drawing.Point(200, 270);
			this.txtSDT.Name = "txtSDT";
			this.txtSDT.Size = new System.Drawing.Size(300, 26);
			this.txtSDT.TabIndex = 4;
			// 
			// lblGioiTinh
			// 
			lblGioiTinh.AutoSize = true;
			lblGioiTinh.Location = new System.Drawing.Point(30, 323);
			lblGioiTinh.Name = "lblGioiTinh";
			lblGioiTinh.Size = new System.Drawing.Size(71, 20);
			lblGioiTinh.TabIndex = 21;
			lblGioiTinh.Text = "Giới tính:";
			// 
			// cbbGioiTinh
			// 
			this.cbbGioiTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbGioiTinh.FormattingEnabled = true;
			this.cbbGioiTinh.Items.AddRange(new object[] {
            "Nam",
            "Nữ"});
			this.cbbGioiTinh.Location = new System.Drawing.Point(200, 320);
			this.cbbGioiTinh.Name = "cbbGioiTinh";
			this.cbbGioiTinh.Size = new System.Drawing.Size(300, 28);
			this.cbbGioiTinh.TabIndex = 5;
			// 
			// lblTrangThai
			// 
			lblTrangThai.AutoSize = true;
			lblTrangThai.Location = new System.Drawing.Point(30, 373);
			lblTrangThai.Name = "lblTrangThai";
			lblTrangThai.Size = new System.Drawing.Size(84, 20);
			lblTrangThai.TabIndex = 22;
			lblTrangThai.Text = "Trạng thái:";
			// 
			// cbbTrangThai
			// 
			this.cbbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTrangThai.FormattingEnabled = true;
			this.cbbTrangThai.Items.AddRange(new object[] {
            "Đang làm việc",
            "Đã nghỉ việc"});
			this.cbbTrangThai.Location = new System.Drawing.Point(200, 370);
			this.cbbTrangThai.Name = "cbbTrangThai";
			this.cbbTrangThai.Size = new System.Drawing.Size(300, 28);
			this.cbbTrangThai.TabIndex = 6;
			// 
			// lblThongTinTK
			// 
			lblThongTinTK.AutoSize = true;
			lblThongTinTK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			lblThongTinTK.Location = new System.Drawing.Point(580, 20);
			lblThongTinTK.Name = "lblThongTinTK";
			lblThongTinTK.Size = new System.Drawing.Size(195, 29);
			lblThongTinTK.TabIndex = 23;
			lblThongTinTK.Text = "Thông tin tài khoản";
			// 
			// lblMaTK
			// 
			lblMaTK.AutoSize = true;
			lblMaTK.Location = new System.Drawing.Point(580, 73);
			lblMaTK.Name = "lblMaTK";
			lblMaTK.Size = new System.Drawing.Size(104, 20);
			lblMaTK.TabIndex = 24;
			lblMaTK.Text = "Mã tài khoản:";
			// 
			// txtMaTK
			// 
			this.txtMaTK.Location = new System.Drawing.Point(750, 70);
			this.txtMaTK.Name = "txtMaTK";
			this.txtMaTK.ReadOnly = true;
			this.txtMaTK.Size = new System.Drawing.Size(300, 26);
			this.txtMaTK.TabIndex = 7;
			// 
			// lblTenDangNhap
			// 
			lblTenDangNhap.AutoSize = true;
			lblTenDangNhap.Location = new System.Drawing.Point(580, 123);
			lblTenDangNhap.Name = "lblTenDangNhap";
			lblTenDangNhap.Size = new System.Drawing.Size(120, 20);
			lblTenDangNhap.TabIndex = 25;
			lblTenDangNhap.Text = "Tên đăng nhập:";
			// 
			// txtTenDangNhap
			// 
			this.txtTenDangNhap.Location = new System.Drawing.Point(750, 120);
			this.txtTenDangNhap.Name = "txtTenDangNhap";
			this.txtTenDangNhap.ReadOnly = true;
			this.txtTenDangNhap.Size = new System.Drawing.Size(300, 26);
			this.txtTenDangNhap.TabIndex = 8;
			// 
			// lblEmail
			// 
			lblEmail.AutoSize = true;
			lblEmail.Location = new System.Drawing.Point(580, 173);
			lblEmail.Name = "lblEmail";
			lblEmail.Size = new System.Drawing.Size(52, 20);
			lblEmail.TabIndex = 26;
			lblEmail.Text = "Email:";
			// 
			// txtEmail
			// 
			this.txtEmail.Location = new System.Drawing.Point(750, 170);
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.ReadOnly = true;
			this.txtEmail.Size = new System.Drawing.Size(300, 26);
			this.txtEmail.TabIndex = 9;
			// 
			// lblQuyen
			// 
			lblQuyen.AutoSize = true;
			lblQuyen.Location = new System.Drawing.Point(580, 223);
			lblQuyen.Name = "lblQuyen";
			lblQuyen.Size = new System.Drawing.Size(58, 20);
			lblQuyen.TabIndex = 27;
			lblQuyen.Text = "Quyền:";
			// 
			// txtQuyen
			// 
			this.txtQuyen.Location = new System.Drawing.Point(750, 220);
			this.txtQuyen.Name = "txtQuyen";
			this.txtQuyen.ReadOnly = true;
			this.txtQuyen.Size = new System.Drawing.Size(300, 26);
			this.txtQuyen.TabIndex = 10;
			// 
			// btnChinhSua
			// 
			this.btnChinhSua.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
			this.btnChinhSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnChinhSua.ForeColor = System.Drawing.Color.White;
			this.btnChinhSua.Location = new System.Drawing.Point(200, 440);
			this.btnChinhSua.Name = "btnChinhSua";
			this.btnChinhSua.Size = new System.Drawing.Size(120, 40);
			this.btnChinhSua.TabIndex = 11;
			this.btnChinhSua.Text = "Chỉnh sửa";
			this.btnChinhSua.UseVisualStyleBackColor = false;
			this.btnChinhSua.Click += new System.EventHandler(this.btnChinhSua_Click);
			// 
			// btnLuu
			// 
			this.btnLuu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
			this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLuu.ForeColor = System.Drawing.Color.White;
			this.btnLuu.Location = new System.Drawing.Point(200, 440);
			this.btnLuu.Name = "btnLuu";
			this.btnLuu.Size = new System.Drawing.Size(120, 40);
			this.btnLuu.TabIndex = 12;
			this.btnLuu.Text = "Lưu";
			this.btnLuu.UseVisualStyleBackColor = false;
			this.btnLuu.Visible = false;
			this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
			// 
			// btnHuy
			// 
			this.btnHuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
			this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnHuy.ForeColor = System.Drawing.Color.White;
			this.btnHuy.Location = new System.Drawing.Point(340, 440);
			this.btnHuy.Name = "btnHuy";
			this.btnHuy.Size = new System.Drawing.Size(120, 40);
			this.btnHuy.TabIndex = 13;
			this.btnHuy.Text = "Hủy";
			this.btnHuy.UseVisualStyleBackColor = false;
			this.btnHuy.Visible = false;
			this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
			// 
			// btnDoiMatKhau
			// 
			this.btnDoiMatKhau.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
			this.btnDoiMatKhau.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDoiMatKhau.ForeColor = System.Drawing.Color.Black;
			this.btnDoiMatKhau.Location = new System.Drawing.Point(750, 440);
			this.btnDoiMatKhau.Name = "btnDoiMatKhau";
			this.btnDoiMatKhau.Size = new System.Drawing.Size(150, 40);
			this.btnDoiMatKhau.TabIndex = 14;
			this.btnDoiMatKhau.Text = "Đổi mật khẩu";
			this.btnDoiMatKhau.UseVisualStyleBackColor = false;
			this.btnDoiMatKhau.Click += new System.EventHandler(this.btnDoiMatKhau_Click);
			// 
			// btnDoiEmail
			// 
			this.btnDoiEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
			this.btnDoiEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDoiEmail.ForeColor = System.Drawing.Color.White;
			this.btnDoiEmail.Location = new System.Drawing.Point(920, 440);
			this.btnDoiEmail.Name = "btnDoiEmail";
			this.btnDoiEmail.Size = new System.Drawing.Size(130, 40);
			this.btnDoiEmail.TabIndex = 15;
			this.btnDoiEmail.Text = "Đổi Email";
			this.btnDoiEmail.UseVisualStyleBackColor = false;
			this.btnDoiEmail.Click += new System.EventHandler(this.btnDoiEmail_Click);
			// 
			// UCThongTin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.btnDoiEmail);
			this.Controls.Add(lblThongTinTK);
			this.Controls.Add(lblQuyen);
			this.Controls.Add(lblEmail);
			this.Controls.Add(lblTenDangNhap);
			this.Controls.Add(lblMaTK);
			this.Controls.Add(lblThongTinNV);
			this.Controls.Add(lblTrangThai);
			this.Controls.Add(lblGioiTinh);
			this.Controls.Add(lblSDT);
			this.Controls.Add(lblDiaChi);
			this.Controls.Add(lblNamSinh);
			this.Controls.Add(lblTenNV);
			this.Controls.Add(lblMaNV);
			this.Controls.Add(this.btnDoiMatKhau);
			this.Controls.Add(this.btnHuy);
			this.Controls.Add(this.btnLuu);
			this.Controls.Add(this.btnChinhSua);
			this.Controls.Add(this.txtQuyen);
			this.Controls.Add(this.txtEmail);
			this.Controls.Add(this.txtTenDangNhap);
			this.Controls.Add(this.txtMaTK);
			this.Controls.Add(this.cbbTrangThai);
			this.Controls.Add(this.cbbGioiTinh);
			this.Controls.Add(this.txtSDT);
			this.Controls.Add(this.txtDiaChi);
			this.Controls.Add(this.dtpNamSinh);
			this.Controls.Add(this.txtTenNV);
			this.Controls.Add(this.txtMaNV);
			this.Name = "UCThongTin";
			this.Size = new System.Drawing.Size(1338, 730);
			this.Load += new System.EventHandler(this.UCThongTin_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private TextBox txtTenNV;
		private TextBox txtMaNV;
		private DateTimePicker dtpNamSinh;
		private TextBox txtDiaChi;
		private TextBox txtSDT;
		private ComboBox cbbGioiTinh;
		private ComboBox cbbTrangThai;
		private TextBox txtMaTK;
		private TextBox txtTenDangNhap;
		private TextBox txtEmail;
		private TextBox txtQuyen;
		private Button btnChinhSua;
		private Button btnLuu;
		private Button btnHuy;
		private Button btnDoiMatKhau;
		private Button btnDoiEmail;
	}
}
