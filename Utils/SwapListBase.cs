using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils
{
    public abstract partial class SwapListBase : UserControl
    {
        public abstract event EventHandler ListItemsChanged;
        protected abstract void SwapListBase_Load(object sender, EventArgs e);
        protected abstract void RaiseListItemsChangedEvent();

        public SwapListBase()
        {
            InitializeComponent();
        }

        public int AvailableItemIndex
        {
            get
            {
                return lstAvailableItems.SelectedIndex;
            }
            set
            {
                lstAvailableItems.SelectedIndex = value;
            }
        }

        public int SelectedItemIndex
        {
            get
            {
                return lstSelectedItems.SelectedIndex;
            }
            set
            {
                lstSelectedItems.SelectedIndex = value;
            }
        }

        private void cmdSelect_Click(object sender, EventArgs e)
        {
            MoveSelectedItems(lstAvailableItems, lstSelectedItems);
        }

        private void cmdSelectAll_Click(object sender, EventArgs e)
        {
            MoveAllItems(lstAvailableItems, lstSelectedItems);
        }

        private void cmdRemove_Click(object sender, EventArgs e)
        {
            MoveSelectedItems(lstSelectedItems, lstAvailableItems);
        }

        private void cmdRemoveAll_Click(object sender, EventArgs e)
        {
            MoveAllItems(lstSelectedItems, lstAvailableItems);
        }

        private void MoveSelectedItems(ListBox lstFrom, ListBox lstTo)
        {
            while (lstFrom.SelectedItems.Count > 0)
            {
                var item = lstFrom.SelectedItems[0];
                lstTo.Items.Add(item);
                lstFrom.Items.Remove(item);
            }

            SetButtonsEditable();
            RaiseListItemsChangedEvent();          
        }

        private void MoveAllItems(ListBox lstFrom, ListBox lstTo)
        {
            lstTo.Items.AddRange(lstFrom.Items);
            lstFrom.Items.Clear();
            SetButtonsEditable();
            RaiseListItemsChangedEvent();
        }

        private void lst_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetButtonsEditable();
        }

        protected void SetButtonsEditable()
        {
            cmdSelect.Enabled = (lstAvailableItems.SelectedItems.Count > 0);
            cmdSelectAll.Enabled = (lstAvailableItems.Items.Count > 0);
            cmdRemove.Enabled = (lstSelectedItems.SelectedItems.Count > 0);
            cmdRemoveAll.Enabled = (lstSelectedItems.Items.Count > 0);
        }

        protected ListBox ListBoxAvailableItems
        {
            get
            {
                return this.lstAvailableItems;
            }
        }

        protected ListBox ListBoxSelectedItems
        {
            get
            {
                return this.lstSelectedItems;
            }
        }

        private void SwapListBase_Resize(object sender, EventArgs e)
        {
            this.DelayInvoke(1, () =>
            {
                HandleResize();
            });
        }

        private void HandleResize()
        {
            if (!this.Created)
            {
                this.DelayInvoke(100, () =>
                {
                    HandleResize();
                });
            }
            else
            {
                lstAvailableItems.Width = panelButtons.Left - 3;
                lstSelectedItems.Width = this.Width - (panelButtons.Right + 3);
            }
        }

        private void lst_DoubleClick(object sender, EventArgs e)
        {
            var clickedList = (ListBox)sender;

            if (clickedList == lstAvailableItems)
            {
                MoveSelectedItems(lstAvailableItems, lstSelectedItems);
            }
            else if (clickedList == lstSelectedItems)
            {
                MoveSelectedItems(lstSelectedItems, lstAvailableItems);
            }
        }
    }
}