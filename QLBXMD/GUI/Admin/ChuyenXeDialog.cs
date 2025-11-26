using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.EF;

// Encoding: UTF-8 (fixed)

namespace QLBXMD.GUI.Admin
{
	public enum ChuyenXeDialogMode { Add, View, Edit, Delete }

	public partial class ChuyenXeDialog : Form
	{
		public ChuyenXeDialogMode Mode { get; private set; } = ChuyenXeDialogMode.Add;

		public string MaChuyen { get => txtMaChuyen.Text; set => txtMaChuyen.Text = value; }
		public DateTime KhoiHanhValue { get => dtpKhoiHanh.Value; set => dtpKhoiHanh.Value = value; }
		public DateTime? DenDuKienValue
		{
			get => dtpDenDuKien.Checked ? dtpDenDuKien.Value : (DateTime?)null;
			set
			{
				if (value.HasValue)
				{
					dtpDenDuKien.Checked = true;
					dtpDenDuKien.Value = value.Value;
				}
				else
				{
					dtpDenDuKien.Checked = false;
				}
			}
		}
		public decimal? GiaVeValue { get => numGiaVe.Value > 0 ? numGiaVe.Value : (decimal?)null; set => numGiaVe.Value = value ?? 0; }

		public int? SelectedMaXe { get { return cbbXe.SelectedValue is int v ? (int?)v : (cbbXe.SelectedValue as int?); } set { if (value.HasValue) cbbXe.SelectedValue = value.Value; else cbbXe.SelectedIndex = -1; } }
		public int? SelectedMaTuyen { get { return cbbTuyen.SelectedValue is int v ? (int?)v : (cbbTuyen.SelectedValue as int?); } set { if (value.HasValue) cbbTuyen.SelectedValue = value.Value; else cbbTuyen.SelectedIndex = -1; } }
		public int? SelectedMaTX { get { return cbbTaiXe.SelectedValue is int v ? (int?)v : (cbbTaiXe.SelectedValue as int?); } set { if (value.HasValue) cbbTaiXe.SelectedValue = value.Value; else cbbTaiXe.SelectedIndex = -1; } }

		public ChuyenXeDialog()
		{
			InitializeComponent();
			
			// Thêm sự kiện khi chọn xe hoặc tuyến
			cbbXe.SelectedIndexChanged += CbbXeOrTuyen_SelectedIndexChanged;
			cbbTuyen.SelectedIndexChanged += CbbXeOrTuyen_SelectedIndexChanged;
			// Khi đổi tài xế cần kiểm tra xung đột
			cbbTaiXe.SelectedIndexChanged += (s, e) => { if (Mode == ChuyenXeDialogMode.Add || Mode == ChuyenXeDialogMode.Edit) CheckTripConflict(); };
			dtpKhoiHanh.ValueChanged += DtpKhoiHanh_ValueChanged;
			
			// Mặc định đến dự kiến không được chọn
			dtpDenDuKien.Checked = false;
		}

		private void CbbXeOrTuyen_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Chỉ tự động lấy giá vé khi đang ở chế độ Add hoặc Edit
			if (Mode != ChuyenXeDialogMode.Add && Mode != ChuyenXeDialogMode.Edit)
				return;

			AutoLoadGiaVe();
			CheckTripConflict();
		}

		private void DtpKhoiHanh_ValueChanged(object sender, EventArgs e)
		{
			if (Mode != ChuyenXeDialogMode.Add && Mode != ChuyenXeDialogMode.Edit)
				return;

			CheckTripConflict();
		}

		private void AutoLoadGiaVe()
		{
			if (SelectedMaXe == null || SelectedMaTuyen == null)
			{
				lblGiaVeInfo.Text = "";
				return;
			}

			try
			{
				using (var db = new BXMDEntities())
				{
					// Lấy thông tin xe để biết MaNX và MaLX
					var xe = db.Xes.AsNoTracking()
						.FirstOrDefault(x => x.MaXe == SelectedMaXe.Value);
					
					if (xe == null || xe.MaNX == null)
					{
						lblGiaVeInfo.Text = "⚠ Không tìm thấy thông tin xe";
						lblGiaVeInfo.ForeColor = Color.Red;
						return;
					}

					// Tìm giá vé trong bảng GiaVe
					var giaVe = db.GiaVes.AsNoTracking()
						.FirstOrDefault(gv => 
							gv.MaNX == xe.MaNX && 
							gv.MaLX == xe.MaLX && 
							gv.MaTuyen == SelectedMaTuyen.Value);

					if (giaVe != null)
					{
						numGiaVe.Value = giaVe.GiaVe1;
						lblGiaVeInfo.Text = "✓ Giá vé được lấy từ bảng giá";
						lblGiaVeInfo.ForeColor = Color.Green;
					}
					else
					{
						lblGiaVeInfo.Text = "⚠ Chưa có giá vé cho tuyến này";
						lblGiaVeInfo.ForeColor = Color.Orange;
					}
				}
			}
			catch (Exception ex)
			{
				lblGiaVeInfo.Text = "✗ Lỗi: " + ex.Message;
				lblGiaVeInfo.ForeColor = Color.Red;
			}
		}

