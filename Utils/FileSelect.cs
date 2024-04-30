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
    public partial class FileSelect : UserControl
    {
        private string labelText;
        public event EventHandler FileSelected;

        public FileSelect()
        {
            InitializeComponent();
        }

        public OpenFileDialog OpenFileDialog
        {
            get
            {
                return openFileDialog;
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

        public void UpdateText()
        {
            txtFilePath.Text = openFileDialog.FileName;
        }

        private void FileSelect_Load(object sender, EventArgs e)
        {
            lblLabel.Text = labelText;
        }

        private void cmdSelectFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                UpdateText();

                FileSelected(this, EventArgs.Empty);
            }
        }
    }
}
