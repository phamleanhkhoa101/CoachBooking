namespace QLBXMD.GUI.DN_DK_QMK
{
	partial class FrmDangNhap
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.label2 = new System.Windows.Forms.Label();
			this.txtMatKhau = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnDangNhap = new System.Windows.Forms.Button();
			this.linkDangKy = new System.Windows.Forms.LinkLabel();
			this.lblTitle = new System.Windows.Forms.Label();
			this.txtTenDangNhap = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.pictureBox1.ErrorImage = global::QLBXMD.Properties.Resources.bus;
			this.pictureBox1.Image = global::QLBXMD.Properties.Resources.bus;
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(398, 574);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.lblTitle);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(398, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(540, 574);
			this.panel1.TabIndex = 1;
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.Controls.Add(this.linkLabel1);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.txtMatKhau);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Controls.Add(this.txtTenDangNhap);
			this.panel2.Controls.Add(this.btnDangNhap);
			this.panel2.Controls.Add(this.linkDangKy);
			this.panel2.Location = new System.Drawing.Point(14, 100);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(514, 471);
			this.panel2.TabIndex = 11;
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.linkLabel1.LinkColor = System.Drawing.Color.Blue;
			this.linkLabel1.Location = new System.Drawing.Point(321, 214);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(131, 20);
			this.linkLabel1.TabIndex = 13;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Quên mật khẩu ?";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label2.Location = new System.Drawing.Point(64, 137);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 20);
			this.label2.TabIndex = 11;
			this.label2.Text = "Mật khẩu:";
			// 
			// txtMatKhau
			// 
			this.txtMatKhau.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtMatKhau.Location = new System.Drawing.Point(64, 160);
			this.txtMatKhau.Name = "txtMatKhau";
			this.txtMatKhau.PasswordChar = '*';
			this.txtMatKhau.Size = new System.Drawing.Size(388, 30);
			this.txtMatKhau.TabIndex = 12;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(64, 66);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 20);
			this.label1.TabIndex = 9;
			this.label1.Text = "Tài khoản:";
			// 
			// btnDangNhap
			// 
			this.btnDangNhap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(103)))), ((int)(((byte)(178)))));
			this.btnDangNhap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDangNhap.ForeColor = System.Drawing.Color.White;
			this.btnDangNhap.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.btnDangNhap.Location = new System.Drawing.Point(117, 279);
			this.btnDangNhap.Name = "btnDangNhap";
			this.btnDangNhap.Size = new System.Drawing.Size(300, 49);
			this.btnDangNhap.TabIndex = 6;
			this.btnDangNhap.Text = "Đăng nhập\r\n";
			this.btnDangNhap.UseVisualStyleBackColor = false;
			this.btnDangNhap.Click += new System.EventHandler(this.btnDangNhap_Click);
			// 
			// linkDangKy
			// 
			this.linkDangKy.AutoSize = true;
			this.linkDangKy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.linkDangKy.LinkColor = System.Drawing.Color.Black;
			this.linkDangKy.Location = new System.Drawing.Point(188, 354);
			this.linkDangKy.Name = "linkDangKy";
			this.linkDangKy.Size = new System.Drawing.Size(180, 20);
			this.linkDangKy.TabIndex = 8;
			this.linkDangKy.TabStop = true;
			this.linkDangKy.Text = "Bạn chưa có tài khoản ?";
			this.linkDangKy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDangKy_LinkClicked);
			// 
			// lblTitle
			// 
			this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lblTitle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblTitle.Location = new System.Drawing.Point(199, 50);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(161, 38);
			this.lblTitle.TabIndex = 3;
			this.lblTitle.Text = "Đăng nhập";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// txtTenDangNhap
			// 
			this.txtTenDangNhap.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTenDangNhap.Location = new System.Drawing.Point(64, 89);
			this.txtTenDangNhap.Name = "txtTenDangNhap";
			this.txtTenDangNhap.Size = new System.Drawing.Size(388, 30);
			this.txtTenDangNhap.TabIndex = 10;
			this.txtTenDangNhap.Tag = "";
			// 
			// FrmDangNhap
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.ClientSize = new System.Drawing.Size(938, 574);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.pictureBox1);
			this.Name = "FrmDangNhap";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtMatKhau;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnDangNhap;
		private System.Windows.Forms.LinkLabel linkDangKy;
		private System.Windows.Forms.TextBox txtTenDangNhap;
	}
}