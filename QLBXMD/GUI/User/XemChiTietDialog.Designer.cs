namespace QLBXMD.GUI.User
{
	partial class XemChiTietDialog
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
			this.pnlHeader = new System.Windows.Forms.Panel();
			this.lblTitle = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.pnlContent = new System.Windows.Forms.Panel();
			this.gbThongTinVe = new System.Windows.Forms.GroupBox();
			this.lblGheValue = new System.Windows.Forms.Label();
			this.lblGhe = new System.Windows.Forms.Label();
			this.lblSoLuongValue = new System.Windows.Forms.Label();
			this.lblSoLuong = new System.Windows.Forms.Label();
			this.lblGiaVeValue = new System.Windows.Forms.Label();
			this.lblGiaVe = new System.Windows.Forms.Label();
			this.gbThongTinChuyen = new System.Windows.Forms.GroupBox();
			this.lblDenDuKienValue = new System.Windows.Forms.Label();
			this.lblDenDuKien = new System.Windows.Forms.Label();
			this.lblKhoiHanhValue = new System.Windows.Forms.Label();
			this.lblKhoiHanh = new System.Windows.Forms.Label();
			this.lblLoaiXeValue = new System.Windows.Forms.Label();
			this.lblLoaiXe = new System.Windows.Forms.Label();
			this.lblNhaXeValue = new System.Windows.Forms.Label();
			this.lblNhaXe = new System.Windows.Forms.Label();
			this.lblTuyenValue = new System.Windows.Forms.Label();
			this.lblTuyen = new System.Windows.Forms.Label();
			this.gbThongTinHoaDon = new System.Windows.Forms.GroupBox();
			this.lblTrangThaiValue = new System.Windows.Forms.Label();
			this.lblTrangThai = new System.Windows.Forms.Label();
			this.lblNgayLapValue = new System.Windows.Forms.Label();
			this.lblNgayLap = new System.Windows.Forms.Label();
			this.lblMaHDValue = new System.Windows.Forms.Label();
			this.lblMaHD = new System.Windows.Forms.Label();
			this.pnlFooter = new System.Windows.Forms.Panel();
			this.lblTongTien = new System.Windows.Forms.Label();
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.btnHuyVe = new System.Windows.Forms.Button();
			this.btnThanhToan = new System.Windows.Forms.Button();
			this.btnDong = new System.Windows.Forms.Button();
			this.pnlHeader.SuspendLayout();
			this.pnlContent.SuspendLayout();
			this.gbThongTinVe.SuspendLayout();
			this.gbThongTinChuyen.SuspendLayout();
			this.gbThongTinHoaDon.SuspendLayout();
			this.pnlFooter.SuspendLayout();
			this.pnlButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlHeader
			// 
			this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.pnlHeader.Controls.Add(this.lblTitle);
			this.pnlHeader.Controls.Add(this.btnClose);
			this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlHeader.Location = new System.Drawing.Point(0, 0);
			this.pnlHeader.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.pnlHeader.Name = "pnlHeader";
			this.pnlHeader.Size = new System.Drawing.Size(1050, 82);
			this.pnlHeader.TabIndex = 0;
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.lblTitle.ForeColor = System.Drawing.Color.White;
			this.lblTitle.Location = new System.Drawing.Point(372, 15);
			this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(313, 45);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "CHI TIẾT HÓA ĐƠN";
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.BackColor = System.Drawing.Color.Transparent;
			this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnClose.FlatAppearance.BorderSize = 0;
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.btnClose.ForeColor = System.Drawing.Color.White;
			this.btnClose.Location = new System.Drawing.Point(975, 15);
			this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(60, 62);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "×";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// pnlContent
			// 
			this.pnlContent.AutoScroll = true;
			this.pnlContent.BackColor = System.Drawing.Color.White;
			this.pnlContent.Controls.Add(this.gbThongTinVe);
			this.pnlContent.Controls.Add(this.gbThongTinChuyen);
			this.pnlContent.Controls.Add(this.gbThongTinHoaDon);
			this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlContent.Location = new System.Drawing.Point(0, 82);
			this.pnlContent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.pnlContent.Name = "pnlContent";
			this.pnlContent.Padding = new System.Windows.Forms.Padding(30, 31, 30, 31);
			this.pnlContent.Size = new System.Drawing.Size(1050, 728);
			this.pnlContent.TabIndex = 1;
			// 
			// gbThongTinVe
			// 
			this.gbThongTinVe.Controls.Add(this.lblGheValue);
			this.gbThongTinVe.Controls.Add(this.lblGhe);
			this.gbThongTinVe.Controls.Add(this.lblSoLuongValue);
			this.gbThongTinVe.Controls.Add(this.lblSoLuong);
			this.gbThongTinVe.Controls.Add(this.lblGiaVeValue);
			this.gbThongTinVe.Controls.Add(this.lblGiaVe);
			this.gbThongTinVe.Dock = System.Windows.Forms.DockStyle.Top;
			this.gbThongTinVe.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.gbThongTinVe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.gbThongTinVe.Location = new System.Drawing.Point(30, 523);
			this.gbThongTinVe.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.gbThongTinVe.Name = "gbThongTinVe";
			this.gbThongTinVe.Padding = new System.Windows.Forms.Padding(22, 23, 22, 23);
			this.gbThongTinVe.Size = new System.Drawing.Size(964, 215);
			this.gbThongTinVe.TabIndex = 2;
			this.gbThongTinVe.TabStop = false;
			this.gbThongTinVe.Text = "THÔNG TIN VÉ";
			// 
			// lblGheValue
			// 
			this.lblGheValue.AutoSize = true;
			this.lblGheValue.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblGheValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
			this.lblGheValue.Location = new System.Drawing.Point(270, 154);
			this.lblGheValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblGheValue.Name = "lblGheValue";
			this.lblGheValue.Size = new System.Drawing.Size(23, 28);
			this.lblGheValue.TabIndex = 5;
			this.lblGheValue.Text = "1";
			// 
			// lblGhe
			// 
			this.lblGhe.AutoSize = true;
			this.lblGhe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblGhe.ForeColor = System.Drawing.Color.Gray;
			this.lblGhe.Location = new System.Drawing.Point(30, 154);
			this.lblGhe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblGhe.Name = "lblGhe";
			this.lblGhe.Size = new System.Drawing.Size(108, 28);
			this.lblGhe.TabIndex = 4;
			this.lblGhe.Text = "💺 Ghế số:";
			// 
			// lblSoLuongValue
			// 
			this.lblSoLuongValue.AutoSize = true;
			this.lblSoLuongValue.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblSoLuongValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
			this.lblSoLuongValue.Location = new System.Drawing.Point(270, 100);
			this.lblSoLuongValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblSoLuongValue.Name = "lblSoLuongValue";
			this.lblSoLuongValue.Size = new System.Drawing.Size(23, 28);
			this.lblSoLuongValue.TabIndex = 3;
			this.lblSoLuongValue.Text = "1";
			// 
			// lblSoLuong
			// 
			this.lblSoLuong.AutoSize = true;
			this.lblSoLuong.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblSoLuong.ForeColor = System.Drawing.Color.Gray;
			this.lblSoLuong.Location = new System.Drawing.Point(30, 100);
			this.lblSoLuong.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblSoLuong.Name = "lblSoLuong";
			this.lblSoLuong.Size = new System.Drawing.Size(153, 28);
			this.lblSoLuong.TabIndex = 2;
			this.lblSoLuong.Text = "🎫 Số lượng vé:";
			// 
			// lblGiaVeValue
			// 
			this.lblGiaVeValue.AutoSize = true;
			this.lblGiaVeValue.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblGiaVeValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
			this.lblGiaVeValue.Location = new System.Drawing.Point(270, 46);
			this.lblGiaVeValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblGiaVeValue.Name = "lblGiaVeValue";
			this.lblGiaVeValue.Size = new System.Drawing.Size(40, 28);
			this.lblGiaVeValue.TabIndex = 1;
			this.lblGiaVeValue.Text = "0 đ";
			// 
			// lblGiaVe
			// 
			this.lblGiaVe.AutoSize = true;
			this.lblGiaVe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblGiaVe.ForeColor = System.Drawing.Color.Gray;
			this.lblGiaVe.Location = new System.Drawing.Point(30, 46);
			this.lblGiaVe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblGiaVe.Name = "lblGiaVe";
			this.lblGiaVe.Size = new System.Drawing.Size(141, 28);
			this.lblGiaVe.TabIndex = 0;
			this.lblGiaVe.Text = "💵 Giá mỗi vé:";
			// 
			// gbThongTinChuyen
			// 
			this.gbThongTinChuyen.Controls.Add(this.lblDenDuKienValue);
			this.gbThongTinChuyen.Controls.Add(this.lblDenDuKien);
			this.gbThongTinChuyen.Controls.Add(this.lblKhoiHanhValue);
			this.gbThongTinChuyen.Controls.Add(this.lblKhoiHanh);
			this.gbThongTinChuyen.Controls.Add(this.lblLoaiXeValue);
			this.gbThongTinChuyen.Controls.Add(this.lblLoaiXe);
			this.gbThongTinChuyen.Controls.Add(this.lblNhaXeValue);
			this.gbThongTinChuyen.Controls.Add(this.lblNhaXe);
			this.gbThongTinChuyen.Controls.Add(this.lblTuyenValue);
			this.gbThongTinChuyen.Controls.Add(this.lblTuyen);
			this.gbThongTinChuyen.Dock = System.Windows.Forms.DockStyle.Top;
			this.gbThongTinChuyen.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.gbThongTinChuyen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.gbThongTinChuyen.Location = new System.Drawing.Point(30, 246);
			this.gbThongTinChuyen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.gbThongTinChuyen.Name = "gbThongTinChuyen";
			this.gbThongTinChuyen.Padding = new System.Windows.Forms.Padding(22, 23, 22, 23);
			this.gbThongTinChuyen.Size = new System.Drawing.Size(964, 277);
			this.gbThongTinChuyen.TabIndex = 1;
			this.gbThongTinChuyen.TabStop = false;
			this.gbThongTinChuyen.Text = "THÔNG TIN CHUYẾN XE";
			// 
			// lblDenDuKienValue
			// 
			this.lblDenDuKienValue.AutoSize = true;
			this.lblDenDuKienValue.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblDenDuKienValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
			this.lblDenDuKienValue.Location = new System.Drawing.Point(270, 215);
			this.lblDenDuKienValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblDenDuKienValue.Name = "lblDenDuKienValue";
			this.lblDenDuKienValue.Size = new System.Drawing.Size(169, 28);
			this.lblDenDuKienValue.TabIndex = 9;
			this.lblDenDuKienValue.Text = "01/01/2024 08:00";
			// 
			// lblDenDuKien
			// 
			this.lblDenDuKien.AutoSize = true;
			this.lblDenDuKien.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblDenDuKien.ForeColor = System.Drawing.Color.Gray;
			this.lblDenDuKien.Location = new System.Drawing.Point(30, 215);
			this.lblDenDuKien.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblDenDuKien.Name = "lblDenDuKien";
			this.lblDenDuKien.Size = new System.Drawing.Size(153, 28);
			this.lblDenDuKien.TabIndex = 8;
			this.lblDenDuKien.Text = "🏁 Đến dự kiến:";
			// 
			// lblKhoiHanhValue
			// 
			this.lblKhoiHanhValue.AutoSize = true;
			this.lblKhoiHanhValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lblKhoiHanhValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
			this.lblKhoiHanhValue.Location = new System.Drawing.Point(270, 169);
			this.lblKhoiHanhValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblKhoiHanhValue.Name = "lblKhoiHanhValue";
			this.lblKhoiHanhValue.Size = new System.Drawing.Size(185, 28);
			this.lblKhoiHanhValue.TabIndex = 7;
			this.lblKhoiHanhValue.Text = "01/01/2024 08:00";
			// 
			// lblKhoiHanh
			// 
			this.lblKhoiHanh.AutoSize = true;
			this.lblKhoiHanh.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblKhoiHanh.ForeColor = System.Drawing.Color.Gray;
			this.lblKhoiHanh.Location = new System.Drawing.Point(30, 169);
			this.lblKhoiHanh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblKhoiHanh.Name = "lblKhoiHanh";
			this.lblKhoiHanh.Size = new System.Drawing.Size(136, 28);
			this.lblKhoiHanh.TabIndex = 6;
			this.lblKhoiHanh.Text = "🕐 Khởi hành:";
			// 
			// lblLoaiXeValue
			// 
			this.lblLoaiXeValue.AutoSize = true;
			this.lblLoaiXeValue.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblLoaiXeValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
			this.lblLoaiXeValue.Location = new System.Drawing.Point(270, 123);
			this.lblLoaiXeValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblLoaiXeValue.Name = "lblLoaiXeValue";
			this.lblLoaiXeValue.Size = new System.Drawing.Size(121, 28);
			this.lblLoaiXeValue.TabIndex = 5;
			this.lblLoaiXeValue.Text = "Giường nằm";
			// 
			// lblLoaiXe
			// 
			this.lblLoaiXe.AutoSize = true;
			this.lblLoaiXe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblLoaiXe.ForeColor = System.Drawing.Color.Gray;
			this.lblLoaiXe.Location = new System.Drawing.Point(30, 123);
			this.lblLoaiXe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblLoaiXe.Name = "lblLoaiXe";
			this.lblLoaiXe.Size = new System.Drawing.Size(108, 28);
			this.lblLoaiXe.TabIndex = 4;
			this.lblLoaiXe.Text = "🚐 Loại xe:";
			// 
			// lblNhaXeValue
			// 
			this.lblNhaXeValue.AutoSize = true;
			this.lblNhaXeValue.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblNhaXeValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
			this.lblNhaXeValue.Location = new System.Drawing.Point(270, 77);
			this.lblNhaXeValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblNhaXeValue.Name = "lblNhaXeValue";
			this.lblNhaXeValue.Size = new System.Drawing.Size(134, 28);
			this.lblNhaXeValue.TabIndex = 3;
			this.lblNhaXeValue.Text = "Phương Trang";
			// 
			// lblNhaXe
			// 
			this.lblNhaXe.AutoSize = true;
			this.lblNhaXe.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblNhaXe.ForeColor = System.Drawing.Color.Gray;
			this.lblNhaXe.Location = new System.Drawing.Point(30, 77);
			this.lblNhaXe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblNhaXe.Name = "lblNhaXe";
			this.lblNhaXe.Size = new System.Drawing.Size(108, 28);
			this.lblNhaXe.TabIndex = 2;
			this.lblNhaXe.Text = "🏢 Nhà xe:";
			// 
			// lblTuyenValue
			// 
			this.lblTuyenValue.AutoSize = true;
			this.lblTuyenValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.lblTuyenValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
			this.lblTuyenValue.Location = new System.Drawing.Point(270, 38);
			this.lblTuyenValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTuyenValue.Name = "lblTuyenValue";
			this.lblTuyenValue.Size = new System.Drawing.Size(270, 32);
			this.lblTuyenValue.TabIndex = 1;
			this.lblTuyenValue.Text = "Hà Nội → Hồ Chí Minh";
			// 
			// lblTuyen
			// 
			this.lblTuyen.AutoSize = true;
			this.lblTuyen.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblTuyen.ForeColor = System.Drawing.Color.Gray;
			this.lblTuyen.Location = new System.Drawing.Point(30, 42);
			this.lblTuyen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTuyen.Name = "lblTuyen";
			this.lblTuyen.Size = new System.Drawing.Size(100, 28);
			this.lblTuyen.TabIndex = 0;
			this.lblTuyen.Text = "🚌 Tuyến:";
			// 
			// gbThongTinHoaDon
			// 
			this.gbThongTinHoaDon.Controls.Add(this.lblTrangThaiValue);
			this.gbThongTinHoaDon.Controls.Add(this.lblTrangThai);
			this.gbThongTinHoaDon.Controls.Add(this.lblNgayLapValue);
			this.gbThongTinHoaDon.Controls.Add(this.lblNgayLap);
			this.gbThongTinHoaDon.Controls.Add(this.lblMaHDValue);
			this.gbThongTinHoaDon.Controls.Add(this.lblMaHD);
			this.gbThongTinHoaDon.Dock = System.Windows.Forms.DockStyle.Top;
			this.gbThongTinHoaDon.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.gbThongTinHoaDon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.gbThongTinHoaDon.Location = new System.Drawing.Point(30, 31);
			this.gbThongTinHoaDon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.gbThongTinHoaDon.Name = "gbThongTinHoaDon";
			this.gbThongTinHoaDon.Padding = new System.Windows.Forms.Padding(22, 23, 22, 23);
			this.gbThongTinHoaDon.Size = new System.Drawing.Size(964, 215);
			this.gbThongTinHoaDon.TabIndex = 0;
			this.gbThongTinHoaDon.TabStop = false;
			this.gbThongTinHoaDon.Text = "THÔNG TIN HÓA ĐƠN";
			// 
			// lblTrangThaiValue
			// 
			this.lblTrangThaiValue.AutoSize = true;
			this.lblTrangThaiValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lblTrangThaiValue.ForeColor = System.Drawing.Color.Green;
			this.lblTrangThaiValue.Location = new System.Drawing.Point(270, 154);
			this.lblTrangThaiValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTrangThaiValue.Name = "lblTrangThaiValue";
			this.lblTrangThaiValue.Size = new System.Drawing.Size(170, 28);
			this.lblTrangThaiValue.TabIndex = 5;
			this.lblTrangThaiValue.Text = "✓ Đã thanh toán";
			// 
			// lblTrangThai
			// 
			this.lblTrangThai.AutoSize = true;
			this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblTrangThai.ForeColor = System.Drawing.Color.Gray;
			this.lblTrangThai.Location = new System.Drawing.Point(30, 154);
			this.lblTrangThai.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTrangThai.Name = "lblTrangThai";
			this.lblTrangThai.Size = new System.Drawing.Size(134, 28);
			this.lblTrangThai.TabIndex = 4;
			this.lblTrangThai.Text = "💳 Trạng thái:";
			// 
			// lblNgayLapValue
			// 
			this.lblNgayLapValue.AutoSize = true;
			this.lblNgayLapValue.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblNgayLapValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
			this.lblNgayLapValue.Location = new System.Drawing.Point(270, 100);
			this.lblNgayLapValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblNgayLapValue.Name = "lblNgayLapValue";
			this.lblNgayLapValue.Size = new System.Drawing.Size(169, 28);
			this.lblNgayLapValue.TabIndex = 3;
			this.lblNgayLapValue.Text = "01/01/2024 08:00";
			// 
			// lblNgayLap
			// 
			this.lblNgayLap.AutoSize = true;
			this.lblNgayLap.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblNgayLap.ForeColor = System.Drawing.Color.Gray;
			this.lblNgayLap.Location = new System.Drawing.Point(30, 100);
			this.lblNgayLap.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblNgayLap.Name = "lblNgayLap";
			this.lblNgayLap.Size = new System.Drawing.Size(129, 28);
			this.lblNgayLap.TabIndex = 2;
			this.lblNgayLap.Text = "📅 Ngày đặt:";
			// 
			// lblMaHDValue
			// 
			this.lblMaHDValue.AutoSize = true;
			this.lblMaHDValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.lblMaHDValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.lblMaHDValue.Location = new System.Drawing.Point(270, 46);
			this.lblMaHDValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblMaHDValue.Name = "lblMaHDValue";
			this.lblMaHDValue.Size = new System.Drawing.Size(42, 32);
			this.lblMaHDValue.TabIndex = 1;
			this.lblMaHDValue.Text = "#1";
			// 
			// lblMaHD
			// 
			this.lblMaHD.AutoSize = true;
			this.lblMaHD.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lblMaHD.ForeColor = System.Drawing.Color.Gray;
			this.lblMaHD.Location = new System.Drawing.Point(30, 49);
			this.lblMaHD.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblMaHD.Name = "lblMaHD";
			this.lblMaHD.Size = new System.Drawing.Size(122, 28);
			this.lblMaHD.TabIndex = 0;
			this.lblMaHD.Text = "Mã hóa đơn:";
			// 
			// pnlFooter
			// 
			this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
			this.pnlFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlFooter.Controls.Add(this.lblTongTien);
			this.pnlFooter.Controls.Add(this.pnlButtons);
			this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlFooter.Location = new System.Drawing.Point(0, 810);
			this.pnlFooter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.pnlFooter.Name = "pnlFooter";
			this.pnlFooter.Size = new System.Drawing.Size(1050, 113);
			this.pnlFooter.TabIndex = 2;
			// 
			// lblTongTien
			// 
			this.lblTongTien.Dock = System.Windows.Forms.DockStyle.Left;
			this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTongTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
			this.lblTongTien.Location = new System.Drawing.Point(0, 0);
			this.lblTongTien.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblTongTien.Name = "lblTongTien";
			this.lblTongTien.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
			this.lblTongTien.Size = new System.Drawing.Size(453, 111);
			this.lblTongTien.TabIndex = 0;
			this.lblTongTien.Text = "💰 0 VNĐ";
			this.lblTongTien.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.btnHuyVe);
			this.pnlButtons.Controls.Add(this.btnThanhToan);
			this.pnlButtons.Controls.Add(this.btnDong);
			this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlButtons.Location = new System.Drawing.Point(480, 0);
			this.pnlButtons.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.pnlButtons.Name = "pnlButtons";
			this.pnlButtons.Padding = new System.Windows.Forms.Padding(15, 15, 15, 15);
			this.pnlButtons.Size = new System.Drawing.Size(568, 111);
			this.pnlButtons.TabIndex = 1;
			// 
			// btnHuyVe
			// 
			this.btnHuyVe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
			this.btnHuyVe.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnHuyVe.FlatAppearance.BorderSize = 0;
			this.btnHuyVe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnHuyVe.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.btnHuyVe.ForeColor = System.Drawing.Color.White;
			this.btnHuyVe.Location = new System.Drawing.Point(366, 35);
			this.btnHuyVe.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnHuyVe.Name = "btnHuyVe";
			this.btnHuyVe.Size = new System.Drawing.Size(165, 56);
			this.btnHuyVe.TabIndex = 2;
			this.btnHuyVe.Text = "Hủy vé";
			this.btnHuyVe.UseVisualStyleBackColor = false;
			this.btnHuyVe.Visible = false;
			this.btnHuyVe.Click += new System.EventHandler(this.btnHuyVe_Click);
			// 
			// btnThanhToan
			// 
			this.btnThanhToan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(136)))));
			this.btnThanhToan.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnThanhToan.FlatAppearance.BorderSize = 0;
			this.btnThanhToan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnThanhToan.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.btnThanhToan.ForeColor = System.Drawing.Color.White;
			this.btnThanhToan.Location = new System.Drawing.Point(175, 35);
			this.btnThanhToan.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnThanhToan.Name = "btnThanhToan";
			this.btnThanhToan.Size = new System.Drawing.Size(180, 56);
			this.btnThanhToan.TabIndex = 1;
			this.btnThanhToan.Text = "Thanh toán";
			this.btnThanhToan.UseVisualStyleBackColor = false;
			this.btnThanhToan.Visible = false;
			this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);
			// 
			// btnDong
			// 
			this.btnDong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(158)))), ((int)(((byte)(158)))));
			this.btnDong.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnDong.FlatAppearance.BorderSize = 0;
			this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDong.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.btnDong.ForeColor = System.Drawing.Color.White;
			this.btnDong.Location = new System.Drawing.Point(366, 35);
			this.btnDong.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnDong.Name = "btnDong";
			this.btnDong.Size = new System.Drawing.Size(165, 56);
			this.btnDong.TabIndex = 0;
			this.btnDong.Text = "Đóng";
			this.btnDong.UseVisualStyleBackColor = false;
			this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
			// 
			// XemChiTietDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1050, 923);
			this.Controls.Add(this.pnlContent);
			this.Controls.Add(this.pnlFooter);
			this.Controls.Add(this.pnlHeader);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "XemChiTietDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Chi tiết hóa đơn";
			this.pnlHeader.ResumeLayout(false);
			this.pnlHeader.PerformLayout();
			this.pnlContent.ResumeLayout(false);
			this.gbThongTinVe.ResumeLayout(false);
			this.gbThongTinVe.PerformLayout();
			this.gbThongTinChuyen.ResumeLayout(false);
			this.gbThongTinChuyen.PerformLayout();
			this.gbThongTinHoaDon.ResumeLayout(false);
			this.gbThongTinHoaDon.PerformLayout();
			this.pnlFooter.ResumeLayout(false);
			this.pnlButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlHeader;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Panel pnlContent;
		private System.Windows.Forms.GroupBox gbThongTinHoaDon;
		private System.Windows.Forms.Label lblMaHD;
		private System.Windows.Forms.Label lblMaHDValue;
		private System.Windows.Forms.Label lblNgayLap;
		private System.Windows.Forms.Label lblNgayLapValue;
		private System.Windows.Forms.Label lblTrangThai;
		private System.Windows.Forms.Label lblTrangThaiValue;
		private System.Windows.Forms.GroupBox gbThongTinChuyen;
		private System.Windows.Forms.Label lblTuyen;
		private System.Windows.Forms.Label lblTuyenValue;
		private System.Windows.Forms.Label lblNhaXe;
		private System.Windows.Forms.Label lblNhaXeValue;
		private System.Windows.Forms.Label lblLoaiXe;
		private System.Windows.Forms.Label lblLoaiXeValue;
		private System.Windows.Forms.Label lblKhoiHanh;
		private System.Windows.Forms.Label lblKhoiHanhValue;
		private System.Windows.Forms.Label lblDenDuKien;
		private System.Windows.Forms.Label lblDenDuKienValue;
		private System.Windows.Forms.GroupBox gbThongTinVe;
		private System.Windows.Forms.Label lblGiaVe;
		private System.Windows.Forms.Label lblGiaVeValue;
		private System.Windows.Forms.Label lblSoLuong;
		private System.Windows.Forms.Label lblSoLuongValue;
		private System.Windows.Forms.Label lblGhe;
		private System.Windows.Forms.Label lblGheValue;
		private System.Windows.Forms.Panel pnlFooter;
		private System.Windows.Forms.Label lblTongTien;
		private System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.Button btnDong;
		private System.Windows.Forms.Button btnThanhToan;
		private System.Windows.Forms.Button btnHuyVe;
	}
}
