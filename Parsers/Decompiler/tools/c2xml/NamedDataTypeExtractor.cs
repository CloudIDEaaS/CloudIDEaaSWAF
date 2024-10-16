﻿#region License
/* 
 * Copyright (C) 1999-2015 John Källén.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; see the file COPYING.  If not, write to
 * the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 */
#endregion

using Decompiler.Core.Types;
using Decompiler.Core.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Decompiler.Tools.C2Xml
{
    public class NamedDataTypeExtractor :
        DeclaratorVisitor<Func<NamedDataType,NamedDataType>>,
        DeclSpecVisitor<SerializedType>
    {
        private IEnumerable<DeclSpec> specs;
        private XmlConverter converter;
        private SerializedType dt;
        private Domain domain;
        private int byteSize;
        private CTokenType callingConvention;
        private CConstantEvaluator eval;
        private SimpleSize simpleSize;

        private enum SimpleSize
        {
            None,
            Char,
            WChar_t,
            Short,
            Int,
            Long,
            LongLong,
            Float,
            Double,
            LongDouble,
            Int64,
        }

        public NamedDataTypeExtractor(IEnumerable<DeclSpec> specs, XmlConverter converter)
        {
            this.specs = specs;
            this.converter = converter;
            this.callingConvention = CTokenType.None;
            this.eval = new CConstantEvaluator(converter.Constants);
            this.simpleSize = SimpleSize.None;
            foreach (var declspec in specs)
            {
                dt = declspec.Accept(this);
            }
        }

        public NamedDataType GetNameAndType(Declarator declarator)
        {
            var nt = new NamedDataType { DataType = dt, Size = byteSize };
            if (declarator != null)
            {
                nt = declarator.Accept(this)(nt);
            }
            return nt;
        }

        public Func<NamedDataType, NamedDataType> VisitId(IdDeclarator id)
        {
            return (nt) => new NamedDataType { Name = id.Name, DataType = nt.DataType, Size = nt.Size};
        }

        public Func<NamedDataType, NamedDataType> VisitArray(ArrayDeclarator array)
        {
            var fn = array.Declarator.Accept(this);
            return (nt) =>
            {
                nt = new NamedDataType
                {
                    Name = nt.Name,
                    DataType = new ArrayType_v1
                    {
                        ElementType = nt.DataType,
                        Length = array.Size != null
                            ? Convert.ToInt32(array.Size.Accept(eval))
                            : 0
                    }
                };
                return fn(nt);
            };
        }

        public Func<NamedDataType, NamedDataType> VisitField(FieldDeclarator field)
        {
            Func<NamedDataType, NamedDataType> fn;
            if (field.Declarator == null)
            {
                fn = (nt) => nt;
            }
            else 
            {
                fn = field.Declarator.Accept(this);
            }
            return fn;
        }

        public Func<NamedDataType,NamedDataType> VisitPointer(PointerDeclarator pointer)
        {
            Func<NamedDataType, NamedDataType> fn;
            if (pointer.Pointee != null)
            {
                fn = pointer.Pointee.Accept(this);
            }
            else
            {
                fn = f => f;
            }
            return (nt) =>
            {
                nt.DataType = new PointerType_v1
                {
                    DataType = nt.DataType,
                };
                nt.Size = 4;            //$BUG: this is also architecture-specific (2 for PDP-11 for instance)
                return fn(nt);
            };
        }

        public Func<NamedDataType, NamedDataType> VisitFunction(FunctionDeclarator function)
        {
            var fn = function.Declarator.Accept(this);
            return (nt) =>
            {
                var parameters = function.Parameters
                    .Select(p => ConvertParameter(p))
                    .ToArray();

                // Special case for C, where foo(void) means a function with no parameters,
                // not a function with one parameter of type "void".
                if (FirstParameterVoid(parameters))
                    parameters = new Argument_v1[0];

                Argument_v1 ret = null;
                if (nt.DataType != null)
                {
                    ret = new Argument_v1
                    {
                        Type = nt.DataType,
                    };
                }
                nt.DataType = new SerializedSignature
                {
                    Convention = callingConvention != CTokenType.None
                        ? callingConvention.ToString().ToLower()
                        : null,
                    ReturnValue = ret,
                    Arguments = parameters.ToArray(),
                };
                return fn(nt);
            };
        }

        private bool FirstParameterVoid(Argument_v1[] parameters)
        {
            if (parameters == null || parameters.Length != 1)
                return false;
            return parameters[0].Type is VoidType_v1;
        }

        private Argument_v1 ConvertParameter(ParamDecl decl)
        {
            if (decl.IsEllipsis)
            {
                return new Argument_v1
                {
                    Kind = new StackVariable_v1 { },
                    Name = "...",
                };
            }
            else
            {
                var ntde = new NamedDataTypeExtractor(decl.DeclSpecs, converter);
                var nt = ConvertArrayToPointer(ntde.GetNameAndType(decl.Declarator));
                return new Argument_v1
                {
                    Name = nt.Name,
                    Type = nt.DataType,
                };
            }
        }

        /// <summary>
        /// Converts any array parameters to pointer parameters.
        /// </summary>
        /// <remarks>The C language treats an array parameter as a pointer. Thus <code>
        /// int foo(int arr[]);
        /// </code> is equivalent to <code>
        /// int foo(int * ptr);
        /// </code>
        /// </remarks>
        /// <param name="nt"></param>
        /// <returns></returns>
        private NamedDataType ConvertArrayToPointer(NamedDataType nt)
        {
            var at = nt.DataType as ArrayType_v1;
            if (at != null)
            {
                return new NamedDataType
                {
                    Name = nt.Name,
                    DataType = new PointerType_v1 { DataType = at.ElementType },
                    Size = 4   //$BUGBUG: this is different for z80 and x86-64
                };
            }
            else
            {
                return nt;
            }
        }

        private int ToStackSize(int p)
        {
            const int align = 4;
            //$REVIEW: depends on type and call convention + alignment
            return ((p + (align-1)) / align) * align;
        }
        
        public Func<NamedDataType,NamedDataType> VisitCallConvention(CallConventionDeclarator conv)
        {
            return (nt) => conv.Declarator.Accept(this)(nt);
        }

        public SerializedType VisitSimpleType(SimpleTypeSpec simpleType)
        {
            switch (simpleType.Type)
            {
            default:
                throw new NotImplementedException(string.Format("{0}", simpleType.Type));
            case CTokenType.Void:
                if (domain != Domain.None)
                    throw new FormatException(string.Format("Can't have 'void' after '{0}'.", domain));
                return new VoidType_v1();
            case CTokenType.__W64:
                return dt;      // Used by Microsoft compilers for 32->64 bit transition, deprecated.
            case CTokenType.Signed:
                if (domain != Domain.None)
                    throw new FormatException(string.Format("Can't have 'signed' after '{0}'.", domain));
                domain = Domain.SignedInt;
                byteSize = 4;                   // 'unsigned' == 'unsigned int'
                //$TODO: bitsize is platform-dependent. For instance, a 'long' is 32-bits on Windows x86-64 but 64-bits on 64-bit Unix
                return CreatePrimitive();
            case CTokenType.Unsigned:
                if (domain != Domain.None)
                    throw new FormatException(string.Format("Can't have 'unsigned' after '{0}'.", domain));
                domain = Domain.UnsignedInt;
                byteSize = 4;                   // 'unsigned' == 'unsigned int'
                //$TODO: bitsize is platform-dependent. For instance, a 'long' is 32-bits on Windows x86-64 but 64-bits on 64-bit Unix
                return CreatePrimitive();
            case CTokenType.Char:
                if (domain == Domain.None)
                    domain = Domain.Character;
                else if (domain != Domain.SignedInt && domain != Domain.UnsignedInt)
                    throw new FormatException(string.Format("Unexpected domain {0}.", domain));
                simpleSize = SimpleSize.Char;
                return CreatePrimitive();
            case CTokenType.Wchar_t:
                if (domain == Domain.None)
                    domain = Domain.Character;
                else if (domain != Domain.SignedInt && domain != Domain.UnsignedInt)
                    throw new FormatException(string.Format("Unexpected domain {0}", domain));
                simpleSize = SimpleSize.WChar_t;
                return CreatePrimitive();
            case CTokenType.Short:
                if (domain != Domain.None && domain != Domain.SignedInt && domain != Domain.UnsignedInt)
                    throw new FormatException(string.Format("Unexpected domain {0}", domain));
                simpleSize = SimpleSize.Short;
                return CreatePrimitive();
            case CTokenType.Int:
                if (domain == Domain.None)
                    domain = Domain.SignedInt;
                else if (domain != Domain.SignedInt && domain != Domain.UnsignedInt)
                    throw new FormatException(string.Format("Unexpected domain {0}", domain));
                if (simpleSize == SimpleSize.None)
                    simpleSize = SimpleSize.Int;
                return CreatePrimitive();
            //$TODO: bitsize is platform-dependent. For instance, an 'int' is 32-bits on Windows x86-64 but 16-bits on MS-DOS
            case CTokenType.Long:
                if (simpleSize == SimpleSize.None)
                    simpleSize = SimpleSize.Long;
                else if (simpleSize == SimpleSize.Long)
                    simpleSize = SimpleSize.LongLong;
                return CreatePrimitive();
            //$TODO: bitsize is platform-dependent. For instance, a 'long' is 32-bits on Windows x86-64 but 64-bits on 64-bit Unix
            case CTokenType.__Int64:
                if (domain == Domain.None)
                    domain = Domain.SignedInt;
                else if (domain != Domain.SignedInt && domain != Domain.UnsignedInt)
                    throw new FormatException(string.Format("Unexpected domain {0}", domain));
                simpleSize = SimpleSize.Int64;
                return CreatePrimitive();
            case CTokenType.Float:
                if (domain != Domain.None)
                    throw new FormatException(string.Format("Unexpected domain {0} before float.", domain));
                domain = Domain.Real;
                simpleSize = SimpleSize.Float;
                return CreatePrimitive();
            case CTokenType.Double:
                if (domain != Domain.None && domain != Domain.SignedInt)  //$REVIEW: short double? long double? long long double?
                    throw new FormatException(string.Format("Unexpected domain {0} before float.", domain));
                domain = Domain.Real;
                if (simpleSize == SimpleSize.None)
                    simpleSize = SimpleSize.Double;
                else if (simpleSize == SimpleSize.Long)
                    simpleSize = SimpleSize.LongDouble;
                return CreatePrimitive();
            }
        }

        private PrimitiveType_v1 CreatePrimitive()
        {
            //$BUG: all these are architecture depeendent.
            switch (simpleSize)
            {
            case SimpleSize.None: byteSize = 0; break;
            case SimpleSize.Char: byteSize = 1; break;
            case SimpleSize.WChar_t: byteSize = 2; break;
            case SimpleSize.Short: byteSize = 2; break;
            case SimpleSize.Int: byteSize = 4; break;
            case SimpleSize.Long: byteSize = 4; break;
            case SimpleSize.LongLong: byteSize = 8; break;
            case SimpleSize.Int64: byteSize = 8; break;
            case SimpleSize.Float: byteSize = 4; break;
            case SimpleSize.Double: byteSize = 8; break;
            case SimpleSize.LongDouble: byteSize = 8; break;
            default: throw new NotImplementedException();
            }
            if (domain == Domain.None)
                domain = Domain.SignedInt;
            return new PrimitiveType_v1
            {
                Domain = domain,
                ByteSize = byteSize
            };
        }

        public SerializedType VisitTypedef(TypeDefName typeDefName)
        {
            SerializedType type;
            if (!converter.NamedTypes.TryGetValue(typeDefName.Name, out type))
            {
                throw new ApplicationException(
                    string.Format(
                        "error: type name {0} not defined.",
                        typeDefName.Name ?? "(null)"));
            }
            byteSize = type.Accept(converter.Sizer);
            return new SerializedTypeReference(typeDefName.Name);
        }

        public SerializedType VisitComplexType(ComplexTypeSpec complexType)
        {
            if (complexType.Type == CTokenType.Struct)
            {
                SerializedStructType str;
                if (complexType.Name == null || converter.StructsSeen.TryGetValue(complexType.Name, out str))
                {
                    str = new SerializedStructType {
                        Name = complexType.Name != null
                            ? complexType.Name
                            : string.Format("struct_{0}", converter.StructsSeen.Count)
                    };
                    converter.StructsSeen.Add(str.Name, str);
                }
                else
                {
                    str = new SerializedStructType { Name = complexType.Name };
                }
                if (!complexType.IsForwardDeclaration() && str.Fields == null)
                {
                    str.Fields = ExpandStructFields(complexType.DeclList).ToArray();
                    converter.Sizer.SetSize(str);
                    converter.Types.Add(str);
                    str = new SerializedStructType { Name = str.Name };
                }
                return str;
            }
            else if (complexType.Type == CTokenType.Union)
            {
                UnionType_v1 un;
                if (complexType.Name == null || !converter.UnionsSeen.TryGetValue(complexType.Name, out un))
                {
                    un = new UnionType_v1 { Name = complexType.Name };
                    if (un.Name != null)
                    {
                        converter.UnionsSeen.Add(un.Name, un);
                    }
                }
                if (!complexType.IsForwardDeclaration() && un.Alternatives == null)
                {
                    un.Alternatives = ExpandUnionFields(complexType.DeclList).ToArray();
                    converter.Sizer.SetSize(un);
                    if (un.Name != null)
                    {
                        converter.Types.Add(un);
                        un = new UnionType_v1 { Name = un.Name };
                    }
                }
                return un;
            }
            else
                throw new NotImplementedException();
        }

        public SerializedType VisitEnum(EnumeratorTypeSpec e)
        {
            SerializedEnumType en;
            if (e.Tag == null || !converter.EnumsSeen.TryGetValue(e.Tag, out en))
            {
                en = new SerializedEnumType {
                    Name = e.Tag != null 
                        ? e.Tag 
                        : string.Format("enum_{0}", converter.EnumsSeen.Count)
                };
                converter.EnumsSeen.Add(en.Name, en);
                var enumEvaluator = new EnumEvaluator(new CConstantEvaluator(converter.Constants));
                var listMembers = new List<SerializedEnumValue>();
                foreach (var item in e.Enums)
                {
                    var ee = new SerializedEnumValue
                    {
                        Name = item.Name,
                        Value = enumEvaluator.GetValue(item.Value),
                    };
                    converter.Constants.Add(ee.Name, ee.Value);
                    listMembers.Add(ee);
                }
                en.Values = listMembers.ToArray();
                converter.Types.Add(en);
                en = new SerializedEnumType { Name = en.Name };
            }
            else
            {
                en = new SerializedEnumType { Name = e.Tag };
            }
            return en;
        }

        private IEnumerable<StructField_v1> ExpandStructFields(IEnumerable<StructDecl> decls)
        {
            int offset = 0;
            foreach (var decl in decls)
            {
                var ntde = new NamedDataTypeExtractor(decl.SpecQualifierList, converter);
                foreach (var declarator in decl.FieldDeclarators)
                {
                    var nt = ntde.GetNameAndType(declarator);
                    var rawSize = nt.DataType.Accept(converter.Sizer);
                    offset = Align(offset, rawSize, 8);     //$BUG: disregards temp. alignment changes. (__declspec(align))
                    yield return new StructField_v1
                    {
                        Offset = offset,
                        Name = nt.Name,
                        Type = nt.DataType,
                    };
                    offset += rawSize;
                }
            }
        }

        private IEnumerable<SerializedUnionAlternative> ExpandUnionFields(IEnumerable<StructDecl> decls)
        {
            foreach (var decl in decls)
            {
                var ndte = new NamedDataTypeExtractor(decl.SpecQualifierList, converter);
                foreach (var declarator in decl.FieldDeclarators)
                {
                    var nt = ndte.GetNameAndType(declarator);
                    yield return new SerializedUnionAlternative
                    {
                        Name = nt.Name,
                        Type = nt.DataType
                    };
                }
            }
        }

        private int Align(int offset, int size, int maxAlign)
        {
            size = Math.Min(maxAlign, size);
            if (size == 0)
                size = maxAlign;
            return size * ((offset + (size - 1)) / size);
        }

        public SerializedType VisitStorageClass(StorageClassSpec storageClassSpec)
        {
            switch (storageClassSpec.Type)
            {
            case CTokenType.__Cdecl:
            case CTokenType.__Fastcall:
            case CTokenType.__Stdcall:
                if (callingConvention != CTokenType.None)
                    throw new FormatException(string.Format("Unexpected extra calling convetion specifier '{0}'.", callingConvention));
                callingConvention = storageClassSpec.Type;
                break;
            }
            return dt;       //$TODO make use of CDECL.
        }

        public SerializedType VisitExtendedDeclspec(ExtendedDeclspec declspec)
        {
            return null;
        }

        public SerializedType VisitTypeQualifier(TypeQualifier typeQualifier)
        {
            return dt;      //$TODO: Ignoring 'const' and 'volatile' for now.
        }
    }
}
