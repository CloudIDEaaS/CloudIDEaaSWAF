using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSecurity.KestrelWAF;

namespace WebSecurity
{
    public class TransformationActionResult
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string? SkipAfter { get; set; }
        public List<ControlVariable> ControlVariables { get; }
        public bool IsChain { get; }

        public TransformationActionResult(HttpStatusCode statusCode, string? skipAfter, bool isChain, List<KestrelWAF.ControlVariable> controlVariables)
        {
            this.HttpStatusCode = statusCode;
            this.SkipAfter = skipAfter;
            this.ControlVariables = controlVariables;
            this.IsChain = isChain;
        }
    }
}
