using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils.Parsing.Nodes
{
    [DebuggerDisplay("{DebugInfo}")]
    public class ParameterDeclaration : Declaration
    {
        public const SyntaxKind kind = SyntaxKind.Parameter;
        public new BindingName Name { get; set; }
        public TypeNode Type { get; set; }
 
        public ParameterDeclaration(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }

        public string DebugInfo
        {
            get
            {
                if (this.Type != null)
                {
                    var builder = new StringBuilder(this.Type.GetDebuggerDisplay());

                    if (this.Name != null)
                    {
                        builder.Append(" " + this.Name.GetDebuggerDisplay());
                    }

                    return builder.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
