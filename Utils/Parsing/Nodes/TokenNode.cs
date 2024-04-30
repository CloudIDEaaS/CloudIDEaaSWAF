using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Parsing.Nodes
{
    public class TokenNode : Node
    {
        public string Text { get; set; }
        public bool IsUnterminated { get; set; }
        public bool? HasExtendedUnicodeEscape { get; set; }
        public bool? IsOctalLiteral { get; set; }

        public TokenNode(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }
   }
}
