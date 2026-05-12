namespace QLBXMD.GUI.Admin
{
    partial class VeDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlLegend = new System.Windows.Forms.Panel();
            this.lblTrong = new System.Windows.Forms.Label();
            this.pnlColorTrong = new System.Windows.Forms.Panel();
            this.lblDangChon = new System.Windows.Forms.Label();
            this.pnlColorDangChon = new System.Windows.Forms.Panel();
            this.lblDaDat = new System.Windows.Forms.Label();
            this.pnlColorDaDat = new System.Windows.Forms.Panel();
            this.pnlSeatMap = new System.Windows.Forms.Panel();
            this.gbTangDuoi = new System.Windows.Forms.GroupBox();
            this.flpTangDuoi = new System.Windows.Forms.FlowLayoutPanel();
            this.gbTangTren = new System.Windows.Forms.GroupBox();
            this.flpTangTren = new System.Windows.Forms.FlowLayoutPanel();
            
            this.pnlHeader.SuspendLayout();
            this.pnlLegend.SuspendLayout();
            this.pnlSeatMap.SuspendLayout();
            this.gbTangDuoi.SuspendLayout();
            this.gbTangTren.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(900, 50);
            this.pnlHeader.TabIndex = 0;
            
            // 
            // lblHeader
            // 
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(900, 50);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "CHỌN GHẾ NGỒI";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // 
            // pnlLegend
            // 
            this.pnlLegend.Controls.Add(this.pnlColorTrong);
            this.pnlLegend.Controls.Add(this.lblTrong);
            this.pnlLegend.Controls.Add(this.pnlColorDangChon);
            this.pnlLegend.Controls.Add(this.lblDangChon);
            this.pnlLegend.Controls.Add(this.pnlColorDaDat);
            this.pnlLegend.Controls.Add(this.lblDaDat);
            this.pnlLegend.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLegend.Location = new System.Drawing.Point(0, 50);
            this.pnlLegend.Name = "pnlLegend";
            this.pnlLegend.Size = new System.Drawing.Size(900, 50);
            this.pnlLegend.TabIndex = 1;
            
            // 
            // pnlColorTrong
            // 
            this.pnlColorTrong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(199)))), ((int)(((byte)(174)))));
            this.pnlColorTrong.Location = new System.Drawing.Point(200, 15);
            this.pnlColorTrong.Name = "pnlColorTrong";
            this.pnlColorTrong.Size = new System.Drawing.Size(20, 20);
            this.pnlColorTrong.TabIndex = 0;
            
            // 
            // lblTrong
            // 
            this.lblTrong.AutoSize = true;
            this.lblTrong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTrong.Location = new System.Drawing.Point(230, 14);
            this.lblTrong.Name = "lblTrong";
            this.lblTrong.Size = new System.Drawing.Size(55, 23);
            this.lblTrong.TabIndex = 1;
            this.lblTrong.Text = "Trống";
            
            // 
            // pnlColorDangChon
            // 
            this.pnlColorDangChon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.pnlColorDangChon.Location = new System.Drawing.Point(380, 15);
            this.pnlColorDangChon.Name = "pnlColorDangChon";
            this.pnlColorDangChon.Size = new System.Drawing.Size(20, 20);
            this.pnlColorDangChon.TabIndex = 2;
            
            // 
            // lblDangChon
            // 
            this.lblDangChon.AutoSize = true;
            this.lblDangChon.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDangChon.Location = new System.Drawing.Point(410, 14);
            this.lblDangChon.Name = "lblDangChon";
            this.lblDangChon.Size = new System.Drawing.Size(94, 23);
            this.lblDangChon.TabIndex = 3;
            this.lblDangChon.Text = "Đang chọn";
            
            // 
            // pnlColorDaDat
            // 
            this.pnlColorDaDat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(83)))), ((int)(((byte)(79)))));
            this.pnlColorDaDat.Location = new System.Drawing.Point(580, 15);
            this.pnlColorDaDat.Name = "pnlColorDaDat";
            this.pnlColorDaDat.Size = new System.Drawing.Size(20, 20);
            this.pnlColorDaDat.TabIndex = 4;
            
            // 
            // lblDaDat
            // 
            this.lblDaDat.AutoSize = true;
            this.lblDaDat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDaDat.Location = new System.Drawing.Point(610, 14);
            this.lblDaDat.Name = "lblDaDat";
            this.lblDaDat.Size = new System.Drawing.Size(60, 23);
            this.lblDaDat.TabIndex = 5;
            this.lblDaDat.Text = "Đã đặt";
            
            // 
            // pnlSeatMap
            // 
            this.pnlSeatMap.Controls.Add(this.gbTangTren);
            this.pnlSeatMap.Controls.Add(this.gbTangDuoi);
            this.pnlSeatMap.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSeatMap.Location = new System.Drawing.Point(0, 100);
            this.pnlSeatMap.Name = "pnlSeatMap";
            this.pnlSeatMap.Padding = new System.Windows.Forms.Padding(20);
            this.pnlSeatMap.Size = new System.Drawing.Size(900, 420);
            this.pnlSeatMap.TabIndex = 2;
            
            // 
            // gbTangDuoi
            // 
            this.gbTangDuoi.Controls.Add(this.flpTangDuoi);
            this.gbTangDuoi.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbTangDuoi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbTangDuoi.Location = new System.Drawing.Point(20, 20);
            this.gbTangDuoi.Name = "gbTangDuoi";
            this.gbTangDuoi.Size = new System.Drawing.Size(420, 380);
            this.gbTangDuoi.TabIndex = 0;
            this.gbTangDuoi.TabStop = false;
            this.gbTangDuoi.Text = "Tầng dưới";
            
            // 
            // flpTangDuoi
            // 
            this.flpTangDuoi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTangDuoi.Location = new System.Drawing.Point(3, 26);
            this.flpTangDuoi.Name = "flpTangDuoi";
            this.flpTangDuoi.Size = new System.Drawing.Size(414, 351);
            this.flpTangDuoi.TabIndex = 0;
            
            // 
            // gbTangTren
            // 
            this.gbTangTren.Controls.Add(this.flpTangTren);
            this.gbTangTren.Dock = System.Windows.Forms.DockStyle.Right;
            this.gbTangTren.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbTangTren.Location = new System.Drawing.Point(460, 20);
            this.gbTangTren.Name = "gbTangTren";
            this.gbTangTren.Size = new System.Drawing.Size(420, 380);
            this.gbTangTren.TabIndex = 1;
            this.gbTangTren.TabStop = false;
            this.gbTangTren.Text = "Tầng trên";
            
            // 
            // flpTangTren
            // 
            this.flpTangTren.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTangTren.Location = new System.Drawing.Point(3, 26);
            this.flpTangTren.Name = "flpTangTren";
            this.flpTangTren.Size = new System.Drawing.Size(414, 351);
            this.flpTangTren.TabIndex = 0;
            
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(680, 540);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 35);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(780, 540);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 35);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
            // 
            // VeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.pnlSeatMap);
            this.Controls.Add(this.pnlLegend);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VeDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chọn ghế ngồi";
            this.pnlHeader.ResumeLayout(false);
            this.pnlLegend.ResumeLayout(false);
            this.pnlLegend.PerformLayout();
            this.pnlSeatMap.ResumeLayout(false);
            this.gbTangDuoi.ResumeLayout(false);
            this.gbTangTren.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlLegend;
        private System.Windows.Forms.Label lblTrong;
        private System.Windows.Forms.Panel pnlColorTrong;
        private System.Windows.Forms.Label lblDangChon;
        private System.Windows.Forms.Panel pnlColorDangChon;
        private System.Windows.Forms.Label lblDaDat;
        private System.Windows.Forms.Panel pnlColorDaDat;
        private System.Windows.Forms.Panel pnlSeatMap;
        private System.Windows.Forms.GroupBox gbTangDuoi;
        private System.Windows.Forms.FlowLayoutPanel flpTangDuoi;
        private System.Windows.Forms.GroupBox gbTangTren;
        private System.Windows.Forms.FlowLayoutPanel flpTangTren;
    }
}
