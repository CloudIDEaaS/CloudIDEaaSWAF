using Utils;
namespace Utils.MemoryView
{
    partial class MemoryView
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
            this.vScrollBar = new Utils.EnhancedVScrollBar();
            this.memoryClientView = new Utils.DoubleBufferedPanel();
            this.SuspendLayout();
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Location = new System.Drawing.Point(1112, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 918);
            this.vScrollBar.TabIndex = 0;
            this.vScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar_Scroll);
            this.vScrollBar.ValueChanged += new System.EventHandler(this.vScrollBar_ValueChanged);
            // 
            // memoryClientView
            // 
            this.memoryClientView.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.memoryClientView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.memoryClientView.Location = new System.Drawing.Point(0, 0);
            this.memoryClientView.Margin = new System.Windows.Forms.Padding(3, 3, 3, 14);
            this.memoryClientView.Name = "memoryClientView";
            this.memoryClientView.Size = new System.Drawing.Size(1112, 918);
            this.memoryClientView.TabIndex = 2;
            this.memoryClientView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ClientView_MouseDown);
            this.memoryClientView.MouseLeave += new System.EventHandler(this.ClientView_MouseLeave);
            this.memoryClientView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ClientView_MouseMove);
            this.memoryClientView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ClientView_MouseUp);
            // 
            // MemoryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.memoryClientView);
            this.Controls.Add(this.vScrollBar);
            this.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.Name = "MemoryView";
            this.Size = new System.Drawing.Size(1129, 918);
            this.ResumeLayout(false);

        }

        #endregion

        private EnhancedVScrollBar vScrollBar;
        private DoubleBufferedPanel memoryClientView;
    }
}
