using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils.Parsing.Nodes
{
    [DebuggerDisplay("{DebugInfo}")]
    public class TypeParameterDeclaration : Declaration
    {
        public const SyntaxKind kind = SyntaxKind.TypeParameter;
        public new Identifier Name { get; set; }
        public TypeNode Constraint { get; set; }

        public TypeParameterDeclaration(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }

        public string DebugInfo
        {
            get
            {
                var builder = new StringBuilder(this.Name.GetDebuggerDisplay());

                return builder.ToString();
            }
        }
    }
}
