namespace QLBXMD.GUI.Admin
{
	partial class HoaDonDialog
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
            this.txtMaHD = new System.Windows.Forms.TextBox();
            this.cbbKhachHang = new System.Windows.Forms.ComboBox();
            this.cbbNhanVien = new System.Windows.Forms.ComboBox();
            this.dtpNgayLap = new System.Windows.Forms.DateTimePicker();
            this.numTongTien = new System.Windows.Forms.NumericUpDown();
            this.cbbTrangThai = new System.Windows.Forms.ComboBox();
            this.btnPrimary = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblDanhSachVe = new System.Windows.Forms.Label();
            this.dgvDanhSachVe = new System.Windows.Forms.DataGridView();
            this.colMaVe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTuyenDuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayKhoiHanh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChoNgoi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGiaVe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNhaXe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numTongTien)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSachVe)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMaHD
            // 
            this.txtMaHD.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtMaHD.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMaHD.Location = new System.Drawing.Point(351, 80);
            this.txtMaHD.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMaHD.Name = "txtMaHD";
            this.txtMaHD.ReadOnly = true;
            this.txtMaHD.Size = new System.Drawing.Size(320, 30);
            this.txtMaHD.TabIndex = 0;
            // 
            // cbbKhachHang
            // 
            this.cbbKhachHang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbKhachHang.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbbKhachHang.FormattingEnabled = true;
            this.cbbKhachHang.Location = new System.Drawing.Point(351, 124);
            this.cbbKhachHang.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbbKhachHang.Name = "cbbKhachHang";
            this.cbbKhachHang.Size = new System.Drawing.Size(320, 31);
            this.cbbKhachHang.TabIndex = 1;
            // 
            // cbbNhanVien
            // 
            this.cbbNhanVien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbNhanVien.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbbNhanVien.FormattingEnabled = true;
            this.cbbNhanVien.Location = new System.Drawing.Point(351, 168);
            this.cbbNhanVien.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbbNhanVien.Name = "cbbNhanVien";
            this.cbbNhanVien.Size = new System.Drawing.Size(320, 31);
            this.cbbNhanVien.TabIndex = 2;
            // 
            // dtpNgayLap
            // 
            this.dtpNgayLap.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpNgayLap.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpNgayLap.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayLap.Location = new System.Drawing.Point(351, 212);
            this.dtpNgayLap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpNgayLap.Name = "dtpNgayLap";
            this.dtpNgayLap.Size = new System.Drawing.Size(223, 30);
            this.dtpNgayLap.TabIndex = 3;
            // 
            // numTongTien
            // 
            this.numTongTien.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numTongTien.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numTongTien.Location = new System.Drawing.Point(351, 256);
            this.numTongTien.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numTongTien.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numTongTien.Name = "numTongTien";
            this.numTongTien.Size = new System.Drawing.Size(222, 30);
            this.numTongTien.TabIndex = 4;
            this.numTongTien.ThousandsSeparator = true;
            // 
            // cbbTrangThai
            // 
            this.cbbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbbTrangThai.FormattingEnabled = true;
            this.cbbTrangThai.Items.AddRange(new object[] {
            "Đã thanh toán",
            "Chưa thanh toán"});
            this.cbbTrangThai.Location = new System.Drawing.Point(351, 300);
            this.cbbTrangThai.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbbTrangThai.Name = "cbbTrangThai";
            this.cbbTrangThai.Size = new System.Drawing.Size(223, 31);
            this.cbbTrangThai.TabIndex = 5;
            // 
            // btnPrimary
            // 
            this.btnPrimary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnPrimary.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrimary.FlatAppearance.BorderSize = 0;
            this.btnPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrimary.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnPrimary.ForeColor = System.Drawing.Color.White;
            this.btnPrimary.Location = new System.Drawing.Point(484, 352);
            this.btnPrimary.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPrimary.Name = "btnPrimary";
            this.btnPrimary.Size = new System.Drawing.Size(89, 36);
            this.btnPrimary.TabIndex = 6;
            this.btnPrimary.Text = "Lưu";
            this.btnPrimary.UseVisualStyleBackColor = false;
            this.btnPrimary.Click += new System.EventHandler(this.btnPrimary_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(582, 352);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 36);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(226, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "Mã hóa đơn:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(226, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Khách hàng:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(226, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 23);
            this.label3.TabIndex = 10;
            this.label3.Text = "Nhân viên:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(226, 214);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 23);
            this.label4.TabIndex = 11;
            this.label4.Text = "Ngày lập:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(226, 258);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 23);
            this.label5.TabIndex = 12;
            this.label5.Text = "Tổng tiền:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label6.Location = new System.Drawing.Point(226, 302);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 23);
            this.label6.TabIndex = 13;
            this.label6.Text = "Trạng thái:";
            // 
            // lblDanhSachVe
            // 
            this.lblDanhSachVe.AutoSize = true;
            this.lblDanhSachVe.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDanhSachVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.lblDanhSachVe.Location = new System.Drawing.Point(226, 352);
            this.lblDanhSachVe.Name = "lblDanhSachVe";
            this.lblDanhSachVe.Size = new System.Drawing.Size(114, 23);
            this.lblDanhSachVe.TabIndex = 14;
            this.lblDanhSachVe.Text = "Danh sách vé";
            this.lblDanhSachVe.Visible = false;
            // 
            // dgvDanhSachVe
            // 
            this.dgvDanhSachVe.AllowUserToAddRows = false;
            this.dgvDanhSachVe.AllowUserToDeleteRows = false;
            this.dgvDanhSachVe.AllowUserToResizeRows = false;
            this.dgvDanhSachVe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDanhSachVe.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDanhSachVe.BackgroundColor = System.Drawing.Color.White;
            this.dgvDanhSachVe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDanhSachVe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaVe,
            this.colTuyenDuong,
            this.colNgayKhoiHanh,
            this.colChoNgoi,
            this.colGiaVe,
            this.colNhaXe,
            this.colAction});
            this.dgvDanhSachVe.Location = new System.Drawing.Point(42, 400);
            this.dgvDanhSachVe.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvDanhSachVe.MultiSelect = false;
            this.dgvDanhSachVe.Name = "dgvDanhSachVe";
            this.dgvDanhSachVe.ReadOnly = true;
            this.dgvDanhSachVe.RowHeadersVisible = false;
            this.dgvDanhSachVe.RowHeadersWidth = 62;
            this.dgvDanhSachVe.RowTemplate.Height = 35;
            this.dgvDanhSachVe.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDanhSachVe.Size = new System.Drawing.Size(876, 226);
            this.dgvDanhSachVe.TabIndex = 15;
            this.dgvDanhSachVe.Visible = false;
            this.dgvDanhSachVe.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDanhSachVe_CellClick);
            this.dgvDanhSachVe.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvDanhSachVe_CellPainting);
            // 
            // colMaVe
            // 
            this.colMaVe.HeaderText = "Mã vé";
            this.colMaVe.MinimumWidth = 6;
            this.colMaVe.Name = "colMaVe";
            this.colMaVe.ReadOnly = true;
            // 
            // colTuyenDuong
            // 
            this.colTuyenDuong.HeaderText = "Tuyến đường";
            this.colTuyenDuong.MinimumWidth = 6;
            this.colTuyenDuong.Name = "colTuyenDuong";
            this.colTuyenDuong.ReadOnly = true;
            // 
            // colNgayKhoiHanh
            // 
            this.colNgayKhoiHanh.HeaderText = "Khởi hành";
            this.colNgayKhoiHanh.MinimumWidth = 6;
            this.colNgayKhoiHanh.Name = "colNgayKhoiHanh";
            this.colNgayKhoiHanh.ReadOnly = true;
            // 
            // colChoNgoi
            // 
            this.colChoNgoi.HeaderText = "Ghế";
            this.colChoNgoi.MinimumWidth = 6;
            this.colChoNgoi.Name = "colChoNgoi";
            this.colChoNgoi.ReadOnly = true;
            // 
            // colGiaVe
            // 
            this.colGiaVe.HeaderText = "Giá vé";
            this.colGiaVe.MinimumWidth = 6;
            this.colGiaVe.Name = "colGiaVe";
            this.colGiaVe.ReadOnly = true;
            // 
            // colNhaXe
            // 
            this.colNhaXe.HeaderText = "Nhà xe";
            this.colNhaXe.MinimumWidth = 6;
            this.colNhaXe.Name = "colNhaXe";
            this.colNhaXe.ReadOnly = true;
            // 
            // colAction
            // 
            this.colAction.HeaderText = "Hành động";
            this.colAction.MinimumWidth = 200;
            this.colAction.Name = "colAction";
            this.colAction.ReadOnly = true;
            this.colAction.Width = 200;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(946, 56);
            this.pnlHeader.TabIndex = 14;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.Teal;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(946, 56);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "THÔNG TIN HÓA ĐƠN";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HoaDonDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(946, 637);
            this.Controls.Add(this.dgvDanhSachVe);
            this.Controls.Add(this.lblDanhSachVe);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPrimary);
            this.Controls.Add(this.cbbTrangThai);
            this.Controls.Add(this.numTongTien);
            this.Controls.Add(this.dtpNgayLap);
            this.Controls.Add(this.cbbNhanVien);
            this.Controls.Add(this.cbbKhachHang);
            this.Controls.Add(this.txtMaHD);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HoaDonDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quản lý hóa đơn";
            ((System.ComponentModel.ISupportInitialize)(this.numTongTien)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSachVe)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtMaHD;
		private System.Windows.Forms.ComboBox cbbKhachHang;
		private System.Windows.Forms.ComboBox cbbNhanVien;
		private System.Windows.Forms.DateTimePicker dtpNgayLap;
		private System.Windows.Forms.NumericUpDown numTongTien;
		private System.Windows.Forms.ComboBox cbbTrangThai;
		private System.Windows.Forms.Button btnPrimary;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lblDanhSachVe;
		private System.Windows.Forms.DataGridView dgvDanhSachVe;
		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.DataGridViewTextBoxColumn colMaVe;
		private System.Windows.Forms.DataGridViewTextBoxColumn colTuyenDuong;
		private System.Windows.Forms.DataGridViewTextBoxColumn colNgayKhoiHanh;
		private System.Windows.Forms.DataGridViewTextBoxColumn colChoNgoi;
		private System.Windows.Forms.DataGridViewTextBoxColumn colGiaVe;
		private System.Windows.Forms.DataGridViewTextBoxColumn colNhaXe;
		private System.Windows.Forms.DataGridViewTextBoxColumn colAction;
	}
}
