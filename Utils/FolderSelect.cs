using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public partial class FolderSelect : UserControl
    {
        private string labelText;
        public event EventHandler FolderSelected;

        public FolderSelect()
        {
            InitializeComponent();
        }

        public FolderBrowserDialog FolderBrowserDialog
        {
            get
            {
                return folderBrowserDialog;
            }
        }

        public int SplitterDistance
        {
            get
            {
                return splitContainer.SplitterDistance;
            }

            set
            {
                splitContainer.SplitterDistance = value;
            }
        }

        public string LabelText 
        {
            get
            {
                return labelText;
            }

            set
            {
                labelText = value;

                if (this.IsHandleCreated)
                {
                    lblLabel.Text = value;
                }
            }
        }

        private void FileSelect_Load(object sender, EventArgs e)
        {
            lblLabel.Text = labelText;
        }

        private void cmdSelectFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                UpdateText();

                FolderSelected(this, EventArgs.Empty);
            }
        }

        public void UpdateText()
        {
            txtFilePath.Text = folderBrowserDialog.SelectedPath;
        }
    }
}
