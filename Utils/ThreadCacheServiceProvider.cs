using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class ThreadCacheServiceProvider : IServiceProvider, IDisposable
    {
        private IServiceProvider internalServiceProvider;
        private static ThreadLocal<Dictionary<Type, object>> cache = new ThreadLocal<Dictionary<Type, object>>(() => new Dictionary<Type, object>());

        public ThreadCacheServiceProvider(IServiceProvider serviceProvider)
        {
            this.internalServiceProvider = serviceProvider;
        }

        public object? GetService(Type serviceType)
        {
            object? service;

            if (cache.Value.ContainsKey(serviceType))
            {
                service = cache.Value[serviceType];
            }
            else
            {
                service = internalServiceProvider.GetService(serviceType);
                cache.Value.Add(serviceType, service);
            }

            return service;
        }

        public void Dispose()
        {
            cache.Value.Clear();
        }
    }
}