		private void CheckTripConflict()
		{
			if (SelectedMaXe == null && SelectedMaTX == null && SelectedMaTuyen == null)
			{
				lblConflictInfo.Text = "";
				btnPrimary.Enabled = true;
				return;
			}

			try
			{
				using (var db = new BXMDEntities())
				{
					// First, validate selected entities are active
					if (SelectedMaXe != null)
					{
						var xe = db.Xes.AsNoTracking().FirstOrDefault(x => x.MaXe == SelectedMaXe.Value);
						if (xe == null || xe.TrangThai != true)
						{
							lblConflictInfo.Text = "⚠ Xe không tồn tại hoặc không ở trạng thái hoạt động.";
							lblConflictInfo.ForeColor = Color.Red;
							btnPrimary.Enabled = false;
							return;
						}
					}

					if (SelectedMaTX != null)
					{
						var tx = db.TaiXes.AsNoTracking().FirstOrDefault(t => t.MaTX == SelectedMaTX.Value);
						if (tx == null || tx.TrangThai != true)
						{
							lblConflictInfo.Text = "⚠ Tài xế không tồn tại hoặc không ở trạng thái hoạt động.";
							lblConflictInfo.ForeColor = Color.Red;
							btnPrimary.Enabled = false;
							return;
						}
					}

					if (SelectedMaTuyen != null)
					{
						var tuyen = db.TuyenDuongs.AsNoTracking().FirstOrDefault(t => t.MaTuyen == SelectedMaTuyen.Value);
						if (tuyen == null || tuyen.TrangThai != true)
						{
							lblConflictInfo.Text = "⚠ Tuyến đường không tồn tại hoặc không ở trạng thái hoạt động.";
							lblConflictInfo.ForeColor = Color.Red;
							btnPrimary.Enabled = false;
							return;
						}
					}

					var newStart = dtpKhoiHanh.Value;
					var newEnd = DenDuKienValue ?? newStart.AddHours(12); // Mặc định 12h nếu không có đến dự kiến

					// Nếu đang edit, loại trừ chính chuyến hiện tại khỏi kiểm tra
					int? currentMaChuyen = null;
					if (int.TryParse(txtMaChuyen.Text, out int tmp)) currentMaChuyen = tmp;

					// Kiểm tra xung đột cho XE
					if (SelectedMaXe != null)
					{
						var conflictTrip = db.ChuyenXes.AsNoTracking()
							.Where(c => c.MaXe == SelectedMaXe.Value && (currentMaChuyen == null || c.MaChuyen != currentMaChuyen.Value))
							.ToList()
							.FirstOrDefault(c => 
							{
								var existStart = c.NgayGioKhoiHanh;
								var existEnd = c.NgayGioDenDuKien ?? existStart.AddHours(12);
								return newStart < existEnd && newEnd > existStart;
							});

						if (conflictTrip != null)
						{
							var khoiHanh = conflictTrip.NgayGioKhoiHanh.ToString("HH:mm dd/MM/yyyy");
							var denDuKien = conflictTrip.NgayGioDenDuKien?.ToString("HH:mm dd/MM/yyyy") ?? "Chưa xác định";
							lblConflictInfo.Text = $"⚠ Xe đã có chuyến: {khoiHanh} - {denDuKien}";
							lblConflictInfo.ForeColor = Color.Red;
							btnPrimary.Enabled = false;
							return;
						}
					}

					// Kiểm tra xung đột cho TÀI XẾ
					if (SelectedMaTX != null)
					{
						var conflictTripTx = db.ChuyenXes.AsNoTracking()
							.Where(c => c.MaTX == SelectedMaTX.Value && (currentMaChuyen == null || c.MaChuyen != currentMaChuyen.Value))
							.ToList()
							.FirstOrDefault(c => 
							{
								var existStart = c.NgayGioKhoiHanh;
								var existEnd = c.NgayGioDenDuKien ?? existStart.AddHours(12);
								return newStart < existEnd && newEnd > existStart;
							});

						if (conflictTripTx != null)
						{
							string tenTx = "Tài xế";
							try { tenTx = conflictTripTx.TaiXe != null ? conflictTripTx.TaiXe.TenTX : tenTx; } catch { }
							var khoiHanh = conflictTripTx.NgayGioKhoiHanh.ToString("HH:mm dd/MM/yyyy");
							var denDuKien = conflictTripTx.NgayGioDenDuKien?.ToString("HH:mm dd/MM/yyyy") ?? "Chưa xác định";
							lblConflictInfo.Text = $"⚠ {tenTx} đã có chuyến: {khoiHanh} - {denDuKien}";
							lblConflictInfo.ForeColor = Color.Red;
							btnPrimary.Enabled = false;
							return;
						}
					}

					// Nếu không phát hiện xung đột
					lblConflictInfo.Text = "";
					btnPrimary.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				lblConflictInfo.Text = "✗ Lỗi kiểm tra: " + ex.Message;
				lblConflictInfo.ForeColor = Color.Red;
				btnPrimary.Enabled = true;
			}
		}

		public void BindXe(IEnumerable<KeyValuePair<int, string>> items)
		{
			cbbXe.DataSource = items?.ToList();
			cbbXe.DisplayMember = "Value";
			cbbXe.ValueMember = "Key";
		}

		public void BindTuyen(IEnumerable<KeyValuePair<int, string>> items)
		{
			cbbTuyen.DataSource = items?.ToList();
			cbbTuyen.DisplayMember = "Value";
			cbbTuyen.ValueMember = "Key";
		}

		public void BindTaiXe(IEnumerable<KeyValuePair<int, string>> items)
		{
			cbbTaiXe.DataSource = items?.ToList();
			cbbTaiXe.DisplayMember = "Value";
			cbbTaiXe.ValueMember = "Key";
		}

		public void SetMode(ChuyenXeDialogMode mode)
		{
			Mode = mode;
			switch (mode)
			{
				case ChuyenXeDialogMode.Add:
					Text = "Thêm chuyến xe";
					btnPrimary.Text = "Thêm";
					txtMaChuyen.Enabled = false;
					SetReadOnly(false);
					lblGiaVeInfo.Visible = true;
					lblConflictInfo.Visible = true;
					AutoLoadGiaVe();
					break;
				case ChuyenXeDialogMode.Edit:
					Text = "Sửa chuyến xe";
					btnPrimary.Text = "Lưu";
					txtMaChuyen.Enabled = false;
					SetReadOnly(false);
					lblGiaVeInfo.Visible = true;
					lblConflictInfo.Visible = true;
					break;
				case ChuyenXeDialogMode.View:
					Text = "Xem chuyến xe";
					btnPrimary.Visible = false;
					btnCancel.Text = "Đóng";
					SetReadOnly(false);
					lblGiaVeInfo.Visible = false;
					lblConflictInfo.Visible = false;
					break;
				case ChuyenXeDialogMode.Delete:
					Text = "Xóa chuyến xe";
					btnPrimary.Text = "Xóa";
					txtMaChuyen.Enabled = false;
					SetReadOnly(true);
					btnPrimary.Visible = true;
					lblGiaVeInfo.Visible = false;
					lblConflictInfo.Visible = false;
					break;
			}
		}

		private void SetReadOnly(bool ro)
		{
			// Disable/enable all inputs consistently for view/edit modes
			txtMaChuyen.Enabled = !ro;
			cbbXe.Enabled = !ro;
			cbbTuyen.Enabled = !ro;
			cbbTaiXe.Enabled = !ro;
			dtpKhoiHanh.Enabled = !ro;
			dtpDenDuKien.Enabled = !ro;
			numGiaVe.Enabled = !ro;

			// Also remove from tab navigation when read-only
			txtMaChuyen.TabStop = !ro;
			cbbXe.TabStop = !ro;
			cbbTuyen.TabStop = !ro;
			cbbTaiXe.TabStop = !ro;
			dtpKhoiHanh.TabStop = !ro;
			dtpDenDuKien.TabStop = !ro;
			numGiaVe.TabStop = !ro;
		}

		private void btnPrimary_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

	}
}
