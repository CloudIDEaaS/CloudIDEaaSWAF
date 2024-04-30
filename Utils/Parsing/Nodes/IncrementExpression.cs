using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Parsing.Nodes
{
    public class IncrementExpression : UnaryExpression
    {
        public IncrementExpression(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }
    }
}
