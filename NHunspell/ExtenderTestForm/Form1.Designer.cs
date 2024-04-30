using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ExtenderTestForm
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Form1 : Form
    {

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
            components = new System.ComponentModel.Container();
            TextBox1 = new TextBox();
            ContextMenuStrip1 = new ContextMenuStrip();
            TestToolStripMenuItem = new ToolStripMenuItem();
            TestToolStripMenuItem1 = new ToolStripMenuItem();
            RichTextBox1 = new RichTextBox();
            RichTextBox1.FontChanged += new EventHandler(RichTextBox1_FontChanged);
            Button1 = new Button();
            Button1.Click += new EventHandler(Button1_Click);
            Label1 = new Label();
            Label2 = new Label();
            GroupBox1 = new GroupBox();
            TextBox3 = new TextBox();
            Label4 = new Label();
            TextBox2 = new TextBox();
            Label3 = new Label();
            GroupBox2 = new GroupBox();
            Button2 = new Button();
            Button2.Click += new EventHandler(Button2_Click);
            Label5 = new Label();
            RichTextBox2 = new RichTextBox();
            NHunspellTextBoxExtender1 = new NHunspellExtender.NHunspellTextBoxExtender();
            CheckBox1 = new CheckBox();
            CheckBox1.CheckedChanged += new EventHandler(CheckBox1_CheckedChanged);
            ContextMenuStrip1.SuspendLayout();
            GroupBox1.SuspendLayout();
            GroupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // TextBox1
            // 
            TextBox1.AcceptsTab = true;
            TextBox1.ContextMenuStrip = ContextMenuStrip1;
            TextBox1.Location = new Point(17, 42);
            TextBox1.Multiline = true;
            TextBox1.Name = "TextBox1";
            TextBox1.Size = new Size(172, 93);
            NHunspellTextBoxExtender1.SetSpellCheckEnabled(TextBox1, true);
            TextBox1.TabIndex = 0;
            // 
            // ContextMenuStrip1
            // 
            ContextMenuStrip1.Items.AddRange(new ToolStripItem[] { TestToolStripMenuItem });
            ContextMenuStrip1.Name = "ContextMenuStrip1";
            ContextMenuStrip1.Size = new Size(107, 26);
            ContextMenuStrip1.Tag = "TextBox1";
            // 
            // TestToolStripMenuItem
            // 
            TestToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { TestToolStripMenuItem1 });
            TestToolStripMenuItem.Name = "TestToolStripMenuItem";
            TestToolStripMenuItem.Size = new Size(106, 22);
            TestToolStripMenuItem.Text = "Test";
            // 
            // TestToolStripMenuItem1
            // 
            TestToolStripMenuItem1.Name = "TestToolStripMenuItem1";
            TestToolStripMenuItem1.Size = new Size(106, 22);
            TestToolStripMenuItem1.Text = "Test";
            // 
            // RichTextBox1
            // 
            RichTextBox1.Location = new Point(16, 56);
            RichTextBox1.Name = "RichTextBox1";
            RichTextBox1.Size = new Size(167, 98);
            NHunspellTextBoxExtender1.SetSpellCheckEnabled(RichTextBox1, true);
            RichTextBox1.TabIndex = 4;
            RichTextBox1.Text = "";
            // 
            // Button1
            // 
            Button1.Location = new Point(16, 160);
            Button1.Name = "Button1";
            Button1.Size = new Size(154, 23);
            Button1.TabIndex = 6;
            Button1.Text = "Change Selected Font";
            Button1.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(13, 40);
            Label1.Name = "Label1";
            Label1.Size = new Size(157, 13);
            Label1.TabIndex = 7;
            Label1.Text = "RichTexBox without WordWrap";
            // 
            // Label2
            // 
            Label2.AutoSize = true;
            Label2.Location = new Point(19, 27);
            Label2.Name = "Label2";
            Label2.Size = new Size(169, 13);
            Label2.TabIndex = 8;
            Label2.Text = "Multiline TextBox w/ ContextMenu";
            // 
            // GroupBox1
            // 
            GroupBox1.Controls.Add(TextBox3);
            GroupBox1.Controls.Add(Label4);
            GroupBox1.Controls.Add(TextBox2);
            GroupBox1.Controls.Add(Label3);
            GroupBox1.Controls.Add(TextBox1);
            GroupBox1.Controls.Add(Label2);
            GroupBox1.Location = new Point(12, 12);
            GroupBox1.Name = "GroupBox1";
            GroupBox1.Size = new Size(448, 153);
            GroupBox1.TabIndex = 9;
            GroupBox1.TabStop = false;
            GroupBox1.Text = "Standard TextBoxes";
            // 
            // TextBox3
            // 
            TextBox3.Location = new Point(213, 101);
            TextBox3.Name = "TextBox3";
            TextBox3.Size = new Size(229, 20);
            TextBox3.TabIndex = 12;
            // 
            // Label4
            // 
            Label4.AutoSize = true;
            Label4.Location = new Point(210, 85);
            Label4.Name = "Label4";
            Label4.Size = new Size(179, 13);
            Label4.TabIndex = 11;
            Label4.Text = "Single-Line TextBox (no spell check)";
            // 
            // TextBox2
            // 
            TextBox2.ContextMenuStrip = ContextMenuStrip1;
            TextBox2.Location = new Point(213, 43);
            TextBox2.Name = "TextBox2";
            TextBox2.Size = new Size(229, 20);
            TextBox2.TabIndex = 10;
            // 
            // Label3
            // 
            Label3.AutoSize = true;
            Label3.Location = new Point(210, 27);
            Label3.Name = "Label3";
            Label3.Size = new Size(101, 13);
            Label3.TabIndex = 9;
            Label3.Text = "Single-Line TextBox";
            // 
            // GroupBox2
            // 
            GroupBox2.Controls.Add(CheckBox1);
            GroupBox2.Controls.Add(Button2);
            GroupBox2.Controls.Add(Label5);
            GroupBox2.Controls.Add(RichTextBox2);
            GroupBox2.Controls.Add(RichTextBox1);
            GroupBox2.Controls.Add(Button1);
            GroupBox2.Controls.Add(Label1);
            GroupBox2.Location = new Point(16, 185);
            GroupBox2.Name = "GroupBox2";
            GroupBox2.Size = new Size(443, 189);
            GroupBox2.TabIndex = 10;
            GroupBox2.TabStop = false;
            GroupBox2.Text = "RichTextBoxes";
            // 
            // Button2
            // 
            Button2.Location = new Point(273, 160);
            Button2.Name = "Button2";
            Button2.Size = new Size(154, 23);
            Button2.TabIndex = 10;
            Button2.Text = "Change Selected Font";
            Button2.UseVisualStyleBackColor = true;
            // 
            // Label5
            // 
            Label5.AutoSize = true;
            Label5.Location = new Point(204, 40);
            Label5.Name = "Label5";
            Label5.Size = new Size(223, 13);
            Label5.TabIndex = 9;
            Label5.Text = "RichTexBox with WordWrap Zoom Factor 1.5";
            // 
            // RichTextBox2
            // 
            RichTextBox2.Font = new Font("Microsoft Sans Serif", 24.0f, FontStyle.Regular, GraphicsUnit.Point, 0);
            RichTextBox2.Location = new Point(207, 56);
            RichTextBox2.Name = "RichTextBox2";
            RichTextBox2.Size = new Size(220, 98);
            NHunspellTextBoxExtender1.SetSpellCheckEnabled(RichTextBox2, true);
            RichTextBox2.TabIndex = 8;
            RichTextBox2.Text = "";
            RichTextBox2.ZoomFactor = 1.5f;
            // 
            // NHunspellTextBoxExtender1
            // 
            NHunspellTextBoxExtender1.NumberofSuggestions = NHunspellExtender.NHunspellTextBoxExtender.SuggestionNumbers.Five;
            NHunspellTextBoxExtender1.ShortcutKey = Shortcut.F7;
            NHunspellTextBoxExtender1.SpellAsYouType = true;
            // 
            // CheckBox1
            // 
            CheckBox1.AutoSize = true;
            CheckBox1.Checked = true;
            CheckBox1.CheckState = CheckState.Checked;
            CheckBox1.Location = new Point(49, 19);
            CheckBox1.Name = "CheckBox1";
            CheckBox1.Size = new Size(94, 17);
            CheckBox1.TabIndex = 11;
            CheckBox1.Text = "SpellChecking";
            CheckBox1.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(478, 389);
            Controls.Add(GroupBox2);
            Controls.Add(GroupBox1);
            Name = "Form1";
            Text = "Form1";
            ContextMenuStrip1.ResumeLayout(false);
            GroupBox1.ResumeLayout(false);
            GroupBox1.PerformLayout();
            GroupBox2.ResumeLayout(false);
            GroupBox2.PerformLayout();
            ResumeLayout(false);

        }
        internal TextBox TextBox1;
        internal RichTextBox RichTextBox1;
        internal Button Button1;
        internal ContextMenuStrip ContextMenuStrip1;
        internal ToolStripMenuItem TestToolStripMenuItem;
        internal ToolStripMenuItem TestToolStripMenuItem1;
        internal Label Label1;
        internal Label Label2;
        internal GroupBox GroupBox1;
        internal TextBox TextBox3;
        internal Label Label4;
        internal TextBox TextBox2;
        internal Label Label3;
        internal GroupBox GroupBox2;
        internal Button Button2;
        internal Label Label5;
        internal RichTextBox RichTextBox2;
        internal NHunspellExtender.NHunspellTextBoxExtender NHunspellTextBoxExtender1;
        internal CheckBox CheckBox1;

    }
}