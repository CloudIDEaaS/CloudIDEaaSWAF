using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils.Parsing.Nodes
{
    [DebuggerDisplay("{DebugInfo}")]
    public class CLRStringEntityName : Node
    {
        public EntityName EntityName { get; set; }
        public NumericLiteral ArgCount { get; set; }

        public CLRStringEntityName(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }

        public static explicit operator CLRStringEntityName(EntityName entityName)
        {
            var value = entityName.GetPopulatedFieldValue<Node>();

            return new CLRStringEntityName(SyntaxKind.CLRStringEntityName, value.Pos, value.End) { EntityName = entityName };
        }

        public string DebugInfo
        {
            get
            {
                if (this.ArgCount != null)
                {
                    return this.EntityName.DebugInfo + "`" + this.ArgCount.DebugInfo;
                }
                else
                {
                    return this.EntityName.DebugInfo;
                }
            }
        }
    }
}
