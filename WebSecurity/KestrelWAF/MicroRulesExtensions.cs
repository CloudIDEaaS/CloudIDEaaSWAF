using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Utils.Hierarchies;
using WebSecurity.KestrelWAF.RulesEngine;

namespace WebSecurity.KestrelWAF
{
    public static class MicroRulesExtensions
    {
        public static bool HandleTargetValue(this Rule rule, Expression param, out Expression result)
        {
            var targetValue = rule.TargetValue;
            var txt = targetValue as string;

            if (txt != null)
            {
                if (txt.StartsWith("*."))
                {
                    txt = txt.Substring(2);
                    result = MicroRulesEngine.GetProperty(param, txt);
                }
                else if (bool.TryParse(txt, out bool boolResult))
                {
                    result = Expression.Constant(boolResult, typeof(bool));
                }
                else if (int.TryParse(txt, out int intResult))
                {
                    result = Expression.Constant(intResult, typeof(int));
                }
                else
                {
                    result = MicroRulesEngine.GetProperty(param, txt);
                }

                return true;
            }

            result = null!;
            return false;
        }

        public static bool HandleParameter(this Rule rule, Expression param, out Expression result)
        {
            var targetValue = rule.TargetValue;
            var txt = targetValue as string;

            if (txt != null)
            {
                if (txt.StartsWith("*."))
                {
                    txt = txt.Substring(2);
                    result = MicroRulesEngine.GetProperty(param, txt);
                }
                else if (bool.TryParse(txt, out bool boolResult))
                {
                    result = Expression.Constant(boolResult, typeof(bool));
                }
                else if (int.TryParse(txt, out int intResult))
                {
                    result = Expression.Constant(intResult, typeof(int));
                }
                else
                {
                    result = MicroRulesEngine.GetProperty(param, txt);
                }

                return true;
            }

            result = null!;
            return false;
        }

        public static RulePropertyPresence GetPropertyPresence(this Rule rule)
        {
            var propertyPresence = RulePropertyPresence.NotSet;

            foreach (var propertyValue in rule.GetPublicPropertyValues())
            {
                if (propertyValue.Value != null && !propertyValue.Value.ToString().IsNullOrEmpty())
                {
                    propertyPresence |= EnumUtils.GetValue<RulePropertyPresence>(propertyValue.Key);
                }
            }

            return propertyPresence;
        }

        public static ConstantExpression MakeDelegate(this Expression expression, Expression paramInput)
        {
            var paramExpression = Expression.Parameter(paramInput.Type);
            var parameterReplacer = new ParameterReplacer(paramExpression).Visit(expression);

            var lambda = Expression.Lambda(parameterReplacer, paramExpression).Compile();
            var delegateExpression = Expression.Constant(lambda);

            return delegateExpression;
        }

        public static Expression ToBinaryExpression(this string operatorText, Expression left, Expression right)
        {
            Func<Expression, Expression, Expression>? resultExpression = null;

            if (ExpressionType.TryParse(operatorText, out ExpressionType expressionType))
            {
                switch (expressionType)
                {
                    case ExpressionType.Or:
                        resultExpression = Expression.Or;
                        break;
                    case ExpressionType.OrElse:
                        resultExpression = Expression.OrElse;
                        break;
                    case ExpressionType.AndAlso:
                        resultExpression = Expression.AndAlso;
                        break;
                    case ExpressionType.And:
                        resultExpression = Expression.And;
                        break;
                    default:
                        throw new ArgumentException($"Invalid operator text {operatorText} for method {nameof(ToBinaryExpression)}", "operatorText");
                }
            }

            return resultExpression(left, right);
        }
    }
}
