namespace QLBXMD.GUI.Admin
{
	partial class UCGiaVe
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabDaCo = new System.Windows.Forms.TabPage();
			this.dgvDaCo = new System.Windows.Forms.DataGridView();
			this.panel2 = new System.Windows.Forms.Panel();
			this.cbbLoai = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.tabChuaCo = new System.Windows.Forms.TabPage();
			this.dgvChuaCo = new System.Windows.Forms.DataGridView();
			this.panel3 = new System.Windows.Forms.Panel();
			this.cbbLoaiChuaCo = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtSearchChuaCo = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnXoa = new System.Windows.Forms.Button();
			this.btnSua = new System.Windows.Forms.Button();
			this.btnXem = new System.Windows.Forms.Button();
			this.btnThem = new System.Windows.Forms.Button();
			this.btnThemChuaCo = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabDaCo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvDaCo)).BeginInit();
			this.panel2.SuspendLayout();
			this.tabChuaCo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvChuaCo)).BeginInit();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.tabControl1);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Location = new System.Drawing.Point(0, 13);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1346, 671);
			this.panel1.TabIndex = 10;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabDaCo);
			this.tabControl1.Controls.Add(this.tabChuaCo);
			this.tabControl1.Location = new System.Drawing.Point(27, 70);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1306, 585);
			this.tabControl1.TabIndex = 3;
			// 
			// tabDaCo
			// 
			this.tabDaCo.Controls.Add(this.dgvDaCo);
			this.tabDaCo.Controls.Add(this.panel2);
			this.tabDaCo.Location = new System.Drawing.Point(4, 29);
			this.tabDaCo.Name = "tabDaCo";
			this.tabDaCo.Padding = new System.Windows.Forms.Padding(3);
			this.tabDaCo.Size = new System.Drawing.Size(1298, 552);
			this.tabDaCo.TabIndex = 0;
			this.tabDaCo.Text = "Đã có giá vé";
			this.tabDaCo.UseVisualStyleBackColor = true;
			// 
			// dgvDaCo
			// 
			this.dgvDaCo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvDaCo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvDaCo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvDaCo.Location = new System.Drawing.Point(20, 122);
			this.dgvDaCo.MultiSelect = false;
			this.dgvDaCo.Name = "dgvDaCo";
			this.dgvDaCo.ReadOnly = true;
			this.dgvDaCo.RowHeadersWidth = 62;
			this.dgvDaCo.RowTemplate.Height = 28;
			this.dgvDaCo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvDaCo.Size = new System.Drawing.Size(1258, 414);
			this.dgvDaCo.TabIndex = 1;
			this.dgvDaCo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDaCo_CellClick);
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.Controls.Add(this.btnXoa);
			this.panel2.Controls.Add(this.btnSua);
			this.panel2.Controls.Add(this.btnXem);
			this.panel2.Controls.Add(this.btnThem);
			this.panel2.Controls.Add(this.cbbLoai);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.txtSearch);
			this.panel2.Location = new System.Drawing.Point(20, 20);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(1258, 96);
			this.panel2.TabIndex = 0;
			// 
			// cbbLoai
			// 
			this.cbbLoai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbLoai.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbbLoai.FormattingEnabled = true;
			this.cbbLoai.Location = new System.Drawing.Point(432, 36);
			this.cbbLoai.Name = "cbbLoai";
			this.cbbLoai.Size = new System.Drawing.Size(155, 33);
			this.cbbLoai.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(11, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(91, 25);
			this.label2.TabIndex = 3;
			this.label2.Text = "Tìm kiếm";
			// 
			// txtSearch
			// 
			this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSearch.Location = new System.Drawing.Point(103, 37);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(305, 30);
			this.txtSearch.TabIndex = 1;
			this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
			// 
			// tabChuaCo
			// 
			this.tabChuaCo.Controls.Add(this.dgvChuaCo);
			this.tabChuaCo.Controls.Add(this.panel3);
			this.tabChuaCo.Location = new System.Drawing.Point(4, 29);
			this.tabChuaCo.Name = "tabChuaCo";
			this.tabChuaCo.Padding = new System.Windows.Forms.Padding(3);
			this.tabChuaCo.Size = new System.Drawing.Size(1298, 552);
			this.tabChuaCo.TabIndex = 1;
			this.tabChuaCo.Text = "Chưa có giá vé";
			this.tabChuaCo.UseVisualStyleBackColor = true;
			// 
			// dgvChuaCo
			// 
			this.dgvChuaCo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvChuaCo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgvChuaCo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvChuaCo.Location = new System.Drawing.Point(20, 105);
			this.dgvChuaCo.MultiSelect = false;
			this.dgvChuaCo.Name = "dgvChuaCo";
			this.dgvChuaCo.ReadOnly = true;
			this.dgvChuaCo.RowHeadersWidth = 62;
			this.dgvChuaCo.RowTemplate.Height = 28;
			this.dgvChuaCo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvChuaCo.Size = new System.Drawing.Size(1258, 431);
			this.dgvChuaCo.TabIndex = 1;
			this.dgvChuaCo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChuaCo_CellClick);
			// 
			// panel3
			// 
			this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel3.Controls.Add(this.btnThemChuaCo);
			this.panel3.Controls.Add(this.cbbLoaiChuaCo);
			this.panel3.Controls.Add(this.label3);
			this.panel3.Controls.Add(this.txtSearchChuaCo);
			this.panel3.Location = new System.Drawing.Point(20, 20);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(1258, 79);
			this.panel3.TabIndex = 0;
			// 
			// cbbLoaiChuaCo
			// 
			this.cbbLoaiChuaCo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbLoaiChuaCo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbbLoaiChuaCo.FormattingEnabled = true;
			this.cbbLoaiChuaCo.Location = new System.Drawing.Point(429, 21);
			this.cbbLoaiChuaCo.Name = "cbbLoaiChuaCo";
			this.cbbLoaiChuaCo.Size = new System.Drawing.Size(155, 33);
			this.cbbLoaiChuaCo.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(8, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(91, 25);
			this.label3.TabIndex = 3;
			this.label3.Text = "Tìm kiếm";
			// 
			// txtSearchChuaCo
			// 
			this.txtSearchChuaCo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSearchChuaCo.Location = new System.Drawing.Point(100, 22);
			this.txtSearchChuaCo.Name = "txtSearchChuaCo";
			this.txtSearchChuaCo.Size = new System.Drawing.Size(305, 30);
			this.txtSearchChuaCo.TabIndex = 1;
			this.txtSearchChuaCo.TextChanged += new System.EventHandler(this.txtSearchChuaCo_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Tomato;
			this.label1.Location = new System.Drawing.Point(32, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(263, 48);
			this.label1.TabIndex = 2;
			this.label1.Text = "Quản lý Giá vé";
			// 
			// btnXoa
			// 
			this.btnXoa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnXoa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
			this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnXoa.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnXoa.ForeColor = System.Drawing.Color.White;
			this.btnXoa.Image = global::QLBXMD.Properties.Resources.delete;
			this.btnXoa.Location = new System.Drawing.Point(1125, 27);
			this.btnXoa.Name = "btnXoa";
			this.btnXoa.Size = new System.Drawing.Size(130, 50);
			this.btnXoa.TabIndex = 18;
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
			this.btnSua.Location = new System.Drawing.Point(989, 27);
			this.btnSua.Name = "btnSua";
			this.btnSua.Size = new System.Drawing.Size(130, 50);
			this.btnSua.TabIndex = 17;
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
			this.btnXem.Location = new System.Drawing.Point(851, 27);
			this.btnXem.Name = "btnXem";
			this.btnXem.Size = new System.Drawing.Size(130, 50);
			this.btnXem.TabIndex = 16;
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
			this.btnThem.Location = new System.Drawing.Point(715, 27);
			this.btnThem.Name = "btnThem";
			this.btnThem.Size = new System.Drawing.Size(130, 50);
			this.btnThem.TabIndex = 15;
			this.btnThem.Text = "Thêm";
			this.btnThem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnThem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnThem.UseVisualStyleBackColor = false;
			this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
			// 
			// btnThemChuaCo
			// 
			this.btnThemChuaCo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnThemChuaCo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
			this.btnThemChuaCo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnThemChuaCo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnThemChuaCo.ForeColor = System.Drawing.Color.White;
			this.btnThemChuaCo.Image = global::QLBXMD.Properties.Resources.add;
			this.btnThemChuaCo.Location = new System.Drawing.Point(1114, 12);
			this.btnThemChuaCo.Name = "btnThemChuaCo";
			this.btnThemChuaCo.Size = new System.Drawing.Size(130, 50);
			this.btnThemChuaCo.TabIndex = 16;
			this.btnThemChuaCo.Text = "Thêm";
			this.btnThemChuaCo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnThemChuaCo.UseVisualStyleBackColor = false;
			this.btnThemChuaCo.Click += new System.EventHandler(this.btnThemChuaCo_Click);
			// 
			// UCGiaVe
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Name = "UCGiaVe";
			this.Size = new System.Drawing.Size(1352, 731);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabDaCo.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvDaCo)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.tabChuaCo.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvChuaCo)).EndInit();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabDaCo;
		private System.Windows.Forms.TabPage tabChuaCo;
		private System.Windows.Forms.DataGridView dgvDaCo;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ComboBox cbbLoai;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.DataGridView dgvChuaCo;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.ComboBox cbbLoaiChuaCo;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtSearchChuaCo;
		private System.Windows.Forms.Button btnXoa;
		private System.Windows.Forms.Button btnSua;
		private System.Windows.Forms.Button btnXem;
		private System.Windows.Forms.Button btnThem;
		private System.Windows.Forms.Button btnThemChuaCo;
	}
}
