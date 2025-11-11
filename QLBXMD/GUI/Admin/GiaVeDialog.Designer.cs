namespace QLBXMD.GUI.Admin
{
	partial class GiaVeDialog
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
			this.txtMaGV = new System.Windows.Forms.TextBox();
			this.cbbNhaXe = new System.Windows.Forms.ComboBox();
			this.cbbLoaiXe = new System.Windows.Forms.ComboBox();
			this.cbbTuyen = new System.Windows.Forms.ComboBox();
			this.numGiaVe = new System.Windows.Forms.NumericUpDown();
			this.btnPrimary = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.lblHeader = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.numGiaVe)).BeginInit();
			this.pnlHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtMaGV
			// 
			this.txtMaGV.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtMaGV.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtMaGV.Location = new System.Drawing.Point(200, 100);
			this.txtMaGV.Name = "txtMaGV";
			this.txtMaGV.ReadOnly = true;
			this.txtMaGV.Size = new System.Drawing.Size(360, 34);
			this.txtMaGV.TabIndex = 0;
			// 
			// cbbNhaXe
			// 
			this.cbbNhaXe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbNhaXe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbNhaXe.FormattingEnabled = true;
			this.cbbNhaXe.Location = new System.Drawing.Point(200, 155);
			this.cbbNhaXe.Name = "cbbNhaXe";
			this.cbbNhaXe.Size = new System.Drawing.Size(360, 36);
			this.cbbNhaXe.TabIndex = 1;
			// 
			// cbbLoaiXe
			// 
			this.cbbLoaiXe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbLoaiXe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbLoaiXe.FormattingEnabled = true;
			this.cbbLoaiXe.Location = new System.Drawing.Point(200, 210);
			this.cbbLoaiXe.Name = "cbbLoaiXe";
			this.cbbLoaiXe.Size = new System.Drawing.Size(360, 36);
			this.cbbLoaiXe.TabIndex = 2;
			// 
			// cbbTuyen
			// 
			this.cbbTuyen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTuyen.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbTuyen.FormattingEnabled = true;
			this.cbbTuyen.Location = new System.Drawing.Point(200, 265);
			this.cbbTuyen.Name = "cbbTuyen";
			this.cbbTuyen.Size = new System.Drawing.Size(360, 36);
			this.cbbTuyen.TabIndex = 3;
			// 
			// numGiaVe
			// 
			this.numGiaVe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.numGiaVe.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.numGiaVe.Location = new System.Drawing.Point(200, 320);
			this.numGiaVe.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
			this.numGiaVe.Name = "numGiaVe";
			this.numGiaVe.Size = new System.Drawing.Size(220, 34);
			this.numGiaVe.TabIndex = 4;
			this.numGiaVe.ThousandsSeparator = true;
			// 
			// btnPrimary
			// 
			this.btnPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
			this.btnPrimary.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnPrimary.FlatAppearance.BorderSize = 0;
			this.btnPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPrimary.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnPrimary.ForeColor = System.Drawing.Color.White;
			this.btnPrimary.Location = new System.Drawing.Point(350, 385);
			this.btnPrimary.Name = "btnPrimary";
			this.btnPrimary.Size = new System.Drawing.Size(100, 45);
			this.btnPrimary.TabIndex = 5;
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
			this.btnCancel.Location = new System.Drawing.Point(460, 385);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(100, 45);
			this.btnCancel.TabIndex = 6;
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
			this.label1.Size = new System.Drawing.Size(101, 28);
			this.label1.TabIndex = 7;
			this.label1.Text = "Mã giá vé:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label2.Location = new System.Drawing.Point(60, 158);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(76, 28);
			this.label2.TabIndex = 8;
			this.label2.Text = "Nhà xe:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label3.Location = new System.Drawing.Point(60, 213);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(76, 28);
			this.label3.TabIndex = 9;
			this.label3.Text = "Loại xe:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label4.Location = new System.Drawing.Point(60, 268);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(132, 28);
			this.label4.TabIndex = 10;
			this.label4.Text = "Tuyến đường:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label5.Location = new System.Drawing.Point(60, 323);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(128, 28);
			this.label5.TabIndex = 11;
			this.label5.Text = "Giá vé (VNĐ):";
			// 
			// pnlHeader
			// 
			this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.pnlHeader.Controls.Add(this.lblHeader);
			this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlHeader.Location = new System.Drawing.Point(0, 0);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(630, 70);
			this.pnlHeader.TabIndex = 12;
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
			this.lblHeader.Text = "THÔNG TIN GIÁ VÉ";
			this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// GiaVeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(630, 460);
			this.Controls.Add(this.pnlHeader);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnPrimary);
			this.Controls.Add(this.numGiaVe);
			this.Controls.Add(this.cbbTuyen);
			this.Controls.Add(this.cbbLoaiXe);
			this.Controls.Add(this.cbbNhaXe);
			this.Controls.Add(this.txtMaGV);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GiaVeDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Quản lý giá vé";
			((System.ComponentModel.ISupportInitialize)(this.numGiaVe)).EndInit();
			this.pnlHeader.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtMaGV;
		private System.Windows.Forms.ComboBox cbbNhaXe;
		private System.Windows.Forms.ComboBox cbbLoaiXe;
		private System.Windows.Forms.ComboBox cbbTuyen;
		private System.Windows.Forms.NumericUpDown numGiaVe;
		private System.Windows.Forms.Button btnPrimary;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Label lblHeader;
	}
}
