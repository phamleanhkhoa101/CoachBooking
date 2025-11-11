namespace QLBXMD.GUI.Admin
{
	partial class KhachHangDialog
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
			this.lblMaKH = new System.Windows.Forms.Label();
			this.txtMaKH = new System.Windows.Forms.TextBox();
			this.lblTenKH = new System.Windows.Forms.Label();
			this.txtTenKH = new System.Windows.Forms.TextBox();
			this.lblNamSinh = new System.Windows.Forms.Label();
			this.dtpNamSinh = new System.Windows.Forms.DateTimePicker();
			this.lblSDT = new System.Windows.Forms.Label();
			this.txtSDT = new System.Windows.Forms.TextBox();
			this.lblGioiTinh = new System.Windows.Forms.Label();
			this.cbbGioiTinh = new System.Windows.Forms.ComboBox();
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
			// lblMaKH
			// 
			this.lblMaKH.AutoSize = true;
			this.lblMaKH.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblMaKH.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblMaKH.Location = new System.Drawing.Point(60, 100);
			this.lblMaKH.Name = "lblMaKH";
			this.lblMaKH.Size = new System.Drawing.Size(149, 28);
			this.lblMaKH.TabIndex = 0;
			this.lblMaKH.Text = "Mã khách hàng:";
			// 
			// txtMaKH
			// 
			this.txtMaKH.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtMaKH.Enabled = false;
			this.txtMaKH.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtMaKH.Location = new System.Drawing.Point(220, 97);
			this.txtMaKH.Name = "txtMaKH";
			this.txtMaKH.ReadOnly = true;
			this.txtMaKH.Size = new System.Drawing.Size(340, 34);
			this.txtMaKH.TabIndex = 1;
			// 
			// lblTenKH
			// 
			this.lblTenKH.AutoSize = true;
			this.lblTenKH.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblTenKH.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblTenKH.Location = new System.Drawing.Point(60, 155);
			this.lblTenKH.Name = "lblTenKH";
			this.lblTenKH.Size = new System.Drawing.Size(150, 28);
			this.lblTenKH.TabIndex = 2;
			this.lblTenKH.Text = "Tên khách hàng:";
			// 
			// txtTenKH
			// 
			this.txtTenKH.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtTenKH.Location = new System.Drawing.Point(220, 152);
			this.txtTenKH.Name = "txtTenKH";
			this.txtTenKH.Size = new System.Drawing.Size(340, 34);
			this.txtTenKH.TabIndex = 3;
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
			// lblSDT
			// 
			this.lblSDT.AutoSize = true;
			this.lblSDT.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblSDT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblSDT.Location = new System.Drawing.Point(60, 265);
			this.lblSDT.Name = "lblSDT";
			this.lblSDT.Size = new System.Drawing.Size(132, 28);
			this.lblSDT.TabIndex = 6;
			this.lblSDT.Text = "Số điện thoại:";
			// 
			// txtSDT
			// 
			this.txtSDT.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtSDT.Location = new System.Drawing.Point(220, 262);
			this.txtSDT.MaxLength = 11;
			this.txtSDT.Name = "txtSDT";
			this.txtSDT.Size = new System.Drawing.Size(340, 34);
			this.txtSDT.TabIndex = 7;
			// 
			// lblGioiTinh
			// 
			this.lblGioiTinh.AutoSize = true;
			this.lblGioiTinh.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblGioiTinh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblGioiTinh.Location = new System.Drawing.Point(60, 320);
			this.lblGioiTinh.Name = "lblGioiTinh";
			this.lblGioiTinh.Size = new System.Drawing.Size(91, 28);
			this.lblGioiTinh.TabIndex = 8;
			this.lblGioiTinh.Text = "Giới tính:";
			// 
			// cbbGioiTinh
			// 
			this.cbbGioiTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbGioiTinh.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbGioiTinh.FormattingEnabled = true;
			this.cbbGioiTinh.Location = new System.Drawing.Point(220, 317);
			this.cbbGioiTinh.Name = "cbbGioiTinh";
			this.cbbGioiTinh.Size = new System.Drawing.Size(340, 36);
			this.cbbGioiTinh.TabIndex = 9;
			// 
			// lblTaiKhoan
			// 
			this.lblTaiKhoan.AutoSize = true;
			this.lblTaiKhoan.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblTaiKhoan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblTaiKhoan.Location = new System.Drawing.Point(60, 375);
			this.lblTaiKhoan.Name = "lblTaiKhoan";
			this.lblTaiKhoan.Size = new System.Drawing.Size(98, 28);
			this.lblTaiKhoan.TabIndex = 10;
			this.lblTaiKhoan.Text = "Tài khoản:";
			// 
			// cbbTaiKhoan
			// 
			this.cbbTaiKhoan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTaiKhoan.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbTaiKhoan.FormattingEnabled = true;
			this.cbbTaiKhoan.Location = new System.Drawing.Point(220, 372);
			this.cbbTaiKhoan.Name = "cbbTaiKhoan";
			this.cbbTaiKhoan.Size = new System.Drawing.Size(340, 36);
			this.cbbTaiKhoan.TabIndex = 11;
			// 
			// btnGanTaiKhoan
			// 
			this.btnGanTaiKhoan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
			this.btnGanTaiKhoan.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnGanTaiKhoan.FlatAppearance.BorderSize = 0;
			this.btnGanTaiKhoan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnGanTaiKhoan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnGanTaiKhoan.ForeColor = System.Drawing.Color.White;
			this.btnGanTaiKhoan.Location = new System.Drawing.Point(568, 372);
			this.btnGanTaiKhoan.Name = "btnGanTaiKhoan";
			this.btnGanTaiKhoan.Size = new System.Drawing.Size(40, 36);
			this.btnGanTaiKhoan.TabIndex = 12;
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
			this.btnPrimary.Location = new System.Drawing.Point(350, 440);
			this.btnPrimary.Name = "btnPrimary";
			this.btnPrimary.Size = new System.Drawing.Size(100, 45);
			this.btnPrimary.TabIndex = 13;
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
			this.btnCancel.Location = new System.Drawing.Point(460, 440);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(100, 45);
			this.btnCancel.TabIndex = 14;
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
			this.pnlHeader.TabIndex = 15;
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
			this.lblHeader.Text = "THÔNG TIN KHÁCH HÀNG";
			this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// KhachHangDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(630, 515);
			this.Controls.Add(this.pnlHeader);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnPrimary);
			this.Controls.Add(this.btnGanTaiKhoan);
			this.Controls.Add(this.cbbTaiKhoan);
			this.Controls.Add(this.lblTaiKhoan);
			this.Controls.Add(this.cbbGioiTinh);
			this.Controls.Add(this.lblGioiTinh);
			this.Controls.Add(this.txtSDT);
			this.Controls.Add(this.lblSDT);
			this.Controls.Add(this.dtpNamSinh);
			this.Controls.Add(this.lblNamSinh);
			this.Controls.Add(this.txtTenKH);
			this.Controls.Add(this.lblTenKH);
			this.Controls.Add(this.txtMaKH);
			this.Controls.Add(this.lblMaKH);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "KhachHangDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Quản lý khách hàng";
			this.pnlHeader.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblMaKH;
		private System.Windows.Forms.TextBox txtMaKH;
		private System.Windows.Forms.Label lblTenKH;
		private System.Windows.Forms.TextBox txtTenKH;
		private System.Windows.Forms.Label lblNamSinh;
		private System.Windows.Forms.DateTimePicker dtpNamSinh;
		private System.Windows.Forms.Label lblSDT;
		private System.Windows.Forms.TextBox txtSDT;
		private System.Windows.Forms.Label lblGioiTinh;
		private System.Windows.Forms.ComboBox cbbGioiTinh;
		private System.Windows.Forms.Label lblTaiKhoan;
		private System.Windows.Forms.ComboBox cbbTaiKhoan;
		private System.Windows.Forms.Button btnGanTaiKhoan;
		private System.Windows.Forms.Button btnPrimary;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Label lblHeader;
	}
}
