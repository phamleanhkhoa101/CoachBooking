namespace QLBXMD.GUI.Admin
{
	partial class UCTrangChu
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
			this.pnlHero = new System.Windows.Forms.Panel();
			this.lblHeroBadge = new System.Windows.Forms.Label();
			this.lblSubtitle = new System.Windows.Forms.Label();
			this.lblWelcome = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.pnlCard1 = new System.Windows.Forms.Panel();
			this.lblKhachHang = new System.Windows.Forms.Label();
			this.lblKhachHangValue = new System.Windows.Forms.Label();
			this.lblIcon1 = new System.Windows.Forms.Label();
			this.pnlCard2 = new System.Windows.Forms.Panel();
			this.lblSoChuyenXe = new System.Windows.Forms.Label();
			this.lblSoChuyenXeValue = new System.Windows.Forms.Label();
			this.lblIcon2 = new System.Windows.Forms.Label();
			this.pnlCard3 = new System.Windows.Forms.Panel();
			this.lblTongDoanhThu = new System.Windows.Forms.Label();
			this.lblTongDoanhThuValue = new System.Windows.Forms.Label();
			this.lblIcon3 = new System.Windows.Forms.Label();
			this.pnlCard4 = new System.Windows.Forms.Panel();
			this.lblSoVe = new System.Windows.Forms.Label();
			this.lblSoVeValue = new System.Windows.Forms.Label();
			this.lblIcon4 = new System.Windows.Forms.Label();
			this.pnlFilter = new System.Windows.Forms.Panel();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.dtpTo = new System.Windows.Forms.DateTimePicker();
			this.lblTo = new System.Windows.Forms.Label();
			this.dtpFrom = new System.Windows.Forms.DateTimePicker();
			this.lblFrom = new System.Windows.Forms.Label();
			this.lblFilterTitle = new System.Windows.Forms.Label();
			this.pnlTableWrap = new System.Windows.Forms.Panel();
			this.dgvVeSold = new System.Windows.Forms.DataGridView();
			this.pnlTableHeader = new System.Windows.Forms.Panel();
			this.lblTableSubtitle = new System.Windows.Forms.Label();
			this.lblTableTitle = new System.Windows.Forms.Label();
			this.pnlHero.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.pnlCard1.SuspendLayout();
			this.pnlCard2.SuspendLayout();
			this.pnlCard3.SuspendLayout();
			this.pnlCard4.SuspendLayout();
			this.pnlFilter.SuspendLayout();
			this.pnlTableWrap.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvVeSold)).BeginInit();
			this.pnlTableHeader.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlHero
			// 
			this.pnlHero.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlHero.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(253)))), ((int)(((byte)(250)))));
			this.pnlHero.Controls.Add(this.lblHeroBadge);
			this.pnlHero.Controls.Add(this.lblSubtitle);
			this.pnlHero.Controls.Add(this.lblWelcome);
			this.pnlHero.Location = new System.Drawing.Point(24, 20);
			this.pnlHero.Name = "pnlHero";
			this.pnlHero.Size = new System.Drawing.Size(1300, 130);
			this.pnlHero.TabIndex = 0;
			// 
			// lblHeroBadge
			// 
			this.lblHeroBadge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblHeroBadge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(251)))), ((int)(((byte)(241)))));
			this.lblHeroBadge.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lblHeroBadge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(118)))), ((int)(((byte)(110)))));
			this.lblHeroBadge.Location = new System.Drawing.Point(1090, 22);
			this.lblHeroBadge.Name = "lblHeroBadge";
			this.lblHeroBadge.Size = new System.Drawing.Size(180, 34);
			this.lblHeroBadge.TabIndex = 2;
			this.lblHeroBadge.Text = "ADMIN DASHBOARD";
			this.lblHeroBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblSubtitle
			// 
			this.lblSubtitle.AutoSize = true;
			this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
			this.lblSubtitle.Location = new System.Drawing.Point(32, 78);
			this.lblSubtitle.Name = "lblSubtitle";
			this.lblSubtitle.Size = new System.Drawing.Size(426, 30);
			this.lblSubtitle.TabIndex = 1;
			this.lblSubtitle.Text = "Tổng quan hoạt động bán vé và vận hành";
			// 
			// lblWelcome
			// 
			this.lblWelcome.AutoSize = true;
			this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
			this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
			this.lblWelcome.Location = new System.Drawing.Point(28, 20);
			this.lblWelcome.Name = "lblWelcome";
			this.lblWelcome.Size = new System.Drawing.Size(404, 60);
			this.lblWelcome.TabIndex = 0;
			this.lblWelcome.Text = "Trang chủ quản trị";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel1.Controls.Add(this.pnlCard1);
			this.flowLayoutPanel1.Controls.Add(this.pnlCard2);
			this.flowLayoutPanel1.Controls.Add(this.pnlCard3);
			this.flowLayoutPanel1.Controls.Add(this.pnlCard4);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(24, 165);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(1300, 150);
			this.flowLayoutPanel1.TabIndex = 1;
			// 
			// pnlCard1
			// 
			this.pnlCard1.BackColor = System.Drawing.Color.White;
			this.pnlCard1.Controls.Add(this.lblKhachHang);
			this.pnlCard1.Controls.Add(this.lblKhachHangValue);
			this.pnlCard1.Controls.Add(this.lblIcon1);
			this.pnlCard1.Location = new System.Drawing.Point(0, 0);
			this.pnlCard1.Margin = new System.Windows.Forms.Padding(0, 0, 18, 0);
			this.pnlCard1.Name = "pnlCard1";
			this.pnlCard1.Size = new System.Drawing.Size(311, 136);
			this.pnlCard1.TabIndex = 0;
			// 
			// lblKhachHang
			// 
			this.lblKhachHang.AutoSize = true;
			this.lblKhachHang.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblKhachHang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
			this.lblKhachHang.Location = new System.Drawing.Point(26, 92);
			this.lblKhachHang.Name = "lblKhachHang";
			this.lblKhachHang.Size = new System.Drawing.Size(103, 28);
			this.lblKhachHang.TabIndex = 2;
			this.lblKhachHang.Text = "Khách hàng";
			// 
			// lblKhachHangValue
			// 
			this.lblKhachHangValue.AutoSize = true;
			this.lblKhachHangValue.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Bold);
			this.lblKhachHangValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
			this.lblKhachHangValue.Location = new System.Drawing.Point(21, 24);
			this.lblKhachHangValue.Name = "lblKhachHangValue";
			this.lblKhachHangValue.Size = new System.Drawing.Size(57, 67);
			this.lblKhachHangValue.TabIndex = 1;
			this.lblKhachHangValue.Text = "0";
			// 
			// lblIcon1
			// 
			this.lblIcon1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(247)))), ((int)(((byte)(237)))));
			this.lblIcon1.Font = new System.Drawing.Font("Segoe UI Emoji", 18F);
			this.lblIcon1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(88)))), ((int)(((byte)(12)))));
			this.lblIcon1.Location = new System.Drawing.Point(235, 22);
			this.lblIcon1.Name = "lblIcon1";
			this.lblIcon1.Size = new System.Drawing.Size(52, 52);
			this.lblIcon1.TabIndex = 0;
			this.lblIcon1.Text = "👥";
			this.lblIcon1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlCard2
			// 
			this.pnlCard2.BackColor = System.Drawing.Color.White;
			this.pnlCard2.Controls.Add(this.lblSoChuyenXe);
			this.pnlCard2.Controls.Add(this.lblSoChuyenXeValue);
			this.pnlCard2.Controls.Add(this.lblIcon2);
			this.pnlCard2.Location = new System.Drawing.Point(329, 0);
			this.pnlCard2.Margin = new System.Windows.Forms.Padding(0, 0, 18, 0);
			this.pnlCard2.Name = "pnlCard2";
			this.pnlCard2.Size = new System.Drawing.Size(311, 136);
			this.pnlCard2.TabIndex = 1;
			// 
			// lblSoChuyenXe
			// 
			this.lblSoChuyenXe.AutoSize = true;
			this.lblSoChuyenXe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblSoChuyenXe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
			this.lblSoChuyenXe.Location = new System.Drawing.Point(26, 92);
			this.lblSoChuyenXe.Name = "lblSoChuyenXe";
			this.lblSoChuyenXe.Size = new System.Drawing.Size(120, 28);
			this.lblSoChuyenXe.TabIndex = 2;
			this.lblSoChuyenXe.Text = "Số chuyến xe";
			// 
			// lblSoChuyenXeValue
			// 
			this.lblSoChuyenXeValue.AutoSize = true;
			this.lblSoChuyenXeValue.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Bold);
			this.lblSoChuyenXeValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
			this.lblSoChuyenXeValue.Location = new System.Drawing.Point(21, 24);
			this.lblSoChuyenXeValue.Name = "lblSoChuyenXeValue";
			this.lblSoChuyenXeValue.Size = new System.Drawing.Size(57, 67);
			this.lblSoChuyenXeValue.TabIndex = 1;
			this.lblSoChuyenXeValue.Text = "0";
			// 
			// lblIcon2
			// 
			this.lblIcon2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
			this.lblIcon2.Font = new System.Drawing.Font("Segoe UI Emoji", 18F);
			this.lblIcon2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
			this.lblIcon2.Location = new System.Drawing.Point(235, 22);
			this.lblIcon2.Name = "lblIcon2";
			this.lblIcon2.Size = new System.Drawing.Size(52, 52);
			this.lblIcon2.TabIndex = 0;
			this.lblIcon2.Text = "🚌";
			this.lblIcon2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlCard3
			// 
			this.pnlCard3.BackColor = System.Drawing.Color.White;
			this.pnlCard3.Controls.Add(this.lblTongDoanhThu);
			this.pnlCard3.Controls.Add(this.lblTongDoanhThuValue);
			this.pnlCard3.Controls.Add(this.lblIcon3);
			this.pnlCard3.Location = new System.Drawing.Point(658, 0);
			this.pnlCard3.Margin = new System.Windows.Forms.Padding(0, 0, 18, 0);
			this.pnlCard3.Name = "pnlCard3";
			this.pnlCard3.Size = new System.Drawing.Size(311, 136);
			this.pnlCard3.TabIndex = 2;
			// 
			// lblTongDoanhThu
			// 
			this.lblTongDoanhThu.AutoSize = true;
			this.lblTongDoanhThu.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblTongDoanhThu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
			this.lblTongDoanhThu.Location = new System.Drawing.Point(26, 92);
			this.lblTongDoanhThu.Name = "lblTongDoanhThu";
			this.lblTongDoanhThu.Size = new System.Drawing.Size(177, 28);
			this.lblTongDoanhThu.TabIndex = 2;
			this.lblTongDoanhThu.Text = "Doanh thu trong kỳ";
			// 
			// lblTongDoanhThuValue
			// 
			this.lblTongDoanhThuValue.AutoSize = true;
			this.lblTongDoanhThuValue.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Bold);
			this.lblTongDoanhThuValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
			this.lblTongDoanhThuValue.Location = new System.Drawing.Point(20, 29);
			this.lblTongDoanhThuValue.Name = "lblTongDoanhThuValue";
			this.lblTongDoanhThuValue.Size = new System.Drawing.Size(111, 51);
			this.lblTongDoanhThuValue.TabIndex = 1;
			this.lblTongDoanhThuValue.Text = "0 VNĐ";
			// 
			// lblIcon3
			// 
			this.lblIcon3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(253)))), ((int)(((byte)(245)))));
			this.lblIcon3.Font = new System.Drawing.Font("Segoe UI Emoji", 18F);
			this.lblIcon3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
			this.lblIcon3.Location = new System.Drawing.Point(235, 22);
			this.lblIcon3.Name = "lblIcon3";
			this.lblIcon3.Size = new System.Drawing.Size(52, 52);
			this.lblIcon3.TabIndex = 0;
			this.lblIcon3.Text = "💰";
			this.lblIcon3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlCard4
			// 
			this.pnlCard4.BackColor = System.Drawing.Color.White;
			this.pnlCard4.Controls.Add(this.lblSoVe);
			this.pnlCard4.Controls.Add(this.lblSoVeValue);
			this.pnlCard4.Controls.Add(this.lblIcon4);
			this.pnlCard4.Location = new System.Drawing.Point(987, 0);
			this.pnlCard4.Margin = new System.Windows.Forms.Padding(0);
			this.pnlCard4.Name = "pnlCard4";
			this.pnlCard4.Size = new System.Drawing.Size(311, 136);
			this.pnlCard4.TabIndex = 3;
			// 
			// lblSoVe
			// 
			this.lblSoVe.AutoSize = true;
			this.lblSoVe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblSoVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
			this.lblSoVe.Location = new System.Drawing.Point(26, 92);
			this.lblSoVe.Name = "lblSoVe";
			this.lblSoVe.Size = new System.Drawing.Size(124, 28);
			this.lblSoVe.TabIndex = 2;
			this.lblSoVe.Text = "Số vé đã bán";
			// 
			// lblSoVeValue
			// 
			this.lblSoVeValue.AutoSize = true;
			this.lblSoVeValue.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Bold);
			this.lblSoVeValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
			this.lblSoVeValue.Location = new System.Drawing.Point(21, 24);
			this.lblSoVeValue.Name = "lblSoVeValue";
			this.lblSoVeValue.Size = new System.Drawing.Size(57, 67);
			this.lblSoVeValue.TabIndex = 1;
			this.lblSoVeValue.Text = "0";
			// 
			// lblIcon4
			// 
			this.lblIcon4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
			this.lblIcon4.Font = new System.Drawing.Font("Segoe UI Emoji", 18F);
			this.lblIcon4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
			this.lblIcon4.Location = new System.Drawing.Point(235, 22);
			this.lblIcon4.Name = "lblIcon4";
			this.lblIcon4.Size = new System.Drawing.Size(52, 52);
			this.lblIcon4.TabIndex = 0;
			this.lblIcon4.Text = "🎫";
			this.lblIcon4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlFilter
			// 
			this.pnlFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlFilter.BackColor = System.Drawing.Color.White;
			this.pnlFilter.Controls.Add(this.btnRefresh);
			this.pnlFilter.Controls.Add(this.dtpTo);
			this.pnlFilter.Controls.Add(this.lblTo);
			this.pnlFilter.Controls.Add(this.dtpFrom);
			this.pnlFilter.Controls.Add(this.lblFrom);
			this.pnlFilter.Controls.Add(this.lblFilterTitle);
			this.pnlFilter.Location = new System.Drawing.Point(24, 330);
			this.pnlFilter.Name = "pnlFilter";
			this.pnlFilter.Size = new System.Drawing.Size(1300, 84);
			this.pnlFilter.TabIndex = 2;
			// 
			// btnRefresh
			// 
			this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(184)))), ((int)(((byte)(166)))));
			this.btnRefresh.FlatAppearance.BorderSize = 0;
			this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnRefresh.ForeColor = System.Drawing.Color.White;
			this.btnRefresh.Location = new System.Drawing.Point(941, 23);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(170, 38);
			this.btnRefresh.TabIndex = 5;
			this.btnRefresh.Text = "Làm mới dữ liệu";
			this.btnRefresh.UseVisualStyleBackColor = false;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// dtpTo
			// 
			this.dtpTo.CalendarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
			this.dtpTo.CustomFormat = "dd/MM/yyyy";
			this.dtpTo.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpTo.Location = new System.Drawing.Point(725, 27);
			this.dtpTo.Name = "dtpTo";
			this.dtpTo.Size = new System.Drawing.Size(170, 34);
			this.dtpTo.TabIndex = 4;
			// 
			// lblTo
			// 
			this.lblTo.AutoSize = true;
			this.lblTo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lblTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
			this.lblTo.Location = new System.Drawing.Point(626, 30);
			this.lblTo.Name = "lblTo";
			this.lblTo.Size = new System.Drawing.Size(93, 28);
			this.lblTo.TabIndex = 3;
			this.lblTo.Text = "Đến ngày";
			// 
			// dtpFrom
			// 
			this.dtpFrom.CalendarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
			this.dtpFrom.CustomFormat = "dd/MM/yyyy";
			this.dtpFrom.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpFrom.Location = new System.Drawing.Point(409, 27);
			this.dtpFrom.Name = "dtpFrom";
			this.dtpFrom.Size = new System.Drawing.Size(170, 34);
			this.dtpFrom.TabIndex = 2;
			// 
			// lblFrom
			// 
			this.lblFrom.AutoSize = true;
			this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lblFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
			this.lblFrom.Location = new System.Drawing.Point(293, 30);
			this.lblFrom.Name = "lblFrom";
			this.lblFrom.Size = new System.Drawing.Size(84, 28);
			this.lblFrom.TabIndex = 1;
			this.lblFrom.Text = "Từ ngày";
			// 
			// lblFilterTitle
			// 
			this.lblFilterTitle.AutoSize = true;
			this.lblFilterTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.lblFilterTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
			this.lblFilterTitle.Location = new System.Drawing.Point(26, 26);
			this.lblFilterTitle.Name = "lblFilterTitle";
			this.lblFilterTitle.Size = new System.Drawing.Size(191, 32);
			this.lblFilterTitle.TabIndex = 0;
			this.lblFilterTitle.Text = "Bộ lọc thống kê";
			// 
			// pnlTableWrap
			// 
			this.pnlTableWrap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlTableWrap.BackColor = System.Drawing.Color.White;
			this.pnlTableWrap.Controls.Add(this.dgvVeSold);
			this.pnlTableWrap.Controls.Add(this.pnlTableHeader);
			this.pnlTableWrap.Location = new System.Drawing.Point(24, 432);
			this.pnlTableWrap.Name = "pnlTableWrap";
			this.pnlTableWrap.Size = new System.Drawing.Size(1300, 275);
			this.pnlTableWrap.TabIndex = 3;
			// 
			// dgvVeSold
			// 
			this.dgvVeSold.AllowUserToAddRows = false;
			this.dgvVeSold.AllowUserToDeleteRows = false;
			this.dgvVeSold.AllowUserToResizeRows = false;
			this.dgvVeSold.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvVeSold.BackgroundColor = System.Drawing.Color.White;
			this.dgvVeSold.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dgvVeSold.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvVeSold.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvVeSold.Location = new System.Drawing.Point(0, 74);
			this.dgvVeSold.Name = "dgvVeSold";
			this.dgvVeSold.ReadOnly = true;
			this.dgvVeSold.RowHeadersVisible = false;
			this.dgvVeSold.RowHeadersWidth = 62;
			this.dgvVeSold.RowTemplate.Height = 32;
			this.dgvVeSold.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvVeSold.Size = new System.Drawing.Size(1300, 201);
			this.dgvVeSold.TabIndex = 1;
			// 
			// pnlTableHeader
			// 
			this.pnlTableHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
			this.pnlTableHeader.Controls.Add(this.lblTableSubtitle);
			this.pnlTableHeader.Controls.Add(this.lblTableTitle);
			this.pnlTableHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTableHeader.Location = new System.Drawing.Point(0, 0);
			this.pnlTableHeader.Name = "pnlTableHeader";
			this.pnlTableHeader.Size = new System.Drawing.Size(1300, 74);
			this.pnlTableHeader.TabIndex = 0;
			// 
			// lblTableSubtitle
			// 
			this.lblTableSubtitle.AutoSize = true;
			this.lblTableSubtitle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
			this.lblTableSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
			this.lblTableSubtitle.Location = new System.Drawing.Point(28, 40);
			this.lblTableSubtitle.Name = "lblTableSubtitle";
			this.lblTableSubtitle.Size = new System.Drawing.Size(228, 25);
			this.lblTableSubtitle.TabIndex = 1;
			this.lblTableSubtitle.Text = "Danh sách vé trong kỳ đã chọn";
			// 
			// lblTableTitle
			// 
			this.lblTableTitle.AutoSize = true;
			this.lblTableTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.lblTableTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
			this.lblTableTitle.Location = new System.Drawing.Point(26, 9);
			this.lblTableTitle.Name = "lblTableTitle";
			this.lblTableTitle.Size = new System.Drawing.Size(176, 32);
			this.lblTableTitle.TabIndex = 0;
			this.lblTableTitle.Text = "Giao dịch gần đây";
			// 
			// UCTrangChu
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
			this.Controls.Add(this.pnlTableWrap);
			this.Controls.Add(this.pnlFilter);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.pnlHero);
			this.Name = "UCTrangChu";
			this.Size = new System.Drawing.Size(1349, 728);
			this.Load += new System.EventHandler(this.UCTrangChu_Load);
			this.pnlHero.ResumeLayout(false);
			this.pnlHero.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.pnlCard1.ResumeLayout(false);
			this.pnlCard1.PerformLayout();
			this.pnlCard2.ResumeLayout(false);
			this.pnlCard2.PerformLayout();
			this.pnlCard3.ResumeLayout(false);
			this.pnlCard3.PerformLayout();
			this.pnlCard4.ResumeLayout(false);
			this.pnlCard4.PerformLayout();
			this.pnlFilter.ResumeLayout(false);
			this.pnlFilter.PerformLayout();
			this.pnlTableWrap.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvVeSold)).EndInit();
			this.pnlTableHeader.ResumeLayout(false);
			this.pnlTableHeader.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlHero;
		private System.Windows.Forms.Label lblHeroBadge;
		private System.Windows.Forms.Label lblSubtitle;
		private System.Windows.Forms.Label lblWelcome;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Panel pnlCard1;
		private System.Windows.Forms.Label lblKhachHang;
		private System.Windows.Forms.Label lblKhachHangValue;
		private System.Windows.Forms.Label lblIcon1;
		private System.Windows.Forms.Panel pnlCard2;
		private System.Windows.Forms.Label lblSoChuyenXe;
		private System.Windows.Forms.Label lblSoChuyenXeValue;
		private System.Windows.Forms.Label lblIcon2;
		private System.Windows.Forms.Panel pnlCard3;
		private System.Windows.Forms.Label lblTongDoanhThu;
		private System.Windows.Forms.Label lblTongDoanhThuValue;
		private System.Windows.Forms.Label lblIcon3;
		private System.Windows.Forms.Panel pnlCard4;
		private System.Windows.Forms.Label lblSoVe;
		private System.Windows.Forms.Label lblSoVeValue;
		private System.Windows.Forms.Label lblIcon4;
		private System.Windows.Forms.Panel pnlFilter;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.DateTimePicker dtpTo;
		private System.Windows.Forms.Label lblTo;
		private System.Windows.Forms.DateTimePicker dtpFrom;
		private System.Windows.Forms.Label lblFrom;
		private System.Windows.Forms.Label lblFilterTitle;
		private System.Windows.Forms.Panel pnlTableWrap;
		private System.Windows.Forms.DataGridView dgvVeSold;
		private System.Windows.Forms.Panel pnlTableHeader;
		private System.Windows.Forms.Label lblTableSubtitle;
		private System.Windows.Forms.Label lblTableTitle;
	}
}
