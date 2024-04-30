using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.TextObjectModel;
using System.Drawing;
using Utils;
using System.Diagnostics;

namespace Utils.TextObjectModel
{
    [DebuggerDisplay(" { DebugInfo } ")]
    public class TextRange : ITextRange
    {
        internal string NamedRange { get; set; }
        private int start = -1;
        private int end = -1;
        private int row = -1;
        private int column = -1;
        private int endRow = -1;
        private int endColumn = -1;
        private bool reversed;
        private ITextDocument document;
        public event EventHandlerT<ITextSelection> SelectionChanged;
        public event EventHandlerT<ITextRange> RangeChanged;
        public event EventHandler<GetRectEventArgs> OnGetRect;
        public event EventHandler<GetTextEventArgs> OnGetText;
        private bool suppressEvents;

        public TextRange()
        {
        }

        internal TextRange(ITextDocument document)
        {
            this.document = document;
        }

        internal TextRange(int start, int end, int startColumn, int startRow, int endColumn, int endRow)
        {
            this.start = start;
            this.end = end;
            this.column = startColumn;
            this.row = startRow;
            this.endColumn = endColumn;
            this.endRow = endRow;
        }

        public TextRange(string namedRange)
        {
            this.NamedRange = namedRange;
        }

        internal IEnumerable<EventHandlerT<ITextSelection>> SelectionChangedInvocationList
        {
            get
            {
                return this.SelectionChanged.GetInvocationList().Select(d => (EventHandlerT<ITextSelection>) d);
            }
        }

        internal IEnumerable<EventHandler<GetRectEventArgs>> OnGetRectInvocationList
        {
            get
            {
                return this.OnGetRect.GetInvocationList().Select(d => (EventHandler<GetRectEventArgs>)d);
            }
        }

        internal IEnumerable<EventHandler<GetTextEventArgs>> OnGetTextInvocationList
        {
            get
            {
                return this.OnGetText.GetInvocationList().Select(d => (EventHandler<GetTextEventArgs>)d);
            }
        }

        public string DebugInfo
        {
            get
            {
                return string.Format(
        			"Start: {0}, "
        			+ "End: {1}, "
        			+ "Row: {2}, "
        			+ "Column: {3}, "
        			+ "EndRow: {4}, "
        			+ "EndColumn: {5}, "
        			+ "Reversed: {6}, "
                    + "Text: '{7}'",
                    this.start,
        			this.end,
        			this.row,
        			this.column,
        			this.endRow,
        			this.endColumn,
        			this.reversed,
                    this.Text);
            }
        }

        public override string ToString()
        {
            return this.DebugInfo;
        }

        public string Text
        {
            set 
            {
                throw new NotImplementedException(); 
            }

            get
            {
                var args = new GetTextEventArgs(this);

                if (OnGetText != null)
                {
                    OnGetText(document, args);
                }

                return args.Text;
            }
        }

        public char Char
        {
            set { throw new NotImplementedException(); }
        }

        public ITextRange Duplicate
        {
            get 
            {
                if (this is ITextSelection)
                {
                    var selection = new TextSelection
                    {
                        start = this.start,
                        end = this.end,
                        row = this.row,
                        endRow = this.EndRow,
                        column = this.column,
                        endColumn = this.endColumn
                    };

                    return selection;
                }
                else
                {
                    var range = new TextRange
                    {
                        start = this.start,
                        end = this.end,
                        row = this.row,
                        endRow = this.EndRow,
                        column = this.column,
                        endColumn = this.endColumn
                    };

                    return range;
                }
            }
        }

        public ITextRange FormattedText
        {
            set { throw new NotImplementedException(); }
        }

        private bool PropertiesValid
        {
            get
            {
                return start != -1 && end != -1 && column != -1 && row != -1 && endColumn != -1 && endRow != -1;
            }
        }

        public int Start
        {
            set 
            {
                var previousValue = start;

                start = value;

                if (start != previousValue && this.PropertiesValid && !suppressEvents)
                {
                    if (this is ITextSelection)
                    {
                        SelectionChanged(document, new EventArgs<ITextSelection>((ITextSelection)this));
                    }
                    else if (RangeChanged != null)
                    {
                        RangeChanged(document, new EventArgs<ITextRange>(this));
                    }
                }
            }

            get
            {
                return start;
            }
        }

        public int End
        {
            set 
            {
                var previousValue = end;

                end = value;

                if (end != previousValue && this.PropertiesValid && !suppressEvents)
                {
                    if (this is ITextSelection)
                    {
                        SelectionChanged(document, new EventArgs<ITextSelection>((ITextSelection)this));
                    }
                    else if (RangeChanged != null)
                    {
                        RangeChanged(document, new EventArgs<ITextRange>(this));
                    }
                }
            }

            get
            {
                return end;
            }
        }

        public Font Font
        {
            set { throw new NotImplementedException(); }
        }

        public ITextParagraph Para
        {
            set { throw new NotImplementedException(); }
        }

        public int StoryLength
        {
            get { throw new NotImplementedException(); }
        }

        public StoryType StoryType
        {
            get { throw new NotImplementedException(); }
        }

        public void Collapse(int bStart)
        {
            throw new NotImplementedException();
        }

        public int Expand(int unit)
        {
            throw new NotImplementedException();
        }

        public int GetIndex(int unit)
        {
            throw new NotImplementedException();
        }

        public void SetIndex(int extend)
        {
            throw new NotImplementedException();
        }

        public void SetRange(int cpOther)
        {
            throw new NotImplementedException();
        }

