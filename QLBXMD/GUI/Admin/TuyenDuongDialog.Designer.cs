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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtDiemDi = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtDiemDen = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtQuangDuong = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtThoiGianDuKien = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.cbbTrangThai = new System.Windows.Forms.ComboBox();
			this.btnThem = new System.Windows.Forms.Button();
			this.btnHuy = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtMaTuyen
			// 
			this.txtMaTuyen.Enabled = false;
			this.txtMaTuyen.Location = new System.Drawing.Point(176, 30);
			this.txtMaTuyen.Name = "txtMaTuyen";
			this.txtMaTuyen.Size = new System.Drawing.Size(420, 26);
			this.txtMaTuyen.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(42, 33);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(74, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "Mã tuyến";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(42, 85);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(62, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "Điểm đi";
			// 
			// txtDiemDi
			// 
			this.txtDiemDi.Location = new System.Drawing.Point(176, 82);
			this.txtDiemDi.Name = "txtDiemDi";
			this.txtDiemDi.Size = new System.Drawing.Size(420, 26);
			this.txtDiemDi.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(42, 137);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(77, 20);
			this.label3.TabIndex = 5;
			this.label3.Text = "Điểm đến";
			// 
			// txtDiemDen
			// 
			this.txtDiemDen.Location = new System.Drawing.Point(176, 134);
			this.txtDiemDen.Name = "txtDiemDen";
			this.txtDiemDen.Size = new System.Drawing.Size(420, 26);
			this.txtDiemDen.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(42, 189);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(141, 20);
			this.label4.TabIndex = 7;
			this.label4.Text = "Quãng đường (km)";
			// 
			// txtQuangDuong
			// 
			this.txtQuangDuong.Location = new System.Drawing.Point(222, 189);
			this.txtQuangDuong.Name = "txtQuangDuong";
			this.txtQuangDuong.Size = new System.Drawing.Size(176, 26);
			this.txtQuangDuong.TabIndex = 6;
			this.txtQuangDuong.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumber_KeyPress);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(42, 241);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(174, 20);
			this.label5.TabIndex = 9;
			this.label5.Text = "Thời gian dự kiến (phút)";
			// 
			// txtThoiGianDuKien
			// 
			this.txtThoiGianDuKien.Location = new System.Drawing.Point(222, 241);
			this.txtThoiGianDuKien.Name = "txtThoiGianDuKien";
			this.txtThoiGianDuKien.Size = new System.Drawing.Size(176, 26);
			this.txtThoiGianDuKien.TabIndex = 8;
			this.txtThoiGianDuKien.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNumber_KeyPress);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(42, 293);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 20);
			this.label6.TabIndex = 11;
			this.label6.Text = "Trạng thái";
			// 
			// cbbTrangThai
			// 
			this.cbbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTrangThai.FormattingEnabled = true;
			this.cbbTrangThai.Items.AddRange(new object[] {
            "Hoạt động",
            "Ngừng hoạt động"});
			this.cbbTrangThai.Location = new System.Drawing.Point(222, 285);
			this.cbbTrangThai.Name = "cbbTrangThai";
			this.cbbTrangThai.Size = new System.Drawing.Size(176, 28);
			this.cbbTrangThai.TabIndex = 10;
			// 
			// btnThem
			// 
			this.btnThem.Location = new System.Drawing.Point(209, 352);
			this.btnThem.Name = "btnThem";
			this.btnThem.Size = new System.Drawing.Size(103, 44);
			this.btnThem.TabIndex = 12;
			this.btnThem.Text = "Lưu";
			this.btnThem.UseVisualStyleBackColor = true;
			this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
			// 
			// btnHuy
			// 
			this.btnHuy.Location = new System.Drawing.Point(359, 352);
			this.btnHuy.Name = "btnHuy";
			this.btnHuy.Size = new System.Drawing.Size(103, 44);
			this.btnHuy.TabIndex = 13;
			this.btnHuy.Text = "Hủy";
			this.btnHuy.UseVisualStyleBackColor = true;
			this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
			// 
			// TuyenDuongDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(640, 420);
			this.Controls.Add(this.btnHuy);
			this.Controls.Add(this.btnThem);
			this.Controls.Add(this.cbbTrangThai);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtThoiGianDuKien);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtQuangDuong);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtDiemDen);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtDiemDi);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtMaTuyen);
			this.Name = "TuyenDuongDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Tuyến đường";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtMaTuyen;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtDiemDi;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtDiemDen;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtQuangDuong;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtThoiGianDuKien;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox cbbTrangThai;
		private System.Windows.Forms.Button btnThem;
		private System.Windows.Forms.Button btnHuy;
	}
}