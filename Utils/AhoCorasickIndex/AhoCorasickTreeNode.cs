using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.AhoCorasickIndex
{
    [DebuggerDisplay("Value = {Value}, TransitionCount = {_transitionsDictionary.Count}")]
    internal class AhoCorasickTreeNode
    {
        public char Value { get; private set; }
        public AhoCorasickTreeNode Failure { get; set; }
        public IEnumerable<string> Results { get { return _results; } }
        public AhoCorasickTreeNode ParentFailure { get { return _parent == null ? null : _parent.Failure; } }
        public IEnumerable<AhoCorasickTreeNode> Transitions { get { return _transitionsDictionary.Values; } }
        private readonly List<string> _results;
        private readonly Dictionary<char, AhoCorasickTreeNode> _transitionsDictionary;
        private readonly AhoCorasickTreeNode _parent;

        public AhoCorasickTreeNode() : this(null, ' ')
        {
        }

        private AhoCorasickTreeNode(AhoCorasickTreeNode parent, char value)
        {
            Value = value;
            _parent = parent;

            _results = new List<string>();
            _transitionsDictionary = new Dictionary<char, AhoCorasickTreeNode>();
        }

        public void AddResult(string result)
        {
            if (!_results.Contains(result))
            {
                _results.Add(result);
            }
        }

        public void AddResults(IEnumerable<string> results)
        {
            foreach (var result in results)
            {
                AddResult(result);
            }
        }

        public AhoCorasickTreeNode AddTransition(char c)
        {
            var node = new AhoCorasickTreeNode(this, c);
            _transitionsDictionary.Add(node.Value, node);

            return node;
        }

        public AhoCorasickTreeNode GetTransition(char c)
        {
            return _transitionsDictionary.ContainsKey(c)
                       ? _transitionsDictionary[c]
                       : null;
        }

        public bool ContainsTransition(char c)
        {
            return _transitionsDictionary.ContainsKey(c);
        }
    }
}
