using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.EF;

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

		public decimal? GiaVeValue
		{
			get => numGiaVe.Value > 0 ? numGiaVe.Value : (decimal?)null;
			set => numGiaVe.Value = value ?? 0;
		}

		public int? SelectedMaXe
		{
			get { return cbbXe.SelectedValue is int v ? (int?)v : (cbbXe.SelectedValue as int?); }
			set { if (value.HasValue) cbbXe.SelectedValue = value.Value; else cbbXe.SelectedIndex = -1; }
		}

		public int? SelectedMaTuyen
		{
			get { return cbbTuyen.SelectedValue is int v ? (int?)v : (cbbTuyen.SelectedValue as int?); }
			set { if (value.HasValue) cbbTuyen.SelectedValue = value.Value; else cbbTuyen.SelectedIndex = -1; }
		}

		public int? SelectedMaTX
		{
			get { return cbbTaiXe.SelectedValue is int v ? (int?)v : (cbbTaiXe.SelectedValue as int?); }
			set { if (value.HasValue) cbbTaiXe.SelectedValue = value.Value; else cbbTaiXe.SelectedIndex = -1; }
		}

		public ChuyenXeDialog()
		{
			InitializeComponent();

			cbbXe.SelectedIndexChanged += CbbXeOrTuyen_SelectedIndexChanged;
			cbbTuyen.SelectedIndexChanged += CbbXeOrTuyen_SelectedIndexChanged;
			cbbTaiXe.SelectedIndexChanged += (s, e) =>
			{
				if (Mode == ChuyenXeDialogMode.Add || Mode == ChuyenXeDialogMode.Edit)
				{
					CheckTripConflict();
				}
			};
			dtpKhoiHanh.ValueChanged += DtpKhoiHanh_ValueChanged;
			dtpDenDuKien.Checked = false;
		}

		private void CbbXeOrTuyen_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (Mode != ChuyenXeDialogMode.Add && Mode != ChuyenXeDialogMode.Edit)
			{
				return;
			}

			AutoLoadGiaVe();
			CheckTripConflict();
		}

		private void DtpKhoiHanh_ValueChanged(object sender, EventArgs e)
		{
			if (Mode != ChuyenXeDialogMode.Add && Mode != ChuyenXeDialogMode.Edit)
			{
				return;
			}

			CheckTripConflict();
		}

		private void AutoLoadGiaVe()
		{
			if (SelectedMaXe == null || SelectedMaTuyen == null)
			{
				lblGiaVeInfo.Text = string.Empty;
				return;
			}

			try
			{
				using (var db = new BXMDEntities())
				{
					var xe = db.Xes.AsNoTracking()
						.FirstOrDefault(x => x.MaXe == SelectedMaXe.Value);

					if (xe == null || xe.MaNX == null)
					{
						lblGiaVeInfo.Text = "Kh\u00f4ng t\u00ecm th\u1ea5y th\u00f4ng tin xe";
						lblGiaVeInfo.ForeColor = Color.Red;
						return;
					}

					var giaVe = db.GiaVes.AsNoTracking()
						.FirstOrDefault(gv =>
							gv.MaNX == xe.MaNX &&
							gv.MaLX == xe.MaLX &&
							gv.MaTuyen == SelectedMaTuyen.Value);

					if (giaVe != null)
					{
						numGiaVe.Value = giaVe.GiaVe1;
						lblGiaVeInfo.Text = "Gi\u00e1 v\u00e9 \u0111\u01b0\u1ee3c l\u1ea5y t\u1eeb b\u1ea3ng gi\u00e1";
						lblGiaVeInfo.ForeColor = Color.Green;
					}
					else
					{
						lblGiaVeInfo.Text = "Ch\u01b0a c\u00f3 gi\u00e1 v\u00e9 cho tuy\u1ebfn n\u00e0y";
						lblGiaVeInfo.ForeColor = Color.Orange;
					}
				}
			}
			catch (Exception ex)
			{
				lblGiaVeInfo.Text = "L\u1ed7i: " + ex.Message;
				lblGiaVeInfo.ForeColor = Color.Red;
			}
		}

		private void CheckTripConflict()
		{
			if (SelectedMaXe == null && SelectedMaTX == null && SelectedMaTuyen == null)
			{
				lblConflictInfo.Text = string.Empty;
				btnPrimary.Enabled = true;
				return;
			}

			try
			{
				using (var db = new BXMDEntities())
				{
					if (SelectedMaXe != null)
					{
						var xe = db.Xes.AsNoTracking().FirstOrDefault(x => x.MaXe == SelectedMaXe.Value);
						if (xe == null || xe.TrangThai != true)
						{
							lblConflictInfo.Text = "Xe kh\u00f4ng t\u1ed3n t\u1ea1i ho\u1eb7c kh\u00f4ng \u1edf tr\u1ea1ng th\u00e1i ho\u1ea1t \u0111\u1ed9ng.";
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
							lblConflictInfo.Text = "T\u00e0i x\u1ebf kh\u00f4ng t\u1ed3n t\u1ea1i ho\u1eb7c kh\u00f4ng \u1edf tr\u1ea1ng th\u00e1i ho\u1ea1t \u0111\u1ed9ng.";
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
							lblConflictInfo.Text = "Tuy\u1ebfn \u0111\u01b0\u1eddng kh\u00f4ng t\u1ed3n t\u1ea1i ho\u1eb7c kh\u00f4ng \u1edf tr\u1ea1ng th\u00e1i ho\u1ea1t \u0111\u1ed9ng.";
							lblConflictInfo.ForeColor = Color.Red;
							btnPrimary.Enabled = false;
							return;
						}
					}

					var newStart = dtpKhoiHanh.Value;
					var newEnd = DenDuKienValue ?? newStart.AddHours(12);

					int? currentMaChuyen = null;
					if (int.TryParse(txtMaChuyen.Text, out int tmp))
					{
						currentMaChuyen = tmp;
					}

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
							var denDuKien = conflictTrip.NgayGioDenDuKien?.ToString("HH:mm dd/MM/yyyy") ?? "Ch\u01b0a x\u00e1c \u0111\u1ecbnh";
							lblConflictInfo.Text = $"Xe \u0111\u00e3 c\u00f3 chuy\u1ebfn: {khoiHanh} - {denDuKien}";
							lblConflictInfo.ForeColor = Color.Red;
							btnPrimary.Enabled = false;
							return;
						}
					}

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
							string tenTx = "T\u00e0i x\u1ebf";
							try { tenTx = conflictTripTx.TaiXe != null ? conflictTripTx.TaiXe.TenTX : tenTx; } catch { }

							var khoiHanh = conflictTripTx.NgayGioKhoiHanh.ToString("HH:mm dd/MM/yyyy");
							var denDuKien = conflictTripTx.NgayGioDenDuKien?.ToString("HH:mm dd/MM/yyyy") ?? "Ch\u01b0a x\u00e1c \u0111\u1ecbnh";
							lblConflictInfo.Text = $"{tenTx} \u0111\u00e3 c\u00f3 chuy\u1ebfn: {khoiHanh} - {denDuKien}";
							lblConflictInfo.ForeColor = Color.Red;
							btnPrimary.Enabled = false;
							return;
						}
					}

					lblConflictInfo.Text = string.Empty;
					btnPrimary.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				lblConflictInfo.Text = "L\u1ed7i ki\u1ec3m tra: " + ex.Message;
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
					Text = "Th\u00eam chuy\u1ebfn xe";
					btnPrimary.Text = "Th\u00eam";
					txtMaChuyen.Enabled = false;
					SetReadOnly(false);
					lblGiaVeInfo.Visible = true;
					lblConflictInfo.Visible = true;
					AutoLoadGiaVe();
					break;
				case ChuyenXeDialogMode.Edit:
					Text = "S\u1eeda chuy\u1ebfn xe";
					btnPrimary.Text = "L\u01b0u";
					txtMaChuyen.Enabled = false;
					SetReadOnly(false);
					lblGiaVeInfo.Visible = true;
					lblConflictInfo.Visible = true;
					break;
				case ChuyenXeDialogMode.View:
					Text = "Xem chuy\u1ebfn xe";
					btnPrimary.Visible = false;
					btnCancel.Text = "\u0110\u00f3ng";
					SetReadOnly(false);
					lblGiaVeInfo.Visible = false;
					lblConflictInfo.Visible = false;
					break;
				case ChuyenXeDialogMode.Delete:
					Text = "X\u00f3a chuy\u1ebfn xe";
					btnPrimary.Text = "X\u00f3a";
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
			txtMaChuyen.Enabled = !ro;
			cbbXe.Enabled = !ro;
			cbbTuyen.Enabled = !ro;
			cbbTaiXe.Enabled = !ro;
			dtpKhoiHanh.Enabled = !ro;
			dtpDenDuKien.Enabled = !ro;
			numGiaVe.Enabled = !ro;

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
