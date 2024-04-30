using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.TextEditElements
{
    public class Selection : TextElement
    {
        public int Start { get; set; }
        public int Length { get; set; }
        private TextEdit textEdit;

        public Selection(TextEdit textEdit)
        {
            this.textEdit = textEdit;
        }

        public bool IsCollapsed
        {
            get
            {
                return this.Length == 0;
            }
        }

        public int End
        {
            get
            {
                return this.Start + this.Length;
            }
        }
    }
}
