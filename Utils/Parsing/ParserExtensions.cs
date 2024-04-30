using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Parsing.Nodes;

namespace Utils.Parsing
{
    public static class ParserExtensions
    {
        public static IDisposable PushNode(this Stack<INode> nodeStack, INode node)
        {
            var disposable = nodeStack.CreateDisposable((sender, e) => nodeStack.Pop());

            node.CurrentNodeStack = nodeStack;
            nodeStack.Push(node);

            return disposable;
        }

        public static List<INode> ToChildren(this IEnumerable<Node> nodes)
        {
           return nodes.Cast<INode>().ToList();
        }

        public static NodeArray<Node> ToNodeArray<T>(this IEnumerable<T> nodes) where T : Node
        {
            var nodeArray = new NodeArray<Node>();

            nodeArray.AddRange(nodes.Cast<Node>());

            return nodeArray;
        }

        public static IDisposable PushAddNode(this Stack<INode> nodeStack, INode node)
        {
            var disposable = nodeStack.CreateDisposable((sender, e) => nodeStack.Pop());

            nodeStack.AddNode(node);
            nodeStack.PushNode(node);

            return disposable;
        }

        public static INode PopNode(this Stack<INode> nodeStack)
        {
            return nodeStack.Pop();
        }

        public static INode PeekNode(this Stack<INode> nodeStack)
        {
            return nodeStack.Peek();
        }

        public static void AddNode(this Stack<INode> nodeStack, INode node)
        {
            if (nodeStack.Count > 0)
            {
                var parentNode = nodeStack.Peek();

                parentNode.Children.Add(node);
                node.Parent = parentNode;
            }

            node.CurrentNodeStack = nodeStack;
        }
    }
}
