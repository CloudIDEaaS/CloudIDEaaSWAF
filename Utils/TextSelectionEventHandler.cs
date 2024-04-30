using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public delegate void TextSelectionEventHandler(object sender, TextSelectionEventArgs e);

    public class TextSelectionEventArgs
    {
        public int SelectionStart { get; }
        public int SelectionLength { get; }
        public string SelectionText { get; }

        public TextSelectionEventArgs(int selectionStart, int selectionLength, string selectionText)
        {
            this.SelectionStart = selectionStart;
            this.SelectionLength = selectionLength;
            this.SelectionText = selectionText;
        }

        public int SelectionEnd
        {
            get
            {
                return this.SelectionStart + this.SelectionEnd;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this.SelectionLength == 0;
            }
        }
    }
}
