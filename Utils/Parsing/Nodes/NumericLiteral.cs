using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Parsing.Nodes
{
    public class NumericLiteral : LiteralExpression
    {
        public const SyntaxKind kind = SyntaxKind.NumericLiteral;
        public string TrailingComment { get; set; }

        public NumericLiteral(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }

        public static explicit operator NumericLiteral(TokenNode node)
        {
            return new NumericLiteral(SyntaxKind.NumericLiteral, node.Pos, node.End) { Text = node.Text, IsUnterminated = node.IsUnterminated, HasExtendedUnicodeEscape = node.HasExtendedUnicodeEscape, IsOctalLiteral = node.IsOctalLiteral };
        }
    }
}
