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
using System.Xml.XPath;
using System.Diagnostics;

namespace Utils
{
    internal static class AssemblyCloneExtensions
    {
        public static XDocument ToXml(this AssemblyClone assembly)
        {
            var serializer = XmlSerializer.FromTypes(new Type[] { typeof(AssemblyClone) })[0];
            var document = new XmlDocument();
            var memoryStream = new MemoryStream();

            serializer.Serialize(memoryStream, assembly);

            memoryStream.Seek(0, SeekOrigin.Begin);

            return XDocument.Load(memoryStream);
        }
    }

    [Serializable, XmlRoot("Assembly")]
    public class AssemblyClone : MarshalByRefObject, _Assembly
    {
        private Assembly assembly;
        private AssemblyNameClone assemblyName;
        private string codeBase;
        private string fullName;
        private string assemblyLocation;
        private ReferencedAssemblies referencedAssemblies;
        private List<CustomAttributeData> customAttributesData;
        private bool globalAssemblyCache;

        public AssemblyClone()
        {
        }

        public AssemblyClone(XDocument document)
        {
            foreach (var element in document.Root.Elements())
            {
                switch (element.Name.LocalName)
                {
                    case "CodeBase":
                        codeBase = element.Value;
                        break;
                    case "FullName":
                        fullName = element.Value;
                        break;
                    case "Name":
                        assemblyName = new AssemblyNameClone(element);
                        break;
                    case "Location":
                        assemblyLocation = element.Value;
                        break;
                    case "GlobalAssemblyCache":
                        globalAssemblyCache = bool.Parse(element.Value);
                        break;
                    case "ReferencedAssemblies":

                        var assemblyNames = element.XPathSelectElements("Assemblies/AssemblyNameClone").Select(e => new AssemblyNameClone(e)).ToList();

                        if (referencedAssemblies == null)
                        {
                            referencedAssemblies = new ReferencedAssemblies(assemblyNames);
                        }

                        break;

                    case "CustomAttributeDataList":

                        customAttributesData = element.XPathSelectElements("CustomAttributeData").Select(e => new CustomAttributeData(e)).ToList();
                        break;

                    default:
                        Debugger.Break();
                        break;
                }
            }
        }

        public AssemblyClone(Assembly assembly)
        {
            this.assembly = assembly;
            this.assemblyName = new AssemblyNameClone(assembly.GetName());
        }

