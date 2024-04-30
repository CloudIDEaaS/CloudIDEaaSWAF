using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.CSharp;
using System.CodeDom;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Reflection.Emit;
using Utils.Parsing;
using Utils.Parsing.Nodes;
using System.Xml.Linq;
using System.Numerics;
using System.CodeDom.Compiler;
using BTreeIndex.Collections.Generic.BTree;
using Aspose.Words.Math;

namespace Utils
{
	public static class TypeExtensions
	{
		public static class ScalarType
		{
			public static List<Type> Types
			{
				get
				{
					return new List<Type>()
				{
					typeof(Guid),
					typeof(DateTime),
					typeof(string)
				};
				}
			}
		}

		public class ReflectionMemberCategoryNameAttribute : Attribute
		{
			public string Name { get; private set; }
			public string PluralName { get; private set; }

			public ReflectionMemberCategoryNameAttribute(string name)
			{
				this.Name = name;
				this.PluralName = name.Pluralize();
			}
		}

		public static class ReflectionModifierStrings
		{
			public static string Public = "public";
			public static string Internal = "internal";
			public static string Protected = "protected";
			public static string Private = "private";
			public static string HideBySig = "hidebysig";
			public static string Abstract = "abstract";
			public static string Const = "const";
			public static string Extern = "extern";
			public static string Override = "override";
			public static string Partial = "partial";
			public static string ReadOnly = "readonly";
			public static string Sealed = "sealed";
			public static string Final = "final";
			public static string Static = "static";
			public static string Unsafe = "unsafe";
			public static string Virtual = "virtual";
			public static string Volatile = "volatile";
			public static string New = "new";
			public static string Async = "async";
		}

		[Flags]
		public enum ReflectionModifiers
		{
			None = 0,
			Public = 1 << 1,
			Internal = 1 << 2,
			Protected = 1 << 3,
			Private = 1 << 4,
			HideBySig = 1 << 4,
			Abstract = 1 << 5,
			Const = 1 << 7,
			Extern = 1 << 8,
			Override = 1 << 9,
			Partial = 1 << 10,
			ReadOnly = 1 << 11,
			Sealed = 1 << 12,
			Final = 1 << 13,
			Static = 1 << 14,
			Unsafe = 1 << 15,
			Virtual = 1 << 16,
			Volatile = 1 << 17,
			New = 1 << 18,
			Async = 1 << 19,
			AccessibilityMask = 0x1f
		}

		public enum ReflectionMemberCategory
		{
			[ReflectionMemberCategoryName("Unspecified")]
			Unspecified,
			[ReflectionMemberCategoryName("Method")]
			Method,
			[ReflectionMemberCategoryName("Property")]
			Property,
			[ReflectionMemberCategoryName("Event")]
			Event,
			[ReflectionMemberCategoryName("Indexer")]
			Indexer,
			[ReflectionMemberCategoryName("Operator")]
			Operator,
			[ReflectionMemberCategoryName("Constructor")]
			Constructor,
			[ReflectionMemberCategoryName("Destructor")]
			Destructor,
			[ReflectionMemberCategoryName("Constant")]
			Constant,
			[ReflectionMemberCategoryName("Field")]
			Field,
			[ReflectionMemberCategoryName("Explicit Interface Implementation")]
			ExplicitInterfaceImplementation,
			[ReflectionMemberCategoryName("Extension Method")]
			ExtensionMethod,
			[ReflectionMemberCategoryName("Nested Type")]
			NestedType,
			[ReflectionMemberCategoryName("Implemented Interface")]
			ImplementedInterface,
			[ReflectionMemberCategoryName("Base Type")]
			BaseType,
			PrimitiveType,
			Type,
			Delegate,
			Enum,
			Struct,
			Interface,
			EnumItem
		}

        public static Delegate CreateDelegate(this MethodInfo methodInfo, object? target = null)
        {
            var parmTypes = methodInfo.GetParameters().Select(parm => parm.ParameterType);
            var parmAndReturnTypes = parmTypes.Append(methodInfo.ReturnType).ToArray();
            var delegateType = System.Linq.Expressions.Expression.GetDelegateType(parmAndReturnTypes);

			if (methodInfo.IsStatic)
			{
				return methodInfo.CreateDelegate(delegateType);
			}

            return methodInfo.CreateDelegate(delegateType, target);
        }

        public static string GetPrimitiveTypeFullName(string shortTypeName)
		{
			switch (shortTypeName)
			{
				case "string":
					return "System.String";
				case "int":
					return "System.Int32";
				case "uint":
					return "System.UInt32";
				case "object":
					return "System.Object";
				case "bool":
					return "System.Boolean";
				case "sbyte":
					return "System.SByte";
				case "byte":
					return "System.Byte";
				case "short":
					return "System.Int16";
				case "ushort":
					return "System.UInt16";
				case "long":
					return "System.Int64";
				case "ulong":
					return "System.UInt64";
				case "float":
					return "System.Single";
				case "double":
					return "System.Double";
				case "decimal":
					return "System.Decimal";
				case "char":
					return "System.Char";
				case "void":
					return "System.Void";
				default:
					DebugUtils.Break();
					return null;
			}
		}

		public static Type GetType(string typeName, TypeCache typeCache = null)
		{
			var type = Type.GetType(typeName);

			if (type == null)
			{
				if (typeCache != null)
				{
					if (typeCache.ContainsKey(typeName))
					{
						return typeCache[typeName];
					}
					else
					{
						if (typeCache.IsStale)
						{
							try
							{
								// exported types always ruin the party

								var exportedTypes = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.FullName.StartsWith("Anonymously Hosted")).SelectMany(a =>
								{
									try
									{
										return a.ExportedTypes;
									}
									catch
									{
										return new Type[0];
									}
								});

								typeCache.BuildIndex(exportedTypes);

								if (typeCache.ContainsKey(typeName))
								{
									return typeCache[typeName];
								}
								else
								{
									return null;
								}
							}
							catch
							{
							}
						}
						else
						{
							return null;
						}
					}
				}
				else
				{
					try
					{
						// exported types always ruin the party

						var exportedTypes = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.FullName.StartsWith("Anonymously Hosted")).SelectMany(a =>
						{
							try
							{
								return a.ExportedTypes;
							}
							catch
							{
								return new Type[0];
							}
						});

						type = exportedTypes.SingleOrDefault(t => t.FullName == typeName);
					}
					catch
					{
					}
				}
			}

