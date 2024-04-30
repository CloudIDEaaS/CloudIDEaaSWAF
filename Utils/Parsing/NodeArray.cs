using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Parsing
{
    public class NodeArray<T> : List<T>, ITextRange
    {
        private List<Nodes.Node> list;

        public int Position { get; set; }
        public int End { get; set; }
        public bool? HasTrailingComma { get; set; }
        public TransformFlags? TransformFlags { get; set; }
    }   
}
