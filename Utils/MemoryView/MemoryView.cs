using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using Utils;
using Utils.MemoryView.TextObjectModel;
using System.Diagnostics;
using System.Collections.Specialized;
using Utils.TextObjectModel;

namespace Utils.MemoryView
{
    public partial class MemoryView : UserControl, IStatusControl
    {
        private const bool DEBUG = false;
        public const int DEFAULT_LAYOUT_BYTE_WIDTH = 16;
        private Stream stream;
        private int rowHeight;
        private int leftMarginWidth = 75;
        private int asciiWidth = 150;
        private int layoutByteWidth = DEFAULT_LAYOUT_BYTE_WIDTH;
        private int byteCellWidth = 28;
        private int asciiCellWidth = 8;
        private int asciiBorder = 10;
        private int oldScrollBarValue = 0;
        private BytesDocument bytesDocument;
        private AsciiDocument asciiDocument;
        private Rectangle bytesRect;
        private Rectangle asciiRect;
        private IMemoryTextDocument mouseDownDocument;
        private Dictionary<string, RangePainter> painters;
        private RangePainterList rangePainters;                 // simply a wrapper around painters
        public event EventHandler<StatusEventArgs> OnStatus;
        private Point mouseDownPoint;
        private RangePainter mouseDownPainter;
        private string currentDragDropFormat;
        private List<RangePainter> hitTestRangePainters;
        private RangePainter currentDragDropPainter;
        private RangePainterDragDropDataObject currentRangePainterDataObject;
        private RangePainterTransaction openTransaction;
        public StoryRanges BackgroundPainters { get; private set; }
        public ITextSelection Selection { get; private set; }
        public event EventHandler TextSelectionChanged;

        public MemoryView()
        {
            var designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

            SetStyle(ControlStyles.Selectable, true);

            InitializeComponent();
            hitTestRangePainters = new List<RangePainter>();

            memoryClientView.Selectable = true;

            this.RowHeight = 14;
            this.LayoutByteWidth = 16;

            if (!designMode)
            {
                InitializeView();
            }
        }

        private void InitializeView()
        {
            RangePainter selectionPainter;

            memoryClientView.AllowDrop = true;
            memoryClientView.Paint += ClientView_Paint;
            memoryClientView.SizeChanged += ClientView_SizeChanged;
            memoryClientView.MouseDown += ClientView_MouseDown;
            memoryClientView.DragEnter += ClientView_DragEnter;
            memoryClientView.DragOver += ClientView_DragOver;
            memoryClientView.DragLeave += ClientView_DragLeave;
            memoryClientView.GiveFeedback += ClientView_GiveFeedback;
            memoryClientView.QueryContinueDrag += ClientView_QueryContinueDrag;
            memoryClientView.DragDrop += ClientView_DragDrop;

            bytesDocument = new BytesDocument();
            asciiDocument = new AsciiDocument();

            painters = new Dictionary<string, RangePainter>();
            rangePainters = new RangePainterList(painters);

            selectionPainter = new RangePainter("SelectionPainter", layoutByteWidth, bytesDocument, new SolidBrush(Color.Black), new SolidBrush(Color.White));

            selectionPainter.Enabled = true;

            rangePainters.Add(selectionPainter);
            rangePainters.LockFromClear("SelectionPainter");

            rangePainters.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(RangePaintersCollectionChanged);

            bytesDocument.SelectionChanged += new EventHandlerT<ITextSelection>(SelectionChanged);
            asciiDocument.SelectionChanged += new EventHandlerT<ITextSelection>(SelectionChanged);

            bytesDocument.OnGetRect += new EventHandler<GetRectEventArgs>(OnGetRect);
            asciiDocument.OnGetRect += new EventHandler<GetRectEventArgs>(OnGetRect);

            bytesDocument.OnGetText += new EventHandler<GetTextEventArgs>(OnGetText);
            asciiDocument.OnGetText += new EventHandler<GetTextEventArgs>(OnGetText);

            this.MouseWheel += new MouseEventHandler(MemoryView_MouseWheel);
        }