			return type;
		}

		public static bool IsType(this ReflectionMemberCategory category)
		{
			return category.IsOneOf(ReflectionMemberCategory.ImplementedInterface,
				ReflectionMemberCategory.NestedType,
				ReflectionMemberCategory.BaseType,
				ReflectionMemberCategory.PrimitiveType,
				ReflectionMemberCategory.Type,
				ReflectionMemberCategory.Delegate,
				ReflectionMemberCategory.Enum,
				ReflectionMemberCategory.Struct,
				ReflectionMemberCategory.Interface);
		}

		public static XDocument GetAssemblyDocumentation(this Type type, BTreeDictionary<string, List<FileSystemInfo>> bTreeDictionary = null)
		{
			return type.Assembly.GetAssemblyDocumentation(bTreeDictionary);
		}

		public static XDocument GetAssemblyDocumentation(this MemberInfo member, BTreeDictionary<string, List<FileSystemInfo>> bTreeDictionary = null)
		{
			return member.DeclaringType.Assembly.GetAssemblyDocumentation(bTreeDictionary);
		}

		public static XDocument GetAssemblyDocumentation(this Assembly assembly, BTreeDictionary<string, List<FileSystemInfo>> bTreeDictionary = null)
		{
			var location = assembly.Location;
			var fileName = Path.GetFileNameWithoutExtension(location) + ".xml";
			var fullFileName = Path.Combine(Path.GetDirectoryName(location), fileName);
			DirectoryInfo directory;
			FileInfo file;

			if (File.Exists(fullFileName))
			{
				return XDocument.Load(fullFileName);
			}
			else
			{
				string programFilesDirectory;

				programFilesDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

                location = Path.Combine(programFilesDirectory, @"Reference Assemblies\Microsoft\Framework");
                directory = new DirectoryInfo(location);

				if (bTreeDictionary.AddToBTreeIfExists(directory, fileName, (f) => f.FullName.Split(@"\").Last(), out file))
				{
					return XDocument.Load(file.FullName);
				}

                programFilesDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

				location = Path.Combine(programFilesDirectory, @"Reference Assemblies\Microsoft\Framework");
				directory = new DirectoryInfo(location);

                if (bTreeDictionary.AddToBTreeIfExists(directory, fileName, (f) => f.FullName.Split(@"\").Last(), out file))
                {
                    return XDocument.Load(file.FullName);
                }
            }

            return null;
		}

        public static bool InheritsFrom(this Type type, Type inheritingFromType)
        {
            return type.GetAncestors().Any(t => t == inheritingFromType);
        }

        public static bool IsOrInheritsFrom(this Type type, Type inheritingFromType)
        {
            var any = type.GetAncestors(true).Any(t => t == inheritingFromType);

            return any;
        }

        public static bool InheritsFrom<TInheritsFrom>(this Type type)
        {
            return type.GetAncestors().Any(t => t == typeof(TInheritsFrom));
        }

        public static bool IsOrInheritsFrom<T>(this Type type)
        {
            var any = type.GetAncestors(true).Any(t => t == typeof(T));

            return any;
        }

        private static bool AddToBTreeIfExists(this BTreeDictionary<string, List<FileSystemInfo>> bTreeDictionary, DirectoryInfo directory, string fileName, Func<FileInfo, string> orderBy, out FileInfo file)
		{
			var extension = Path.GetExtension(fileName);
            List<FileSystemInfo> list;

			if (!bTreeDictionary.ContainsKey(directory.FullName))
			{
				foreach (var file2 in directory.GetFiles("*" + extension, SearchOption.AllDirectories))
				{
					if (bTreeDictionary.ContainsKey(file2.Name))
					{
						list = bTreeDictionary[file2.Name];
                    }
					else
					{
						list = new List<FileSystemInfo>();

                        bTreeDictionary.Add(file2.Name, list);
					}

                    list.Add(file2);
				}

				bTreeDictionary.Add(directory.FullName, new List<FileSystemInfo>() { directory });
            }

			if (bTreeDictionary.ContainsKey(fileName))
			{
                list = bTreeDictionary[fileName];
				file = list.Cast<FileInfo>().OrderBy(orderBy).Last();
            }

			file = null;
			return false;
		}

		public static bool TryCast<T>(this object obj, Action<T> action)
		{
			if (obj is T)
			{
				action((T)obj);
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool TryCast<T>(this object obj, Action<T> action, Func<bool> elseAction)
		{
			if (obj is T)
			{
				action((T)obj);
				return true;
			}
			else
			{
				return elseAction();
			}
		}

		public static bool TryCast<T>(this object obj, Action<T> action, Action elseAction)
		{
			if (obj is T)
			{
				action((T)obj);
				return true;
			}
			else
			{
				elseAction();
				return false;
			}
		}

		public static string GetNameOrNestedName(this Type type)
		{
			if (type.IsNested)
			{
				return type.ReflectedType.Name + "." + type.Name;
			}
			else
			{
				return type.Name;
			}
		}

		public static bool CanBeNull(this Type type)
		{
			if (type.IsValueType)
			{
				return type.IsValueType && type.IsGenericType && !type.IsGenericTypeDefinition && type.GetGenericTypeDefinition() == typeof(Nullable<>);
			}
			else if (!type.IsValueType)
			{
				return true;
			}

			return false;
		}

		public static bool IsNullable(this Type type)
		{
			return type.IsValueType && type.IsGenericType && !type.IsGenericTypeDefinition && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		public static IEnumerable<Type> FindLowestLevelInterfacesOf<T>(this Type type)
		{
			var interfaces = type.GetInterfaces();
			var interfacesBaseTypes = interfaces.Concat(new List<Type>() { type.BaseType });
			var list = new List<Type>();
			var found = false;

			// although similarly named from one below.  It is totally different.
			// This one searches up the hierarchy until finding the first of type T

			while (!found)
			{
				foreach (var interfaceOrBaseType in interfacesBaseTypes)
				{
					if (interfaceOrBaseType.IsInterface && interfaceOrBaseType.IsOfType<T>())
					{
						list.Add(interfaceOrBaseType);
						found = true;
					}
					else if (interfaceOrBaseType.IsInterface && interfaceOrBaseType.Implements<T>())
					{
						list.Add(interfaceOrBaseType);
						found = true;
					}
				}

				if (!found)
				{
					interfacesBaseTypes = interfacesBaseTypes.SelectMany(t => t.GetInterfaces());
					type = type.BaseType;

					if (type != null)
					{
						interfacesBaseTypes = interfacesBaseTypes.Concat(new List<Type>() { type });
					}
				}

				if (interfacesBaseTypes.Count() == 0)
				{
					break;
				}
			}

			if (list.Count > 1)
			{
				var inheritedInterfaces = list.SelectMany(i => i.GetInterfaces().Where(i2 => list.Contains(i2)));

				foreach (var inheritedInterface in inheritedInterfaces.ToList())
				{
					list.Remove(inheritedInterface);
				}
			}

			return list;
		}

		public static IEnumerable<Type> GetLowestLevelInterfaces(this Type type)
		{
			var interfaces = type.GetInterfaces();
			var ancestors = type.GetAncestors();
			var interfacesAncestors = interfaces.Concat(ancestors);
			var list = new List<Type>();

			// get any interface that neither a base class or base interfaces implement.  
			// i.e. any immediately implemented interfaces

			foreach (var _interface in interfaces)
			{
				if (!interfacesAncestors.Any(a => a.GetInterfaces().Any(a2 => a2 == _interface)))
				{
					if (!list.Contains(_interface))
					{
						list.Add(_interface);
					}
				}
			}

			return list;
		}

		public static IEnumerable<Type> GetImmediatelyDerivedTypes(this Type type)
		{
			var assembly = type.Assembly;

			return assembly.GetTypes().Where(t => t.BaseType == type);
		}

		public static IEnumerable<Type> GetAllDerivedTypes(this Type type, bool includeInterfaces = false)
		{
			var assembly = type.Assembly;
			var allTypes = assembly.GetTypes();
			
			return allTypes.Where(t => t.GetAncestors(false, includeInterfaces).Any(a => a == type));
		}

		public static ReflectionModifiers ApplyAccessibilityMask(this ReflectionModifiers reflectionModifiers)
		{
			return reflectionModifiers & ReflectionModifiers.AccessibilityMask;
		}

		public static bool IsDelegate(this Type type)
		{
			return typeof(MulticastDelegate).IsAssignableFrom(type.BaseType);
		}

		public static bool IsScalar(this Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				type = type.GetGenericArguments().Single();

				return type.IsScalar();
			}
			else if (type.Name.EndsWith("&"))
			{
				type = type.GetElementType();

				return type.IsScalar();
			}
			else if (type.IsArray)
			{
				type = type.GetElementType();

				return type.IsScalar();
			}
			else
			{
				return type.IsPrimitive || type.IsEnum || type.IsOneOf(typeof(string), typeof(Guid), typeof(DateTime), typeof(BigInteger));
			}
		}

		public static bool IsAtomicallyEnum(this Type type)
		{
			if (type.Name.EndsWith("&"))
			{
				type = type.GetElementType();
			}
			else if (type.IsArray)
			{
				type = type.GetElementType();
			}

			return type.IsEnum;
		}

		public static bool IsAction(this Type type)
		{
			Type generic = null;

			if (type == typeof(System.Action))
			{
				return true;
			}

			if (type.IsGenericTypeDefinition)
			{
				generic = type;
			}
			else if (type.IsGenericType)
			{
				generic = type.GetGenericTypeDefinition();
			}

			if (generic == null)
			{
				return false;
			}
			else 
			{
				foreach (var actionType in typeof(System.Action<>).Assembly.GetTypes().Where(t => t.Namespace == "System" && (t.Name == "Action" || t.Name.StartsWith("Action`"))))
				{
					if (generic == actionType)
					{
						return true;
					}
				}
			}

			return false;
		}

		public static bool IsFunc(this Type type)
		{
			Type generic = null;

			if (type == typeof(System.Action))
			{
				return true;
			}

			if (type.IsGenericTypeDefinition)
			{
				generic = type;
			}
			else if (type.IsGenericType)
			{
				generic = type.GetGenericTypeDefinition();
			}

			if (generic == null)
			{
				return false;
			}
			else
			{
				foreach (var funcType in typeof(System.Func<>).Assembly.GetTypes().Where(t => t.Namespace == "System" && (t.Name == "Func" || t.Name.StartsWith("Func`"))))
				{
					if (generic == funcType)
					{
						return true;
					}
				}
			}

			return false;
		}

		public static Type GetMemberType(this MemberInfo member)
		{
			if (member is PropertyInfo)
			{
				return ((PropertyInfo)member).PropertyType;
			}
			else if (member is FieldInfo)
			{
				return ((FieldInfo)member).FieldType;
			}
			else if (member is MethodInfo)
			{
				return ((MethodInfo)member).ReturnType;
			}
			else if (member is Type)
			{
				return (Type)member;
			}

			Debugger.Break();
			return null;
		}

		public static bool Is<T>(this object obj)
		{
			return obj is T;
		}

		public static string GetMemberCategoryName(this ReflectionMemberCategory category)
		{
			var field = EnumUtils.GetField<ReflectionMemberCategory>(category);
			var attribute = field.GetCustomAttribute<ReflectionMemberCategoryNameAttribute>();

			return attribute.Name;
		}

		public static string GetMemberCategoryPluralName(this ReflectionMemberCategory category)
		{
			var field = EnumUtils.GetField<ReflectionMemberCategory>(category);
			var attribute = field.GetCustomAttribute<ReflectionMemberCategoryNameAttribute>();

			return attribute.PluralName;
		}

		public static bool SignatureMatches(this MethodInfo m, MethodInfo method)
		{
			var parms1 = m.GetParameters();
			var parms2 = method.GetParameters();

			if (parms1.Length != parms2.Length)
			{
				return false;
			}

			if (m.ReturnType != method.ReturnType)
			{
				return false;
			}

			for (var x = 0; x < parms1.Length; x++)
			{
				if (parms1[x].ParameterType != parms2[x].ParameterType)
				{
					return false;
				}
			}

			return true;
		}

		public static bool IsOperatorAssignableFrom(this Type type, Type typeToConvertFrom)
		{
			return type.GetMethods(BindingFlags.Public | BindingFlags.Static).Any(m => m.Name == "op_Implicit" && m.ReturnType == type && m.GetParameters().Length == 1 && m.GetParameters().SingleOrDefault(p => p.ParameterType == typeToConvertFrom) != null);
		}

		public static object OperatorAssignFrom(this Type type, object obj)
		{
			var typeToConvertFrom = obj.GetType();
			var method = type.GetMethods(BindingFlags.Public | BindingFlags.Static).Single(m => m.Name == "op_Implicit" && m.ReturnType == type && m.GetParameters().Length == 1 && m.GetParameters().SingleOrDefault(p => p.ParameterType == typeToConvertFrom) != null);

			return method.Invoke(null, new object[] { obj });
		}

		public static DotNetType GetDotNetType(this VariantType type)
		{
			var variantType = typeof(VariantType);
			var field = variantType.GetField(Enum<VariantType>.GetName(type));

			if (field.HasCustomAttribute<DotNetTypeAttribute>())
			{
				var attr = field.GetCustomAttribute<DotNetTypeAttribute>();

				return attr.DotNetType;
			}
			else
			{
				return DotNetType.Unknown;
			}
		}

		public static IEnumerable<FieldInfo> GetDependencyProperties(this object obj)
		{
			var flags = BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public;
			var dependencyProperties = obj.GetType().GetFields(flags).Where(f => f.FieldType.IsOfType("System.Windows.DependencyProperty"));

			return dependencyProperties;
		}

		public static IEnumerable<FieldInfo> GetDependencyProperties(this Type type)
		{
			var flags = BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public;
			var dependencyProperties = type.GetFields(flags).Where(f => f.FieldType.IsOfType("System.Windows.DependencyProperty"));

			return dependencyProperties;
		}

		public static IEnumerable<FieldInfo> GetConstants(this Type type)
		{
			var flags = BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
			var fields = type.GetFields(flags).Where(f => f.IsLiteral);

			return fields;
		}

		public static IEnumerable<FieldInfo> GetConstants<T>(this Type type)
		{
			var flags = BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
			var fields = type.GetFields(flags).Where(f => f.IsLiteral);

			return fields.Where(f => f.FieldType == typeof(T));
		}

		public static FieldInfo GetConstant<T>(this Type type, Func<T, bool> func)
		{
			return type.GetConstants<T>().SingleOrDefault(f => func((T) f.GetRawConstantValue()));
		}

		public static IEnumerable<MethodInfo> GetExtensionMethods(this Type type)
		{
			var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
				.Where(m => m.IsDefined(typeof(ExtensionAttribute), false));

			return methods;
		}

		public static IEnumerable<MethodInfo> GetExtensionMethods(this Type type, BindingFlags flags)
		{
			var methods = type.GetMethods(BindingFlags.Static | flags)
				.Where(m => m.IsDefined(typeof(ExtensionAttribute), false));

			return methods;
		}

#if !SILVERLIGHT
		public static string GetShortName(this Type type)
		{
			var compiler = new CSharpCodeProvider();
			var typeRef = new CodeTypeReference(type);
			var name = compiler.GetTypeOutput(typeRef);

			if (name == type.FullName)
			{
				name = type.Name;
			}

			return name;
		}
#endif
        public static object GetDefaultValue<T>() where T : new()
        {
			var type = typeof(T);

            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        public static object GetDefaultValue(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }

        public static T GetPrivateFieldValue<T>(this object obj, string field)
		{
			var type = obj.GetType();
			var fieldInfo = type.GetField(field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetField);
			T value = default(T);

			if (fieldInfo != null)
			{
				value = (T)fieldInfo.GetValue(obj);
			}
			else
			{
				var baseType = type.BaseType;

				while (baseType != null)
				{
					fieldInfo = baseType.GetField(field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetField);

					if (fieldInfo != null)
					{
						value = (T)fieldInfo.GetValue(obj);
						break;
					}

					baseType = baseType.BaseType;
				}
			}

			return value;
		}

		public static T GetFieldValue<T>(this object obj, string field)
		{
			var type = obj.GetType();
			var fieldInfo = type.GetField(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField);
			T value = default(T);

			if (fieldInfo != null)
			{
				value = (T)fieldInfo.GetValue(obj);
			}
			else
			{
				fieldInfo = type.GetField(field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetField);

				if (fieldInfo != null)
				{
					value = (T)fieldInfo.GetValue(obj);
				}
				else
				{
					var baseType = type.BaseType;

					while (baseType != null)
					{
						fieldInfo = baseType.GetField(field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetField);

						if (fieldInfo != null)
						{
							value = (T)fieldInfo.GetValue(obj);
							break;
						}

						baseType = baseType.BaseType;
					}
				}
			}

			return value;
		}

		public static void SetFieldValue<T>(this object obj, string field, T value)
		{
			var type = obj.GetType();
			var fieldInfo = type.GetField(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField);

			if (fieldInfo != null)
			{
				fieldInfo.SetValue(obj, value);
			}
			else
			{
				fieldInfo = type.GetField(field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetField);

				if (fieldInfo != null)
				{
					fieldInfo.SetValue(obj, value);
				}
				else
				{
					var baseType = type.BaseType;

					while (baseType != null)
					{
						fieldInfo = baseType.GetField(field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetField);

						if (fieldInfo != null)
						{
							fieldInfo.SetValue(obj, value);
							break;
						}

						baseType = baseType.BaseType;
					}
				}
			}
		}

		public static T GetStaticFieldValue<T>(this object obj, string field)
		{
			var type = obj.GetType();
			var fieldInfo = type.GetField(field, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField);
			T value = default(T);

			if (fieldInfo != null)
			{
				value = (T)fieldInfo.GetValue(obj);
			}
			else
			{
				fieldInfo = type.GetField(field, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetField);

				if (fieldInfo != null)
				{
					value = (T)fieldInfo.GetValue(obj);
				}
				else
				{
					var baseType = type.BaseType;

					while (baseType != null)
					{
						fieldInfo = baseType.GetField(field, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetField);

						if (fieldInfo != null)
						{
							value = (T)fieldInfo.GetValue(obj);
							break;
						}

						baseType = baseType.BaseType;
					}
				}
			}

			return value;
		}

		public static T GetStaticFieldValue<T>(this Type type, string field)
		{
			var fieldInfo = type.GetField(field, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField);
			T value = default(T);

			if (fieldInfo != null)
			{
				value = (T)fieldInfo.GetValue(null);
			}
			else
			{
				fieldInfo = type.GetField(field, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetField);

				if (fieldInfo != null)
				{
					value = (T)fieldInfo.GetValue(null);
				}
				else
				{
					var baseType = type.BaseType;

					while (baseType != null)
					{
						fieldInfo = baseType.GetField(field, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetField);

						if (fieldInfo != null)
						{
							value = (T)fieldInfo.GetValue(null);
							break;
						}

						baseType = baseType.BaseType;
					}
				}
			}

			return value;
		}

		public static T GetPropertyValue<T>(this object obj, string property)
		{
			var type = obj.GetType();
			var propertyInfo = type.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);
			T value = default(T);

			if (propertyInfo != null)
			{
				value = (T)propertyInfo.GetValue(obj, null);
			}
			else
			{
				propertyInfo = type.GetProperty(property, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);

				if (propertyInfo != null)
				{
					value = (T)propertyInfo.GetValue(obj, null);
				}
				else
				{
					var baseType = type.BaseType;

					while (baseType != null)
					{
						propertyInfo = type.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);

						if (propertyInfo != null)
						{
							value = (T)propertyInfo.GetValue(obj, null);
							break;
						}

						baseType = baseType.BaseType;
					}
				}
			}

			return value;
		}

		public static T GetIndexerValue<T>(this object obj, string property, object indexParm)
		{
			var type = obj.GetType();
			var propertyInfo = type.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);
			T value = default(T);

			if (propertyInfo != null)
			{
				value = (T)propertyInfo.GetValue(obj, new object[] { indexParm });
			}
			else
			{
				propertyInfo = type.GetProperty(property, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);

				if (propertyInfo != null)
				{
					value = (T)propertyInfo.GetValue(obj, new object[] { indexParm });
				}
				else
				{
					var baseType = type.BaseType;

					while (baseType != null)
					{
						propertyInfo = type.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);

						if (propertyInfo != null)
						{
							value = (T)propertyInfo.GetValue(obj, new object[] { indexParm });
							break;
						}

						baseType = baseType.BaseType;
					}
				}
			}

			return value;
		}

		public static void SetPropertyValue(this object obj, string property, object propertyValue)
		{
			var type = obj.GetType();
			var propertyInfo = type.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.SetProperty);

			if (propertyInfo != null)
			{
				propertyInfo.SetValue(obj,  propertyValue, null);
			}
			else
			{
				var baseType = type.BaseType;

				while (baseType != null)
				{
					propertyInfo = type.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);

					if (propertyInfo != null)
					{
						propertyInfo.SetValue(obj, propertyValue, null);
						break;
					}

					baseType = baseType.BaseType;
				}
			}
		}

		public static bool HasProperty(this object obj, string property)
		{
			var type = obj.GetType();
			var propertyInfo = type.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);

			if (propertyInfo != null)
			{
				return true;
			}
			else
			{
				var baseType = type.BaseType;

				while (baseType != null)
				{
					propertyInfo = type.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);

					if (propertyInfo != null)
					{
						return true;
					}

					baseType = baseType.BaseType;
				}
			}

			return false;
		}

		public static bool HasField(this object obj, string field)
		{
			var type = obj.GetType();
			var fieldInfo = type.GetField(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField);

			if (fieldInfo != null)
			{
				return true;
			}
			else
			{
				var baseType = type.BaseType;

				while (baseType != null)
				{
					fieldInfo = type.GetField(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField);

					if (fieldInfo != null)
					{
						return true;
					}

					baseType = baseType.BaseType;
				}
			}

			return false;
		}

		public static PropertyInfo GetProperty(this object obj, string property)
		{
			var type = obj.GetType();
			var propertyInfo = type.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);

			if (propertyInfo != null)
			{
				return propertyInfo;
			}
			else
			{
				var baseType = type.BaseType;

				while (baseType != null)
				{
					propertyInfo = type.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);

					if (propertyInfo != null)
					{
						return propertyInfo;
					}

					baseType = baseType.BaseType;
				}
			}

			return null;
		}

		public static FieldInfo GetField(this object obj, string field)
		{
			var type = obj.GetType();
			var fieldInfo = type.GetField(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField);

			if (fieldInfo != null)
			{
				return fieldInfo;
			}
			else
			{
				var baseType = type.BaseType;

				while (baseType != null)
				{
					fieldInfo = type.GetField(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField);

					if (fieldInfo != null)
					{
						return fieldInfo;
					}

					baseType = baseType.BaseType;
				}
			}

			return null;
		}

		public static T GetEvent<T>(this object obj, string _event)
		{
			var type = obj.GetType();
			var eventInfo = type.GetEvent(_event, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);
			T value = Activator.CreateInstance<T>();

			if (eventInfo != null)
			{
				eventInfo.AddEventHandler(obj, (Delegate)(object)value);
			}

			return value;
		}

		public static T GetPrivateEvent<T>(this object obj, string _event)
		{
			var type = obj.GetType();
			var eventInfo = type.GetEvent(_event, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);
			T value = Activator.CreateInstance<T>();

			if (eventInfo != null)
			{
				eventInfo.AddEventHandler(obj, (Delegate)(object) value);
			}

			return value;
		}

		public static IEnumerable<KeyValuePair<string, object>> GetPublicPropertyValues(this object obj, bool skipNullValues = false)
		{
			var type = obj.GetType();

			foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty))
			{
				var value = propertyInfo.GetValue(obj, null);

				if (!skipNullValues || (value != null))
				{
					yield return new KeyValuePair<string, object>(propertyInfo.Name, value);
				}
			}
		}

		public static IEnumerable<KeyValuePair<string, T>> GetPublicPropertyValuesOfType<T>(this object obj)
		{
			var type = obj.GetType();

			foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty))
			{
				if (propertyInfo.PropertyType == typeof(T) || propertyInfo.PropertyType.Implements<T>())
				{
					var value = (T)propertyInfo.GetValue(obj, null);

					yield return new KeyValuePair<string, T>(propertyInfo.Name, value);
				}
			}
		}

		public static IEnumerable<PropertyInfo> GetPublicPropertiesOfType<T>(this object obj)
		{
			var type = obj.GetType();

			foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty))
			{
				if (propertyInfo.PropertyType == typeof(T) || propertyInfo.PropertyType.Implements<T>())
				{
					yield return propertyInfo;
				}
			}
		}

		public static IEnumerable<string> GetPublicPropertyNames<T>()
		{
			var type = typeof(T);

			foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.FlattenHierarchy))
			{
				yield return propertyInfo.Name;
			}
		}

		public static IEnumerable<string> GetImmediatePublicPropertyNames<T>()
		{
			var type = typeof(T);

			foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.DeclaredOnly))
			{
				yield return propertyInfo.Name;
			}
		}

		public static IEnumerable<PropertyInfo> GetPublicProperties(this object obj)
		{
			var type = obj.GetType();

			foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty))
			{
				yield return propertyInfo;
			}
		}

        public static IEnumerable<string> GetPublicPropertyNames(this object obj)
        {
            var type = obj.GetType();

            foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty))
            {
                yield return propertyInfo.Name;
            }
        }

        public static IEnumerable<string> GetPublicPropertyNames(this Type type)
        {
            foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty))
            {
                yield return propertyInfo.Name;
            }
        }

        public static IEnumerable<FieldInfo> GetPublicFields(this object obj)
		{
			var type = obj.GetType();

			foreach (var fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetField))
			{
				yield return fieldInfo;
			}
		}

		public static IEnumerable<KeyValuePair<T, PropertyInfo>> GetPublicPropertiesWithAttributeType<T>(this object obj) where T: Attribute
		{
			var type = obj.GetType();

			foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty))
			{
				if (propertyInfo.HasCustomAttribute<T>())
				{
					var attribute = propertyInfo.GetCustomAttribute<T>();

					yield return new KeyValuePair<T, PropertyInfo>(attribute, propertyInfo);
				}
			}
		}

		public static T GetAnonymousPropertyValue<T>(this object obj, string property)
		{
			var type = obj.GetType();
			var propertyInfo = type.GetProperty(property);
			T value = default(T);

			if (propertyInfo != null)
			{
				value = (T)propertyInfo.GetValue(obj, null);
			}
			else
			{
				var baseType = type.BaseType;

				while (baseType != null)
				{
					propertyInfo = type.GetProperty(property);

					if (propertyInfo != null)
					{
						value = (T)propertyInfo.GetValue(obj, null);
						break;
					}

					baseType = baseType.BaseType;
				}
			}

			return value;
		}

		public static T CallPrivateMethod<T>(this object obj, string method, params object[] parms)
		{
			var type = obj.GetType();
			var methodInfo = type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod);
			T value = default(T);

			if (methodInfo == null)
			{
				throw new NotImplementedException();
			}

			value = (T) methodInfo.Invoke(obj, parms);

			return value;
		}

		public static void CallPrivateMethod(this object obj, string method, params object[] parms)
		{
			var type = obj.GetType();
			var methodInfo = type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod);

			if (methodInfo == null)
			{
				throw new NotImplementedException();
			}

			methodInfo.Invoke(obj, parms);
		}

		public static void CallInternalMethod(this object obj, string method, params object[] parms)
		{
			var type = obj.GetType();
			var methodInfo = type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod);

			if (methodInfo == null)
			{
				throw new NotImplementedException();
			}

			methodInfo.Invoke(obj, parms);
		}

        public static T CallInternalMethod<T>(this object obj, string method, params object[] parms)
        {
            var type = obj.GetType();
            var methodInfo = type.GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod);

            if (methodInfo == null)
            {
                throw new NotImplementedException();
            }

            return (T) methodInfo.Invoke(obj, parms)!;
        }

        public static object CallMethod(this object obj, string method, params object[] parms)
		{
			var type = obj.GetType();
			var methodInfo = type.GetMethod(method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod, parms.Select(p => p.GetType()).ToArray());

			if (methodInfo == null)
			{
				throw new NotImplementedException();
			}

			return methodInfo.Invoke(obj, parms);
		}

		public static Type GetAnyInnerType(this Type type)
		{
			if (type.IsGenericType)
			{
				return type.GetGenericArguments()[0];
			}
			else if (type.IsArray)
			{
				return type.GetElementType();
			}

			return type;
		}

		public static T CreateInstance<T>(params object[] args)
		{
			var type = typeof(T);
			var instance = type.Assembly.CreateInstance(type.FullName, false, BindingFlags.Instance | BindingFlags.NonPublic, null, args, null, null);

			return (T)instance;
		}

		public static T CallPrivateStaticMethod<T>(this object obj, string method, params object[] parms)
		{
			var type = obj.GetType();
			var methodInfo = type.GetMethod(method, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod);
			T value = default(T);

			if (methodInfo == null)
			{
				throw new NotImplementedException();
			}

			value = (T)methodInfo.Invoke(obj, parms);

			return value;
		}

        public static void CallPublicStaticMethod(this object obj, string method, params object[] parms)
        {
            var type = obj.GetType();
            MethodInfo methodInfo;

            if (obj is Type objType)
            {
                type = objType;
                methodInfo = type.GetMethod(method, BindingFlags.Static | BindingFlags.Public);
            }
            else
            {
                type = obj.GetType();
                methodInfo = type.GetMethod(method, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod);
            }

            if (methodInfo == null)
            {
                throw new NotImplementedException();
            }

            methodInfo.Invoke(obj, parms);
        }

        public static void CallPrivateStaticMethod(this object obj, string method, params object[] parms)
		{
			var type = obj.GetType();
			MethodInfo methodInfo;

			if (obj is Type objType)
			{
				type = objType;
                methodInfo = type.GetMethod(method, BindingFlags.Static | BindingFlags.NonPublic);
            }
            else
			{
				type = obj.GetType();
                methodInfo = type.GetMethod(method, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod);
            }

            if (methodInfo == null)
			{
				throw new NotImplementedException();
			}

			methodInfo.Invoke(obj, parms);
		}

		public static T ActivatePrivate<T>(params object[] parms)
		{
			var type = typeof(T);
			var constructorInfo = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance, null, parms.Select(p => p.GetType()).ToArray(), null);
            T value = default(T);

            if (constructorInfo != null)
            {
                value = (T)constructorInfo.Invoke(parms);
            }
            else
            {
                var baseType = type.BaseType;

                while (baseType != null)
                {
                    constructorInfo = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance, null, parms.Select(p => p.GetType()).ToArray(), null);

                    if (constructorInfo != null)
                    {
                        value = (T)constructorInfo.Invoke(parms);
                        break;
                    }

                    baseType = baseType.BaseType;
                }
            }

            return value;
        }

		public static T GetPrivatePropertyValue<T>(this object obj, string property)
		{
			var type = obj.GetType();
			var propertyInfo = type.GetProperty(property, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);
			T value = default(T);

			if (propertyInfo != null)
			{
				value = (T)propertyInfo.GetValue(obj, null);
			}
			else
			{
				var baseType = type.BaseType;

				while (baseType != null)
				{
					propertyInfo = type.GetProperty(property, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);

					if (propertyInfo != null)
					{
						value = (T)propertyInfo.GetValue(obj, null);
						break;
					}

					baseType = baseType.BaseType;
				}
			}

			return value;
		}

		public static void SetPrivateFieldValue(this Type type, string fieldName, object value)
		{
			var field = type.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);

			field.SetValue(null, value);
		}

		public static void SetPrivateFieldValue(this Type type, string fieldName, object instance, object value)
		{
			while (type != null)
			{
				var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

				if (field != null)
				{
					field.SetValue(instance, value);
					break;
				}

				type = type.BaseType;
			}
		}

		public static void SetFieldValue(this Type type, string fieldName, object instance, object value)
		{
			var field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.SetField | BindingFlags.Instance);

			field.SetValue(instance, value);
		}

		public static T GetCustomAttribute<T>(this Type type)
		{
			var attribute = (T)type.GetCustomAttributes(true).OfType<T>().First();

			return attribute;
		}

        public static bool HasCustomAttribute<T>(this MemberInfo member)
        {
            return member.GetCustomAttributes(true).OfType<T>().Count() > 0;
        }

		public static T GetCustomAttribute<T>(this FieldInfo field)
		{
			var attribute = (T)field.GetCustomAttributes(true).OfType<T>().Single();

			return attribute;
		}

		public static T GetCustomAttribute<T>(this PropertyInfo property)
		{
			var attribute = (T)property.GetCustomAttributes(true).OfType<T>().First();

			return attribute;
		}

		public static T CreateInstance<T>(this Type type, params object[] args)
		{
			return (T) Activator.CreateInstance(type, args);
		}

		public static int SizeOf(this object obj)
		{
			return Marshal.SizeOf(obj);
		}

		public static int SizeOf<T>() where T : new()
		{
			var type = typeof(T);

			return Marshal.SizeOf(type);
		}

		public static int PrimitiveFieldSizeOf<T>() where T : new()
		{
			var size = 0;
			var type = typeof(T);
			var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);

			foreach (var field in fields)
			{
				if (field.FieldType.IsPrimitive)
				{
					size += Marshal.SizeOf(field.FieldType);
				}
			}

			return size;
		}

		public static bool IsVoid(this Type type)
		{
			return type == typeof(void);
		}

		public static string GetPropertySignature(this PropertyInfo property)
		{
			var builder = new StringBuilder();
			var propertyName = property.Name;

			if (propertyName.Contains("`"))
			{
				builder.Append(propertyName.Substring(0, propertyName.IndexOf('`')));
			}
			else
			{
				builder.Append(propertyName);
			}

			if (property.PropertyType.IsArray)
			{
				builder.Append("[]");
			}

			if (property.PropertyType.IsGenericType)
			{
				builder.Append(property.PropertyType.GetGenericArguments().GetGenericArgsSignature());
			}

			builder.Append(" " + property.Name);

			return builder.ToString();
		}

		public static string GetParmSignature(this ParameterInfo[] parameters)
		{
			var builder = new StringBuilder("(");
			var hasComma = false;

			foreach (ParameterInfo parameter in parameters)
			{
				var parameterTypeName = parameter.ParameterType.Name;

				if (parameter.IsOut)
				{
					builder.Append("out ");
				}
				else if (parameter.ParameterType.IsByRef)
				{
					builder.Append("ref ");
				}

				if (parameterTypeName.Contains("`"))
				{
					builder.Append(parameterTypeName.Substring(0, parameterTypeName.IndexOf('`')));
				}
				else
				{
					builder.Append(parameterTypeName);
				}

				builder.Append(parameter.ParameterType.Name);

				if (parameter.ParameterType.IsArray)
				{
					builder.Append("[]");
				}

				if (parameter.ParameterType.IsGenericType)
				{
					builder.Append(parameter.ParameterType.GetGenericArguments().GetGenericArgsSignature());
				}

				if (parameter.IsOptional)
				{
					builder.Append(" = " + parameter.DefaultValue.ToString());
				}

				builder.Append(", ");
				hasComma = true;
			}

			if (hasComma)
			{
				builder.Remove(builder.Length - 2, 2);
			}

			builder.Append(")");

			return builder.ToString();
		}

		public static string GetGenericArgsSignature(this System.Type[] arguments)
		{
			var builder = new StringBuilder("<");

			var hasComma = false;

			foreach (System.Type argumentType in arguments)
			{
				if (argumentType.IsGenericType)
				{
					builder.Append(argumentType.GetGenericArguments().GetGenericArgsSignature());
				}
				else
				{
					var name = argumentType.Name;

					if (argumentType.IsPrimitive)
					{
						name = argumentType.Name.ToCamelCase();
					}

					builder.Append(name);
				}

				builder.Append(", ");
				hasComma = true;
			}

			if (hasComma)
			{
				builder.Remove(builder.Length - 2, 2);
			}

			builder.Append(">");

			return builder.ToString();
		}

		public static string GetGenericSignature(this MemberInfo memberInfo, Type[] genericArgs, bool includeDeclaringType = false, bool includeModifiers = false)
		{
			string signature = null;

			SwitchExtensions.Switch(memberInfo, () => memberInfo.MemberType,

				SwitchExtensions.Case<MethodInfo>(MemberTypes.Method, (methodInfo) =>
				{
					signature = methodInfo.GetSignature(includeDeclaringType, includeModifiers);
				}),
				SwitchExtensions.Case<ConstructorInfo>(MemberTypes.Constructor, (constructorInfo) =>
				{
					signature = constructorInfo.GetGenericSignature(genericArgs, includeDeclaringType, includeModifiers);
				}),
				SwitchExtensions.CaseElse(() =>
				{
					Debugger.Break();
				})
			);

			return signature;
		}

		public static string GetGenericSignature(this MemberInfo memberInfo, string[] genericArgs, bool includeDeclaringType = false, bool includeModifiers = false)
		{
			string signature = null;

			SwitchExtensions.Switch(memberInfo, () => memberInfo.MemberType,

				SwitchExtensions.Case<MethodInfo>(MemberTypes.Method, (methodInfo) =>
				{
					signature = methodInfo.GetSignature(includeDeclaringType, includeModifiers);
				}),
				SwitchExtensions.Case<ConstructorInfo>(MemberTypes.Constructor, (constructorInfo) =>
				{
					signature = constructorInfo.GetGenericSignature(genericArgs, includeDeclaringType, includeModifiers);
				}),
				SwitchExtensions.CaseElse(() =>
				{
					Debugger.Break();
				})
			);

			return signature;
		}

		public static string GetSignature(this MemberInfo memberInfo, bool includeDeclaringType = false, bool includeModifiers = false, bool addAttributes = false)
		{
			string signature = null;

			SwitchExtensions.Switch(memberInfo, () => memberInfo.MemberType,

				SwitchExtensions.Case<MethodInfo>(MemberTypes.Method, (methodInfo) =>
				{
					signature = methodInfo.GetSignature(includeDeclaringType, includeModifiers, addAttributes);
				}),
				SwitchExtensions.Case<PropertyInfo>(MemberTypes.Property, (propertyInfo) =>
				{
					signature = propertyInfo.GetSignature(includeDeclaringType, includeModifiers, addAttributes);
				}),
				SwitchExtensions.Case<EventInfo>(MemberTypes.Event, (eventInfo) =>
				{
					signature = eventInfo.GetSignature(includeDeclaringType, includeModifiers, addAttributes);
				}),
				SwitchExtensions.Case<ConstructorInfo>(MemberTypes.Constructor, (constructorInfo) =>
				{
					signature = constructorInfo.GetSignature(includeDeclaringType, includeModifiers, addAttributes);
				}),
				SwitchExtensions.Case<FieldInfo>(MemberTypes.Field, (fieldInfo) =>
				{
					signature = fieldInfo.GetSignature(includeDeclaringType, includeModifiers, addAttributes);
				}),
				SwitchExtensions.CaseElse(() =>
				{
					Debugger.Break();
				})
			);

			return signature;
		}

		public static string GetGenericSignature(this ConstructorInfo constructor, Type[] genericArgs, bool includeDeclaringType = false, bool includeModifiers = false)
		{
			return constructor.GenerateGenericSignature(genericArgs, includeDeclaringType, includeModifiers);
		}

		public static string GetGenericSignature(this ConstructorInfo constructor, string[] genericArgs, bool includeDeclaringType = false, bool includeModifiers = false)
		{
			return constructor.GenerateGenericSignature(genericArgs, includeDeclaringType, includeModifiers);
		}

		public static string GetSignature(this ConstructorInfo constructor, bool includeDeclaringType = false, bool includeModifiers = false, bool addAttributes = false)
		{
			return constructor.GenerateSignature(includeDeclaringType, includeModifiers);
		}

		public static string GetSignature(this EventInfo _event, bool includeDeclaringType = false, bool includeModifiers = false, bool addAttributes = false)
		{
			var type = _event.EventHandlerType.GetCodeDeclaration();
			var name = _event.Name;
			var modifiers = string.Empty;
            var customAttributes = string.Empty;

            if (includeModifiers)
			{
				modifiers = _event.GetModifiersString().AppendIfNotNullOrEmpty(" ");
			}

            if (addAttributes)
            {
                if (_event.HasCustomAttributes())
                {
                    customAttributes = _event.CustomAttributes.GetCustomAttributesString();
                }
            }

            if (includeDeclaringType)
			{
				return customAttributes + modifiers + type + " " + _event.DeclaringType.GetCodeDeclaration() + "." + name;
			}
			else
			{
				return customAttributes + modifiers + type + " " + name;
			}
		}

		public static bool IsPrimitiveTypeShortName(string shortTypeName)
		{
			bool flag;
			string str = shortTypeName;
			switch (str)
			{
				case "string":
					{
						flag = true;
						break;
					}
				case "int":
					{
						flag = true;
						break;
					}
				case "uint":
					{
						flag = true;
						break;
					}
				case "object":
					{
						flag = true;
						break;
					}
				case "bool":
					{
						flag = true;
						break;
					}
				case "sbyte":
					{
						flag = true;
						break;
					}
				case "byte":
					{
						flag = true;
						break;
					}
				case "short":
					{
						flag = true;
						break;
					}
				case "ushort":
					{
						flag = true;
						break;
					}
				case "long":
					{
						flag = true;
						break;
					}
				case "ulong":
					{
						flag = true;
						break;
					}
				case "float":
					{
						flag = true;
						break;
					}
				case "double":
					{
						flag = true;
						break;
					}
				case "decimal":
					{
						flag = true;
						break;
					}
				case "char":
					{
						flag = true;
						break;
					}
				default:
					{
						flag = (str == "void" ? true : false);
						break;
					}
			}
			return flag;
		}


		public static List<Type> GetAncestorInterfaces(this Type type, bool includeMe = false)
		{
			List<Type> types = new List<Type>();
			Type baseType = type.BaseType;
		
			if (includeMe)
			{
				types.Add(type);
			}
			
			Type[] interfaces = type.GetInterfaces();
			
			for (int i = 0; i < (int)interfaces.Length; i++)
			{
				types.AddRange(interfaces[i].GetAncestorInterfaces(true));
			}
			
			return types.Distinct<Type>().ToList<Type>();
		}

		public static string GetFullName(this MemberInfo memberInfo, bool removeAccessorPrefix = false)
		{
			if (memberInfo is Type type)
			{
				if (type.FullName == null)
				{
					return type.Namespace + "." + type.Name;
				}
				else
				{
					return type.FullName;
				}
			}
			else
			{
				if (removeAccessorPrefix)
				{
					return memberInfo.DeclaringType.FullName + "." + memberInfo.Name.RemoveStartIfMatches("get_").RemoveStartIfMatches("set_");
				}
				else
				{
                    return memberInfo.DeclaringType.FullName + "." + memberInfo.Name;
                }	
			}
		}

        public static string GetAssemblyName(this MemberInfo memberInfo)
        {
            if (memberInfo is Type type)
            {
				return type.Assembly.GetNameParts().AssemblyName;
            }
            else
            {
                return memberInfo.DeclaringType.Assembly.GetNameParts().AssemblyName;
            }
        }

        public static string GetSignature(this PropertyInfo property, bool includeDeclaringType = false, bool includeModifiers = false, bool addAttributes = false, bool useExplicitInterfaceFormat = false)
		{
			Type declaringType;
			List<Type> types;
			var type = property.PropertyType.GetCodeDeclaration();
			var name = property.Name;
			var modifiers = string.Empty;
            var customAttributes = string.Empty;
            string accessors;

			if (IsPrimitiveTypeShortName(name))
			{
				name = name.Prepend("@");
			}
			if (!useExplicitInterfaceFormat)
			{
				declaringType = property.DeclaringType;
			}
			else
			{
				types = (!property.DeclaringType.IsInterface ? property.DeclaringType.GetAncestorInterfaces(false) : property.DeclaringType.GetAncestorInterfaces(true));

				if (!types.Any(t => t.GetProperties().Any(p => p.Name != name ? false : p.PropertyType == property.PropertyType)))
				{
					declaringType = property.DeclaringType;
				}
				else
				{
					declaringType = types.First(t => t.GetProperties().Any(p => p.Name != name ? false : p.PropertyType == property.PropertyType));
					includeDeclaringType = true;
				}
			}

            if (addAttributes)
            {
                if (property.HasCustomAttributes())
                {
                    customAttributes = property.CustomAttributes.GetCustomAttributesString();
                }
            }

            if (includeModifiers)
			{
				modifiers = property.GetModifiersString().AppendIfNotNullOrEmpty(" ");
			}

			accessors = property.GetAccessorSignatures(includeModifiers, addAttributes, modifiers);

			if (includeDeclaringType)
			{
				return customAttributes + modifiers + type + " " + property.DeclaringType.GetCodeDeclaration() + "." + name + accessors.PrependIfNotNullOrEmpty(" ");
			}
			else
			{
				return customAttributes + modifiers + type + " " + name + accessors.PrependIfNotNullOrEmpty(" ");
			}
		}

		public static string GetSignature(this FieldInfo field, bool includeDeclaringType = false, bool includeModifiers = false, bool addAttributes = false)
		{
			var type = field.FieldType.GetCodeDeclaration();
			var name = field.Name;
			var modifiers = string.Empty;
            var customAttributes = string.Empty;

            if (includeModifiers)
			{
				modifiers = field.GetModifiersString().AppendIfNotNullOrEmpty(" ");
			}

            if (addAttributes)
            {
                if (field.HasCustomAttributes())
                {
                    customAttributes = field.CustomAttributes.GetCustomAttributesString();
                }
            }

			if (field.DeclaringType.IsEnum)
			{
                return field.DeclaringType.Name + "." + name;
            }
			else
			{
				if (includeDeclaringType)
				{
					return customAttributes + modifiers + type + " " + field.DeclaringType.GetCodeDeclaration() + "." + name;
				}
				else
				{
					return customAttributes + modifiers + type + " " + name;
				}
			}
		}

		public static bool IsExtensionMethod(this MethodInfo method)
		{
			if (method.IsStatic && method.IsDefined(typeof(ExtensionAttribute), false))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static string GetExtensionMethodSignature(this MethodInfo method, MethodInfo genericMethodInfo, bool skipThisParameter, bool includeDeclaringType = false, bool includeModifiers = false)
		{
			return method.GenerateExtensionMethodSignature(genericMethodInfo, skipThisParameter, includeDeclaringType, includeModifiers);
		}

		public static string GetTypeSimpleName(string typeName)
		{
			if (typeName.Contains("."))
			{
				typeName = typeName.Right(typeName.Length - typeName.LastIndexOf(".") - 1);

				if (typeName.Contains("`"))
				{
					return typeName.Left(typeName.LastIndexOf("`"));
				}
				else
				{
					return typeName;
				}
			}
			else
			{
				if (typeName.Contains("`"))
				{
					return typeName.Left(typeName.LastIndexOf("`"));
				}
				else
				{
					return typeName;
				}
			}
		}

		public static bool IsCollectionType(this Type type)
		{
			if (type.IsArray)
			{
				return true;
			}
			else if (type.IsDictionaryGeneric())
			{
				if (type.Namespace.StartsWith("System.Collections"))
				{
					return true;
				}
			}
			else if (type.IsEnumerableGeneric())
			{
				if (type.Namespace.StartsWith("System.Collections") || type.Namespace.StartsWith("Google.Protobuf.Collections"))
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsEnumerableGeneric(this Type type)
		{
			try
			{
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return true;
                }

                foreach (Type intType in type.GetInterfaces())
				{
					if (intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
					{
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				return type.BaseType.IsEnumerableGeneric() || type.GetGenericTypeDefinition().IsEnumerableGeneric();
			}

			return false;
		}

		public static bool IsDictionaryGeneric(this Type type)
		{
			try
			{
				foreach (Type intType in type.GetInterfaces())
				{
					if (intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
					{
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				var underlyingType = type.BaseType;

				return underlyingType.IsDictionaryGeneric();
			}

			return false;
		}


		public static string GenerateExtensionMethodSignature(this MethodInfo method, MethodInfo genericMethodInfo, bool skipThisParameter, bool includeDeclaringType = false, bool includeModifiers = false)
		{
			// Char Aggregate[Char](System.Collections.Generic.IEnumerable`1[System.Char], System.Func`3[System.Char,System.Char,System.Char])
			// System.Collections.Generic.Dictionary`2[System.String,System.Collections.Generic.List`1[System.Int32]] MakeDictionaryList[Int32](System.Collections.Generic.List`1[System.Collections.Generic.KeyValuePair`2[System.String,System.Int32]])

			var parser = new Parser();
			var syntaxTree = parser.Parse(genericMethodInfo);
			var clrStringMethod = (CLRStringMethod) syntaxTree.Children.Single();
			var type = clrStringMethod.Type.GetName();
			var name = clrStringMethod.GetMethodName();
			var modifiers = string.Empty;

			if (clrStringMethod.DebugInfo != genericMethodInfo.ToString())
			{
				Debugger.Break();
			}

			if (includeModifiers)
			{
				modifiers = method.GetModifiersString().Append(" ");
			}

			if (includeDeclaringType)
			{
				return modifiers + type + " " + method.DeclaringType.GetCodeDeclaration() + "." + name + GenerateParmStringSignature(method.GetParameters(), clrStringMethod.Parameters, skipThisParameter);
			}
			else
			{
				return modifiers + type + " " + name + GenerateParmStringSignature(method.GetParameters(), clrStringMethod.Parameters, skipThisParameter);
			}
		}

		public static string GenerateMethodSignature(this MethodInfo method, bool includeDeclaringType = false, bool includeModifiers = false)
		{
			var parser = new Parser();
			var syntaxTree = parser.Parse(method);
			var clrStringMethod = (CLRStringMethod)syntaxTree.Children.Single();
			var type = clrStringMethod.Type.GetName();
			var name = clrStringMethod.GetMethodName();
			var modifiers = string.Empty;

			if (includeModifiers)
			{
				modifiers = method.GetModifiersString().Append(" ");
			}

			if (includeDeclaringType)
			{
				return modifiers + type + " " + method.DeclaringType.GetCodeDeclaration() + "." + name + GenerateParmStringSignature(method.GetParameters(), clrStringMethod.Parameters);
			}
			else
			{
				return modifiers + type + " " + name + GenerateParmStringSignature(method.GetParameters(), clrStringMethod.Parameters);
			}
		}

		public static CLRStringType ParseName(this Type type)
		{
			var parser = new Parser();
			var syntaxTree = parser.Parse(type);
			var clrStringType = (CLRStringType)syntaxTree.Children.Single();

			return clrStringType;
		}

		public static CLRStringMethod ParseName(this MethodInfo method)
		{
			var parser = new Parser();
			var syntaxTree = parser.Parse(method);
			var clrStringMethod = (CLRStringMethod)syntaxTree.Children.Single();

			return clrStringMethod;
		}

		public static string GetShortName(this IUnionNode unionName)
		{
			string shortName = null;
			var name = unionName.GetUnionPopulatedFieldValue();

			SwitchExtensions.Switch(name, () => name.GetType().Name,

				SwitchExtensions.Case<QualifiedName>("QualifiedName", (qualifiedName) =>
				{
					shortName = qualifiedName.Right.Text;

					DebugUtils.NoOp();
				}),
				SwitchExtensions.Case<Identifier>("Identifier", (identifier) =>
				{
					shortName = identifier.Text;

					DebugUtils.NoOp();
				}),
				SwitchExtensions.CaseElse(() =>
				{
					// implementation here or throw error

					Debugger.Break();
				})
			);

			return shortName;
		}

		public static string GetNameOnly(this TypeNode typeNode)
		{
			string shortName = null;

			SwitchExtensions.Switch(typeNode, () => typeNode.GetType().Name,

				SwitchExtensions.Case<CLRStringType>("CLRStringType", (typeReferenceNode) =>
				{
					var entityName = typeReferenceNode.TypeName;

					shortName = entityName.EntityName.GetShortName();

					DebugUtils.NoOp();
				}),
				SwitchExtensions.Case<TypeReferenceNode>("TypeReferenceNode", (typeReferenceNode) =>
				{
					shortName = typeReferenceNode.TypeName.GetShortName();

					DebugUtils.NoOp();
				}),
				SwitchExtensions.Case<ArrayTypeNode>("ArrayTypeNode", (arrayTypeNode) =>
				{
					shortName = arrayTypeNode.ElementType.GetName() + "[]";

					DebugUtils.NoOp();
				}),
				SwitchExtensions.CaseElse(() =>
				{
					// implementation here or throw error

					Debugger.Break();
				})
			);

			return shortName;
		}

		public static List<TypeNode> GetTypeArguments(this TypeNode typeNode)
		{
			List<TypeNode> typeArguments = null;

			SwitchExtensions.Switch(typeNode, () => typeNode.GetType().Name,

				SwitchExtensions.Case<CLRStringType>("CLRStringType", (typeReferenceNode) =>
				{
					if (typeReferenceNode.TypeArguments != null)
					{
						typeArguments = typeReferenceNode.TypeArguments.Cast<TypeNode>().ToList();
					}
					else
					{
						typeArguments = new List<TypeNode>();
					}

					DebugUtils.NoOp();
				}),
				SwitchExtensions.Case<TypeReferenceNode>("TypeReferenceNode", (typeReferenceNode) =>
				{
					if (typeReferenceNode.TypeArguments != null)
					{
						typeArguments = typeReferenceNode.TypeArguments.Cast<TypeNode>().ToList();
					}
					else
					{
						typeArguments = new List<TypeNode>();
					}

					DebugUtils.NoOp();
				}),
				SwitchExtensions.CaseElse(() =>
				{
					typeArguments = new List<TypeNode>();
				})
			);

			return typeArguments;
		}

		public static string GetName(this TypeNode typeNode)
		{
			var codeDeclaration = string.Empty;
			var typeName = typeNode.GetNameOnly();
			var typeArguments = typeNode.GetTypeArguments();

			if (typeArguments.Count > 0)
			{
				var args = string.Join<string>(", ", typeArguments.Select(n => n.GetName()).ToArray<string>());

				codeDeclaration = typeName + "<" + args + ">";
			}
			else
			{
				switch (typeName)
				{
					case "Void":
						codeDeclaration = "void";
						break;
					case "Boolean":
						codeDeclaration = "bool";
						break;
					case "Byte":
						codeDeclaration = "byte";
						break;
					case "SByte":
						codeDeclaration = "sbyte";
						break;
					case "Char":
						codeDeclaration = "char";
						break;
					case "Decimal":
						codeDeclaration = "decimal";
						break;
					case "Double":
						codeDeclaration = "double";
						break;
					case "Single":
						codeDeclaration = "float";
						break;
					case "Int32":
						codeDeclaration = "int";
						break;
					case "UInt32":
						codeDeclaration = "uint";
						break;
					case "Int64":
						codeDeclaration = "long";
						break;
					case "UInt64":
						codeDeclaration = "ulong";
						break;
					case "Object":
						codeDeclaration = "object";
						break;
					case "Int16":
						codeDeclaration = "short";
						break;
					case "UInt16":
						codeDeclaration = "ushort";
						break;
					case "String":
						codeDeclaration = "string";
						break;
					default:
						codeDeclaration = typeName;
						break;
				}
			}

			return codeDeclaration;
		}

		public static string GetSignature(this MethodInfo method, bool includeDeclaringType = false, bool includeModifiers = false, bool addAttributes = false, bool skipThisParameter = false, bool useExplicitInterfaceFormat = false, bool noReturnType = false)
		{
			return method.GenerateSignature(includeDeclaringType, includeModifiers, addAttributes, skipThisParameter, useExplicitInterfaceFormat, noReturnType);
		}

		public static string GetAccessorSignatures(this PropertyInfo property, bool includeModifiers = false, bool addAttributes = false, string propertyModifiers = null)
		{
			var signature = new StringBuilder();
            var customAttributes = string.Empty;

            foreach (var accessor in property.GetAccessors(true))
			{
                if (addAttributes)
                {
                    if (accessor.HasCustomAttributes())
                    {
                        customAttributes = accessor.CustomAttributes.GetCustomAttributesString().RegexReplace(@"\r?\n", " ");
                        signature.Append(customAttributes);
                    }
                }

                if (accessor.IsSetter())
				{
					if (includeModifiers)
					{
						var modifiers = accessor.GetModifiersString();

						if (propertyModifiers == null || propertyModifiers.Trim() != modifiers)
						{
							signature.Append(modifiers);
						}
					}

					signature.AppendWithLeadingIfLength(" ", "set;");
				}
				else if (accessor.IsGetter())
				{
					if (includeModifiers)
					{
						var modifiers = accessor.GetModifiersString();

						if (propertyModifiers == null || propertyModifiers.Trim() != modifiers)
						{
							signature.Append(modifiers);
						}
					}

					signature.AppendWithLeadingIfLength(" ", "get;");
				}
			}

			return signature.ToString().Trim().SurroundWithIfNotNullOrEmpty("{ ", " }");
		}

		public static bool IsGetter(this MethodInfo method)
		{
			if (method.Name.StartsWith("get_") && method.ReturnType != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool IsSetter(this MethodInfo method)
		{
			if (method.Name.StartsWith("set_") && (method.ReturnType == null || method.ReturnType == typeof(void)) && method.GetParameters().Length == 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		public static bool IsAccessor(this MethodInfo method)
		{
			return method.IsGetter() || method.IsSetter() || method.IsAdder() || method.IsRemover();
		}

		public static bool IsAdder(this MethodInfo method)
		{
			if (method.Name.StartsWith("add_") && method.ReturnType != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool IsRemover(this MethodInfo method)
		{
			if (method.Name.StartsWith("remove_") && method.ReturnType != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static Type GetGetterSetterType(this MethodInfo method)
		{
			return method.ReturnType != null ? method.ReturnType : method.GetParameters().First().ParameterType;
		}

		public static string GenerateTypeString(this System.Type type, Type[] genericArgs = null)
		{
			if (type.IsGenericType)
			{
				var builder = new StringBuilder();
				var name = type.Name;
				var index = name.IndexOf("`");

				if (type.IsPrimitive)
				{
					name = type.Name.ToCamelCase();
				}

				if (index != -1)
				{
					name = name.Remove(index);
				}

				builder.Append(name);

				if (genericArgs != null)
				{
					builder.Append(GenerateGenericArgsString(genericArgs));
				}
				else
				{
					builder.Append(GenerateGenericArgsString(type.GetGenericArguments()));
				}

				return builder.ToString();
			}
			else
			{
				return type.Name;
			}
		}

		public static string GenerateTypeString(this System.Type type, string[] genericArgs)
		{
			if (type.IsGenericType)
			{
				var builder = new StringBuilder();
				var name = type.Name;
				var index = name.IndexOf("`");

				if (type.IsPrimitive)
				{
					name = type.Name.ToCamelCase();
				}

				if (index != -1)
				{
					name = name.Remove(index);
				}

				builder.Append(name);

				builder.Append(GenerateGenericArgsString(genericArgs));

				return builder.ToString();
			}
			else
			{
				return type.Name;
			}
		}

		public static string GenerateGenericArgsString(System.Type[] arguments)
		{
			var builder = new StringBuilder("<");

			var hasComma = false;

			foreach (System.Type argumentType in arguments)
			{
				if (argumentType.IsGenericType)
				{
					builder.Append(GenerateGenericArgsString(argumentType.GetGenericArguments()));
				}
				else
				{
					var name = argumentType.Name;

					if (argumentType.IsPrimitive)
					{
						name = argumentType.Name.ToCamelCase();
					}

					builder.Append(name);
				}

				builder.Append(", ");
				hasComma = true;
			}

			if (hasComma)
			{
				builder.Remove(builder.Length - 2, 2);
			}

			builder.Append(">");

			return builder.ToString();
		}

		public static string GenerateGenericArgsString(string[] arguments)
		{
			var builder = new StringBuilder("<");

			var hasComma = false;

			foreach (string argumentTypeText in arguments)
			{
				var argumentType = Type.GetType(argumentTypeText);
				string name;

				if (argumentType != null)
				{
					name = argumentType.Name;
					name = argumentType.GetShortName();
				}
				else
				{
					name = argumentTypeText;
				}

				builder.Append(name);
				
				builder.Append(", ");
				hasComma = true;
			}

			if (hasComma)
			{
				builder.Remove(builder.Length - 2, 2);
			}

			builder.Append(">");

			return builder.ToString();
		}

		public static string GenerateParmString(ParameterInfo[] parameters)
		{
			var builder = new StringBuilder("(");
			var hasComma = false;

			foreach (ParameterInfo parameter in parameters)
			{
				if (parameter.IsOut)
				{
					builder.Append("out ");
				}
				else if (parameter.ParameterType.IsByRef)
				{
					builder.Append("ref ");
				}

				builder.Append(parameter.ParameterType.Name);

				if (parameter.ParameterType.IsArray)
				{
					builder.Append("[]");
				}

				if (parameter.ParameterType.IsGenericType)
				{
					builder.Append(GenerateGenericArgsString(parameter.ParameterType.GetGenericArguments()));
				}

				if (parameter.IsOptional)
				{
					builder.Append(" = " + parameter.DefaultValue.ToString());
				}

				builder.Append(", ");
				hasComma = true;
			}

			if (hasComma)
			{
				builder.Remove(builder.Length - 2, 2);
			}

			builder.Append(")");

			return builder.ToString();
		}

		public static int GetCategorySort(this ReflectionMemberCategory category)
		{
			switch (category)
			{
				case ReflectionMemberCategory.BaseType:
					return 1;
				case ReflectionMemberCategory.ImplementedInterface:
					return 2;
				case ReflectionMemberCategory.Constructor:
					return 3;
				case ReflectionMemberCategory.Destructor:
					return 4;
				case ReflectionMemberCategory.Property:
					return 5;
				case ReflectionMemberCategory.Indexer:
					return 6;
				case ReflectionMemberCategory.Method:
					return 7;
				case ReflectionMemberCategory.Constant:
					return 8;
				case ReflectionMemberCategory.Field:
					return 9;
				case ReflectionMemberCategory.Operator:
					return 10;
				case ReflectionMemberCategory.Event:
					return 11;
				case ReflectionMemberCategory.ExplicitInterfaceImplementation:
					return 12;
				case ReflectionMemberCategory.ExtensionMethod:
					return 13;
				default:
					Debugger.Break();
					return 14;
			}
		}

		public static ReflectionMemberCategory GetCategory(this MemberInfo member)
		{
			var category = ReflectionMemberCategory.Unspecified;

			SwitchExtensions.Switch(member, () => member.MemberType,

				SwitchExtensions.Case<MethodInfo>(MemberTypes.Method, (methodInfo) =>
				{
					// operators

					if (methodInfo.IsGenericMethod)
					{
						category = ReflectionMemberCategory.Method;
					}
					else if (methodInfo.IsStatic && methodInfo.GetParameters().Length > 1 && methodInfo.Name.StartsWith("op_") && methodInfo.IsSpecialName)
					{
						category = ReflectionMemberCategory.Operator;
					}
					else if (methodInfo.Name == "Finalize" && methodInfo.GetParameters().Length == 0)
					{
						category = ReflectionMemberCategory.Destructor;
					}
					else
					{
						category = ReflectionMemberCategory.Method;
					}
				}),
				SwitchExtensions.Case<PropertyInfo>(MemberTypes.Property, (propertyInfo) =>
				{
					if (propertyInfo.GetIndexParameters().Length > 0)
					{
						category = ReflectionMemberCategory.Indexer;
					}
					else
					{
						category = ReflectionMemberCategory.Property;
					}
				}),
				SwitchExtensions.Case<EventInfo>(MemberTypes.Event, (eventInfo) =>
				{
					category = ReflectionMemberCategory.Event;
				}),
				SwitchExtensions.Case<ConstructorInfo>(MemberTypes.Constructor, (constructorInfo) =>
				{
					category = ReflectionMemberCategory.Constructor;
				}),
				SwitchExtensions.Case<FieldInfo>(MemberTypes.Field, (fieldInfo) =>
				{
					if (fieldInfo.IsPublic && fieldInfo.IsStatic && fieldInfo.IsLiteral && fieldInfo.IsInitOnly)
					{
						category = ReflectionMemberCategory.Constant;
					}
					else
					{
						category = ReflectionMemberCategory.Field;
					}
				}),
				SwitchExtensions.CaseElse(() =>
				{
					Debugger.Break();
				})
			);

			return category;
		}

		public static ReflectionModifiers GetModifiers(this Type type)
		{
			var modifiers = ReflectionModifiers.None;
			var nested = false;

			if (type.IsNestedPublic)
			{
				modifiers |= ReflectionModifiers.Public;
				nested = true;
			}
			else if (type.IsNestedFamily)
			{
				modifiers |= ReflectionModifiers.Protected;
				nested = true;
			}
			else if (type.IsNestedAssembly)
			{
				modifiers |= ReflectionModifiers.Internal;
				nested = true;
			}
			else if (type.IsNestedPrivate)
			{
				modifiers |= ReflectionModifiers.Private;
				nested = true;
			}

			if (!nested)
			{
				if (type.IsPublic)
				{
					modifiers |= ReflectionModifiers.Public;
				}
				else
				{
					modifiers |= ReflectionModifiers.Private;
				}
			}

			if (type.IsAbstract)
			{
				if (type.IsSealed)
				{
					modifiers |= ReflectionModifiers.Static;
				}
				else
				{
					modifiers |= ReflectionModifiers.Abstract;
				}
			}

			return modifiers;
		}

		public static ReflectionModifiers GetModifiers(this MemberInfo member)
		{
			var modifiers = ReflectionModifiers.None;

			// order:

			//{ public / protected / internal / private / protected internal } // access modifiers
			//new
			//{ abstract / virtual / override } // inheritance modifiers
			//sealed
			//static
			//readonly
			//extern
			//unsafe
			//volatile
			//async

			// nested types

			/// Type.GetNestedTypes Method

			SwitchExtensions.Switch(member, () => member.MemberType,

				SwitchExtensions.Case<TypeInfo>(MemberTypes.TypeInfo, (typeInfo) =>
				{
					modifiers = ((Type)typeInfo).GetModifiers();
				}),
				SwitchExtensions.Case<MethodInfo>(MemberTypes.Method, (methodInfo) =>
				{
					if (methodInfo.IsPublic)
					{
						modifiers |= ReflectionModifiers.Public;
					}
					else if (methodInfo.IsFamilyOrAssembly)
					{
						modifiers |= ReflectionModifiers.Protected;
						modifiers |= ReflectionModifiers.Internal;
					}
					else if (methodInfo.IsFamily)
					{
						modifiers |= ReflectionModifiers.Protected;
					}
					else if (methodInfo.IsAssembly)
					{
						modifiers |= ReflectionModifiers.Internal;
					}
					else if (methodInfo.IsPrivate)
					{
						modifiers |= ReflectionModifiers.Private;
					}

					if (methodInfo.IsAbstract)
					{
						modifiers |= ReflectionModifiers.Abstract;
					}
					else if (methodInfo.IsVirtual && !methodInfo.IsFinal)
					{
						modifiers |= ReflectionModifiers.Virtual;
					}

					if (methodInfo.IsStatic)
					{
						modifiers |= ReflectionModifiers.Static;
					}

					// operators

					//MethodInfo mi = EndType.GetMethod(
					//    "op_Implicit",
					//    (BindingFlags.Public | BindingFlags.Static),
					//    null,
					//    new Type[] { StartType },
					//    new ParameterModifier[0]
					//);

					// destructors = Finalize no parms
				}),
				SwitchExtensions.Case<PropertyInfo>(MemberTypes.Property, (propertyInfo) =>
				{
					modifiers = ReflectionModifiers.Private;

					foreach (var accessor in propertyInfo.GetAccessors(true))
					{
						var accessibility = modifiers.ApplyAccessibilityMask();

						if (accessor.IsPublic)
						{
							modifiers = EnumUtils.Min(accessibility, ReflectionModifiers.Public);
						}
						else if (accessor.IsFamilyOrAssembly)
						{
							modifiers = EnumUtils.Min(accessibility, ReflectionModifiers.Protected | ReflectionModifiers.Internal);
						}
						else if (accessor.IsFamily)
						{
							modifiers = EnumUtils.Min(accessibility, ReflectionModifiers.Protected);
						}
						else if (accessor.IsAssembly)
						{
							modifiers = EnumUtils.Min(accessibility, ReflectionModifiers.Internal);
						}
						else if (accessor.IsPrivate)
						{
							modifiers = EnumUtils.Min(accessibility, ReflectionModifiers.Private);
						}

						if (accessor.IsAbstract)
						{
							modifiers |= ReflectionModifiers.Abstract;
						}
						else if (accessor.IsVirtual && !accessor.IsFinal)
						{
							modifiers |= ReflectionModifiers.Virtual;
						}

						if (accessor.IsStatic)
						{
							modifiers |= ReflectionModifiers.Static;
						}
					}
				}),
				SwitchExtensions.Case<EventInfo>(MemberTypes.Event, (eventInfo) =>
				{
					var methodInfo = eventInfo.GetAddMethod();

					if (methodInfo.IsPublic)
					{
						modifiers |= ReflectionModifiers.Public;
					}
					else if (methodInfo.IsFamilyOrAssembly)
					{
						modifiers |= ReflectionModifiers.Protected;
						modifiers |= ReflectionModifiers.Internal;
					}
					else if (methodInfo.IsFamily)
					{
						modifiers |= ReflectionModifiers.Protected;
					}
					else if (methodInfo.IsAssembly)
					{
						modifiers |= ReflectionModifiers.Internal;
					}
					else if (methodInfo.IsPrivate)
					{
						modifiers |= ReflectionModifiers.Private;
					}

					if (methodInfo.IsAbstract)
					{
						modifiers |= ReflectionModifiers.Abstract;
					}
					else if (methodInfo.IsVirtual && !methodInfo.IsFinal)
					{
						modifiers |= ReflectionModifiers.Virtual;
					}

					if (methodInfo.IsStatic)
					{
						modifiers |= ReflectionModifiers.Static;
					}
				}),
				SwitchExtensions.Case<ConstructorInfo>(MemberTypes.Constructor, (constructorInfo) =>
				{
					if (constructorInfo.IsPublic)
					{
						modifiers |= ReflectionModifiers.Public;
					}
					else if (constructorInfo.IsFamilyOrAssembly)
					{
						modifiers |= ReflectionModifiers.Protected;
						modifiers |= ReflectionModifiers.Internal;
					}
					else if (constructorInfo.IsFamily)
					{
						modifiers |= ReflectionModifiers.Protected;
					}
					else if (constructorInfo.IsAssembly)
					{
						modifiers |= ReflectionModifiers.Internal;
					}
					else if (constructorInfo.IsPrivate)
					{
						modifiers |= ReflectionModifiers.Private;
					}

					if (constructorInfo.IsAbstract)
					{
						modifiers |= ReflectionModifiers.Abstract;
					}
					else if (constructorInfo.IsVirtual && !constructorInfo.IsFinal)
					{
						modifiers |= ReflectionModifiers.Virtual;
					}

					if (constructorInfo.IsStatic)
					{
						modifiers |= ReflectionModifiers.Static;
					}
				}),
				SwitchExtensions.Case<FieldInfo>(MemberTypes.Field, (fieldInfo) =>
				{
					// consts

					//type.GetFields(BindingFlags.Public | BindingFlags.Static |
					//               BindingFlags.FlattenHierarchy)
					//    .Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();


					// order:

					//{ public / protected / internal / private / protected internal } // access modifiers
					//new
					//{ abstract / virtual / override } // inheritance modifiers
					//sealed
					//static
					//readonly
					//extern
					//unsafe
					//volatile
					//async

					if (fieldInfo.IsPublic)
					{
						modifiers |= ReflectionModifiers.Public;
					}
					else if (fieldInfo.IsFamilyOrAssembly)
					{
						modifiers |= ReflectionModifiers.Protected;
						modifiers |= ReflectionModifiers.Internal;
					}
					else if (fieldInfo.IsFamily)
					{
						modifiers |= ReflectionModifiers.Protected;
					}
					else if (fieldInfo.IsFamily)
					{
						modifiers |= ReflectionModifiers.Protected;
					}
					else if (fieldInfo.IsAssembly)
					{
						modifiers |= ReflectionModifiers.Internal;
					}
					else if (fieldInfo.IsPrivate)
					{
						modifiers |= ReflectionModifiers.Private;
					}

					if (fieldInfo.IsStatic)
					{
						modifiers |= ReflectionModifiers.Static;
					}

					if (fieldInfo.IsInitOnly)
					{
						modifiers |= ReflectionModifiers.ReadOnly;
					}

					if (fieldInfo.GetRequiredCustomModifiers().Any(x => x == typeof(IsVolatile)))
					{
						modifiers |= ReflectionModifiers.Volatile;
					}
				}),
				SwitchExtensions.CaseElse(() =>
				{
					Debugger.Break();
				})
			);

			return modifiers;
		}

		public static bool IsStatic(this Type type)
        {
			var attribForStaticClass = TypeAttributes.AutoLayout | TypeAttributes.AnsiClass | TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit;
			
			return type.Attributes == attribForStaticClass;
		}

		public static string GetModifiersString(this MemberInfo member)
		{
			var modifiersBuilder = new StringBuilder();

			// order:

			//{ public / protected / internal / private / protected internal } // access modifiers
			//new
			//{ abstract / virtual / override } // inheritance modifiers
			//sealed
			//static
			//readonly
			//extern
			//unsafe
			//volatile
			//async

			// nested types

				/// Type.GetNestedTypes Method

			SwitchExtensions.Switch(member, () => member.MemberType,

				SwitchExtensions.Case<TypeInfo>(MemberTypes.TypeInfo, (typeInfo) =>
				{
					if (typeInfo.IsPublic)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Public);
					}
					else if (typeInfo.IsNestedFamORAssem)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Protected);
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Internal);
					}
					else if (typeInfo.IsNestedFamily)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Protected);
					}
					else if (typeInfo.IsNestedAssembly)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Internal);
					}
					else if (typeInfo.IsNestedPrivate)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Private);
					}

					if (typeInfo.IsAbstract)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Abstract);
					}

					if (typeInfo.IsStatic())
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Static);
					}
				}),
				SwitchExtensions.Case<MethodInfo>(MemberTypes.Method, (methodInfo) =>
				{
					if (methodInfo.IsPublic)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Public);
					}
					else if (methodInfo.IsFamilyOrAssembly)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Protected);
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Internal);
					}
					else if (methodInfo.IsFamily)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Protected);
					}
					else if (methodInfo.IsFamily)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Protected);
					}
					else if (methodInfo.IsAssembly)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Internal);
					}
					else if (methodInfo.IsPrivate)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Private);
					}

					if (methodInfo.IsAbstract)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Abstract);
					}
					else if (methodInfo.IsVirtual && !methodInfo.IsFinal)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Virtual);
					}

					if (methodInfo.IsStatic)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Static);
					}

					// operators

					//MethodInfo mi = EndType.GetMethod(
					//    "op_Implicit",
					//    (BindingFlags.Public | BindingFlags.Static),
					//    null,
					//    new Type[] { StartType },
					//    new ParameterModifier[0]
					//);

					// destructors = Finalize no parms
				}),
				SwitchExtensions.Case<PropertyInfo>(MemberTypes.Property, (propertyInfo) =>
				{
					var modifiers = propertyInfo.GetModifiers();

					if (modifiers.HasFlag(ReflectionModifiers.Public))
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Public);
					}
					else if (modifiers.HasFlag(ReflectionModifiers.Protected))
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Protected);

						if (modifiers.HasFlag(ReflectionModifiers.Internal))
						{
							modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Internal);
						}
					}
					else if (modifiers.HasFlag(ReflectionModifiers.Internal))
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Internal);
					}
					else if (modifiers.HasFlag(ReflectionModifiers.Private))
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Private);
					}

					if (modifiers.HasFlag(ReflectionModifiers.Abstract))
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Abstract);
					}
					else if (modifiers.HasFlag(ReflectionModifiers.Virtual))
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Virtual);
					}

					if (modifiers.HasFlag(ReflectionModifiers.Static))
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Static);
					}

				}),
				SwitchExtensions.Case<EventInfo>(MemberTypes.Event, (eventInfo) =>
				{
					var methodInfo = eventInfo.GetAddMethod();

					if (methodInfo.IsPublic)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Public);
					}
					else if (methodInfo.IsFamilyOrAssembly)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Protected);
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Internal);
					}
					else if (methodInfo.IsFamily)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Protected);
					}
					else if (methodInfo.IsFamily)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Protected);
					}
					else if (methodInfo.IsAssembly)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Internal);
					}
					else if (methodInfo.IsPrivate)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Private);
					}

					if (methodInfo.IsAbstract)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Abstract);
					}
					else if (methodInfo.IsVirtual && !methodInfo.IsFinal)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Virtual);
					}

					if (methodInfo.IsStatic)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Static);
					}
				}),
				SwitchExtensions.Case<ConstructorInfo>(MemberTypes.Constructor, (constructorInfo) =>
				{
					if (constructorInfo.IsPublic)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Public);
					}
					else if (constructorInfo.IsFamilyOrAssembly)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Protected);
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Internal);
					}
					else if (constructorInfo.IsFamily)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Protected);
					}
					else if (constructorInfo.IsFamily)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Protected);
					}
					else if (constructorInfo.IsAssembly)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Internal);
					}
					else if (constructorInfo.IsPrivate)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Private);
					}

					if (constructorInfo.IsAbstract)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Abstract);
					}
					else if (constructorInfo.IsVirtual && !constructorInfo.IsFinal)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Virtual);
					}

					if (constructorInfo.IsStatic)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Static);
					}
				}),
				SwitchExtensions.Case<FieldInfo>(MemberTypes.Field, (fieldInfo) =>
				{
					// consts

					//type.GetFields(BindingFlags.Public | BindingFlags.Static |
					//               BindingFlags.FlattenHierarchy)
					//    .Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();


					// order:

					//{ public / protected / internal / private / protected internal } // access modifiers
					//new
					//{ abstract / virtual / override } // inheritance modifiers
					//sealed
					//static
					//readonly
					//extern
					//unsafe
					//volatile
					//async

					if (fieldInfo.IsPublic)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Public);
					}
					else if (fieldInfo.IsFamilyOrAssembly)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Protected);
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Internal);
					}
					else if (fieldInfo.IsFamily)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Protected);
					}
					else if (fieldInfo.IsFamily)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Protected);
					}
					else if (fieldInfo.IsAssembly)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Internal);
					}
					else if (fieldInfo.IsPrivate)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Private);
					}

					if (fieldInfo.IsStatic)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Static);
					}

					if (fieldInfo.IsInitOnly)
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.ReadOnly);
					}

					if (fieldInfo.GetRequiredCustomModifiers().Any(x => x == typeof(IsVolatile)))
					{
						modifiersBuilder.AppendWithLeadingIfLength(" ", ReflectionModifierStrings.Volatile);
					}
				}),
				SwitchExtensions.CaseElse(() =>
				{
					Debugger.Break();
				})
			);

			return modifiersBuilder.ToString();
		}

		public static string GenerateSignature(this MethodInfo method, bool includeDeclaringType = false, bool includeModifiers = false, bool addAttributes = false, bool skipThisParameter = false, bool useExplicitInterfaceFormat = false, bool noReturnType = false)
		{
			var type = method.ReturnType.GetCodeDeclaration();
			var name = method.GetMethodName(useExplicitInterfaceFormat);
			var modifierString = string.Empty;
			var isExtension = method.IsExtensionMethod();
			var declaringType = method.DeclaringType;
            var customAttributes = string.Empty;

            if (useExplicitInterfaceFormat)
			{
				var modifiers = method.GetModifiers();
				var interfaceTypes = method.DeclaringType.GetInterfaces();

				includeDeclaringType = true;
				
				if (method.Name.Contains('.'))
				{
					if (modifiers.IsOneOf(ReflectionModifiers.Private, ReflectionModifiers.Public, ReflectionModifiers.Internal))
					{
						var interfaceName = method.Name.LeftUpToLastIndexOf('.');
						var closeBracketCount = 0;

						interfaceName = interfaceName.RightAt(true, (n, c) => LastDotCharProcessor(n, c, ref closeBracketCount));

						if (interfaceTypes.Any(t => t.GetCodeDeclaration(true, false) == interfaceName && t.GetMethods().Cast<MethodInfo>().Any(m => m.Name == name && m.SignatureMatches(method))))
						{
							declaringType = interfaceTypes.Single(t => t.GetCodeDeclaration(true, false) == interfaceName && t.GetMethods().Cast<MethodInfo>().Any(m => m.Name == name && m.SignatureMatches(method)));
						}
					}
				}
			}

			if (includeModifiers)
			{
				modifierString = method.GetModifiersString().Append(" ");
			}

            if (addAttributes)
            {
                if (method.HasCustomAttributes())
                {
                    customAttributes = method.CustomAttributes.GetCustomAttributesString();
                }
            }

            if (includeDeclaringType)
			{
				if (noReturnType)
				{
					return customAttributes + modifierString + type + " " + declaringType.GetCodeDeclaration() + "." + name + GenerateParmStringSignature(method.GetParameters(), isExtension, addAttributes, skipThisParameter);
				}
				else
				{
					return customAttributes + modifierString + type + " " + declaringType.GetCodeDeclaration() + "." + name + GenerateParmStringSignature(method.GetParameters(), isExtension, addAttributes, skipThisParameter);
				}
			}
			else
			{
				if (noReturnType)
				{
					return customAttributes + name + GenerateParmStringSignature(method.GetParameters(), isExtension, addAttributes, skipThisParameter);
				}
				else
				{
					return customAttributes + modifierString + type + " " + name + GenerateParmStringSignature(method.GetParameters(), isExtension, addAttributes, skipThisParameter);
				}
			}
		}

		public static bool LastDotCharProcessor(int index, char ch, ref int bracketOrParenCount)
		{
			if (ch == '.' && bracketOrParenCount == 0)
			{
				return true;
			}
			else if (ch.IsOneOf('[', '<', '{', '('))
			{
				bracketOrParenCount++;
			}
			else if (ch.IsOneOf(']', '>', '}', ')'))
			{
				bracketOrParenCount--;
			}

			return false;
		}

		public static Type CreateDynamicType(string typeName, string assemblyFullName, string fileName)
		{
			var assemblyName = new AssemblyName(assemblyFullName);
			var domain = Thread.GetDomain();
			var name = new AssemblyName();
			var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(Path.GetFileName(fileName));
			var typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public);

			typeBuilder.CreateType();

			return moduleBuilder.GetTypes().Single();
		}

		public static AssemblyBuilder CreateDynamicAssembly(string assemblyFullName, string fileName)
		{
			var assemblyName = new AssemblyName(assemblyFullName);
			var domain = Thread.GetDomain();
			var name = new AssemblyName();
			var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
			var moduleBuilder = assemblyBuilder.DefineDynamicModule(Path.GetFileName(fileName));

			return assemblyBuilder;
		}

		public static void AddType(this AssemblyBuilder assemblyBuilder, string typeName)
		{
			var moduleBuilder = assemblyBuilder.GetDynamicModule("Module");
			var typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public);

			typeBuilder.CreateType();
		}

		public static string GenerateSignature(this ConstructorInfo constructor, bool includeDeclaringType = false, bool addAttributes = false, bool includeModifiers = false)
		{
			string name;
			var modifiers = string.Empty;
            var customAttributes = string.Empty;

            if (includeModifiers)
			{
				modifiers = constructor.GetModifiersString().Append(" ");
			}

            if (addAttributes)
            {
                if (constructor.HasCustomAttributes())
                {
                    customAttributes = constructor.CustomAttributes.GetCustomAttributesString();
                }
            }

            if (includeDeclaringType)
			{
				name = constructor.DeclaringType.GenerateTypeString();
			}
			else
			{
				name = constructor.Name;
			}

			return customAttributes + modifiers + name + GenerateParmStringSignature(constructor.GetParameters());
		}

		public static string GenerateGenericSignature(this ConstructorInfo constructor, Type[] genericArgs, bool includeDeclaringType = false, bool includeModifiers = false)
		{
			string name;
			var modifiers = string.Empty;

			if (includeModifiers)
			{
				modifiers = constructor.GetModifiersString().Append(" ");
			}

			if (includeDeclaringType)
			{
				name = constructor.DeclaringType.GenerateTypeString(genericArgs);
			}
			else
			{
				name = constructor.Name;
			}

			return modifiers + name + GenerateParmStringSignature(constructor.GetParameters());
		}

		public static string GenerateGenericSignature(this ConstructorInfo constructor, string[] genericArgs, bool includeDeclaringType = false, bool includeModifiers = false)
		{
			string name;
			var modifiers = string.Empty;

			if (includeModifiers)
			{
				modifiers = constructor.GetModifiersString().Append(" ");
			}

			if (includeDeclaringType)
			{
				name = constructor.DeclaringType.GenerateTypeString(genericArgs);
			}
			else
			{
				name = constructor.Name;
			}

			return modifiers + name + GenerateParmStringSignature(constructor.GetParameters());
		}

		private static string GenerateParmStringSignature(ParameterInfo[] parameters, IEnumerable<ParameterDeclaration> clrStringParameters, bool skipThisParameter = false)
		{
			var builder = new StringBuilder("(");
			var hasComma = false;
			var x = 0;
			var skipThisKeyword = skipThisParameter;

			foreach (var parameter in parameters)
			{
				var clrStringParameter = clrStringParameters.ElementAt(x);
				var parmTypeName = clrStringParameter.Type.GetName();

				if (skipThisParameter)
				{
					x++;
					skipThisParameter = false;
					continue;
				}
				else if (!skipThisKeyword)
				{
					builder.Append("this ");
				}

				if (parameter.IsOut)
				{
					builder.Append("out ");
				}
				else if (parameter.ParameterType.IsByRef)
				{
					builder.Append("ref ");
				}

				if (parmTypeName.Contains("`"))
				{
					builder.Append(parmTypeName.Substring(0, parmTypeName.IndexOf('`')));
				}
				else
				{
					builder.Append(parmTypeName);
				}

				builder.Append(" " + parameter.Name.ToCamelCase());

				if (parameter.IsOptional)
				{
					builder.Append(" = " + parameter.DefaultValue.AsDisplayText());
				}

				builder.Append(", ");
				hasComma = true;
				x++;
			}

			if (hasComma)
			{
				builder.Remove(builder.Length - 2, 2);
			}

			builder.Append(")");

			return builder.ToString();
		}

		private static string GenerateParmStringSignature(ParameterInfo[] parameters, bool isExtensionMethod = false, bool addAttributes = false, bool skipThisParameter = false)
		{
			var builder = new StringBuilder("(");
			var hasComma = false;
            var customAttributes = string.Empty;

            foreach (ParameterInfo parameter in parameters)
			{
				var parmTypeName = parameter.ParameterType.GetCodeDeclaration();

                if (addAttributes)
                {
                    if (parameter.HasCustomAttributes())
                    {
						customAttributes = parameter.CustomAttributes.GetCustomAttributesString().RegexReplace(@"\r?\n", " ");
						builder.Append(customAttributes);
                    }
                }

                if (isExtensionMethod && !skipThisParameter)
				{
					builder.Append("this ");
				}
				else if (parameter.IsOut)
				{
					builder.Append("out ");
				}
				else if (parameter.ParameterType.IsByRef)
				{
					builder.Append("ref ");
				}

				if (parmTypeName.Contains("`"))
				{
					builder.Append(parmTypeName.Substring(0, parmTypeName.IndexOf('`')));
				}
				else
				{
					builder.Append(parmTypeName);
				}

				if (parameter.ParameterType.IsGenericType)
				{
					builder.Append(GenerateGenericArgsString(parameter.ParameterType.GetGenericArguments()));
				}

				builder.Append(" " + parameter.Name.ToCamelCase());

				if (parameter.IsOptional)
				{
					builder.Append(" = " + parameter.DefaultValue.AsDisplayText(QuoteTextType.DoubleQuote));
				}

				builder.Append(", ");
				hasComma = true;
			}

			if (hasComma)
			{
				builder.Remove(builder.Length - 2, 2);
			}

			builder.Append(")");

			return builder.ToString();
		}

		public static string GetMethodName(this CLRStringMethod method)
		{
			var genericArguments = method.GetGenericArguments();
			var methodName = method.Name.GetShortName();
			string name;

			if (genericArguments.Count > 0)
			{
				var args = String.Join<string>(", ", genericArguments.ToArray());

				name = methodName + "<" + args + ">";
			}
			else
			{
				name = methodName;
			}

			return name;
		}


		public static string GetName(this TypeParameterDeclaration typeParameter)
		{
			string codeDeclaration;
			var typeName = typeParameter.Name.Text;

			switch (typeName)
			{
				case "Void":
					codeDeclaration = "void";
					break;
				case "Boolean":
					codeDeclaration = "bool";
					break;
				case "Byte":
					codeDeclaration = "byte";
					break;
				case "SByte":
					codeDeclaration = "sbyte";
					break;
				case "Char":
					codeDeclaration = "char";
					break;
				case "Decimal":
					codeDeclaration = "decimal";
					break;
				case "Double":
					codeDeclaration = "double";
					break;
				case "Single":
					codeDeclaration = "float";
					break;
				case "Int32":
					codeDeclaration = "int";
					break;
				case "UInt32":
					codeDeclaration = "uint";
					break;
				case "Int64":
					codeDeclaration = "long";
					break;
				case "UInt64":
					codeDeclaration = "ulong";
					break;
				case "Object":
					codeDeclaration = "object";
					break;
				case "Int16":
					codeDeclaration = "short";
					break;
				case "UInt16":
					codeDeclaration = "ushort";
					break;
				case "String":
					codeDeclaration = "string";
					break;
				default:
					codeDeclaration = typeName;
					break;
			}

			return codeDeclaration;
		}

		public static List<string> GetGenericArguments(this CLRStringMethod method)
		{
			var list = new List<string>();

			foreach (var argument in method.TypeParameters)
			{
				list.Add(argument.GetName());
			}

			return list;
		}

		public static string GetMethodName(this MethodInfo method, bool truncateDottedMethodName = false)
		{
			var genericArguments = method.GetGenericArguments();
			string name;

			if (genericArguments.Length > 0)
			{
				var args = String.Join<string>(", ", genericArguments.AsEnumerable().Select(a => a.GetCodeDeclaration()).ToArray<string>());

				if (truncateDottedMethodName && method.Name.Contains('.'))
				{
					name = method.Name.RightAtLastIndexOf('.') + "<" + args + ">";
				}
				else
				{
					name = method.Name + "<" + args + ">";
				}
			}
			else
			{
				if (truncateDottedMethodName && method.Name.Contains('.'))
				{
					name = method.Name.RightAtLastIndexOf('.');
				}
				else
				{
					name = method.Name;
				}
			}

			return name;
		}

		public static string GetCodeDeclaration(this Type type, bool usePrimitiveShortNames = true, bool useGenericPrimitiveShortNames = true, bool addAttributes = false, bool addNamespace = false)
		{
			var codeDeclaration = string.Empty;
			var customAttributes = string.Empty;

			if (type.IsGenericType)
			{
				var args = string.Join<string>(", ", type.GetGenericArguments().AsEnumerable().Select(a => a.GetCodeDeclaration(useGenericPrimitiveShortNames)).ToArray<string>());

				codeDeclaration = type.Name.Substring(0, type.Name.IndexOf('`')) + "<" + args + ">";
			}
			else
			{
				if (usePrimitiveShortNames)
				{
					switch (type.FullName)
					{
						case "System.Void":
							codeDeclaration = "void";
							break;
						case "System.Boolean":
							codeDeclaration = "bool";
							break;
						case "System.Byte":
							codeDeclaration = "byte";
							break;
						case "System.SByte":
							codeDeclaration = "sbyte";
							break;
						case "System.Char":
							codeDeclaration = "char";
							break;
						case "System.Decimal":
							codeDeclaration = "decimal";
							break;
						case "System.Double":
							codeDeclaration = "double";
							break;
						case "System.Single":
							codeDeclaration = "float";
							break;
						case "System.Int32":
							codeDeclaration = "int";
							break;
						case "System.UInt32":
							codeDeclaration = "uint";
							break;
						case "System.Int64":
							codeDeclaration = "long";
							break;
						case "System.UInt64":
							codeDeclaration = "ulong";
							break;
						case "System.Object":
							codeDeclaration = "object";
							break;
						case "System.Int16":
							codeDeclaration = "short";
							break;
						case "System.UInt16":
							codeDeclaration = "ushort";
							break;
						case "System.String":
							codeDeclaration = "string";
							break;
						default:

							if (type.Name.EndsWith("&"))
							{
								codeDeclaration = type.GetElementType().Name;
							}
							else
							{
								codeDeclaration = type.Name;
							}

							break;
					}
				}
				else
				{
					codeDeclaration = type.FullName;
				}
			}

			if (addAttributes)
			{
				if (type.HasCustomAttributes())
				{
					customAttributes = type.CustomAttributes.GetCustomAttributesString();
                }
			}

			if (addNamespace)
			{
				return customAttributes + type.Namespace + "." + codeDeclaration;
			}
			else
			{
				return customAttributes + codeDeclaration;
			}
		}

		public static string GetCustomAttributesString(this IEnumerable<System.Reflection.CustomAttributeData> customAttributes)
		{
			var builder = new StringBuilder();

			foreach (var customAttribute in customAttributes)
			{
				builder.AppendLine(customAttribute.ToString());
			}

			return builder.ToString();
		}

		public static IEnumerable<T> GetCustomAttributes<T>(this Type type)
		{
			var attributes = type.GetCustomAttributes(true).OfType<T>();

			return attributes;
		}

		public static bool IsArrayOf<T>(this Type type)
		{
			return type == typeof(T[]);
		}

		public static bool HasCustomAttribute<T>(this Type type)
		{
			return type.GetCustomAttributes(true).OfType<T>().Count() > 0;
		}

        public static bool HasCustomAttribute<T>(this ParameterInfo parm)
        {
            return parm.GetCustomAttributes(true).OfType<T>().Count() > 0;
        }

        public static bool HasCustomAttributes(this Type type)
        {
            return type.GetCustomAttributes(true).Count() > 0;
        }

        public static bool HasCustomAttributes(this MemberInfo member)
        {
            return member.GetCustomAttributes(true).Count() > 0;
        }

        public static bool HasCustomAttributes(this ParameterInfo parm)
        {
            return parm.GetCustomAttributes(true).Count() > 0;
        }

        public static T GetCustomAttribute<T>(this Type type, string memberName) where T : Attribute
		{
			var memberInfo = type.GetMember(memberName).First();
			var attribute = memberInfo.GetCustomAttribute<T>();

			return attribute;
		}

		public static T GetCustomAttribute<T>(this Type enumType, object value) where T : Attribute
		{
			var memberInfo = (MemberInfo) enumType.GetFields().Single(f =>
			{
				return f.Name != "value__" && ((int) f.GetRawConstantValue()) == (int) value;
			});

			var attribute = memberInfo.GetCustomAttribute<T>();

			return attribute;
		}

		public static bool IsOfType(this object obj, Type typeToCheck)
		{
			return obj.GetType().GetAncestors(true).Any(t => t == typeToCheck || (t.IsGenericType && t.GetGenericTypeDefinition() == typeToCheck));
		}

		public static bool IsOfType(this Type type, Type typeToCheck)
		{
			return type.GetAncestors(true).Any(t => t == typeToCheck || (t.IsGenericType && t.GetGenericTypeDefinition() == typeToCheck));
		}

		public static bool IsOfType(this Type type, string typeToCheck)
		{
			return type.GetAncestors(true).Any(t => t.FullName == typeToCheck || (t.IsGenericType && t.GetGenericTypeDefinition().FullName == typeToCheck));
		}

		public static bool IsOfType<T>(this object obj)
		{
			return obj.GetType().GetAncestors(true).Any(t => t == typeof(T));
		}

		public static bool IsOfType<T>(this Type type)
		{
			return type.GetAncestors(true).Any(t => t == typeof(T));
		}

		public static List<Type> GetAncestors(this Type type, bool includeMe = false, bool includeInterfaces = false)
		{
			var list = new List<Type>();
			var parent = type.BaseType;

			if (includeMe)
			{
				list.Add(type);
			}

			while (parent != null)
			{
				list.Add(parent);
				parent = parent.BaseType;
			}

			if (includeInterfaces)
			{
				return list.Concat(type.GetInterfaces()).ToList();
			}
			else
			{
				return list;
			}
		}

		public static bool IsQualifiedName(string name)
		{
			var regex = new Regex(@"^(?<assembly>[\w\.]+)(,\s?Version=(?<version>\d+\.\d+\.\d+\.\d+))?(,\s?Culture=(?<culture>\w+))?(,\s?PublicKeyToken=(?<token>\w+))?$");

			if (regex.IsMatch(name))
			{
				var match = regex.Match(name);
				var assemblyName = match.Groups["assembly"].Value;
				var version = match.Groups["version"].Value;
				var culture = match.Groups["culture"].Value;
				var token = match.Groups["token"].Value;

				return AnyHasValue(version, culture, token);
			}
			else
			{
				return false;
			}
		}

		public static bool Is<T>(this Type type)
		{
			return type.IsOneOf(typeof(T));
		}

		public static bool IsOneOf<T1, T2>(this Type type)
		{
			return type.IsOneOf(typeof(T1), typeof(T2));
		}

		public static bool IsOneOf<T1, T2, T3>(this Type type)
		{
			return type.IsOneOf(typeof(T1), typeof(T2), typeof(T3));
		}

		public static bool IsOneOf<T1, T2, T3, T4>(this Type type)
		{
			return type.IsOneOf(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
		}

		public static bool IsOneOf<T1, T2, T3, T4, T5>(this Type type)
		{
			return type.IsOneOf(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
		}

		public static bool Implements(this Type type, Type typeImplemented)
		{
			return type.GetAncestors(true).Any(t => t.FullName == typeImplemented.FullName || t.GetInterfaces().Any(i => i.FullName == typeImplemented.FullName || i.Implements(typeImplemented)));
		}

		public static bool Implements<T>(this Type type)
		{
			return type.GetAncestors(true).Any(t => t.FullName == typeof(T).FullName || t.GetInterfaces().Any(i => i.FullName == typeof(T).FullName || i.Implements<T>()));
		}

		public static bool IsGenericCollectionOf<T>(this Type type)
		{
			if (type.IsGenericType && type.Implements<IEnumerable>())
			{
				return type.GetGenericArguments().Any(t => t.Implements<T>());
			}

			return false;
		}

		public static bool IsGenericCollectionOf(this Type type, Type genericCollectionOfType)
		{
			if (type.IsGenericType && type.Implements<IEnumerable>())
			{
				return type.GetGenericArguments().Any(t => t.Implements(genericCollectionOfType));
			}

			return false;
		}

		public static bool IsGenericCollection(this Type type)
		{
			if (type.IsGenericType && type.Implements<IEnumerable>())
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static Type GetGenericArgumentOfType<T>(this Type type)
		{
			return type.GetGenericArguments().Single(t => t.Implements<T>());
		}

		public static bool AnyHasValue(params object[] values)
		{
			foreach (var value in values)
			{
				if (value is string && !string.IsNullOrEmpty((string) value))
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsStruct(this Type type)
		{
			return type.IsValueType && !(type.IsPrimitive || type.IsEnum || type.IsInterface);
		}
		
		public static bool IsSerializable(this Type type)
		{
			try
			{
				if (type.IsValueType || type == typeof(string))
				{
					return true;
				}
				else if (type.IsArray)
				{
					var elementType = type.GetElementType();

					return elementType.IsSerializable();
				}
				else
				{
					var instance = Activator.CreateInstance(type);
					var stream = Serialize(instance);
					var newInstance = instance.Clone(type);

					if (instance.GetType() == newInstance.GetType())
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
			catch
			{
				return false;
			}
		}

		public static Stream Serialize(object source) 
		{
			var serializer = new XmlSerializer(source.GetType());
			var stream = new MemoryStream(); 

			serializer.Serialize(stream, source); 

			return stream; 
		}  
		
		public static T Deserialize<T>(this Stream stream) 
		{
			var serializer = new XmlSerializer(typeof(T));
			stream.Position = 0;

			return (T)serializer.Deserialize(stream); 
		}

		public static object Deserialize(this Stream stream, Type type)
		{
			var serializer = new XmlSerializer(type);

			stream.Position = 0;

			return serializer.Deserialize(stream);
		}

		public static object CloneJson(this object source)
		{
			var json = JsonExtensions.ToJsonDynamic(source);

			return JsonExtensions.ReadJson<object>(json);
		}

		public static T CloneJson<T>(this object source)
		{
			var json = JsonExtensions.ToJsonDynamic(source);

			return JsonExtensions.ReadJson<T>(json);
		}

		public static T CloneJson<T>(this T source)
		{
			var json = JsonExtensions.ToJsonText(source);

			return JsonExtensions.ReadJson<T>(json);
		}

		public static T Clone<T>(this object source) 
		{ 
			return Deserialize<T>(Serialize(source)); 
		}

		public static object Clone(this object source, Type type)
		{
			return Deserialize(Serialize(source), type);
		}
	}
}
