using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Utils;

namespace Utils.TextObjectModel
{
    public interface ITextRange : ICloneable<ITextRange>
    {
        event EventHandlerT<ITextSelection> SelectionChanged;
        event EventHandlerT<ITextRange> RangeChanged;
        event EventHandler<GetRectEventArgs> OnGetRect;
        event EventHandler<GetTextEventArgs> OnGetText;
        IDisposable SuppressUpdate();
        string Text { get; set; }
        void Clear();
        bool Reversed { get; set; }
        char Char { set; }
        Rectangle Rect { get; }
        ITextRange Duplicate { get; }
        ITextRange FormattedText { set; }
        int Start { get; set; }
        int End { get; set; }
        int Row { get; set; }
        int Column { get; set; }
        int EndRow { get; set; }
        int EndColumn { get; set; }
        Font Font { set; }
        ITextParagraph Para { set; }
        int StoryLength { get; }
        StoryType StoryType { get; }
        void Collapse(int bStart);
        int Expand(int unit);
        int GetIndex(int unit);
        void SetIndex(int extend);
        void SetRange(int cpOther);
        int InRange(ITextRange pRange);
        int InStory(ITextRange pRange);
        int IsEqual(ITextRange pRange);
        void Select();
        int StartOf(int extend);
        int EndOf(int extend);
        int Move(int count);
        int MoveStart(int count);
        int MoveEnd(int count);
        int MoveWhile(int count);
        int MoveStartWhile(int count);
        int MoveEndWhile(int count);
        int MoveUntil(int count);
        int MoveStartUntil(int count);
        int MoveEndUntil(int count);
        int FindText(int flags);
        int FindTextStart(int flags);
        int FindTextEnd(int flags);
        int Delete(int count);
        void Cut(out object pVar);
        void Copy(out object pVar);
        void Paste(int format);
        int CanPaste(int format);
        int CanEdit();
        void ChangeCase(int Type);
        void GetPoint(PointType type, out int px, out int py);
        void SetPoint(int x, int y, PointType type, int extend);
        void ScrollIntoView(int Value);
        object GetEmbeddedObject();
        void AttachDocumentHandlers(ITextDocument document);
        bool HasOnGetHandlers { get; }
    }
}
