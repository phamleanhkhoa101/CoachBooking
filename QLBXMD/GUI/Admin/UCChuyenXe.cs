using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.BUS;
using QLBXMD.EF;

namespace QLBXMD.GUI.Admin
{
	public partial class UCChuyenXe : UserControl
	{
		private readonly ChuyenXeBUS _bus = new ChuyenXeBUS();
		private List<dynamic> _data;

		public UCChuyenXe()
		{
			InitializeComponent();
			this.Load += UCChuyenXe_Load;
			this.dgv.SelectionChanged += Dgv_SelectionChanged;
		}

		private void Dgv_SelectionChanged(object sender, EventArgs e)
		{
			bool has = dgv.SelectedRows.Count > 0 && dgv.SelectedRows[0].Cells[0].Value != null;
			btnXem.Enabled = has; btnSua.Enabled = has; btnXoa.Enabled = has;
		}

		private void UCChuyenXe_Load(object sender, EventArgs e)
		{
			try
			{
				SetupGridHeaders();

				var list = _bus.GetAll() ?? new List<ChuyenXe>();
				_data = list
					.Select(cx => new
					{
						MaChuyen = cx.MaChuyen,
						XeName = cx.Xe != null ? cx.Xe.BienSoXe : (cx.MaXe != 0 ? $"Xe {cx.MaXe}" : null),
						TuyenName = cx.TuyenDuong != null ? ($"{cx.TuyenDuong.DiemDi} -> {cx.TuyenDuong.DiemDen}") : (cx.MaTuyen != 0 ? $"Tuyến {cx.MaTuyen}" : null),
						TaiXeName = cx.TaiXe != null ? cx.TaiXe.TenTX : (cx.MaTX != 0 ? $"TX {cx.MaTX}" : null),
						KhoiHanh = cx.NgayGioKhoiHanh.ToString("dd/MM/yyyy HH:mm"),
						GiaVe = cx.GiaVeThucTe?.ToString("N0"),
						MaXe = cx.MaXe,
						MaTuyen = cx.MaTuyen,
						MaTX = cx.MaTX,
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
			if (dgv.Columns.Count >= 6)
			{
				dgv.Columns[0].HeaderText = "Mã chuyến";
				dgv.Columns[1].HeaderText = "Xe";
				dgv.Columns[2].HeaderText = "Tuyến";
				dgv.Columns[3].HeaderText = "Tài xế";
				dgv.Columns[4].HeaderText = "Khởi hành";
				dgv.Columns[5].HeaderText = "Giá vé (đ)";
			}
		}

		private void InitCbb()
		{
			cbbLoai.Items.Clear();
			cbbLoai.Items.Add("Tất cả");
			cbbLoai.Items.Add("Mã chuyến");
			cbbLoai.Items.Add("Xe");
			cbbLoai.Items.Add("Tuyến");
			cbbLoai.Items.Add("Tài xế");
			cbbLoai.Items.Add("Khởi hành");
			cbbLoai.Items.Add("Giá vé");
			cbbLoai.SelectedIndex = 0;
		}

		private void LoadData(List<dynamic> list)
		{
			dgv.DataSource = null;
			dgv.Rows.Clear();

			foreach (var item in list)
			{
				dgv.Rows.Add(item.MaChuyen, item.XeName, item.TuyenName, item.TaiXeName, item.KhoiHanh, item.GiaVe);
			}

			//dgv.ClearSelection();
		}

		private void txtSearch_TextChanged(object sender, EventArgs e) => Filter();

		private void cbbLoai_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadData(_data);
			txtSearch.Clear();
		}

		private IEnumerable<KeyValuePair<int, string>> GetXeItems()
		{
			using (var db = new BXMDEntities())
			{
				var data = db.Xes.AsNoTracking()
								 .Select(x => new { x.MaXe, x.BienSoXe })
								 .ToList(); // SQL happens here
				return data.Select(x => new KeyValuePair<int, string>(x.MaXe, x.BienSoXe))
						   .ToList(); // KeyValuePair created in memory
			}
		}
		private IEnumerable<KeyValuePair<int, string>> GetTuyenItems()
		{
			using (var db = new BXMDEntities())
			{
				var data = db.TuyenDuongs.AsNoTracking()
                     .Select(t => new { t.MaTuyen, Text = t.DiemDi + " -> " + t.DiemDen })
                     .ToList();
                return data.Select(t => new KeyValuePair<int, string>(t.MaTuyen, t.Text))
                   .ToList();
			}
		}
		private IEnumerable<KeyValuePair<int, string>> GetTaiXeItems()
		{
			using (var db = new BXMDEntities())
			{
				var data = db.TaiXes.AsNoTracking()
                     .Select(tx => new { tx.MaTX, tx.TenTX })
                     .ToList();
                return data.Select(tx => new KeyValuePair<int, string>(tx.MaTX, tx.TenTX))
                   .ToList();
			}
		}

		private void btnThem_Click(object sender, EventArgs e)
		{
			using (var dialog = new ChuyenXeDialog())
			{
				dialog.BindXe(GetXeItems());
				dialog.BindTuyen(GetTuyenItems());
				dialog.BindTaiXe(GetTaiXeItems());
				dialog.SetMode(ChuyenXeDialogMode.Add);
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var cx = new ChuyenXe
					{
						MaXe = dialog.SelectedMaXe ?? 0,
						MaTuyen = dialog.SelectedMaTuyen ?? 0,
						MaTX = dialog.SelectedMaTX ?? 0,
						NgayGioKhoiHanh = dialog.KhoiHanhValue,
						NgayGioDenDuKien = dialog.DenDuKienValue,
						GiaVeThucTe = dialog.GiaVeValue
					};
					string result = _bus.Add(cx);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
					if (result.Contains("thành công")) UCChuyenXe_Load(null, null);
				}
			}
		}

		private void btnXem_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0) return;
			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null) return;
			int maChuyen = int.Parse(r.Cells[0].Value.ToString());
			var existing = _bus.GetById(maChuyen);
			if (existing == null) return;
			using (var dialog = new ChuyenXeDialog())
			{
				dialog.BindXe(GetXeItems());
				dialog.BindTuyen(GetTuyenItems());
				dialog.BindTaiXe(GetTaiXeItems());
				dialog.SetMode(ChuyenXeDialogMode.View);
				dialog.MaChuyen = existing.MaChuyen.ToString();
				dialog.SelectedMaXe = existing.MaXe;
				dialog.SelectedMaTuyen = existing.MaTuyen;
				dialog.SelectedMaTX = existing.MaTX;
				dialog.KhoiHanhValue = existing.NgayGioKhoiHanh;
				dialog.DenDuKienValue = existing.NgayGioDenDuKien;
				dialog.GiaVeValue = existing.GiaVeThucTe;
				dialog.ShowDialog();
			}
		}

		private void btnSua_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0) return;
			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null) return;
			int maChuyen = int.Parse(r.Cells[0].Value.ToString());
			var existing = _bus.GetById(maChuyen);
			if (existing == null)
			{
				MessageBox.Show("Không tìm thấy chuyến xe.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			using (var dialog = new ChuyenXeDialog())
			{
				dialog.BindXe(GetXeItems());
				dialog.BindTuyen(GetTuyenItems());
				dialog.BindTaiXe(GetTaiXeItems());
				dialog.SetMode(ChuyenXeDialogMode.Edit);
				dialog.MaChuyen = existing.MaChuyen.ToString();
				dialog.SelectedMaXe = existing.MaXe;
				dialog.SelectedMaTuyen = existing.MaTuyen;
				dialog.SelectedMaTX = existing.MaTX;
				dialog.KhoiHanhValue = existing.NgayGioKhoiHanh;
				dialog.DenDuKienValue = existing.NgayGioDenDuKien;
				dialog.GiaVeValue = existing.GiaVeThucTe;

				if (dialog.ShowDialog() == DialogResult.OK)
				{
					var update = new ChuyenXe
					{
						MaChuyen = existing.MaChuyen,
						MaXe = dialog.SelectedMaXe ?? 0,
						MaTuyen = dialog.SelectedMaTuyen ?? 0,
						MaTX = dialog.SelectedMaTX ?? 0,
						NgayGioKhoiHanh = dialog.KhoiHanhValue,
						NgayGioDenDuKien = dialog.DenDuKienValue,
						GiaVeThucTe = dialog.GiaVeValue
					};

					string result = _bus.Update(update);
					MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
						result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
					if (result.Contains("thành công")) UCChuyenXe_Load(null, null);
				}
			}
		}

		private void btnXoa_Click(object sender, EventArgs e)
		{
			if (dgv.SelectedRows.Count == 0)
			{
				MessageBox.Show("Vui lòng chọn chuyến xe cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			var r = dgv.SelectedRows[0];
			if (r.Cells[0].Value == null)
			{
				MessageBox.Show("Vui lòng chọn chuyến xe cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			int maChuyen = int.Parse(r.Cells[0].Value.ToString());
			
			// Lấy thông tin để hiển thị trong câu hỏi xác nhận
			string xeName = r.Cells[1].Value?.ToString() ?? "N/A";
			string tuyenName = r.Cells[2].Value?.ToString() ?? "N/A";
			
			// Hiển thị dialog xác nhận xóa
			var confirmResult = MessageBox.Show(
				$"Bạn có chắc chắn muốn xóa chuyến xe '{xeName} - {tuyenName}'?",
				"Xác nhận xóa",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Question
			);
			
			if (confirmResult == DialogResult.Yes)
			{
				string result = _bus.Delete(maChuyen);
				MessageBox.Show(result, "Thông báo", MessageBoxButtons.OK,
					result.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);
				if (result.Contains("thành công")) 
				{
					UCChuyenXe_Load(null, null);
				}
			}
		}

		private void Filter()
		{
			if (_data == null) return;
			string keyword = (txtSearch.Text ?? string.Empty).Trim().ToLowerInvariant();
			if (keyword.Length == 0) { LoadData(_data); return; }
			int selected = cbbLoai.SelectedIndex;
			List<dynamic> filtered = _data;
			switch (selected)
			{
				case 1: filtered = _data.Where(x => x.MaChuyen.ToString().ToLowerInvariant().Contains(keyword)).ToList(); break;
				case 2: filtered = _data.Where(x => (x.XeName ?? "").ToLowerInvariant().Contains(keyword)).ToList(); break;
				case 3: filtered = _data.Where(x => (x.TuyenName ?? "").ToLowerInvariant().Contains(keyword)).ToList(); break;
				case 4: filtered = _data.Where(x => (x.TaiXeName ?? "").ToLowerInvariant().Contains(keyword)).ToList(); break;
				case 5: filtered = _data.Where(x => (x.KhoiHanh ?? "").ToLowerInvariant().Contains(keyword)).ToList(); break;
				case 6: filtered = _data.Where(x => (x.GiaVe ?? "").ToLowerInvariant().Contains(keyword)).ToList(); break;
				default:
					filtered = _data.Where(x => new[] { x.MaChuyen.ToString(), x.XeName, x.TuyenName, x.TaiXeName, x.KhoiHanh, x.GiaVe }
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

		private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}
	}
}
