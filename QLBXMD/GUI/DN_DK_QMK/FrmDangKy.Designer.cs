namespace QLBXMD.GUI
{
	partial class FrmDangKy
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDangKy));
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.txtMatKhau = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtTenDangNhap = new System.Windows.Forms.TextBox();
			this.lblEmail = new System.Windows.Forms.Label();
			this.txtEmail = new System.Windows.Forms.TextBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtMatKhau2 = new System.Windows.Forms.TextBox();
			this.btnDangNhap = new System.Windows.Forms.Button();
			this.linkDangKy = new System.Windows.Forms.LinkLabel();
			this.lblTitle = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.lblTitle);
			resources.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			// 
			// panel2
			// 
			resources.ApplyResources(this.panel2, "panel2");
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.txtMatKhau);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Controls.Add(this.txtTenDangNhap);
			this.panel2.Controls.Add(this.lblEmail);
			this.panel2.Controls.Add(this.txtEmail);
			this.panel2.Controls.Add(this.lblPassword);
			this.panel2.Controls.Add(this.txtMatKhau2);
			this.panel2.Controls.Add(this.btnDangNhap);
			this.panel2.Controls.Add(this.linkDangKy);
			this.panel2.Name = "panel2";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// txtMatKhau
			// 
			resources.ApplyResources(this.txtMatKhau, "txtMatKhau");
			this.txtMatKhau.Name = "txtMatKhau";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// txtTenDangNhap
			// 
			resources.ApplyResources(this.txtTenDangNhap, "txtTenDangNhap");
			this.txtTenDangNhap.Name = "txtTenDangNhap";
			this.txtTenDangNhap.Tag = "";
			// 
			// lblEmail
			// 
			resources.ApplyResources(this.lblEmail, "lblEmail");
			this.lblEmail.Name = "lblEmail";
			// 
			// txtEmail
			// 
			resources.ApplyResources(this.txtEmail, "txtEmail");
			this.txtEmail.Name = "txtEmail";
			// 
			// lblPassword
			// 
			resources.ApplyResources(this.lblPassword, "lblPassword");
			this.lblPassword.Name = "lblPassword";
			// 
			// txtMatKhau2
			// 
			resources.ApplyResources(this.txtMatKhau2, "txtMatKhau2");
			this.txtMatKhau2.Name = "txtMatKhau2";
			// 
			// btnDangNhap
			// 
			this.btnDangNhap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(103)))), ((int)(((byte)(178)))));
			resources.ApplyResources(this.btnDangNhap, "btnDangNhap");
			this.btnDangNhap.ForeColor = System.Drawing.Color.White;
			this.btnDangNhap.Name = "btnDangNhap";
			this.btnDangNhap.UseVisualStyleBackColor = false;
			this.btnDangNhap.Click += new System.EventHandler(this.btnDangNhap_Click);
			// 
			// linkDangKy
			// 
			resources.ApplyResources(this.linkDangKy, "linkDangKy");
			this.linkDangKy.LinkColor = System.Drawing.Color.Black;
			this.linkDangKy.Name = "linkDangKy";
			this.linkDangKy.TabStop = true;
			this.linkDangKy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDangKy_LinkClicked);
			// 
			// lblTitle
			// 
			resources.ApplyResources(this.lblTitle, "lblTitle");
			this.lblTitle.Name = "lblTitle";
			// 
			// pictureBox1
			// 
			resources.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Image = global::QLBXMD.Properties.Resources.bus;
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			// 
			// FrmDangKy
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.pictureBox1);
			this.Name = "FrmDangKy";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label lblEmail;
		private System.Windows.Forms.TextBox txtEmail;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox txtMatKhau2;
		private System.Windows.Forms.Button btnDangNhap;
		private System.Windows.Forms.LinkLabel linkDangKy;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtMatKhau;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtTenDangNhap;
	}
}