        private void RangePaintersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    foreach (var painter in e.NewItems.Cast<RangePainter>())
                    {
                        painter.PropertyChanged += (sender2, e2) =>
                        {
                            HandlePainterChange(painter);
                        };

                        HandlePainterChange(painter);
                    }

                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    memoryClientView.Invalidate();
                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    memoryClientView.Invalidate();
                    break;
                }
            }
        }

        public BytesDocument BytesDocument
        {
            get 
            {
                return bytesDocument; 
            }
        }

        public AsciiDocument AsciiDocument
        {
            get 
            {
                return asciiDocument; 
            }
        }

        public RangePainterList RangePainters
        {
            get 
            {
                return rangePainters; 
            }
        }

        public Control ClientView
        {
            get
            {
                return memoryClientView;
            }
        }

        public RangePainter SelectionPainter
        {
            get
            {
                return painters["SelectionPainter"];
            }
        }

        public EnhancedVScrollBar VScrollBar
        {
            get
            {
                return vScrollBar;
            }
        }

        private void MemoryView_MouseWheel(object sender, MouseEventArgs e)
        {
            var delta = -(e.Delta / 60);
            var value = vScrollBar.Value + delta;

            vScrollBar.Value = Math.Min(Math.Max(value, 0), vScrollBar.Maximum);
        }

        private void ClientView_SizeChanged(object sender, EventArgs e)
        {
            CalculateScroll();
        }

        public int LeftMarginWidth
        {
            get
            {
                return leftMarginWidth;
            }
        }

        public int RowHeight
        {
            get 
            { 
                return rowHeight; 
            }
            
            set 
            {
                rowHeight = value;
                CalculateScroll();
            }
        }

        private int HeaderRowCount
        {
            get
            {
                return 1;
            }
        }

        private void CalculateScroll()
        {
            if (stream != null)
            {
                vScrollBar.Maximum = this.TotalByteRows;
                vScrollBar.LargeChange = this.rowHeight * 3;
                vScrollBar.SmallChange = this.rowHeight;
            }
            else
            {
                vScrollBar.Maximum = 0;
            }

            oldScrollBarValue = vScrollBar.Value;
        }

        public int TotalByteRows
        {
            get
            {
                if (stream != null)
                {
                    var bytes = stream.Length;
                    var rows = (int) bytes / layoutByteWidth;

                    return rows - this.HeaderRowCount;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int VisibleByteRows
        {
            get
            {
                return (int)((memoryClientView.Height.As<float>() / this.rowHeight.As<float>()) - this.HeaderRowCount); 
            }
        }

        public Stream Stream
        {
            get 
            { 
                return stream; 
            }
            
            set 
            {
                stream = value;

                if (stream == null)
                {
                    this.RecreateHandle();
                }
                else
                {
                    bytesDocument.Read(stream, layoutByteWidth);
                    asciiDocument.Read(stream, layoutByteWidth);
                }
            }
        }

        private void ClientView_Paint(object sender, PaintEventArgs e)
        {
            using (var graphics = e.Graphics)
            {
                var startByteCellX = leftMarginWidth;
                var startAsciiCellX = memoryClientView.Width - asciiWidth;
                var linePen = SystemPens.WindowText;
                var font = this.Font;
                var row = vScrollBar.Value;
                int offsetStart;
                var backgroundBrush = SystemBrushes.Window;
                var textBrush = SystemBrushes.WindowText;
                var textBrushSelected = SystemBrushes.Window;
                var headerBrush = new SolidBrush(Color.Blue);
                var ownerDrawnPainters = new Dictionary<RangePainter, List<DrawRect>>();
                Rectangle textRect;
                string text;
                var lines = new List<Line>()
                {
                    new Line(0, this.RowHeight, memoryClientView.Width, this.RowHeight),    // vertical top bar
                    new Line(leftMarginWidth, 0, leftMarginWidth, memoryClientView.Height), // offset bar
                    new Line(startAsciiCellX, 0, startAsciiCellX, memoryClientView.Height), // ascii bar
                };

                if (bytesRect.IsEmpty)
                {
                    bytesRect = new Rectangle(startByteCellX, this.RowHeight + this.HeaderRowCount, startAsciiCellX - startByteCellX - 1, memoryClientView.Height);
                    asciiRect = new Rectangle(startAsciiCellX + asciiBorder, this.RowHeight + this.HeaderRowCount, asciiWidth - asciiBorder, memoryClientView.Height);
                }

                graphics.FillRectangle(backgroundBrush, memoryClientView.Bounds);

                // draw lines

                foreach (var line in lines)
                {
                    graphics.DrawLine(linePen, line.PointStart, line.PointEnd);
                }

                // draw headings

                text = "Offset";
                textRect = new Rectangle(0, 0, startByteCellX, this.RowHeight);

                graphics.DrawString(text, font, headerBrush, textRect, StringAlignment.Center);

                text = "Ascii";
                textRect = new Rectangle(startAsciiCellX, 0, asciiWidth / 2, this.RowHeight);

                graphics.DrawString(text, font, headerBrush, textRect, StringAlignment.Center);

                for (var x = 0; x < layoutByteWidth; x++)
                {
                    text = x.ToString("x1").ToUpper();
                    textRect = new Rectangle(startByteCellX + (byteCellWidth * x), 0, byteCellWidth, this.RowHeight);

                    graphics.DrawString(text, font, headerBrush, textRect, StringAlignment.Center);
                }

                offsetStart = row * layoutByteWidth;

                // draw offsets && bytes

                for (var y = 0; y <= Math.Min(this.VisibleByteRows, this.TotalByteRows - row); y++)
                {
                    var rowY = (y + this.HeaderRowCount) * this.rowHeight;
                    var offset = (offsetStart + y * layoutByteWidth);
                    DrawRect drawRect = null;

                    text = offset.ToString("x8").ToUpper();
                    textRect = new Rectangle(0, rowY, leftMarginWidth, this.RowHeight);

                    graphics.DrawString(text, font, headerBrush, textRect, StringAlignment.Center);

                    if (bytesDocument.Lines.Count > (row + y))
                    {
                        var bytesLine = bytesDocument.Lines[y + row];
                        var asciiLine = asciiDocument.Lines[y + row];

                        for (var x = 0; x < bytesLine.TextItems.Count; x++)
                        {
                            var hasPainter = false;

                            // draw hex

                            text = bytesLine.TextItems[x];
                            textRect = new Rectangle(startByteCellX + (x * byteCellWidth), rowY, byteCellWidth, this.rowHeight);

                            foreach (var painter in painters.Values)
                            {
                                if (painter.PainterType == RangePainterType.OwnerDrawn)
                                {
                                    DrawRect oldRect;

                                    if (painter.InRange(x, y + row))
                                    {
                                        if (!ownerDrawnPainters.ContainsKey(painter))
                                        {
                                            ownerDrawnPainters.AddToDictionaryListCreateIfNotExist(painter, new DrawRect());
                                            drawRect = ownerDrawnPainters[painter].Last();
                                        }
                                        else
                                        {
                                            if (x == 0)
                                            {
                                                ownerDrawnPainters[painter].Add(new DrawRect());
                                                drawRect = ownerDrawnPainters[painter].Last();
                                            }
                                            else
                                            {
                                                drawRect = ownerDrawnPainters[painter].Last();
                                            }
                                        }

                                        oldRect = drawRect;

                                        if (drawRect.IsEmpty)
                                        {
                                            drawRect = textRect;

                                            ownerDrawnPainters.ReplaceInDictionaryListIfExist(painter, oldRect, drawRect);
                                        }
                                        else
                                        {
                                            drawRect = Rectangle.Union(drawRect, textRect);
                                            ownerDrawnPainters.ReplaceInDictionaryListIfExist(painter, oldRect, drawRect);
                                        }

                                        painter.BackgroundDraw(painter, graphics, textRect);
                                    }
                                }
                                else if (painter.InRange(x, y + row, (i, bb, tb, f, p) =>
                                {
                                    if (i.hasbb)
                                    {
                                        graphics.FillRectangle(bb, textRect);
                                    }

                                    f = i.hasf ? f : font;

                                    graphics.DrawString(text, f, tb, textRect, StringAlignment.Center);
                                }))
                                {
                                    hasPainter = true;
                                }
                            }

                            if (!hasPainter)
                            {
                                graphics.DrawString(text, font, textBrush, textRect, StringAlignment.Center);
                            }

                            hasPainter = false;

                            // draw ascii

                            text = asciiLine.TextItems[x];
                            textRect = new Rectangle(startAsciiCellX + asciiBorder + (x * asciiCellWidth), rowY, asciiCellWidth, this.rowHeight);

                            foreach (var painter in painters.Values)
                            {
                                if (painter.InRange(x, y + row, (i, bb, tb, f, p) =>
                                {
                                    if (i.hasbb)
                                    {
                                        var rect = textRect;

                                        rect.Inflate(0, -1);

                                        graphics.FillRectangle(bb, rect);
                                    }

                                    f = i.hasf ? f : font;

                                    graphics.DrawString(text, f, tb, textRect, StringAlignment.Center);
                                }))
                                {
                                    hasPainter = true;
                                }
                            }

                            if (!hasPainter)
                            {
                                graphics.DrawString(text, font, textBrush, textRect, StringAlignment.Center);
                            }
                        }
                    }
                }

                foreach (var ownerDrawnPainter in ownerDrawnPainters.Keys)
                {
                    var rects = ownerDrawnPainters[ownerDrawnPainter];

                    foreach (var rect in rects)
                    {
                        if (!ownerDrawnPainter.DrawRects.Any(r => r.Rectangle.Contains(rect)))
                        {
                            ownerDrawnPainter.DrawRects.Add(rect);
                        }
                    }

                    ownerDrawnPainter.Draw(ownerDrawnPainter, graphics);
                }
            }
        }

        public int LayoutByteWidth
        {
            get 
            { 
                return layoutByteWidth; 
            }

            set 
            {
                layoutByteWidth = value;
                this.Width = leftMarginWidth + (layoutByteWidth * byteCellWidth) + asciiWidth + vScrollBar.Width;
            }
        }

        private void vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            var rect = new Rectangle(new Point(this.HeaderRowCount * this.rowHeight, memoryClientView.Bounds.Top), memoryClientView.Size);

            memoryClientView.Invalidate(rect);
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            var rect = new Rectangle(new Point(this.HeaderRowCount * this.rowHeight, memoryClientView.Bounds.Top), memoryClientView.Size);

            memoryClientView.Invalidate(rect);
        }

        private void OnGetText(object sender, GetTextEventArgs e)
        {
            var range = new RangeOrientation(e.Range);
            var startRow = range.StartRow;
            var endRow = range.EndRow;
            var startColumn = range.StartColumn;
            var endColumn = range.EndColumn;
            var text = string.Empty;
            var rows = Math.Min(bytesDocument.Lines.Count - 1, endRow);

            if (range.StartRow == -1 || range.EndRow == -1)
            {
                return;
            }

            for (var y = startRow; y <= rows; y++)
            {
                var line = bytesDocument.Lines[y];
                var lineText = line.ToString();

                if (endRow > startRow)
                {
                    if (y < endRow)
                    {
                        endColumn = layoutByteWidth - 1;
                    }

                    if (y > startRow)
                    {
                        startColumn = 0;
                    }
                }

                for (var x = startColumn; x <= Math.Min(endColumn, layoutByteWidth); x++)
                {
                    text += line.TextItems[x];
                }
            }

            e.Text = text;
        }

        private void OnGetRect(object sender, GetRectEventArgs e)
        {
            var range = e.Range;
            var selectionPainter = this.SelectionPainter;
            Action<int> getRectAction;

            getRectAction = (cellWidth) =>
            {
                var orientation = new RangeOrientation(range);

                var x = (orientation.EndRow > orientation.StartRow) ? 0 : orientation.StartColumn * cellWidth;
                var y = orientation.StartRow * rowHeight;
                var cx = (orientation.EndRow > orientation.StartRow) ? (layoutByteWidth * cellWidth) : (orientation.EndColumn + 1) * cellWidth;
                var cy = (orientation.EndRow + 1) * rowHeight;

                e.Rect = new Rectangle(x, y, cx - x, cy - y);
            };

            if (sender is BytesDocument)
            {
                getRectAction(byteCellWidth);
            }
            else if (sender is AsciiDocument)
            {
                getRectAction(asciiCellWidth);
            }
            else
            {
                Debugger.Break();
            }
        }

        private void HandlePainterChange(RangePainter rangePainter)
        {
            Action<MemoryTextDocument, Rectangle, Rectangle, Action<Rectangle>> changeAction;

            changeAction = (document, offsetRect, lastRect, setAction) =>
            {
                var scrollY = (vScrollBar.Value + this.HeaderRowCount) * this.rowHeight;

                if (rangePainter.TextRange == null)
                {
                    if (!lastRect.IsEmpty)
                    {
                        memoryClientView.Invalidate(lastRect);
                        memoryClientView.Refresh();
                    }

                    setAction(Rectangle.Empty);
                }
                else
                {
                    var orientation = new RangeOrientation(rangePainter.TextRange);
                    Rectangle rect;
                    Rectangle invalidRect;
                    EventHandler<GetRectEventArgs> onGetRectHandler = null;

                    onGetRectHandler = (sender, e) =>
                    {
                        OnGetRect(document, e);

                        rangePainter.TextRange.OnGetRect -= onGetRectHandler;
                    };

                    rangePainter.TextRange.OnGetRect += onGetRectHandler;

                    if (!rangePainter.TextRange.HasOnGetHandlers)
                    {
                        rangePainter.TextRange.AttachDocumentHandlers(document);
                    }

                    rect = orientation.Rect;

                    rect.Offset(offsetRect.X, offsetRect.Y);

                    if (!lastRect.IsEmpty && !rect.Contains(lastRect))
                    {
                        invalidRect = lastRect;
                        invalidRect.Offset(0, -scrollY + this.RowHeight);

                        memoryClientView.Invalidate(invalidRect);
                    }

                    if (DEBUG)
                    {
                        using (var graphics = memoryClientView.CreateGraphics())
                        {
                            graphics.DrawRectangle(new Pen(Color.Red), rect);
                        }
                    }

                    invalidRect = rect;
                    invalidRect.Offset(0, -scrollY);

                    memoryClientView.Invalidate(invalidRect);

                    setAction(rect);
                }
            };

            changeAction(bytesDocument, bytesRect, rangePainter.LastBytesSelectionRect, (r) => rangePainter.LastBytesSelectionRect = r);
            changeAction(asciiDocument, asciiRect, rangePainter.LastAsciiSelectionRect, (r) => rangePainter.LastAsciiSelectionRect = r);
        }

        private void SelectionChanged(object sender, EventArgs<ITextSelection> e)
        {
            Action<Rectangle, Rectangle, Action<Rectangle>> selectionAction;
            var selectionPainter = this.SelectionPainter;

            selectionAction = (offsetRect, lastRect, setAction) =>
            {
                var scrollY = (vScrollBar.Value + this.HeaderRowCount) * this.rowHeight;

                if (e.Value == null)
                {
                    if (!lastRect.IsEmpty)
                    {
                        memoryClientView.Invalidate(lastRect);
                        memoryClientView.Refresh();
                    }

                    setAction(Rectangle.Empty);
                }
                else
                {
                    var orientation = new RangeOrientation(e.Value);
                    var rect = orientation.Rect;
                    Rectangle invalidRect;

                    rect.Offset(offsetRect.X, offsetRect.Y);

                    if (!lastRect.IsEmpty && !rect.Contains(lastRect))
                    {
                        invalidRect = lastRect;
                        invalidRect.Offset(0, -scrollY);

                        memoryClientView.Invalidate(invalidRect);
                    }

                    if (DEBUG)
                    {
                        using (var graphics = memoryClientView.CreateGraphics())
                        {
                            graphics.DrawRectangle(new Pen(Color.Red), rect);
                        }
                    }

                    invalidRect = rect;
                    invalidRect.Offset(0, -scrollY);

                    memoryClientView.Invalidate(invalidRect);

                    setAction(rect);
                }
            };

            if (sender is BytesDocument)
            {
                selectionAction(bytesRect, selectionPainter.LastBytesSelectionRect, (r) => selectionPainter.LastBytesSelectionRect = r);
            }
            else if (sender is AsciiDocument)
            {
                selectionAction(asciiRect, selectionPainter.LastAsciiSelectionRect, (r) => selectionPainter.LastAsciiSelectionRect = r);
            }
            else
            {
                Debugger.Break();
            }

            if (TextSelectionChanged != null)
            {
                TextSelectionChanged(this, EventArgs.Empty);
            }
        }

        public RangePainterTransaction BeginTransaction(RangePainter painter)
        {
            openTransaction = new RangePainterTransaction(this, painter);

            ((INotifyCollectionChanged)openTransaction).CollectionChanged += new NotifyCollectionChangedEventHandler(MemoryView_CollectionChanged);

            painter.TextRange.RangeChanged += new EventHandlerT<ITextRange>(TextRange_RangeChanged);

            return openTransaction;
        }

        private void MemoryView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var painter in e.NewItems.Cast<RangePainter>())
                {
                    painter.TextRange.RangeChanged += new EventHandlerT<ITextRange>(TextRange_RangeChanged);
                }
            }
        }

        private void TextRange_RangeChanged(object sender, EventArgs<ITextRange> e)
        {
            var textRange = e.Value;
            var painter = painters.Values.Single(p => p.TextRange == textRange);

            painter.DrawRects.Clear();

            textRange.Row = (int)(textRange.Start.As<float>() / layoutByteWidth.As<float>());
            textRange.EndRow = (int)(textRange.End.As<float>() / layoutByteWidth.As<float>());
            textRange.Column = (int)(textRange.Start.As<float>() % layoutByteWidth.As<float>());
            textRange.EndColumn = (int)(textRange.End.As<float>() % layoutByteWidth.As<float>());

            memoryClientView.Refresh();
        }

        private void CalculateRowColumn(RangePainterDragDropDataObject dataObject, Point mouseLocation)
        {
            dataObject.MouseRow = (int)((mouseLocation.Y - rowHeight).As<float>() / rowHeight.As<float>());
            dataObject.MouseColumn = (int)((mouseLocation.X - leftMarginWidth).As<float>() / byteCellWidth.As<float>());

            if (dataObject.MouseRow < 0 || dataObject.MouseRow >= bytesDocument.Lines.Count)
            {
                dataObject.MouseRow = -1;
            }

            if (dataObject.MouseColumn < 0 || dataObject.MouseColumn >= layoutByteWidth)
            {
                dataObject.MouseColumn = -1;
            }
        }

        private void ClientView_DragEnter(object sender, DragEventArgs e)
        {
            var dataObject = e.Data;
            var selection = (ITextSelection)dataObject.GetData(TextSelection.DATA_FORMAT);
            var point = memoryClientView.PointToClient(new Point(e.X, e.Y));
            Action<Rectangle, int> dragAction;

            if (mouseDownPainter != null)
            {
                var rangePainterDataObject = (RangePainterDragDropDataObject) dataObject.GetData(currentDragDropFormat);

                rangePainterDataObject.CurrentOperation = DragDropOperation.DragEnter;
                rangePainterDataObject.MouseLocation = point;
                CalculateRowColumn(rangePainterDataObject, point);

                mouseDownPainter.RaiseDragEnter(sender, e);

                return;
            }

            dragAction = (rect, cellWidth) =>
            {
                var scrollRow = vScrollBar.Value;
                int row;
                int cell;

                point.Offset(-rect.Left, -rect.Top);

                row = (int)((((float)point.Y) / ((float)rect.Height)) * (((float)rect.Height) / ((float)rowHeight))) + scrollRow;
                cell = (int)((((float)point.X) / ((float)rect.Width)) * (((float)rect.Width) / ((float)cellWidth)));

                e.Effect = DragDropEffects.Move;

                selection.Start = (row * layoutByteWidth) + cell;
                selection.Row = row;
                selection.Column = cell;

                if (DEBUG)
                {
                    OnStatus(this, new StatusEventArgs(string.Format("Drag started in bytesRect at row: {0}, cell: {1}", row, cell)));
                }
            };

            if (bytesRect.Contains(point))
            {
                dragAction(bytesRect, byteCellWidth);
            }
            else if (asciiRect.Contains(point))
            {
                dragAction(asciiRect, asciiCellWidth);
            }
        }

        private void ClientView_DragOver(object sender, DragEventArgs e)
        {
            var dataObject = e.Data;
            var selection = (ITextSelection)dataObject.GetData(TextSelection.DATA_FORMAT);
            var point = memoryClientView.PointToClient(new Point(e.X, e.Y));
            Action<Rectangle, int, MemoryTextDocument> dragAction;

            if (mouseDownPainter != null)
            {
                var rangePainterDataObject = (RangePainterDragDropDataObject)dataObject.GetData(currentDragDropFormat);

                rangePainterDataObject.CurrentOperation = DragDropOperation.DragOver;
                rangePainterDataObject.MouseLocation = point;
                CalculateRowColumn(rangePainterDataObject, point);

                mouseDownPainter.RaiseDragOver(sender, e);

                return;
            }

            dragAction = (rect, cellWidth, otherDoc) =>
            {
                var scrollRow = vScrollBar.Value;
                var orientation = new RangeOrientation(selection);
                int row;
                int cell;
                int end;
                int textLength;
                string selStart;
                string size;
                string selectionText;
                string text;
                byte[] bytes;
                const int TEXT_LIMIT = 25;

                point.Offset(-rect.Left, -rect.Top);

                row = (int)((((float)point.Y) / ((float)rect.Height)) * (((float)rect.Height) / ((float)rowHeight))) + scrollRow;
                cell = (int)((((float)point.X) / ((float)rect.Width)) * (((float)rect.Width) / ((float)cellWidth)));

                end = (row * layoutByteWidth) + cell;

                selection.EndRow = row;
                selection.EndColumn = Math.Min(cell, layoutByteWidth - 1);
                selection.End = end;

                otherDoc.Selection = selection;

                e.Effect = DragDropEffects.Move;

                selStart = orientation.Start.ToString("x8");
                size = (orientation.End - orientation.Start).ToString("x8");
                selectionText = selection.Text;

                if (selectionText != null)
                {
                    textLength = selectionText.Length / 2;
                    text = selectionText.Crop(TEXT_LIMIT * 2);
                    bytes = text.FromHex();

                    if (DEBUG)
                    {
                        OnStatus(this, new StatusEventArgs(string.Format("Sel Start:  {0}    Size:  {1}    [{2},{3}] Bytes: {4}", selStart, size, point.X, point.Y, text)));
                    }
                    else
                    {
                        string byteText = ASCIIEncoding.ASCII.GetString(bytes);
                        var outputString = string.Format("    Text: {0}", byteText.Sanitize());

                        if (textLength > TEXT_LIMIT)
                        {
                            outputString = outputString.Append(" ...");
                        }

                        switch (bytes.Length)
                        {
                            case 1:
                            {
                                var value = bytes[0];
                                var signedValue = (sbyte)unchecked(value);
                                outputString = string.Format("    Byte: {0} [0x{0:x2}]    Signed Byte: {1}{2}", value, signedValue, outputString);
                                break;
                            }
                            case 2:
                            {
                                var value = BitConverter.ToUInt16(bytes, 0);
                                var signedValue = (short)unchecked(value);
                                outputString = string.Format("    Short: {0} [0x{0:x2}]    Signed Short: {1}{2}", value, signedValue, outputString);
                                break;
                            }
                            case 4:
                            {
                                var value = BitConverter.ToUInt32(bytes, 0);
                                var signedValue = (int)unchecked(value);
                                outputString = string.Format("    Int: {0} [0x{0:x2}]    Signed Int: {1}{2}", value, signedValue, outputString);
                                break;
                            }
                            case 8:
                            {
                                var value = BitConverter.ToUInt64(bytes, 0);
                                var signedValue = (long)unchecked(value);
                                outputString = string.Format("    Long: {0} [0x{0:x2}]    Signed: {1}{2}", value, signedValue, outputString);
                                break;
                            }
                            case 16:
                            {
                                var value = new Guid(bytes);
                                outputString = string.Format("    Guid: {0}{1}", value.ToString(), outputString);
                                break;
                            }
                        }

                        if (OnStatus != null)
                        {
                            OnStatus(this, new StatusEventArgs(string.Format("Sel Start:  {0}    Size:  {1}{2}", selStart, size, outputString)));
                        }
                    }
                }
            };

            if (bytesRect.Contains(point))
            {
                dragAction(bytesRect, byteCellWidth, asciiDocument);
            }
            else if (asciiRect.Contains(point))
            {
                dragAction(asciiRect, asciiCellWidth, bytesDocument);
            }
        }

        private void ClientView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (mouseDownPainter != null)
            {
                mouseDownPainter.RaiseGiveFeedback(sender, e);
                return;
            }

            e.UseDefaultCursors = false;
        }

        private void ClientView_DragLeave(object sender, EventArgs e)
        {
            if (mouseDownPainter != null)
            {
                currentRangePainterDataObject.CurrentOperation = DragDropOperation.DragLeave;
                mouseDownPainter.RaiseDragLeave(sender, e);
            }

            if (DEBUG)
            {
                OnStatus(this, new StatusEventArgs("Drag leave"));
            }
        }

        private void ClientView_DragDrop(object sender, DragEventArgs e)
        {
            var dataObject = e.Data;
            this.Selection = (ITextSelection)dataObject.GetData(TextSelection.DATA_FORMAT);
            var point = memoryClientView.PointToClient(new Point(e.X, e.Y));

            if (TextSelectionChanged != null)
            {
                TextSelectionChanged(this, EventArgs.Empty);
            }

            if (mouseDownPainter != null)
            {
                var rangePainterDataObject = (RangePainterDragDropDataObject)dataObject.GetData(currentDragDropFormat);

                rangePainterDataObject.CurrentOperation = DragDropOperation.DragDrop;
                rangePainterDataObject.MouseLocation = point;
                CalculateRowColumn(rangePainterDataObject, point);

                mouseDownPainter.RaiseDragDrop(sender, e);
                mouseDownPainter = null;
                currentDragDropPainter = null;
                currentRangePainterDataObject = null;

                return;
            }

            if (DEBUG)
            {
                OnStatus(this, new StatusEventArgs("Drop completed"));
            }
        }

        private void ClientView_MouseDown(object sender, MouseEventArgs e)
        {
            var point = new Point(e.X, e.Y);
            var hasSelection = this.Selection != null;

            foreach (var painter in painters.Values.Where(p => p.HandleDragOperation))
            {
                if (painter.DrawRects.Any(r => r.Rectangle.Contains(point)))
                {
                    mouseDownPainter = painter;
                }
            }

            if (bytesRect.Contains(point))
            {
                bytesDocument.Selection.Clear();
                mouseDownDocument = bytesDocument;
                mouseDownPoint = point;
            }
            else if (asciiRect.Contains(point))
            {
                asciiDocument.Selection.Clear();
                mouseDownPoint = point;
                mouseDownDocument = asciiDocument;
            }

            if (hasSelection)
            {
                if (TextSelectionChanged != null)
                {
                    TextSelectionChanged(this, EventArgs.Empty);
                }
            }
        }

        private void ClientView_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            var point = memoryClientView.PointToClient(Control.MousePosition);

            if (mouseDownPainter != null)
            {
                var currentOperation = currentRangePainterDataObject.CurrentOperation;

                if (!Keys.LButton.IsPressed() || e.EscapePressed)
                {
                    if (currentOperation == DragDropOperation.DragLeave || e.EscapePressed)
                    {
                        mouseDownDocument = null;
                        mouseDownPainter = null;

                        currentRangePainterDataObject.CurrentOperation = DragDropOperation.Cancelled;
                        currentDragDropPainter.RaiseDragDropCanceled(currentDragDropPainter, new DragEventArgs(currentRangePainterDataObject, 0, 0, 0, DragDropEffects.None, DragDropEffects.None));

                        currentDragDropPainter = null;
                        currentRangePainterDataObject = null;

                        e.Action = DragAction.Cancel;
                    }
                    else if (currentOperation == DragDropOperation.DragOver)
                    {
                        mouseDownPainter.RaiseQueryContinueDrag(sender, e);
                    }
                    else
                    {
                        DebugUtils.Break();
                    }
                }

                return;
            }

            if (bytesRect.Contains(point) || asciiRect.Contains(point))
            {
                if (!Keys.LButton.IsPressed())
                {
                    e.Action = DragAction.Drop;
                }
                else
                {
                    e.Action = DragAction.Continue;
                }
            }
        }

        public string SelectedHex
        {
            get
            {
                if (this.Selection != null)
                {
                    var text = this.Selection.Text;
                    var builder = new StringBuilder();
                    var chars = text.ToCharArray();
                    var pair = string.Empty;
                    var byteIndex = 0;
                    var byteWidth = this.LayoutByteWidth;

                    foreach (var ch in chars)
                    {
                        pair += ch;

                        if (pair.Length == 2)
                        {
                            byteIndex++;

                            if (byteIndex == byteWidth)
                            {
                                byteIndex = 0;
                                builder.AppendLine(pair);
                            }
                            else
                            {
                                builder.Append(pair + " ");
                            }

                            pair = string.Empty;
                        }
                    }

                    if (byteIndex < byteIndex.RoundTo(byteWidth))
                    {
                        builder.RemoveEnd(1);
                    }

                    return builder.ToString();
                }

                return string.Empty;
            }
        }

        public string SelectedAscii
        {
            get
            {
                if (this.Selection != null)
                {
                    var text = this.Selection.Text;
                    var builder = new StringBuilder();
                    var bytes = IOExtensions.HexToByteArray(text);

                    for (var x = 0; x < bytes.Length; x++)
                    {
                        var _byte = bytes[x];
                        char _char = (char)_byte;

                        text = (_byte < 0x20 ? "." : new string(_char, 1));
                        builder.Append(text);
                    }

                    return builder.ToString();
                }

                return string.Empty;
            }
        }

        public byte[] SelectedBytes
        {
            get
            {
                if (this.Selection != null)
                {
                    var text = this.Selection.Text;
                    var builder = new StringBuilder();
                    var bytes = IOExtensions.HexToByteArray(text);

                    return bytes;
                }

                return null;
            }
        }

        public string GetHex(RangePainter painter)
        {
            var range = painter.TextRange;

            if (range != null)
            {
                var text = range.Text;
                var builder = new StringBuilder();
                var chars = text.ToCharArray();
                var pair = string.Empty;
                var byteIndex = 0;
                var byteWidth = this.LayoutByteWidth;

                foreach (var ch in chars)
                {
                    pair += ch;

                    if (pair.Length == 2)
                    {
                        byteIndex++;

                        if (byteIndex == byteWidth)
                        {
                            byteIndex = 0;
                            builder.AppendLine(pair);
                        }
                        else
                        {
                            builder.Append(pair + " ");
                        }

                        pair = string.Empty;
                    }
                }

                if (byteIndex < byteIndex.RoundTo(byteWidth))
                {
                    builder.RemoveEnd(1);
                }

                return builder.ToString();
            }

            return string.Empty;
        }

        public string GetAscii(RangePainter painter)
        {
            var range = painter.TextRange;

            if (range != null)
            {
                var text = range.Text;
                var builder = new StringBuilder();
                var bytes = IOExtensions.HexToByteArray(text);

                for (var x = 0; x < bytes.Length; x++)
                {
                    var _byte = bytes[x];
                    char _char = (char)_byte;

                    text = (_byte < 0x20 ? "." : new string(_char, 1));
                    builder.Append(text);
                }

                return builder.ToString();
            }

            return string.Empty;
        }

        public byte[] GetBytes(RangePainter painter)
        {
            var range = painter.TextRange;

            if (range != null)
            {
                var text = range.Text;
                var builder = new StringBuilder();
                var bytes = IOExtensions.HexToByteArray(text);

                return bytes;
            }

            return null;
        }

        private void ClientView_MouseLeave(object sender, EventArgs e)
        {
            mouseDownDocument = null;
        }

        private void ClientView_MouseMove(object sender, MouseEventArgs e)
        {
            var point = new Point(e.X, e.Y);

            if (currentDragDropPainter != null)
            {
                return;
            }

            foreach (var rangePainter in rangePainters)
            {
                RangePainterDragDropDataObject rangePainterDataObject = null;

                if (rangePainter == mouseDownPainter)
                {
                    var args = new QueryDoDragDropEventArgs(e);

                    mouseDownPainter.RaiseQueryDoDragDrop(args);

                    if (args.DragAction == DragAction.Continue)
                    {
                        rangePainterDataObject = args.Data;
                        currentDragDropFormat = rangePainterDataObject.DataFormat;
                        currentDragDropPainter = rangePainter;
                        currentRangePainterDataObject = rangePainterDataObject;

                        if (hitTestRangePainters.Contains(rangePainter))
                        {
                            hitTestRangePainters.Remove(rangePainter);
                        }

                        rangePainterDataObject.Start = rangePainter.TextRange.Start;
                        rangePainterDataObject.End = rangePainter.TextRange.End;
                        rangePainterDataObject.Row = rangePainter.TextRange.Row;
                        rangePainterDataObject.EndRow = rangePainter.TextRange.EndRow;
                        rangePainterDataObject.Column = rangePainter.TextRange.Column;
                        rangePainterDataObject.EndColumn = rangePainter.TextRange.EndColumn;
                        rangePainterDataObject.RangePainter = rangePainter;
                        rangePainterDataObject.HitTestArea = rangePainter.LastHitTestArea;
                        rangePainterDataObject.CurrentOperation = DragDropOperation.DoDragDrop;

                        CalculateRowColumn(rangePainterDataObject, point);

                        memoryClientView.DoDragDrop(rangePainterDataObject, args.AllowedEffects);
                    }
                    else
                    {
                        mouseDownPainter = null;
                    }
                }
                
                if (rangePainter.DrawRects.Any(r => r.Rectangle.Contains(point)))
                {
                    var rect = rangePainter.DrawRects.Single(r => r.Rectangle.Contains(point));
                    var area = DetermineHitTestArea(point, rect);
                    RangePainterHitTestEventArgs args;

                    if (rangePainterDataObject == null)
                    {
                        args = new RangePainterHitTestEventArgs(point, area);

                        rangePainter.RaiseHitTest(args);
                        rangePainter.LastHitTestArea = area;

                        if (!hitTestRangePainters.Contains(rangePainter))
                        {
                            hitTestRangePainters.Add(rangePainter);
                        }
                    }
                    else
                    {
                        rangePainterDataObject.HitTestArea = area;
                    }
                }
                else if (hitTestRangePainters.Contains(rangePainter))
                {
                    var args = new RangePainterHitTestEventArgs(point, HitTestArea.None);

                    rangePainter.RaiseHitTest(args);
                    hitTestRangePainters.Remove(rangePainter);
                }
            }

            if (bytesRect.Contains(point))
            {
                if (mouseDownDocument == bytesDocument)
                {
                    var dragSize = SystemInformation.DragSize;
                    var rect = new Rectangle(mouseDownPoint, Size.Empty);

                    rect.Inflate(dragSize);

                    if (!rect.Contains(point))
                    {
                        memoryClientView.DoDragDrop(bytesDocument.Selection, DragDropEffects.Move);
                    }
                }
            }
            else if (asciiRect.Contains(point))
            {
                if (mouseDownDocument == asciiDocument)
                {
                    var dragSize = SystemInformation.DragSize;
                    var rect = new Rectangle(mouseDownPoint, Size.Empty);

                    rect.Inflate(dragSize);

                    if (!rect.Contains(point))
                    {
                        memoryClientView.DoDragDrop(asciiDocument.Selection, DragDropEffects.Move);
                    }
                }
            }
        }

        private static HitTestArea DetermineHitTestArea(Point point, DrawRect rect)
        {
            var area = HitTestArea.Interior;
            var leftRect = new Rectangle(rect.X, rect.Y + 2, 2, rect.Height - 4);
            var topRect = new Rectangle(rect.X + 2, rect.Y, rect.Width - 4, 2);
            var rightRect = new Rectangle(rect.X + rect.Width - 2, rect.Y + 2, 2, rect.Height - 4);
            var bottomRect = new Rectangle(rect.X + 2, rect.Y + rect.Height - 2, rect.Width - 4, 2);
            var leftTopCorner = new Rectangle(rect.X, rect.Y, 2, 2);
            var rightTopCorner = new Rectangle(rect.X + rect.Width - 2, rect.Y, 2, 2);
            var rightBottomCorner = new Rectangle(rect.X + rect.Width - 2, rect.Y + rect.Height - 2, 2, 2);
            var leftBottomCorner = new Rectangle(rect.X, rect.Y + rect.Height - 2, 2, 2);

            if (leftRect.Contains(point))
            {
                area = HitTestArea.LeftEdge;
            }
            else if (topRect.Contains(point))
            {
                area = HitTestArea.TopEdge;
            }
            else if (rightRect.Contains(point))
            {
                area = HitTestArea.RightEdge;
            }
            else if (bottomRect.Contains(point))
            {
                area = HitTestArea.BottomEdge;
            }
            else if (leftTopCorner.Contains(point))
            {
                area = HitTestArea.LeftTopCorner;
            }
            else if (rightTopCorner.Contains(point))
            {
                area = HitTestArea.RightTopCorner;
            }
            else if (rightBottomCorner.Contains(point))
            {
                area = HitTestArea.RightBottomCorner;
            }
            else if (leftBottomCorner.Contains(point))
            {
                area = HitTestArea.LeftBottomCorner;
            }

            return area;
        }

        private void ClientView_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownDocument = null;
            mouseDownPainter = null;

            if (currentDragDropPainter != null)
            {
                currentRangePainterDataObject.CurrentOperation = DragDropOperation.Cancelled;
                currentDragDropPainter.RaiseDragDropCanceled(currentDragDropPainter, new DragEventArgs(currentRangePainterDataObject, 0, 0, 0, DragDropEffects.None, DragDropEffects.None));
            }

            currentDragDropPainter = null;
            currentRangePainterDataObject = null;
        }

        internal void CommitPainter(RangePainter painter)
        {
            painter.TextRange.RangeChanged -= new EventHandlerT<ITextRange>(TextRange_RangeChanged);
            openTransaction = null;
        }

        internal void RollbackPainter(RangePainter painter)
        {
            painter.TextRange.RangeChanged -= new EventHandlerT<ITextRange>(TextRange_RangeChanged);
            openTransaction = null;
            memoryClientView.Invalidate();
        }
    }
}
