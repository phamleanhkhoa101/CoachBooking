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
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.lblHeader = new System.Windows.Forms.Label();
			this.pnlHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblMaNV
			// 
			this.lblMaNV.AutoSize = true;
			this.lblMaNV.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblMaNV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblMaNV.Location = new System.Drawing.Point(60, 100);
			this.lblMaNV.Name = "lblMaNV";
			this.lblMaNV.Size = new System.Drawing.Size(133, 28);
			this.lblMaNV.TabIndex = 0;
			this.lblMaNV.Text = "Mã nhân viên:";
			// 
			// txtMaNV
			// 
			this.txtMaNV.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtMaNV.Enabled = false;
			this.txtMaNV.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtMaNV.Location = new System.Drawing.Point(220, 97);
			this.txtMaNV.Name = "txtMaNV";
			this.txtMaNV.ReadOnly = true;
			this.txtMaNV.Size = new System.Drawing.Size(340, 34);
			this.txtMaNV.TabIndex = 1;
			// 
			// lblTenNV
			// 
			this.lblTenNV.AutoSize = true;
			this.lblTenNV.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblTenNV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblTenNV.Location = new System.Drawing.Point(60, 155);
			this.lblTenNV.Name = "lblTenNV";
			this.lblTenNV.Size = new System.Drawing.Size(134, 28);
			this.lblTenNV.TabIndex = 2;
			this.lblTenNV.Text = "Tên nhân viên:";
			// 
			// txtTenNV
			// 
			this.txtTenNV.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtTenNV.Location = new System.Drawing.Point(220, 152);
			this.txtTenNV.Name = "txtTenNV";
			this.txtTenNV.Size = new System.Drawing.Size(340, 34);
			this.txtTenNV.TabIndex = 3;
			// 
			// lblNamSinh
			// 
			this.lblNamSinh.AutoSize = true;
			this.lblNamSinh.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblNamSinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblNamSinh.Location = new System.Drawing.Point(60, 210);
			this.lblNamSinh.Name = "lblNamSinh";
			this.lblNamSinh.Size = new System.Drawing.Size(98, 28);
			this.lblNamSinh.TabIndex = 4;
			this.lblNamSinh.Text = "Năm sinh:";
			// 
			// dtpNamSinh
			// 
			this.dtpNamSinh.CustomFormat = "dd/MM/yyyy";
			this.dtpNamSinh.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.dtpNamSinh.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpNamSinh.Location = new System.Drawing.Point(220, 207);
			this.dtpNamSinh.Name = "dtpNamSinh";
			this.dtpNamSinh.Size = new System.Drawing.Size(340, 34);
			this.dtpNamSinh.TabIndex = 5;
			// 
			// lblDiaChi
			// 
			this.lblDiaChi.AutoSize = true;
			this.lblDiaChi.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblDiaChi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblDiaChi.Location = new System.Drawing.Point(60, 265);
			this.lblDiaChi.Name = "lblDiaChi";
			this.lblDiaChi.Size = new System.Drawing.Size(75, 28);
			this.lblDiaChi.TabIndex = 6;
			this.lblDiaChi.Text = "Địa chỉ:";
			// 
			// txtDiaChi
			// 
			this.txtDiaChi.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtDiaChi.Location = new System.Drawing.Point(220, 262);
			this.txtDiaChi.Multiline = true;
			this.txtDiaChi.Name = "txtDiaChi";
			this.txtDiaChi.Size = new System.Drawing.Size(340, 60);
			this.txtDiaChi.TabIndex = 7;
			// 
			// lblSDT
			// 
			this.lblSDT.AutoSize = true;
			this.lblSDT.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblSDT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblSDT.Location = new System.Drawing.Point(60, 345);
			this.lblSDT.Name = "lblSDT";
			this.lblSDT.Size = new System.Drawing.Size(132, 28);
			this.lblSDT.TabIndex = 8;
			this.lblSDT.Text = "Số điện thoại:";
			// 
			// txtSDT
			// 
			this.txtSDT.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtSDT.Location = new System.Drawing.Point(220, 342);
			this.txtSDT.MaxLength = 11;
			this.txtSDT.Name = "txtSDT";
			this.txtSDT.Size = new System.Drawing.Size(340, 34);
			this.txtSDT.TabIndex = 9;
			// 
			// lblGioiTinh
			// 
			this.lblGioiTinh.AutoSize = true;
			this.lblGioiTinh.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblGioiTinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblGioiTinh.Location = new System.Drawing.Point(60, 400);
			this.lblGioiTinh.Name = "lblGioiTinh";
			this.lblGioiTinh.Size = new System.Drawing.Size(91, 28);
			this.lblGioiTinh.TabIndex = 10;
			this.lblGioiTinh.Text = "Giới tính:";
			// 
			// cbbGioiTinh
			// 
			this.cbbGioiTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbGioiTinh.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbGioiTinh.FormattingEnabled = true;
			this.cbbGioiTinh.Location = new System.Drawing.Point(220, 397);
			this.cbbGioiTinh.Name = "cbbGioiTinh";
			this.cbbGioiTinh.Size = new System.Drawing.Size(340, 36);
			this.cbbGioiTinh.TabIndex = 11;
			// 
			// lblTrangThai
			// 
			this.lblTrangThai.AutoSize = true;
			this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblTrangThai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblTrangThai.Location = new System.Drawing.Point(60, 455);
			this.lblTrangThai.Name = "lblTrangThai";
			this.lblTrangThai.Size = new System.Drawing.Size(102, 28);
			this.lblTrangThai.TabIndex = 12;
			this.lblTrangThai.Text = "Trạng thái:";
			// 
			// cbbTrangThai
			// 
			this.cbbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbTrangThai.FormattingEnabled = true;
			this.cbbTrangThai.Location = new System.Drawing.Point(220, 452);
			this.cbbTrangThai.Name = "cbbTrangThai";
			this.cbbTrangThai.Size = new System.Drawing.Size(340, 36);
			this.cbbTrangThai.TabIndex = 13;
			// 
			// lblTaiKhoan
			// 
			this.lblTaiKhoan.AutoSize = true;
			this.lblTaiKhoan.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblTaiKhoan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblTaiKhoan.Location = new System.Drawing.Point(60, 510);
			this.lblTaiKhoan.Name = "lblTaiKhoan";
			this.lblTaiKhoan.Size = new System.Drawing.Size(98, 28);
			this.lblTaiKhoan.TabIndex = 14;
			this.lblTaiKhoan.Text = "Tài khoản:";
			// 
			// cbbTaiKhoan
			// 
			this.cbbTaiKhoan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTaiKhoan.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbTaiKhoan.FormattingEnabled = true;
			this.cbbTaiKhoan.Location = new System.Drawing.Point(220, 507);
			this.cbbTaiKhoan.Name = "cbbTaiKhoan";
			this.cbbTaiKhoan.Size = new System.Drawing.Size(340, 36);
			this.cbbTaiKhoan.TabIndex = 15;
			// 
			// btnGanTaiKhoan
			// 
			this.btnGanTaiKhoan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
			this.btnGanTaiKhoan.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnGanTaiKhoan.FlatAppearance.BorderSize = 0;
			this.btnGanTaiKhoan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnGanTaiKhoan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnGanTaiKhoan.ForeColor = System.Drawing.Color.White;
			this.btnGanTaiKhoan.Location = new System.Drawing.Point(578, 507);
			this.btnGanTaiKhoan.Name = "btnGanTaiKhoan";
			this.btnGanTaiKhoan.Size = new System.Drawing.Size(40, 36);
			this.btnGanTaiKhoan.TabIndex = 16;
			this.btnGanTaiKhoan.Text = "⚙";
			this.btnGanTaiKhoan.UseVisualStyleBackColor = false;
			this.btnGanTaiKhoan.Visible = false;
			this.btnGanTaiKhoan.Click += new System.EventHandler(this.btnGanTaiKhoan_Click);
			// 
			// btnPrimary
			// 
			this.btnPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
			this.btnPrimary.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnPrimary.FlatAppearance.BorderSize = 0;
			this.btnPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPrimary.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnPrimary.ForeColor = System.Drawing.Color.White;
			this.btnPrimary.Location = new System.Drawing.Point(350, 575);
			this.btnPrimary.Name = "btnPrimary";
			this.btnPrimary.Size = new System.Drawing.Size(100, 45);
			this.btnPrimary.TabIndex = 17;
			this.btnPrimary.Text = "Thêm";
			this.btnPrimary.UseVisualStyleBackColor = false;
			this.btnPrimary.Click += new System.EventHandler(this.btnPrimary_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
			this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnCancel.FlatAppearance.BorderSize = 0;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnCancel.ForeColor = System.Drawing.Color.White;
			this.btnCancel.Location = new System.Drawing.Point(460, 575);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(100, 45);
			this.btnCancel.TabIndex = 18;
			this.btnCancel.Text = "Hủy";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// pnlHeader
			// 
			this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.pnlHeader.Controls.Add(this.lblHeader);
			this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlHeader.Location = new System.Drawing.Point(0, 0);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(630, 70);
			this.pnlHeader.TabIndex = 19;
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
			this.lblHeader.Text = "THÔNG TIN NHÂN VIÊN";
			this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// NhanVienDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(630, 650);
			this.Controls.Add(this.pnlHeader);
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
			this.Text = "Quản lý nhân viên";
			this.pnlHeader.ResumeLayout(false);
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
		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Label lblHeader;
	}
}
