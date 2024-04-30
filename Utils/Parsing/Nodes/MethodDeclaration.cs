using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Parsing.Nodes
{
    public class MethodDeclaration : FunctionLikeDeclaration
    {
        public const SyntaxKind kind = SyntaxKind.MethodDeclaration;
        public new PropertyName Name { get; set; }

        public MethodDeclaration(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }
    }
}
