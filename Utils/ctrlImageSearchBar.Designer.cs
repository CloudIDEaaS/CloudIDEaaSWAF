
using System;

namespace Utils
{
    partial class ctrlImageSearchBar
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
            this.components = new System.ComponentModel.Container();
            this.timerImageHandler = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuChangeImage = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpenImageLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteImage = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdPicture = new System.Windows.Forms.OpenFileDialog();
            this.sfdPicture = new System.Windows.Forms.SaveFileDialog();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuChangeImage,
            this.mnuOpenImageLocation,
            this.mnuSaveImage,
            this.mnuDeleteImage});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(198, 70);
            //
            // timerImageHandler
            //
            this.timerImageHandler.Tick += timerImageHandler_Tick;
            // 
            // mnuChangeImage
            // 
            this.mnuChangeImage.Name = "mnuChangeImage";
            this.mnuChangeImage.Size = new System.Drawing.Size(197, 22);
            this.mnuChangeImage.Text = "Change Image...";
            this.mnuChangeImage.Click += mnuChangeImage_Click;
            // 
            // mnuOpenImageLocation
            // 
            this.mnuOpenImageLocation.Name = "mnuOpenImageLocation";
            this.mnuOpenImageLocation.Size = new System.Drawing.Size(197, 22);
            this.mnuOpenImageLocation.Text = "Open Image Location...";
            this.mnuOpenImageLocation.Click += mnuOpenImageLocation_Click;
            // 
            // mnuSaveImage
            // 
            this.mnuSaveImage.Name = "mnuSaveImage";
            this.mnuSaveImage.Size = new System.Drawing.Size(197, 22);
            this.mnuSaveImage.Text = "Save Image...";
            this.mnuSaveImage.Click += mnuSaveImage_Click;
            // 
            // mnuDeleteImage
            // 
            this.mnuDeleteImage.Name = "mnuDeleteImage";
            this.mnuDeleteImage.Size = new System.Drawing.Size(197, 22);
            this.mnuDeleteImage.Text = "Delete Image...";
            this.mnuDeleteImage.Click += mnuDeleteImage_Click;
            // 
            // ofdPicture
            // 
            this.ofdPicture.Filter = "Bitmaps|*.bmp|PNG files|*.png|JPEG files|*.jpg|Picture Files|*.bmp;*.jpg;*.gif;*." +
    "png;*.tif";
            // 
            // sfdPicture
            // 
            this.sfdPicture.Filter = "Bitmaps|*.bmp|PNG files|*.png|JPEG files|*.jpg|Picture Files|*.bmp;*.jpg;*.gif;*." +
    "png;*.tif";
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerImageHandler;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mnuChangeImage;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenImageLocation;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveImage;
        private System.Windows.Forms.ToolStripMenuItem mnuDeleteImage;
        private System.Windows.Forms.OpenFileDialog ofdPicture;
        private System.Windows.Forms.SaveFileDialog sfdPicture;
    }
}
