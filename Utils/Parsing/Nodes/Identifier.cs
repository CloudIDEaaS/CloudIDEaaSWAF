using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils.Parsing.Nodes
{
    [DebuggerDisplay("{DebugInfo}")]
    public class Identifier : PrimaryExpression
    {
        public SyntaxKind OriginalKeywordKind { get; set; }
        public string Text { get; set; }

        public Identifier(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
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
