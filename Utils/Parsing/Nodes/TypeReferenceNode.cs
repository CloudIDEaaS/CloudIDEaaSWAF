using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils.Parsing.Nodes
{
    public class TypeReferenceNode : TypeNode
    {
        public const SyntaxKind kind = SyntaxKind.TypeReference;
        public EntityName TypeName { get; set; }
        public NodeArray<Node> TypeArguments { get; set; }

        public TypeReferenceNode(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }
    }
}
