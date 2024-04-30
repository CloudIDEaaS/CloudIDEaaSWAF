using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Parsing.Nodes;

namespace Utils.Parsing
{
    public class Diagnostic
    {
        public SyntaxTree SyntaxTree;
        public int Start;
        public int Length;
        public string MessageText;
        public DiagnosticCategory Category;
        public int Code;
    }

    public class DiagnosticMessage
    {
        public string Key;
        public DiagnosticCategory Category;
        public int Code;
        public string Message;
    }
}
