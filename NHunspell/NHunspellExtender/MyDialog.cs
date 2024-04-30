using System;
using System.Diagnostics;
using System.Windows.Forms;

public partial class MyDialog : Form
{
    private void OK_Button_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Yes;
        this.Close();
    }

    private void lblText_Resize(object sender, EventArgs e)
    {
        // We need to resize the form to match
        if (lblText.Width + 25 > 290)
        {
            this.Width = lblText.Width + 25;
        }
        else
        {
            this.Width = 290;
        }
        if (lblText.Height + 100 > 110)
        {
            this.Height = lblText.Height + 100;
        }
        else
        {
            this.Height = 110;
        }
    }

    public static new DialogResult Show(string Text, string Caption = "")
    {
        MyDialog newDialog = new MyDialog
        {
            Text = Caption,
            lblText = { Text = Text }
        };
        newDialog.ShowDialog();
        DialogResult disable = newDialog.chkDisable.Checked ? DialogResult.Ignore : DialogResult.None;
        return newDialog.DialogResult + (int)disable;
    }

    private void cmdCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.No;
        this.Close();
    }

    public bool DisableFuturePrompts
    {
        get { return chkDisable.Checked; }
    }
}


