using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.MemoryView.TextObjectModel;
using System.Drawing;
using Utils;
using System.Diagnostics;
using System.ComponentModel;
using Utils.TextObjectModel;
using Utils.MemoryView;
using System.Windows.Forms;

namespace Utils.MemoryView
{
    public delegate void DrawAction(RangePainter painter, Graphics graphics);
    public delegate void BackgroundDrawAction(RangePainter painter, Graphics graphics, Rectangle rect);

    [DebuggerDisplay(" { DebugInfo } ")]
    public class RangePainter : RangeOrientation, INotifyPropertyChanged, INotifyPropertyChanging, ICloneable<RangePainter>
    {
        private Brush backgroundBrush;
        private Brush textBrush;
        private Pen selectionPen;
        private Font font;
        private bool enabled;
        public string Name { get; set; }
        public Rectangle LastBytesSelectionRect { get; set; }
        public Rectangle LastAsciiSelectionRect { get; set; }
        public RangePainterType PainterType { get; set; }
        public event RangePainterHitTestHandler HitTest;
        public bool HandleDragOperation { get; set; }
        public HitTestArea LastHitTestArea { get; set; }
        public object Tag { get; set; } 
        private PainterInfo info;
        private int layoutByteWidth;
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
        public DrawAction Draw { get; set; }
        public BackgroundDrawAction BackgroundDraw { get; set; }
        public event QueryDoDragDropEventHandler QueryDoDragDrop;
        public event DragEventHandler DragEnter;
        public event DragEventHandler DragOver;
        public event EventHandler DragLeave;
        public event GiveFeedbackEventHandler GiveFeedback;
        public event QueryContinueDragEventHandler QueryContinueDrag;
        public event DragEventHandler DragDrop;
        public event DragEventHandler DragDropCanceled;

        public RangePainter(string name, int layoutByteWidth, ITextRange range, Brush backgroundBrush, Brush textBrush, Font font = null, Pen selectionPen = null) : base(range)
        {
            this.Name = name;
            this.BackgroundBrush = backgroundBrush;
            this.TextBrush = textBrush;
            this.Font = font;
            this.SelectionPen = selectionPen;
            this.layoutByteWidth = layoutByteWidth;

            info = new PainterInfo();

            info.hasbb = backgroundBrush != null;
            info.hasf = font != null;
            info.hassp = selectionPen != null;
        }

        public RangePainter(string name, int layoutByteWidth, ITextDocument document, Brush backgroundBrush, Brush textBrush, Font font = null, Pen selectionPen = null) : base(document)
        {
            this.Name = name;
            this.BackgroundBrush = backgroundBrush;
            this.TextBrush = textBrush;
            this.Font = font;
            this.SelectionPen = selectionPen;
            this.layoutByteWidth = layoutByteWidth;

            info = new PainterInfo();

            info.hasbb = backgroundBrush != null;
            info.hasf = font != null;
            info.hassp = selectionPen != null;
        }

        public string DebugInfo
        {
            get
            {
                return string.Format("Name: '{0}', Tag: '{1}', Range: {2}", this.Name, this.Tag.AsDisplayText(), this.TextRange.ToString());
            }
        }

        public bool InRange(int column, int row, Action<PainterInfo, Brush, Brush, Font, Pen> action)
        {
            var startRow = this.StartRow;
            var endRow = this.EndRow;
            var startColumn = this.StartColumn;
            var endColumn = this.EndColumn;

            if (!this.Enabled)
            {
                return false;
            }

            if (endRow > startRow)
            {
                if (row < endRow)
                {
                    endColumn = layoutByteWidth;
                }
                
                if (row > startRow)
                {
                    startColumn = 0;
                }
            }

            if (row.IsBetween(startRow, endRow) && column.IsBetween(startColumn, endColumn))
            {
                action(info, BackgroundBrush, TextBrush, Font, SelectionPen);
                return true;
            }

            return false;
        }

