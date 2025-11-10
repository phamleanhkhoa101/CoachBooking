namespace QLBXMD.GUI.User
{
	partial class UCLichSuMuaVe
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.flowMain = new System.Windows.Forms.FlowLayoutPanel();
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.btnLamMoi = new System.Windows.Forms.Button();
			this.lblTitle = new System.Windows.Forms.Label();
			this.pnlContent = new System.Windows.Forms.Panel();
			this.flpHoaDon = new System.Windows.Forms.FlowLayoutPanel();
			this.lblThongBao = new System.Windows.Forms.Label();
			this.flowMain.SuspendLayout();
			this.pnlHeader.SuspendLayout();
			this.pnlContent.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowMain
			// 
			this.flowMain.AutoScroll = true;
			this.flowMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.flowMain.Controls.Add(this.pnlHeader);
			this.flowMain.Controls.Add(this.pnlContent);
			this.flowMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowMain.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowMain.Location = new System.Drawing.Point(0, 0);
			this.flowMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.flowMain.Name = "flowMain";
			this.flowMain.Padding = new System.Windows.Forms.Padding(15, 20, 15, 20);
			this.flowMain.Size = new System.Drawing.Size(1650, 1538);
			this.flowMain.TabIndex = 0;
			this.flowMain.WrapContents = false;
			// 
			// pnlHeader
			// 
			this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.pnlHeader.Controls.Add(this.btnLamMoi);
			this.pnlHeader.Controls.Add(this.lblTitle);
			this.pnlHeader.Location = new System.Drawing.Point(19, 25);
			this.pnlHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 20);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(1598, 100);
			this.pnlHeader.TabIndex = 0;
			// 
			// btnLamMoi
			// 
			this.btnLamMoi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
			this.btnLamMoi.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnLamMoi.FlatAppearance.BorderSize = 0;
			this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnLamMoi.ForeColor = System.Drawing.Color.White;
			this.btnLamMoi.Location = new System.Drawing.Point(1428, 30);
			this.btnLamMoi.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnLamMoi.Name = "btnLamMoi";
			this.btnLamMoi.Size = new System.Drawing.Size(150, 45);
			this.btnLamMoi.TabIndex = 1;
			this.btnLamMoi.Text = "🔄 Làm mới";
			this.btnLamMoi.UseVisualStyleBackColor = false;
			this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
			// 
			// lblTitle
			// 
			this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(1594, 100);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "LỊCH SỬ MUA VÉ";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlContent
			// 
			this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlContent.BackColor = System.Drawing.Color.White;
			this.pnlContent.Controls.Add(this.flpHoaDon);
			this.pnlContent.Controls.Add(this.lblThongBao);
			this.pnlContent.Location = new System.Drawing.Point(19, 150);
			this.pnlContent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 20);
			this.pnlContent.MinimumSize = new System.Drawing.Size(1080, 600);
			this.pnlContent.Name = "pnlContent";
			this.pnlContent.Padding = new System.Windows.Forms.Padding(30, 25, 30, 25);
			this.pnlContent.Size = new System.Drawing.Size(1598, 1200);
			this.pnlContent.TabIndex = 1;
			// 
			// flpHoaDon
			// 
			this.flpHoaDon.AutoScroll = true;
			this.flpHoaDon.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpHoaDon.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flpHoaDon.Location = new System.Drawing.Point(30, 25);
			this.flpHoaDon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.flpHoaDon.Name = "flpHoaDon";
			this.flpHoaDon.Size = new System.Drawing.Size(1538, 1150);
			this.flpHoaDon.TabIndex = 1;
			this.flpHoaDon.WrapContents = false;
			// 
			// lblThongBao
			// 
			this.lblThongBao.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblThongBao.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic);
			this.lblThongBao.ForeColor = System.Drawing.Color.Gray;
			this.lblThongBao.Location = new System.Drawing.Point(30, 25);
			this.lblThongBao.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblThongBao.Name = "lblThongBao";
			this.lblThongBao.Size = new System.Drawing.Size(1538, 1150);
			this.lblThongBao.TabIndex = 0;
			this.lblThongBao.Text = "Bạn chưa có lịch sử mua vé nào.";
			this.lblThongBao.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblThongBao.Visible = false;
			// 
			// UCLichSuMuaVe
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.flowMain);
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "UCLichSuMuaVe";
			this.Size = new System.Drawing.Size(1650, 1538);
			this.flowMain.ResumeLayout(false);
			this.pnlHeader.ResumeLayout(false);
			this.pnlContent.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowMain;
		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Panel pnlContent;
		private System.Windows.Forms.FlowLayoutPanel flpHoaDon;
		private System.Windows.Forms.Label lblThongBao;
		private System.Windows.Forms.Button btnLamMoi;
	}
}
