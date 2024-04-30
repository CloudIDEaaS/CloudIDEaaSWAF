
namespace Utils.DropdownTextEntry
{
    partial class frmDropdownText
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDropdownText));
            this.textBox = new System.Windows.Forms.TextBox();
#if !NOHUNSPELL
            this.nHunspellTextBoxExtender = new NHunspellExtender.NHunspellTextBoxExtender();
#endif
            this.cmdCollapse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.Location = new System.Drawing.Point(4, 3);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(479, 323);
            this.textBox.TabIndex = 0;
            this.textBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.textBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textBox_PreviewKeyDown);
            // 
            // nHunspellTextBoxExtender
            // 
#if !NOHUNSPELL
            this.nHunspellTextBoxExtender.Language = "English";
            this.nHunspellTextBoxExtender.MaintainUserChoice = true;
            this.nHunspellTextBoxExtender.ShortcutKey = System.Windows.Forms.Shortcut.F7;
            this.nHunspellTextBoxExtender.SpellAsYouType = true;
#endif
            // 
            // cmdCollapse
            // 
            this.cmdCollapse.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmdCollapse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdCollapse.Image = ((System.Drawing.Image)(resources.GetObject("cmdCollapse.Image")));
            this.cmdCollapse.Location = new System.Drawing.Point(0, 328);
            this.cmdCollapse.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCollapse.Name = "cmdCollapse";
            this.cmdCollapse.Size = new System.Drawing.Size(487, 10);
            this.cmdCollapse.TabIndex = 1;
            this.cmdCollapse.UseVisualStyleBackColor = true;
            this.cmdCollapse.Click += new System.EventHandler(this.cmdCollapse_Click);
            // 
            // frmDropdownText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(487, 338);
            this.Controls.Add(this.cmdCollapse);
            this.Controls.Add(this.textBox);
            this.Name = "frmDropdownText";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Load += new System.EventHandler(this.frmDropdownText_Load);
#if !NOHUNSPELL
            ((System.ComponentModel.ISupportInitialize)(this.nHunspellTextBoxExtender)).EndInit();
#endif
            this.ResumeLayout(false);
            this.PerformLayout();

        }

#endregion

        private System.Windows.Forms.TextBox textBox;
#if !NOHUNSPELL
        private NHunspellExtender.NHunspellTextBoxExtender nHunspellTextBoxExtender;
#endif
        private System.Windows.Forms.Button cmdCollapse;
    }
}