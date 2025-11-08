namespace QLBXMD.GUI.Admin
{
	partial class GanTaiKhoanDialog
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
			this.lblTaiKhoan = new System.Windows.Forms.Label();
			this.cbbTaiKhoan = new System.Windows.Forms.ComboBox();
			this.btnChon = new System.Windows.Forms.Button();
			this.btnHuy = new System.Windows.Forms.Button();
			this.lblThongTin = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblTaiKhoan
			// 
			this.lblTaiKhoan.AutoSize = true;
			this.lblTaiKhoan.Location = new System.Drawing.Point(30, 70);
			this.lblTaiKhoan.Name = "lblTaiKhoan";
			this.lblTaiKhoan.Size = new System.Drawing.Size(131, 20);
			this.lblTaiKhoan.TabIndex = 0;
			this.lblTaiKhoan.Text = "Ch?n tài kho?n:";
			// 
			// cbbTaiKhoan
			// 
			this.cbbTaiKhoan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTaiKhoan.FormattingEnabled = true;
			this.cbbTaiKhoan.Location = new System.Drawing.Point(34, 93);
			this.cbbTaiKhoan.Name = "cbbTaiKhoan";
			this.cbbTaiKhoan.Size = new System.Drawing.Size(450, 28);
			this.cbbTaiKhoan.TabIndex = 1;
			// 
			// btnChon
			// 
			this.btnChon.Location = new System.Drawing.Point(260, 150);
			this.btnChon.Name = "btnChon";
			this.btnChon.Size = new System.Drawing.Size(100, 35);
			this.btnChon.TabIndex = 2;
			this.btnChon.Text = "Ch?n";
			this.btnChon.UseVisualStyleBackColor = true;
			this.btnChon.Click += new System.EventHandler(this.btnChon_Click);
			// 
			// btnHuy
			// 
			this.btnHuy.Location = new System.Drawing.Point(384, 150);
			this.btnHuy.Name = "btnHuy";
			this.btnHuy.Size = new System.Drawing.Size(100, 35);
			this.btnHuy.TabIndex = 3;
			this.btnHuy.Text = "H?y";
			this.btnHuy.UseVisualStyleBackColor = true;
			this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
			// 
			// lblThongTin
			// 
			this.lblThongTin.AutoSize = true;
			this.lblThongTin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblThongTin.ForeColor = System.Drawing.Color.Gray;
			this.lblThongTin.Location = new System.Drawing.Point(31, 20);
			this.lblThongTin.Name = "lblThongTin";
			this.lblThongTin.Size = new System.Drawing.Size(411, 22);
			this.lblThongTin.TabIndex = 4;
			this.lblThongTin.Text = "Ch?n tài kho?n ?? g?n (ch? hi?n th? tài kho?n ch?a g?n)";
			// 
			// GanTaiKhoanDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(518, 211);
			this.Controls.Add(this.lblThongTin);
			this.Controls.Add(this.btnHuy);
			this.Controls.Add(this.btnChon);
			this.Controls.Add(this.cbbTaiKhoan);
			this.Controls.Add(this.lblTaiKhoan);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GanTaiKhoanDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "G?n tài kho?n";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblTaiKhoan;
		private System.Windows.Forms.ComboBox cbbTaiKhoan;
		private System.Windows.Forms.Button btnChon;
		private System.Windows.Forms.Button btnHuy;
		private System.Windows.Forms.Label lblThongTin;
	}
}
