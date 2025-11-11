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
			this.pnlTop = new System.Windows.Forms.Panel();
			this.lblTitle = new System.Windows.Forms.Label();
			this.pnlFilter = new System.Windows.Forms.Panel();
			this.btnThongKe = new System.Windows.Forms.Button();
			this.dtpDenNgay = new System.Windows.Forms.DateTimePicker();
			this.dtpTuNgay = new System.Windows.Forms.DateTimePicker();
			this.lblDenNgay = new System.Windows.Forms.Label();
			this.lblTuNgay = new System.Windows.Forms.Label();
			this.pnlStats = new System.Windows.Forms.Panel();
			this.pnlStat4 = new System.Windows.Forms.Panel();
			this.lblSoVe = new System.Windows.Forms.Label();
			this.lblSoVeValue = new System.Windows.Forms.Label();
			this.pnlStat3 = new System.Windows.Forms.Panel();
			this.lblSoKhachHang = new System.Windows.Forms.Label();
			this.lblSoKhachHangValue = new System.Windows.Forms.Label();
			this.pnlStat2 = new System.Windows.Forms.Panel();
			this.lblSoChuyenXe = new System.Windows.Forms.Label();
			this.lblSoChuyenXeValue = new System.Windows.Forms.Label();
			this.pnlStat1 = new System.Windows.Forms.Panel();
			this.lblDoanhThu = new System.Windows.Forms.Label();
			this.lblDoanhThuValue = new System.Windows.Forms.Label();
			this.pnlCharts = new System.Windows.Forms.Panel();
			this.chartTuyenDuong = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.chartDoanhThu = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.pnlTop.SuspendLayout();
			this.pnlFilter.SuspendLayout();
			this.pnlStats.SuspendLayout();
			this.pnlStat4.SuspendLayout();
			this.pnlStat3.SuspendLayout();
			this.pnlStat2.SuspendLayout();
			this.pnlStat1.SuspendLayout();
			this.pnlCharts.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chartTuyenDuong)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartDoanhThu)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlTop
			// 
			this.pnlTop.BackColor = System.Drawing.Color.White;
			this.pnlTop.Controls.Add(this.lblTitle);
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTop.Location = new System.Drawing.Point(0, 0);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(1322, 60);
			this.pnlTop.TabIndex = 0;
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
			this.lblTitle.Location = new System.Drawing.Point(27, 9);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(181, 48);
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
			this.pnlFilter.Location = new System.Drawing.Point(0, 60);
			this.pnlFilter.Name = "pnlFilter";
			this.pnlFilter.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
			this.pnlFilter.Size = new System.Drawing.Size(1322, 70);
			this.pnlFilter.TabIndex = 1;
			// 
			// btnThongKe
			// 
			this.btnThongKe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
			this.btnThongKe.FlatAppearance.BorderSize = 0;
			this.btnThongKe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnThongKe.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnThongKe.ForeColor = System.Drawing.Color.White;
			this.btnThongKe.Location = new System.Drawing.Point(750, 15);
			this.btnThongKe.Name = "btnThongKe";
			this.btnThongKe.Size = new System.Drawing.Size(120, 40);
			this.btnThongKe.TabIndex = 4;
			this.btnThongKe.Text = "Thống Kê";
			this.btnThongKe.UseVisualStyleBackColor = false;
			this.btnThongKe.Click += new System.EventHandler(this.btnThongKe_Click);
			// 
			// dtpDenNgay
			// 
			this.dtpDenNgay.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.dtpDenNgay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpDenNgay.Location = new System.Drawing.Point(500, 20);
			this.dtpDenNgay.Name = "dtpDenNgay";
			this.dtpDenNgay.Size = new System.Drawing.Size(200, 34);
			this.dtpDenNgay.TabIndex = 3;
			// 
			// dtpTuNgay
			// 
			this.dtpTuNgay.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.dtpTuNgay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpTuNgay.Location = new System.Drawing.Point(150, 20);
			this.dtpTuNgay.Name = "dtpTuNgay";
			this.dtpTuNgay.Size = new System.Drawing.Size(200, 34);
			this.dtpTuNgay.TabIndex = 2;
			// 
			// lblDenNgay
			// 
			this.lblDenNgay.AutoSize = true;
			this.lblDenNgay.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblDenNgay.Location = new System.Drawing.Point(400, 23);
			this.lblDenNgay.Name = "lblDenNgay";
			this.lblDenNgay.Size = new System.Drawing.Size(99, 28);
			this.lblDenNgay.TabIndex = 1;
			this.lblDenNgay.Text = "Đến ngày:";
			// 
			// lblTuNgay
			// 
			this.lblTuNgay.AutoSize = true;
			this.lblTuNgay.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblTuNgay.Location = new System.Drawing.Point(30, 23);
			this.lblTuNgay.Name = "lblTuNgay";
			this.lblTuNgay.Size = new System.Drawing.Size(86, 28);
			this.lblTuNgay.TabIndex = 0;
			this.lblTuNgay.Text = "Từ ngày:";
			// 
			// pnlStats
			// 
			this.pnlStats.Controls.Add(this.pnlStat4);
			this.pnlStats.Controls.Add(this.pnlStat3);
			this.pnlStats.Controls.Add(this.pnlStat2);
			this.pnlStats.Controls.Add(this.pnlStat1);
			this.pnlStats.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlStats.Location = new System.Drawing.Point(0, 130);
			this.pnlStats.Name = "pnlStats";
			this.pnlStats.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
			this.pnlStats.Size = new System.Drawing.Size(1322, 144);
			this.pnlStats.TabIndex = 2;
			// 
			// pnlStat4
			// 
			this.pnlStat4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
			this.pnlStat4.Controls.Add(this.lblSoVe);
			this.pnlStat4.Controls.Add(this.lblSoVeValue);
			this.pnlStat4.Location = new System.Drawing.Point(1000, 15);
			this.pnlStat4.Name = "pnlStat4";
			this.pnlStat4.Size = new System.Drawing.Size(300, 116);
			this.pnlStat4.TabIndex = 3;
			// 
			// lblSoVe
			// 
			this.lblSoVe.AutoSize = true;
			this.lblSoVe.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.lblSoVe.ForeColor = System.Drawing.Color.White;
			this.lblSoVe.Location = new System.Drawing.Point(15, 15);
			this.lblSoVe.Name = "lblSoVe";
			this.lblSoVe.Size = new System.Drawing.Size(153, 32);
			this.lblSoVe.TabIndex = 0;
			this.lblSoVe.Text = "Số vé đã bán";
			// 
			// lblSoVeValue
			// 
			this.lblSoVeValue.AutoSize = true;
			this.lblSoVeValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.lblSoVeValue.ForeColor = System.Drawing.Color.White;
			this.lblSoVeValue.Location = new System.Drawing.Point(15, 40);
			this.lblSoVeValue.Name = "lblSoVeValue";
			this.lblSoVeValue.Size = new System.Drawing.Size(46, 54);
			this.lblSoVeValue.TabIndex = 1;
			this.lblSoVeValue.Text = "0";
			// 
			// pnlStat3
			// 
			this.pnlStat3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.pnlStat3.Controls.Add(this.lblSoKhachHang);
			this.pnlStat3.Controls.Add(this.lblSoKhachHangValue);
			this.pnlStat3.Location = new System.Drawing.Point(680, 15);
			this.pnlStat3.Name = "pnlStat3";
			this.pnlStat3.Size = new System.Drawing.Size(300, 116);
			this.pnlStat3.TabIndex = 2;
			// 
			// lblSoKhachHang
			// 
			this.lblSoKhachHang.AutoSize = true;
			this.lblSoKhachHang.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.lblSoKhachHang.ForeColor = System.Drawing.Color.White;
			this.lblSoKhachHang.Location = new System.Drawing.Point(15, 15);
			this.lblSoKhachHang.Name = "lblSoKhachHang";
			this.lblSoKhachHang.Size = new System.Drawing.Size(172, 32);
			this.lblSoKhachHang.TabIndex = 0;
			this.lblSoKhachHang.Text = "Số khách hàng";
			// 
			// lblSoKhachHangValue
			// 
			this.lblSoKhachHangValue.AutoSize = true;
			this.lblSoKhachHangValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.lblSoKhachHangValue.ForeColor = System.Drawing.Color.White;
			this.lblSoKhachHangValue.Location = new System.Drawing.Point(15, 40);
			this.lblSoKhachHangValue.Name = "lblSoKhachHangValue";
			this.lblSoKhachHangValue.Size = new System.Drawing.Size(46, 54);
			this.lblSoKhachHangValue.TabIndex = 1;
			this.lblSoKhachHangValue.Text = "0";
			// 
			// pnlStat2
			// 
			this.pnlStat2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
			this.pnlStat2.Controls.Add(this.lblSoChuyenXe);
			this.pnlStat2.Controls.Add(this.lblSoChuyenXeValue);
			this.pnlStat2.Location = new System.Drawing.Point(360, 15);
			this.pnlStat2.Name = "pnlStat2";
			this.pnlStat2.Size = new System.Drawing.Size(300, 116);
			this.pnlStat2.TabIndex = 1;
			// 
			// lblSoChuyenXe
			// 
			this.lblSoChuyenXe.AutoSize = true;
			this.lblSoChuyenXe.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.lblSoChuyenXe.ForeColor = System.Drawing.Color.White;
			this.lblSoChuyenXe.Location = new System.Drawing.Point(15, 15);
			this.lblSoChuyenXe.Name = "lblSoChuyenXe";
			this.lblSoChuyenXe.Size = new System.Drawing.Size(157, 32);
			this.lblSoChuyenXe.TabIndex = 0;
			this.lblSoChuyenXe.Text = "Số chuyến xe";
			// 
			// lblSoChuyenXeValue
			// 
			this.lblSoChuyenXeValue.AutoSize = true;
			this.lblSoChuyenXeValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.lblSoChuyenXeValue.ForeColor = System.Drawing.Color.White;
			this.lblSoChuyenXeValue.Location = new System.Drawing.Point(15, 40);
			this.lblSoChuyenXeValue.Name = "lblSoChuyenXeValue";
			this.lblSoChuyenXeValue.Size = new System.Drawing.Size(46, 54);
			this.lblSoChuyenXeValue.TabIndex = 1;
			this.lblSoChuyenXeValue.Text = "0";
			// 
			// pnlStat1
			// 
			this.pnlStat1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
			this.pnlStat1.Controls.Add(this.lblDoanhThu);
			this.pnlStat1.Controls.Add(this.lblDoanhThuValue);
			this.pnlStat1.Location = new System.Drawing.Point(40, 15);
			this.pnlStat1.Name = "pnlStat1";
			this.pnlStat1.Size = new System.Drawing.Size(300, 116);
			this.pnlStat1.TabIndex = 0;
			// 
			// lblDoanhThu
			// 
			this.lblDoanhThu.AutoSize = true;
			this.lblDoanhThu.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.lblDoanhThu.ForeColor = System.Drawing.Color.White;
			this.lblDoanhThu.Location = new System.Drawing.Point(15, 15);
			this.lblDoanhThu.Name = "lblDoanhThu";
			this.lblDoanhThu.Size = new System.Drawing.Size(187, 32);
			this.lblDoanhThu.TabIndex = 0;
			this.lblDoanhThu.Text = "Tổng doanh thu";
			// 
			// lblDoanhThuValue
			// 
			this.lblDoanhThuValue.AutoSize = true;
			this.lblDoanhThuValue.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.lblDoanhThuValue.ForeColor = System.Drawing.Color.White;
			this.lblDoanhThuValue.Location = new System.Drawing.Point(15, 40);
			this.lblDoanhThuValue.Name = "lblDoanhThuValue";
			this.lblDoanhThuValue.Size = new System.Drawing.Size(46, 54);
			this.lblDoanhThuValue.TabIndex = 1;
			this.lblDoanhThuValue.Text = "0";
			// 
			// pnlCharts
			// 
			this.pnlCharts.BackColor = System.Drawing.Color.White;
			this.pnlCharts.Controls.Add(this.chartTuyenDuong);
			this.pnlCharts.Controls.Add(this.chartDoanhThu);
			this.pnlCharts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlCharts.Location = new System.Drawing.Point(0, 274);
			this.pnlCharts.Name = "pnlCharts";
			this.pnlCharts.Padding = new System.Windows.Forms.Padding(20);
			this.pnlCharts.Size = new System.Drawing.Size(1322, 457);
			this.pnlCharts.TabIndex = 3;
			// 
			// chartTuyenDuong
			// 
			chartArea1.Name = "ChartArea1";
			this.chartTuyenDuong.ChartAreas.Add(chartArea1);
			this.chartTuyenDuong.Dock = System.Windows.Forms.DockStyle.Fill;
			legend1.Name = "Legend1";
			this.chartTuyenDuong.Legends.Add(legend1);
			this.chartTuyenDuong.Location = new System.Drawing.Point(661, 20);
			this.chartTuyenDuong.Name = "chartTuyenDuong";
			series1.ChartArea = "ChartArea1";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
			series1.Legend = "Legend1";
			series1.Name = "Series1";
			this.chartTuyenDuong.Series.Add(series1);
			this.chartTuyenDuong.Size = new System.Drawing.Size(641, 417);
			this.chartTuyenDuong.TabIndex = 1;
			this.chartTuyenDuong.Text = "chart2";
			// 
			// chartDoanhThu
			// 
			chartArea2.Name = "ChartArea1";
			this.chartDoanhThu.ChartAreas.Add(chartArea2);
			this.chartDoanhThu.Dock = System.Windows.Forms.DockStyle.Left;
			legend2.Name = "Legend1";
			this.chartDoanhThu.Legends.Add(legend2);
			this.chartDoanhThu.Location = new System.Drawing.Point(20, 20);
			this.chartDoanhThu.Name = "chartDoanhThu";
			series2.ChartArea = "ChartArea1";
			series2.Legend = "Legend1";
			series2.Name = "Series1";
			this.chartDoanhThu.Series.Add(series2);
			this.chartDoanhThu.Size = new System.Drawing.Size(641, 417);
			this.chartDoanhThu.TabIndex = 0;
			this.chartDoanhThu.Text = "chart1";
			// 
			// UCThongKe
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.Controls.Add(this.pnlCharts);
			this.Controls.Add(this.pnlStats);
			this.Controls.Add(this.pnlFilter);
			this.Controls.Add(this.pnlTop);
			this.Name = "UCThongKe";
			this.Size = new System.Drawing.Size(1322, 731);
			this.Load += new System.EventHandler(this.UCThongKe_Load);
			this.pnlTop.ResumeLayout(false);
			this.pnlTop.PerformLayout();
			this.pnlFilter.ResumeLayout(false);
			this.pnlFilter.PerformLayout();
			this.pnlStats.ResumeLayout(false);
			this.pnlStat4.ResumeLayout(false);
			this.pnlStat4.PerformLayout();
			this.pnlStat3.ResumeLayout(false);
			this.pnlStat3.PerformLayout();
			this.pnlStat2.ResumeLayout(false);
			this.pnlStat2.PerformLayout();
			this.pnlStat1.ResumeLayout(false);
			this.pnlStat1.PerformLayout();
			this.pnlCharts.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.chartTuyenDuong)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartDoanhThu)).EndInit();
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
		private System.Windows.Forms.Panel pnlCharts;
		private System.Windows.Forms.DataVisualization.Charting.Chart chartDoanhThu;
		private System.Windows.Forms.DataVisualization.Charting.Chart chartTuyenDuong;
	}
}