        public bool InRange(int column, int row)
        {
            var startRow = this.StartRow;
            var endRow = this.EndRow;
            var startColumn = this.StartColumn;
            var endColumn = this.EndColumn;

            if (!this.Enabled)
            {
                return false;
            }

            if (endRow > startRow)
            {
                if (row < endRow)
                {
                    endColumn = layoutByteWidth;
                }

                if (row > startRow)
                {
                    startColumn = 0;
                }
            }

            if (row.IsBetween(startRow, endRow) && column.IsBetween(startColumn, endColumn))
            {
                return true;
            }

            return false;
        }

		public Brush BackgroundBrush
		{
			get
			{
				return backgroundBrush;
			}

			set
			{
                using (var notifier = this.Notify(PropertyChanging, PropertyChanged, "BackgroundBrush"))
                {
                    backgroundBrush = value;
                }
			}
		}

		public Brush TextBrush
		{
			get
			{
				return textBrush;
			}

			set
			{
                using (var notifier = this.Notify(PropertyChanging, PropertyChanged, "TextBrush"))
                {
                    textBrush = value;
                }
			}
		}

		public Pen SelectionPen
		{
			get
			{
				return selectionPen;
			}

			set
			{
                using (var notifier = this.Notify(PropertyChanging, PropertyChanged, "SelectionPen"))
                {
                    selectionPen = value;
                }
			}
		}

		public Font Font
		{
			get
			{
				return font;
			}

			set
			{
                using (var notifier = this.Notify(PropertyChanging, PropertyChanged, "Font"))
                {
                    font = value;
                }
			}
		}

		public bool Enabled
		{
			get
			{
				return enabled;
			}

			set
			{
                using (var notifier = this.Notify(PropertyChanging, PropertyChanged, "Enabled"))
                {
                    enabled = value;
                }
			}
		}

        public RangePainter DefaultClone()
        {
            return this.DeepClone();
        }

        public RangePainter ShallowClone()
        {
            return this.DeepClone();
        }

        public RangePainter DeepClone()
        {
            var range = this.TextRange.DeepClone();
            var painter = new RangePainter(string.Empty, layoutByteWidth, range, this.BackgroundBrush, this.TextBrush, this.Font, this.SelectionPen);

            painter.LastBytesSelectionRect = painter.Rect;
            painter.PainterType = RangePainterType.OwnerDrawn;

            return painter;
        }

        internal void RaiseDragEnter(object sender, DragEventArgs e)
        {
            if (DragEnter != null)
            {
                DragEnter(sender, e);
            }
        }

        internal void RaiseDragOver(object sender, DragEventArgs e)
        {
            if (DragOver != null)
            {
                DragOver(sender, e);
            }
        }

        internal void RaiseDragLeave(object sender, EventArgs e)
        {
            if (DragLeave != null)
            {
                DragLeave(sender, e);
            }
        }

        internal void RaiseGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (GiveFeedback != null)
            {
                GiveFeedback(sender, e);
            }
        }

        internal void RaiseQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (QueryContinueDrag != null)
            {
                QueryContinueDrag(sender, e);
            }
        }

        internal void RaiseDragDrop(object sender, DragEventArgs e)
        {
            if (DragDrop != null)
            {
                DragDrop(sender, e);
            }
        }

        internal void RaiseDragDropCanceled(object sender, DragEventArgs e)
        {
            if (DragDropCanceled != null)
            {
                DragDropCanceled(sender, e);
            }
        }

        internal void RaiseHitTest(RangePainterHitTestEventArgs e)
        {
            if (HitTest != null)
            {
                HitTest(this, e);
            }
        }

        internal void RaiseQueryDoDragDrop(QueryDoDragDropEventArgs e)
        {
            if (QueryDoDragDrop != null)
            {
                QueryDoDragDrop(this, e);
            }
        }
    }
}
