using BTreeIndex.Collections.Generic.BTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class TypeCache : Dictionary<string, Type>
    {
        public bool IsStale { get; private set; }

        public TypeCache(bool rebuildOnAssemblyLoad)
        {
            this.IsStale = true;
            
            if (rebuildOnAssemblyLoad)
            {
                AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
            }
        }

        private void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            this.IsStale = true;
        }

        public void BuildIndex(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                if (!this.ContainsKey(type.FullName))
                {
                    this.Add(type.FullName, type);
                }
            }
        }
    }
}
