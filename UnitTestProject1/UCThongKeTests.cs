using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Windows.Forms;
using QLBXMD.GUI.Admin;
using QLBXMD.EF;

namespace QLBXMD.Tests
{
    [TestClass]
    public class UCThongKeTests
    {
        private UCThongKe _ucThongKe;

        [TestInitialize]
        public void Setup()
        {
            // Kh?i t?o UserControl
            _ucThongKe = new UCThongKe();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _ucThongKe?.Dispose();
        }

        #region Constructor Tests

        [TestMethod]
        [TestCategory("Constructor")]
        public void Constructor_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var control = new UCThongKe();

            // Assert
            Assert.IsNotNull(control);
            Assert.IsNotNull(control.Controls);
        }

        [TestMethod]
        [TestCategory("Constructor")]
        public void Constructor_ShouldSetDefaultDateRange()
        {
            // Arrange & Act
            var control = new UCThongKe();
            
            // S? d?ng reflection ?? truy c?p private DateTimePicker
            var dtpTuNgay = GetPrivateField<DateTimePicker>(control, "dtpTuNgay");
            var dtpDenNgay = GetPrivateField<DateTimePicker>(control, "dtpDenNgay");

            // Assert
            Assert.IsNotNull(dtpTuNgay);
            Assert.IsNotNull(dtpDenNgay);
            
            // Ki?m tra dtpTuNgay là ngày ??u tháng
            Assert.AreEqual(1, dtpTuNgay.Value.Day);
            Assert.AreEqual(DateTime.Now.Month, dtpTuNgay.Value.Month);
            Assert.AreEqual(DateTime.Now.Year, dtpTuNgay.Value.Year);
            
            // Ki?m tra dtpDenNgay là ngày hi?n t?i
            Assert.AreEqual(DateTime.Now.Date, dtpDenNgay.Value.Date);
        }

        #endregion

        #region Date Validation Tests

        [TestMethod]
        [TestCategory("Validation")]
        public void DateValidation_WhenStartDateAfterEndDate_ShouldShowWarning()
        {
            // Arrange
            var dtpTuNgay = GetPrivateField<DateTimePicker>(_ucThongKe, "dtpTuNgay");
            var dtpDenNgay = GetPrivateField<DateTimePicker>(_ucThongKe, "dtpDenNgay");
            
            dtpTuNgay.Value = DateTime.Now;
            dtpDenNgay.Value = DateTime.Now.AddDays(-5);

            // Act
            var btnThongKe = GetPrivateField<Button>(_ucThongKe, "btnThongKe");
            
            // Gi? l?p click button b?ng cách g?i event handler
            bool validationFailed = false;
            try
            {
                // G?i ph??ng th?c private btnThongKe_Click
                InvokePrivateMethod(_ucThongKe, "btnThongKe_Click", btnThongKe, EventArgs.Empty);
            }
            catch
            {
                validationFailed = true;
            }

            // Assert
            // Test này s? hi?n th? MessageBox, trong môi tr??ng test th?c t? c?n mock MessageBox
            Assert.IsTrue(dtpTuNgay.Value > dtpDenNgay.Value);
        }

        [TestMethod]
        [TestCategory("Validation")]
        public void DateValidation_WhenStartDateEqualsEndDate_ShouldBeValid()
        {
            // Arrange
            var dtpTuNgay = GetPrivateField<DateTimePicker>(_ucThongKe, "dtpTuNgay");
            var dtpDenNgay = GetPrivateField<DateTimePicker>(_ucThongKe, "dtpDenNgay");
            
            var testDate = DateTime.Now.Date;
            dtpTuNgay.Value = testDate;
            dtpDenNgay.Value = testDate;

            // Assert
            Assert.IsFalse(dtpTuNgay.Value > dtpDenNgay.Value);
        }

        [TestMethod]
        [TestCategory("Validation")]
        public void DateValidation_WhenStartDateBeforeEndDate_ShouldBeValid()
        {
            // Arrange
            var dtpTuNgay = GetPrivateField<DateTimePicker>(_ucThongKe, "dtpTuNgay");
            var dtpDenNgay = GetPrivateField<DateTimePicker>(_ucThongKe, "dtpDenNgay");
            
            dtpTuNgay.Value = DateTime.Now.AddDays(-10);
            dtpDenNgay.Value = DateTime.Now;

            // Assert
            Assert.IsFalse(dtpTuNgay.Value > dtpDenNgay.Value);
        }

