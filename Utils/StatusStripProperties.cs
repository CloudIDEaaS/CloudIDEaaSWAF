using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Utils
{
    public class StatusStripProperties
    {
        public Stack<StatusEntry> StatusStack { get; private set; }
        public Queue<StatusEntry> PendingPopStatusQueue { get; private set; }
        public Color StatusStripDefaultBackColor { get; set; }
        public Color StatusStripDefaultForeColor { get; set; }
        public ToolStripStatusLabel StatusLabel { get; set; }
        public ToolStripProgressBar ProgressBar { get; set; }
        public bool CancelTempStatus { get; set; }
        public bool PendingTempStatus { get; set; }
        public static Dictionary<StatusStrip, StatusStripProperties> RegisteredStatusStrips { get; private set; }

        static StatusStripProperties()
        {
            RegisteredStatusStrips = new Dictionary<StatusStrip, StatusStripProperties>();
        }

        public StatusStripProperties(StatusStrip statusStrip)
        {
            this.StatusStack = new Stack<StatusEntry>();
            this.PendingPopStatusQueue = new Queue<StatusEntry>();

            this.StatusStripDefaultBackColor = statusStrip.BackColor;
            this.StatusStripDefaultForeColor = statusStrip.ForeColor;

            this.StatusLabel = statusStrip.Items.OfType<ToolStripStatusLabel>().FirstOrDefault();
            this.ProgressBar = statusStrip.Items.OfType<ToolStripProgressBar>().FirstOrDefault();
        }

        public StatusStripProperties(StatusStrip statusStrip, ToolStripProgressBar progressBar)
        {
            this.StatusStack = new Stack<StatusEntry>();
            this.PendingPopStatusQueue = new Queue<StatusEntry>();

            this.StatusStripDefaultBackColor = statusStrip.BackColor;
            this.StatusStripDefaultForeColor = statusStrip.ForeColor;

            this.StatusLabel = statusStrip.Items.OfType<ToolStripStatusLabel>().FirstOrDefault();
            this.ProgressBar = progressBar;
        }

        public StatusStripProperties(StatusStrip statusStrip, ToolStripStatusLabel statusLabel)
        {
            this.StatusStack = new Stack<StatusEntry>();
            this.PendingPopStatusQueue = new Queue<StatusEntry>();

            this.StatusStripDefaultBackColor = statusStrip.BackColor;
            this.StatusStripDefaultForeColor = statusStrip.ForeColor;

            this.StatusLabel = statusLabel;
            this.ProgressBar = statusStrip.Items.OfType<ToolStripProgressBar>().FirstOrDefault();
        }

        public StatusStripProperties(StatusStrip statusStrip, ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel)
        {
            this.StatusStack = new Stack<StatusEntry>();
            this.PendingPopStatusQueue = new Queue<StatusEntry>();

            this.StatusStripDefaultBackColor = statusStrip.BackColor;
            this.StatusStripDefaultForeColor = statusStrip.ForeColor;

            this.StatusLabel = statusLabel;
            this.ProgressBar = progressBar;
        }

        public void CheckColors()
        {
            if (this.StatusLabel.ForeColor != this.StatusStripDefaultForeColor)
            {
                this.StatusLabel.ForeColor = this.StatusStripDefaultForeColor;
            }

            if (this.StatusLabel.BackColor != this.StatusStripDefaultBackColor)
            {
                this.StatusLabel.BackColor = this.StatusStripDefaultBackColor;
            }
        }
    }
}
