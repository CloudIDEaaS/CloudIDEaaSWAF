using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Parsing.Nodes
{
    public class Declaration : Node
    {
        public object DeclarationBrand { get; set; }
        public DeclarationName Name { get; set; }

        public Declaration(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }
    }
}
