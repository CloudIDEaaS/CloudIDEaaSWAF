using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Parsing.Nodes
{
    public class SignatureDeclaration : Declaration
    {
        public new PropertyName Name { get; set; }
        public List<TypeParameterDeclaration> TypeParameters { get; set; }
        public List<ParameterDeclaration> Parameters { get; set; }
        public TypeNode Type { get; set; }

        public SignatureDeclaration(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }
    }
}
