namespace QLBXMD.GUI.Admin
{
	partial class TuyenDuongDialog
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>Clean up any resources being used.</summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent()
		{
			this.txtMaTuyen = new System.Windows.Forms.TextBox();
			this.lblMaTuyen = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtDiemDi = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtDiemDen = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.numQuangDuong = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.numThoiGian = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.cbbTrangThai = new System.Windows.Forms.ComboBox();
			this.btnPrimary = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.lblHeader = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numQuangDuong)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numThoiGian)).BeginInit();
			this.pnlHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtMaTuyen
			// 
			this.txtMaTuyen.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtMaTuyen.Enabled = false;
			this.txtMaTuyen.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtMaTuyen.Location = new System.Drawing.Point(220, 97);
			this.txtMaTuyen.Name = "txtMaTuyen";
			this.txtMaTuyen.ReadOnly = true;
			this.txtMaTuyen.Size = new System.Drawing.Size(340, 34);
			this.txtMaTuyen.TabIndex = 0;
			// 
			// lblMaTuyen
			// 
			this.lblMaTuyen.AutoSize = true;
			this.lblMaTuyen.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblMaTuyen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblMaTuyen.Location = new System.Drawing.Point(60, 100);
			this.lblMaTuyen.Name = "lblMaTuyen";
			this.lblMaTuyen.Size = new System.Drawing.Size(98, 28);
			this.lblMaTuyen.TabIndex = 1;
			this.lblMaTuyen.Text = "Mã tuyến:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label2.Location = new System.Drawing.Point(60, 155);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 28);
			this.label2.TabIndex = 3;
			this.label2.Text = "Điểm đi:";
			// 
			// txtDiemDi
			// 
			this.txtDiemDi.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtDiemDi.Location = new System.Drawing.Point(220, 152);
			this.txtDiemDi.Name = "txtDiemDi";
			this.txtDiemDi.Size = new System.Drawing.Size(340, 34);
			this.txtDiemDi.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label3.Location = new System.Drawing.Point(60, 210);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 28);
			this.label3.TabIndex = 5;
			this.label3.Text = "Điểm đến:";
			// 
			// txtDiemDen
			// 
			this.txtDiemDen.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtDiemDen.Location = new System.Drawing.Point(220, 207);
			this.txtDiemDen.Name = "txtDiemDen";
			this.txtDiemDen.Size = new System.Drawing.Size(340, 34);
			this.txtDiemDen.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label4.Location = new System.Drawing.Point(60, 265);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(139, 28);
			this.label4.TabIndex = 7;
			this.label4.Text = "Quãng đường:";
			// 
			// numQuangDuong
			// 
			this.numQuangDuong.DecimalPlaces = 1;
			this.numQuangDuong.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.numQuangDuong.Location = new System.Drawing.Point(220, 262);
			this.numQuangDuong.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.numQuangDuong.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numQuangDuong.Name = "numQuangDuong";
			this.numQuangDuong.Size = new System.Drawing.Size(200, 34);
			this.numQuangDuong.TabIndex = 6;
			this.numQuangDuong.ThousandsSeparator = true;
			this.numQuangDuong.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label5.Location = new System.Drawing.Point(60, 320);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(167, 28);
			this.label5.TabIndex = 9;
			this.label5.Text = "Thời gian dự kiến:";
			// 
			// numThoiGian
			// 
			this.numThoiGian.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.numThoiGian.Location = new System.Drawing.Point(220, 317);
			this.numThoiGian.Maximum = new decimal(new int[] {
            2880,
            0,
            0,
            0});
			this.numThoiGian.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numThoiGian.Name = "numThoiGian";
			this.numThoiGian.Size = new System.Drawing.Size(200, 34);
			this.numThoiGian.TabIndex = 8;
			this.numThoiGian.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label6.Location = new System.Drawing.Point(60, 375);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(102, 28);
			this.label6.TabIndex = 11;
			this.label6.Text = "Trạng thái:";
			// 
			// cbbTrangThai
			// 
			this.cbbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbTrangThai.FormattingEnabled = true;
			this.cbbTrangThai.Items.AddRange(new object[] {
            "Hoạt động",
            "Ngừng hoạt động"});
			this.cbbTrangThai.Location = new System.Drawing.Point(220, 372);
			this.cbbTrangThai.Name = "cbbTrangThai";
			this.cbbTrangThai.Size = new System.Drawing.Size(340, 36);
			this.cbbTrangThai.TabIndex = 10;
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
			this.btnPrimary.TabIndex = 12;
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
			this.btnCancel.TabIndex = 13;
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
			this.pnlHeader.TabIndex = 14;
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
			this.lblHeader.Text = "THÔNG TIN TUYẾN ĐƯỜNG";
			this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TuyenDuongDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(630, 515);
			this.Controls.Add(this.pnlHeader);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnPrimary);
			this.Controls.Add(this.cbbTrangThai);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.numThoiGian);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.numQuangDuong);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtDiemDen);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtDiemDi);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lblMaTuyen);
			this.Controls.Add(this.txtMaTuyen);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TuyenDuongDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Quản lý tuyến đường";
			((System.ComponentModel.ISupportInitialize)(this.numQuangDuong)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numThoiGian)).EndInit();
			this.pnlHeader.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtMaTuyen;
		private System.Windows.Forms.Label lblMaTuyen;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtDiemDi;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtDiemDen;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown numQuangDuong;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown numThoiGian;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox cbbTrangThai;
		private System.Windows.Forms.Button btnPrimary;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Label lblHeader;
	}
}