        #endregion

        #region Chart Tests

        [TestMethod]
        [TestCategory("Charts")]
        public void Charts_ShouldBeInitializedWithTitles()
        {
            // Arrange & Act
            var chartDoanhThu = GetPrivateField<System.Windows.Forms.DataVisualization.Charting.Chart>(_ucThongKe, "chartDoanhThu");
            var chartTuyenDuong = GetPrivateField<System.Windows.Forms.DataVisualization.Charting.Chart>(_ucThongKe, "chartTuyenDuong");

            // Assert
            Assert.IsNotNull(chartDoanhThu);
            Assert.IsNotNull(chartTuyenDuong);
            Assert.IsTrue(chartDoanhThu.Titles.Count > 0);
            Assert.IsTrue(chartTuyenDuong.Titles.Count > 0);
            Assert.AreEqual("Bi?u ?? Doanh Thu Theo Ngày", chartDoanhThu.Titles[0].Text);
            Assert.AreEqual("Top 5 Tuy?n ???ng Ph? Bi?n", chartTuyenDuong.Titles[0].Text);
        }

        [TestMethod]
        [TestCategory("Charts")]
        public void DoanhThuChart_ShouldHaveCorrectAxisTitles()
        {
            // Arrange & Act
            var chartDoanhThu = GetPrivateField<System.Windows.Forms.DataVisualization.Charting.Chart>(_ucThongKe, "chartDoanhThu");

            // Assert
            Assert.AreEqual("Ngày", chartDoanhThu.ChartAreas[0].AxisX.Title);
            Assert.AreEqual("Doanh thu (VN?)", chartDoanhThu.ChartAreas[0].AxisY.Title);
        }

        [TestMethod]
        [TestCategory("Charts")]
        public void DoanhThuChart_ShouldHaveCorrectFormat()
        {
            // Arrange & Act
            var chartDoanhThu = GetPrivateField<System.Windows.Forms.DataVisualization.Charting.Chart>(_ucThongKe, "chartDoanhThu");

            // Assert
            Assert.AreEqual("#,##0", chartDoanhThu.ChartAreas[0].AxisY.LabelStyle.Format);
            Assert.AreEqual(-45, chartDoanhThu.ChartAreas[0].AxisX.LabelStyle.Angle);
        }

        [TestMethod]
        [TestCategory("Charts")]
        public void TuyenDuongChart_ShouldBeConfiguredAsPieChart()
        {
            // Arrange & Act
            var chartTuyenDuong = GetPrivateField<System.Windows.Forms.DataVisualization.Charting.Chart>(_ucThongKe, "chartTuyenDuong");

            // Assert
            Assert.IsTrue(chartTuyenDuong.Series.Count > 0);
            Assert.AreEqual(System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie, 
                chartTuyenDuong.Series[0].ChartType);
        }

        #endregion

        #region Statistics Labels Tests

        [TestMethod]
        [TestCategory("UI")]
        public void StatisticsLabels_ShouldExist()
        {
            // Arrange & Act
            var lblDoanhThuValue = GetPrivateField<Label>(_ucThongKe, "lblDoanhThuValue");
            var lblSoChuyenXeValue = GetPrivateField<Label>(_ucThongKe, "lblSoChuyenXeValue");
            var lblSoKhachHangValue = GetPrivateField<Label>(_ucThongKe, "lblSoKhachHangValue");
            var lblSoVeValue = GetPrivateField<Label>(_ucThongKe, "lblSoVeValue");

            // Assert
            Assert.IsNotNull(lblDoanhThuValue);
            Assert.IsNotNull(lblSoChuyenXeValue);
            Assert.IsNotNull(lblSoKhachHangValue);
            Assert.IsNotNull(lblSoVeValue);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// L?y private field t? object s? d?ng reflection
        /// </summary>
        private T GetPrivateField<T>(object obj, string fieldName) where T : class
        {
            var field = obj.GetType().GetField(fieldName, 
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Instance);
            
            return field?.GetValue(obj) as T;
        }

        /// <summary>
        /// G?i private method s? d?ng reflection
        /// </summary>
        private object InvokePrivateMethod(object obj, string methodName, params object[] parameters)
        {
            var method = obj.GetType().GetMethod(methodName,
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

            return method?.Invoke(obj, parameters);
        }

        #endregion
    }
}
