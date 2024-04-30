using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.InteropServices;

namespace Utils
{
    public class AssemblyLoader : MarshalByRefObject
    {
        private static Dictionary<object, AppDomain> domainMap;

        public static _Assembly LoadClone(string assemblyPath)
        {
            return LoadClone(assemblyPath, null);
        }

        public static _Assembly LoadClone(AssemblyName name)
        {
            return LoadClone(name, null);
        }

        internal static _Assembly LoadClone(AssemblyName name, AppDomain domain = null)
        {
            bool oneTimeLoad = false;

            if (domain == null)
            {
                var setup = AppDomain.CurrentDomain.SetupInformation;

                domain = AppDomain.CreateDomain("AssemblyLoaderDomain", AppDomain.CurrentDomain.Evidence, setup);

                oneTimeLoad = true;
            }

            var handle = domain.CreateInstance(typeof(AssemblyLoader).Assembly.FullName, typeof(AssemblyLoader).FullName);
            var loader = (AssemblyLoader)handle.Unwrap();

            var document = loader.InternalLoad(name).ToXml();

            if (oneTimeLoad)
            {
                AppDomain.Unload(domain);
            }

            return new AssemblyClone(document);
        }

        internal static _Assembly LoadClone(string assemblyPath, AppDomain domain = null)
        {
            bool oneTimeLoad = false;

            if (domain == null)
            {
                var setup = AppDomain.CurrentDomain.SetupInformation;

                domain = AppDomain.CreateDomain("AssemblyLoaderDomain", AppDomain.CurrentDomain.Evidence, setup);

                oneTimeLoad = true;
            }

            var handle = domain.CreateInstance(typeof(AssemblyLoader).Assembly.FullName, typeof(AssemblyLoader).FullName);
            var loader = (AssemblyLoader)handle.Unwrap();

            var document = loader.InternalLoad(assemblyPath).ToXml();

            if (oneTimeLoad)
            {
                AppDomain.Unload(domain);
            }

            return new AssemblyClone(document);
        }

        public static T LoadAssemblyInAppDomain<T>(AppDomain domain = null)
        {
            var storeDomain = false;
            var assembly = typeof(T).Assembly;

            if (domain == null)
            {
                var setup = AppDomain.CurrentDomain.SetupInformation;

                domain = AppDomain.CreateDomain("AssemblyLoaderDomain", AppDomain.CurrentDomain.Evidence, setup);

                if (domainMap == null)
                {
                    domainMap = new Dictionary<object, AppDomain>();
                }

                storeDomain = true;
            }

            var handle = domain.CreateInstance(assembly.FullName, typeof(T).FullName);
            var instance = (T)handle.Unwrap();

            if (storeDomain)
            {
                domainMap.Add(instance, domain);
            }

            return instance;
        }

        public static AppDomain GetDomain(object obj)
        {
            if (domainMap.ContainsKey(obj))
            {
                var domain = domainMap[obj];

                return domain;
            }

            return null;
        }

        public static void UnloadAssemblyForWrappedObject(object obj)
        {
            if (domainMap.ContainsKey(obj))
            {
                var domain = domainMap[obj];

                AppDomain.Unload(domain);
            }
        }

        public static IAssemblyCloneLoader CreateLoader(bool useTempAssemblies = false)
        {
            return new AppDomainCloneLoader(useTempAssemblies);
        }

        private AssemblyClone InternalLoad(string path)
        {
            var assembly = Assembly.Load(AssemblyName.GetAssemblyName(path));
            var clone = new AssemblyClone(assembly);

            return clone;
        }

        private AssemblyClone InternalLoad(AssemblyName assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);
            var clone = new AssemblyClone(assembly);

            return clone;
        }
    }
}
