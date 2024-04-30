using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Utils.Parsing.Nodes
{
    [DebuggerDisplay("{DebugInfo}"), StructLayout(LayoutKind.Explicit)]
    public class PropertyName : IUnionNode
    {
        [FieldOffset(0)]
        public Identifier Identifier;
        [FieldOffset(0)]
        public StringLiteral StringLiteral;
        [FieldOffset(0)]
        public NumericLiteral NumericLiteral;
        [FieldOffset(0)]
        public ComputedPropertyName ComputedPropertyName;

        public int ID
        {
            get
            {
                return this.GetUnionPropertyValue<int>("ID");
            }
            set
            {
                this.SetUnionPropertyValue("ID", value);
            }
        }

        public SyntaxKind Kind
        {
            get
            {
                return this.GetUnionPropertyValue<SyntaxKind>("Kind");
            }
            set
            {
                this.SetUnionPropertyValue("Kind", value);
            }
        }

        public int Pos
        {
            get
            {
                return this.GetUnionPropertyValue<int>("Pos");
            }
            set
            {
                this.SetUnionPropertyValue("Pos", value);
            }
        }

        public int End
        {
            get
            {
                return this.GetUnionPropertyValue<int>("End");
            }
            set
            {
                this.SetUnionPropertyValue("End", value);
            }
        }

        public NodeFlags Flags
        {
            get
            {
                return this.GetUnionPropertyValue<NodeFlags>("Flags");
            }
            set
            {
                this.SetUnionPropertyValue("Flags", value);
            }
        }

        public ModifierFlags ModifierFlagsCache
        {
            get
            {
                return this.GetUnionPropertyValue<ModifierFlags>("ModifierFlagsCache");
            }
            set
            {
                this.SetUnionPropertyValue("ModifierFlagsCache", value);
            }
        }

        public List<INode> Children
        {
            get
            {
                return this.GetUnionPropertyValue<List<INode>>("Children");
            }
            set
            {
                this.SetUnionPropertyValue("Children", value);
            }
        }

        public INode Parent
        {
            get
            {
                return this.GetUnionPropertyValue<INode>("Parent");
            }
            set
            {
                this.SetUnionPropertyValue("Parent", value);
            }
        }

        public IDisposable CurrentDisposer
        {
            get
            {
                return this.GetUnionPropertyValue<IDisposable>("CurrentDisposer");
            }
            set
            {
                this.SetUnionPropertyValue("CurrentDisposer", value);
            }
        }

        public Stack<INode> CurrentNodeStack
        {
            get
            {
                return this.GetUnionPropertyValue<Stack<INode>>("CurrentNodeStack");
            }
            set
            {
                this.SetUnionPropertyValue("CurrentNodeStack", value);
            }
        }

        public string DebugInfo
        {
            get
            {
                return this.GetUnionPropertyValue<string>("DebugInfo");
            }
        }

        public static explicit operator PropertyName(Identifier identifier)
        {
            return new PropertyName { Identifier = identifier };
        }

        public static explicit operator PropertyName(LiteralExpression expression)
        {
            return new PropertyName { Identifier = new Identifier(SyntaxKind.Identifier, expression.Pos, expression.End) { Text = expression.Text } };
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public class DeclarationName : IUnionNode
    {
        [FieldOffset(0)]
        public Identifier Identifier;
        [FieldOffset(0)]
        public StringLiteral StringLiteral;
        [FieldOffset(0)]
        public NumericLiteral NumericLiteral;
        [FieldOffset(0)]
        public ComputedPropertyName ComputedPropertyName;
        [FieldOffset(0)]
        public BindingPattern BindingPattern;

        public int ID
        {
            get
            {
                return this.GetUnionPropertyValue<int>("ID");
            }
            set
            {
                this.SetUnionPropertyValue("ID", value);
            }
        }

        public SyntaxKind Kind
        {
            get
            {
                return this.GetUnionPropertyValue<SyntaxKind>("Kind");
            }
            set
            {
                this.SetUnionPropertyValue("Kind", value);
            }
        }

        public int Pos
        {
            get
            {
                return this.GetUnionPropertyValue<int>("Pos");
            }
            set
            {
                this.SetUnionPropertyValue("Pos", value);
            }
        }

        public int End
        {
            get
            {
                return this.GetUnionPropertyValue<int>("End");
            }
            set
            {
                this.SetUnionPropertyValue("End", value);
            }
        }

        public NodeFlags Flags
        {
            get
            {
                return this.GetUnionPropertyValue<NodeFlags>("Flags");
            }
            set
            {
                this.SetUnionPropertyValue("Flags", value);
            }
        }

        public ModifierFlags ModifierFlagsCache
        {
            get
            {
                return this.GetUnionPropertyValue<ModifierFlags>("ModifierFlagsCache");
            }
            set
            {
                this.SetUnionPropertyValue("ModifierFlagsCache", value);
            }
        }

        public List<INode> Children
        {
            get
            {
                return this.GetUnionPropertyValue<List<INode>>("Children");
            }
            set
            {
                this.SetUnionPropertyValue("Children", value);
            }
        }

        public INode Parent
        {
            get
            {
                return this.GetUnionPropertyValue<INode>("Parent");
            }
            set
            {
                this.SetUnionPropertyValue("Parent", value);
            }
        }

        public IDisposable CurrentDisposer
        {
            get
            {
                return this.GetUnionPropertyValue<IDisposable>("CurrentDisposer");
            }
            set
            {
                this.SetUnionPropertyValue("CurrentDisposer", value);
            }
        }

        public Stack<INode> CurrentNodeStack
        {
            get
            {
                return this.GetUnionPropertyValue<Stack<INode>>("CurrentNodeStack");
            }
            set
            {
                this.SetUnionPropertyValue("CurrentNodeStack", value);
            }
        }

        public static explicit operator DeclarationName(Identifier identifier)
        {
            return new DeclarationName { Identifier = identifier };
        }
    }

    [DebuggerDisplay("{DebugInfo}"), StructLayout(LayoutKind.Explicit)]
    public class BindingName : IUnionNode
    {
        [FieldOffset(0)]
        public Identifier Identifier;
        [FieldOffset(0)]
        public BindingPattern BindingPattern;

        public int ID
        {
            get
            {
                return this.GetUnionPropertyValue<int>("ID");
            }
            set
            {
                this.SetUnionPropertyValue("ID", value);
            }
        }

        public SyntaxKind Kind
        {
            get
            {
                return this.GetUnionPropertyValue<SyntaxKind>("Kind");
            }
            set
            {
                this.SetUnionPropertyValue("Kind", value);
            }
        }

        public int Pos
        {
            get
            {
                return this.GetUnionPropertyValue<int>("Pos");
            }
            set
            {
                this.SetUnionPropertyValue("Pos", value);
            }
        }

        public int End
        {
            get
            {
                return this.GetUnionPropertyValue<int>("End");
            }
            set
            {
                this.SetUnionPropertyValue("End", value);
            }
        }

        public NodeFlags Flags
        {
            get
            {
                return this.GetUnionPropertyValue<NodeFlags>("Flags");
            }
            set
            {
                this.SetUnionPropertyValue("Flags", value);
            }
        }

        public ModifierFlags ModifierFlagsCache
        {
            get
            {
                return this.GetUnionPropertyValue<ModifierFlags>("ModifierFlagsCache");
            }
            set
            {
                this.SetUnionPropertyValue("ModifierFlagsCache", value);
            }
        }

        public List<INode> Children
        {
            get
            {
                return this.GetUnionPropertyValue<List<INode>>("Children");
            }
            set
            {
                this.SetUnionPropertyValue("Children", value);
            }
        }

        public INode Parent
        {
            get
            {
                return this.GetUnionPropertyValue<INode>("Parent");
            }
            set
            {
                this.SetUnionPropertyValue("Parent", value);
            }
        }

        public IDisposable CurrentDisposer
        {
            get
            {
                return this.GetUnionPropertyValue<IDisposable>("CurrentDisposer");
            }
            set
            {
                this.SetUnionPropertyValue("CurrentDisposer", value);
            }
        }

        public Stack<INode> CurrentNodeStack
        {
            get
            {
                return this.GetUnionPropertyValue<Stack<INode>>("CurrentNodeStack");
            }
            set
            {
                this.SetUnionPropertyValue("CurrentNodeStack", value);
            }
        }

        public string DebugInfo
        {
            get
            {
                return this.GetUnionPropertyValue<string>("DebugInfo");
            }
        }

        public static explicit operator Identifier(BindingName name)
        {
            return name.Identifier;
        }

        public static explicit operator BindingName(Identifier identifier)
        {
            return new BindingName { Identifier = identifier };
        }
    }

    [DebuggerDisplay("{DebugInfo}"), StructLayout(LayoutKind.Explicit)]
    public class EntityName : IUnionNode
    {
        [FieldOffset(0)]
        public Identifier Identifier;
        [FieldOffset(0)]
        public QualifiedName QualifiedName;

        public int ID
        {
            get
            {
                return this.GetUnionPropertyValue<int>("ID");
            }
            set
            {
                this.SetUnionPropertyValue("ID", value);
            }
        }

        public SyntaxKind Kind
        {
            get
            {
                return this.GetUnionPropertyValue<SyntaxKind>("Kind");
            }
            set
            {
                this.SetUnionPropertyValue("Kind", value);
            }
        }

        public int Pos
        {
            get
            {
                return this.GetUnionPropertyValue<int>("Pos");
            }
            set
            {
                this.SetUnionPropertyValue("Pos", value);
            }
        }

        public int End
        {
            get
            {
                return this.GetUnionPropertyValue<int>("End");
            }
            set
            {
                this.SetUnionPropertyValue("End", value);
            }
        }

        public NodeFlags Flags
        {
            get
            {
                return this.GetUnionPropertyValue<NodeFlags>("Flags");
            }
            set
            {
                this.SetUnionPropertyValue("Flags", value);
            }
        }

        public ModifierFlags ModifierFlagsCache
        {
            get
            {
                return this.GetUnionPropertyValue<ModifierFlags>("ModifierFlagsCache");
            }
            set
            {
                this.SetUnionPropertyValue("ModifierFlagsCache", value);
            }
        }

        public List<INode> Children
        {
            get
            {
                return this.GetUnionPropertyValue<List<INode>>("Children");
            }
            set
            {
                this.SetUnionPropertyValue("Children", value);
            }
        }

        public INode Parent
        {
            get
            {
                return this.GetUnionPropertyValue<INode>("Parent");
            }
            set
            {
                this.SetUnionPropertyValue("Parent", value);
            }
        }

        public IDisposable CurrentDisposer
        {
            get
            {
                return this.GetUnionPropertyValue<IDisposable>("CurrentDisposer");
            }
            set
            {
                this.SetUnionPropertyValue("CurrentDisposer", value);
            }
        }

        public Stack<INode> CurrentNodeStack
        {
            get
            {
                return this.GetUnionPropertyValue<Stack<INode>>("CurrentNodeStack");
            }
            set
            {
                this.SetUnionPropertyValue("CurrentNodeStack", value);
            }
        }

        public string DebugInfo
        {
            get
            {
                return this.GetUnionPropertyValue<string>("DebugInfo");
            }
        }

        public static explicit operator EntityName(Identifier identifier)
        {
            return new EntityName { Identifier = identifier };
        }

        public static explicit operator EntityName(QualifiedName qualifiedName)
        {
            return new EntityName { QualifiedName = qualifiedName };
        }
    }
}
