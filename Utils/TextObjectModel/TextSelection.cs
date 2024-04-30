using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.TextObjectModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Utils;

namespace Utils.TextObjectModel
{
    public class TextSelection : TextRange, ITextSelection, System.Windows.Forms.IDataObject
    {
        public const string DATA_FORMAT = "TextSelection";

        public TextSelection() : base()
        {
        }

        internal TextSelection(ITextDocument document) : base(document)
        {
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
            if (format == DATA_FORMAT)
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
            if (format == DATA_FORMAT)
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
            if (format == DATA_FORMAT)
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
            if (format == DATA_FORMAT)
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
            return new string[] { DATA_FORMAT };
        }

        public string[] GetFormats(bool autoConvert)
        {
            return new string[] { DATA_FORMAT };
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