        [XmlElement]
        public string CodeBase
        {
            get 
            {
                if (assembly != null)
                {
                    return assembly.CodeBase;
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

        [XmlArray("CustomAttributeDataList")]
        [XmlArrayItem(typeof(CustomAttributeData), ElementName = "CustomAttributeData")]
        public List<CustomAttributeData> CustomAttributeDataList
        {
            get
            {
                if (assembly != null)
                {
                    var list = Utils.CustomAttributeData.FromList(assembly.GetCustomAttributesData());

                    return list;
                }
                else
                {
                    return customAttributesData;
                }
            }

            set
            {
                customAttributesData = value;
            }
        }

        public IList<System.Reflection.CustomAttributeData> GetCustomAttributesData()
        {
            if (assembly != null)
            {
                return assembly.GetCustomAttributesData();
            }
            else
            {
                return Utils.CustomAttributeData.ToList(customAttributesData);
            }
        }

        public object CreateInstance(string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes)
        {
            throw new NotImplementedException();
        }

        public object CreateInstance(string typeName, bool ignoreCase)
        {
            throw new NotImplementedException();
        }

        public object CreateInstance(string typeName)
        {
            throw new NotImplementedException();
        }

        [XmlIgnore]
        public MethodInfo EntryPoint
        {
            get { throw new NotImplementedException(); }
        }

        [XmlIgnore]
        public string EscapedCodeBase
        {
            get { throw new NotImplementedException(); }
        }

        [XmlIgnore]
        public Evidence Evidence
        {
            get { throw new NotImplementedException(); }
        }

        [XmlElement]
        public string FullName
        {
            get
            {
                if (assembly != null)
                {
                    return assembly.FullName;
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
        public AssemblyNameClone Name
        {
            get 
            {
                return assemblyName;
            }

            set
            {
                assemblyName = value;
            }
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public Type[] GetExportedTypes()
        {
            throw new NotImplementedException();
        }

        public FileStream GetFile(string name)
        {
            throw new NotImplementedException();
        }

        public FileStream[] GetFiles(bool getResourceModules)
        {
            throw new NotImplementedException();
        }

        public FileStream[] GetFiles()
        {
            throw new NotImplementedException();
        }

        public Module[] GetLoadedModules(bool getResourceModules)
        {
            throw new NotImplementedException();
        }

        public Module[] GetLoadedModules()
        {
            throw new NotImplementedException();
        }

        public ManifestResourceInfo GetManifestResourceInfo(string resourceName)
        {
            throw new NotImplementedException();
        }

        public string[] GetManifestResourceNames()
        {
            throw new NotImplementedException();
        }

        public Stream GetManifestResourceStream(string name)
        {
            throw new NotImplementedException();
        }

        public Stream GetManifestResourceStream(Type type, string name)
        {
            throw new NotImplementedException();
        }

        public Module GetModule(string name)
        {
            throw new NotImplementedException();
        }

        public Module[] GetModules(bool getResourceModules)
        {
            throw new NotImplementedException();
        }

        public Module[] GetModules()
        {
            throw new NotImplementedException();
        }

        public AssemblyName GetName(bool copiedName)
        {
            throw new NotImplementedException();
        }

        public AssemblyName GetName()
        {
            return new AssemblyName(assemblyName.FullName) { CodeBase = assemblyName.CodeBase, Name = assemblyName.Name, Version = assemblyName.Version };
        }

        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            throw new NotImplementedException();
        }

        [XmlElement]
        public ReferencedAssemblies ReferencedAssemblies
        {
            get
            {
                if (assembly != null)
                {
                    return new ReferencedAssemblies(assembly.GetReferencedAssemblies().Select(a => new AssemblyNameClone(a)).ToList<AssemblyNameClone>());
                }
                else
                {
                    return referencedAssemblies;
                }
            }

            set
            {
                referencedAssemblies = value;
            }
        }

        public AssemblyName[] GetReferencedAssemblies()
        {
            if (assembly != null)
            {
                return assembly.GetReferencedAssemblies();
            }
            else
            {
                return referencedAssemblies.Assemblies.Select(a => new AssemblyName(a.FullName) { CodeBase = a.CodeBase, Name = a.Name, Version = a.Version }).ToArray<AssemblyName>();
            }
        }

        public Assembly GetSatelliteAssembly(System.Globalization.CultureInfo culture, Version version)
        {
            throw new NotImplementedException();
        }

        public Assembly GetSatelliteAssembly(System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public Type GetType(string name, bool throwOnError, bool ignoreCase)
        {
            throw new NotImplementedException();
        }

        public Type GetType(string name, bool throwOnError)
        {
            throw new NotImplementedException();
        }

        public Type GetType(string name)
        {
            throw new NotImplementedException();
        }

        public Type[] GetTypes()
        {
            throw new NotImplementedException();
        }

        [XmlElement]
        public bool GlobalAssemblyCache
        {
            get 
            {
                if (assembly != null)
                {
                    return assembly.GlobalAssemblyCache;
                }
                else
                {
                    return globalAssemblyCache;
                }
            }

            set
            {
                globalAssemblyCache = value;
            }
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public Module LoadModule(string moduleName, byte[] rawModule, byte[] rawSymbolStore)
        {
            throw new NotImplementedException();
        }

        public Module LoadModule(string moduleName, byte[] rawModule)
        {
            throw new NotImplementedException();
        }

        [XmlElement]
        public string Location
        {
            get 
            {
                if (assembly != null)
                {
                    return assembly.Location;
                }
                else
                {
                    return assemblyLocation;
                }
            }

            set
            {
                assemblyLocation = value;
            }
        }

        public event ModuleResolveEventHandler ModuleResolve;
    }
}
