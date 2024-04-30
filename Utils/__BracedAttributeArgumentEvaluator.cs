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
    public class BracedAttributeArgumentEvaluator
    {
        private ILGenerator methodGenerator;
        private Type evaluatorType;
        private TypeBuilder typeBuilder;
        private MethodBuilder methodEvaluateBuilder;
        private Type objType;
        private static ModuleBuilder moduleBuilder;
        private static AssemblyBuilder assemblyBuilder;
        private static int currentTypeCount;
        private string fileName;
        private string methodName;
        private string typeName;

        public BracedAttributeArgumentEvaluator(Type objType, string fileName = null)
        {
            this.objType = objType;

            if (fileName == null && moduleBuilder == null)
            {
                var assemblyName = new AssemblyName("BracedAttributeArgumentEvaluator");
                var domain = Thread.GetDomain();
                var name = new AssemblyName();

                this.fileName = fileName;

                if (fileName != null)
                {
                    assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave, Path.GetDirectoryName(fileName));
                    moduleBuilder = assemblyBuilder.DefineDynamicModule("Module", Path.GetFileName(fileName), true);
                }
                else
                {
                    assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
                    moduleBuilder = assemblyBuilder.DefineDynamicModule("Module");
                }
            }

            typeName = string.Format("BracedAttributeArgumentEvaluator.Evaluator{0}", Guid.NewGuid().ToString().RegexRemove("-"));
            typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public);

            currentTypeCount = moduleBuilder.GetTypes().Length;

            methodName = "Evaluate";

            methodEvaluateBuilder = typeBuilder.DefineMethod(methodName, MethodAttributes.Public, typeof(string), new Type[] { objType });

            this.methodGenerator = methodEvaluateBuilder.GetILGenerator();

            PreProcess();
        }

        private void PreProcess()
        {
            var constructor = typeof(object).GetConstructor(new Type[0]);

            methodGenerator.DeclareLocal(typeof(string));  //Declares a local variable to the method
        }

        public void PostProcess(int argCount)
        {
            var stringType = typeof(string);
            var objType = typeof(object);
            var parmsList = new List<Type>();
            MethodInfo method;
            Type[] parms;

            parmsList.Add(stringType);

            for (var x = 0; x < argCount; x++)
            {
                parmsList.Add(objType);
            }

            parms = parmsList.ToArray();

            method = stringType.GetMethod("Format", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, parms, null);

            methodGenerator.EmitCall(OpCodes.Call, method, parms);
            methodGenerator.Emit(OpCodes.Stloc_0);
            methodGenerator.Emit(OpCodes.Ldloc_0);

            methodGenerator.Emit(OpCodes.Ret);

            this.evaluatorType = typeBuilder.CreateType();
        }

        public void ProcessFormat(string formatString)
        {
            methodGenerator.Emit(OpCodes.Ldstr, formatString);
        }

        public void ProcessArg(CsNode arg, string code)
        {
            switch (arg.e)
            {
                case cs_node.n_block:

                    var block = (CsBlock)arg;

                    foreach (var statement in block.statements)
                    {
                        ProcessStatement(statement);
                    }

                    break;
                default:
                    Debugger.Break();
                    break;
            }
        }

        private Type ProcessResultExpression(CsNode node)
        {
            switch (node.e)
            {
                case cs_node.n_simple_name:

                    var simpleName = (CsSimpleName)node;
                    var memberResult = ProcessSimpleName(simpleName);

                    methodGenerator.Emit(OpCodes.Ldarg_1);

                    if (memberResult.IsField)
                    {
                        if (memberResult.IsPublic)
                        {
                            methodGenerator.Emit(OpCodes.Ldfld, memberResult.Field);
                        }
                        else
                        {
                            CallGetPrivateField(memberResult);
                        }

                        if (memberResult.IsBoxType)
                        {
                            methodGenerator.Emit(OpCodes.Box, memberResult.Type);
                        }

                        return memberResult.Type;
                    }
                    else if (memberResult.IsProperty)
                    {
                        if (memberResult.IsPublic)
                        {
                            var property = memberResult.Property;   
                            var method = property.GetGetMethod();

                            methodGenerator.EmitCall(OpCodes.Callvirt, method, null);
                        }
                        else
                        {
                            CallGetPrivateProperty(memberResult);
                        }

                        if (memberResult.IsBoxType)
                        {
                            methodGenerator.Emit(OpCodes.Box, memberResult.Type);
                        }

                        return memberResult.Type;
                    }

                    break;

                case cs_node.n_invocation_expression:

                default:
                    Debugger.Break();
                    break;
            }

            return null;
        }

        private Type ProcessResultIdentifier(CsIdentifier identifier, Type parentType = null)
        {
            MemberResult memberResult;
            
            if (parentType != null)
            {
                var previousType = objType;

                objType = parentType;
                memberResult = ProcessIdentifier(identifier);
                objType = previousType;
            }
            else
            {
                memberResult = ProcessIdentifier(identifier);
                methodGenerator.Emit(OpCodes.Ldarg_1);
            }

            if (memberResult.IsField)
            {
                if (memberResult.IsPublic)
                {
                    methodGenerator.Emit(OpCodes.Ldfld, memberResult.Field);
                }
                else
                {
                    CallGetPrivateField(memberResult);
                }

                if (memberResult.IsBoxType)
                {
                    methodGenerator.Emit(OpCodes.Box, memberResult.Type);
                }

                return memberResult.Type;
            }
            else if (memberResult.IsProperty)
            {
                if (memberResult.IsPublic)
                {
                    var property = memberResult.Property;
                    var method = property.GetGetMethod();

                    methodGenerator.EmitCall(OpCodes.Callvirt, method, null);
                }
                else
                {
                    CallGetPrivateProperty(memberResult);
                }

                if (memberResult.IsBoxType)
                {
                    methodGenerator.Emit(OpCodes.Box, memberResult.Type);
                }

                return memberResult.Type;
            }

            return null;
        }

        private void CallGetPrivateProperty(MemberResult memberResult)
        {
            var typeExtensionsType = typeof(Utils.TypeExtensions);
            var method = typeExtensionsType.GetMethod("GetPrivatePropertyValue", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod);

            method = method.MakeGenericMethod(memberResult.Type);

            methodGenerator.Emit(OpCodes.Ldstr, memberResult.Name);

            methodGenerator.EmitCall(OpCodes.Call, method, null);
        }

        private void CallGetPrivateField(MemberResult memberResult)
        {
            var typeExtensionsType = typeof(Utils.TypeExtensions);
            var method = typeExtensionsType.GetMethod("GetPrivateFieldValue", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod);

            method = method.MakeGenericMethod(memberResult.Type);

            methodGenerator.Emit(OpCodes.Ldstr, memberResult.Name);

            methodGenerator.EmitCall(OpCodes.Call, method, null);
        }

        private MemberResult ProcessSimpleName(CsSimpleName name)
        {
            var result = new MemberResult();
            var fieldInfo = objType.GetField(name.identifier.original_text, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);

            if (fieldInfo != null)
            {
                result.MemberInfo = fieldInfo;
                result.IsPublic = fieldInfo.IsPublic;
            }
            else
            {
                var propertyInfo = objType.GetProperty(name.identifier.original_text, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);

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
            var fieldInfo = objType.GetField(identifier.original_text, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);

            if (fieldInfo != null)
            {
                result.MemberInfo = fieldInfo;
                result.IsPublic = fieldInfo.IsPublic;
            }
            else
            {
                var propertyInfo = objType.GetProperty(identifier.original_text, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);

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

        private Type ProcessExpression(CsExpression expression)
        {
            switch (expression.e)
            {
                case cs_node.n_binary_expression:

                    var binaryExpression = (CsBinaryExpression)expression;

                    ProcessBinaryExpression(binaryExpression);
                    break;

                case cs_node.n_simple_name:
                    return ProcessResultExpression(expression);
                case cs_node.n_invocation_expression:
                    return ProcessResultExpression(expression);
                case cs_node.n_primary_expression_member_access:

                    var primaryExpression = (CsPrimaryExpressionMemberAccess) expression;

                    ProcessPrimaryExpressionMemberAccess(primaryExpression);
                    break;

                default:
                    Debugger.Break();
                    break;
            }

            return null;
        }

        private void ProcessPrimaryExpressionMemberAccess(CsPrimaryExpressionMemberAccess expression)
        {
            var type = ProcessExpression(expression.expression);
            ProcessResultIdentifier(expression.identifier, type);
        }

        private void ProcessBinaryExpression(CsBinaryExpression binaryExpression)
        {
            var left = ProcessResultExpression(binaryExpression.lhs);
            var right = ProcessResultExpression(binaryExpression.rhs);

            Debug.Assert(left == right);

            switch (left.Name)
            {
                case "String":
                    ProcessStringOperator(binaryExpression.oper);
                    break;
            }
        }

        private void ProcessStringOperator(CsTokenType csTokenType)
        {
            switch (csTokenType)
            {
                case CsTokenType.tkPLUS:

                    var stringType = typeof(string);
                    var parms = new Type[] { stringType, stringType };
                    var method = stringType.GetMethod("Concat", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, parms, null);

                    methodGenerator.EmitCall(OpCodes.Call, method, parms);

                    break;
            }
        }

        public void Save()
        {
            assemblyBuilder.Save(Path.GetFileName(fileName));
        }

        public string Evaluate(object obj)
        {
            var methodInfo = evaluatorType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
            var evaluator = Activator.CreateInstance(evaluatorType);

            var result = (string)methodInfo.Invoke(evaluator, BindingFlags.Public | BindingFlags.CreateInstance, null, new object[] { obj }, Thread.CurrentThread.CurrentCulture);

            return result;
        }

        private class MemberResult
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

            public bool IsBoxType
            {
                get
                {
                    return this.Type.IsValueType;
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
