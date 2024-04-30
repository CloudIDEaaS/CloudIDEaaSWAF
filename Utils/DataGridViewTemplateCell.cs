using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Utils
{
    public class DataGridViewTemplateCell : DataGridViewTextBoxCell
    {
        const int HORIZONTALOFFSET = 1;
        const int SPACER = 4;
        private bool controlAdded = false;
        private Control control;

        public DataGridViewTemplateCell()
        {
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (!controlAdded)
            {
                var dataGridView = this.DataGridView;

                if (dataGridView.Controls.Cast<Control>().Any(c => c.Bounds.IntersectsWith(cellBounds) || c.Bounds == cellBounds))
                {
                    var control = dataGridView.Controls.Cast<Control>().Single(c => c.Bounds.IntersectsWith(cellBounds) || c.Bounds == cellBounds);

                    this.control = control;

                    controlAdded = true;
                }
                else
                {
                    var column = (DataGridViewTemplateColumn) this.OwningColumn;
                    var dataItem = ((IEnumerable<object>)dataGridView.DataSource).ElementAt(rowIndex);
                    var args = new DataGridViewTemplateColumnCreateControlEventArgs(rowIndex, this.ColumnIndex, column, dataItem, value);
                    Control control;

                    column.RaiseCreateControl(this, args);

                    control = args.Control;
                    control.Location = cellBounds.Location;
                    control.Size = cellBounds.Size;

                    dataGridView.Controls.Add(args.Control);

                    dataGridView.DelayInvoke(1, () =>
                    {
                        var position = Cursor.Position;

                        dataGridView.ClickWindow(cellBounds.Left, cellBounds.Top);

                        dataGridView.DelayInvoke(1, () =>
                        {
                            Cursor.Position = position;
                        });
                    });

                    control = args.Control;

                    controlAdded = true;
                }
            }

            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, value, errorText, cellStyle, advancedBorderStyle, paintParts);
        }
    }
}
