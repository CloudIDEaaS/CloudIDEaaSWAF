using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Parsing.Nodes
{
    public interface INode
    {
        int ID { get; set; }
        SyntaxKind Kind { get; set; }
        int Pos { get; set; }
        int End { get; set; }
        NodeFlags Flags { get; set; }
        ModifierFlags ModifierFlagsCache { get; set; }
        List<INode> Children { get; set; }
        INode Parent { get; set; }
        IDisposable CurrentDisposer { get; set; }
        Stack<INode> CurrentNodeStack { get; set; }
    }

    public abstract class Node : INode, IDisposable
    {
        public int ID { get; set; }
        public SyntaxKind Kind { get; set; }
        public int Pos { get; set; }
        public int End { get; set; }
        public NodeFlags Flags { get; set; }
        public ModifierFlags ModifierFlagsCache { get; set; }
        public List<INode> Children { get; set; }
        public INode Parent { get; set; }
        public IDisposable CurrentDisposer { get; set; }
        public Stack<INode> CurrentNodeStack { get; set; }

        public Node(SyntaxKind kind, int pos, int end)
        {
            this.ID = 0;
            this.Kind = kind;
            this.Pos = pos;
            this.End = end;
            this.Flags = NodeFlags.None;
            this.ModifierFlagsCache = ModifierFlags.None;

            this.Children = new List<INode>();
        }

        public static TNode Construct<TNode>(SyntaxKind kind, int pos, int end) where TNode : Node
        {
            var constructor = typeof(TNode).GetConstructor(new Type[] { typeof(SyntaxKind), typeof(int), typeof(int) });

            return (TNode) constructor.Invoke(new object[] { kind, pos, end });
        }

        public void Dispose()
        {
            if (CurrentDisposer != null)
            {
                CurrentDisposer.Dispose();
            }
        }
    }
}
