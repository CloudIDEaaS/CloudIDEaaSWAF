using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.MemoryView.TextObjectModel;
using System.Drawing;
using Utils.TextObjectModel;
using System.Collections.Specialized;

namespace Utils.MemoryView
{
    public class RangeOrientation
    {
        protected ITextRange textRange;
        public ITextDocument Document { get; private set; }
        public BaseList<DrawRect> DrawRects { get; private set; }
        public event EventHandler OrientationChanged;

        public RangeOrientation(ITextDocument document)
        {
            this.Document = document;
            this.DrawRects = new BaseList<DrawRect>();

            ((INotifyCollectionChanged) this.DrawRects).CollectionChanged += new NotifyCollectionChangedEventHandler(RangeOrientation_CollectionChanged);
        }

        public RangeOrientation(ITextRange range)
        {
            this.textRange = range;
            this.DrawRects = new BaseList<DrawRect>();
        }

        private void RangeOrientation_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (OrientationChanged != null)
            {
                OrientationChanged(this, EventArgs.Empty);
            }
        }

        public ITextRange TextRange
        {
            get
            {
                if (Document != null)
                {
                    return Document.Selection;
                }
                else
                {
                    return textRange;
                }
            }
        }

        public bool IsReversed
        {
            get
            {
                return TextRange.Start > TextRange.End;
            }
        }

        public bool IsCollapsed
        {
            get
            {
                return TextRange.Start == TextRange.End;
            }
        }

        public Rectangle Rect
        {
            get
            {
                var rect = TextRange.Rect;

                if (rect.Width < 0)
                {
                    rect.Width = Math.Abs(rect.Width);
                    rect.X -= rect.Width;
                }

                if (rect.Height < 0)
                {
                    rect.Height = Math.Abs(rect.Height);
                    rect.Y -= rect.Height;
                }

                return rect;
            }
        }

        public int Start
        {
            get
            {
                if (TextRange.Start >= TextRange.End)
                {
                    return TextRange.End - 1;
                }
                else
                {
                    return TextRange.Start;
                }
            }
        }

        public int End
        {
            get
            {
                if (TextRange.Start >= TextRange.End)
                {
                    return TextRange.Start + 1;
                }
                else
                {
                    return TextRange.End;
                }
            }
        }

        public int StartRow
        {
            get
            {
                if (TextRange.Start >= TextRange.End)
                {
                    return this.TextRange.EndRow;
                }
                else
                {
                    return this.TextRange.Row;
                }
            }
        }

        public int StartColumn
        {
            get
            {
                if (TextRange.Start >= TextRange.End)
                {
                    return this.TextRange.EndColumn;
                }
                else
                {
                    return this.TextRange.Column;
                }
            }
        }

        public int EndRow
        {
            get
            {
                if (TextRange.Start >= TextRange.End)
                {
                    return this.TextRange.Row;
                }
                else
                {
                    return this.TextRange.EndRow;
                }
            }
        }

        public int EndColumn
        {
            get
            {
                if (TextRange.Start > TextRange.End)
                {
                    return TextRange.Column;
                }
                else
                {
                    return this.TextRange.EndColumn;
                }
            }
        }
    }
}
