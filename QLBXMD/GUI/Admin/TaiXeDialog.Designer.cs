namespace QLBXMD.GUI.Admin
{
	partial class TaiXeDialog
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
			this.txtMaTX = new System.Windows.Forms.TextBox();
			this.txtTenTX = new System.Windows.Forms.TextBox();
			this.txtCCCD = new System.Windows.Forms.TextBox();
			this.txtSDT = new System.Windows.Forms.TextBox();
			this.txtDiaChi = new System.Windows.Forms.TextBox();
			this.txtBangLai = new System.Windows.Forms.TextBox();
			this.dtpNamSinh = new System.Windows.Forms.DateTimePicker();
			this.dtpNgayCap = new System.Windows.Forms.DateTimePicker();
			this.cbbGioiTinh = new System.Windows.Forms.ComboBox();
			this.cbbTrangThai = new System.Windows.Forms.ComboBox();
			this.btnPrimary = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblMaTX = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.lblHeader = new System.Windows.Forms.Label();
			this.pnlHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtMaTX
			// 
			this.txtMaTX.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtMaTX.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtMaTX.Location = new System.Drawing.Point(220, 97);
			this.txtMaTX.Name = "txtMaTX";
			this.txtMaTX.ReadOnly = true;
			this.txtMaTX.Size = new System.Drawing.Size(340, 34);
			this.txtMaTX.TabIndex = 0;
			// 
			// txtTenTX
			// 
			this.txtTenTX.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtTenTX.Location = new System.Drawing.Point(220, 152);
			this.txtTenTX.Name = "txtTenTX";
			this.txtTenTX.Size = new System.Drawing.Size(340, 34);
			this.txtTenTX.TabIndex = 1;
			// 
			// txtCCCD
			// 
			this.txtCCCD.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtCCCD.Location = new System.Drawing.Point(220, 262);
			this.txtCCCD.MaxLength = 12;
			this.txtCCCD.Name = "txtCCCD";
			this.txtCCCD.Size = new System.Drawing.Size(340, 34);
			this.txtCCCD.TabIndex = 3;
			// 
			// txtSDT
			// 
			this.txtSDT.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtSDT.Location = new System.Drawing.Point(220, 372);
			this.txtSDT.MaxLength = 11;
			this.txtSDT.Name = "txtSDT";
			this.txtSDT.Size = new System.Drawing.Size(340, 34);
			this.txtSDT.TabIndex = 5;
			// 
			// txtDiaChi
			// 
			this.txtDiaChi.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtDiaChi.Location = new System.Drawing.Point(220, 427);
			this.txtDiaChi.Multiline = true;
			this.txtDiaChi.Name = "txtDiaChi";
			this.txtDiaChi.Size = new System.Drawing.Size(340, 60);
			this.txtDiaChi.TabIndex = 6;
			// 
			// txtBangLai
			// 
			this.txtBangLai.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtBangLai.Location = new System.Drawing.Point(220, 507);
			this.txtBangLai.Name = "txtBangLai";
			this.txtBangLai.Size = new System.Drawing.Size(340, 34);
			this.txtBangLai.TabIndex = 7;
			// 
			// dtpNamSinh
			// 
			this.dtpNamSinh.CustomFormat = "dd/MM/yyyy";
			this.dtpNamSinh.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.dtpNamSinh.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpNamSinh.Location = new System.Drawing.Point(220, 207);
			this.dtpNamSinh.Name = "dtpNamSinh";
			this.dtpNamSinh.Size = new System.Drawing.Size(340, 34);
			this.dtpNamSinh.TabIndex = 2;
			// 
			// dtpNgayCap
			// 
			this.dtpNgayCap.CustomFormat = "dd/MM/yyyy";
			this.dtpNgayCap.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.dtpNgayCap.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpNgayCap.Location = new System.Drawing.Point(220, 562);
			this.dtpNgayCap.Name = "dtpNgayCap";
			this.dtpNgayCap.Size = new System.Drawing.Size(340, 34);
			this.dtpNgayCap.TabIndex = 8;
			// 
			// cbbGioiTinh
			// 
			this.cbbGioiTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbGioiTinh.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbGioiTinh.FormattingEnabled = true;
			this.cbbGioiTinh.Items.AddRange(new object[] {
				"Nam",
				"Nữ"});
			this.cbbGioiTinh.Location = new System.Drawing.Point(220, 317);
			this.cbbGioiTinh.Name = "cbbGioiTinh";
			this.cbbGioiTinh.Size = new System.Drawing.Size(340, 36);
			this.cbbGioiTinh.TabIndex = 4;
			// 
			// cbbTrangThai
			// 
			this.cbbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbTrangThai.FormattingEnabled = true;
			this.cbbTrangThai.Items.AddRange(new object[] {
				"Hoạt động",
				"Ngừng hoạt động"});
			this.cbbTrangThai.Location = new System.Drawing.Point(220, 617);
			this.cbbTrangThai.Name = "cbbTrangThai";
			this.cbbTrangThai.Size = new System.Drawing.Size(250, 36);
			this.cbbTrangThai.TabIndex = 9;
			// 
			// btnPrimary
			// 
			this.btnPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
			this.btnPrimary.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnPrimary.FlatAppearance.BorderSize = 0;
			this.btnPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPrimary.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnPrimary.ForeColor = System.Drawing.Color.White;
			this.btnPrimary.Location = new System.Drawing.Point(350, 685);
			this.btnPrimary.Name = "btnPrimary";
			this.btnPrimary.Size = new System.Drawing.Size(100, 45);
			this.btnPrimary.TabIndex = 10;
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
			this.btnCancel.Location = new System.Drawing.Point(460, 685);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(100, 45);
			this.btnCancel.TabIndex = 11;
			this.btnCancel.Text = "Hủy";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblMaTX
			// 
			this.lblMaTX.AutoSize = true;
			this.lblMaTX.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblMaTX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblMaTX.Location = new System.Drawing.Point(60, 100);
			this.lblMaTX.Name = "lblMaTX";
			this.lblMaTX.Size = new System.Drawing.Size(95, 28);
			this.lblMaTX.TabIndex = 12;
			this.lblMaTX.Text = "Mã tài xế:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label2.Location = new System.Drawing.Point(60, 155);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 28);
			this.label2.TabIndex = 13;
			this.label2.Text = "Tên tài xế:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label3.Location = new System.Drawing.Point(60, 210);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 28);
			this.label3.TabIndex = 14;
			this.label3.Text = "Năm sinh:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label4.Location = new System.Drawing.Point(60, 265);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(69, 28);
			this.label4.TabIndex = 15;
			this.label4.Text = "CCCD:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label5.Location = new System.Drawing.Point(60, 320);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(91, 28);
			this.label5.TabIndex = 16;
			this.label5.Text = "Giới tính:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label6.Location = new System.Drawing.Point(60, 375);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(132, 28);
			this.label6.TabIndex = 17;
			this.label6.Text = "Số điện thoại:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label7.Location = new System.Drawing.Point(60, 430);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(76, 28);
			this.label7.TabIndex = 18;
			this.label7.Text = "Địa chỉ:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label8.Location = new System.Drawing.Point(60, 510);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(89, 28);
			this.label8.TabIndex = 19;
			this.label8.Text = "Bằng lái:";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label9.Location = new System.Drawing.Point(60, 565);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(98, 28);
			this.label9.TabIndex = 20;
			this.label9.Text = "Ngày cấp:";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label10.Location = new System.Drawing.Point(60, 620);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(102, 28);
			this.label10.TabIndex = 21;
			this.label10.Text = "Trạng thái:";
			// 
			// pnlHeader
			// 
			this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.pnlHeader.Controls.Add(this.lblHeader);
			this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlHeader.Location = new System.Drawing.Point(0, 0);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(630, 70);
			this.pnlHeader.TabIndex = 22;
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
			this.lblHeader.Text = "THÔNG TIN TÀI XẾ";
			this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TaiXeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(630, 760);
			this.Controls.Add(this.pnlHeader);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblMaTX);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnPrimary);
			this.Controls.Add(this.cbbTrangThai);
			this.Controls.Add(this.cbbGioiTinh);
			this.Controls.Add(this.dtpNgayCap);
			this.Controls.Add(this.dtpNamSinh);
			this.Controls.Add(this.txtBangLai);
			this.Controls.Add(this.txtDiaChi);
			this.Controls.Add(this.txtSDT);
			this.Controls.Add(this.txtCCCD);
			this.Controls.Add(this.txtTenTX);
			this.Controls.Add(this.txtMaTX);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TaiXeDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Quản lý tài xế";
			this.pnlHeader.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtMaTX;
		private System.Windows.Forms.TextBox txtTenTX;
		private System.Windows.Forms.TextBox txtCCCD;
		private System.Windows.Forms.TextBox txtSDT;
		private System.Windows.Forms.TextBox txtDiaChi;
		private System.Windows.Forms.TextBox txtBangLai;
		private System.Windows.Forms.DateTimePicker dtpNamSinh;
		private System.Windows.Forms.DateTimePicker dtpNgayCap;
		private System.Windows.Forms.ComboBox cbbGioiTinh;
		private System.Windows.Forms.ComboBox cbbTrangThai;
		private System.Windows.Forms.Button btnPrimary;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblMaTX;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Label lblHeader;
	}
}
