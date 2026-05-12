using QLBXMD.EF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
    public enum VeDialogMode { View, Edit }

    public partial class VeDialog : Form
    {
        public VeDialogMode Mode { get; private set; } = VeDialogMode.Edit;
        private int _selectedSeat = 0;
        public int ChoNgoiValue { get => _selectedSeat; set => _selectedSeat = value; }

        private int _maChuyen = 0;
        private List<int> _bookedSeats = new List<int>();

        public VeDialog()
        {
            InitializeComponent();
            SetupButtonEffects();
        }

        private void SetupButtonEffects()
        {
            btnSave.MouseEnter += (s, e) => btnSave.BackColor = Color.FromArgb(53, 170, 255);
            btnSave.MouseLeave += (s, e) => btnSave.BackColor = Color.FromArgb(33, 150, 243);
            
            btnCancel.MouseEnter += (s, e) => btnCancel.BackColor = Color.FromArgb(255, 87, 74);
            btnCancel.MouseLeave += (s, e) => btnCancel.BackColor = Color.FromArgb(244, 67, 54);
        }

        public void LoadData(int maChuyen, int currentChoNgoi, VeDialogMode mode)
        {
            Mode = mode;
            _maChuyen = maChuyen;
            _selectedSeat = currentChoNgoi;

            if (mode == VeDialogMode.View)
            {
                this.Text = "Thông tin vé";
                lblHeader.Text = "CHI TIẾT GHẾ NGỒI";
                btnSave.Visible = false;
                btnCancel.Text = "Đóng";
            }
            else
            {
                this.Text = "Chọn ghế ngồi";
                lblHeader.Text = "CHỈNH SỬA GHẾ NGỒI";
                btnSave.Visible = true;
                btnCancel.Text = "Hủy";
            }

            LoadSeatMap();
        }

        private void LoadSeatMap()
        {
            try
            {
                using (var db = new BXMDEntities())
                {
                    var chuyen = db.ChuyenXes.Find(_maChuyen);
                    if (chuyen == null) return;

                    var xe = db.Xes.Find(chuyen.MaXe);
                    if (xe == null) return;

                    var loaiXe = db.LoaiXes.Find(xe.MaLX);
                    if (loaiXe == null) return;

                    int soCho = loaiXe.SoCho;
                    if (soCho == 0) return;

                    bool isGiuongNam = loaiXe.TenLX.ToLower().Contains("giường");

                    _bookedSeats = db.VeXes
                        .Where(v => v.MaChuyen == _maChuyen && v.ChoNgoi != _selectedSeat)
                        .Select(v => v.ChoNgoi)
                        .ToList();

                    flpTangDuoi.Controls.Clear();
                    flpTangTren.Controls.Clear();

                    if (isGiuongNam)
                    {
                        gbTangDuoi.Visible = true;
                        gbTangTren.Visible = true;

                        int choMoiTang = soCho / 2;
                        CreateSeatButtons(flpTangDuoi, 1, choMoiTang);
                        CreateSeatButtons(flpTangTren, choMoiTang + 1, soCho);
                    }
                    else
                    {
                        gbTangDuoi.Visible = true;
                        gbTangTren.Visible = false;
                        gbTangDuoi.Text = "Sơ đồ ghế";

                        CreateSeatButtons(flpTangDuoi, 1, soCho);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải sơ đồ ghế: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateSeatButtons(FlowLayoutPanel panel, int startSeat, int endSeat)
        {
            panel.SuspendLayout();
            
            for (int i = startSeat; i <= endSeat; i++)
            {
                Button btnSeat = new Button
                {
                    Name = $"btnGhe_{i}",
                    Text = i.ToString(),
                    Width = 50,
                    Height = 50,
                    Margin = new Padding(5),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Tag = i
                };

                btnSeat.FlatAppearance.BorderSize = 1;
                btnSeat.FlatAppearance.BorderColor = Color.Gray;

                if (_bookedSeats.Contains(i))
                {
                    btnSeat.BackColor = Color.FromArgb(217, 83, 79);
                    btnSeat.ForeColor = Color.White;
                    btnSeat.Enabled = false;
                }
                else if (i == _selectedSeat)
                {
                    btnSeat.BackColor = Color.FromArgb(255, 152, 0);
                    btnSeat.ForeColor = Color.White;
                    if (Mode == VeDialogMode.Edit)
                    {
                        btnSeat.Click += BtnSeat_Click;
                    }
                }
                else
                {
                    btnSeat.BackColor = Color.FromArgb(0, 199, 174);
                    btnSeat.ForeColor = Color.White;
                    if (Mode == VeDialogMode.Edit)
                    {
                        btnSeat.Click += BtnSeat_Click;
                    }
                }

                panel.Controls.Add(btnSeat);
            }
            
            panel.ResumeLayout();
        }

        private void BtnSeat_Click(object sender, EventArgs e)
        {
            Button clickedBtn = sender as Button;
            int seatNo = (int)clickedBtn.Tag;

            if (_selectedSeat > 0 && _selectedSeat != seatNo)
            {
                Button oldBtn = FindSeatButton(_selectedSeat);
                if (oldBtn != null)
                {
                    oldBtn.BackColor = Color.FromArgb(0, 199, 174);
                }
            }

            clickedBtn.BackColor = Color.FromArgb(255, 152, 0);
            _selectedSeat = seatNo;
        }

        private Button FindSeatButton(int seatNo)
        {
            string btnName = $"btnGhe_{seatNo}";
            var btns = flpTangDuoi.Controls.Find(btnName, false);
            if (btns.Length > 0) return btns[0] as Button;

            btns = flpTangTren.Controls.Find(btnName, false);
            if (btns.Length > 0) return btns[0] as Button;

            return null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_selectedSeat <= 0)
            {
                MessageBox.Show("Vui lòng chọn 1 ghế.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
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
