namespace Utils.Controls.ScreenCapture
{
    partial class ScreenCaptureControlPanel
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
            this.btnCaptureScreen = new System.Windows.Forms.Button();
            this.btnCaptureArea = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.txtTips = new System.Windows.Forms.RichTextBox();
            this.saveToClipboard = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.upDownScreens = new System.Windows.Forms.DomainUpDown();
            this.SuspendLayout();
            // 
            // btnCaptureScreen
            // 
            this.btnCaptureScreen.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCaptureScreen.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCaptureScreen.Location = new System.Drawing.Point(2, 15);
            this.btnCaptureScreen.Name = "btnCaptureScreen";
            this.btnCaptureScreen.Size = new System.Drawing.Size(104, 32);
            this.btnCaptureScreen.TabIndex = 0;
            this.btnCaptureScreen.TabStop = false;
            this.btnCaptureScreen.Text = "Capture Screen";
            this.btnCaptureScreen.UseVisualStyleBackColor = false;
            this.btnCaptureScreen.Click += new System.EventHandler(this.btnCaptureScreen_Click);
            this.btnCaptureScreen.KeyUp += new System.Windows.Forms.KeyEventHandler(this.bttCaptureScreen_KeyUp);
            // 
            // btnCaptureArea
            // 
            this.btnCaptureArea.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCaptureArea.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCaptureArea.Location = new System.Drawing.Point(2, 57);
            this.btnCaptureArea.Name = "btnCaptureArea";
            this.btnCaptureArea.Size = new System.Drawing.Size(104, 32);
            this.btnCaptureArea.TabIndex = 2;
            this.btnCaptureArea.TabStop = false;
            this.btnCaptureArea.Text = "Capture Area";
            this.btnCaptureArea.UseVisualStyleBackColor = false;
            this.btnCaptureArea.Click += new System.EventHandler(this.btnCaptureArea_Click);
            this.btnCaptureArea.KeyUp += new System.Windows.Forms.KeyEventHandler(this.bttCaptureArea_KeyUp);
            // 
            // txtTips
            // 
            this.txtTips.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtTips.Location = new System.Drawing.Point(125, 7);
            this.txtTips.Name = "txtTips";
            this.txtTips.ReadOnly = true;
            this.txtTips.Size = new System.Drawing.Size(247, 105);
            this.txtTips.TabIndex = 5;
            this.txtTips.Text = "";
            this.txtTips.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtTips_KeyUp);
            // 
            // saveToClipboard
            // 
            this.saveToClipboard.AutoSize = true;
            this.saveToClipboard.Location = new System.Drawing.Point(9, 95);
            this.saveToClipboard.Name = "saveToClipboard";
            this.saveToClipboard.Size = new System.Drawing.Size(110, 17);
            this.saveToClipboard.TabIndex = 6;
            this.saveToClipboard.TabStop = false;
            this.saveToClipboard.Text = "Save to Clipboard";
            this.saveToClipboard.UseVisualStyleBackColor = true;
            this.saveToClipboard.CheckedChanged += new System.EventHandler(this.saveToClipboard_CheckedChanged);
            this.saveToClipboard.KeyUp += new System.Windows.Forms.KeyEventHandler(this.saveToClipboard_KeyUp);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(293, 120);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(212, 120);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // upDownScreens
            // 
            this.upDownScreens.Location = new System.Drawing.Point(9, 123);
            this.upDownScreens.Name = "upDownScreens";
            this.upDownScreens.Size = new System.Drawing.Size(143, 20);
            this.upDownScreens.TabIndex = 10;
            // 
            // ScreenCaptureControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(380, 155);
            this.Controls.Add(this.upDownScreens);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.saveToClipboard);
            this.Controls.Add(this.txtTips);
            this.Controls.Add(this.btnCaptureScreen);
            this.Controls.Add(this.btnCaptureArea);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScreenCaptureControlPanel";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Screen Capture";
            this.Load += new System.EventHandler(this.ControlPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCaptureScreen;
        private System.Windows.Forms.Button btnCaptureArea;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.RichTextBox txtTips;
        private System.Windows.Forms.CheckBox saveToClipboard;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.DomainUpDown upDownScreens;
    }
}