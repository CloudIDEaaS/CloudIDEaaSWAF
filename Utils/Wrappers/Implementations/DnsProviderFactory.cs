using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Utils.Wrappers.Interfaces;

namespace Utils
{
    public class DnsProviderFactory : IDnsProviderFactory
    {
        public DnsProviderFactory()
        {
        }

        public IDnsProvider CreateProvider()
        {
            return new DnsProvider();
        }
    }
}
