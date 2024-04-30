using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Utils
{
    [DebuggerDisplay(" { DebugInfo } ")]
    public class AssemblyNameParts
    {
        public string TypeName { get; private set; }
        public string AssemblyName { get; private set; }
        public string Version { get; private set; }
        public string Culture { get; private set; }
        public string PublicKeyToken { get; private set; }
        public string ProcessorArchitecture { get; private set; }

        public AssemblyNameParts(string assemblyName, string version, string culture, string publicKeyToken, string processorArchitecture, string typeName = null)
        {
            this.AssemblyName = assemblyName;
            this.Version = version;
            this.Culture = culture;
            this.PublicKeyToken = publicKeyToken;
            this.ProcessorArchitecture = processorArchitecture;
            this.TypeName = typeName;
        }

        public string DebugInfo
        {
            get
            {
                if (this.TypeName != null)
                {
                    return string.Format(
                        "{0}, "
                        + "{1}, "
                        + "Version={2}, "
                        + "Culture={3}, "
                        + "PublicKeyToken={4}, "
                        + "processorArchitecture={5}",
                        this.TypeName,
                        this.AssemblyName,
                        this.Version.AsDisplayText(),
                        this.Culture.AsDisplayText(),
                        this.PublicKeyToken.AsDisplayText(),
                        this.ProcessorArchitecture.AsDisplayText());
                }
                else
                {
                    return string.Format(
                        "{0}, "
                        + "Version={1}, "
                        + "Culture={2}, "
                        + "PublicKeyToken={3}, "
                        + "processorArchitecture={4}",
                        this.AssemblyName,
                        this.Version.AsDisplayText(),
                        this.Culture.AsDisplayText(),
                        this.PublicKeyToken.AsDisplayText(),
                        this.ProcessorArchitecture.AsDisplayText());
                }
            }
        }
    }
}
