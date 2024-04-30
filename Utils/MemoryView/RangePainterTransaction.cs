using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.TextObjectModel;
using System.Diagnostics;

namespace Utils.MemoryView
{
    public class RangePainterTransaction : BaseList<RangePainter>
    {
        public MemoryView MemoryView { get; set; }
        public RangePainter Painter { get; set; }
        private ITextRange textRangeClone;
        private Dictionary<RangePainter, ITextRange> childClones;
        private List<IDisposable> suppressions;

        internal RangePainterTransaction(MemoryView view, RangePainter painter)
        {
            this.MemoryView = view;
            this.Painter = painter;
            suppressions = new List<IDisposable>();

            textRangeClone = this.Painter.TextRange.DeepClone();
            childClones = new Dictionary<RangePainter, ITextRange>();
        }
    
        public void Commit()
        {
            this.MemoryView.CommitPainter(this.Painter);

            foreach (var painter in this)
            {
                this.MemoryView.CommitPainter(painter);
            }
        }

        public IDisposable SuppressUpdates()
        {
            suppressions.Add(this.Painter.TextRange.SuppressUpdate());

            foreach (var painter in this)
            {
                suppressions.Add(painter.TextRange.SuppressUpdate());
            }

            return this.CreateDisposable(() => 
            {
                foreach (var suppression in suppressions)
                {
                    suppression.Dispose();
                }

                this.MemoryView.Refresh();
            });
        }

        public void Rollback()
        {
            Painter.TextRange.Start = textRangeClone.Start;
            Painter.TextRange.End = textRangeClone.End;
            Painter.TextRange.Row = textRangeClone.Row;
            Painter.TextRange.Column = textRangeClone.Column;
            Painter.TextRange.EndRow = textRangeClone.EndRow;

            this.MemoryView.RollbackPainter(this.Painter);

            foreach (var painter in this)
            {
                var clone = childClones[painter];

                painter.TextRange.Start = clone.Start;
                painter.TextRange.End = clone.End;
                painter.TextRange.Row = clone.Row;
                painter.TextRange.Column = clone.Column;
                painter.TextRange.EndRow = clone.EndRow;

                this.MemoryView.RollbackPainter(painter);
            }
        }

        public override void AddRange(IEnumerable<RangePainter> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public override void Add(RangePainter item)
        {
            childClones.Add(item, item.TextRange.DeepClone());

            Debug.Assert(item != Painter, "Cannot add painter twice to the same transaction");

            base.Add(item);
        }
    }
}
