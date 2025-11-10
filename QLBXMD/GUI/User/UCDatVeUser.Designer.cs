namespace QLBXMD.GUI.User
{
	partial class UCDatVeUser
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
			this.lblTitle = new System.Windows.Forms.Label();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.cboGioDi = new System.Windows.Forms.ComboBox();
			this.lblGioDi = new System.Windows.Forms.Label();
			this.btnTimChuyen = new System.Windows.Forms.Button();
			this.dtpNgayDi = new System.Windows.Forms.DateTimePicker();
			this.btnSwap = new System.Windows.Forms.Button();
			this.cboDiemDen = new System.Windows.Forms.ComboBox();
			this.cboDiemDi = new System.Windows.Forms.ComboBox();
			this.lblNgayDi = new System.Windows.Forms.Label();
			this.lblDiemDen = new System.Windows.Forms.Label();
			this.lblDiemDi = new System.Windows.Forms.Label();
			this.pnlResults = new System.Windows.Forms.Panel();
			this.flpChuyenXe = new System.Windows.Forms.FlowLayoutPanel();
			this.lblKetQua = new System.Windows.Forms.Label();
			this.pnlSeatMap = new System.Windows.Forms.Panel();
			this.gbTangDuoi = new System.Windows.Forms.GroupBox();
			this.flpTangDuoi = new System.Windows.Forms.FlowLayoutPanel();
			this.gbTangTren = new System.Windows.Forms.GroupBox();
			this.flpTangTren = new System.Windows.Forms.FlowLayoutPanel();
			this.pnlLegend = new System.Windows.Forms.Panel();
			this.lblDaChon = new System.Windows.Forms.Label();
			this.btnLegendDaChon = new System.Windows.Forms.Button();
			this.lblDaDat = new System.Windows.Forms.Label();
			this.btnLegendDaDat = new System.Windows.Forms.Button();
			this.lblTrong = new System.Windows.Forms.Label();
			this.btnLegendTrong = new System.Windows.Forms.Button();
			this.lblLoaiGhe = new System.Windows.Forms.Label();
			this.pnlSeatInfo = new System.Windows.Forms.Panel();
			this.btnDatVe = new System.Windows.Forms.Button();
			this.lblTongTien = new System.Windows.Forms.Label();
			this.lblGheDaChon = new System.Windows.Forms.Label();
			this.flowMain.SuspendLayout();
			this.pnlHeader.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			this.pnlResults.SuspendLayout();
			this.pnlSeatMap.SuspendLayout();
			this.gbTangDuoi.SuspendLayout();
			this.gbTangTren.SuspendLayout();
			this.pnlLegend.SuspendLayout();
			this.pnlSeatInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowMain
			// 
			this.flowMain.AutoScroll = true;
			this.flowMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.flowMain.Controls.Add(this.pnlHeader);
			this.flowMain.Controls.Add(this.pnlSearch);
			this.flowMain.Controls.Add(this.pnlResults);
			this.flowMain.Controls.Add(this.pnlSeatMap);
			this.flowMain.Controls.Add(this.pnlSeatInfo);
			this.flowMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowMain.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowMain.Location = new System.Drawing.Point(0, 0);
			this.flowMain.Margin = new System.Windows.Forms.Padding(0);
			this.flowMain.Name = "flowMain";
			this.flowMain.Size = new System.Drawing.Size(1650, 1538);
			this.flowMain.TabIndex = 0;
			this.flowMain.WrapContents = false;
			// 
			// pnlHeader
			// 
			this.pnlHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.pnlHeader.Controls.Add(this.lblTitle);
			this.pnlHeader.Location = new System.Drawing.Point(4, 5);
			this.pnlHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 20);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(1686, 100);
			this.pnlHeader.TabIndex = 0;
			// 
			// lblTitle
			// 
			this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(0, 0);
			this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(1550, 100);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "ĐẶT VÉ XE TRỰC TUYẾN";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.Color.White;
			this.pnlSearch.Controls.Add(this.cboGioDi);
			this.pnlSearch.Controls.Add(this.lblGioDi);
			this.pnlSearch.Controls.Add(this.btnTimChuyen);
			this.pnlSearch.Controls.Add(this.dtpNgayDi);
			this.pnlSearch.Controls.Add(this.btnSwap);
			this.pnlSearch.Controls.Add(this.cboDiemDen);
			this.pnlSearch.Controls.Add(this.cboDiemDi);
			this.pnlSearch.Controls.Add(this.lblNgayDi);
			this.pnlSearch.Controls.Add(this.lblDiemDen);
			this.pnlSearch.Controls.Add(this.lblDiemDi);
			this.pnlSearch.Location = new System.Drawing.Point(4, 130);
			this.pnlSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 20);
			this.pnlSearch.MinimumSize = new System.Drawing.Size(1080, 240);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Padding = new System.Windows.Forms.Padding(30, 25, 30, 25);
			this.pnlSearch.Size = new System.Drawing.Size(1686, 255);
			this.pnlSearch.TabIndex = 1;
			// 
			// cboGioDi
			// 
			this.cboGioDi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGioDi.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.cboGioDi.FormattingEnabled = true;
			this.cboGioDi.Items.AddRange(new object[] {
            "Tất cả",
            "00:00 - 06:00",
            "06:00 - 12:00",
            "12:00 - 18:00",
            "18:00 - 24:00"});
			this.cboGioDi.Location = new System.Drawing.Point(1258, 105);
			this.cboGioDi.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.cboGioDi.Name = "cboGioDi";
			this.cboGioDi.Size = new System.Drawing.Size(298, 38);
			this.cboGioDi.TabIndex = 11;
			// 
			// lblGioDi
			// 
			this.lblGioDi.AutoSize = true;
			this.lblGioDi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lblGioDi.Location = new System.Drawing.Point(1258, 67);
			this.lblGioDi.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblGioDi.Name = "lblGioDi";
			this.lblGioDi.Size = new System.Drawing.Size(70, 28);
			this.lblGioDi.TabIndex = 10;
			this.lblGioDi.Text = "Giờ đi";
			// 
			// btnTimChuyen
			// 
			this.btnTimChuyen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
			this.btnTimChuyen.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnTimChuyen.FlatAppearance.BorderSize = 0;
			this.btnTimChuyen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnTimChuyen.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnTimChuyen.ForeColor = System.Drawing.Color.White;
			this.btnTimChuyen.Location = new System.Drawing.Point(1258, 169);
			this.btnTimChuyen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnTimChuyen.Name = "btnTimChuyen";
			this.btnTimChuyen.Size = new System.Drawing.Size(300, 77);
			this.btnTimChuyen.TabIndex = 9;
			this.btnTimChuyen.Text = "TÌM CHUYẾN XE";
			this.btnTimChuyen.UseVisualStyleBackColor = false;
			this.btnTimChuyen.Click += new System.EventHandler(this.btnTimChuyen_Click);
			// 
			// dtpNgayDi
			// 
			this.dtpNgayDi.CustomFormat = "dd/MM/yyyy";
			this.dtpNgayDi.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.dtpNgayDi.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpNgayDi.Location = new System.Drawing.Point(905, 103);
			this.dtpNgayDi.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.dtpNgayDi.Name = "dtpNgayDi";
			this.dtpNgayDi.Size = new System.Drawing.Size(268, 37);
			this.dtpNgayDi.TabIndex = 7;
			// 
			// btnSwap
			// 
			this.btnSwap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.btnSwap.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnSwap.FlatAppearance.BorderSize = 0;
			this.btnSwap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSwap.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnSwap.Location = new System.Drawing.Point(412, 95);
			this.btnSwap.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnSwap.Name = "btnSwap";
			this.btnSwap.Size = new System.Drawing.Size(60, 54);
			this.btnSwap.TabIndex = 6;
			this.btnSwap.Text = "⇄";
			this.btnSwap.UseVisualStyleBackColor = false;
			this.btnSwap.Click += new System.EventHandler(this.btnSwap_Click);
			// 
			// cboDiemDen
			// 
			this.cboDiemDen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDiemDen.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.cboDiemDen.FormattingEnabled = true;
			this.cboDiemDen.Location = new System.Drawing.Point(536, 102);
			this.cboDiemDen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.cboDiemDen.Name = "cboDiemDen";
			this.cboDiemDen.Size = new System.Drawing.Size(298, 38);
			this.cboDiemDen.TabIndex = 5;
			// 
			// cboDiemDi
			// 
			this.cboDiemDi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDiemDi.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.cboDiemDi.FormattingEnabled = true;
			this.cboDiemDi.Location = new System.Drawing.Point(45, 102);
			this.cboDiemDi.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.cboDiemDi.Name = "cboDiemDi";
			this.cboDiemDi.Size = new System.Drawing.Size(298, 38);
			this.cboDiemDi.TabIndex = 4;
			// 
			// lblNgayDi
			// 
			this.lblNgayDi.AutoSize = true;
			this.lblNgayDi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lblNgayDi.Location = new System.Drawing.Point(905, 68);
			this.lblNgayDi.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblNgayDi.Name = "lblNgayDi";
			this.lblNgayDi.Size = new System.Drawing.Size(87, 28);
			this.lblNgayDi.TabIndex = 2;
			this.lblNgayDi.Text = "Ngày đi";
			// 
			// lblDiemDen
			// 
			this.lblDiemDen.AutoSize = true;
			this.lblDiemDen.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lblDiemDen.Location = new System.Drawing.Point(536, 67);
			this.lblDiemDen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblDiemDen.Name = "lblDiemDen";
			this.lblDiemDen.Size = new System.Drawing.Size(104, 28);
			this.lblDiemDen.TabIndex = 1;
			this.lblDiemDen.Text = "Điểm đến";
			// 
			// lblDiemDi
			// 
			this.lblDiemDi.AutoSize = true;
			this.lblDiemDi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lblDiemDi.Location = new System.Drawing.Point(45, 67);
			this.lblDiemDi.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblDiemDi.Name = "lblDiemDi";
			this.lblDiemDi.Size = new System.Drawing.Size(87, 28);
			this.lblDiemDi.TabIndex = 0;
			this.lblDiemDi.Text = "Điểm đi";
			// 
			// pnlResults
			// 
			this.pnlResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlResults.BackColor = System.Drawing.Color.White;
			this.pnlResults.Controls.Add(this.flpChuyenXe);
			this.pnlResults.Controls.Add(this.lblKetQua);
			this.pnlResults.Location = new System.Drawing.Point(4, 410);
			this.pnlResults.Margin = new System.Windows.Forms.Padding(4, 5, 4, 20);
			this.pnlResults.MinimumSize = new System.Drawing.Size(1080, 350);
			this.pnlResults.Name = "pnlResults";
			this.pnlResults.Padding = new System.Windows.Forms.Padding(30, 25, 30, 25);
			this.pnlResults.Size = new System.Drawing.Size(1686, 456);
			this.pnlResults.TabIndex = 2;
			this.pnlResults.Visible = false;
			// 
			// flpChuyenXe
			// 
			this.flpChuyenXe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.flpChuyenXe.AutoScroll = true;
			this.flpChuyenXe.Location = new System.Drawing.Point(38, 88);
			this.flpChuyenXe.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.flpChuyenXe.Name = "flpChuyenXe";
			this.flpChuyenXe.Size = new System.Drawing.Size(1560, 356);
			this.flpChuyenXe.TabIndex = 1;
			// 
			// lblKetQua
			// 
			this.lblKetQua.AutoSize = true;
			this.lblKetQua.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lblKetQua.Location = new System.Drawing.Point(30, 25);
			this.lblKetQua.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblKetQua.Name = "lblKetQua";
			this.lblKetQua.Size = new System.Drawing.Size(285, 38);
			this.lblKetQua.TabIndex = 0;
			this.lblKetQua.Text = "Chuyến xe khả dụng";
			// 
			// pnlSeatMap
			// 
			this.pnlSeatMap.AutoSize = true;
			this.pnlSeatMap.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlSeatMap.BackColor = System.Drawing.Color.White;
			this.pnlSeatMap.Controls.Add(this.gbTangDuoi);
			this.pnlSeatMap.Controls.Add(this.gbTangTren);
			this.pnlSeatMap.Controls.Add(this.pnlLegend);
			this.pnlSeatMap.Controls.Add(this.lblLoaiGhe);
			this.pnlSeatMap.Location = new System.Drawing.Point(4, 891);
			this.pnlSeatMap.Margin = new System.Windows.Forms.Padding(4, 5, 4, 20);
			this.pnlSeatMap.MinimumSize = new System.Drawing.Size(1080, 500);
			this.pnlSeatMap.Name = "pnlSeatMap";
			this.pnlSeatMap.Padding = new System.Windows.Forms.Padding(30, 25, 30, 25);
			this.pnlSeatMap.Size = new System.Drawing.Size(1080, 500);
			this.pnlSeatMap.TabIndex = 3;
			this.pnlSeatMap.Visible = false;
			// 
			// gbTangDuoi
			// 
			this.gbTangDuoi.AutoSize = true;
			this.gbTangDuoi.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.gbTangDuoi.Controls.Add(this.flpTangDuoi);
			this.gbTangDuoi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbTangDuoi.Location = new System.Drawing.Point(38, 151);
			this.gbTangDuoi.MaximumSize = new System.Drawing.Size(600, 0);
			this.gbTangDuoi.Name = "gbTangDuoi";
			this.gbTangDuoi.Padding = new System.Windows.Forms.Padding(10);
			this.gbTangDuoi.Size = new System.Drawing.Size(40, 67);
			this.gbTangDuoi.TabIndex = 4;
			this.gbTangDuoi.TabStop = false;
			this.gbTangDuoi.Text = "Tầng dưới";
			this.gbTangDuoi.Visible = false;
			// 
			// flpTangDuoi
			// 
			this.flpTangDuoi.AutoSize = true;
			this.flpTangDuoi.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flpTangDuoi.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpTangDuoi.Location = new System.Drawing.Point(10, 37);
			this.flpTangDuoi.MaximumSize = new System.Drawing.Size(580, 0);
			this.flpTangDuoi.Name = "flpTangDuoi";
			this.flpTangDuoi.Padding = new System.Windows.Forms.Padding(10);
			this.flpTangDuoi.Size = new System.Drawing.Size(20, 20);
			this.flpTangDuoi.TabIndex = 0;
			// 
			// gbTangTren
			// 
			this.gbTangTren.AutoSize = true;
			this.gbTangTren.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.gbTangTren.Controls.Add(this.flpTangTren);
			this.gbTangTren.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbTangTren.Location = new System.Drawing.Point(641, 160);
			this.gbTangTren.MaximumSize = new System.Drawing.Size(600, 0);
			this.gbTangTren.Name = "gbTangTren";
			this.gbTangTren.Padding = new System.Windows.Forms.Padding(10);
			this.gbTangTren.Size = new System.Drawing.Size(40, 67);
			this.gbTangTren.TabIndex = 5;
			this.gbTangTren.TabStop = false;
			this.gbTangTren.Text = "Tầng trên";
			this.gbTangTren.Visible = false;
			// 
			// flpTangTren
			// 
			this.flpTangTren.AutoSize = true;
			this.flpTangTren.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flpTangTren.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flpTangTren.Location = new System.Drawing.Point(10, 37);
			this.flpTangTren.MaximumSize = new System.Drawing.Size(580, 0);
			this.flpTangTren.Name = "flpTangTren";
			this.flpTangTren.Padding = new System.Windows.Forms.Padding(10);
			this.flpTangTren.Size = new System.Drawing.Size(20, 20);
			this.flpTangTren.TabIndex = 0;
			// 
			// pnlLegend
			// 
			this.pnlLegend.Controls.Add(this.lblDaChon);
			this.pnlLegend.Controls.Add(this.btnLegendDaChon);
			this.pnlLegend.Controls.Add(this.lblDaDat);
			this.pnlLegend.Controls.Add(this.btnLegendDaDat);
			this.pnlLegend.Controls.Add(this.lblTrong);
			this.pnlLegend.Controls.Add(this.btnLegendTrong);
			this.pnlLegend.Location = new System.Drawing.Point(30, 70);
			this.pnlLegend.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.pnlLegend.Name = "pnlLegend";
			this.pnlLegend.Size = new System.Drawing.Size(600, 50);
			this.pnlLegend.TabIndex = 1;
			// 
			// lblDaChon
			// 
			this.lblDaChon.AutoSize = true;
			this.lblDaChon.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.lblDaChon.Location = new System.Drawing.Point(460, 15);
			this.lblDaChon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblDaChon.Name = "lblDaChon";
			this.lblDaChon.Size = new System.Drawing.Size(78, 25);
			this.lblDaChon.TabIndex = 5;
			this.lblDaChon.Text = "Đã chọn";
			// 
			// btnLegendDaChon
			// 
			this.btnLegendDaChon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
			this.btnLegendDaChon.Enabled = false;
			this.btnLegendDaChon.FlatAppearance.BorderSize = 0;
			this.btnLegendDaChon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLegendDaChon.Location = new System.Drawing.Point(410, 10);
			this.btnLegendDaChon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnLegendDaChon.Name = "btnLegendDaChon";
			this.btnLegendDaChon.Size = new System.Drawing.Size(40, 35);
			this.btnLegendDaChon.TabIndex = 4;
			this.btnLegendDaChon.UseVisualStyleBackColor = false;
			// 
			// lblDaDat
			// 
			this.lblDaDat.AutoSize = true;
			this.lblDaDat.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.lblDaDat.Location = new System.Drawing.Point(255, 15);
			this.lblDaDat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblDaDat.Name = "lblDaDat";
			this.lblDaDat.Size = new System.Drawing.Size(65, 25);
			this.lblDaDat.TabIndex = 3;
			this.lblDaDat.Text = "Đã đặt";
			// 
			// btnLegendDaDat
			// 
			this.btnLegendDaDat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
			this.btnLegendDaDat.Enabled = false;
			this.btnLegendDaDat.FlatAppearance.BorderSize = 0;
			this.btnLegendDaDat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLegendDaDat.Location = new System.Drawing.Point(205, 10);
			this.btnLegendDaDat.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnLegendDaDat.Name = "btnLegendDaDat";
			this.btnLegendDaDat.Size = new System.Drawing.Size(40, 35);
			this.btnLegendDaDat.TabIndex = 2;
			this.btnLegendDaDat.UseVisualStyleBackColor = false;
			// 
			// lblTrong
			// 
			this.lblTrong.AutoSize = true;
			this.lblTrong.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.lblTrong.Location = new System.Drawing.Point(55, 15);
			this.lblTrong.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTrong.Name = "lblTrong";
			this.lblTrong.Size = new System.Drawing.Size(57, 25);
			this.lblTrong.TabIndex = 1;
			this.lblTrong.Text = "Trống";
			// 
			// btnLegendTrong
			// 
			this.btnLegendTrong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(199)))), ((int)(((byte)(174)))));
			this.btnLegendTrong.Enabled = false;
			this.btnLegendTrong.FlatAppearance.BorderSize = 0;
			this.btnLegendTrong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLegendTrong.Location = new System.Drawing.Point(5, 10);
			this.btnLegendTrong.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnLegendTrong.Name = "btnLegendTrong";
			this.btnLegendTrong.Size = new System.Drawing.Size(40, 35);
			this.btnLegendTrong.TabIndex = 0;
			this.btnLegendTrong.UseVisualStyleBackColor = false;
			// 
			// lblLoaiGhe
			// 
			this.lblLoaiGhe.AutoSize = true;
			this.lblLoaiGhe.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lblLoaiGhe.Location = new System.Drawing.Point(30, 25);
			this.lblLoaiGhe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblLoaiGhe.Name = "lblLoaiGhe";
			this.lblLoaiGhe.Size = new System.Drawing.Size(142, 38);
			this.lblLoaiGhe.TabIndex = 0;
			this.lblLoaiGhe.Text = "Chọn ghế";
			// 
			// pnlSeatInfo
			// 
			this.pnlSeatInfo.AutoSize = true;
			this.pnlSeatInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlSeatInfo.BackColor = System.Drawing.Color.White;
			this.pnlSeatInfo.Controls.Add(this.btnDatVe);
			this.pnlSeatInfo.Controls.Add(this.lblTongTien);
			this.pnlSeatInfo.Controls.Add(this.lblGheDaChon);
			this.pnlSeatInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSeatInfo.Location = new System.Drawing.Point(4, 1416);
			this.pnlSeatInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 20);
			this.pnlSeatInfo.MinimumSize = new System.Drawing.Size(1080, 150);
			this.pnlSeatInfo.Name = "pnlSeatInfo";
			this.pnlSeatInfo.Padding = new System.Windows.Forms.Padding(30, 25, 30, 25);
			this.pnlSeatInfo.Size = new System.Drawing.Size(1686, 198);
			this.pnlSeatInfo.TabIndex = 4;
			this.pnlSeatInfo.Visible = false;
			// 
			// btnDatVe
			// 
			this.btnDatVe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
			this.btnDatVe.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnDatVe.Enabled = false;
			this.btnDatVe.FlatAppearance.BorderSize = 0;
			this.btnDatVe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDatVe.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.btnDatVe.ForeColor = System.Drawing.Color.White;
			this.btnDatVe.Location = new System.Drawing.Point(30, 108);
			this.btnDatVe.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnDatVe.Name = "btnDatVe";
			this.btnDatVe.Size = new System.Drawing.Size(180, 60);
			this.btnDatVe.TabIndex = 2;
			this.btnDatVe.Text = "ĐẶT VÉ";
			this.btnDatVe.UseVisualStyleBackColor = false;
			this.btnDatVe.Click += new System.EventHandler(this.btnDatVe_Click);
			// 
			// lblTongTien
			// 
			this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.lblTongTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
			this.lblTongTien.Location = new System.Drawing.Point(30, 60);
			this.lblTongTien.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTongTien.Name = "lblTongTien";
			this.lblTongTien.Size = new System.Drawing.Size(600, 46);
			this.lblTongTien.TabIndex = 1;
			this.lblTongTien.Text = "Tổng tiền: 0 VNĐ";
			// 
			// lblGheDaChon
			// 
			this.lblGheDaChon.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.lblGheDaChon.Location = new System.Drawing.Point(30, 25);
			this.lblGheDaChon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblGheDaChon.Name = "lblGheDaChon";
			this.lblGheDaChon.Size = new System.Drawing.Size(1200, 38);
			this.lblGheDaChon.TabIndex = 0;
			this.lblGheDaChon.Text = "Ghế đã chọn: Chưa chọn";
			// 
			// UCDatVeUser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.flowMain);
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "UCDatVeUser";
			this.Size = new System.Drawing.Size(1650, 1538);
			this.flowMain.ResumeLayout(false);
			this.flowMain.PerformLayout();
			this.pnlHeader.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearch.PerformLayout();
			this.pnlResults.ResumeLayout(false);
			this.pnlResults.PerformLayout();
			this.pnlSeatMap.ResumeLayout(false);
			this.pnlSeatMap.PerformLayout();
			this.gbTangDuoi.ResumeLayout(false);
			this.gbTangDuoi.PerformLayout();
			this.gbTangTren.ResumeLayout(false);
			this.gbTangTren.PerformLayout();
			this.pnlLegend.ResumeLayout(false);
			this.pnlLegend.PerformLayout();
			this.pnlSeatInfo.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowMain;
		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Panel pnlSearch;
		private System.Windows.Forms.ComboBox cboGioDi;
		private System.Windows.Forms.Label lblGioDi;
		private System.Windows.Forms.Button btnTimChuyen;
		private System.Windows.Forms.DateTimePicker dtpNgayDi;
		private System.Windows.Forms.Button btnSwap;
		private System.Windows.Forms.ComboBox cboDiemDen;
		private System.Windows.Forms.ComboBox cboDiemDi;
		private System.Windows.Forms.Label lblNgayDi;
		private System.Windows.Forms.Label lblDiemDen;
		private System.Windows.Forms.Label lblDiemDi;
		private System.Windows.Forms.Panel pnlResults;
		private System.Windows.Forms.FlowLayoutPanel flpChuyenXe;
		private System.Windows.Forms.Label lblKetQua;
		private System.Windows.Forms.Panel pnlSeatMap;
		private System.Windows.Forms.Label lblLoaiGhe;
		private System.Windows.Forms.GroupBox gbTangTren;
		private System.Windows.Forms.FlowLayoutPanel flpTangTren;
		private System.Windows.Forms.GroupBox gbTangDuoi;
		private System.Windows.Forms.FlowLayoutPanel flpTangDuoi;
		private System.Windows.Forms.Panel pnlLegend;
		private System.Windows.Forms.Label lblDaChon;
		private System.Windows.Forms.Button btnLegendDaChon;
		private System.Windows.Forms.Label lblDaDat;
		private System.Windows.Forms.Button btnLegendDaDat;
		private System.Windows.Forms.Label lblTrong;
		private System.Windows.Forms.Button btnLegendTrong;
		private System.Windows.Forms.Panel pnlSeatInfo;
		private System.Windows.Forms.Button btnDatVe;
		private System.Windows.Forms.Label lblTongTien;
		private System.Windows.Forms.Label lblGheDaChon;
	}
}
