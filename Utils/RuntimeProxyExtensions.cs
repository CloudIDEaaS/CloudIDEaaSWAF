using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public static class RuntimeProxyExtensions
    {
        public static T WrapInstance<T>(this IRuntimeProxy proxy, string identifier)
        {
            return (T) proxy.WrapInstance(identifier, typeof(T));
        }

        public static object WrapInstance(this IRuntimeProxy proxy, string identifier, Type wrapperType)
        {
            /// assembly/type ******************************************************************************************************************
            /// 

#if NETCOREAPP3_1_OR_GREATER
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("DynamicProxyAssembly"), AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyBuilder.GetName().Name);
#else
            var assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(new AssemblyName("DynamicProxyAssembly"), AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyBuilder.GetName().Name, false);
#endif

            // This NewGuid call is just to get a unique name for the new construct
            var typeName = wrapperType.Name + "_Proxy_" + Guid.NewGuid().ToString();
            TypeBuilder typeBuilder;

            if (wrapperType.IsInterface)
            {
                typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public
                    | TypeAttributes.Class
                    | TypeAttributes.AutoClass
                    | TypeAttributes.AnsiClass
                    | TypeAttributes.BeforeFieldInit
                    | TypeAttributes.AutoLayout, typeof(object), new Type[] { wrapperType }
                );
            }
            else
            {
                typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public
                    | TypeAttributes.Class
                    | TypeAttributes.AutoClass
                    | TypeAttributes.AnsiClass
                    | TypeAttributes.BeforeFieldInit
                    | TypeAttributes.AutoLayout, wrapperType
                );
            }

            /// constructor ******************************************************************************************************************
            /// 

            var proxyField = typeBuilder.DefineField("proxy", typeof(IRuntimeProxy), FieldAttributes.Private);
            var identifierField = typeBuilder.DefineField("identifier", typeof(string), FieldAttributes.Private);

            var ctorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { typeof(IRuntimeProxy), typeof(string) });

            // Generate: base.ctor()
            // 
            var ilCtor = ctorBuilder.GetILGenerator();
            ilCtor.Emit(OpCodes.Ldarg_0);
            ilCtor.Emit(OpCodes.Call, typeBuilder.BaseType.GetConstructor(Type.EmptyTypes));

            // Generate: proxy = proxy
            // 
            ilCtor.Emit(OpCodes.Ldarg_0);
            ilCtor.Emit(OpCodes.Ldarg_1);
            ilCtor.Emit(OpCodes.Stfld, proxyField);
            ilCtor.Emit(OpCodes.Ldarg_0);
            ilCtor.Emit(OpCodes.Ldarg_2);
            ilCtor.Emit(OpCodes.Stfld, identifierField);

            // All done!
            // 
            ilCtor.Emit(OpCodes.Ret);

            /// properties ******************************************************************************************************************
            /// 

            foreach (var property in wrapperType.GetProperties())
            {
                var propertyGetMember = typeof(IRuntimeProxy).GetMethod("PropertyGet").MakeGenericMethod(property.PropertyType);

                // Prepare the property we'll add get and/or set accessors to

                var propBuilder = typeBuilder.DefineProperty(property.Name, PropertyAttributes.None, property.PropertyType, Type.EmptyTypes);

                // Define get method, if required
                // 
                if (property.CanRead)
                {
                    var getFuncBuilder = typeBuilder.DefineMethod("get_" + property.Name, MethodAttributes.Public
                        | MethodAttributes.HideBySig
                        | MethodAttributes.NewSlot
                        | MethodAttributes.SpecialName
                        | MethodAttributes.Virtual
                        | MethodAttributes.Final, property.PropertyType, Type.EmptyTypes
                    );

                    // Generate:
                    //   return _src.GetType().InvokeMember(property.Name, BindingFlags.GetProperty, null, _src, null)

                    var ilFunc = getFuncBuilder.GetILGenerator();

                    ilFunc.Emit(OpCodes.Ldarg_0);
                    ilFunc.Emit(OpCodes.Ldfld, proxyField);
                    ilFunc.Emit(OpCodes.Ldarg_0);
                    ilFunc.Emit(OpCodes.Ldfld, identifierField);
                    ilFunc.Emit(OpCodes.Ldstr, property.Name);
                    ilFunc.Emit(OpCodes.Callvirt, propertyGetMember);

                    if (property.PropertyType.IsValueType)
                    {
                        ilFunc.Emit(OpCodes.Unbox_Any, property.PropertyType);
                    }

                    ilFunc.Emit(OpCodes.Ret);

                    propBuilder.SetGetMethod(getFuncBuilder);
                }

                // Define set method, if required

                if (property.CanWrite)
                {
                    var setFuncBuilder = typeBuilder.DefineMethod(
                      "set_" + property.Name,
                      MethodAttributes.Public
                        | MethodAttributes.HideBySig
                        | MethodAttributes.SpecialName
                        | MethodAttributes.Virtual,
                      null,
                      new Type[] { property.PropertyType }
                    );
                    var valueParameter = setFuncBuilder.DefineParameter(1, ParameterAttributes.None, "value");
                    var ilFunc = setFuncBuilder.GetILGenerator();

                    var propertySetMember = typeof(IRuntimeProxy).GetMethod("PropertySet").MakeGenericMethod(property.PropertyType);

                    // Generate:
                    //   _src.GetType().InvokeMember(
                    //     property.Name, BindingFlags.SetProperty, null, _src, new object[1] { value }
                    //   );
                    // Note: Need to declare assignment of local array to pass to InvokeMember (argValues)
                    // 

                    ilFunc.Emit(OpCodes.Ldarg_0);
                    ilFunc.Emit(OpCodes.Ldfld, proxyField);
                    ilFunc.Emit(OpCodes.Ldarg_0);
                    ilFunc.Emit(OpCodes.Ldfld, identifierField);
                    ilFunc.Emit(OpCodes.Ldstr, property.Name);
                    ilFunc.Emit(OpCodes.Ldarg_1);
                    if (property.PropertyType.IsValueType)
                        ilFunc.Emit(OpCodes.Box, property.PropertyType);
                    ilFunc.Emit(OpCodes.Callvirt, propertySetMember);
                    ilFunc.Emit(OpCodes.Ret);

                    propBuilder.SetSetMethod(setFuncBuilder);
                }
            }

            /// methods ******************************************************************************************************************
            /// 

            foreach (var method in wrapperType.GetMethods())
            {
                var parameters = method.GetParameters();
                var parameterTypes = new List<Type>();
                
                foreach (var parameter in parameters)
                {
                    if (parameter.IsOut)
                        throw new ArgumentException("Output parameters are not supported");
                    if (parameter.IsOptional)
                        throw new ArgumentException("Optional parameters are not supported");
                    if (parameter.ParameterType.IsByRef)
                        throw new ArgumentException("Ref parameters are not supported");
                    parameterTypes.Add(parameter.ParameterType);
                }

                var funcBuilder = typeBuilder.DefineMethod(
                  method.Name,
                  MethodAttributes.Public
                    | MethodAttributes.HideBySig
                    | MethodAttributes.NewSlot
                    | MethodAttributes.Virtual
                    | MethodAttributes.Final,
                  method.ReturnType,
                  parameterTypes.ToArray()
                );
                var ilFunc = funcBuilder.GetILGenerator();

                // Generate: object[] args

                var argValues = ilFunc.DeclareLocal(typeof(object[]));

                // Generate: args = new object[x]
                ilFunc.Emit(OpCodes.Ldc_I4, parameters.Length);
                ilFunc.Emit(OpCodes.Newarr, typeof(Object));
                ilFunc.Emit(OpCodes.Stloc_0);

                for (var index = 0; index < parameters.Length; index++)
                {
                    // Generate: args[n] = ..;
                    var parameter = parameters[index];
                    ilFunc.Emit(OpCodes.Ldloc_0);
                    ilFunc.Emit(OpCodes.Ldc_I4, index);
                    ilFunc.Emit(OpCodes.Ldarg, index + 1);
                    if (parameter.ParameterType.IsValueType)
                        ilFunc.Emit(OpCodes.Box, parameter.ParameterType);
                    ilFunc.Emit(OpCodes.Stelem_Ref);
                }

                MethodInfo callMethodMember;

                if (method.ReturnType == typeof(void))
                {
                    callMethodMember = typeof(IRuntimeProxy).GetMethod("CallMethod").MakeGenericMethod(typeof(object));
                }
                else
                {
                    callMethodMember = typeof(IRuntimeProxy).GetMethod("CallMethod").MakeGenericMethod(method.ReturnType);
                }

                // Generate:
                //   [return] T CallMethod<T>(string identifier, string methodName, params object[] args);

                ilFunc.Emit(OpCodes.Ldarg_0);
                ilFunc.Emit(OpCodes.Ldfld, proxyField);
                ilFunc.Emit(OpCodes.Ldarg_0);
                ilFunc.Emit(OpCodes.Ldfld, identifierField);
                ilFunc.Emit(OpCodes.Ldstr, method.Name);
                ilFunc.Emit(OpCodes.Ldloc_0);
                ilFunc.Emit(OpCodes.Callvirt, callMethodMember);

                if (method.ReturnType.Equals(typeof(void)))
                {
                    ilFunc.Emit(OpCodes.Pop);
                }
                else if (method.ReturnType.IsValueType)
                {
                    ilFunc.Emit(OpCodes.Unbox_Any, method.ReturnType);
                }

                ilFunc.Emit(OpCodes.Ret);
            }

            var wrapper = Activator.CreateInstance(typeBuilder.CreateType(), proxy, identifier);

            return wrapper;
        }
    }
}
