using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Utils.TextObjectModel;
using System.Windows.Forms;

namespace Utils.MemoryView
{
    public abstract class RangePainterDragDropDataObject : TextRange, System.Windows.Forms.IDataObject
    {
        public RangePainter RangePainter { get; set; }
        public Point MouseLocation { get; set; }
        public Point LastMouseLocation { get; set; }
        public abstract string DataFormat { get; }
        public HitTestArea HitTestArea { get; set; }
        public int MouseRow { get; set; }
        public int MouseColumn { get; set; }
        public int LastMouseRow { get; set; }
        public int LastMouseColumn { get; set; }
        public DragDropOperation CurrentOperation { get; set; }

        public RangePainterDragDropDataObject() : base()
        {
            LastMouseColumn = -1;
            LastMouseRow = -1;
        }

        internal RangePainterDragDropDataObject(ITextDocument document) : base(document)
        {
            LastMouseColumn = -1;
            LastMouseRow = -1;
        }

        public override void Clear()
        {
            base.Clear();
        }

        public TextSelectionFlags Flags
        {
            set { throw new NotImplementedException(); }
        }

        public TextSelectionType Type
        {
            get { throw new NotImplementedException(); }
        }

        public int MoveLeft(int extend)
        {
            throw new NotImplementedException();
        }

        public int MoveRight(int extend)
        {
            throw new NotImplementedException();
        }

        public int MoveUp(int extend)
        {
            throw new NotImplementedException();
        }

        public int MoveDown(int extend)
        {
            throw new NotImplementedException();
        }

        public int HomeKey(int extend)
        {
            throw new NotImplementedException();
        }

        public int EndKey(int extend)
        {
            throw new NotImplementedException();
        }

        public void TypeText(string bstr)
        {
            throw new NotImplementedException();
        }

        public object GetData(Type format)
        {
            throw new NotImplementedException();
        }

        public object GetData(string format)
        {
            if (format == this.DataFormat)
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        public object GetData(string format, bool autoConvert)
        {
            if (format == this.DataFormat)
            {
                return this;
            }
            else
            {
                return null;
            }
        }

        public bool GetDataPresent(Type format)
        {
            throw new NotImplementedException();
        }

        public bool GetDataPresent(string format)
        {
            if (format == this.DataFormat)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetDataPresent(string format, bool autoConvert)
        {
            if (format == this.DataFormat)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string[] GetFormats()
        {
            return new string[] { this.DataFormat };
        }

        public string[] GetFormats(bool autoConvert)
        {
            return new string[] { this.DataFormat };
        }

        public void SetData(object data)
        {
            throw new NotImplementedException();
        }

        public void SetData(Type format, object data)
        {
            throw new NotImplementedException();
        }

        public void SetData(string format, object data)
        {
            throw new NotImplementedException();
        }

        public void SetData(string format, bool autoConvert, object data)
        {
            throw new NotImplementedException();
        }
    }
}
