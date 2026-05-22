namespace QLBXMD.GUI.Admin
{
	partial class UCThongKe
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.btnThongKe = new System.Windows.Forms.Button();
            this.dtpDenNgay = new System.Windows.Forms.DateTimePicker();
            this.dtpTuNgay = new System.Windows.Forms.DateTimePicker();
            this.lblDenNgay = new System.Windows.Forms.Label();
            this.lblTuNgay = new System.Windows.Forms.Label();
            
            // New Tabbed Controls
            this.tabControlThongKe = new System.Windows.Forms.TabControl();
            this.tabTongQuan = new System.Windows.Forms.TabPage();
            this.tabChiTiet = new System.Windows.Forms.TabPage();
            
            this.pnlStats = new System.Windows.Forms.Panel();
            this.pnlStat1 = new System.Windows.Forms.Panel();
            this.lblDoanhThu = new System.Windows.Forms.Label();
            this.lblDoanhThuValue = new System.Windows.Forms.Label();
            this.pnlStat2 = new System.Windows.Forms.Panel();
            this.lblSoChuyenXe = new System.Windows.Forms.Label();
            this.lblSoChuyenXeValue = new System.Windows.Forms.Label();
            this.pnlStat3 = new System.Windows.Forms.Panel();
            this.lblSoKhachHang = new System.Windows.Forms.Label();
            this.lblSoKhachHangValue = new System.Windows.Forms.Label();
            this.pnlStat4 = new System.Windows.Forms.Panel();
            this.lblSoVe = new System.Windows.Forms.Label();
            this.lblSoVeValue = new System.Windows.Forms.Label();
            this.pnlStat5 = new System.Windows.Forms.Panel();
            this.lblTrungBinhVe = new System.Windows.Forms.Label();
            this.lblTrungBinhVeValue = new System.Windows.Forms.Label();
            
            this.pnlCharts = new System.Windows.Forms.Panel();
            this.chartTuyenDuong = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartDoanhThu = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartNhaXe = new System.Windows.Forms.DataVisualization.Charting.Chart();
            
            // New Detail Report Controls
            this.pnlDetailHeader = new System.Windows.Forms.Panel();
            this.lblSelectReport = new System.Windows.Forms.Label();
            this.cboLoaiBaoCao = new System.Windows.Forms.ComboBox();
            this.btnExportCSV = new System.Windows.Forms.Button();
            this.pnlDetailGrid = new System.Windows.Forms.Panel();
            this.dgvChiTiet = new System.Windows.Forms.DataGridView();

            this.pnlTop.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.tabControlThongKe.SuspendLayout();
            this.tabTongQuan.SuspendLayout();
            this.tabChiTiet.SuspendLayout();
            this.pnlStats.SuspendLayout();
            this.pnlStat1.SuspendLayout();
            this.pnlStat2.SuspendLayout();
            this.pnlStat3.SuspendLayout();
            this.pnlStat4.SuspendLayout();
            this.pnlStat5.SuspendLayout();
            this.pnlCharts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTuyenDuong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDoanhThu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartNhaXe)).BeginInit();
            this.pnlDetailHeader.SuspendLayout();
            this.pnlDetailGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1499, 48);
            this.pnlTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.lblTitle.Location = new System.Drawing.Point(24, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(152, 41);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Thống Kê";
            // 
            // pnlFilter
            // 
            this.pnlFilter.BackColor = System.Drawing.Color.White;
            this.pnlFilter.Controls.Add(this.btnThongKe);
            this.pnlFilter.Controls.Add(this.dtpDenNgay);
            this.pnlFilter.Controls.Add(this.dtpTuNgay);
            this.pnlFilter.Controls.Add(this.lblDenNgay);
            this.pnlFilter.Controls.Add(this.lblTuNgay);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 48);
            this.pnlFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Padding = new System.Windows.Forms.Padding(18, 8, 18, 8);
            this.pnlFilter.Size = new System.Drawing.Size(1499, 56);
            this.pnlFilter.TabIndex = 1;
            // 
            // btnThongKe
            // 
            this.btnThongKe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnThongKe.FlatAppearance.BorderSize = 0;
            this.btnThongKe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThongKe.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnThongKe.ForeColor = System.Drawing.Color.White;
            this.btnThongKe.Location = new System.Drawing.Point(667, 12);
            this.btnThongKe.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnThongKe.Name = "btnThongKe";
            this.btnThongKe.Size = new System.Drawing.Size(107, 32);
            this.btnThongKe.TabIndex = 4;
            this.btnThongKe.Text = "Thống Kê";
            this.btnThongKe.UseVisualStyleBackColor = false;
            this.btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click);
            // 
            // dtpDenNgay
            // 
            this.dtpDenNgay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpDenNgay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDenNgay.Location = new System.Drawing.Point(444, 16);
            this.dtpDenNgay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpDenNgay.Name = "dtpDenNgay";
            this.dtpDenNgay.Size = new System.Drawing.Size(178, 30);
            this.dtpDenNgay.TabIndex = 3;
            // 
            // dtpTuNgay
            // 
            this.dtpTuNgay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpTuNgay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTuNgay.Location = new System.Drawing.Point(133, 16);
            this.dtpTuNgay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpTuNgay.Name = "dtpTuNgay";
            this.dtpTuNgay.Size = new System.Drawing.Size(178, 30);
            this.dtpTuNgay.TabIndex = 2;
            // 
            // lblDenNgay
            // 
            this.lblDenNgay.AutoSize = true;
            this.lblDenNgay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDenNgay.Location = new System.Drawing.Point(356, 18);
            this.lblDenNgay.Name = "lblDenNgay";
            this.lblDenNgay.Size = new System.Drawing.Size(87, 23);
            this.lblDenNgay.TabIndex = 1;
            this.lblDenNgay.Text = "Đến ngày:";
            // 
            // lblTuNgay
            // 
            this.lblTuNgay.AutoSize = true;
            this.lblTuNgay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTuNgay.Location = new System.Drawing.Point(27, 18);
            this.lblTuNgay.Name = "lblTuNgay";
            this.lblTuNgay.Size = new System.Drawing.Size(75, 23);
            this.lblTuNgay.TabIndex = 0;
            this.lblTuNgay.Text = "Từ ngày:";
            // 
            // tabControlThongKe
            // 
            this.tabControlThongKe.Controls.Add(this.tabTongQuan);
            this.tabControlThongKe.Controls.Add(this.tabChiTiet);
            this.tabControlThongKe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlThongKe.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControlThongKe.Location = new System.Drawing.Point(0, 104);
            this.tabControlThongKe.Name = "tabControlThongKe";
            this.tabControlThongKe.Padding = new System.Drawing.Point(12, 6);
            this.tabControlThongKe.SelectedIndex = 0;
            this.tabControlThongKe.Size = new System.Drawing.Size(1499, 771);
            this.tabControlThongKe.TabIndex = 2;
            // 
            // tabTongQuan
            // 
            this.tabTongQuan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tabTongQuan.Controls.Add(this.pnlCharts);
            this.tabTongQuan.Controls.Add(this.pnlStats);
            this.tabTongQuan.Location = new System.Drawing.Point(4, 38);
            this.tabTongQuan.Name = "tabTongQuan";
            this.tabTongQuan.Padding = new System.Windows.Forms.Padding(0);
            this.tabTongQuan.Size = new System.Drawing.Size(1491, 729);
            this.tabTongQuan.TabIndex = 0;
            this.tabTongQuan.Text = "Tổng Quan & Biểu Đồ";
            // 
            // pnlStats
            // 
            this.pnlStats.BackColor = System.Drawing.Color.White;
            this.pnlStats.Controls.Add(this.pnlStat5);
            this.pnlStats.Controls.Add(this.pnlStat4);
            this.pnlStats.Controls.Add(this.pnlStat3);
            this.pnlStats.Controls.Add(this.pnlStat2);
            this.pnlStats.Controls.Add(this.pnlStat1);
            this.pnlStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStats.Location = new System.Drawing.Point(0, 0);
            this.pnlStats.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlStats.Name = "pnlStats";
            this.pnlStats.Padding = new System.Windows.Forms.Padding(18, 8, 18, 8);
            this.pnlStats.Size = new System.Drawing.Size(1491, 115);
            this.pnlStats.TabIndex = 2;
            // 
            // pnlStat1
            // 
            this.pnlStat1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(144)))), ((int)(((byte)(255)))));
            this.pnlStat1.Controls.Add(this.lblDoanhThu);
            this.pnlStat1.Controls.Add(this.lblDoanhThuValue);
            this.pnlStat1.Location = new System.Drawing.Point(20, 12);
            this.pnlStat1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlStat1.Name = "pnlStat1";
            this.pnlStat1.Size = new System.Drawing.Size(275, 93);
            this.pnlStat1.TabIndex = 0;
            // 
            // lblDoanhThu
            // 
            this.lblDoanhThu.AutoSize = true;
            this.lblDoanhThu.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblDoanhThu.ForeColor = System.Drawing.Color.White;
            this.lblDoanhThu.Location = new System.Drawing.Point(13, 12);
            this.lblDoanhThu.Name = "lblDoanhThu";
            this.lblDoanhThu.Size = new System.Drawing.Size(152, 28);
            this.lblDoanhThu.TabIndex = 0;
            this.lblDoanhThu.Text = "Tổng doanh thu";
            // 
            // lblDoanhThuValue
            // 
            this.lblDoanhThuValue.AutoSize = true;
            this.lblDoanhThuValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblDoanhThuValue.ForeColor = System.Drawing.Color.White;
            this.lblDoanhThuValue.Location = new System.Drawing.Point(13, 32);
            this.lblDoanhThuValue.Name = "lblDoanhThuValue";
            this.lblDoanhThuValue.Size = new System.Drawing.Size(40, 46);
            this.lblDoanhThuValue.TabIndex = 1;
            this.lblDoanhThuValue.Text = "0";
            // 
            // pnlStat2
            // 
            this.pnlStat2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(196)))), ((int)(((byte)(26)))));
            this.pnlStat2.Controls.Add(this.lblSoChuyenXe);
            this.pnlStat2.Controls.Add(this.lblSoChuyenXeValue);
            this.pnlStat2.Location = new System.Drawing.Point(315, 12);
            this.pnlStat2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlStat2.Name = "pnlStat2";
            this.pnlStat2.Size = new System.Drawing.Size(275, 93);
            this.pnlStat2.TabIndex = 1;
            // 
            // lblSoChuyenXe
            // 
            this.lblSoChuyenXe.AutoSize = true;
            this.lblSoChuyenXe.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSoChuyenXe.ForeColor = System.Drawing.Color.White;
            this.lblSoChuyenXe.Location = new System.Drawing.Point(13, 12);
            this.lblSoChuyenXe.Name = "lblSoChuyenXe";
            this.lblSoChuyenXe.Size = new System.Drawing.Size(126, 28);
            this.lblSoChuyenXe.TabIndex = 0;
            this.lblSoChuyenXe.Text = "Số chuyến xe";
            // 
            // lblSoChuyenXeValue
            // 
            this.lblSoChuyenXeValue.AutoSize = true;
            this.lblSoChuyenXeValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblSoChuyenXeValue.ForeColor = System.Drawing.Color.White;
            this.lblSoChuyenXeValue.Location = new System.Drawing.Point(13, 32);
            this.lblSoChuyenXeValue.Name = "lblSoChuyenXeValue";
            this.lblSoChuyenXeValue.Size = new System.Drawing.Size(40, 46);
            this.lblSoChuyenXeValue.TabIndex = 1;
            this.lblSoChuyenXeValue.Text = "0";
            // 
            // pnlStat3
            // 
            this.pnlStat3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(194)))), ((int)(((byte)(194)))));
            this.pnlStat3.Controls.Add(this.lblSoKhachHang);
            this.pnlStat3.Controls.Add(this.lblSoKhachHangValue);
            this.pnlStat3.Location = new System.Drawing.Point(610, 12);
            this.pnlStat3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlStat3.Name = "pnlStat3";
            this.pnlStat3.Size = new System.Drawing.Size(275, 93);
            this.pnlStat3.TabIndex = 2;
            // 
            // lblSoKhachHang
            // 
            this.lblSoKhachHang.AutoSize = true;
            this.lblSoKhachHang.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSoKhachHang.ForeColor = System.Drawing.Color.White;
            this.lblSoKhachHang.Location = new System.Drawing.Point(13, 12);
            this.lblSoKhachHang.Name = "lblSoKhachHang";
            this.lblSoKhachHang.Size = new System.Drawing.Size(140, 28);
            this.lblSoKhachHang.TabIndex = 0;
            this.lblSoKhachHang.Text = "Số khách hàng";
            // 
            // lblSoKhachHangValue
            // 
            this.lblSoKhachHangValue.AutoSize = true;
            this.lblSoKhachHangValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblSoKhachHangValue.ForeColor = System.Drawing.Color.White;
            this.lblSoKhachHangValue.Location = new System.Drawing.Point(13, 32);
            this.lblSoKhachHangValue.Name = "lblSoKhachHangValue";
            this.lblSoKhachHangValue.Size = new System.Drawing.Size(40, 46);
            this.lblSoKhachHangValue.TabIndex = 1;
            this.lblSoKhachHangValue.Text = "0";
            // 
            // pnlStat4
            // 
            this.pnlStat4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(140)))), ((int)(((byte)(22)))));
            this.pnlStat4.Controls.Add(this.lblSoVe);
            this.pnlStat4.Controls.Add(this.lblSoVeValue);
            this.pnlStat4.Location = new System.Drawing.Point(905, 12);
            this.pnlStat4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlStat4.Name = "pnlStat4";
            this.pnlStat4.Size = new System.Drawing.Size(275, 93);
            this.pnlStat4.TabIndex = 3;
            // 
            // lblSoVe
            // 
            this.lblSoVe.AutoSize = true;
            this.lblSoVe.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSoVe.ForeColor = System.Drawing.Color.White;
            this.lblSoVe.Location = new System.Drawing.Point(13, 12);
            this.lblSoVe.Name = "lblSoVe";
            this.lblSoVe.Size = new System.Drawing.Size(125, 28);
            this.lblSoVe.TabIndex = 0;
            this.lblSoVe.Text = "Số vé đã bán";
            // 
            // lblSoVeValue
            // 
            this.lblSoVeValue.AutoSize = true;
            this.lblSoVeValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblSoVeValue.ForeColor = System.Drawing.Color.White;
            this.lblSoVeValue.Location = new System.Drawing.Point(13, 32);
            this.lblSoVeValue.Name = "lblSoVeValue";
            this.lblSoVeValue.Size = new System.Drawing.Size(40, 46);
            this.lblSoVeValue.TabIndex = 1;
            this.lblSoVeValue.Text = "0";
            // 
            // pnlStat5
            // 
            this.pnlStat5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(46)))), ((int)(((byte)(209)))));
            this.pnlStat5.Controls.Add(this.lblTrungBinhVe);
            this.pnlStat5.Controls.Add(this.lblTrungBinhVeValue);
            this.pnlStat5.Location = new System.Drawing.Point(1200, 12);
            this.pnlStat5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlStat5.Name = "pnlStat5";
            this.pnlStat5.Size = new System.Drawing.Size(275, 93);
            this.pnlStat5.TabIndex = 4;
            // 
            // lblTrungBinhVe
            // 
            this.lblTrungBinhVe.AutoSize = true;
            this.lblTrungBinhVe.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTrungBinhVe.ForeColor = System.Drawing.Color.White;
            this.lblTrungBinhVe.Location = new System.Drawing.Point(13, 12);
            this.lblTrungBinhVe.Name = "lblTrungBinhVe";
            this.lblTrungBinhVe.Size = new System.Drawing.Size(161, 28);
            this.lblTrungBinhVe.TabIndex = 0;
            this.lblTrungBinhVe.Text = "Doanh thu TB/Vé";
            // 
            // lblTrungBinhVeValue
            // 
            this.lblTrungBinhVeValue.AutoSize = true;
            this.lblTrungBinhVeValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTrungBinhVeValue.ForeColor = System.Drawing.Color.White;
            this.lblTrungBinhVeValue.Location = new System.Drawing.Point(13, 32);
            this.lblTrungBinhVeValue.Name = "lblTrungBinhVeValue";
            this.lblTrungBinhVeValue.Size = new System.Drawing.Size(40, 46);
            this.lblTrungBinhVeValue.TabIndex = 1;
            this.lblTrungBinhVeValue.Text = "0";
            // 
            // pnlCharts
            // 
            this.pnlCharts.BackColor = System.Drawing.Color.White;
            this.pnlCharts.Controls.Add(this.chartTuyenDuong);
            this.pnlCharts.Controls.Add(this.chartDoanhThu);
            this.pnlCharts.Controls.Add(this.chartNhaXe);
            this.pnlCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCharts.Location = new System.Drawing.Point(0, 115);
            this.pnlCharts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlCharts.Name = "pnlCharts";
            this.pnlCharts.Padding = new System.Windows.Forms.Padding(18, 16, 18, 16);
            this.pnlCharts.Size = new System.Drawing.Size(1491, 614);
            this.pnlCharts.TabIndex = 3;
            // 
            // chartTuyenDuong
            // 
            chartArea1.Name = "ChartArea1";
            this.chartTuyenDuong.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartTuyenDuong.Legends.Add(legend1);
            this.chartTuyenDuong.Location = new System.Drawing.Point(500, 16);
            this.chartTuyenDuong.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chartTuyenDuong.Name = "chartTuyenDuong";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartTuyenDuong.Series.Add(series1);
            this.chartTuyenDuong.Size = new System.Drawing.Size(460, 520);
            this.chartTuyenDuong.TabIndex = 1;
            this.chartTuyenDuong.Text = "chart2";
            // 
            // chartDoanhThu
            // 
            chartArea2.Name = "ChartArea1";
            this.chartDoanhThu.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartDoanhThu.Legends.Add(legend2);
            this.chartDoanhThu.Location = new System.Drawing.Point(20, 16);
            this.chartDoanhThu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chartDoanhThu.Name = "chartDoanhThu";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartDoanhThu.Series.Add(series2);
            this.chartDoanhThu.Size = new System.Drawing.Size(460, 520);
            this.chartDoanhThu.TabIndex = 0;
            this.chartDoanhThu.Text = "chart1";
            // 
            // chartNhaXe
            // 
            chartArea3.Name = "ChartArea1";
            this.chartNhaXe.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartNhaXe.Legends.Add(legend3);
            this.chartNhaXe.Location = new System.Drawing.Point(980, 16);
            this.chartNhaXe.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chartNhaXe.Name = "chartNhaXe";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartNhaXe.Series.Add(series3);
            this.chartNhaXe.Size = new System.Drawing.Size(460, 520);
            this.chartNhaXe.TabIndex = 2;
            this.chartNhaXe.Text = "chartNhaXe";
            // 
            // tabChiTiet
            // 
            this.tabChiTiet.BackColor = System.Drawing.Color.White;
            this.tabChiTiet.Controls.Add(this.pnlDetailGrid);
            this.tabChiTiet.Controls.Add(this.pnlDetailHeader);
            this.tabChiTiet.Location = new System.Drawing.Point(4, 38);
            this.tabChiTiet.Name = "tabChiTiet";
            this.tabChiTiet.Padding = new System.Windows.Forms.Padding(18, 16, 18, 16);
            this.tabChiTiet.Size = new System.Drawing.Size(1491, 729);
            this.tabChiTiet.TabIndex = 1;
            this.tabChiTiet.Text = "Báo Cáo Chi Tiết";
            // 
            // pnlDetailHeader
            // 
            this.pnlDetailHeader.Controls.Add(this.btnExportCSV);
            this.pnlDetailHeader.Controls.Add(this.cboLoaiBaoCao);
            this.pnlDetailHeader.Controls.Add(this.lblSelectReport);
            this.pnlDetailHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDetailHeader.Location = new System.Drawing.Point(18, 16);
            this.pnlDetailHeader.Name = "pnlDetailHeader";
            this.pnlDetailHeader.Size = new System.Drawing.Size(1455, 56);
            this.pnlDetailHeader.TabIndex = 0;
            // 
            // lblSelectReport
            // 
            this.lblSelectReport.AutoSize = true;
            this.lblSelectReport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSelectReport.Location = new System.Drawing.Point(3, 16);
            this.lblSelectReport.Name = "lblSelectReport";
            this.lblSelectReport.Size = new System.Drawing.Size(161, 23);
            this.lblSelectReport.TabIndex = 0;
            this.lblSelectReport.Text = "Chọn loại báo cáo:";
            // 
            // cboLoaiBaoCao
            // 
            this.cboLoaiBaoCao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaiBaoCao.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboLoaiBaoCao.FormattingEnabled = true;
            this.cboLoaiBaoCao.Location = new System.Drawing.Point(180, 12);
            this.cboLoaiBaoCao.Name = "cboLoaiBaoCao";
            this.cboLoaiBaoCao.Size = new System.Drawing.Size(350, 31);
            this.cboLoaiBaoCao.TabIndex = 1;
            this.cboLoaiBaoCao.SelectedIndexChanged += new System.EventHandler(this.cboLoaiBaoCao_SelectedIndexChanged);
            // 
            // btnExportCSV
            // 
            this.btnExportCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportCSV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnExportCSV.FlatAppearance.BorderSize = 0;
            this.btnExportCSV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportCSV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExportCSV.ForeColor = System.Drawing.Color.White;
            this.btnExportCSV.Location = new System.Drawing.Point(1300, 10);
            this.btnExportCSV.Name = "btnExportCSV";
            this.btnExportCSV.Size = new System.Drawing.Size(150, 34);
            this.btnExportCSV.TabIndex = 2;
            this.btnExportCSV.Text = "Xuất CSV (Excel)";
            this.btnExportCSV.UseVisualStyleBackColor = false;
            this.btnExportCSV.Click += new System.EventHandler(this.btnExportCSV_Click);
            // 
            // pnlDetailGrid
            // 
            this.pnlDetailGrid.Controls.Add(this.dgvChiTiet);
            this.pnlDetailGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetailGrid.Location = new System.Drawing.Point(18, 72);
            this.pnlDetailGrid.Name = "pnlDetailGrid";
            this.pnlDetailGrid.Size = new System.Drawing.Size(1455, 641);
            this.pnlDetailGrid.TabIndex = 1;
            // 
            // dgvChiTiet
            // 
            this.dgvChiTiet.AllowUserToAddRows = false;
            this.dgvChiTiet.AllowUserToDeleteRows = false;
            this.dgvChiTiet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChiTiet.BackgroundColor = System.Drawing.Color.White;
            this.dgvChiTiet.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvChiTiet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChiTiet.Location = new System.Drawing.Point(0, 0);
            this.dgvChiTiet.Name = "dgvChiTiet";
            this.dgvChiTiet.ReadOnly = true;
            this.dgvChiTiet.RowHeadersVisible = false;
            this.dgvChiTiet.RowTemplate.Height = 24;
            this.dgvChiTiet.Size = new System.Drawing.Size(1455, 641);
            this.dgvChiTiet.TabIndex = 0;
            // 
            // UCThongKe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.tabControlThongKe);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlTop);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCThongKe";
            this.Size = new System.Drawing.Size(1499, 875);
            this.Load += new System.EventHandler(this.UCThongKe_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.tabControlThongKe.ResumeLayout(false);
            this.tabTongQuan.ResumeLayout(false);
            this.tabChiTiet.ResumeLayout(false);
            this.pnlStats.ResumeLayout(false);
            this.pnlStat1.ResumeLayout(false);
            this.pnlStat1.PerformLayout();
            this.pnlStat2.ResumeLayout(false);
            this.pnlStat2.PerformLayout();
            this.pnlStat3.ResumeLayout(false);
            this.pnlStat3.PerformLayout();
            this.pnlStat4.ResumeLayout(false);
            this.pnlStat4.PerformLayout();
            this.pnlStat5.ResumeLayout(false);
            this.pnlStat5.PerformLayout();
            this.pnlCharts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartTuyenDuong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDoanhThu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartNhaXe)).EndInit();
            this.pnlDetailHeader.ResumeLayout(false);
            this.pnlDetailHeader.PerformLayout();
            this.pnlDetailGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Panel pnlFilter;
		private System.Windows.Forms.DateTimePicker dtpTuNgay;
		private System.Windows.Forms.Label lblTuNgay;
		private System.Windows.Forms.Label lblDenNgay;
		private System.Windows.Forms.DateTimePicker dtpDenNgay;
		private System.Windows.Forms.Button btnThongKe;
		
		private System.Windows.Forms.TabControl tabControlThongKe;
		private System.Windows.Forms.TabPage tabTongQuan;
		private System.Windows.Forms.TabPage tabChiTiet;
		
		private System.Windows.Forms.Panel pnlStats;
		private System.Windows.Forms.Panel pnlStat1;
		private System.Windows.Forms.Label lblDoanhThu;
		private System.Windows.Forms.Label lblDoanhThuValue;
		private System.Windows.Forms.Panel pnlStat2;
		private System.Windows.Forms.Label lblSoChuyenXe;
		private System.Windows.Forms.Label lblSoChuyenXeValue;
		private System.Windows.Forms.Panel pnlStat3;
		private System.Windows.Forms.Label lblSoKhachHang;
		private System.Windows.Forms.Label lblSoKhachHangValue;
		private System.Windows.Forms.Panel pnlStat4;
		private System.Windows.Forms.Label lblSoVe;
		private System.Windows.Forms.Label lblSoVeValue;
		private System.Windows.Forms.Panel pnlStat5;
		private System.Windows.Forms.Label lblTrungBinhVe;
		private System.Windows.Forms.Label lblTrungBinhVeValue;
		
		private System.Windows.Forms.Panel pnlCharts;
		private System.Windows.Forms.DataVisualization.Charting.Chart chartDoanhThu;
		private System.Windows.Forms.DataVisualization.Charting.Chart chartTuyenDuong;
		private System.Windows.Forms.DataVisualization.Charting.Chart chartNhaXe;
		
		private System.Windows.Forms.Panel pnlDetailHeader;
		private System.Windows.Forms.Label lblSelectReport;
		private System.Windows.Forms.ComboBox cboLoaiBaoCao;
		private System.Windows.Forms.Button btnExportCSV;
		private System.Windows.Forms.Panel pnlDetailGrid;
		private System.Windows.Forms.DataGridView dgvChiTiet;
	}
}
