using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Parsing.Nodes
{
    public class LiteralExpression : LiteralLikeNode
    {
        public object LiteralExpressionBrand { get; set; }

        public LiteralExpression(SyntaxKind kind, int pos, int end) : base(kind, pos, end)
        {
        }

        public static explicit operator LiteralExpression(TokenNode node)
        {
            return new LiteralExpression(SyntaxKind.Unknown, node.Pos, node.End) { Text = node.Text, IsUnterminated = node.IsUnterminated, HasExtendedUnicodeEscape = node.HasExtendedUnicodeEscape, IsOctalLiteral = node.IsOctalLiteral };
        }

        public static explicit operator TokenNode(LiteralExpression expression)
        {
            return new TokenNode(SyntaxKind.Unknown, expression.Pos, expression.End) { Text = expression.Text, IsUnterminated = expression.IsUnterminated, HasExtendedUnicodeEscape = expression.HasExtendedUnicodeEscape, IsOctalLiteral = expression.IsOctalLiteral };
        }
    }
}
