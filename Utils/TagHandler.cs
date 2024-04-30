using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utils
{
    public class TagHandler : DisposableBase<string>
    {
        public int Indent { get; private set; }

        public TagHandler(TextWriter writer, int indent, string tag) : base(tag, TagHandler.GetEndTagHandler(writer, tag, indent))
        {
            this.Indent = indent;
        }

        private static EventHandlerT<string> GetEndTagHandler(TextWriter writer, string tag, int indent)
        {
            return new EventHandlerT<string>((o, e) =>
            {
                writer.WriteLineFormatTabIndent(indent, "</{0}>", tag);
            });
        }

        public TagHandler(StringBuilder builder, int indent, string tag) : base(tag, TagHandler.GetEndTagHandler(builder, tag, indent))
        {
            this.Indent = indent;
        }

        private static EventHandlerT<string> GetEndTagHandler(StringBuilder builder, string tag, int indent)
        {
            return new EventHandlerT<string>((o, e) =>
            {
                builder.AppendLineFormatTabIndent(indent, "</{0}>", tag);
            });
        }

        public string Tag 
        {
            get
            {
                return this.InternalObject;
            }
        }
    }
}
