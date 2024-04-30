using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils
{
    public delegate void DataGridViewTemplateColumnCreateControlHandler(object sender, DataGridViewTemplateColumnCreateControlEventArgs e);

    public class DataGridViewTemplateColumnCreateControlEventArgs : EventArgs
    {
        public Control Control { get; set; }
        public int RowIndex { get; private set; }
        public int ColumnIndex { get; private set; }
        public DataGridViewTemplateColumn OwningColumn { get; private set; }
        public object DataItem { get; private set; }
        public object Value { get; private set; }

        public DataGridViewTemplateColumnCreateControlEventArgs(int rowIndex, int columnIndex, DataGridViewTemplateColumn owningColumn, object dataItem, object value)
        {
            this.ColumnIndex = columnIndex;
            this.OwningColumn = owningColumn;
            this.DataItem = dataItem;
            this.Value = value;
        }
    }
}
