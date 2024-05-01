using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Utils;

namespace WebSecurity.StartupTests
{
    [CommandLineParser(typeof(Program), null, typeof(Switches), "Runs tests in the '{ AssemblyProduct }' suite.")]
    public class CommandLineParseResult : ParseResultBase
    {
        public TestKind TestKinds { get; set; } = TestKind.UnitTests;
        public DatabaseKind DatabaseKind { get; set; } = DatabaseKind.SqlLite;
        public string TestToRun { get; set; }
        public MethodInfo TestToRunMethod { get; set; }
        public bool NoHeadlessBrowser { get; internal set; }
    }
}
