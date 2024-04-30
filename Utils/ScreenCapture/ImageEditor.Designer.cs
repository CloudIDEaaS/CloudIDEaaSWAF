namespace Utils.Controls.ScreenCapture
{
    partial class ImageEditor
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ofdPicture = new System.Windows.Forms.OpenFileDialog();
            this.picCropped = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPictureReset = new System.Windows.Forms.ToolStripMenuItem();
            this.scaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuScale50 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuScale100 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuScale200 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuScale300 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuScale400 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuScale500 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuScale1000 = new System.Windows.Forms.ToolStripMenuItem();
            this.sfdPicture = new System.Windows.Forms.SaveFileDialog();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.panelScrollable = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picCropped)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelScrollable.SuspendLayout();
            this.SuspendLayout();
            // 
            // ofdPicture
            // 
            this.ofdPicture.FileName = "openFileDialog1";
            this.ofdPicture.Filter = "Bitmaps|*.bmp|PNG files|*.png|JPEG files|*.jpg|Picture Files|*.bmp;*.jpg;*.gif;*." +
    "png;*.tif";
            // 
            // picCropped
            // 
            this.picCropped.BackColor = System.Drawing.Color.White;
            this.picCropped.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCropped.Cursor = System.Windows.Forms.Cursors.Cross;
            this.picCropped.Location = new System.Drawing.Point(24, 16);
            this.picCropped.Name = "picCropped";
            this.picCropped.Size = new System.Drawing.Size(200, 200);
            this.picCropped.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picCropped.TabIndex = 5;
            this.picCropped.TabStop = false;
            this.picCropped.Visible = false;
            this.picCropped.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picCropped_MouseDown);
            this.picCropped.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picCropped_MouseMove);
            this.picCropped.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picCropped_MouseUp);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.pictureToolStripMenuItem,
            this.scaleToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(447, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileOpen,
            this.mnuFileSave,
            this.toolStripMenuItem1,
            this.mnuFileExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // mnuFileOpen
            // 
            this.mnuFileOpen.Name = "mnuFileOpen";
            this.mnuFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnuFileOpen.Size = new System.Drawing.Size(155, 22);
            this.mnuFileOpen.Text = "&Open...";
            this.mnuFileOpen.Click += new System.EventHandler(this.mnuFileOpen_Click);
            // 
            // mnuFileSave
            // 
            this.mnuFileSave.Name = "mnuFileSave";
            this.mnuFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnuFileSave.Size = new System.Drawing.Size(155, 22);
            this.mnuFileSave.Text = "&Save...";
            this.mnuFileSave.Click += new System.EventHandler(this.mnuFileSave_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 6);
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Name = "mnuFileExit";
            this.mnuFileExit.Size = new System.Drawing.Size(155, 22);
            this.mnuFileExit.Text = "E&xit";
            this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
            // 
            // pictureToolStripMenuItem
            // 
            this.pictureToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPictureReset});
            this.pictureToolStripMenuItem.Name = "pictureToolStripMenuItem";
            this.pictureToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.pictureToolStripMenuItem.Text = "&Picture";
            // 
            // mnuPictureReset
            // 
            this.mnuPictureReset.Name = "mnuPictureReset";
            this.mnuPictureReset.Size = new System.Drawing.Size(102, 22);
            this.mnuPictureReset.Text = "&Reset";
            this.mnuPictureReset.Click += new System.EventHandler(this.mnuPictureReset_Click);
            // 
            // scaleToolStripMenuItem
            // 
            this.scaleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuScale50,
            this.mnuScale100,
            this.mnuScale200,
            this.mnuScale300,
            this.mnuScale400,
            this.mnuScale500,
            this.mnuScale1000});
            this.scaleToolStripMenuItem.Name = "scaleToolStripMenuItem";
            this.scaleToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.scaleToolStripMenuItem.Text = "&Scale";
            // 
            // mnuScale50
            // 
            this.mnuScale50.Name = "mnuScale50";
            this.mnuScale50.Size = new System.Drawing.Size(108, 22);
            this.mnuScale50.Text = "50%";
            this.mnuScale50.Click += new System.EventHandler(this.mnuScale_Click);
            // 
            // mnuScale100
            // 
            this.mnuScale100.Checked = true;
            this.mnuScale100.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mnuScale100.Name = "mnuScale100";
            this.mnuScale100.Size = new System.Drawing.Size(108, 22);
            this.mnuScale100.Text = "&100%";
            this.mnuScale100.Click += new System.EventHandler(this.mnuScale_Click);
            // 
            // mnuScale200
            // 
            this.mnuScale200.Name = "mnuScale200";
            this.mnuScale200.Size = new System.Drawing.Size(108, 22);
            this.mnuScale200.Text = "&200%";
            this.mnuScale200.Click += new System.EventHandler(this.mnuScale_Click);
            // 
            // mnuScale300
            // 
            this.mnuScale300.Name = "mnuScale300";
            this.mnuScale300.Size = new System.Drawing.Size(108, 22);
            this.mnuScale300.Text = "&300%";
            this.mnuScale300.Click += new System.EventHandler(this.mnuScale_Click);
            // 
            // mnuScale400
            // 
            this.mnuScale400.Name = "mnuScale400";
            this.mnuScale400.Size = new System.Drawing.Size(108, 22);
            this.mnuScale400.Text = "&400%";
            this.mnuScale400.Click += new System.EventHandler(this.mnuScale_Click);
            // 
            // mnuScale500
            // 
            this.mnuScale500.Name = "mnuScale500";
            this.mnuScale500.Size = new System.Drawing.Size(108, 22);
            this.mnuScale500.Text = "&500%";
            this.mnuScale500.Click += new System.EventHandler(this.mnuScale_Click);
            // 
            // mnuScale1000
            // 
            this.mnuScale1000.Name = "mnuScale1000";
            this.mnuScale1000.Size = new System.Drawing.Size(108, 22);
            this.mnuScale1000.Text = "1000%";
            this.mnuScale1000.Click += new System.EventHandler(this.mnuScale_Click);
            // 
            // sfdPicture
            // 
            this.sfdPicture.Filter = "Bitmaps|*.bmp|PNG files|*.png|JPEG files|*.jpg|Picture Files|*.bmp;*.jpg;*.gif;*." +
    "png;*.tif";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(360, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.BackColor = System.Drawing.Color.White;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(279, 15);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.SystemColors.Control;
            this.panelButtons.Controls.Add(this.btnCancel);
            this.panelButtons.Controls.Add(this.btnOk);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 340);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(447, 50);
            this.panelButtons.TabIndex = 10;
            // 
            // panelScrollable
            // 
            this.panelScrollable.AutoScroll = true;
            this.panelScrollable.Controls.Add(this.picCropped);
            this.panelScrollable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScrollable.Location = new System.Drawing.Point(0, 24);
            this.panelScrollable.Name = "panelScrollable";
            this.panelScrollable.Size = new System.Drawing.Size(447, 316);
            this.panelScrollable.TabIndex = 11;
            // 
            // ImageEditor
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Gray;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(447, 390);
            this.Controls.Add(this.panelScrollable);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.menuStrip1);
            this.Name = "ImageEditor";
            this.ShowIcon = false;
            this.Text = "Image Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ImageEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picCropped)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelScrollable.ResumeLayout(false);
            this.panelScrollable.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdPicture;
        private System.Windows.Forms.PictureBox picCropped;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuFileSave;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.ToolStripMenuItem pictureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuPictureReset;
        private System.Windows.Forms.SaveFileDialog sfdPicture;
        private System.Windows.Forms.ToolStripMenuItem scaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuScale50;
        private System.Windows.Forms.ToolStripMenuItem mnuScale100;
        private System.Windows.Forms.ToolStripMenuItem mnuScale200;
        private System.Windows.Forms.ToolStripMenuItem mnuScale300;
        private System.Windows.Forms.ToolStripMenuItem mnuScale400;
        private System.Windows.Forms.ToolStripMenuItem mnuScale500;
        private System.Windows.Forms.ToolStripMenuItem mnuScale1000;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Panel panelScrollable;
    }
}

