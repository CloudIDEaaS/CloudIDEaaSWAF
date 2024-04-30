using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using Metaspec;
using System.Diagnostics;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Utils
{
    public static class ExpressionExtensions
    {
        public static Expression NullableToDefault(this Expression expr)
        {
            if (expr.Type.IsNullable())
            {
                var innerType = expr.Type.GetAnyInnerType();

                if (innerType == typeof(int))
                {
                    expr = Expression.Condition(Expression.Equal(expr, Expression.Constant(null)), Expression.Constant(0), Expression.Property(expr, "Value"));
                }
                else if (innerType == typeof(string))
                {
                    expr = Expression.Condition(Expression.Equal(expr, Expression.Constant(null)), Expression.Constant(null), Expression.Property(expr, "Value"));
                }
                else if (innerType == typeof(long))
                {
                    expr = Expression.Condition(Expression.Equal(expr, Expression.Constant(null)), Expression.Constant(0L), Expression.Property(expr, "Value"));
                }
                else if (innerType == typeof(bool))
                {
                    expr = Expression.Condition(Expression.Equal(expr, Expression.Constant(null)), Expression.Constant(false), Expression.Property(expr, "Value"));
                }
                else
                {
                    DebugUtils.Break();
                }
            }

            return expr;
        }

        public static MemberInfo GetMemberFromSelector<TItem, TKey>(this Expression<Func<TItem, TKey>> referencingPropertySelector)
        {
            var memberSelectorEvaluator = new MemberSelectorEvaluator<TItem, TKey>();
            var project = ICsProjectFactory.create(project_namespace.pn_project_namespace);
            var code = referencingPropertySelector.ToString();
            var snippet = ICsSnippetFactory.create(code.ToCharArray(), null);
            MemberSelectorEvaluator<TItem, TKey>.MemberResult result;
            CsNode rootNode;

            try
            {
                project.parseSnippet(snippet, CsExpectedSnippet.cses_statement, null, true);
            }
            catch
            {
                Debugger.Break();
            }

            rootNode = snippet.getNodes()[0];

            result = memberSelectorEvaluator.ProcessRoot(rootNode);

            return result.MemberInfo;
        }
    }
}
