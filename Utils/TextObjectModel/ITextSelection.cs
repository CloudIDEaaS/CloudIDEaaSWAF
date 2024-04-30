using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Utils;

namespace Utils.TextObjectModel
{
    public interface ITextSelection : ITextRange
    {
        object GetEmbeddedObject();
        TextSelectionFlags Flags { set; }
        TextSelectionType Type { get; }
        int MoveLeft(int extend);
        int MoveRight(int extend);
        int MoveUp(int extend);
        int MoveDown(int extend);
        int HomeKey(int extend);
        int EndKey(int extend);
        void TypeText(string bstr);
    }
}
