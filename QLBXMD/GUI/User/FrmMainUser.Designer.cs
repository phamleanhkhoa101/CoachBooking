namespace QLBXMD.GUI.User
{
	partial class FrmMainUser
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
			this.pnlMenu = new System.Windows.Forms.Panel();
			this.btnDangXuat = new System.Windows.Forms.Button();
			this.btnThongTin = new System.Windows.Forms.Button();
			this.btnLichSu = new System.Windows.Forms.Button();
			this.btnDatVe = new System.Windows.Forms.Button();
			this.pnlLogo = new System.Windows.Forms.Panel();
			this.lblAppName = new System.Windows.Forms.Label();
			this.pnlContent = new System.Windows.Forms.Panel();
			this.pnlMenu.SuspendLayout();
			this.pnlLogo.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlMenu
			// 
			this.pnlMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
			this.pnlMenu.Controls.Add(this.btnDangXuat);
			this.pnlMenu.Controls.Add(this.btnThongTin);
			this.pnlMenu.Controls.Add(this.btnLichSu);
			this.pnlMenu.Controls.Add(this.btnDatVe);
			this.pnlMenu.Controls.Add(this.pnlLogo);
			this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlMenu.Location = new System.Drawing.Point(0, 0);
			this.pnlMenu.Name = "pnlMenu";
			this.pnlMenu.Size = new System.Drawing.Size(250, 800);
			this.pnlMenu.TabIndex = 0;
			// 
			// btnDangXuat
			// 
			this.btnDangXuat.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnDangXuat.FlatAppearance.BorderSize = 0;
			this.btnDangXuat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDangXuat.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.btnDangXuat.ForeColor = System.Drawing.Color.White;
			this.btnDangXuat.Location = new System.Drawing.Point(0, 740);
			this.btnDangXuat.Name = "btnDangXuat";
			this.btnDangXuat.Size = new System.Drawing.Size(250, 60);
			this.btnDangXuat.TabIndex = 4;
			this.btnDangXuat.Text = "🚪 Đăng xuất";
			this.btnDangXuat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnDangXuat.UseVisualStyleBackColor = true;
			this.btnDangXuat.Click += new System.EventHandler(this.btnDangXuat_Click);
			// 
			// btnThongTin
			// 
			this.btnThongTin.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnThongTin.FlatAppearance.BorderSize = 0;
			this.btnThongTin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnThongTin.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.btnThongTin.ForeColor = System.Drawing.Color.White;
			this.btnThongTin.Location = new System.Drawing.Point(0, 240);
			this.btnThongTin.Name = "btnThongTin";
			this.btnThongTin.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
			this.btnThongTin.Size = new System.Drawing.Size(250, 60);
			this.btnThongTin.TabIndex = 3;
			this.btnThongTin.Text = "👤 Thông tin cá nhân";
			this.btnThongTin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnThongTin.UseVisualStyleBackColor = true;
			this.btnThongTin.Click += new System.EventHandler(this.btnThongTin_Click);
			// 
			// btnLichSu
			// 
			this.btnLichSu.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnLichSu.FlatAppearance.BorderSize = 0;
			this.btnLichSu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLichSu.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.btnLichSu.ForeColor = System.Drawing.Color.White;
			this.btnLichSu.Location = new System.Drawing.Point(0, 180);
			this.btnLichSu.Name = "btnLichSu";
			this.btnLichSu.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
			this.btnLichSu.Size = new System.Drawing.Size(250, 60);
			this.btnLichSu.TabIndex = 2;
			this.btnLichSu.Text = "📜 Lịch sử đặt vé";
			this.btnLichSu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnLichSu.UseVisualStyleBackColor = true;
			this.btnLichSu.Click += new System.EventHandler(this.btnLichSu_Click);
			// 
			// btnDatVe
			// 
			this.btnDatVe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.btnDatVe.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnDatVe.FlatAppearance.BorderSize = 0;
			this.btnDatVe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDatVe.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.btnDatVe.ForeColor = System.Drawing.Color.White;
			this.btnDatVe.Location = new System.Drawing.Point(0, 120);
			this.btnDatVe.Name = "btnDatVe";
			this.btnDatVe.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
			this.btnDatVe.Size = new System.Drawing.Size(250, 60);
			this.btnDatVe.TabIndex = 1;
			this.btnDatVe.Text = "🎫 Đặt vé";
			this.btnDatVe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnDatVe.UseVisualStyleBackColor = false;
			this.btnDatVe.Click += new System.EventHandler(this.btnDatVe_Click);
			// 
			// pnlLogo
			// 
			this.pnlLogo.Controls.Add(this.lblAppName);
			this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlLogo.Location = new System.Drawing.Point(0, 0);
			this.pnlLogo.Name = "pnlLogo";
			this.pnlLogo.Size = new System.Drawing.Size(250, 120);
			this.pnlLogo.TabIndex = 0;
			// 
			// lblAppName
			// 
			this.lblAppName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblAppName.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lblAppName.ForeColor = System.Drawing.Color.White;
			this.lblAppName.Location = new System.Drawing.Point(0, 0);
			this.lblAppName.Name = "lblAppName";
			this.lblAppName.Size = new System.Drawing.Size(250, 120);
			this.lblAppName.TabIndex = 0;
			this.lblAppName.Text = "🚌 MY BUS";
			this.lblAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlContent
			// 
			this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlContent.Location = new System.Drawing.Point(250, 0);
			this.pnlContent.Name = "pnlContent";
			this.pnlContent.Size = new System.Drawing.Size(1126, 800);
			this.pnlContent.TabIndex = 1;
			// 
			// FrmMainUser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1376, 800);
			this.Controls.Add(this.pnlContent);
			this.Controls.Add(this.pnlMenu);
			this.Name = "FrmMainUser";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Hệ thống đặt vé xe - Khách hàng";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.pnlMenu.ResumeLayout(false);
			this.pnlLogo.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlMenu;
		private System.Windows.Forms.Panel pnlLogo;
		private System.Windows.Forms.Label lblAppName;
		private System.Windows.Forms.Button btnDatVe;
		private System.Windows.Forms.Button btnLichSu;
		private System.Windows.Forms.Button btnThongTin;
		private System.Windows.Forms.Button btnDangXuat;
		private System.Windows.Forms.Panel pnlContent;
	}
}