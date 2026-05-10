using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QLBXMD.GUI.Admin
{
    internal static class AdminGridTheme
    {
        private static readonly Color HeaderBackColor = Color.FromArgb(66, 103, 178);
        private static readonly Color HeaderForeColor = Color.White;
        private static readonly Color GridLineColor = Color.FromArgb(226, 232, 240);
        private static readonly Color AlternateRowBackColor = Color.FromArgb(248, 250, 252);
        private static readonly Color CellBackColor = Color.White;
        private static readonly Color CellForeColor = Color.FromArgb(30, 41, 59);
        private static readonly Color SelectionBackColor = Color.FromArgb(204, 251, 241);
        private static readonly Color SelectionForeColor = Color.FromArgb(15, 23, 42);

        public static void ApplyToControlTree(Control root)
        {
            if (root == null)
            {
                return;
            }

            foreach (DataGridView grid in root.Controls.OfType<DataGridView>())
            {
                Apply(grid);
            }

            foreach (Control child in root.Controls)
            {
                ApplyToControlTree(child);
            }
        }

        public static void Apply(DataGridView grid)
        {
            if (grid == null)
            {
                return;
            }

            grid.AllowUserToResizeRows = false;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeight = 42;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.EnableHeadersVisualStyles = false;
            grid.GridColor = GridLineColor;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowTemplate.Height = 32;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.ColumnAdded -= Grid_ColumnAdded;
            grid.ColumnAdded += Grid_ColumnAdded;

            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = HeaderBackColor,
                ForeColor = HeaderForeColor,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                SelectionBackColor = HeaderBackColor,
                SelectionForeColor = HeaderForeColor,
                WrapMode = DataGridViewTriState.True
            };

            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = CellBackColor,
                ForeColor = CellForeColor,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                SelectionBackColor = SelectionBackColor,
                SelectionForeColor = SelectionForeColor,
                WrapMode = DataGridViewTriState.False
            };

            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = AlternateRowBackColor,
                ForeColor = CellForeColor,
                SelectionBackColor = SelectionBackColor,
                SelectionForeColor = SelectionForeColor
            };

            grid.RowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = CellBackColor,
                ForeColor = CellForeColor,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                SelectionBackColor = SelectionBackColor,
                SelectionForeColor = SelectionForeColor
            };

            foreach (DataGridViewColumn column in grid.Columns)
            {
                ConfigureColumnAppearance(column);
            }
        }

        private static void Grid_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            ConfigureColumnAppearance(e.Column);
        }

        private static void ConfigureColumnAppearance(DataGridViewColumn column)
        {
            if (column == null)
            {
                return;
            }

            column.SortMode = column is DataGridViewButtonColumn
                ? DataGridViewColumnSortMode.NotSortable
                : DataGridViewColumnSortMode.Automatic;

            var buttonColumn = column as DataGridViewButtonColumn;
            if (buttonColumn == null)
            {
                return;
            }

            buttonColumn.FlatStyle = FlatStyle.Flat;
            buttonColumn.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Color.White,
                ForeColor = HeaderBackColor,
                SelectionBackColor = SelectionBackColor,
                SelectionForeColor = HeaderBackColor
            };
        }
    }
}
