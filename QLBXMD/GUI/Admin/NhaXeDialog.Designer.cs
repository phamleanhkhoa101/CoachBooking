namespace QLBXMD.GUI.Admin
{
	partial class NhaXeDialog
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
			this.txtMaNhaXe = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTenNhaXe = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtSDT = new System.Windows.Forms.TextBox();
			this.txtEmail = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.cbbTrangThai = new System.Windows.Forms.ComboBox();
			this.btnThem = new System.Windows.Forms.Button();
			this.btnHuy = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtMaNhaXe
			// 
			this.txtMaNhaXe.Enabled = false;
			this.txtMaNhaXe.Location = new System.Drawing.Point(143, 60);
			this.txtMaNhaXe.Name = "txtMaNhaXe";
			this.txtMaNhaXe.Size = new System.Drawing.Size(447, 26);
			this.txtMaNhaXe.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(33, 63);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "Mã nhà xe";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(33, 127);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(87, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "Tên nhà xe";
			// 
			// txtTenNhaXe
			// 
			this.txtTenNhaXe.Location = new System.Drawing.Point(143, 124);
			this.txtTenNhaXe.Name = "txtTenNhaXe";
			this.txtTenNhaXe.Size = new System.Drawing.Size(447, 26);
			this.txtTenNhaXe.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(33, 254);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(102, 20);
			this.label3.TabIndex = 5;
			this.label3.Text = "Số điện thoại";
			// 
			// txtSDT
			// 
			this.txtSDT.Location = new System.Drawing.Point(143, 251);
			this.txtSDT.Name = "txtSDT";
			this.txtSDT.Size = new System.Drawing.Size(447, 26);
			this.txtSDT.TabIndex = 4;
			this.txtSDT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSDT_KeyPress);
			// 
			// txtEmail
			// 
			this.txtEmail.Location = new System.Drawing.Point(143, 184);
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.Size = new System.Drawing.Size(447, 26);
			this.txtEmail.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(33, 187);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 20);
			this.label4.TabIndex = 5;
			this.label4.Text = "Email";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(33, 317);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 20);
			this.label5.TabIndex = 7;
			this.label5.Text = "Trạng thái";
			// 
			// cbbTrangThai
			// 
			this.cbbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTrangThai.FormattingEnabled = true;
			this.cbbTrangThai.Items.AddRange(new object[] {
            "Hoạt động",
            "Ngừng hoạt động"});
			this.cbbTrangThai.Location = new System.Drawing.Point(143, 317);
			this.cbbTrangThai.Name = "cbbTrangThai";
			this.cbbTrangThai.Size = new System.Drawing.Size(176, 28);
			this.cbbTrangThai.TabIndex = 8;
			// 
			// btnThem
			// 
			this.btnThem.Location = new System.Drawing.Point(206, 402);
			this.btnThem.Name = "btnThem";
			this.btnThem.Size = new System.Drawing.Size(103, 44);
			this.btnThem.TabIndex = 9;
			this.btnThem.Text = "Thêm";
			this.btnThem.UseVisualStyleBackColor = true;
			this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
			// 
			// btnHuy
			// 
			this.btnHuy.Location = new System.Drawing.Point(400, 402);
			this.btnHuy.Name = "btnHuy";
			this.btnHuy.Size = new System.Drawing.Size(103, 44);
			this.btnHuy.TabIndex = 10;
			this.btnHuy.Text = "Hủy";
			this.btnHuy.UseVisualStyleBackColor = true;
			this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
			// 
			// NhaXeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(698, 497);
			this.Controls.Add(this.btnHuy);
			this.Controls.Add(this.btnThem);
			this.Controls.Add(this.cbbTrangThai);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtEmail);
			this.Controls.Add(this.txtSDT);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtTenNhaXe);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtMaNhaXe);
			this.Name = "NhaXeDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "NhaXeDialog";
			this.Load += new System.EventHandler(this.NhaXeDialog_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtMaNhaXe;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtTenNhaXe;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtSDT;
		private System.Windows.Forms.TextBox txtEmail;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox cbbTrangThai;
		private System.Windows.Forms.Button btnThem;
		private System.Windows.Forms.Button btnHuy;
	}
}