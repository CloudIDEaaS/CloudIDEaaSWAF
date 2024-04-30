using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSecurity.KestrelWAF.RulesEngine;

namespace WebSecurity.KestrelWAF
{
    public interface ITransformationsActionsProvider
    {
        TransformationActionResult HandleTransformationsActions(Rule rule, WebContext webContext, Dictionary<string, string> groupCaptures, ContextMatches contextMatches, JToken TransformationsActions, bool failed, HttpContext httpContext);
    }
}
