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
			this.btnHuy = new System.Windows.Forms.Button();
			this.btnThem = new System.Windows.Forms.Button();
			this.cbbTrangThai = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtBienSoXe = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtMaXe = new System.Windows.Forms.TextBox();
			this.cbbNhaXe = new System.Windows.Forms.ComboBox();
			this.cbbLoaiXe = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtSoCho = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(83, 191);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(58, 20);
			this.label3.TabIndex = 34;
			this.label3.Text = "Nhà xe";
			// 
			// btnHuy
			// 
			this.btnHuy.Location = new System.Drawing.Point(444, 454);
			this.btnHuy.Name = "btnHuy";
			this.btnHuy.Size = new System.Drawing.Size(103, 44);
			this.btnHuy.TabIndex = 32;
			this.btnHuy.Text = "Hủy";
			this.btnHuy.UseVisualStyleBackColor = true;
			this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
			// 
			// btnThem
			// 
			this.btnThem.Location = new System.Drawing.Point(250, 454);
			this.btnThem.Name = "btnThem";
			this.btnThem.Size = new System.Drawing.Size(103, 44);
			this.btnThem.TabIndex = 31;
			this.btnThem.Text = "Thêm";
			this.btnThem.UseVisualStyleBackColor = true;
			this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
			// 
			// cbbTrangThai
			// 
			this.cbbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTrangThai.FormattingEnabled = true;
			this.cbbTrangThai.Items.AddRange(new object[] {
            "Hoạt động",
            "Ngừng hoạt động"});
			this.cbbTrangThai.Location = new System.Drawing.Point(191, 379);
			this.cbbTrangThai.Name = "cbbTrangThai";
			this.cbbTrangThai.Size = new System.Drawing.Size(176, 28);
			this.cbbTrangThai.TabIndex = 30;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(81, 379);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 20);
			this.label5.TabIndex = 29;
			this.label5.Text = "Trạng thái";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(83, 129);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(82, 20);
			this.label2.TabIndex = 28;
			this.label2.Text = "Biển số xe";
			// 
			// txtBienSoXe
			// 
			this.txtBienSoXe.Location = new System.Drawing.Point(193, 126);
			this.txtBienSoXe.Name = "txtBienSoXe";
			this.txtBienSoXe.Size = new System.Drawing.Size(447, 26);
			this.txtBienSoXe.TabIndex = 27;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(83, 65);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 20);
			this.label1.TabIndex = 26;
			this.label1.Text = "Mã xe";
			// 
			// txtMaXe
			// 
			this.txtMaXe.Enabled = false;
			this.txtMaXe.Location = new System.Drawing.Point(193, 62);
			this.txtMaXe.Name = "txtMaXe";
			this.txtMaXe.Size = new System.Drawing.Size(447, 26);
			this.txtMaXe.TabIndex = 25;
			// 
			// cbbNhaXe
			// 
			this.cbbNhaXe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbNhaXe.FormattingEnabled = true;
			this.cbbNhaXe.Items.AddRange(new object[] {
            "Hoạt động",
            "Ngừng hoạt động"});
			this.cbbNhaXe.Location = new System.Drawing.Point(193, 188);
			this.cbbNhaXe.Name = "cbbNhaXe";
			this.cbbNhaXe.Size = new System.Drawing.Size(447, 28);
			this.cbbNhaXe.TabIndex = 35;
			// 
			// cbbLoaiXe
			// 
			this.cbbLoaiXe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbLoaiXe.FormattingEnabled = true;
			this.cbbLoaiXe.Items.AddRange(new object[] {
            "Hoạt động",
            "Ngừng hoạt động"});
			this.cbbLoaiXe.Location = new System.Drawing.Point(193, 250);
			this.cbbLoaiXe.Name = "cbbLoaiXe";
			this.cbbLoaiXe.Size = new System.Drawing.Size(447, 28);
			this.cbbLoaiXe.TabIndex = 37;
			this.cbbLoaiXe.SelectedIndexChanged += new System.EventHandler(this.cbbLoaiXe_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(83, 253);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(59, 20);
			this.label4.TabIndex = 36;
			this.label4.Text = "Loại xe";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(80, 318);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(59, 20);
			this.label6.TabIndex = 39;
			this.label6.Text = "Số chỗ";
			// 
			// txtSoCho
			// 
			this.txtSoCho.Location = new System.Drawing.Point(190, 315);
			this.txtSoCho.Name = "txtSoCho";
			this.txtSoCho.Size = new System.Drawing.Size(176, 26);
			this.txtSoCho.TabIndex = 38;
			// 
			// XeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(698, 521);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtSoCho);
			this.Controls.Add(this.cbbLoaiXe);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cbbNhaXe);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnHuy);
			this.Controls.Add(this.btnThem);
			this.Controls.Add(this.cbbTrangThai);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtBienSoXe);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtMaXe);
			this.Name = "XeDialog";
			this.Text = "XeDialog";
			this.Load += new System.EventHandler(this.XeDialog_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnHuy;
		private System.Windows.Forms.Button btnThem;
		private System.Windows.Forms.ComboBox cbbTrangThai;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtBienSoXe;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtMaXe;
		private System.Windows.Forms.ComboBox cbbNhaXe;
		private System.Windows.Forms.ComboBox cbbLoaiXe;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtSoCho;
	}
}