namespace QLBXMD.GUI.Admin
{
	partial class XeDialog
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
			this.label3 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnPrimary = new System.Windows.Forms.Button();
			this.cbbTrangThai = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtBienSoXe = new System.Windows.Forms.TextBox();
			this.lblMaXe = new System.Windows.Forms.Label();
			this.txtMaXe = new System.Windows.Forms.TextBox();
			this.cbbNhaXe = new System.Windows.Forms.ComboBox();
			this.cbbLoaiXe = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtSoCho = new System.Windows.Forms.TextBox();
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.lblHeader = new System.Windows.Forms.Label();
			this.pnlHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label3.Location = new System.Drawing.Point(60, 210);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(74, 28);
			this.label3.TabIndex = 34;
			this.label3.Text = "Nhà xe:";
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
			this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnCancel.FlatAppearance.BorderSize = 0;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnCancel.ForeColor = System.Drawing.Color.White;
			this.btnCancel.Location = new System.Drawing.Point(460, 475);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(100, 45);
			this.btnCancel.TabIndex = 32;
			this.btnCancel.Text = "Hủy";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnHuy_Click);
			// 
			// btnPrimary
			// 
			this.btnPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
			this.btnPrimary.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnPrimary.FlatAppearance.BorderSize = 0;
			this.btnPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPrimary.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnPrimary.ForeColor = System.Drawing.Color.White;
			this.btnPrimary.Location = new System.Drawing.Point(350, 475);
			this.btnPrimary.Name = "btnPrimary";
			this.btnPrimary.Size = new System.Drawing.Size(100, 45);
			this.btnPrimary.TabIndex = 31;
			this.btnPrimary.Text = "Thêm";
			this.btnPrimary.UseVisualStyleBackColor = false;
			this.btnPrimary.Click += new System.EventHandler(this.btnThem_Click);
			// 
			// cbbTrangThai
			// 
			this.cbbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbTrangThai.FormattingEnabled = true;
			this.cbbTrangThai.Items.AddRange(new object[] {
            "Hoạt động",
            "Ngừng hoạt động"});
			this.cbbTrangThai.Location = new System.Drawing.Point(220, 407);
			this.cbbTrangThai.Name = "cbbTrangThai";
			this.cbbTrangThai.Size = new System.Drawing.Size(250, 36);
			this.cbbTrangThai.TabIndex = 30;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label5.Location = new System.Drawing.Point(60, 410);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(102, 28);
			this.label5.TabIndex = 29;
			this.label5.Text = "Trạng thái:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label2.Location = new System.Drawing.Point(60, 155);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(103, 28);
			this.label2.TabIndex = 28;
			this.label2.Text = "Biển số xe:";
			// 
			// txtBienSoXe
			// 
			this.txtBienSoXe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtBienSoXe.Location = new System.Drawing.Point(220, 152);
			this.txtBienSoXe.MaxLength = 15;
			this.txtBienSoXe.Name = "txtBienSoXe";
			this.txtBienSoXe.Size = new System.Drawing.Size(340, 34);
			this.txtBienSoXe.TabIndex = 27;
			// 
			// lblMaXe
			// 
			this.lblMaXe.AutoSize = true;
			this.lblMaXe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblMaXe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lblMaXe.Location = new System.Drawing.Point(60, 100);
			this.lblMaXe.Name = "lblMaXe";
			this.lblMaXe.Size = new System.Drawing.Size(67, 28);
			this.lblMaXe.TabIndex = 26;
			this.lblMaXe.Text = "Mã xe:";
			// 
			// txtMaXe
			// 
			this.txtMaXe.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtMaXe.Enabled = false;
			this.txtMaXe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtMaXe.Location = new System.Drawing.Point(220, 97);
			this.txtMaXe.Name = "txtMaXe";
			this.txtMaXe.ReadOnly = true;
			this.txtMaXe.Size = new System.Drawing.Size(340, 34);
			this.txtMaXe.TabIndex = 25;
			// 
			// cbbNhaXe
			// 
			this.cbbNhaXe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbNhaXe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbNhaXe.FormattingEnabled = true;
			this.cbbNhaXe.Location = new System.Drawing.Point(220, 207);
			this.cbbNhaXe.Name = "cbbNhaXe";
			this.cbbNhaXe.Size = new System.Drawing.Size(340, 36);
			this.cbbNhaXe.TabIndex = 35;
			// 
			// cbbLoaiXe
			// 
			this.cbbLoaiXe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbLoaiXe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbLoaiXe.FormattingEnabled = true;
			this.cbbLoaiXe.Location = new System.Drawing.Point(220, 262);
			this.cbbLoaiXe.Name = "cbbLoaiXe";
			this.cbbLoaiXe.Size = new System.Drawing.Size(340, 36);
			this.cbbLoaiXe.TabIndex = 37;
			this.cbbLoaiXe.SelectedIndexChanged += new System.EventHandler(this.cbbLoaiXe_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label4.Location = new System.Drawing.Point(60, 265);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(78, 28);
			this.label4.TabIndex = 36;
			this.label4.Text = "Loại xe:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label6.Location = new System.Drawing.Point(60, 320);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(76, 28);
			this.label6.TabIndex = 39;
			this.label6.Text = "Số chỗ:";
			// 
			// txtSoCho
			// 
			this.txtSoCho.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtSoCho.Enabled = false;
			this.txtSoCho.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtSoCho.Location = new System.Drawing.Point(220, 317);
			this.txtSoCho.Name = "txtSoCho";
			this.txtSoCho.ReadOnly = true;
			this.txtSoCho.Size = new System.Drawing.Size(150, 34);
			this.txtSoCho.TabIndex = 38;
			// 
			// pnlHeader
			// 
			this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.pnlHeader.Controls.Add(this.lblHeader);
			this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlHeader.Location = new System.Drawing.Point(0, 0);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(630, 70);
			this.pnlHeader.TabIndex = 40;
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
			this.lblHeader.Text = "THÔNG TIN XE";
			this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// XeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(630, 550);
			this.Controls.Add(this.pnlHeader);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtSoCho);
			this.Controls.Add(this.cbbLoaiXe);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cbbNhaXe);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnPrimary);
			this.Controls.Add(this.cbbTrangThai);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtBienSoXe);
			this.Controls.Add(this.lblMaXe);
			this.Controls.Add(this.txtMaXe);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "XeDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Quản lý xe";
			this.Load += new System.EventHandler(this.XeDialog_Load);
			this.pnlHeader.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnPrimary;
		private System.Windows.Forms.ComboBox cbbTrangThai;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtBienSoXe;
		private System.Windows.Forms.Label lblMaXe;
		private System.Windows.Forms.TextBox txtMaXe;
		private System.Windows.Forms.ComboBox cbbNhaXe;
		private System.Windows.Forms.ComboBox cbbLoaiXe;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtSoCho;
		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Label lblHeader;
	}
}