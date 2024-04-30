using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.StaticFiles;
using System.Linq;

namespace Utils.Kestrel
{
    public interface IKestrelOptionsService
    {
        FixedDictionary<int, KestrelOptionsInfo> Listeners { get; }
    }

    public class KestrelOptionsService : IKestrelOptionsService
    {
        private IServiceProvider serviceProvider;

        public FixedDictionary<int, KestrelOptionsInfo> Listeners { get; }

        public KestrelOptionsService()
        {
            Listeners = new FixedDictionary<int, KestrelOptionsInfo>(255);
        }

        public IKestrelOptionsService AddProvider(IServiceProvider p)
        {
            serviceProvider = p;

            return this;
        }
    }
}