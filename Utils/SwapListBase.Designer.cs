namespace Utils
{
    partial class SwapListBase
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstAvailableItems = new System.Windows.Forms.ListBox();
            this.lstSelectedItems = new System.Windows.Forms.ListBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.panelInnerButtons = new System.Windows.Forms.Panel();
            this.cmdSelect = new System.Windows.Forms.Button();
            this.cmdRemove = new System.Windows.Forms.Button();
            this.cmdSelectAll = new System.Windows.Forms.Button();
            this.cmdRemoveAll = new System.Windows.Forms.Button();
            this.panelButtons.SuspendLayout();
            this.panelInnerButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstAvailableItems
            // 
            this.lstAvailableItems.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstAvailableItems.FormattingEnabled = true;
            this.lstAvailableItems.Location = new System.Drawing.Point(0, 0);
            this.lstAvailableItems.Name = "lstAvailableItems";
            this.lstAvailableItems.Size = new System.Drawing.Size(298, 196);
            this.lstAvailableItems.TabIndex = 0;
            this.lstAvailableItems.SelectedIndexChanged += new System.EventHandler(this.lst_SelectedIndexChanged);
            this.lstAvailableItems.DoubleClick += new System.EventHandler(this.lst_DoubleClick);
            // 
            // lstSelectedItems
            // 
            this.lstSelectedItems.Dock = System.Windows.Forms.DockStyle.Right;
            this.lstSelectedItems.FormattingEnabled = true;
            this.lstSelectedItems.Location = new System.Drawing.Point(252, 0);
            this.lstSelectedItems.Name = "lstSelectedItems";
            this.lstSelectedItems.Size = new System.Drawing.Size(298, 196);
            this.lstSelectedItems.TabIndex = 0;
            this.lstSelectedItems.SelectedIndexChanged += new System.EventHandler(this.lst_SelectedIndexChanged);
            this.lstSelectedItems.DoubleClick += new System.EventHandler(this.lst_DoubleClick);
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Controls.Add(this.panelInnerButtons);
            this.panelButtons.Location = new System.Drawing.Point(248, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(55, 196);
            this.panelButtons.TabIndex = 2;
            // 
            // panelInnerButtons
            // 
            this.panelInnerButtons.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelInnerButtons.Controls.Add(this.cmdSelect);
            this.panelInnerButtons.Controls.Add(this.cmdRemove);
            this.panelInnerButtons.Controls.Add(this.cmdSelectAll);
            this.panelInnerButtons.Controls.Add(this.cmdRemoveAll);
            this.panelInnerButtons.Location = new System.Drawing.Point(8, 14);
            this.panelInnerButtons.Name = "panelInnerButtons";
            this.panelInnerButtons.Size = new System.Drawing.Size(39, 159);
            this.panelInnerButtons.TabIndex = 2;
            // 
            // cmdSelect
            // 
            this.cmdSelect.Location = new System.Drawing.Point(3, 10);
            this.cmdSelect.Name = "cmdSelect";
            this.cmdSelect.Size = new System.Drawing.Size(32, 32);
            this.cmdSelect.TabIndex = 4;
            this.cmdSelect.Text = ">";
            this.cmdSelect.UseVisualStyleBackColor = true;
            this.cmdSelect.Click += new System.EventHandler(this.cmdSelect_Click);
            // 
            // cmdRemove
            // 
            this.cmdRemove.Location = new System.Drawing.Point(3, 124);
            this.cmdRemove.Name = "cmdRemove";
            this.cmdRemove.Size = new System.Drawing.Size(32, 32);
            this.cmdRemove.TabIndex = 5;
            this.cmdRemove.Text = "<";
            this.cmdRemove.UseVisualStyleBackColor = true;
            this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
            // 
            // cmdSelectAll
            // 
            this.cmdSelectAll.Location = new System.Drawing.Point(3, 48);
            this.cmdSelectAll.Name = "cmdSelectAll";
            this.cmdSelectAll.Size = new System.Drawing.Size(32, 32);
            this.cmdSelectAll.TabIndex = 2;
            this.cmdSelectAll.Text = ">>";
            this.cmdSelectAll.UseVisualStyleBackColor = true;
            this.cmdSelectAll.Click += new System.EventHandler(this.cmdSelectAll_Click);
            // 
            // cmdRemoveAll
            // 
            this.cmdRemoveAll.Location = new System.Drawing.Point(3, 86);
            this.cmdRemoveAll.Name = "cmdRemoveAll";
            this.cmdRemoveAll.Size = new System.Drawing.Size(32, 32);
            this.cmdRemoveAll.TabIndex = 3;
            this.cmdRemoveAll.Text = "<<";
            this.cmdRemoveAll.UseVisualStyleBackColor = true;
            this.cmdRemoveAll.Click += new System.EventHandler(this.cmdRemoveAll_Click);
            // 
            // SwapListBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.lstSelectedItems);
            this.Controls.Add(this.lstAvailableItems);
            this.Name = "SwapListBase";
            this.Size = new System.Drawing.Size(550, 196);
            this.Load += new System.EventHandler(this.SwapListBase_Load);
            this.Resize += new System.EventHandler(this.SwapListBase_Resize);
            this.panelButtons.ResumeLayout(false);
            this.panelInnerButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstAvailableItems;
        private System.Windows.Forms.ListBox lstSelectedItems;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Panel panelInnerButtons;
        private System.Windows.Forms.Button cmdSelect;
        private System.Windows.Forms.Button cmdRemove;
        private System.Windows.Forms.Button cmdSelectAll;
        private System.Windows.Forms.Button cmdRemoveAll;
    }
}
