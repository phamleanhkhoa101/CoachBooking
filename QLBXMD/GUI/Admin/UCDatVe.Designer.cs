namespace QLBXMD.GUI.Admin
{
	partial class UCDatVe
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
			this.lblTitle = new System.Windows.Forms.Label();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.cboGioDi = new System.Windows.Forms.ComboBox();
			this.labelGioDi = new System.Windows.Forms.Label();
			this.btnTimChuyen = new System.Windows.Forms.Button();
			this.dtpNgayDi = new System.Windows.Forms.DateTimePicker();
			this.cboDiemDen = new System.Windows.Forms.ComboBox();
			this.cboDiemDi = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pnlResults = new System.Windows.Forms.Panel();
			this.dgvChuyenXe = new System.Windows.Forms.DataGridView();
			this.label4 = new System.Windows.Forms.Label();
			this.pnlSeatMap = new System.Windows.Forms.Panel();
			this.gbTangTren = new System.Windows.Forms.GroupBox();
			this.flpTangTren = new System.Windows.Forms.FlowLayoutPanel();
			this.gbTangDuoi = new System.Windows.Forms.GroupBox();
			this.flpTangDuoi = new System.Windows.Forms.FlowLayoutPanel();
			this.pnlLegend = new System.Windows.Forms.Panel();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.btnLegendDaDat = new System.Windows.Forms.Button();
			this.btnLegendDangChon = new System.Windows.Forms.Button();
			this.btnLegendTrong = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.pnlThongTinKH = new System.Windows.Forms.Panel();
			this.btnTimKH = new System.Windows.Forms.Button();
			this.btnDatVe = new System.Windows.Forms.Button();
			this.lblTongTien = new System.Windows.Forms.Label();
			this.lblThongTinChuyen = new System.Windows.Forms.Label();
			this.cboGioiTinh = new System.Windows.Forms.ComboBox();
			this.dtpNamSinh = new System.Windows.Forms.DateTimePicker();
			this.txtSDT = new System.Windows.Forms.TextBox();
			this.txtTenKH = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.flowMain.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			this.pnlResults.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvChuyenXe)).BeginInit();
			this.pnlSeatMap.SuspendLayout();
			this.gbTangTren.SuspendLayout();
			this.gbTangDuoi.SuspendLayout();
			this.pnlLegend.SuspendLayout();
			this.pnlThongTinKH.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowMain
			// 
			this.flowMain.AutoScroll = true;
			this.flowMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
			this.flowMain.Controls.Add(this.lblTitle);
			this.flowMain.Controls.Add(this.pnlSearch);
			this.flowMain.Controls.Add(this.pnlResults);
			this.flowMain.Controls.Add(this.pnlSeatMap);
			this.flowMain.Controls.Add(this.pnlThongTinKH);
			this.flowMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowMain.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowMain.Location = new System.Drawing.Point(0, 0);
			this.flowMain.Name = "flowMain";
			this.flowMain.Padding = new System.Windows.Forms.Padding(20);
			this.flowMain.Size = new System.Drawing.Size(1351, 745);
			this.flowMain.TabIndex = 0;
			this.flowMain.WrapContents = false;
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.lblTitle.Location = new System.Drawing.Point(23, 20);
			this.lblTitle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 20);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(221, 54);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "ĐẶT VÉ XE";
			// 
			// pnlSearch
			// 
			this.pnlSearch.AutoSize = true;
			this.pnlSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlSearch.BackColor = System.Drawing.Color.White;
			this.pnlSearch.Controls.Add(this.cboGioDi);
			this.pnlSearch.Controls.Add(this.labelGioDi);
			this.pnlSearch.Controls.Add(this.btnTimChuyen);
			this.pnlSearch.Controls.Add(this.dtpNgayDi);
			this.pnlSearch.Controls.Add(this.cboDiemDen);
			this.pnlSearch.Controls.Add(this.cboDiemDi);
			this.pnlSearch.Controls.Add(this.label3);
			this.pnlSearch.Controls.Add(this.label2);
			this.pnlSearch.Controls.Add(this.label1);
			this.pnlSearch.Location = new System.Drawing.Point(23, 97);
			this.pnlSearch.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
			this.pnlSearch.MaximumSize = new System.Drawing.Size(1280, 0);
			this.pnlSearch.MinimumSize = new System.Drawing.Size(1280, 180);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Padding = new System.Windows.Forms.Padding(20);
			this.pnlSearch.Size = new System.Drawing.Size(1280, 180);
			this.pnlSearch.TabIndex = 1;
			// 
			// cboGioDi
			// 
			this.cboGioDi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGioDi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboGioDi.FormattingEnabled = true;
			this.cboGioDi.Items.AddRange(new object[] {
            "Tất cả",
            "00:00 - 06:00",
            "06:00 - 12:00",
            "12:00 - 18:00",
            "18:00 - 24:00"});
			this.cboGioDi.Location = new System.Drawing.Point(23, 124);
			this.cboGioDi.Name = "cboGioDi";
			this.cboGioDi.Size = new System.Drawing.Size(250, 36);
			this.cboGioDi.TabIndex = 8;
			// 
			// labelGioDi
			// 
			this.labelGioDi.AutoSize = true;
			this.labelGioDi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelGioDi.Location = new System.Drawing.Point(18, 85);
			this.labelGioDi.Name = "labelGioDi";
			this.labelGioDi.Size = new System.Drawing.Size(70, 28);
			this.labelGioDi.TabIndex = 7;
			this.labelGioDi.Text = "Giờ đi";
			// 
			// btnTimChuyen
			// 
			this.btnTimChuyen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.btnTimChuyen.FlatAppearance.BorderSize = 0;
			this.btnTimChuyen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnTimChuyen.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnTimChuyen.ForeColor = System.Drawing.Color.White;
			this.btnTimChuyen.Location = new System.Drawing.Point(380, 117);
			this.btnTimChuyen.Name = "btnTimChuyen";
			this.btnTimChuyen.Size = new System.Drawing.Size(180, 45);
			this.btnTimChuyen.TabIndex = 6;
			this.btnTimChuyen.Text = "Tìm chuyến";
			this.btnTimChuyen.UseVisualStyleBackColor = false;
			this.btnTimChuyen.Click += new System.EventHandler(this.btnTimChuyen_Click);
			// 
			// dtpNgayDi
			// 
			this.dtpNgayDi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dtpNgayDi.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpNgayDi.Location = new System.Drawing.Point(713, 36);
			this.dtpNgayDi.Name = "dtpNgayDi";
			this.dtpNgayDi.Size = new System.Drawing.Size(250, 34);
			this.dtpNgayDi.TabIndex = 5;
			// 
			// cboDiemDen
			// 
			this.cboDiemDen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDiemDen.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboDiemDen.FormattingEnabled = true;
			this.cboDiemDen.Location = new System.Drawing.Point(380, 36);
			this.cboDiemDen.Name = "cboDiemDen";
			this.cboDiemDen.Size = new System.Drawing.Size(250, 36);
			this.cboDiemDen.TabIndex = 4;
			// 
			// cboDiemDi
			// 
			this.cboDiemDi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDiemDi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboDiemDi.FormattingEnabled = true;
			this.cboDiemDi.Location = new System.Drawing.Point(23, 36);
			this.cboDiemDi.Name = "cboDiemDi";
			this.cboDiemDi.Size = new System.Drawing.Size(250, 36);
			this.cboDiemDi.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(708, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(87, 28);
			this.label3.TabIndex = 2;
			this.label3.Text = "Ngày đi";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(375, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 28);
			this.label2.TabIndex = 1;
			this.label2.Text = "Điểm đến";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(18, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(87, 28);
			this.label1.TabIndex = 0;
			this.label1.Text = "Điểm đi";
			// 
			// pnlResults
			// 
			this.pnlResults.AutoSize = true;
			this.pnlResults.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlResults.BackColor = System.Drawing.Color.White;
			this.pnlResults.Controls.Add(this.dgvChuyenXe);
			this.pnlResults.Controls.Add(this.label4);
			this.pnlResults.Location = new System.Drawing.Point(23, 300);
			this.pnlResults.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
			this.pnlResults.MaximumSize = new System.Drawing.Size(1280, 0);
			this.pnlResults.MinimumSize = new System.Drawing.Size(1280, 350);
			this.pnlResults.Name = "pnlResults";
			this.pnlResults.Padding = new System.Windows.Forms.Padding(20);
			this.pnlResults.Size = new System.Drawing.Size(1280, 350);
			this.pnlResults.TabIndex = 2;
			this.pnlResults.Visible = false;
			// 
			// dgvChuyenXe
			// 
			this.dgvChuyenXe.AllowUserToAddRows = false;
			this.dgvChuyenXe.AllowUserToDeleteRows = false;
			this.dgvChuyenXe.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvChuyenXe.BackgroundColor = System.Drawing.Color.White;
			this.dgvChuyenXe.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dgvChuyenXe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvChuyenXe.Location = new System.Drawing.Point(23, 70);
			this.dgvChuyenXe.MultiSelect = false;
			this.dgvChuyenXe.Name = "dgvChuyenXe";
			this.dgvChuyenXe.ReadOnly = true;
			this.dgvChuyenXe.RowHeadersWidth = 62;
			this.dgvChuyenXe.RowTemplate.Height = 28;
			this.dgvChuyenXe.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvChuyenXe.Size = new System.Drawing.Size(1237, 260);
			this.dgvChuyenXe.TabIndex = 1;
			this.dgvChuyenXe.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChuyenXe_CellContentClick);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(23, 20);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(292, 38);
			this.label4.TabIndex = 0;
			this.label4.Text = "Danh sách chuyến xe";
			// 
			// pnlSeatMap
			// 
			this.pnlSeatMap.AutoSize = true;
			this.pnlSeatMap.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlSeatMap.BackColor = System.Drawing.Color.White;
			this.pnlSeatMap.Controls.Add(this.gbTangTren);
			this.pnlSeatMap.Controls.Add(this.gbTangDuoi);
			this.pnlSeatMap.Controls.Add(this.pnlLegend);
			this.pnlSeatMap.Controls.Add(this.label5);
			this.pnlSeatMap.Location = new System.Drawing.Point(23, 673);
			this.pnlSeatMap.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
			this.pnlSeatMap.MaximumSize = new System.Drawing.Size(1280, 0);
			this.pnlSeatMap.Name = "pnlSeatMap";
			this.pnlSeatMap.Padding = new System.Windows.Forms.Padding(20);
			this.pnlSeatMap.Size = new System.Drawing.Size(1280, 0);
			this.pnlSeatMap.TabIndex = 3;
			this.pnlSeatMap.Visible = false;
			// 
			// gbTangTren
			// 
			this.gbTangTren.AutoSize = true;
			this.gbTangTren.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.gbTangTren.Controls.Add(this.flpTangTren);
			this.gbTangTren.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbTangTren.Location = new System.Drawing.Point(640, 140);
			this.gbTangTren.MaximumSize = new System.Drawing.Size(600, 0);
			this.gbTangTren.Name = "gbTangTren";
			this.gbTangTren.Padding = new System.Windows.Forms.Padding(10);
			this.gbTangTren.Size = new System.Drawing.Size(600, 0);
			this.gbTangTren.TabIndex = 3;
			this.gbTangTren.TabStop = false;
			this.gbTangTren.Text = "Tầng trên";
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
			this.flpTangTren.Size = new System.Drawing.Size(580, 0);
			this.flpTangTren.TabIndex = 0;
			// 
			// gbTangDuoi
			// 
			this.gbTangDuoi.AutoSize = true;
			this.gbTangDuoi.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.gbTangDuoi.Controls.Add(this.flpTangDuoi);
			this.gbTangDuoi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbTangDuoi.Location = new System.Drawing.Point(23, 140);
			this.gbTangDuoi.MaximumSize = new System.Drawing.Size(600, 0);
			this.gbTangDuoi.Name = "gbTangDuoi";
			this.gbTangDuoi.Padding = new System.Windows.Forms.Padding(10);
			this.gbTangDuoi.Size = new System.Drawing.Size(600, 0);
			this.gbTangDuoi.TabIndex = 2;
			this.gbTangDuoi.TabStop = false;
			this.gbTangDuoi.Text = "Tầng dưới";
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
			this.flpTangDuoi.Size = new System.Drawing.Size(580, 0);
			this.flpTangDuoi.TabIndex = 0;
			// 
			// pnlLegend
			// 
			this.pnlLegend.Controls.Add(this.label13);
			this.pnlLegend.Controls.Add(this.label12);
			this.pnlLegend.Controls.Add(this.label11);
			this.pnlLegend.Controls.Add(this.btnLegendDaDat);
			this.pnlLegend.Controls.Add(this.btnLegendDangChon);
			this.pnlLegend.Controls.Add(this.btnLegendTrong);
			this.pnlLegend.Location = new System.Drawing.Point(23, 70);
			this.pnlLegend.Name = "pnlLegend";
			this.pnlLegend.Size = new System.Drawing.Size(600, 50);
			this.pnlLegend.TabIndex = 1;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label13.Location = new System.Drawing.Point(460, 15);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(65, 25);
			this.label13.TabIndex = 5;
			this.label13.Text = "Đã đặt";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(255, 15);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(99, 25);
			this.label12.TabIndex = 4;
			this.label12.Text = "Đang chọn";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(55, 15);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(57, 25);
			this.label11.TabIndex = 3;
			this.label11.Text = "Trống";
			// 
			// btnLegendDaDat
			// 
			this.btnLegendDaDat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(83)))), ((int)(((byte)(79)))));
			this.btnLegendDaDat.Enabled = false;
			this.btnLegendDaDat.FlatAppearance.BorderSize = 0;
			this.btnLegendDaDat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLegendDaDat.Location = new System.Drawing.Point(410, 10);
			this.btnLegendDaDat.Name = "btnLegendDaDat";
			this.btnLegendDaDat.Size = new System.Drawing.Size(40, 35);
			this.btnLegendDaDat.TabIndex = 2;
			this.btnLegendDaDat.UseVisualStyleBackColor = false;
			// 
			// btnLegendDangChon
			// 
			this.btnLegendDangChon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(128)))), ((int)(((byte)(37)))));
			this.btnLegendDangChon.Enabled = false;
			this.btnLegendDangChon.FlatAppearance.BorderSize = 0;
			this.btnLegendDangChon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLegendDangChon.Location = new System.Drawing.Point(205, 10);
			this.btnLegendDangChon.Name = "btnLegendDangChon";
			this.btnLegendDangChon.Size = new System.Drawing.Size(40, 35);
			this.btnLegendDangChon.TabIndex = 1;
			this.btnLegendDangChon.UseVisualStyleBackColor = false;
			// 
			// btnLegendTrong
			// 
			this.btnLegendTrong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(199)))), ((int)(((byte)(174)))));
			this.btnLegendTrong.Enabled = false;
			this.btnLegendTrong.FlatAppearance.BorderSize = 0;
			this.btnLegendTrong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLegendTrong.Location = new System.Drawing.Point(5, 10);
			this.btnLegendTrong.Name = "btnLegendTrong";
			this.btnLegendTrong.Size = new System.Drawing.Size(40, 35);
			this.btnLegendTrong.TabIndex = 6;
			this.btnLegendTrong.UseVisualStyleBackColor = false;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(23, 20);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(209, 38);
			this.label5.TabIndex = 0;
			this.label5.Text = "Chọn ghế ngồi";
			// 
			// pnlThongTinKH
			// 
			this.pnlThongTinKH.AutoSize = true;
			this.pnlThongTinKH.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.pnlThongTinKH.BackColor = System.Drawing.Color.White;
			this.pnlThongTinKH.Controls.Add(this.btnTimKH);
			this.pnlThongTinKH.Controls.Add(this.btnDatVe);
			this.pnlThongTinKH.Controls.Add(this.lblTongTien);
			this.pnlThongTinKH.Controls.Add(this.lblThongTinChuyen);
			this.pnlThongTinKH.Controls.Add(this.cboGioiTinh);
			this.pnlThongTinKH.Controls.Add(this.dtpNamSinh);
			this.pnlThongTinKH.Controls.Add(this.txtSDT);
			this.pnlThongTinKH.Controls.Add(this.txtTenKH);
			this.pnlThongTinKH.Controls.Add(this.label10);
			this.pnlThongTinKH.Controls.Add(this.label9);
			this.pnlThongTinKH.Controls.Add(this.label8);
			this.pnlThongTinKH.Controls.Add(this.label7);
			this.pnlThongTinKH.Controls.Add(this.label6);
			this.pnlThongTinKH.Location = new System.Drawing.Point(23, 1196);
			this.pnlThongTinKH.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
			this.pnlThongTinKH.MaximumSize = new System.Drawing.Size(1280, 0);
			this.pnlThongTinKH.MinimumSize = new System.Drawing.Size(1280, 450);
			this.pnlThongTinKH.Name = "pnlThongTinKH";
			this.pnlThongTinKH.Padding = new System.Windows.Forms.Padding(20);
			this.pnlThongTinKH.Size = new System.Drawing.Size(1280, 450);
			this.pnlThongTinKH.TabIndex = 4;
			this.pnlThongTinKH.Visible = false;
			// 
			// btnTimKH
			// 
			this.btnTimKH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
			this.btnTimKH.FlatAppearance.BorderSize = 0;
			this.btnTimKH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnTimKH.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnTimKH.ForeColor = System.Drawing.Color.White;
			this.btnTimKH.Location = new System.Drawing.Point(470, 120);
			this.btnTimKH.Name = "btnTimKH";
			this.btnTimKH.Size = new System.Drawing.Size(80, 34);
			this.btnTimKH.TabIndex = 12;
			this.btnTimKH.Text = "Tìm";
			this.btnTimKH.UseVisualStyleBackColor = false;
			this.btnTimKH.Click += new System.EventHandler(this.btnTimKH_Click);
			// 
			// btnDatVe
			// 
			this.btnDatVe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.btnDatVe.FlatAppearance.BorderSize = 0;
			this.btnDatVe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDatVe.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDatVe.ForeColor = System.Drawing.Color.White;
			this.btnDatVe.Location = new System.Drawing.Point(23, 380);
			this.btnDatVe.Name = "btnDatVe";
			this.btnDatVe.Size = new System.Drawing.Size(200, 50);
			this.btnDatVe.TabIndex = 11;
			this.btnDatVe.Text = "Đặt Vé";
			this.btnDatVe.UseVisualStyleBackColor = false;
			this.btnDatVe.Click += new System.EventHandler(this.btnDatVe_Click);
			// 
			// lblTongTien
			// 
			this.lblTongTien.AutoSize = true;
			this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTongTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
			this.lblTongTien.Location = new System.Drawing.Point(23, 330);
			this.lblTongTien.Name = "lblTongTien";
			this.lblTongTien.Size = new System.Drawing.Size(186, 38);
			this.lblTongTien.TabIndex = 10;
			this.lblTongTien.Text = "Tổng: 0 VNĐ";
			// 
			// lblThongTinChuyen
			// 
			this.lblThongTinChuyen.AutoSize = true;
			this.lblThongTinChuyen.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblThongTinChuyen.Location = new System.Drawing.Point(23, 180);
			this.lblThongTinChuyen.MaximumSize = new System.Drawing.Size(1200, 0);
			this.lblThongTinChuyen.Name = "lblThongTinChuyen";
			this.lblThongTinChuyen.Size = new System.Drawing.Size(172, 28);
			this.lblThongTinChuyen.TabIndex = 9;
			this.lblThongTinChuyen.Text = "Chưa chọn chuyến";
			// 
			// cboGioiTinh
			// 
			this.cboGioiTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGioiTinh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboGioiTinh.FormattingEnabled = true;
			this.cboGioiTinh.Items.AddRange(new object[] {
            "Nam",
            "Nữ"});
			this.cboGioiTinh.Location = new System.Drawing.Point(720, 120);
			this.cboGioiTinh.Name = "cboGioiTinh";
			this.cboGioiTinh.Size = new System.Drawing.Size(200, 36);
			this.cboGioiTinh.TabIndex = 8;
			// 
			// dtpNamSinh
			// 
			this.dtpNamSinh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dtpNamSinh.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtpNamSinh.Location = new System.Drawing.Point(720, 70);
			this.dtpNamSinh.Name = "dtpNamSinh";
			this.dtpNamSinh.Size = new System.Drawing.Size(200, 34);
			this.dtpNamSinh.TabIndex = 7;
			// 
			// txtSDT
			// 
			this.txtSDT.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSDT.Location = new System.Drawing.Point(160, 120);
			this.txtSDT.Name = "txtSDT";
			this.txtSDT.Size = new System.Drawing.Size(300, 34);
			this.txtSDT.TabIndex = 6;
			// 
			// txtTenKH
			// 
			this.txtTenKH.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtTenKH.Location = new System.Drawing.Point(160, 70);
			this.txtTenKH.Name = "txtTenKH";
			this.txtTenKH.Size = new System.Drawing.Size(300, 34);
			this.txtTenKH.TabIndex = 5;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(600, 123);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(91, 28);
			this.label10.TabIndex = 4;
			this.label10.Text = "Giới tính:";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(600, 73);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(98, 28);
			this.label9.TabIndex = 3;
			this.label9.Text = "Năm sinh:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(23, 123);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(132, 28);
			this.label8.TabIndex = 2;
			this.label8.Text = "Số điện thoại:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(23, 73);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(75, 28);
			this.label7.TabIndex = 1;
			this.label7.Text = "Họ tên:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(23, 20);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(305, 38);
			this.label6.TabIndex = 0;
			this.label6.Text = "Thông tin khách hàng";
			// 
			// UCDatVe
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.flowMain);
			this.Name = "UCDatVe";
			this.Size = new System.Drawing.Size(1351, 745);
			this.flowMain.ResumeLayout(false);
			this.flowMain.PerformLayout();
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearch.PerformLayout();
			this.pnlResults.ResumeLayout(false);
			this.pnlResults.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvChuyenXe)).EndInit();
			this.pnlSeatMap.ResumeLayout(false);
			this.pnlSeatMap.PerformLayout();
			this.gbTangTren.ResumeLayout(false);
			this.gbTangDuoi.ResumeLayout(false);
			this.pnlLegend.ResumeLayout(false);
			this.pnlLegend.PerformLayout();
			this.pnlThongTinKH.ResumeLayout(false);
			this.pnlThongTinKH.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowMain;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Panel pnlSearch;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cboDiemDi;
		private System.Windows.Forms.DateTimePicker dtpNgayDi;
		private System.Windows.Forms.ComboBox cboDiemDen;
		private System.Windows.Forms.Button btnTimChuyen;
		private System.Windows.Forms.Panel pnlResults;
		private System.Windows.Forms.DataGridView dgvChuyenXe;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel pnlSeatMap;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Panel pnlLegend;
		private System.Windows.Forms.Button btnLegendTrong;
		private System.Windows.Forms.Button btnLegendDaDat;
		private System.Windows.Forms.Button btnLegendDangChon;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.GroupBox gbTangDuoi;
		private System.Windows.Forms.FlowLayoutPanel flpTangDuoi;
		private System.Windows.Forms.GroupBox gbTangTren;
		private System.Windows.Forms.FlowLayoutPanel flpTangTren;
		private System.Windows.Forms.Panel pnlThongTinKH;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtTenKH;
		private System.Windows.Forms.TextBox txtSDT;
		private System.Windows.Forms.DateTimePicker dtpNamSinh;
		private System.Windows.Forms.ComboBox cboGioiTinh;
		private System.Windows.Forms.Label lblThongTinChuyen;
		private System.Windows.Forms.Label lblTongTien;
		private System.Windows.Forms.Button btnDatVe;
		private System.Windows.Forms.ComboBox cboGioDi;
		private System.Windows.Forms.Label labelGioDi;
		private System.Windows.Forms.Button btnTimKH;
	}
}
