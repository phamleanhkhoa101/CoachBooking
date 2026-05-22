using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.BUS;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	public partial class UCTaiXe : UserControl
	{
		private readonly TaiXeBUS _bus = new TaiXeBUS();
		private List<dynamic> _data;

		public UCTaiXe()
		{
			InitializeComponent();
			this.Load += UCTaiXe_Load;
			this.dgv.SelectionChanged += Dgv_SelectionChanged;
		}

		private void Dgv_SelectionChanged(object sender, EventArgs e)
		{
			bool has = dgv.SelectedRows.Count > 0 && dgv.SelectedRows[0].Cells[0].Value != null;
			btnXem.Enabled = has; 
			btnSua.Enabled = has; 
			btnXoa.Enabled = has;
		}

		private void UCTaiXe_Load(object sender, EventArgs e)
		{
			try
			{
				SetupGridHeaders();

				var list = _bus.GetAll() ?? new List<TaiXe>();
				_data = list
					.Select(tx => new
					{
						MaTX = tx.MaTX,
						TenTX = tx.TenTX,
						NamSinh = tx.NamSinh.ToString("dd/MM/yyyy"),
						CCCD = tx.CCCD,
						GioiTinh = tx.GioiTinh == true ? "Nam" : "Nữ",
						SDT = tx.SDT,
						DiaChi = tx.DiaChi,
						BangLai = tx.BangLai,
						NgayCap = tx.NgayCap.ToString("dd/MM/yyyy"),
						TrangThai = tx.TrangThai == true ? "Hoạt động" : "Ngừng hoạt động",
						NamSinhDate = tx.NamSinh,
						NgayCapDate = tx.NgayCap,
						GioiTinhValue = tx.GioiTinh,
						TrangThaiValue = tx.TrangThai
					})
					.ToList<dynamic>();

				LoadData(_data);
				InitCbb();
				btnXem.Enabled = btnSua.Enabled = btnXoa.Enabled = false;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Tải dữ liệu thất bại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void SetupGridHeaders()
		{
			dgv.Columns.Clear();
			dgv.Columns.Add("MaTX", "Mã tài xế");
			dgv.Columns.Add("TenTX", "Tên tài xế");
			dgv.Columns.Add("NamSinh", "Năm sinh");
			dgv.Columns.Add("CCCD", "CCCD");
			dgv.Columns.Add("GioiTinh", "Giới tính");
			dgv.Columns.Add("SDT", "Số điện thoại");
			dgv.Columns.Add("DiaChi", "Địa chỉ");
			dgv.Columns.Add("BangLai", "Bằng lái");
			dgv.Columns.Add("NgayCap", "Ngày cấp");
			dgv.Columns.Add("TrangThai", "Trạng thái");
		}

		private void InitCbb()
		{
			cbbLoai.Items.Clear();
			cbbLoai.Items.Add("Tất cả");
			cbbLoai.Items.Add("Mã tài xế");
			cbbLoai.Items.Add("Tên tài xế");
			cbbLoai.Items.Add("Năm sinh");
			cbbLoai.Items.Add("CCCD");
			cbbLoai.Items.Add("Giới tính");
			cbbLoai.Items.Add("Số điện thoại");
			cbbLoai.Items.Add("Địa chỉ");
			cbbLoai.Items.Add("Bằng lái");
			cbbLoai.Items.Add("Ngày cấp");
			cbbLoai.Items.Add("Trạng thái");
			cbbLoai.SelectedIndex = 0;
			cbbLoai.SelectedIndexChanged += cbbLoai_SelectedIndexChanged;
		}

		private void LoadData(List<dynamic> list)
		{
			dgv.Rows.Clear();

			foreach (var item in list)
			{
				dgv.Rows.Add(
					item.MaTX, 
					item.TenTX, 
					item.NamSinh, 
					item.CCCD, 
					item.GioiTinh, 
					item.SDT, 
					item.DiaChi, 
					item.BangLai, 
					item.NgayCap, 
					item.TrangThai
				);
			}

			dgv.ClearSelection();
		}

		private void txtSearch_TextChanged(object sender, EventArgs e)
		{
			Filter();
		}

		private void cbbLoai_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadData(_data);
			txtSearch.Clear();
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			using (var dialog = new TaiXeDialog())
			{
				dialog.SetMode(TaiXeDialogMode.Add);
				
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var tx = new TaiXe
					{
						TenTX = dialog.TenTX,
						NamSinh = dialog.NamSinhValue,
						CCCD = dialog.CCCD,
						GioiTinh = dialog.GioiTinhValue,
						SDT = dialog.SDT,
						DiaChi = dialog.DiaChi,
						BangLai = dialog.BangLai,
						NgayCap = dialog.NgayCapValue,
						TrangThai = dialog.TrangThaiValue
					};
					
					string result = _bus.Add(tx);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
					
					if (result.Contains("thành công")) 
						UCTaiXe_Load(null, null);
				}
			}
		}

		private void btnXem_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0) return;
			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null) return;
			
			int maTX = int.Parse(r.Cells[0].Value.ToString());
			var existing = _bus.GetById(maTX);
			if (existing == null) return;
			
			using (var dialog = new TaiXeDialog())
			{
				dialog.SetMode(TaiXeDialogMode.View);
				dialog.MaTX = existing.MaTX.ToString();
				dialog.TenTX = existing.TenTX;
				dialog.NamSinhValue = existing.NamSinh;
				dialog.CCCD = existing.CCCD;
				dialog.GioiTinhValue = existing.GioiTinh;
				dialog.SDT = existing.SDT;
				dialog.DiaChi = existing.DiaChi;
				dialog.BangLai = existing.BangLai;
				dialog.NgayCapValue = existing.NgayCap;
				dialog.TrangThaiValue = existing.TrangThai;
				dialog.ShowDialog();
			}
		}

		private void btnSua_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0) return;
			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null) return;
			
			int maTX = int.Parse(r.Cells[0].Value.ToString());
			var existing = _bus.GetById(maTX);
			if (existing == null)
			{
				MessageBox.Show("Không tìm thấy tài xế.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			using (var dialog = new TaiXeDialog())
			{
				dialog.SetMode(TaiXeDialogMode.Edit);
				dialog.MaTX = existing.MaTX.ToString();
				dialog.TenTX = existing.TenTX;
				dialog.NamSinhValue = existing.NamSinh;
				dialog.CCCD = existing.CCCD;
				dialog.GioiTinhValue = existing.GioiTinh;
				dialog.SDT = existing.SDT;
				dialog.DiaChi = existing.DiaChi;
				dialog.BangLai = existing.BangLai;
				dialog.NgayCapValue = existing.NgayCap;
				dialog.TrangThaiValue = existing.TrangThai;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var update = new TaiXe
					{
						MaTX = existing.MaTX,
						TenTX = dialog.TenTX,
						NamSinh = dialog.NamSinhValue,
						CCCD = dialog.CCCD,
						GioiTinh = dialog.GioiTinhValue,
						SDT = dialog.SDT,
						DiaChi = dialog.DiaChi,
						BangLai = dialog.BangLai,
						NgayCap = dialog.NgayCapValue,
						TrangThai = dialog.TrangThaiValue
					};

					string result = _bus.Update(update);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
					
					if (result.Contains("thành công")) 
						UCTaiXe_Load(null, null);
				}
			}
		}

		private void btnXoa_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0)
			{
				MessageBox.Show("Vui lòng chọn tài xế cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			
			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null)
			{
				MessageBox.Show("Vui lòng chọn tài xế cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			
			int maTX = int.Parse(r.Cells[0].Value.ToString());
			
			// Lấy thông tin để hiển thị trong câu hỏi xác nhận
			string tenTX = r.Cells[1].Value?.ToString() ?? "N/A";
			string cccd = r.Cells[3].Value?.ToString() ?? "N/A";
			string sdt = r.Cells[5].Value?.ToString() ?? "N/A";
			
			// Hiển thị dialog xác nhận xóa
			var confirmResult = MessageBox.Show(
				$"Bạn có chắc chắn muốn xóa tài xế?\n\n" +
				$"Tên: {tenTX}\n" +
				$"CCCD: {cccd}\n" +
				$"SĐT: {sdt}",
				"Xác nhận xóa",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);
			
			if (confirmResult == DialogResult.Yes)
			{
				string result = _bus.Delete(maTX);
				MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
					result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
				
				if (result.Contains("thành công")) 
				{
					UCTaiXe_Load(null, null);
				}
			}
		}

		private void Filter()
		{
			if (_data == null) return;
			
			string keyword = (txtSearch.Text ?? string.Empty).Trim().ToLowerInvariant();
			if (keyword.Length == 0) 
			{ 
				LoadData(_data); 
				return; 
			}
			
			int selected = cbbLoai.SelectedIndex;
			List<dynamic> filtered = _data;
			
			switch (selected)
			{
				case 1: // Mã tài xế
					filtered = _data.Where(x => x.MaTX.ToString().ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 2: // Tên tài xế
					filtered = _data.Where(x => (x.TenTX ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 3: // Năm sinh
					filtered = _data.Where(x => (x.NamSinh ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 4: // CCCD
					filtered = _data.Where(x => (x.CCCD ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 5: // Giới tính
					filtered = _data.Where(x => (x.GioiTinh ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 6: // Số điện thoại
					filtered = _data.Where(x => (x.SDT ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 7: // Địa chỉ
					filtered = _data.Where(x => (x.DiaChi ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 8: // Bằng lái
					filtered = _data.Where(x => (x.BangLai ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 9: // Ngày cấp
					filtered = _data.Where(x => (x.NgayCap ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				case 10: // Trạng thái
					filtered = _data.Where(x => (x.TrangThai ?? "").ToLowerInvariant().Contains(keyword)).ToList(); 
					break;
				default: // Tất cả
					filtered = _data.Where(x => new[] 
					{ 
						x.MaTX.ToString(), 
						x.TenTX, 
						x.NamSinh,
						x.CCCD, 
						x.GioiTinh,
						x.SDT, 
						x.DiaChi,
						x.BangLai, 
						x.NgayCap,
						x.TrangThai 
					}
					.Any(v => (v ?? string.Empty).ToLowerInvariant().Contains(keyword))).ToList();
					break;
			}
			
			LoadData(filtered);
		}

		private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				EnableBtn(false);
				return;
			}
			
			if (dgv.Rows[e.RowIndex].Cells[0].Value == null)
			{
				EnableBtn(false);
				return;
			}
			
			dgv.Rows[e.RowIndex].Selected = true;
			
			if (dgv.Rows[e.RowIndex].Selected && btnSua.Enabled == false)
			{
				EnableBtn(true);
			}
		}

		private void EnableBtn(bool enabled)
		{
			btnXem.Enabled = enabled;
			btnSua.Enabled = enabled;
			btnXoa.Enabled = enabled;
		}
	}
}
