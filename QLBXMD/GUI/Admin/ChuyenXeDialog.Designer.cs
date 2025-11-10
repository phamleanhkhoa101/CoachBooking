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
			this.panel1 = new System.Windows.Forms.Panel();
			this.label8 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numGiaVe)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtMaChuyen
			// 
			this.txtMaChuyen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtMaChuyen.Location = new System.Drawing.Point(216, 88);
			this.txtMaChuyen.Name = "txtMaChuyen";
			this.txtMaChuyen.ReadOnly = true;
			this.txtMaChuyen.Size = new System.Drawing.Size(360, 30);
			this.txtMaChuyen.TabIndex = 0;
			// 
			// cbbXe
			// 
			this.cbbXe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbXe.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbbXe.FormattingEnabled = true;
			this.cbbXe.Location = new System.Drawing.Point(216, 144);
			this.cbbXe.Name = "cbbXe";
			this.cbbXe.Size = new System.Drawing.Size(360, 33);
			this.cbbXe.TabIndex = 1;
			// 
			// cbbTuyen
			// 
			this.cbbTuyen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTuyen.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbbTuyen.FormattingEnabled = true;
			this.cbbTuyen.Location = new System.Drawing.Point(216, 195);
			this.cbbTuyen.Name = "cbbTuyen";
			this.cbbTuyen.Size = new System.Drawing.Size(360, 33);
			this.cbbTuyen.TabIndex = 2;
			// 
			// cbbTaiXe
			// 
			this.cbbTaiXe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTaiXe.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbbTaiXe.FormattingEnabled = true;
			this.cbbTaiXe.Location = new System.Drawing.Point(216, 250);
			this.cbbTaiXe.Name = "cbbTaiXe";
			this.cbbTaiXe.Size = new System.Drawing.Size(360, 33);
			this.cbbTaiXe.TabIndex = 3;
			// 
			// dtpKhoiHanh
			// 
			this.dtpKhoiHanh.CustomFormat = "dd/MM/yyyy HH:mm";
			this.dtpKhoiHanh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dtpKhoiHanh.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpKhoiHanh.Location = new System.Drawing.Point(216, 304);
			this.dtpKhoiHanh.Name = "dtpKhoiHanh";
			this.dtpKhoiHanh.Size = new System.Drawing.Size(220, 30);
			this.dtpKhoiHanh.TabIndex = 4;
			// 
			// dtpDenDuKien
			// 
			this.dtpDenDuKien.CustomFormat = "dd/MM/yyyy HH:mm";
			this.dtpDenDuKien.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dtpDenDuKien.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpDenDuKien.Location = new System.Drawing.Point(216, 352);
			this.dtpDenDuKien.Name = "dtpDenDuKien";
			this.dtpDenDuKien.ShowCheckBox = true;
			this.dtpDenDuKien.Size = new System.Drawing.Size(220, 30);
			this.dtpDenDuKien.TabIndex = 5;
			// 
			// numGiaVe
			// 
			this.numGiaVe.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.numGiaVe.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numGiaVe.Location = new System.Drawing.Point(216, 397);
			this.numGiaVe.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
			this.numGiaVe.Name = "numGiaVe";
			this.numGiaVe.Size = new System.Drawing.Size(220, 30);
			this.numGiaVe.TabIndex = 6;
			this.numGiaVe.ThousandsSeparator = true;
			// 
			// lblGiaVeInfo
			// 
			this.lblGiaVeInfo.AutoSize = true;
			this.lblGiaVeInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblGiaVeInfo.ForeColor = System.Drawing.Color.Green;
			this.lblGiaVeInfo.Location = new System.Drawing.Point(216, 425);
			this.lblGiaVeInfo.Name = "lblGiaVeInfo";
			this.lblGiaVeInfo.Size = new System.Drawing.Size(0, 25);
			this.lblGiaVeInfo.TabIndex = 7;
			// 
			// btnPrimary
			// 
			this.btnPrimary.BackColor = System.Drawing.Color.SpringGreen;
			this.btnPrimary.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnPrimary.Location = new System.Drawing.Point(365, 476);
			this.btnPrimary.Name = "btnPrimary";
			this.btnPrimary.Size = new System.Drawing.Size(100, 44);
			this.btnPrimary.TabIndex = 8;
			this.btnPrimary.Text = "Lưu";
			this.btnPrimary.UseVisualStyleBackColor = false;
			this.btnPrimary.Click += new System.EventHandler(this.btnPrimary_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.LightCoral;
			this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel.Location = new System.Drawing.Point(475, 476);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(101, 44);
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "Hủy";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(76, 92);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(109, 25);
			this.label1.TabIndex = 10;
			this.label1.Text = "Mã chuyến";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(76, 148);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(37, 25);
			this.label2.TabIndex = 11;
			this.label2.Text = "Xe";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(76, 199);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(68, 25);
			this.label3.TabIndex = 12;
			this.label3.Text = "Tuyến";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(76, 254);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(66, 25);
			this.label4.TabIndex = 13;
			this.label4.Text = "Tài xế";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(76, 308);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(101, 25);
			this.label5.TabIndex = 14;
			this.label5.Text = "Khởi hành";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(76, 356);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(116, 25);
			this.label6.TabIndex = 15;
			this.label6.Text = "Đến dự kiến";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(76, 401);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(74, 25);
			this.label7.TabIndex = 16;
			this.label7.Text = "Giá vé:";
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Teal;
			this.panel1.Controls.Add(this.label8);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(690, 67);
			this.panel1.TabIndex = 17;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.ForeColor = System.Drawing.Color.White;
			this.label8.Location = new System.Drawing.Point(198, 18);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(323, 37);
			this.label8.TabIndex = 18;
			this.label8.Text = "Thông tin chuyến xe";
			// 
			// ChuyenXeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(690, 575);
			this.Controls.Add(this.panel1);
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
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
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
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label8;
	}
}
