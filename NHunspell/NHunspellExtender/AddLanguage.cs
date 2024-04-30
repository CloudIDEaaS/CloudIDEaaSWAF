using System;
using System.Windows.Forms;
using System.Xml.Linq;

public partial class AddLanguage
{
    private DialogResult _result;

    private void cmdAff_Click(object sender, EventArgs e)
    {
        OpenFileDialog newDialog = new OpenFileDialog
        {
            Title = "Select the Aff file",
            Filter = "Aff Files (*.aff)|*.aff|All Files (*.*)|*.*",
            FilterIndex = 1
        };
        if (newDialog.ShowDialog() == DialogResult.OK)
        {
            txtAff.Text = newDialog.FileName;
        }
    }

    private void cmdDic_Click(object sender, EventArgs e)
    {
        OpenFileDialog newDialog = new OpenFileDialog
        {
            Title = "Select the Dic file",
            Filter = "Dic Files (*.dic)|*.dic|All Files (*.*)|*.*",
            FilterIndex = 1
        };
        if (newDialog.ShowDialog() == DialogResult.OK)
        {
            txtDic.Text = newDialog.FileName;
        }
    }

    public DialogResult Result
    {
        get { return _result; }
    }

    private void cmdSubmit_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtAff.Text) || string.IsNullOrEmpty(txtDic.Text) || string.IsNullOrEmpty(txtName.Text))
        {
            MessageBox.Show("You must enter all 3 values");
            return;
        }
        _result = DialogResult.OK;
        this.Hide();
    }
}


