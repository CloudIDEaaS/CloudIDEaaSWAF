using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils.Parsing.Nodes
{
    [DebuggerDisplay("{DebugInfo}")]
    public class LiteralLikeNode : Node
    {
        public string Text { get; set; }
        public bool IsUnterminated { get; set; }
        public bool? HasExtendedUnicodeEscape { get; set; }
        public bool? IsOctalLiteral { get; set; }

        public LiteralLikeNode(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }

        public string DebugInfo
        {
            get
            {
                return this.Text;
            }
        }
    }
}
