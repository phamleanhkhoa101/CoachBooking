namespace QLBXMD.GUI.Admin
{
	partial class NhanVienDialog
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblMaNV = new System.Windows.Forms.Label();
			this.txtMaNV = new System.Windows.Forms.TextBox();
			this.lblTenNV = new System.Windows.Forms.Label();
			this.txtTenNV = new System.Windows.Forms.TextBox();
			this.lblNamSinh = new System.Windows.Forms.Label();
			this.dtpNamSinh = new System.Windows.Forms.DateTimePicker();
			this.lblDiaChi = new System.Windows.Forms.Label();
			this.txtDiaChi = new System.Windows.Forms.TextBox();
			this.lblSDT = new System.Windows.Forms.Label();
			this.txtSDT = new System.Windows.Forms.TextBox();
			this.lblGioiTinh = new System.Windows.Forms.Label();
			this.cbbGioiTinh = new System.Windows.Forms.ComboBox();
			this.lblTrangThai = new System.Windows.Forms.Label();
			this.cbbTrangThai = new System.Windows.Forms.ComboBox();
			this.lblTaiKhoan = new System.Windows.Forms.Label();
			this.cbbTaiKhoan = new System.Windows.Forms.ComboBox();
			this.btnGanTaiKhoan = new System.Windows.Forms.Button();
			this.btnPrimary = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblMaNV
			// 
			this.lblMaNV.AutoSize = true;
			this.lblMaNV.Location = new System.Drawing.Point(30, 30);
			this.lblMaNV.Name = "lblMaNV";
			this.lblMaNV.Size = new System.Drawing.Size(103, 20);
			this.lblMaNV.TabIndex = 0;
			this.lblMaNV.Text = "Mã nhân viên";
			// 
			// txtMaNV
			// 
			this.txtMaNV.Enabled = false;
			this.txtMaNV.Location = new System.Drawing.Point(200, 27);
			this.txtMaNV.Name = "txtMaNV";
			this.txtMaNV.Size = new System.Drawing.Size(300, 26);
			this.txtMaNV.TabIndex = 1;
			// 
			// lblTenNV
			// 
			this.lblTenNV.AutoSize = true;
			this.lblTenNV.Location = new System.Drawing.Point(30, 82);
			this.lblTenNV.Name = "lblTenNV";
			this.lblTenNV.Size = new System.Drawing.Size(118, 20);
			this.lblTenNV.TabIndex = 2;
			this.lblTenNV.Text = "Tên nhân viên *";
			// 
			// txtTenNV
			// 
			this.txtTenNV.Location = new System.Drawing.Point(200, 79);
			this.txtTenNV.Name = "txtTenNV";
			this.txtTenNV.Size = new System.Drawing.Size(300, 26);
			this.txtTenNV.TabIndex = 3;
			// 
			// lblNamSinh
			// 
			this.lblNamSinh.AutoSize = true;
			this.lblNamSinh.Location = new System.Drawing.Point(30, 131);
			this.lblNamSinh.Name = "lblNamSinh";
			this.lblNamSinh.Size = new System.Drawing.Size(85, 20);
			this.lblNamSinh.TabIndex = 4;
			this.lblNamSinh.Text = "N?m sinh *";
			// 
			// dtpNamSinh
			// 
			this.dtpNamSinh.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpNamSinh.Location = new System.Drawing.Point(200, 128);
			this.dtpNamSinh.Name = "dtpNamSinh";
			this.dtpNamSinh.Size = new System.Drawing.Size(300, 26);
			this.dtpNamSinh.TabIndex = 5;
			// 
			// lblDiaChi
			// 
			this.lblDiaChi.AutoSize = true;
			this.lblDiaChi.Location = new System.Drawing.Point(30, 181);
			this.lblDiaChi.Name = "lblDiaChi";
			this.lblDiaChi.Size = new System.Drawing.Size(76, 20);
			this.lblDiaChi.TabIndex = 6;
			this.lblDiaChi.Text = "??a ch? *";
			// 
			// txtDiaChi
			// 
			this.txtDiaChi.Location = new System.Drawing.Point(200, 178);
			this.txtDiaChi.Multiline = true;
			this.txtDiaChi.Name = "txtDiaChi";
			this.txtDiaChi.Size = new System.Drawing.Size(300, 60);
			this.txtDiaChi.TabIndex = 7;
			// 
			// lblSDT
			// 
			this.lblSDT.AutoSize = true;
			this.lblSDT.Location = new System.Drawing.Point(30, 263);
			this.lblSDT.Name = "lblSDT";
			this.lblSDT.Size = new System.Drawing.Size(112, 20);
			this.lblSDT.TabIndex = 8;
			this.lblSDT.Text = "S? ?i?n tho?i *";
			// 
			// txtSDT
			// 
			this.txtSDT.Location = new System.Drawing.Point(200, 260);
			this.txtSDT.Name = "txtSDT";
			this.txtSDT.Size = new System.Drawing.Size(300, 26);
			this.txtSDT.TabIndex = 9;
			// 
			// lblGioiTinh
			// 
			this.lblGioiTinh.AutoSize = true;
			this.lblGioiTinh.Location = new System.Drawing.Point(30, 317);
			this.lblGioiTinh.Name = "lblGioiTinh";
			this.lblGioiTinh.Size = new System.Drawing.Size(77, 20);
			this.lblGioiTinh.TabIndex = 10;
			this.lblGioiTinh.Text = "Gi?i tính *";
			// 
			// cbbGioiTinh
			// 
			this.cbbGioiTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbGioiTinh.FormattingEnabled = true;
			this.cbbGioiTinh.Location = new System.Drawing.Point(200, 314);
			this.cbbGioiTinh.Name = "cbbGioiTinh";
			this.cbbGioiTinh.Size = new System.Drawing.Size(300, 28);
			this.cbbGioiTinh.TabIndex = 11;
			// 
			// lblTrangThai
			// 
			this.lblTrangThai.AutoSize = true;
			this.lblTrangThai.Location = new System.Drawing.Point(30, 372);
			this.lblTrangThai.Name = "lblTrangThai";
			this.lblTrangThai.Size = new System.Drawing.Size(90, 20);
			this.lblTrangThai.TabIndex = 12;
			this.lblTrangThai.Text = "Tr?ng thái *";
			// 
			// cbbTrangThai
			// 
			this.cbbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTrangThai.FormattingEnabled = true;
			this.cbbTrangThai.Location = new System.Drawing.Point(200, 369);
			this.cbbTrangThai.Name = "cbbTrangThai";
			this.cbbTrangThai.Size = new System.Drawing.Size(300, 28);
			this.cbbTrangThai.TabIndex = 13;
			// 
			// lblTaiKhoan
			// 
			this.lblTaiKhoan.AutoSize = true;
			this.lblTaiKhoan.Location = new System.Drawing.Point(30, 430);
			this.lblTaiKhoan.Name = "lblTaiKhoan";
			this.lblTaiKhoan.Size = new System.Drawing.Size(78, 20);
			this.lblTaiKhoan.TabIndex = 14;
			this.lblTaiKhoan.Text = "Tài kho?n";
			// 
			// cbbTaiKhoan
			// 
			this.cbbTaiKhoan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTaiKhoan.FormattingEnabled = true;
			this.cbbTaiKhoan.Location = new System.Drawing.Point(200, 427);
			this.cbbTaiKhoan.Name = "cbbTaiKhoan";
			this.cbbTaiKhoan.Size = new System.Drawing.Size(270, 28);
			this.cbbTaiKhoan.TabIndex = 15;
			// 
			// btnGanTaiKhoan
			// 
			this.btnGanTaiKhoan.Location = new System.Drawing.Point(476, 427);
			this.btnGanTaiKhoan.Name = "btnGanTaiKhoan";
			this.btnGanTaiKhoan.Size = new System.Drawing.Size(24, 28);
			this.btnGanTaiKhoan.TabIndex = 16;
			this.btnGanTaiKhoan.Text = "...";
			this.btnGanTaiKhoan.UseVisualStyleBackColor = true;
			this.btnGanTaiKhoan.Click += new System.EventHandler(this.btnGanTaiKhoan_Click);
			// 
			// btnPrimary
			// 
			this.btnPrimary.Location = new System.Drawing.Point(270, 490);
			this.btnPrimary.Name = "btnPrimary";
			this.btnPrimary.Size = new System.Drawing.Size(100, 35);
			this.btnPrimary.TabIndex = 17;
			this.btnPrimary.Text = "Thêm";
			this.btnPrimary.UseVisualStyleBackColor = true;
			this.btnPrimary.Click += new System.EventHandler(this.btnPrimary_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(400, 490);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(100, 35);
			this.btnCancel.TabIndex = 18;
			this.btnCancel.Text = "H?y";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// NhanVienDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(601, 538);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnPrimary);
			this.Controls.Add(this.btnGanTaiKhoan);
			this.Controls.Add(this.cbbTaiKhoan);
			this.Controls.Add(this.lblTaiKhoan);
			this.Controls.Add(this.cbbTrangThai);
			this.Controls.Add(this.lblTrangThai);
			this.Controls.Add(this.cbbGioiTinh);
			this.Controls.Add(this.lblGioiTinh);
			this.Controls.Add(this.txtSDT);
			this.Controls.Add(this.lblSDT);
			this.Controls.Add(this.txtDiaChi);
			this.Controls.Add(this.lblDiaChi);
			this.Controls.Add(this.dtpNamSinh);
			this.Controls.Add(this.lblNamSinh);
			this.Controls.Add(this.txtTenNV);
			this.Controls.Add(this.lblTenNV);
			this.Controls.Add(this.txtMaNV);
			this.Controls.Add(this.lblMaNV);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NhanVienDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Nhân viên";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblMaNV;
		private System.Windows.Forms.TextBox txtMaNV;
		private System.Windows.Forms.Label lblTenNV;
		private System.Windows.Forms.TextBox txtTenNV;
		private System.Windows.Forms.Label lblNamSinh;
		private System.Windows.Forms.DateTimePicker dtpNamSinh;
		private System.Windows.Forms.Label lblDiaChi;
		private System.Windows.Forms.TextBox txtDiaChi;
		private System.Windows.Forms.Label lblSDT;
		private System.Windows.Forms.TextBox txtSDT;
		private System.Windows.Forms.Label lblGioiTinh;
		private System.Windows.Forms.ComboBox cbbGioiTinh;
		private System.Windows.Forms.Label lblTrangThai;
		private System.Windows.Forms.ComboBox cbbTrangThai;
		private System.Windows.Forms.Label lblTaiKhoan;
		private System.Windows.Forms.ComboBox cbbTaiKhoan;
		private System.Windows.Forms.Button btnGanTaiKhoan;
		private System.Windows.Forms.Button btnPrimary;
		private System.Windows.Forms.Button btnCancel;
	}
}
