using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Parsing.Nodes
{
    public class TypeNode : Node
    {
        public object TypeNodeBrand { get; set; }

        public TypeNode(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }
    }
}
