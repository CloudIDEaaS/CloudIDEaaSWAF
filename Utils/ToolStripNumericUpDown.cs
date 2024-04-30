using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public partial class ToolStripNumericUpDown : ToolStripControlHost
    {
        private NumericUpDown numericUpDown;

        public ToolStripNumericUpDown() : base(new NumericUpDown(), "ToolStripNumericUpDown")
        {
            numericUpDown = (NumericUpDown) this.Control;
        }

        public event EventHandler ValueChanged
        {
            add
            {
                numericUpDown.ValueChanged += value;
            }

            remove
            {
                numericUpDown.ValueChanged -= value;
            }
        }

        public decimal Value
        {
            get
            {
                return numericUpDown.Value;
            }

            set
            {
                numericUpDown.Value = value;
            }
        }
    }
}
