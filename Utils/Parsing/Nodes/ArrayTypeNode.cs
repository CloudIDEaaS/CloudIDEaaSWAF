using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils.Parsing.Nodes
{
    [DebuggerDisplay("{DebugInfo}")]
    public class ArrayTypeNode : TypeNode
    {
        public const SyntaxKind kind = SyntaxKind.ArrayType;
        public TypeNode ElementType { get; set; }

        public ArrayTypeNode(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }

        public string DebugInfo
        {
            get
            {
                var builder = new StringBuilder(this.ElementType.GetDebuggerDisplay());

                builder.Append("[]");

                return builder.ToString();
            }
        }
    }
}
