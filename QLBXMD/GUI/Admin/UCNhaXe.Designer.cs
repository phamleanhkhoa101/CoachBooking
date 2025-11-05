namespace QLBXMD.GUI.Admin
{
	partial class UCNhaXe
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
		/// 
		private void InitializeComponent()
		{
			this.dgvNhaXe = new System.Windows.Forms.DataGridView();
			this.MaNX = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TenNX = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.SDT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TrangThai = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cbbLoai = new System.Windows.Forms.ComboBox();
			this.btnThem = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnXoa = new System.Windows.Forms.Button();
			this.btnSua = new System.Windows.Forms.Button();
			this.btnXem = new System.Windows.Forms.Button();
			this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
			((System.ComponentModel.ISupportInitialize)(this.dgvNhaXe)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgvNhaXe
			// 
			this.dgvNhaXe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvNhaXe.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvNhaXe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvNhaXe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MaNX,
            this.TenNX,
            this.SDT,
            this.Email,
            this.TrangThai});
			this.dgvNhaXe.Location = new System.Drawing.Point(27, 192);
			this.dgvNhaXe.Name = "dgvNhaXe";
			this.dgvNhaXe.ReadOnly = true;
			this.dgvNhaXe.RowHeadersWidth = 62;
			this.dgvNhaXe.RowTemplate.Height = 28;
			this.dgvNhaXe.Size = new System.Drawing.Size(1254, 440);
			this.dgvNhaXe.TabIndex = 0;
			this.dgvNhaXe.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNhaXe_CellClick);
			this.dgvNhaXe.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNhaXe_CellDoubleClick);
			this.dgvNhaXe.SelectionChanged += new System.EventHandler(this.dgvNhaXe_SelectionChanged);
			// 
			// MaNX
			// 
			this.MaNX.FillWeight = 79.32772F;
			this.MaNX.HeaderText = "Mã nhà xe";
			this.MaNX.MinimumWidth = 8;
			this.MaNX.Name = "MaNX";
			this.MaNX.ReadOnly = true;
			// 
			// TenNX
			// 
			this.TenNX.FillWeight = 83.96416F;
			this.TenNX.HeaderText = "Tên nhà xe";
			this.TenNX.MinimumWidth = 8;
			this.TenNX.Name = "TenNX";
			this.TenNX.ReadOnly = true;
			// 
			// SDT
			// 
			this.SDT.FillWeight = 109.4238F;
			this.SDT.HeaderText = "Số điện thoại";
			this.SDT.MinimumWidth = 8;
			this.SDT.Name = "SDT";
			this.SDT.ReadOnly = true;
			// 
			// Email
			// 
			this.Email.FillWeight = 82.59615F;
			this.Email.HeaderText = "Email";
			this.Email.MinimumWidth = 8;
			this.Email.Name = "Email";
			this.Email.ReadOnly = true;
			// 
			// TrangThai
			// 
			this.TrangThai.FillWeight = 150.2612F;
			this.TrangThai.HeaderText = "Trạng thái";
			this.TrangThai.MinimumWidth = 8;
			this.TrangThai.Name = "TrangThai";
			this.TrangThai.ReadOnly = true;
			// 
			// txtSearch
			// 
			this.txtSearch.Location = new System.Drawing.Point(127, 116);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(305, 26);
			this.txtSearch.TabIndex = 1;
			this.txtSearch.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(32, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(251, 37);
			this.label1.TabIndex = 2;
			this.label1.Text = "Quản lý Nhà xe";
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
			// cbbLoai
			// 
			this.cbbLoai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbLoai.FormattingEnabled = true;
			this.cbbLoai.Items.AddRange(new object[] {
            "Tất cả",
            "Mã nhà xe",
            "Tên nhà xe",
            "Số điện thoại",
            "Email",
            "Trạng Thái"});
			this.cbbLoai.Location = new System.Drawing.Point(456, 116);
			this.cbbLoai.Name = "cbbLoai";
			this.cbbLoai.Size = new System.Drawing.Size(155, 28);
			this.cbbLoai.TabIndex = 4;
			this.cbbLoai.SelectedIndexChanged += new System.EventHandler(this.cbbLoai_SelectedIndexChanged);
			// 
			// btnThem
			// 
			this.btnThem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnThem.Location = new System.Drawing.Point(791, 111);
			this.btnThem.Name = "btnThem";
			this.btnThem.Size = new System.Drawing.Size(118, 38);
			this.btnThem.TabIndex = 5;
			this.btnThem.Text = "Thêm";
			this.btnThem.UseVisualStyleBackColor = true;
			this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
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
			this.panel1.Controls.Add(this.dgvNhaXe);
			this.panel1.Location = new System.Drawing.Point(32, 47);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1294, 651);
			this.panel1.TabIndex = 6;
			// 
			// btnXoa
			// 
			this.btnXoa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnXoa.Location = new System.Drawing.Point(1163, 110);
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
			this.btnSua.Location = new System.Drawing.Point(1039, 110);
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
			this.btnXem.Location = new System.Drawing.Point(915, 110);
			this.btnXem.Name = "btnXem";
			this.btnXem.Size = new System.Drawing.Size(118, 39);
			this.btnXem.TabIndex = 6;
			this.btnXem.Text = "Xem";
			this.btnXem.UseVisualStyleBackColor = true;
			this.btnXem.Click += new System.EventHandler(this.btnXem_Click);
			// 
			// dataGridViewImageColumn1
			// 
			this.dataGridViewImageColumn1.HeaderText = "test";
			this.dataGridViewImageColumn1.Image = global::QLBXMD.Properties.Resources.plus;
			this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
			this.dataGridViewImageColumn1.MinimumWidth = 8;
			this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
			this.dataGridViewImageColumn1.Width = 150;
			// 
			// UCNhaXe
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Name = "UCNhaXe";
			this.Size = new System.Drawing.Size(1351, 731);
			this.Load += new System.EventHandler(this.UCNhaXe_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvNhaXe)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvNhaXe;
		private System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbbLoai;
		private System.Windows.Forms.Button btnThem;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn MaNX;
		private System.Windows.Forms.DataGridViewTextBoxColumn TenNX;
		private System.Windows.Forms.DataGridViewTextBoxColumn SDT;
		private System.Windows.Forms.DataGridViewTextBoxColumn Email;
		private System.Windows.Forms.DataGridViewTextBoxColumn TrangThai;
		private System.Windows.Forms.Button btnXoa;
		private System.Windows.Forms.Button btnSua;
		private System.Windows.Forms.Button btnXem;
	}
}
