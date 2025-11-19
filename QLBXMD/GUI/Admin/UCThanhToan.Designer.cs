namespace QLBXMD.GUI.Admin
{
	partial class UCThanhToan
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.lblTitle = new System.Windows.Forms.Label();
			this.pnlStatistics = new System.Windows.Forms.Panel();
			this.pnlStat3 = new System.Windows.Forms.Panel();
			this.lblTrungBinh = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.pnlStat2 = new System.Windows.Forms.Panel();
			this.lblTongGiaoDich = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.pnlStat1 = new System.Windows.Forms.Panel();
			this.lblTongDoanhThu = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pnlFilter = new System.Windows.Forms.Panel();
			this.btnXuatBaoCao = new System.Windows.Forms.Button();
			this.btnLamMoi = new System.Windows.Forms.Button();
			this.btnTimKiem = new System.Windows.Forms.Button();
			this.cbbLoaiTimKiem = new System.Windows.Forms.ComboBox();
			this.txtTimKiem = new System.Windows.Forms.TextBox();
			this.cbbPhuongThuc = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.dtpDenNgay = new System.Windows.Forms.DateTimePicker();
			this.label7 = new System.Windows.Forms.Label();
			this.dtpTuNgay = new System.Windows.Forms.DateTimePicker();
			this.label5 = new System.Windows.Forms.Label();
			this.pnlData = new System.Windows.Forms.Panel();
			this.dgvThanhToan = new System.Windows.Forms.DataGridView();
			this.MaTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MaHD = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TenKH = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SDT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TongTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.NgayTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PhuongThuc = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SoVe = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.pnlDataHeader = new System.Windows.Forms.Panel();
			this.btnXemChiTiet = new System.Windows.Forms.Button();
			this.lblTongSo = new System.Windows.Forms.Label();
			this.pnlHeader.SuspendLayout();
			this.pnlStatistics.SuspendLayout();
			this.pnlStat3.SuspendLayout();
			this.pnlStat2.SuspendLayout();
			this.pnlStat1.SuspendLayout();
			this.pnlFilter.SuspendLayout();
			this.pnlData.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvThanhToan)).BeginInit();
			this.pnlDataHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlHeader
			// 
			this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.pnlHeader.Controls.Add(this.lblTitle);
			this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlHeader.Location = new System.Drawing.Point(0, 0);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(1336, 80);
			this.pnlHeader.TabIndex = 0;
			// 
			// lblTitle
			// 
			this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(1336, 80);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "QUẢN LÝ THANH TOÁN";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlStatistics
			// 
			this.pnlStatistics.BackColor = System.Drawing.Color.White;
			this.pnlStatistics.Controls.Add(this.pnlStat3);
			this.pnlStatistics.Controls.Add(this.pnlStat2);
			this.pnlStatistics.Controls.Add(this.pnlStat1);
			this.pnlStatistics.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlStatistics.Location = new System.Drawing.Point(0, 80);
			this.pnlStatistics.Name = "pnlStatistics";
			this.pnlStatistics.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
			this.pnlStatistics.Size = new System.Drawing.Size(1336, 120);
			this.pnlStatistics.TabIndex = 1;
			// 
			// pnlStat3
			// 
			this.pnlStat3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
			this.pnlStat3.Controls.Add(this.lblTrungBinh);
			this.pnlStat3.Controls.Add(this.label6);
			this.pnlStat3.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlStat3.Location = new System.Drawing.Point(920, 15);
			this.pnlStat3.Name = "pnlStat3";
			this.pnlStat3.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
			this.pnlStat3.Size = new System.Drawing.Size(400, 90);
			this.pnlStat3.TabIndex = 2;
			// 
			// lblTrungBinh
			// 
			this.lblTrungBinh.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblTrungBinh.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lblTrungBinh.ForeColor = System.Drawing.Color.White;
			this.lblTrungBinh.Location = new System.Drawing.Point(15, 38);
			this.lblTrungBinh.Name = "lblTrungBinh";
			this.lblTrungBinh.Size = new System.Drawing.Size(370, 42);
			this.lblTrungBinh.TabIndex = 1;
			this.lblTrungBinh.Text = "0 ₫";
			this.lblTrungBinh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label6
			// 
			this.label6.Dock = System.Windows.Forms.DockStyle.Top;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label6.ForeColor = System.Drawing.Color.White;
			this.label6.Location = new System.Drawing.Point(15, 10);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(370, 28);
			this.label6.TabIndex = 0;
			this.label6.Text = "Trung bình / giao dịch";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlStat2
			// 
			this.pnlStat2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
			this.pnlStat2.Controls.Add(this.lblTongGiaoDich);
			this.pnlStat2.Controls.Add(this.label4);
			this.pnlStat2.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlStat2.Location = new System.Drawing.Point(470, 15);
			this.pnlStat2.Name = "pnlStat2";
			this.pnlStat2.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
			this.pnlStat2.Size = new System.Drawing.Size(450, 90);
			this.pnlStat2.TabIndex = 1;
			// 
			// lblTongGiaoDich
			// 
			this.lblTongGiaoDich.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblTongGiaoDich.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lblTongGiaoDich.ForeColor = System.Drawing.Color.White;
			this.lblTongGiaoDich.Location = new System.Drawing.Point(15, 38);
			this.lblTongGiaoDich.Name = "lblTongGiaoDich";
			this.lblTongGiaoDich.Size = new System.Drawing.Size(420, 42);
			this.lblTongGiaoDich.TabIndex = 1;
			this.lblTongGiaoDich.Text = "0";
			this.lblTongGiaoDich.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.Dock = System.Windows.Forms.DockStyle.Top;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(15, 10);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(420, 28);
			this.label4.TabIndex = 0;
			this.label4.Text = "Tổng giao dịch";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlStat1
			// 
			this.pnlStat1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
			this.pnlStat1.Controls.Add(this.lblTongDoanhThu);
			this.pnlStat1.Controls.Add(this.label1);
			this.pnlStat1.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlStat1.Location = new System.Drawing.Point(20, 15);
			this.pnlStat1.Name = "pnlStat1";
			this.pnlStat1.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
			this.pnlStat1.Size = new System.Drawing.Size(450, 90);
			this.pnlStat1.TabIndex = 0;
			// 
			// lblTongDoanhThu
			// 
			this.lblTongDoanhThu.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblTongDoanhThu.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lblTongDoanhThu.ForeColor = System.Drawing.Color.White;
			this.lblTongDoanhThu.Location = new System.Drawing.Point(15, 38);
			this.lblTongDoanhThu.Name = "lblTongDoanhThu";
			this.lblTongDoanhThu.Size = new System.Drawing.Size(420, 42);
			this.lblTongDoanhThu.TabIndex = 1;
			this.lblTongDoanhThu.Text = "0 ₫";
			this.lblTongDoanhThu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(15, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(420, 28);
			this.label1.TabIndex = 0;
			this.label1.Text = "Tổng doanh thu";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlFilter
			// 
			this.pnlFilter.BackColor = System.Drawing.Color.White;
			this.pnlFilter.Controls.Add(this.btnXuatBaoCao);
			this.pnlFilter.Controls.Add(this.btnLamMoi);
			this.pnlFilter.Controls.Add(this.btnTimKiem);
			this.pnlFilter.Controls.Add(this.cbbLoaiTimKiem);
			this.pnlFilter.Controls.Add(this.txtTimKiem);
			this.pnlFilter.Controls.Add(this.cbbPhuongThuc);
			this.pnlFilter.Controls.Add(this.label8);
			this.pnlFilter.Controls.Add(this.dtpDenNgay);
			this.pnlFilter.Controls.Add(this.label7);
			this.pnlFilter.Controls.Add(this.dtpTuNgay);
			this.pnlFilter.Controls.Add(this.label5);
			this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlFilter.Location = new System.Drawing.Point(0, 200);
			this.pnlFilter.Name = "pnlFilter";
			this.pnlFilter.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
			this.pnlFilter.Size = new System.Drawing.Size(1336, 140);
			this.pnlFilter.TabIndex = 2;
			// 
			// btnXuatBaoCao
			// 
			this.btnXuatBaoCao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnXuatBaoCao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
			this.btnXuatBaoCao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnXuatBaoCao.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnXuatBaoCao.ForeColor = System.Drawing.Color.White;
			this.btnXuatBaoCao.Location = new System.Drawing.Point(1156, 80);
			this.btnXuatBaoCao.Name = "btnXuatBaoCao";
			this.btnXuatBaoCao.Size = new System.Drawing.Size(150, 40);
			this.btnXuatBaoCao.TabIndex = 10;
			this.btnXuatBaoCao.Text = "Xuất báo cáo";
			this.btnXuatBaoCao.UseVisualStyleBackColor = false;
			this.btnXuatBaoCao.Click += new System.EventHandler(this.btnXuatBaoCao_Click);
			// 
			// btnLamMoi
			// 
			this.btnLamMoi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(158)))), ((int)(((byte)(158)))));
			this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnLamMoi.ForeColor = System.Drawing.Color.White;
			this.btnLamMoi.Location = new System.Drawing.Point(1000, 80);
			this.btnLamMoi.Name = "btnLamMoi";
			this.btnLamMoi.Size = new System.Drawing.Size(150, 40);
			this.btnLamMoi.TabIndex = 9;
			this.btnLamMoi.Text = "Làm mới";
			this.btnLamMoi.UseVisualStyleBackColor = false;
			this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
			// 
			// btnTimKiem
			// 
			this.btnTimKiem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTimKiem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnTimKiem.ForeColor = System.Drawing.Color.White;
			this.btnTimKiem.Location = new System.Drawing.Point(844, 80);
			this.btnTimKiem.Name = "btnTimKiem";
			this.btnTimKiem.Size = new System.Drawing.Size(150, 40);
			this.btnTimKiem.TabIndex = 8;
			this.btnTimKiem.Text = "Tìm kiếm";
			this.btnTimKiem.UseVisualStyleBackColor = false;
			this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
			// 
			// cbbLoaiTimKiem
			// 
			this.cbbLoaiTimKiem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbLoaiTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbLoaiTimKiem.FormattingEnabled = true;
			this.cbbLoaiTimKiem.Location = new System.Drawing.Point(23, 85);
			this.cbbLoaiTimKiem.Name = "cbbLoaiTimKiem";
			this.cbbLoaiTimKiem.Size = new System.Drawing.Size(200, 36);
			this.cbbLoaiTimKiem.TabIndex = 7;
			// 
			// txtTimKiem
			// 
			this.txtTimKiem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txtTimKiem.Location = new System.Drawing.Point(229, 85);
			this.txtTimKiem.Name = "txtTimKiem";
			this.txtTimKiem.Size = new System.Drawing.Size(609, 34);
			this.txtTimKiem.TabIndex = 6;
			this.txtTimKiem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimKiem_KeyDown);
			// 
			// cbbPhuongThuc
			// 
			this.cbbPhuongThuc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbbPhuongThuc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbPhuongThuc.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.cbbPhuongThuc.FormattingEnabled = true;
			this.cbbPhuongThuc.Location = new System.Drawing.Point(1066, 28);
			this.cbbPhuongThuc.Name = "cbbPhuongThuc";
			this.cbbPhuongThuc.Size = new System.Drawing.Size(240, 36);
			this.cbbPhuongThuc.TabIndex = 5;
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label8.Location = new System.Drawing.Point(913, 31);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(147, 28);
			this.label8.TabIndex = 4;
			this.label8.Text = "Phương thức:";
			// 
			// dtpDenNgay
			// 
			this.dtpDenNgay.CustomFormat = "dd/MM/yyyy";
			this.dtpDenNgay.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.dtpDenNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpDenNgay.Location = new System.Drawing.Point(585, 28);
			this.dtpDenNgay.Name = "dtpDenNgay";
			this.dtpDenNgay.Size = new System.Drawing.Size(200, 34);
			this.dtpDenNgay.TabIndex = 3;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label7.Location = new System.Drawing.Point(465, 31);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(114, 28);
			this.label7.TabIndex = 2;
			this.label7.Text = "Đến ngày:";
			// 
			// dtpTuNgay
			// 
			this.dtpTuNgay.CustomFormat = "dd/MM/yyyy";
			this.dtpTuNgay.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.dtpTuNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpTuNgay.Location = new System.Drawing.Point(120, 28);
			this.dtpTuNgay.Name = "dtpTuNgay";
			this.dtpTuNgay.Size = new System.Drawing.Size(200, 34);
			this.dtpTuNgay.TabIndex = 1;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label5.Location = new System.Drawing.Point(23, 31);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(96, 28);
			this.label5.TabIndex = 0;
			this.label5.Text = "Từ ngày:";
			// 
			// pnlData
			// 
			this.pnlData.BackColor = System.Drawing.Color.White;
			this.pnlData.Controls.Add(this.dgvThanhToan);
			this.pnlData.Controls.Add(this.pnlDataHeader);
			this.pnlData.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlData.Location = new System.Drawing.Point(0, 340);
			this.pnlData.Name = "pnlData";
			this.pnlData.Padding = new System.Windows.Forms.Padding(20);
			this.pnlData.Size = new System.Drawing.Size(1336, 405);
			this.pnlData.TabIndex = 3;
			// 
			// dgvThanhToan
			// 
			this.dgvThanhToan.AllowUserToAddRows = false;
			this.dgvThanhToan.AllowUserToDeleteRows = false;
			this.dgvThanhToan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvThanhToan.BackgroundColor = System.Drawing.Color.White;
			this.dgvThanhToan.BorderStyle = System.Windows.Forms.BorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvThanhToan.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvThanhToan.ColumnHeadersHeight = 40;
			this.dgvThanhToan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvThanhToan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MaTT,
            this.MaHD,
            this.TenKH,
            this.SDT,
            this.TongTien,
            this.NgayTT,
            this.PhuongThuc,
            this.SoVe});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(223)))), ((int)(((byte)(219)))));
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvThanhToan.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvThanhToan.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvThanhToan.EnableHeadersVisualStyles = false;
			this.dgvThanhToan.Location = new System.Drawing.Point(20, 70);
			this.dgvThanhToan.MultiSelect = false;
			this.dgvThanhToan.Name = "dgvThanhToan";
			this.dgvThanhToan.ReadOnly = true;
			this.dgvThanhToan.RowHeadersVisible = false;
			this.dgvThanhToan.RowHeadersWidth = 62;
			this.dgvThanhToan.RowTemplate.Height = 35;
			this.dgvThanhToan.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvThanhToan.Size = new System.Drawing.Size(1296, 315);
			this.dgvThanhToan.TabIndex = 1;
			this.dgvThanhToan.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvThanhToan_CellDoubleClick);
			// 
			// MaTT
			// 
			this.MaTT.FillWeight = 50F;
			this.MaTT.HeaderText = "Mã TT";
			this.MaTT.MinimumWidth = 8;
			this.MaTT.Name = "MaTT";
			this.MaTT.ReadOnly = true;
			// 
			// MaHD
			// 
			this.MaHD.FillWeight = 50F;
			this.MaHD.HeaderText = "Mã HĐ";
			this.MaHD.MinimumWidth = 8;
			this.MaHD.Name = "MaHD";
			this.MaHD.ReadOnly = true;
			// 
			// TenKH
			// 
			this.TenKH.FillWeight = 120F;
			this.TenKH.HeaderText = "Tên khách hàng";
		 this.TenKH.MinimumWidth = 8;
			this.TenKH.Name = "TenKH";
			this.TenKH.ReadOnly = true;
			// 
			// SDT
			// 
			this.SDT.FillWeight = 80F;
			this.SDT.HeaderText = "Số điện thoại";
			this.SDT.MinimumWidth = 8;
			this.SDT.Name = "SDT";
			this.SDT.ReadOnly = true;
			// 
			// TongTien
			// 
			this.TongTien.FillWeight = 90F;
			this.TongTien.HeaderText = "Tổng tiền";
			this.TongTien.MinimumWidth = 8;
			this.TongTien.Name = "TongTien";
			this.TongTien.ReadOnly = true;
			// 
			// NgayTT
			// 
			this.NgayTT.FillWeight = 100F;
			this.NgayTT.HeaderText = "Ngày thanh toán";
			this.NgayTT.MinimumWidth = 8;
			this.NgayTT.Name = "NgayTT";
			this.NgayTT.ReadOnly = true;
			// 
			// PhuongThuc
			// 
			this.PhuongThuc.FillWeight = 80F;
			this.PhuongThuc.HeaderText = "Phương thức";
			this.PhuongThuc.MinimumWidth = 8;
			this.PhuongThuc.Name = "PhuongThuc";
			this.PhuongThuc.ReadOnly = true;
			// 
			// SoVe
			// 
			this.SoVe.FillWeight = 50F;
			this.SoVe.HeaderText = "Số vé";
			this.SoVe.MinimumWidth = 8;
			this.SoVe.Name = "SoVe";
			this.SoVe.ReadOnly = true;
			// 
			// pnlDataHeader
			// 
			this.pnlDataHeader.Controls.Add(this.btnXemChiTiet);
			this.pnlDataHeader.Controls.Add(this.lblTongSo);
			this.pnlDataHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDataHeader.Location = new System.Drawing.Point(20, 20);
			this.pnlDataHeader.Name = "pnlDataHeader";
			this.pnlDataHeader.Size = new System.Drawing.Size(1296, 50);
			this.pnlDataHeader.TabIndex = 0;
			// 
			// btnXemChiTiet
			// 
			this.btnXemChiTiet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnXemChiTiet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
			this.btnXemChiTiet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnXemChiTiet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnXemChiTiet.ForeColor = System.Drawing.Color.White;
			this.btnXemChiTiet.Location = new System.Drawing.Point(1126, 5);
			this.btnXemChiTiet.Name = "btnXemChiTiet";
			this.btnXemChiTiet.Size = new System.Drawing.Size(150, 40);
			this.btnXemChiTiet.TabIndex = 1;
			this.btnXemChiTiet.Text = "Xem chi tiết";
			this.btnXemChiTiet.UseVisualStyleBackColor = false;
			this.btnXemChiTiet.Click += new System.EventHandler(this.btnXemChiTiet_Click);
			// 
			// lblTongSo
			// 
			this.lblTongSo.AutoSize = true;
			this.lblTongSo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.lblTongSo.Location = new System.Drawing.Point(3, 12);
			this.lblTongSo.Name = "lblTongSo";
			this.lblTongSo.Size = new System.Drawing.Size(249, 30);
			this.lblTongSo.TabIndex = 0;
			this.lblTongSo.Text = "Tổng số: 0 thanh toán";
			// 
			// UCThanhToan
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.Controls.Add(this.pnlData);
			this.Controls.Add(this.pnlFilter);
			this.Controls.Add(this.pnlStatistics);
			this.Controls.Add(this.pnlHeader);
			this.Name = "UCThanhToan";
			this.Size = new System.Drawing.Size(1336, 745);
			this.pnlHeader.ResumeLayout(false);
			this.pnlStatistics.ResumeLayout(false);
			this.pnlStat3.ResumeLayout(false);
			this.pnlStat2.ResumeLayout(false);
			this.pnlStat1.ResumeLayout(false);
			this.pnlFilter.ResumeLayout(false);
			this.pnlFilter.PerformLayout();
			this.pnlData.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvThanhToan)).EndInit();
			this.pnlDataHeader.ResumeLayout(false);
			this.pnlDataHeader.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Panel pnlStatistics;
		private System.Windows.Forms.Panel pnlStat1;
		private System.Windows.Forms.Label lblTongDoanhThu;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel pnlStat2;
		private System.Windows.Forms.Label lblTongGiaoDich;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel pnlStat3;
		private System.Windows.Forms.Label lblTrungBinh;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel pnlFilter;
		private System.Windows.Forms.DateTimePicker dtpTuNgay;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.DateTimePicker dtpDenNgay;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cbbPhuongThuc;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtTimKiem;
		private System.Windows.Forms.ComboBox cbbLoaiTimKiem;
		private System.Windows.Forms.Button btnTimKiem;
		private System.Windows.Forms.Button btnLamMoi;
		private System.Windows.Forms.Button btnXuatBaoCao;
		private System.Windows.Forms.Panel pnlData;
		private System.Windows.Forms.DataGridView dgvThanhToan;
		private System.Windows.Forms.Panel pnlDataHeader;
		private System.Windows.Forms.Label lblTongSo;
		private System.Windows.Forms.Button btnXemChiTiet;
		private System.Windows.Forms.DataGridViewTextBoxColumn MaTT;
		private System.Windows.Forms.DataGridViewTextBoxColumn MaHD;
		private System.Windows.Forms.DataGridViewTextBoxColumn TenKH;
		private System.Windows.Forms.DataGridViewTextBoxColumn SDT;
		private System.Windows.Forms.DataGridViewTextBoxColumn TongTien;
		private System.Windows.Forms.DataGridViewTextBoxColumn NgayTT;
		private System.Windows.Forms.DataGridViewTextBoxColumn PhuongThuc;
		private System.Windows.Forms.DataGridViewTextBoxColumn SoVe;
	}
}
