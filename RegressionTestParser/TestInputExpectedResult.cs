using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWASPCoreRulesetParser.Emitter
{
    public class TestInputExpectedResult
    {
        public DefaultHttpContext HttpContext { get; set; }
        public ExpectedResult ExpectedResult { get; set; }
        public object Title { get; }
        public string TestFileName { get; }
        public bool StopMagic { get; set; }
        public bool IsEncodedRequest { get; internal set; }
        public string EncodedRequest { get; internal set; }

        public TestInputExpectedResult(object title, string testFileName)
        {
            this.Title = title;
            this.TestFileName = testFileName;
        }
    }
}
