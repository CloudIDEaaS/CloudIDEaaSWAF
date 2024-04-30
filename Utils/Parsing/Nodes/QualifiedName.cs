using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils.Parsing.Nodes
{
    [DebuggerDisplay("{DebugInfo}")]
    public class QualifiedName : Node
    {
        public const SyntaxKind kind = SyntaxKind.QualifiedName;
        public EntityName Left { get; set; }
        public Identifier Right { get; set; }

        public QualifiedName(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }

        public string DebugInfo
        {
            get
            {
                return Left.DebugInfo + "." + Right.DebugInfo;
            }
        }
    }
}
