namespace QLBXMD.GUI.Admin
{
	partial class ChuyenXeDialog
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
			this.txtMaChuyen = new System.Windows.Forms.TextBox();
			this.cbbXe = new System.Windows.Forms.ComboBox();
			this.cbbTuyen = new System.Windows.Forms.ComboBox();
			this.cbbTaiXe = new System.Windows.Forms.ComboBox();
			this.dtpKhoiHanh = new System.Windows.Forms.DateTimePicker();
			this.dtpDenDuKien = new System.Windows.Forms.DateTimePicker();
			this.numGiaVe = new System.Windows.Forms.NumericUpDown();
			this.lblGiaVeInfo = new System.Windows.Forms.Label();
			this.btnPrimary = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numGiaVe)).BeginInit();
			this.SuspendLayout();
			// 
			// txtMaChuyen
			// 
			this.txtMaChuyen.Location = new System.Drawing.Point(160, 16);
			this.txtMaChuyen.Name = "txtMaChuyen";
			this.txtMaChuyen.ReadOnly = true;
			this.txtMaChuyen.Size = new System.Drawing.Size(360, 26);
			this.txtMaChuyen.TabIndex = 0;
			// 
			// cbbXe
			// 
			this.cbbXe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbXe.FormattingEnabled = true;
			this.cbbXe.Location = new System.Drawing.Point(160, 61);
			this.cbbXe.Name = "cbbXe";
			this.cbbXe.Size = new System.Drawing.Size(360, 28);
			this.cbbXe.TabIndex = 1;
			// 
			// cbbTuyen
			// 
			this.cbbTuyen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTuyen.FormattingEnabled = true;
			this.cbbTuyen.Location = new System.Drawing.Point(160, 106);
			this.cbbTuyen.Name = "cbbTuyen";
			this.cbbTuyen.Size = new System.Drawing.Size(360, 28);
			this.cbbTuyen.TabIndex = 2;
			// 
			// cbbTaiXe
			// 
			this.cbbTaiXe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTaiXe.FormattingEnabled = true;
			this.cbbTaiXe.Location = new System.Drawing.Point(160, 151);
			this.cbbTaiXe.Name = "cbbTaiXe";
			this.cbbTaiXe.Size = new System.Drawing.Size(360, 28);
			this.cbbTaiXe.TabIndex = 3;
			// 
			// dtpKhoiHanh
			// 
			this.dtpKhoiHanh.CustomFormat = "dd/MM/yyyy HH:mm";
			this.dtpKhoiHanh.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpKhoiHanh.Location = new System.Drawing.Point(160, 196);
			this.dtpKhoiHanh.Name = "dtpKhoiHanh";
			this.dtpKhoiHanh.Size = new System.Drawing.Size(220, 26);
			this.dtpKhoiHanh.TabIndex = 4;
			// 
			// dtpDenDuKien
			// 
			this.dtpDenDuKien.CustomFormat = "dd/MM/yyyy HH:mm";
			this.dtpDenDuKien.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpDenDuKien.Location = new System.Drawing.Point(160, 241);
			this.dtpDenDuKien.Name = "dtpDenDuKien";
			this.dtpDenDuKien.ShowCheckBox = true;
			this.dtpDenDuKien.Size = new System.Drawing.Size(220, 26);
			this.dtpDenDuKien.TabIndex = 5;
			// 
			// numGiaVe
			// 
			this.numGiaVe.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numGiaVe.Location = new System.Drawing.Point(160, 286);
			this.numGiaVe.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
			this.numGiaVe.Name = "numGiaVe";
			this.numGiaVe.Size = new System.Drawing.Size(220, 26);
			this.numGiaVe.TabIndex = 6;
			this.numGiaVe.ThousandsSeparator = true;
			// 
			// lblGiaVeInfo
			// 
			this.lblGiaVeInfo.AutoSize = true;
			this.lblGiaVeInfo.ForeColor = System.Drawing.Color.Green;
			this.lblGiaVeInfo.Location = new System.Drawing.Point(160, 314);
			this.lblGiaVeInfo.Name = "lblGiaVeInfo";
			this.lblGiaVeInfo.Size = new System.Drawing.Size(0, 20);
			this.lblGiaVeInfo.TabIndex = 7;
			// 
			// btnPrimary
			// 
			this.btnPrimary.Location = new System.Drawing.Point(330, 380);
			this.btnPrimary.Name = "btnPrimary";
			this.btnPrimary.Size = new System.Drawing.Size(90, 35);
			this.btnPrimary.TabIndex = 8;
			this.btnPrimary.Text = "Lưu";
			this.btnPrimary.UseVisualStyleBackColor = true;
			this.btnPrimary.Click += new System.EventHandler(this.btnPrimary_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(430, 380);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(90, 35);
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "Hủy";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(20, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(86, 20);
			this.label1.TabIndex = 10;
			this.label1.Text = "Mã chuyến";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(20, 65);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(29, 20);
			this.label2.TabIndex = 11;
			this.label2.Text = "Xe";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(20, 110);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(52, 20);
			this.label3.TabIndex = 12;
			this.label3.Text = "Tuyến";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(20, 155);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(50, 20);
			this.label4.TabIndex = 13;
			this.label4.Text = "Tài xế";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(20, 200);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 20);
			this.label5.TabIndex = 14;
			this.label5.Text = "Khởi hành";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(20, 245);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(94, 20);
			this.label6.TabIndex = 15;
			this.label6.Text = "Đến dự kiến";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(20, 290);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(58, 20);
			this.label7.TabIndex = 16;
			this.label7.Text = "Giá vé:";
			// 
			// ChuyenXeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(613, 472);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnPrimary);
			this.Controls.Add(this.lblGiaVeInfo);
			this.Controls.Add(this.numGiaVe);
			this.Controls.Add(this.dtpDenDuKien);
			this.Controls.Add(this.dtpKhoiHanh);
			this.Controls.Add(this.cbbTaiXe);
			this.Controls.Add(this.cbbTuyen);
			this.Controls.Add(this.cbbXe);
			this.Controls.Add(this.txtMaChuyen);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChuyenXeDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Chuyến xe";
			((System.ComponentModel.ISupportInitialize)(this.numGiaVe)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}


		#endregion

		private System.Windows.Forms.TextBox txtMaChuyen;
		private System.Windows.Forms.ComboBox cbbXe;
		private System.Windows.Forms.ComboBox cbbTuyen;
		private System.Windows.Forms.ComboBox cbbTaiXe;
		private System.Windows.Forms.DateTimePicker dtpKhoiHanh;
		private System.Windows.Forms.DateTimePicker dtpDenDuKien;
		private System.Windows.Forms.NumericUpDown numGiaVe;
		private System.Windows.Forms.Label lblGiaVeInfo;
		private System.Windows.Forms.Button btnPrimary;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
	}
}
