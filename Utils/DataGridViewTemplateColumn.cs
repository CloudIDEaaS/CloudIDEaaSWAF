using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils
{
    public class DataGridViewTemplateColumn : DataGridViewColumn
    {
        public long MaxValue;
        private bool needsRecalc = true;
        public event DataGridViewTemplateColumnCreateControlHandler CreateControl;
        private bool watchingForChanges;

        public DataGridViewTemplateColumn()
        {
            this.CellTemplate = new DataGridViewTemplateCell();
        }

        protected override void OnDataGridViewChanged()
        {
            if (!watchingForChanges)
            {
                this.DataGridView.DataSourceChanged += new EventHandler(DataGridView_DataSourceChanged);
                watchingForChanges = true;
            }

            base.OnDataGridViewChanged();
        }

        private void DataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            this.DataGridView.Controls.Clear();
        }

        internal void RaiseCreateControl(object sender, DataGridViewTemplateColumnCreateControlEventArgs e)
        {
            this.CreateControl(sender, e);
        }
    }
}