        public int InRange(ITextRange pRange)
        {
            throw new NotImplementedException();
        }

        public int InStory(ITextRange pRange)
        {
            throw new NotImplementedException();
        }

        public int IsEqual(ITextRange pRange)
        {
            throw new NotImplementedException();
        }

        public void Select()
        {
            throw new NotImplementedException();
        }

        public int StartOf(int extend)
        {
            throw new NotImplementedException();
        }

        public int EndOf(int extend)
        {
            throw new NotImplementedException();
        }

        public IDisposable SuppressUpdate()
        {
            suppressEvents = true;

            return this.CreateDisposable(() => suppressEvents = false);
        }

        public int Move(int count)
        {
            using (this.SuppressUpdate())
            {
                this.End += count;
            }

            this.Start += count;

            return 0;
        }

        public int MoveStart(int count)
        {
            this.Start += count;
            return 0;
        }

        public int MoveEnd(int count)
        {
            this.End += count;
            return 0;
        }

        public int MoveWhile(int count)
        {
            throw new NotImplementedException();
        }

        public int MoveStartWhile(int count)
        {
            throw new NotImplementedException();
        }

        public int MoveEndWhile(int count)
        {
            throw new NotImplementedException();
        }

        public int MoveUntil(int count)
        {
            throw new NotImplementedException();
        }

        public int MoveStartUntil(int count)
        {
            throw new NotImplementedException();
        }

        public int MoveEndUntil(int count)
        {
            throw new NotImplementedException();
        }

        public int FindText(int flags)
        {
            throw new NotImplementedException();
        }

        public int FindTextStart(int flags)
        {
            throw new NotImplementedException();
        }

        public int FindTextEnd(int flags)
        {
            throw new NotImplementedException();
        }

        public int Delete(int count)
        {
            throw new NotImplementedException();
        }

        public void Cut(out object pVar)
        {
            throw new NotImplementedException();
        }

        public void Copy(out object pVar)
        {
            throw new NotImplementedException();
        }

        public void Paste(int format)
        {
            throw new NotImplementedException();
        }

        public int CanPaste(int format)
        {
            throw new NotImplementedException();
        }

        public int CanEdit()
        {
            throw new NotImplementedException();
        }

        public void ChangeCase(int Type)
        {
            throw new NotImplementedException();
        }

        public void GetPoint(PointType type, out int px, out int py)
        {
            throw new NotImplementedException();
        }

        public void SetPoint(int x, int y, PointType type, int extend)
        {
            throw new NotImplementedException();
        }

        public void ScrollIntoView(int Value)
        {
            throw new NotImplementedException();
        }

        public object GetEmbeddedObject()
        {
            throw new NotImplementedException();
        }

        public int Row
        {
            get
            {
                return row;
            }
            set
            {
                var previousValue = row;

                row = value;

                if (row != previousValue && this.PropertiesValid && this is ITextSelection)
                {
                    SelectionChanged(document, new EventArgs<ITextSelection>((ITextSelection)this));
                }
            }
        }

        public int Column
        {
            get
            {
                return column;
            }

            set
            {
                var previousValue = column;

                column = value;

                if (column != previousValue && this.PropertiesValid && this is ITextSelection)
                {
                    SelectionChanged(document, new EventArgs<ITextSelection>((ITextSelection)this));
                }
            }
        }

        public Rectangle Rect
        {
            get 
            {
                if (Cleared)
                {
                    return Rectangle.Empty;
                }
                else
                {
                    var args = new GetRectEventArgs(this);

                    OnGetRect(this.document, args);

                    return args.Rect;
                }
            }
        }

        private bool Cleared
        {
            get
            {
                return start == -1 &&
                    end == -1 &&
                    row == -1 &&
                    column == -1 &&
                    endRow == -1 &&
                    endColumn == -1;
            }
        }

        public int EndRow
        {
            get
            {
                return endRow;
            }

            set
            {
                var previousValue = endRow;

                endRow = value;

                if (endRow != previousValue && this.PropertiesValid && this is ITextSelection)
                {
                    SelectionChanged(document, new EventArgs<ITextSelection>((ITextSelection)this));
                }
            }
        }

        public int EndColumn
        {
            get
            {
                return endColumn;
            }

            set
            {
                var previousValue = endColumn;

                endColumn = value;

                if (endColumn != previousValue && this.PropertiesValid && this is ITextSelection)
                {
                    SelectionChanged(document, new EventArgs<ITextSelection>((ITextSelection)this));
                }
            }
        }

        public bool Reversed
        {
            set 
            {
                reversed = value;
            }

            get
            {
                return reversed;
            }
        }

        public virtual void Clear()
        {
            start = -1;
            end = -1;
            row = -1;
            column = -1;
            endRow = -1;
            endColumn = -1;

            SelectionChanged(document, new EventArgs<ITextSelection>(null));
        }

        public ITextRange ShallowClone()
        {
            return this.DeepClone();
        }

        public ITextRange DefaultClone()
        {
            return this.DeepClone();
        }

        public ITextRange DeepClone()
        {
            return new TextRange
            {
                Start = this.Start,
                End = this.End,
                Row = this.Row,
                Column = this.Column,
                EndRow = this.EndRow,
                EndColumn = this.EndColumn,
                OnGetRect = this.OnGetRect,
                OnGetText = this.OnGetText
            };
        }

        public void AttachDocumentHandlers(ITextDocument document)
        {
            document.ApplyRange(this);
        }

        public bool HasOnGetHandlers
        {
            get 
            {
                return OnGetText != null && OnGetRect != null;
            }
        }
    }
}
