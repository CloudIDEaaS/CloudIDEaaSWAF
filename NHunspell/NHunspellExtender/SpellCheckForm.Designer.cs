using System.Diagnostics;

[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
public partial class SpellCheckForm : System.Windows.Forms.Form
{
    public SpellCheckForm()
    {
        InitializeComponent();
    }

    // Form overrides dispose to clean up the component list.
    [DebuggerNonUserCode()]
    protected override void Dispose(bool disposing)
    {
        try
        {
            if (disposing && components is not null)
            {
                components.Dispose();
            }
        }
        finally
        {
            base.Dispose(disposing);
        }
    }

    // Required by the Windows Form Designer
    private System.ComponentModel.IContainer components;

    // NOTE: The following procedure is required by the Windows Form Designer
    // It can be modified using the Windows Form Designer.  
    // Do not modify it using the code editor.
    [DebuggerStepThrough()]
    private void InitializeComponent()
    {
        Label1 = new System.Windows.Forms.Label();
        Label2 = new System.Windows.Forms.Label();
        lboSuggestions = new System.Windows.Forms.ListBox();
        cmdIgnoreOnce = new System.Windows.Forms.Button();
        cmdIgnoreAll = new System.Windows.Forms.Button();
        cmdAdd = new System.Windows.Forms.Button();
        cmdChange = new System.Windows.Forms.Button();
        cmdChangeAll = new System.Windows.Forms.Button();
        cmdCancel = new System.Windows.Forms.Button();
        txtCurrentSentence = new System.Windows.Forms.RichTextBox();
        this.SuspendLayout();
        // 
        // Label1
        // 
        Label1.AutoSize = true;
        Label1.Location = new System.Drawing.Point(12, 9);
        Label1.Name = "Label1";
        Label1.Size = new System.Drawing.Size(88, 13);
        Label1.TabIndex = 0;
        Label1.Text = "Not in Dictionary:";
        // 
        // Label2
        // 
        Label2.AutoSize = true;
        Label2.Location = new System.Drawing.Point(12, 111);
        Label2.Name = "Label2";
        Label2.Size = new System.Drawing.Size(68, 13);
        Label2.TabIndex = 2;
        Label2.Text = "Suggestions:";
        // 
        // lboSuggestions
        // 
        lboSuggestions.FormattingEnabled = true;
        lboSuggestions.Location = new System.Drawing.Point(15, 127);
        lboSuggestions.Name = "lboSuggestions";
        lboSuggestions.ScrollAlwaysVisible = true;
        lboSuggestions.Size = new System.Drawing.Size(297, 82);
        lboSuggestions.TabIndex = 3;
        // 
        // cmdIgnoreOnce
        // 
        cmdIgnoreOnce.Location = new System.Drawing.Point(328, 23);
        cmdIgnoreOnce.Name = "cmdIgnoreOnce";
        cmdIgnoreOnce.Size = new System.Drawing.Size(104, 22);
        cmdIgnoreOnce.TabIndex = 4;
        cmdIgnoreOnce.Text = "Ignore Once";
        cmdIgnoreOnce.UseVisualStyleBackColor = true;
        // 
        // cmdIgnoreAll
        // 
        cmdIgnoreAll.Location = new System.Drawing.Point(328, 56);
        cmdIgnoreAll.Name = "cmdIgnoreAll";
        cmdIgnoreAll.Size = new System.Drawing.Size(104, 21);
        cmdIgnoreAll.TabIndex = 5;
        cmdIgnoreAll.Text = "Ignore All";
        cmdIgnoreAll.UseVisualStyleBackColor = true;
        // 
        // cmdAdd
        // 
        cmdAdd.Location = new System.Drawing.Point(328, 87);
        cmdAdd.Name = "cmdAdd";
        cmdAdd.Size = new System.Drawing.Size(104, 21);
        cmdAdd.TabIndex = 6;
        cmdAdd.Text = "Add to Dictionary";
        cmdAdd.UseVisualStyleBackColor = true;
        // 
        // cmdChange
        // 
        cmdChange.Location = new System.Drawing.Point(328, 127);
        cmdChange.Name = "cmdChange";
        cmdChange.Size = new System.Drawing.Size(104, 21);
        cmdChange.TabIndex = 7;
        cmdChange.Text = "Change";
        cmdChange.UseVisualStyleBackColor = true;
        // 
        // cmdChangeAll
        // 
        cmdChangeAll.Location = new System.Drawing.Point(328, 158);
        cmdChangeAll.Name = "cmdChangeAll";
        cmdChangeAll.Size = new System.Drawing.Size(104, 21);
        cmdChangeAll.TabIndex = 8;
        cmdChangeAll.Text = "Change All";
        cmdChangeAll.UseVisualStyleBackColor = true;
        // 
        // cmdCancel
        // 
        cmdCancel.Location = new System.Drawing.Point(328, 188);
        cmdCancel.Name = "cmdCancel";
        cmdCancel.Size = new System.Drawing.Size(104, 21);
        cmdCancel.TabIndex = 9;
        cmdCancel.Text = "Cancel";
        cmdCancel.UseVisualStyleBackColor = true;
        // 
        // txtCurrentSentence
        // 
        txtCurrentSentence.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
        txtCurrentSentence.Location = new System.Drawing.Point(15, 25);
        txtCurrentSentence.Name = "txtCurrentSentence";
        txtCurrentSentence.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
        txtCurrentSentence.Size = new System.Drawing.Size(297, 82);
        txtCurrentSentence.TabIndex = 10;
        txtCurrentSentence.Text = "";
        // 
        // SpellCheckForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6.0f, 13.0f);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(451, 229);
        this.Controls.Add(txtCurrentSentence);
        this.Controls.Add(cmdCancel);
        this.Controls.Add(cmdChangeAll);
        this.Controls.Add(cmdChange);
        this.Controls.Add(cmdAdd);
        this.Controls.Add(cmdIgnoreAll);
        this.Controls.Add(cmdIgnoreOnce);
        this.Controls.Add(lboSuggestions);
        this.Controls.Add(Label2);
        this.Controls.Add(Label1);
        this.Name = "SpellCheckForm";
        this.Text = "Spelling";
        this.ResumeLayout(false);
        this.PerformLayout();

    }
    internal System.Windows.Forms.Label Label1;
    internal System.Windows.Forms.Label Label2;
    internal System.Windows.Forms.ListBox lboSuggestions;
    internal System.Windows.Forms.Button cmdIgnoreOnce;
    internal System.Windows.Forms.Button cmdIgnoreAll;
    internal System.Windows.Forms.Button cmdAdd;
    internal System.Windows.Forms.Button cmdChange;
    internal System.Windows.Forms.Button cmdChangeAll;
    internal System.Windows.Forms.Button cmdCancel;
    internal System.Windows.Forms.RichTextBox txtCurrentSentence;
}