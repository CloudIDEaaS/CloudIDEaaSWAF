using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Kestrel
{
    public class KestrelMiddlewareStaticFileOptions : IOptions<StaticFileOptions>
    {
        public StaticFileOptions Value { get; }

        public KestrelMiddlewareStaticFileOptions(KestrelStaticFileOptions options)
        {
            Value = new StaticFileOptions
            {
                RequestPath = options.RequestPath
            };
        }
    }
}
