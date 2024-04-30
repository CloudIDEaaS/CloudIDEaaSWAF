
namespace Utils
{
    partial class ctrlEditHyperlink
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
            this.linkLabelUrl = new System.Windows.Forms.LinkLabel();
            this.lblUrlNote = new System.Windows.Forms.Label();
            this.timerUrlValidation = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // linkLabelUrl
            // 
            this.linkLabelUrl.AutoSize = true;
            this.linkLabelUrl.Location = new System.Drawing.Point(3, 3);
            this.linkLabelUrl.Name = "linkLabelUrl";
            this.linkLabelUrl.Size = new System.Drawing.Size(39, 13);
            this.linkLabelUrl.TabIndex = 3;
            this.linkLabelUrl.TabStop = true;
            this.linkLabelUrl.Text = "[None]";
            this.toolTip.SetToolTip(this.linkLabelUrl, "[None]");
            this.linkLabelUrl.Paint += new System.Windows.Forms.PaintEventHandler(this.linkLabelUrl_Paint);
            this.linkLabelUrl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.linkLabelUrl_MouseClick);
            this.linkLabelUrl.MouseLeave += new System.EventHandler(this.linkLabelUrl_MouseLeave);
            this.linkLabelUrl.MouseHover += new System.EventHandler(this.linkLabelUrl_MouseHover);
            this.linkLabelUrl.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.linkLabelUrl_PreviewKeyDown);
            // 
            // lblUrlNote
            // 
            this.lblUrlNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUrlNote.Location = new System.Drawing.Point(42, 3);
            this.lblUrlNote.Name = "lblUrlNote";
            this.lblUrlNote.Size = new System.Drawing.Size(199, 13);
            this.lblUrlNote.TabIndex = 4;
            this.lblUrlNote.Text = "Hover hyperlink or press [Enter] to edit";
            // 
            // timerUrlValidation
            // 
            this.timerUrlValidation.Interval = 2000;
            this.timerUrlValidation.Tick += new System.EventHandler(this.timerUrlValidation_Tick);
            // 
            // toolTip
            // 
            this.toolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip_Popup);
            // 
            // ctrlEditHyperlink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkLabelUrl);
            this.Controls.Add(this.lblUrlNote);
            this.Name = "ctrlEditHyperlink";
            this.Size = new System.Drawing.Size(242, 20);
            this.toolTip.SetToolTip(this, "[None]");
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabelUrl;
        private System.Windows.Forms.Label lblUrlNote;
        private System.Windows.Forms.Timer timerUrlValidation;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
