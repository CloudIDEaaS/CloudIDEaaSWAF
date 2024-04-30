using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public class ctrlValueBoundCheckedListBox : CheckedListBox
    {
        protected override void OnDataSourceChanged(EventArgs e)
        {
            if (this.DataSource is BindingSource bindingSource)
            {
                bindingSource.DataSourceChanged += BindingSource_DataSourceChanged;
                bindingSource.ListChanged += BindingSource_ListChanged;
            }
            else if (this.DataSource is IBindingList bindingList)
            {
                bindingList.ListChanged += BindingSource_ListChanged;
            }

            try
            {
                base.OnDataSourceChanged(e);
            }
            catch (Exception ex)
            {
            }

            UpdateList();
        }

        private void BindingSource_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            UpdateList();
        }

        private void BindingSource_DataSourceChanged(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            var x = 0;

            foreach (var item in this.Items.Cast<object>().ToList())
            {
                var value = item.GetPropertyValue<bool>(this.ValueMember);

                if (value)
                {
                    this.SetItemChecked(x, value);
                }
                else
                {
                    this.SetItemChecked(x, value);
                }

                x++;
            }
        }

        protected override void OnItemCheck(ItemCheckEventArgs ice)
        {
            var item = this.Items.Cast<object>().ElementAt(ice.Index);

            item.SetPropertyValue(this.ValueMember, ice.NewValue == CheckState.Checked);

            base.OnItemCheck(ice);
        }
    }
}
