using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Security.Policy;
using System.IO;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;

namespace Utils
{
    [Serializable, DebuggerDisplay(" { FullName } "), XmlRoot("AssemblyName")]
    public class AssemblyNameClone : MarshalByRefObject
    {
        private AssemblyName assemblyName;
        private string codeBase;
        private string fullName;
        private string assemblyLocation;
        private string escapedCodeBase;
        private string name;
        private Utils.VersionClone versionClone;

        public AssemblyNameClone()
        {
        }

        public AssemblyNameClone(XElement element)
        {
            foreach (var subElement in element.Elements())
            {
                switch (subElement.Name.LocalName)
                {
                    case "CodeBase":
                        codeBase = subElement.Value;
                        break;
                    case "FullName":
                        fullName = subElement.Value;
                        break;
                    case "AssemblyLocation":
                        assemblyLocation = subElement.Value;
                        break;
                    case "EscapedCodeBase":
                        escapedCodeBase = subElement.Value;
                        break;
                    case "Name":
                        name = subElement.Value;
                        break;
                    case "VersionClone":
                        versionClone = new VersionClone(subElement);
                        break;
                    default:
                        Debugger.Break();
                        break;
                }
            }
        }

        public AssemblyNameClone(AssemblyName assemblyName)
        {
            this.assemblyName = assemblyName;
        }

        [XmlElement]
        public string CodeBase
        {
            get
            {
                if (this.assemblyName != null)
                {
                    return assemblyName.CodeBase;
                }
                else
                {
                    return codeBase;
                }
            }

            set
            {
                codeBase = value;
            }
        }

        [XmlElement]
        public string EscapedCodeBase
        {
            get
            {
                if (this.assemblyName != null)
                {
                    return assemblyName.EscapedCodeBase;
                }
                else
                {
                    return escapedCodeBase;
                }
            }

            set
            {
                escapedCodeBase = value;
            }
        }

        [XmlElement]
        public string FullName
        {
            get
            {
                if (this.assemblyName != null)
                {
                    return assemblyName.FullName;
                }
                else
                {
                    return fullName;
                }
            }

            set
            {
                fullName = value;
            }
        }

        [XmlElement]
        public string Name
        {
            get
            {
                if (this.assemblyName != null)
                {
                    return assemblyName.Name;
                }
                else
                {
                    return name;
                }
            }

            set
            {
                name = value;
            }
        }

        [XmlElement]
        public VersionClone VersionClone
        {
            get
            {
                if (this.assemblyName != null)
                {
                    return new VersionClone(assemblyName.Version);
                }
                else
                {
                    return versionClone;
                }
            }

            set
            {
                versionClone = value;
            }
        }

        [XmlIgnore]
        public Version Version
        {
            get
            {
                if (this.assemblyName != null)
                {
                    return assemblyName.Version;
                }
                else
                {
                    return new Version(versionClone.Major, versionClone.Minor, versionClone.Build, versionClone.Revision);
                }
            }
        }
    }
}
