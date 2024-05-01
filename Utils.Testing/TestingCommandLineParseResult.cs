using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Utils;

namespace Utils
{
    public class TestingCommandLineParseResult : ParseResultBase
    {
        public TestKind TestKinds { get; set; } = TestKind.UnitTests;
        public DatabaseKind DatabaseKind { get; set; } = DatabaseKind.SqlLite;
        public string TestToRun { get; set; }
        public MethodInfo TestToRunMethod { get; set; }
    }
}
