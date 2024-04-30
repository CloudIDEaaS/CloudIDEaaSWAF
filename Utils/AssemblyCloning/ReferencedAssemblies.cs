using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Utils
{
    [Serializable, XmlRoot("ReferencedAssemblies")]
    public class ReferencedAssemblies
    {
        private List<AssemblyNameClone> assemblies;

        public ReferencedAssemblies()
        {
        }

        public ReferencedAssemblies(List<AssemblyNameClone> assemblies)
        {
            this.assemblies = assemblies;
        }

        public List<AssemblyNameClone> Assemblies
        {
            get
            {
                return assemblies;
            }
        }
    }
}
