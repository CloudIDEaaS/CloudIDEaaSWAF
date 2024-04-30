using System;
using System.Windows.Forms;

namespace ExtenderTestForm
{
    public partial class Form1
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void Button1_Click(object sender, EventArgs e)
        {
            var newFont = new FontDialog();
            if (newFont.ShowDialog() == DialogResult.OK)
            {
                if (RichTextBox1.SelectionLength == 0)
                {
                    RichTextBox1.Font = newFont.Font;
                }
                else
                {
                    RichTextBox1.SelectionFont = newFont.Font;
                }
            }
        }


        private void Button2_Click(object sender, EventArgs e)
        {
            // Using w As New System.IO.StreamWriter("D:\Messagelog2.txt", True)
            // w.WriteLine("Change Font")
            // w.Flush()
            // w.Close()
            // End Using

            var newFont = new FontDialog();
            if (newFont.ShowDialog() == DialogResult.OK)
            {
                if (RichTextBox2.SelectionLength == 0)
                {
                    RichTextBox2.Font = newFont.Font;
                }
                else
                {
                    RichTextBox2.SelectionFont = newFont.Font;
                }
            }
        }

        private void RichTextBox1_FontChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Font Changed");
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox1.Checked)
            {
                NHunspellTextBoxExtender1.EnableTextBoxBase(RichTextBox1);
            }
            else
            {
                TextBoxBase argTextBoxToDisable = RichTextBox1;
                NHunspellTextBoxExtender1.DisableTextBoxBase(ref argTextBoxToDisable);
                RichTextBox1 = (RichTextBox)argTextBoxToDisable;
            }
        }
    }
}