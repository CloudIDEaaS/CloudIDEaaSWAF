using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Kestrel
{
    public class KestrelStaticFileResponseContext
    {
        /// <summary>
        /// Constructs the <see cref="StaticFileResponseContext"/>.
        /// </summary>
        /// <param name="context">The request and response information.</param>
        /// <param name="file">The file to be served.</param>
        public KestrelStaticFileResponseContext(HttpContext context, IFileInfo file)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            File = file ?? throw new ArgumentNullException(nameof(file));
        }

        /// <summary>
        /// The request and response information.
        /// </summary>
        public HttpContext Context { get; }

        /// <summary>
        /// The file to be served.
        /// </summary>
        public IFileInfo File { get; }
    }
}