namespace QLBXMD.GUI.Admin
{
	partial class UCLoaiXe
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
			this.cbbLoai = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.dgv = new System.Windows.Forms.DataGridView();
			this.MaLX = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TenLX = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SoCho = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnXoa = new System.Windows.Forms.Button();
			this.btnSua = new System.Windows.Forms.Button();
			this.btnXem = new System.Windows.Forms.Button();
			this.btnThem = new System.Windows.Forms.Button();
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
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1344, 688);
			this.panel1.TabIndex = 7;
			// 
			// cbbLoai
			// 
			this.cbbLoai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbLoai.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbbLoai.FormattingEnabled = true;
			this.cbbLoai.Items.AddRange(new object[] {
            "Tất cả",
            "Mã nhà xe",
            "Tên nhà xe",
            "Số điện thoại",
            "Email",
            "Trạng Thái"});
			this.cbbLoai.Location = new System.Drawing.Point(456, 92);
			this.cbbLoai.Name = "cbbLoai";
			this.cbbLoai.Size = new System.Drawing.Size(155, 33);
			this.cbbLoai.TabIndex = 4;
			this.cbbLoai.SelectedIndexChanged += new System.EventHandler(this.cbbLoai_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(35, 96);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(91, 25);
			this.label2.TabIndex = 3;
			this.label2.Text = "Tìm kiếm";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.IndianRed;
			this.label1.Location = new System.Drawing.Point(32, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(276, 48);
			this.label1.TabIndex = 2;
			this.label1.Text = "Quản lý Loại xe";
			// 
			// txtSearch
			// 
			this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSearch.Location = new System.Drawing.Point(132, 92);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(305, 30);
			this.txtSearch.TabIndex = 1;
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
            this.MaLX,
            this.TenLX,
            this.SoCho,
            this.TrangThai});
			this.dgv.Location = new System.Drawing.Point(27, 192);
			this.dgv.Name = "dgv";
			this.dgv.ReadOnly = true;
			this.dgv.RowHeadersWidth = 62;
			this.dgv.RowTemplate.Height = 28;
			this.dgv.Size = new System.Drawing.Size(1304, 477);
			this.dgv.TabIndex = 0;
			this.dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLoaiXe_CellClick);
			this.dgv.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLoaiXe_CellDoubleClick);
			this.dgv.SelectionChanged += new System.EventHandler(this.dgvLoaiXe_SelectionChanged);
			// 
			// MaLX
			// 
			this.MaLX.FillWeight = 79.32772F;
			this.MaLX.HeaderText = "Mã loại xe";
			this.MaLX.MinimumWidth = 8;
			this.MaLX.Name = "MaLX";
			this.MaLX.ReadOnly = true;
			// 
			// TenLX
			// 
			this.TenLX.HeaderText = "Tên loại xe";
			this.TenLX.MinimumWidth = 8;
			this.TenLX.Name = "TenLX";
			this.TenLX.ReadOnly = true;
			// 
			// SoCho
			// 
			this.SoCho.HeaderText = "Số chỗ";
			this.SoCho.MinimumWidth = 8;
			this.SoCho.Name = "SoCho";
			this.SoCho.ReadOnly = true;
			// 
			// TrangThai
			// 
			this.TrangThai.FillWeight = 150.2612F;
			this.TrangThai.HeaderText = "Trạng thái";
			this.TrangThai.MinimumWidth = 8;
			this.TrangThai.Name = "TrangThai";
			this.TrangThai.ReadOnly = true;
			// 
			// btnXoa
			// 
			this.btnXoa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnXoa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
			this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnXoa.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnXoa.ForeColor = System.Drawing.Color.White;
			this.btnXoa.Image = global::QLBXMD.Properties.Resources.delete;
			this.btnXoa.Location = new System.Drawing.Point(1201, 83);
			this.btnXoa.Name = "btnXoa";
			this.btnXoa.Size = new System.Drawing.Size(130, 50);
			this.btnXoa.TabIndex = 22;
			this.btnXoa.Text = "Xóa";
			this.btnXoa.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnXoa.UseVisualStyleBackColor = false;
			this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
			// 
			// btnSua
			// 
			this.btnSua.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSua.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
			this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSua.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSua.ForeColor = System.Drawing.Color.White;
			this.btnSua.Image = global::QLBXMD.Properties.Resources.edit;
			this.btnSua.Location = new System.Drawing.Point(1065, 83);
			this.btnSua.Name = "btnSua";
			this.btnSua.Size = new System.Drawing.Size(130, 50);
			this.btnSua.TabIndex = 21;
			this.btnSua.Text = "Sửa";
			this.btnSua.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnSua.UseVisualStyleBackColor = false;
			this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
			// 
			// btnXem
			// 
			this.btnXem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnXem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
			this.btnXem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnXem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnXem.ForeColor = System.Drawing.Color.White;
			this.btnXem.Image = global::QLBXMD.Properties.Resources.view;
			this.btnXem.Location = new System.Drawing.Point(927, 83);
			this.btnXem.Name = "btnXem";
			this.btnXem.Size = new System.Drawing.Size(130, 50);
			this.btnXem.TabIndex = 20;
			this.btnXem.Text = "Xem";
			this.btnXem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnXem.UseVisualStyleBackColor = false;
			this.btnXem.Click += new System.EventHandler(this.btnXem_Click);
			// 
			// btnThem
			// 
			this.btnThem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnThem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
			this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnThem.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnThem.ForeColor = System.Drawing.Color.White;
			this.btnThem.Image = global::QLBXMD.Properties.Resources.add;
			this.btnThem.Location = new System.Drawing.Point(791, 83);
			this.btnThem.Name = "btnThem";
			this.btnThem.Size = new System.Drawing.Size(130, 50);
			this.btnThem.TabIndex = 19;
			this.btnThem.Text = "Thêm";
			this.btnThem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnThem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnThem.UseVisualStyleBackColor = false;
			this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
			// 
			// UCLoaiXe
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Name = "UCLoaiXe";
			this.Size = new System.Drawing.Size(1350, 731);
			this.Load += new System.EventHandler(this.UCLoaiXe_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ComboBox cbbLoai;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.DataGridView dgv;
		private System.Windows.Forms.DataGridViewTextBoxColumn MaLX;
		private System.Windows.Forms.DataGridViewTextBoxColumn TenLX;
		private System.Windows.Forms.DataGridViewTextBoxColumn SoCho;
		private System.Windows.Forms.DataGridViewTextBoxColumn TrangThai;
		private System.Windows.Forms.Button btnXoa;
		private System.Windows.Forms.Button btnSua;
		private System.Windows.Forms.Button btnXem;
		private System.Windows.Forms.Button btnThem;
	}
}
