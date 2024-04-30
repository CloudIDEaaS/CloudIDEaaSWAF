
namespace Utils
{
    partial class frmLockingProcesses
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
            this.components = new System.ComponentModel.Container();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listViewProcesses = new System.Windows.Forms.ListView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.cmdCloseSelected = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdCancel.Location = new System.Drawing.Point(475, 382);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 0;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(494, 35);
            this.label1.TabIndex = 1;
            this.label1.Text = "The following processes are preventing further processing. Click \'Close Selected\'" +
    " to close the processes and continue, or click \'Cancel\' to stop processing.\r\n";
            // 
            // listViewProcesses
            // 
            this.listViewProcesses.CheckBoxes = true;
            this.listViewProcesses.HideSelection = false;
            this.listViewProcesses.Location = new System.Drawing.Point(12, 47);
            this.listViewProcesses.Name = "listViewProcesses";
            this.listViewProcesses.Size = new System.Drawing.Size(538, 322);
            this.listViewProcesses.SmallImageList = this.imageList;
            this.listViewProcesses.TabIndex = 2;
            this.listViewProcesses.UseCompatibleStateImageBehavior = false;
            this.listViewProcesses.View = System.Windows.Forms.View.List;
            this.listViewProcesses.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewProcesses_ItemChecked);
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // cmdCloseSelected
            // 
            this.cmdCloseSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCloseSelected.Enabled = false;
            this.cmdCloseSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdCloseSelected.Location = new System.Drawing.Point(370, 382);
            this.cmdCloseSelected.Name = "cmdCloseSelected";
            this.cmdCloseSelected.Size = new System.Drawing.Size(99, 23);
            this.cmdCloseSelected.TabIndex = 0;
            this.cmdCloseSelected.Text = "Close Selected";
            this.cmdCloseSelected.UseVisualStyleBackColor = true;
            this.cmdCloseSelected.Click += new System.EventHandler(this.cmdCloseSelected_Click);
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Maroon;
            this.label2.Location = new System.Drawing.Point(12, 375);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(352, 38);
            this.label2.TabIndex = 3;
            this.label2.Text = "WARNING: Closing processes may cause system instability, and/or loss of data or u" +
    "nsaved documents.  Please do so at your own risk.";
            // 
            // frmLockingProcesses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(562, 417);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listViewProcesses);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdCloseSelected);
            this.Controls.Add(this.cmdCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLockingProcesses";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Locking Processes - Searching...";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLockingProcesses_FormClosed);
            this.Load += new System.EventHandler(this.frmLockingProcesses_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listViewProcesses;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button cmdCloseSelected;
        private System.Windows.Forms.Label label2;
    }
}