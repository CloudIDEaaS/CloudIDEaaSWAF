using System;
using System.Collections.Generic;
using System.Text;
using Utils.Wrappers.Interfaces;

namespace Utils
{
    public class TestingDnsProviderFactory : IDnsProviderFactory
    {
        public IDnsProvider CreateProvider()
        {
            return new TestingDnsProvider();
        }
    }
}
