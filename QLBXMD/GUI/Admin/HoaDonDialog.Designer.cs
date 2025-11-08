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
			((System.ComponentModel.ISupportInitialize)(this.numTongTien)).BeginInit();
			this.SuspendLayout();
			// 
			// txtMaHD
			// 
			this.txtMaHD.BackColor = System.Drawing.Color.LightGray;
			this.txtMaHD.Location = new System.Drawing.Point(160, 16);
			this.txtMaHD.Name = "txtMaHD";
			this.txtMaHD.ReadOnly = true;
			this.txtMaHD.Size = new System.Drawing.Size(400, 26);
			this.txtMaHD.TabIndex = 0;
			// 
			// cbbKhachHang
			// 
			this.cbbKhachHang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbKhachHang.FormattingEnabled = true;
			this.cbbKhachHang.Location = new System.Drawing.Point(160, 66);
			this.cbbKhachHang.Name = "cbbKhachHang";
			this.cbbKhachHang.Size = new System.Drawing.Size(400, 28);
			this.cbbKhachHang.TabIndex = 1;
			// 
			// cbbNhanVien
			// 
			this.cbbNhanVien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbNhanVien.FormattingEnabled = true;
			this.cbbNhanVien.Location = new System.Drawing.Point(160, 116);
			this.cbbNhanVien.Name = "cbbNhanVien";
			this.cbbNhanVien.Size = new System.Drawing.Size(400, 28);
			this.cbbNhanVien.TabIndex = 2;
			// 
			// dtpNgayLap
			// 
			this.dtpNgayLap.CustomFormat = "dd/MM/yyyy HH:mm";
			this.dtpNgayLap.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpNgayLap.Location = new System.Drawing.Point(160, 166);
			this.dtpNgayLap.Name = "dtpNgayLap";
			this.dtpNgayLap.Size = new System.Drawing.Size(250, 26);
			this.dtpNgayLap.TabIndex = 3;
			// 
			// numTongTien
			// 
			this.numTongTien.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numTongTien.Location = new System.Drawing.Point(160, 216);
			this.numTongTien.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
			this.numTongTien.Name = "numTongTien";
			this.numTongTien.Size = new System.Drawing.Size(250, 26);
			this.numTongTien.TabIndex = 4;
			this.numTongTien.ThousandsSeparator = true;
			// 
			// cbbTrangThai
			// 
			this.cbbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTrangThai.FormattingEnabled = true;
			this.cbbTrangThai.Items.AddRange(new object[] {
            "?ã thanh toán",
            "Ch?a thanh toán"});
			this.cbbTrangThai.Location = new System.Drawing.Point(160, 266);
			this.cbbTrangThai.Name = "cbbTrangThai";
			this.cbbTrangThai.Size = new System.Drawing.Size(250, 28);
			this.cbbTrangThai.TabIndex = 5;
			// 
			// btnPrimary
			// 
			this.btnPrimary.Location = new System.Drawing.Point(370, 370);
			this.btnPrimary.Name = "btnPrimary";
			this.btnPrimary.Size = new System.Drawing.Size(90, 35);
			this.btnPrimary.TabIndex = 6;
			this.btnPrimary.Text = "L?u";
			this.btnPrimary.UseVisualStyleBackColor = true;
			this.btnPrimary.Click += new System.EventHandler(this.btnPrimary_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(470, 370);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(90, 35);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "H?y";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(20, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(94, 20);
			this.label1.TabIndex = 8;
			this.label1.Text = "Mã hóa ??n";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(20, 70);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(94, 20);
			this.label2.TabIndex = 9;
			this.label2.Text = "Khách hàng";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(20, 120);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(81, 20);
			this.label3.TabIndex = 10;
			this.label3.Text = "Nhân viên";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(20, 170);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 20);
			this.label4.TabIndex = 11;
			this.label4.Text = "Ngày l?p";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(20, 220);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(104, 20);
			this.label5.TabIndex = 12;
			this.label5.Text = "T?ng ti?n (?)";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(20, 270);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 20);
			this.label6.TabIndex = 13;
			this.label6.Text = "Tr?ng thái";
			// 
			// HoaDonDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(600, 450);
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
			this.Text = "Hóa ??n";
			((System.ComponentModel.ISupportInitialize)(this.numTongTien)).EndInit();
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
	}
}
