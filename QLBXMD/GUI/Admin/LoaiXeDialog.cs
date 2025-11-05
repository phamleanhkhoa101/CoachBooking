using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
	public partial class LoaiXeDialog : Form
	{
		private Button btnHuy;
		private Button btnThem;
		private ComboBox cbbTrangThai;
		private Label label5;
		private Label label2;
		private TextBox txtTenLoaiXe;
		private Label label1;
		private Label label3;
		private TextBox txtSoCho;
		private TextBox txtMaLoaiXe;

		public string MaLoaiXe
		{
			get => txtMaLoaiXe.Text.Trim();
			set => txtMaLoaiXe.Text = value;
		}
		
		public string TenLoaiXe
		{
			get => txtTenLoaiXe.Text.Trim();
			set => txtTenLoaiXe.Text = value;
		}
		
		public string SoCho
		{
			get => txtSoCho.Text.Trim();
			set => txtSoCho.Text = value;
		}
		public string TrangThai
		{
			get => cbbTrangThai.SelectedItem?.ToString();
			set => cbbTrangThai.SelectedItem = value;
		}
		
		public LoaiXeDialog()
		{
			InitializeComponent();
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			// Validate dữ liệu trước khi đóng dialog
			if (string.IsNullOrWhiteSpace(TenLoaiXe))
			{
				MessageBox.Show("Vui lòng nhập tên loại xe.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtTenLoaiXe.Focus();
				return;
			}

			if (string.IsNullOrWhiteSpace(SoCho))
			{
				MessageBox.Show("Vui lòng nhập số chỗ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtSoCho.Focus();
				return;
			}

			if (!int.TryParse(SoCho, out int SoChoInt) || SoChoInt <=0)
			{
				MessageBox.Show("Số chỗ phải là số dương.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtSoCho.Focus();
				return;
			}
			if (cbbTrangThai.SelectedItem == null)
			{
				MessageBox.Show("Vui lòng chọn trạng thái.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				cbbTrangThai.Focus();
				return;
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnHuy_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void LoaiXeDialog_Load(object sender, EventArgs e)
		{
			// Khởi tạo combobox trạng thái nếu chưa có
			if (cbbTrangThai.Items.Count == 0)
			{
				cbbTrangThai.Items.Add("Hoạt động");
				cbbTrangThai.Items.Add("Ngừng hoạt động");
			}
		}

		private void InitializeComponent()
		{
			this.btnHuy = new System.Windows.Forms.Button();
			this.btnThem = new System.Windows.Forms.Button();
			this.cbbTrangThai = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtTenLoaiXe = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtMaLoaiXe = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtSoCho = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnHuy
			// 
			this.btnHuy.Location = new System.Drawing.Point(438, 341);
			this.btnHuy.Name = "btnHuy";
			this.btnHuy.Size = new System.Drawing.Size(103, 44);
			this.btnHuy.TabIndex = 22;
			this.btnHuy.Text = "Hủy";
			this.btnHuy.UseVisualStyleBackColor = true;
			this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
			// 
			// btnThem
			// 
			this.btnThem.Location = new System.Drawing.Point(244, 341);
			this.btnThem.Name = "btnThem";
			this.btnThem.Size = new System.Drawing.Size(103, 44);
			this.btnThem.TabIndex = 21;
			this.btnThem.Text = "Thêm";
			this.btnThem.UseVisualStyleBackColor = true;
			this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
			// 
			// cbbTrangThai
			// 
			this.cbbTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbbTrangThai.FormattingEnabled = true;
			this.cbbTrangThai.Items.AddRange(new object[] {
            "Hoạt động",
            "Ngừng hoạt động"});
			this.cbbTrangThai.Location = new System.Drawing.Point(181, 246);
			this.cbbTrangThai.Name = "cbbTrangThai";
			this.cbbTrangThai.Size = new System.Drawing.Size(176, 28);
			this.cbbTrangThai.TabIndex = 20;
			this.cbbTrangThai.SelectedIndexChanged += new System.EventHandler(this.cbbTrangThai_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(71, 246);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 20);
			this.label5.TabIndex = 19;
			this.label5.Text = "Trạng thái";
			this.label5.Click += new System.EventHandler(this.label5_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(71, 121);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 20);
			this.label2.TabIndex = 14;
			this.label2.Text = "Tên loại xe";
			this.label2.Click += new System.EventHandler(this.label2_Click);
			// 
			// txtTenLoaiXe
			// 
			this.txtTenLoaiXe.Location = new System.Drawing.Point(181, 118);
			this.txtTenLoaiXe.Name = "txtTenLoaiXe";
			this.txtTenLoaiXe.Size = new System.Drawing.Size(447, 26);
			this.txtTenLoaiXe.TabIndex = 13;
			this.txtTenLoaiXe.TextChanged += new System.EventHandler(this.txtTenLoaiXe_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(71, 57);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 20);
			this.label1.TabIndex = 12;
			this.label1.Text = "Mã loại xe";
			this.label1.Click += new System.EventHandler(this.label1_Click);
			// 
			// txtMaLoaiXe
			// 
			this.txtMaLoaiXe.Enabled = false;
			this.txtMaLoaiXe.Location = new System.Drawing.Point(181, 54);
			this.txtMaLoaiXe.Name = "txtMaLoaiXe";
			this.txtMaLoaiXe.Size = new System.Drawing.Size(447, 26);
			this.txtMaLoaiXe.TabIndex = 11;
			this.txtMaLoaiXe.TextChanged += new System.EventHandler(this.txtMaLoaiXe_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(71, 183);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(59, 20);
			this.label3.TabIndex = 24;
			this.label3.Text = "Số chỗ";
			this.label3.Click += new System.EventHandler(this.label3_Click);
			// 
			// txtSoCho
			// 
			this.txtSoCho.Location = new System.Drawing.Point(181, 180);
			this.txtSoCho.Name = "txtSoCho";
			this.txtSoCho.Size = new System.Drawing.Size(176, 26);
			this.txtSoCho.TabIndex = 23;
			this.txtSoCho.TextChanged += new System.EventHandler(this.txtSoCho_TextChanged);
			this.txtSoCho.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSoCho_KeyPress);
			// 
			// LoaiXeDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(698, 494);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtSoCho);
			this.Controls.Add(this.btnHuy);
			this.Controls.Add(this.btnThem);
			this.Controls.Add(this.cbbTrangThai);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtTenLoaiXe);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtMaLoaiXe);
			this.Name = "LoaiXeDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void txtSoCho_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void txtSoCho_TextChanged(object sender, EventArgs e)
		{

		}

		private void label3_Click(object sender, EventArgs e)
		{

		}

		private void cbbTrangThai_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void label5_Click(object sender, EventArgs e)
		{

		}

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void txtTenLoaiXe_TextChanged(object sender, EventArgs e)
		{

		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void txtMaLoaiXe_TextChanged(object sender, EventArgs e)
		{

		}
	}
}