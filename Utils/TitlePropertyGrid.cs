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
    public partial class TitlePropertyGrid : UserControl
    {
        public event PropertyValueChangedEventHandler PropertyValueChanged
        {
            add
            {
                propertyGrid.PropertyValueChanged += value;
            }

            remove
            {
                propertyGrid.PropertyValueChanged -= value;
            }
        }

        public TitlePropertyGrid()
        {
            InitializeComponent();
        }

        public object SelectedObject
        {
            get
            {
                return propertyGrid.SelectedObject;
            }

            set
            {
                propertyGrid.SelectedObject = value;

                if (value == null)
                {
                    this.Title = null;
                }
            }
        }
        
        public object[] SelectedObjects 
        {
            get
            {
                return propertyGrid.SelectedObjects;
            }

            set
            {
                propertyGrid.SelectedObjects = value;
            }
        }

        public System.Windows.Forms.PropertySort PropertySort
        {
            get
            {
                return propertyGrid.PropertySort;
            }

            set
            {
                propertyGrid.PropertySort = value;
            }
        }

        public bool ToolbarVisible
        {
            get
            {
                return propertyGrid.ToolbarVisible;
            }

            set
            {
                propertyGrid.ToolbarVisible = value;
            }
        }

        public string Title
        {
            get
            {
                return lblCaption.Text;
            }

            set
            {
                lblCaption.Text = value;
            }
        }

        public PropertyGrid PropertyGrid
        {
            get
            {
                return propertyGrid;
            }
        }
    }
}
