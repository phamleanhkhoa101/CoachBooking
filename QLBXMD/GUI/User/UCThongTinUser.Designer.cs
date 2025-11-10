namespace QLBXMD.GUI.User
{
	partial class UCThongTinUser
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
			this.flowMain = new System.Windows.Forms.FlowLayoutPanel();
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.lblTitle = new System.Windows.Forms.Label();
			this.pnlThongTin = new System.Windows.Forms.Panel();
			this.btnDoiMatKhau = new System.Windows.Forms.Button();
			this.btnCapNhat = new System.Windows.Forms.Button();
			this.cboGioiTinh = new System.Windows.Forms.ComboBox();
			this.dtpNamSinh = new System.Windows.Forms.DateTimePicker();
			this.txtEmail = new System.Windows.Forms.TextBox();
			this.txtSDT = new System.Windows.Forms.TextBox();
			this.txtTenKH = new System.Windows.Forms.TextBox();
			this.txtTenDangNhap = new System.Windows.Forms.TextBox();
			this.lblGioiTinh = new System.Windows.Forms.Label();
			this.lblNamSinh = new System.Windows.Forms.Label();
			this.lblEmail = new System.Windows.Forms.Label();
			this.lblSDT = new System.Windows.Forms.Label();
			this.lblTenKH = new System.Windows.Forms.Label();
			this.lblTenDangNhap = new System.Windows.Forms.Label();
			this.lblThongTinTitle = new System.Windows.Forms.Label();
			this.flowMain.SuspendLayout();
			this.pnlHeader.SuspendLayout();
			this.pnlThongTin.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowMain
			// 
			this.flowMain.AutoScroll = true;
			this.flowMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.flowMain.Controls.Add(this.pnlHeader);
			this.flowMain.Controls.Add(this.pnlThongTin);
			this.flowMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowMain.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowMain.Location = new System.Drawing.Point(0, 0);
			this.flowMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.flowMain.Name = "flowMain";
			this.flowMain.Padding = new System.Windows.Forms.Padding(15, 20, 15, 20);
			this.flowMain.Size = new System.Drawing.Size(1550, 1000);
			this.flowMain.TabIndex = 0;
			this.flowMain.WrapContents = false;
			// 
			// pnlHeader
			// 
			this.pnlHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.pnlHeader.Controls.Add(this.lblTitle);
			this.pnlHeader.Location = new System.Drawing.Point(19, 25);
			this.pnlHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 20);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(1500, 100);
			this.pnlHeader.TabIndex = 0;
			// 
			// lblTitle
			// 
			this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(1500, 100);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "THÔNG TIN CÁ NHÂN";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlThongTin
			// 
			this.pnlThongTin.BackColor = System.Drawing.Color.White;
			this.pnlThongTin.Controls.Add(this.btnDoiMatKhau);
			this.pnlThongTin.Controls.Add(this.btnCapNhat);
			this.pnlThongTin.Controls.Add(this.cboGioiTinh);
			this.pnlThongTin.Controls.Add(this.dtpNamSinh);
			this.pnlThongTin.Controls.Add(this.txtEmail);
			this.pnlThongTin.Controls.Add(this.txtSDT);
			this.pnlThongTin.Controls.Add(this.txtTenKH);
			this.pnlThongTin.Controls.Add(this.txtTenDangNhap);
			this.pnlThongTin.Controls.Add(this.lblGioiTinh);
			this.pnlThongTin.Controls.Add(this.lblNamSinh);
			this.pnlThongTin.Controls.Add(this.lblEmail);
			this.pnlThongTin.Controls.Add(this.lblSDT);
			this.pnlThongTin.Controls.Add(this.lblTenKH);
			this.pnlThongTin.Controls.Add(this.lblTenDangNhap);
			this.pnlThongTin.Controls.Add(this.lblThongTinTitle);
			this.pnlThongTin.Location = new System.Drawing.Point(19, 150);
			this.pnlThongTin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 20);
			this.pnlThongTin.MinimumSize = new System.Drawing.Size(1080, 600);
			this.pnlThongTin.Name = "pnlThongTin";
			this.pnlThongTin.Padding = new System.Windows.Forms.Padding(30, 25, 30, 25);
			this.pnlThongTin.Size = new System.Drawing.Size(1500, 600);
			this.pnlThongTin.TabIndex = 1;
			// 
			// btnDoiMatKhau
			// 
			this.btnDoiMatKhau.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
			this.btnDoiMatKhau.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnDoiMatKhau.FlatAppearance.BorderSize = 0;
			this.btnDoiMatKhau.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDoiMatKhau.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.btnDoiMatKhau.ForeColor = System.Drawing.Color.White;
			this.btnDoiMatKhau.Location = new System.Drawing.Point(240, 520);
			this.btnDoiMatKhau.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnDoiMatKhau.Name = "btnDoiMatKhau";
			this.btnDoiMatKhau.Size = new System.Drawing.Size(180, 50);
			this.btnDoiMatKhau.TabIndex = 14;
			this.btnDoiMatKhau.Text = "Đổi mật khẩu";
			this.btnDoiMatKhau.UseVisualStyleBackColor = false;
			this.btnDoiMatKhau.Click += new System.EventHandler(this.btnDoiMatKhau_Click);
			// 
			// btnCapNhat
			// 
			this.btnCapNhat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.btnCapNhat.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnCapNhat.FlatAppearance.BorderSize = 0;
			this.btnCapNhat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCapNhat.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.btnCapNhat.ForeColor = System.Drawing.Color.White;
			this.btnCapNhat.Location = new System.Drawing.Point(40, 520);
			this.btnCapNhat.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnCapNhat.Name = "btnCapNhat";
			this.btnCapNhat.Size = new System.Drawing.Size(180, 50);
			this.btnCapNhat.TabIndex = 13;
			this.btnCapNhat.Text = "Cập nhật";
			this.btnCapNhat.UseVisualStyleBackColor = false;
			this.btnCapNhat.Click += new System.EventHandler(this.btnCapNhat_Click);
			// 
			// cboGioiTinh
			// 
			this.cboGioiTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGioiTinh.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.cboGioiTinh.FormattingEnabled = true;
			this.cboGioiTinh.Items.AddRange(new object[] {
            "Nam",
            "Nữ"});
			this.cboGioiTinh.Location = new System.Drawing.Point(240, 440);
			this.cboGioiTinh.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.cboGioiTinh.Name = "cboGioiTinh";
			this.cboGioiTinh.Size = new System.Drawing.Size(400, 38);
			this.cboGioiTinh.TabIndex = 12;
			// 
			// dtpNamSinh
			// 
			this.dtpNamSinh.CustomFormat = "dd/MM/yyyy";
			this.dtpNamSinh.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.dtpNamSinh.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpNamSinh.Location = new System.Drawing.Point(240, 370);
			this.dtpNamSinh.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.dtpNamSinh.Name = "dtpNamSinh";
			this.dtpNamSinh.Size = new System.Drawing.Size(400, 37);
			this.dtpNamSinh.TabIndex = 11;
			// 
			// txtEmail
			// 
			this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.txtEmail.Location = new System.Drawing.Point(240, 300);
			this.txtEmail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.Size = new System.Drawing.Size(400, 37);
			this.txtEmail.TabIndex = 10;
			// 
			// txtSDT
			// 
			this.txtSDT.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.txtSDT.Location = new System.Drawing.Point(240, 230);
			this.txtSDT.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtSDT.MaxLength = 11;
			this.txtSDT.Name = "txtSDT";
			this.txtSDT.Size = new System.Drawing.Size(400, 37);
			this.txtSDT.TabIndex = 9;
			// 
			// txtTenKH
			// 
			this.txtTenKH.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.txtTenKH.Location = new System.Drawing.Point(240, 160);
			this.txtTenKH.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtTenKH.Name = "txtTenKH";
			this.txtTenKH.Size = new System.Drawing.Size(400, 37);
			this.txtTenKH.TabIndex = 8;
			// 
			// txtTenDangNhap
			// 
			this.txtTenDangNhap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.txtTenDangNhap.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.txtTenDangNhap.Location = new System.Drawing.Point(240, 90);
			this.txtTenDangNhap.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtTenDangNhap.Name = "txtTenDangNhap";
			this.txtTenDangNhap.ReadOnly = true;
			this.txtTenDangNhap.Size = new System.Drawing.Size(400, 37);
			this.txtTenDangNhap.TabIndex = 7;
			// 
			// lblGioiTinh
			// 
			this.lblGioiTinh.AutoSize = true;
			this.lblGioiTinh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lblGioiTinh.Location = new System.Drawing.Point(40, 445);
			this.lblGioiTinh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblGioiTinh.Name = "lblGioiTinh";
			this.lblGioiTinh.Size = new System.Drawing.Size(93, 28);
			this.lblGioiTinh.TabIndex = 6;
			this.lblGioiTinh.Text = "Giới tính";
			// 
			// lblNamSinh
			// 
			this.lblNamSinh.AutoSize = true;
			this.lblNamSinh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lblNamSinh.Location = new System.Drawing.Point(40, 375);
			this.lblNamSinh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblNamSinh.Name = "lblNamSinh";
			this.lblNamSinh.Size = new System.Drawing.Size(102, 28);
			this.lblNamSinh.TabIndex = 5;
			this.lblNamSinh.Text = "Năm sinh";
			// 
			// lblEmail
			// 
			this.lblEmail.AutoSize = true;
			this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lblEmail.Location = new System.Drawing.Point(40, 305);
			this.lblEmail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblEmail.Name = "lblEmail";
			this.lblEmail.Size = new System.Drawing.Size(62, 28);
			this.lblEmail.TabIndex = 4;
			this.lblEmail.Text = "Email";
			// 
			// lblSDT
			// 
			this.lblSDT.AutoSize = true;
			this.lblSDT.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lblSDT.Location = new System.Drawing.Point(40, 235);
			this.lblSDT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblSDT.Name = "lblSDT";
			this.lblSDT.Size = new System.Drawing.Size(136, 28);
			this.lblSDT.TabIndex = 3;
			this.lblSDT.Text = "Số điện thoại";
			// 
			// lblTenKH
			// 
			this.lblTenKH.AutoSize = true;
			this.lblTenKH.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lblTenKH.Location = new System.Drawing.Point(40, 165);
			this.lblTenKH.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTenKH.Name = "lblTenKH";
			this.lblTenKH.Size = new System.Drawing.Size(165, 28);
			this.lblTenKH.TabIndex = 2;
			this.lblTenKH.Text = "Tên khách hàng";
			// 
			// lblTenDangNhap
			// 
			this.lblTenDangNhap.AutoSize = true;
			this.lblTenDangNhap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lblTenDangNhap.Location = new System.Drawing.Point(40, 95);
			this.lblTenDangNhap.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTenDangNhap.Name = "lblTenDangNhap";
			this.lblTenDangNhap.Size = new System.Drawing.Size(151, 28);
			this.lblTenDangNhap.TabIndex = 1;
			this.lblTenDangNhap.Text = "Tên đăng nhập";
			// 
			// lblThongTinTitle
			// 
			this.lblThongTinTitle.AutoSize = true;
			this.lblThongTinTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lblThongTinTitle.Location = new System.Drawing.Point(30, 25);
			this.lblThongTinTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblThongTinTitle.Name = "lblThongTinTitle";
			this.lblThongTinTitle.Size = new System.Drawing.Size(238, 38);
			this.lblThongTinTitle.TabIndex = 0;
			this.lblThongTinTitle.Text = "Thông tin chi tiết";
			// 
			// UCThongTinUser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.flowMain);
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "UCThongTinUser";
			this.Size = new System.Drawing.Size(1550, 1000);
			this.flowMain.ResumeLayout(false);
			this.pnlHeader.ResumeLayout(false);
			this.pnlThongTin.ResumeLayout(false);
			this.pnlThongTin.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowMain;
		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Panel pnlThongTin;
		private System.Windows.Forms.Label lblThongTinTitle;
		private System.Windows.Forms.Label lblTenDangNhap;
		private System.Windows.Forms.Label lblTenKH;
		private System.Windows.Forms.Label lblSDT;
		private System.Windows.Forms.Label lblEmail;
		private System.Windows.Forms.Label lblNamSinh;
		private System.Windows.Forms.Label lblGioiTinh;
		private System.Windows.Forms.TextBox txtTenDangNhap;
		private System.Windows.Forms.TextBox txtTenKH;
		private System.Windows.Forms.TextBox txtSDT;
		private System.Windows.Forms.TextBox txtEmail;
		private System.Windows.Forms.DateTimePicker dtpNamSinh;
		private System.Windows.Forms.ComboBox cboGioiTinh;
		private System.Windows.Forms.Button btnCapNhat;
		private System.Windows.Forms.Button btnDoiMatKhau;
	}
}
