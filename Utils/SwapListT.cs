using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace Utils
{
    public class SwapList<T> : SwapListBase
    {
        public override event EventHandler ListItemsChanged;
        private bool suppressListChangedEvent;
        public BindingList<T> AvailableItems { get; set; }
        public BindingList<T> SelectedItems { get; set; }
        public event EventHandlerT<T> AvailableItemSelected;
        public event EventHandlerT<T> SelectedItemSelected;

        public SwapList()
        {
            AvailableItems = new BindingList<T>();
            SelectedItems = new BindingList<T>();
        }

        protected override void SwapListBase_Load(object sender, EventArgs e)
        {
            AvailableItems.ListChanged += new ListChangedEventHandler(AvailableItems_ListChanged);
            SelectedItems.ListChanged += new ListChangedEventHandler(SelectedItems_ListChanged);
            ListBoxAvailableItems.SelectedIndexChanged += new EventHandler(ListBoxAvailableItems_SelectedIndexChanged);
            ListBoxSelectedItems.SelectedIndexChanged += new EventHandler(ListBoxSelectedItems_SelectedIndexChanged);
        }

        private void ListBoxAvailableItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AvailableItemSelected != null)
            {
                AvailableItemSelected(this, new EventArgs<T>((T)ListBoxAvailableItems.SelectedItem));
            }
        }

        private void ListBoxSelectedItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedItemSelected != null)
            {
                SelectedItemSelected(this, new EventArgs<T>((T)ListBoxSelectedItems.SelectedItem));
            }
        }

        private void AvailableItems_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (!suppressListChangedEvent)
            {
                ListBoxAvailableItems.Items.Clear();

                foreach (var item in AvailableItems)
                {
                    ListBoxAvailableItems.Items.Add(item);
                }

                SetButtonsEditable();
            }
        }

        private void SelectedItems_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (!suppressListChangedEvent)
            {
                ListBoxSelectedItems.Items.Clear();

                foreach (var item in SelectedItems)
                {
                    ListBoxSelectedItems.Items.Add(item);
                }

                SetButtonsEditable();
            }
        }

        protected override void RaiseListItemsChangedEvent()
        {
            suppressListChangedEvent = true;

            AvailableItems.Clear();
            SelectedItems.Clear();

            foreach (T item in ListBoxAvailableItems.Items)
            {
                AvailableItems.Add(item);
            }

            foreach (T item in ListBoxSelectedItems.Items)
            {
                SelectedItems.Add(item);
            }

            suppressListChangedEvent = false;

            if (ListItemsChanged != null)
            {
                ListItemsChanged(this, EventArgs.Empty);
            }
        }
    }
}
