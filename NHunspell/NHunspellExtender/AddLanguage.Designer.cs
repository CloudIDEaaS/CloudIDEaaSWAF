using System;
using System.Reflection.Emit;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

partial class AddLanguage : Form
{
    public TextBox txtName;
    private Label label1;
    private Label label2;
    public TextBox txtAff;
    private Label label3;
    public TextBox txtDic;
    private Button cmdAff;
    private Button cmdDic;
    private Button cmdSubmit;

    public AddLanguage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.txtName = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.txtAff = new System.Windows.Forms.TextBox();
        this.label3 = new System.Windows.Forms.Label();
        this.txtDic = new System.Windows.Forms.TextBox();
        this.cmdAff = new System.Windows.Forms.Button();
        this.cmdDic = new System.Windows.Forms.Button();
        this.cmdSubmit = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // txtName
        // 
        this.txtName.Location = new System.Drawing.Point(69, 6);
        this.txtName.Name = "txtName";
        this.txtName.Size = new System.Drawing.Size(224, 20);
        this.txtName.TabIndex = 0;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(22, 9);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(38, 13);
        this.label1.TabIndex = 1;
        this.label1.Text = "Name:";
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(12, 35);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(48, 13);
        this.label2.TabIndex = 3;
        this.label2.Text = "Aff Path:";
        // 
        // txtAff
        // 
        this.txtAff.Location = new System.Drawing.Point(69, 32);
        this.txtAff.Name = "txtAff";
        this.txtAff.ReadOnly = true;
        this.txtAff.Size = new System.Drawing.Size(224, 20);
        this.txtAff.TabIndex = 2;
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(12, 61);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(51, 13);
        this.label3.TabIndex = 5;
        this.label3.Text = "Dic Path:";
        // 
        // txtDic
        // 
        this.txtDic.Location = new System.Drawing.Point(69, 58);
        this.txtDic.Name = "txtDic";
        this.txtDic.ReadOnly = true;
        this.txtDic.Size = new System.Drawing.Size(224, 20);
        this.txtDic.TabIndex = 4;
        // 
        // cmdAff
        // 
        this.cmdAff.Image = My.Resources.folder_go;
        this.cmdAff.Location = new System.Drawing.Point(299, 30);
        this.cmdAff.Name = "cmdAff";
        this.cmdAff.Size = new System.Drawing.Size(29, 23);
        this.cmdAff.TabIndex = 6;
        this.cmdAff.UseVisualStyleBackColor = true;
        // 
        // cmdDic
        // 
        this.cmdDic.Image = My.Resources.folder_go;
        this.cmdDic.Location = new System.Drawing.Point(299, 56);
        this.cmdDic.Name = "cmdDic";
        this.cmdDic.Size = new System.Drawing.Size(29, 23);
        this.cmdDic.TabIndex = 7;
        this.cmdDic.UseVisualStyleBackColor = true;
        // 
        // cmdSubmit
        // 
        this.cmdSubmit.Location = new System.Drawing.Point(253, 85);
        this.cmdSubmit.Name = "cmdSubmit";
        this.cmdSubmit.Size = new System.Drawing.Size(75, 23);
        this.cmdSubmit.TabIndex = 8;
        this.cmdSubmit.Text = "Submit";
        this.cmdSubmit.UseVisualStyleBackColor = true;
        // 
        // AddLanguage
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(333, 118);
        this.Controls.Add(this.cmdSubmit);
        this.Controls.Add(this.cmdDic);
        this.Controls.Add(this.cmdAff);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.txtDic);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.txtAff);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.txtName);
        this.Name = "AddLanguage";
        this.Text = "Add a New Language";
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}



