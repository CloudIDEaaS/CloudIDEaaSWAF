using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils.Parsing.Nodes
{
    [DebuggerDisplay("{DebugInfo}")]
    public class CLRStringMethod : MethodDeclaration
    {
        public CLRStringMethod(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }

        public string DebugInfo
        {
            get
            {
                var builder = new StringBuilder(this.Type.GetDebuggerDisplay());

                builder.Append(" " + this.Name.GetDebuggerDisplay());

                if (this.TypeParameters != null && this.TypeParameters.Count > 0)
                {
                    var typeParameterBuilder = new StringBuilder();

                    foreach (var typeParameter in this.TypeParameters)
                    {
                        typeParameterBuilder.AppendWithLeadingIfLength(",", typeParameter.GetDebuggerDisplay());
                    }

                    builder.AppendFormat("[{0}]", typeParameterBuilder.ToString());
                }

                if (this.Parameters != null && this.Parameters.Count > 0)
                {
                    var parameterBuilder = new StringBuilder();

                    foreach (var parameter in this.Parameters)
                    {
                        parameterBuilder.AppendWithLeadingIfLength(", ", parameter.GetDebuggerDisplay());
                    }

                    builder.AppendFormat("({0})", parameterBuilder.ToString());
                }

                return builder.ToString();
            }
        }
    }
}
