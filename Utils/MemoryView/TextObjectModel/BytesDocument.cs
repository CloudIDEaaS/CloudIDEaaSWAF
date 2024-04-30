using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.MemoryView.TextObjectModel;
using Utils.MemoryView.TextObjectModel;
using System.Drawing;
using System.IO;
using Utils;
using Utils.TextObjectModel;

namespace Utils.MemoryView.TextObjectModel
{
    public class BytesDocument : MemoryTextDocument
    {
        public BytesDocument()
        {
        }

        public override void Read(Stream stream, int lineWidth)
        {
            var reader = new BinaryReader(stream);
            var length = reader.BaseStream.Length;
            var rows = (int)length / lineWidth;
            string text;

            using (var reset = reader.MarkForReset())
            {
                reader.Seek(0);

                for (var y = 0; y < rows; y++)
                {
                    var offset = (y * lineWidth);
                    byte[] bytes;
                    var byteArray = new string[lineWidth];
                    var count = 0;

                    count = Math.Max(0, Math.Min(lineWidth, (int)(reader.BaseStream.Length - reader.BaseStream.Position)));

                    if (count > 0)
                    {
                        bytes = reader.ReadBytes(count);

                        for (var x = 0; x < bytes.Length; x++)
                        {
                            var _byte = bytes[x];
                            char _char = (char)_byte;

                            text = _byte.ToString("x2").ToUpper();
                            byteArray[x] = text;
                        }

                        this.Lines.Add((TextLine)byteArray);
                    }
                }
            }
        }   
    }
}
