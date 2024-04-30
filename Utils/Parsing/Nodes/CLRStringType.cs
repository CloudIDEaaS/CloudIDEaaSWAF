using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Utils;

namespace Utils.Parsing.Nodes
{
    [DebuggerDisplay("{DebugInfo}")]
    public class CLRStringType : TypeReferenceNode
    {
        public new const SyntaxKind kind = SyntaxKind.CLRStringTypeReference;
        public new CLRStringEntityName TypeName { get; set; }

        public CLRStringType(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }

        public string DebugInfo
        {
            get
            {
                var builder = new StringBuilder(this.TypeName.DebugInfo);

                if (this.TypeArguments != null && this.TypeArguments.Count > 0)
                {
                    var typeArgsBuilder = new StringBuilder();

                    foreach (var typeArgument in this.TypeArguments)
                    {
                        typeArgsBuilder.AppendWithLeadingIfLength(",", typeArgument.GetDebuggerDisplay()); 
                    }

                    builder.AppendFormat("[{0}]", typeArgsBuilder.ToString());
                }

                return builder.ToString();
            }
        }
    }
}
