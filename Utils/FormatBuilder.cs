using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class FormatBuilder
    {
        private List<object> args;
        private string format;
        private int index = 0;

        public FormatBuilder()
        {
            args = new List<object>();
            format = string.Empty;
        }

        public void Add(string format, object obj)
        {
            this.format += string.Format(format.Replace("{", "{{{").Replace("}", "}}}"), index++);
            args.Add(obj.AsDisplayText());
        }

        public override string ToString()
        {
            return string.Format(format, args.ToArray());
        }
    }
}
