using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Parsing
{
    public interface ITextRange
    {
        int Position { get; set; }
        int End { get; set; }
    }
}
