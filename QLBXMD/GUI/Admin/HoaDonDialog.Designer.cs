namespace QLBXMD.GUI.Admin
{
	partial class HoaDonDialog
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
			this.txtMaHD = new System.Windows.Forms.TextBox();
			this.cbbKhachHang = new System.Windows.Forms.ComboBox();
			this.cbbNhanVien = new System.Windows.Forms.ComboBox();
			this.dtpNgayLap = new System.Windows.Forms.DateTimePicker();
			this.numTongTien = new System.Windows.Forms.NumericUpDown();
			this.cbbTrangThai = new System.Windows.Forms.ComboBox();
			this.btnPrimary = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.lblHeader = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numTongTien)).BeginInit();
			this.pnlHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtMaHD
			// 
			this.txtMaHD.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtMaHD.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtMaHD.Location = new System.Drawing.Point(200, 100);
			this.txtMaHD.Name = "txtMaHD";
			this.txtMaHD.ReadOnly = true;
			this.txtMaHD.Size = new System.Drawing.Size(360, 34);
			this.txtMaHD.TabIndex = 0;
			// 
			// cbbKhachHang
			// 
			this.cbbKhachHang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbKhachHang.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbKhachHang.FormattingEnabled = true;
			this.cbbKhachHang.Location = new System.Drawing.Point(200, 155);
			this.cbbKhachHang.Name = "cbbKhachHang";
			this.cbbKhachHang.Size = new System.Drawing.Size(360, 36);
			this.cbbKhachHang.TabIndex = 1;
			// 
			// cbbNhanVien
			// 
			this.cbbNhanVien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbNhanVien.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbNhanVien.FormattingEnabled = true;
			this.cbbNhanVien.Location = new System.Drawing.Point(200, 210);
			this.cbbNhanVien.Name = "cbbNhanVien";
			this.cbbNhanVien.Size = new System.Drawing.Size(360, 36);
			this.cbbNhanVien.TabIndex = 2;
			// 
			// dtpNgayLap
			// 
			this.dtpNgayLap.CustomFormat = "dd/MM/yyyy HH:mm";
			this.dtpNgayLap.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.dtpNgayLap.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpNgayLap.Location = new System.Drawing.Point(200, 265);
			this.dtpNgayLap.Name = "dtpNgayLap";
			this.dtpNgayLap.Size = new System.Drawing.Size(250, 34);
			this.dtpNgayLap.TabIndex = 3;
			// 
			// numTongTien
			// 
			this.numTongTien.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.numTongTien.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.numTongTien.Location = new System.Drawing.Point(200, 320);
			this.numTongTien.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
			this.numTongTien.Name = "numTongTien";
			this.numTongTien.Size = new System.Drawing.Size(250, 34);
			this.numTongTien.TabIndex = 4;
			this.numTongTien.ThousandsSeparator = true;
			// 
			// cbbTrangThai
			// 
			this.cbbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbTrangThai.FormattingEnabled = true;
			this.cbbTrangThai.Items.AddRange(new object[] {
            "Đã thanh toán",
            "Chưa thanh toán"});
			this.cbbTrangThai.Location = new System.Drawing.Point(200, 375);
			this.cbbTrangThai.Name = "cbbTrangThai";
			this.cbbTrangThai.Size = new System.Drawing.Size(250, 36);
			this.cbbTrangThai.TabIndex = 5;
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
			this.btnPrimary.TabIndex = 6;
			this.btnPrimary.Text = "Lưu";
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
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Hủy";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label1.Location = new System.Drawing.Point(60, 103);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(122, 28);
			this.label1.TabIndex = 8;
			this.label1.Text = "Mã hóa đơn:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label2.Location = new System.Drawing.Point(60, 158);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(118, 28);
			this.label2.TabIndex = 9;
			this.label2.Text = "Khách hàng:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label3.Location = new System.Drawing.Point(60, 213);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 28);
			this.label3.TabIndex = 10;
			this.label3.Text = "Nhân viên:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label4.Location = new System.Drawing.Point(60, 268);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(95, 28);
			this.label4.TabIndex = 11;
			this.label4.Text = "Ngày lập:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label5.Location = new System.Drawing.Point(60, 323);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(99, 28);
			this.label5.TabIndex = 12;
			this.label5.Text = "Tổng tiền:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label6.Location = new System.Drawing.Point(60, 378);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(102, 28);
			this.label6.TabIndex = 13;
			this.label6.Text = "Trạng thái:";
			// 
			// pnlHeader
			// 
			this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
			this.pnlHeader.Controls.Add(this.lblHeader);
			this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlHeader.Location = new System.Drawing.Point(0, 0);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(630, 70);
			this.pnlHeader.TabIndex = 14;
			// 
			// lblHeader
			// 
			this.lblHeader.BackColor = System.Drawing.Color.Teal;
			this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.lblHeader.ForeColor = System.Drawing.Color.White;
			this.lblHeader.Location = new System.Drawing.Point(0, 0);
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.Size = new System.Drawing.Size(630, 70);
			this.lblHeader.TabIndex = 0;
			this.lblHeader.Text = "THÔNG TIN HÓA ĐƠN";
			this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// HoaDonDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(630, 515);
			this.Controls.Add(this.pnlHeader);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnPrimary);
			this.Controls.Add(this.cbbTrangThai);
			this.Controls.Add(this.numTongTien);
			this.Controls.Add(this.dtpNgayLap);
			this.Controls.Add(this.cbbNhanVien);
			this.Controls.Add(this.cbbKhachHang);
			this.Controls.Add(this.txtMaHD);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HoaDonDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Quản lý hóa đơn";
			((System.ComponentModel.ISupportInitialize)(this.numTongTien)).EndInit();
			this.pnlHeader.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtMaHD;
		private System.Windows.Forms.ComboBox cbbKhachHang;
		private System.Windows.Forms.ComboBox cbbNhanVien;
		private System.Windows.Forms.DateTimePicker dtpNgayLap;
		private System.Windows.Forms.NumericUpDown numTongTien;
		private System.Windows.Forms.ComboBox cbbTrangThai;
		private System.Windows.Forms.Button btnPrimary;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Label lblHeader;
	}
}
