 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Reflection.Emit;
using System.IO;
using Metaspec;
using System.Diagnostics;

namespace Utils
{
    public class MemberSelectorEvaluator<TItem, TKey>
    {
        private Type keyType;
        private Type itemType;
        private MemberResult result;

        public MemberSelectorEvaluator()
        {
            this.keyType = typeof(TKey);
            this.itemType = typeof(TItem);
        }

        public MemberResult ProcessRoot(CsNode root)
        {
            switch (root.e)
            {
                case cs_node.n_block:

                    var block = (CsBlock)root;

                    foreach (var statement in block.statements)
                    {
                        ProcessStatement(statement);
                    }

                    break;
                case cs_node.n_expression_statement:

                    ProcessStatement((CsStatement)root);
                    break;

                default:
                    Debugger.Break();
                    break;
            }

            return result;
        }

        private MemberResult ProcessResultExpression(CsNode node)
        {
            switch (node.e)
            {
                case cs_node.n_simple_name:

                    var simpleName = (CsSimpleName)node;
                    var memberResult = ProcessSimpleName(simpleName);

                    result = memberResult;
                    break;

                default:
                    Debugger.Break();
                    break;
            }

            return null;
        }

        private MemberResult ProcessSimpleName(CsSimpleName name)
        {
            var result = new MemberResult();
            var fieldInfo = itemType.GetField(name.identifier.original_text, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);

            if (fieldInfo != null)
            {
                result.MemberInfo = fieldInfo;
                result.IsPublic = fieldInfo.IsPublic;
            }
            else
            {
                var propertyInfo = itemType.GetProperty(name.identifier.original_text, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);

                if (propertyInfo != null)
                {
                    var getMethod = propertyInfo.GetGetMethod();

                    result.MemberInfo = propertyInfo;

                    if (getMethod == null)
                    {
                        getMethod = propertyInfo.GetGetMethod(true);
                    }

                    result.IsPublic = getMethod.IsPublic;
                }
                else
                {
                    Debugger.Break();
                }
            }

            return result;
        }

        private MemberResult ProcessIdentifier(CsIdentifier identifier)
        {
            var result = new MemberResult();
            var fieldInfo = itemType.GetField(identifier.original_text, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);

            if (fieldInfo != null)
            {
                result.MemberInfo = fieldInfo;
                result.IsPublic = fieldInfo.IsPublic;
            }
            else
            {
                var propertyInfo = itemType.GetProperty(identifier.original_text, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);

                if (propertyInfo != null)
                {
                    var getMethod = propertyInfo.GetGetMethod();

                    result.MemberInfo = propertyInfo;

                    if (getMethod == null)
                    {
                        getMethod = propertyInfo.GetGetMethod(true);
                    }

                    result.IsPublic = getMethod.IsPublic;
                }
                else
                {
                    Debugger.Break();
                }
            }

            return result;
        }

        private void ProcessStatement(CsStatement statement)
        {
            switch (statement.e)
            {
                case cs_node.n_expression_statement:

                    var expressionStatement = (CsExpressionStatement) statement;

                    ProcessExpression(expressionStatement.expression);
                    break;
                default:
                    Debugger.Break();
                    break;
            }
        }

        private void ProcessExpression(CsExpression expression)
        {
            switch (expression.e)
            {
                case cs_node.n_simple_name:

                    ProcessResultExpression(expression);
                    break;

                case cs_node.n_lambda_expression:

                    ProcessLambdaExpression((CsLambdaExpression) expression);
                    break;

                default:
                    Debugger.Break();
                    break;
            }
        }

        private void ProcessLambdaExpression(CsLambdaExpression lambdaExpression)
        {
            ProcessLambdaExpressionBody(lambdaExpression.body);
        }

        private void ProcessLambdaExpressionBody(CsNode csNode)
        {
            switch (csNode.e)
            {
                case cs_node.n_primary_expression_member_access:

                    var memberAccess = (CsPrimaryExpressionMemberAccess)csNode;
                    var memberResult = ProcessIdentifier(memberAccess.identifier);

                    result = memberResult;

                    break;

                default:
                    Debugger.Break();
                    break;
            }
        }

        public class MemberResult
        {
            public MemberInfo MemberInfo { get; set; }
            public bool IsPublic { get; set; }

            public bool IsField
            {
                get
                {
                    return this.MemberInfo is FieldInfo;
                }
            }

            public string Name
            {
                get
                {
                    if (this.IsField)
                    {
                        return this.Field.Name;
                    }
                    else if (this.IsProperty)
                    {
                        return this.Property.Name;
                    }

                    return null;
                }
            }

            public Type Type
            {
                get
                {
                    if (this.IsField)
                    {
                        return this.Field.FieldType;
                    }
                    else if (this.IsProperty)
                    {
                        return this.Property.PropertyType;
                    }

                    return null;
                }
            }

            public FieldInfo Field
            {
                get
                {
                    return (FieldInfo)this.MemberInfo;
                }
            }

            public bool IsProperty
            {
                get
                {
                    return this.MemberInfo is PropertyInfo;
                }
            }

            public PropertyInfo Property
            {
                get
                {
                    return (PropertyInfo)this.MemberInfo;
                }
            }
        }
    }
}
