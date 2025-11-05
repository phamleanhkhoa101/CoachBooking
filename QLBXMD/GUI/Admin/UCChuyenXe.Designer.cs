namespace QLBXMD.GUI.Admin
{
	partial class UCChuyenXe
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnXoa = new System.Windows.Forms.Button();
			this.btnSua = new System.Windows.Forms.Button();
			this.btnXem = new System.Windows.Forms.Button();
			this.btnThem = new System.Windows.Forms.Button();
			this.cbbLoai = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.dgv = new System.Windows.Forms.DataGridView();
			this.MaTuyen = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DiemDi = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DiemDen = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.QuangDuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ThoiGianDuKien = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.btnXoa);
			this.panel1.Controls.Add(this.btnSua);
			this.panel1.Controls.Add(this.btnXem);
			this.panel1.Controls.Add(this.btnThem);
			this.panel1.Controls.Add(this.cbbLoai);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.txtSearch);
			this.panel1.Controls.Add(this.dgv);
			this.panel1.Location = new System.Drawing.Point(6, 49);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1339, 637);
			this.panel1.TabIndex = 9;
			// 
			// btnXoa
			// 
			this.btnXoa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnXoa.Location = new System.Drawing.Point(1208, 110);
			this.btnXoa.Name = "btnXoa";
			this.btnXoa.Size = new System.Drawing.Size(118, 39);
			this.btnXoa.TabIndex = 8;
			this.btnXoa.Text = "Xóa";
			this.btnXoa.UseVisualStyleBackColor = true;
			this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
			// 
			// btnSua
			// 
			this.btnSua.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSua.Location = new System.Drawing.Point(1084, 110);
			this.btnSua.Name = "btnSua";
			this.btnSua.Size = new System.Drawing.Size(118, 39);
			this.btnSua.TabIndex = 7;
			this.btnSua.Text = "Sửa";
			this.btnSua.UseVisualStyleBackColor = true;
			this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
			// 
			// btnXem
			// 
			this.btnXem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnXem.Location = new System.Drawing.Point(960, 110);
			this.btnXem.Name = "btnXem";
			this.btnXem.Size = new System.Drawing.Size(118, 39);
			this.btnXem.TabIndex = 6;
			this.btnXem.Text = "Xem";
			this.btnXem.UseVisualStyleBackColor = true;
			this.btnXem.Click += new System.EventHandler(this.btnXem_Click);
			// 
			// btnThem
			// 
			this.btnThem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnThem.Location = new System.Drawing.Point(836, 111);
			this.btnThem.Name = "btnThem";
			this.btnThem.Size = new System.Drawing.Size(118, 38);
			this.btnThem.TabIndex = 5;
			this.btnThem.Text = "Thêm";
			this.btnThem.UseVisualStyleBackColor = true;
			this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
			// 
			// cbbLoai
			// 
			this.cbbLoai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbLoai.FormattingEnabled = true;
			this.cbbLoai.Location = new System.Drawing.Point(456, 116);
			this.cbbLoai.Name = "cbbLoai";
			this.cbbLoai.Size = new System.Drawing.Size(155, 28);
			this.cbbLoai.TabIndex = 4;
			this.cbbLoai.SelectedIndexChanged += new System.EventHandler(this.cbbLoai_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(35, 116);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(71, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "Tìm kiếm";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(32, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(303, 37);
			this.label1.TabIndex = 2;
			this.label1.Text = "Quản lý Chuyến xe";
			// 
			// txtSearch
			// 
			this.txtSearch.Location = new System.Drawing.Point(127, 116);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(305, 26);
			this.txtSearch.TabIndex = 1;
			this.txtSearch.Text = "Tìm kiếm";
			this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
			// 
			// dgv
			// 
			this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MaTuyen,
            this.DiemDi,
            this.DiemDen,
            this.QuangDuong,
            this.ThoiGianDuKien,
            this.TrangThai});
			this.dgv.Location = new System.Drawing.Point(27, 195);
			this.dgv.MultiSelect = false;
			this.dgv.Name = "dgv";
			this.dgv.ReadOnly = true;
			this.dgv.RowHeadersWidth = 62;
			this.dgv.RowTemplate.Height = 28;
			this.dgv.Size = new System.Drawing.Size(1299, 426);
			this.dgv.TabIndex = 0;
			this.dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);
			this.dgv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellContentClick);
			// 
			// MaTuyen
			// 
			this.MaTuyen.FillWeight = 79.32772F;
			this.MaTuyen.HeaderText = "Mã Tuyến";
			this.MaTuyen.MinimumWidth = 8;
			this.MaTuyen.Name = "MaTuyen";
			this.MaTuyen.ReadOnly = true;
			// 
			// DiemDi
			// 
			this.DiemDi.HeaderText = "Điểm đi";
			this.DiemDi.MinimumWidth = 8;
			this.DiemDi.Name = "DiemDi";
			this.DiemDi.ReadOnly = true;
			// 
			// DiemDen
			// 
			this.DiemDen.HeaderText = "Điểm đến";
			this.DiemDen.MinimumWidth = 8;
			this.DiemDen.Name = "DiemDen";
			this.DiemDen.ReadOnly = true;
			// 
			// QuangDuong
			// 
			this.QuangDuong.HeaderText = "Quãng đường (km)";
			this.QuangDuong.MinimumWidth = 8;
			this.QuangDuong.Name = "QuangDuong";
			this.QuangDuong.ReadOnly = true;
			// 
			// ThoiGianDuKien
			// 
			this.ThoiGianDuKien.HeaderText = "Thời gian dự kiến (phút)";
			this.ThoiGianDuKien.MinimumWidth = 8;
			this.ThoiGianDuKien.Name = "ThoiGianDuKien";
			this.ThoiGianDuKien.ReadOnly = true;
			// 
			// TrangThai
			// 
			this.TrangThai.FillWeight = 150.2612F;
			this.TrangThai.HeaderText = "Trạng thái";
			this.TrangThai.MinimumWidth = 8;
			this.TrangThai.Name = "TrangThai";
			this.TrangThai.ReadOnly = true;
			// 
			// UCChuyenXe
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Name = "UCChuyenXe";
			this.Size = new System.Drawing.Size(1351, 734);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnXoa;
		private System.Windows.Forms.Button btnSua;
		private System.Windows.Forms.Button btnXem;
		private System.Windows.Forms.Button btnThem;
		private System.Windows.Forms.ComboBox cbbLoai;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.DataGridView dgv;
		private System.Windows.Forms.DataGridViewTextBoxColumn MaTuyen;
		private System.Windows.Forms.DataGridViewTextBoxColumn DiemDi;
		private System.Windows.Forms.DataGridViewTextBoxColumn DiemDen;
		private System.Windows.Forms.DataGridViewTextBoxColumn QuangDuong;
		private System.Windows.Forms.DataGridViewTextBoxColumn ThoiGianDuKien;
		private System.Windows.Forms.DataGridViewTextBoxColumn TrangThai;
	}
}
