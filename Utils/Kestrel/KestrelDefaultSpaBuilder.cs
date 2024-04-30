using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Kestrel
{
    internal sealed class KestrelDefaultSpaBuilder : IKestrelSpaBuilder
    {
        public IApplicationBuilder ApplicationBuilder { get; }

        public  KestrelSpaOptions Options { get; }

        public KestrelDefaultSpaBuilder(IApplicationBuilder applicationBuilder, KestrelSpaOptions options)
        {
            ApplicationBuilder = applicationBuilder ?? throw new ArgumentNullException("applicationBuilder");
            Options = options ?? throw new ArgumentNullException("options");
        }
    }
}
