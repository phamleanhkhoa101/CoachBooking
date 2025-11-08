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
			((System.ComponentModel.ISupportInitialize)(this.numGiaVe)).BeginInit();
			this.SuspendLayout();
			// 
			// txtMaGV
			// 
			this.txtMaGV.BackColor = System.Drawing.Color.LightGray;
			this.txtMaGV.Location = new System.Drawing.Point(160, 16);
			this.txtMaGV.Name = "txtMaGV";
			this.txtMaGV.ReadOnly = true;
			this.txtMaGV.Size = new System.Drawing.Size(360, 26);
			this.txtMaGV.TabIndex = 0;
			// 
			// cbbNhaXe
			// 
			this.cbbNhaXe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbNhaXe.FormattingEnabled = true;
			this.cbbNhaXe.Location = new System.Drawing.Point(160, 61);
			this.cbbNhaXe.Name = "cbbNhaXe";
			this.cbbNhaXe.Size = new System.Drawing.Size(360, 28);
			this.cbbNhaXe.TabIndex = 1;
			// 
			// cbbLoaiXe
			// 
			this.cbbLoaiXe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbLoaiXe.FormattingEnabled = true;
			this.cbbLoaiXe.Location = new System.Drawing.Point(160, 106);
			this.cbbLoaiXe.Name = "cbbLoaiXe";
			this.cbbLoaiXe.Size = new System.Drawing.Size(360, 28);
			this.cbbLoaiXe.TabIndex = 2;
			// 
			// cbbTuyen
			// 
			this.cbbTuyen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTuyen.FormattingEnabled = true;
			this.cbbTuyen.Location = new System.Drawing.Point(160, 153);
			this.cbbTuyen.Name = "cbbTuyen";
			this.cbbTuyen.Size = new System.Drawing.Size(360, 28);
			this.cbbTuyen.TabIndex = 3;
			// 
			// numGiaVe
			// 
			this.numGiaVe.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numGiaVe.Location = new System.Drawing.Point(160, 200);
			this.numGiaVe.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
			this.numGiaVe.Name = "numGiaVe";
			this.numGiaVe.Size = new System.Drawing.Size(220, 26);
			this.numGiaVe.TabIndex = 4;
			this.numGiaVe.ThousandsSeparator = true;
			// 
			// btnPrimary
			// 
			this.btnPrimary.Location = new System.Drawing.Point(330, 304);
			this.btnPrimary.Name = "btnPrimary";
			this.btnPrimary.Size = new System.Drawing.Size(90, 35);
			this.btnPrimary.TabIndex = 5;
			this.btnPrimary.Text = "L?u";
			this.btnPrimary.UseVisualStyleBackColor = true;
			this.btnPrimary.Click += new System.EventHandler(this.btnPrimary_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(430, 304);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(90, 35);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "H?y";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(20, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 20);
			this.label1.TabIndex = 7;
			this.label1.Text = "Mã giá vé";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(20, 65);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 20);
			this.label2.TabIndex = 8;
			this.label2.Text = "Nhà xe";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(20, 110);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(59, 20);
			this.label3.TabIndex = 9;
			this.label3.Text = "Lo?i xe";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(20, 157);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(101, 20);
			this.label4.TabIndex = 10;
			this.label4.Text = "Tuy?n ???ng";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(20, 204);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(77, 20);
			this.label5.TabIndex = 11;
			this.label5.Text = "Giá vé (?)";
			// 
			// GiaVeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(602, 426);
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
			this.Text = "Giá vé";
			((System.ComponentModel.ISupportInitialize)(this.numGiaVe)).EndInit();
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
	}
}
