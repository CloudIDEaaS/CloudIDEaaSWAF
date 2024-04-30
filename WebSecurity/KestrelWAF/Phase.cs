using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.KestrelWAF;

public enum Phase
{
    Invalid = 0,
    RequestHeaders,
    RequestBody,
    ResponseHeaders,
    ResponseBody,
    Logging
}
