// -----------------------------------------------------------------------
// <copyright file="AssemblyAttributes.cs" company="CloudIDEaaS Inc.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AssemblyAttributes
    {
        private Assembly assembly;

        public AssemblyAttributes(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public string Title
        {
            get
            {
                var attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

                return attributes.Length == 0 ? "" : ((AssemblyTitleAttribute)attributes[0]).Title;
            }
        }

        public string Description
        {
            get
            {
                var attributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

                return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string Product
        {
            get
            {
                var attributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);

                return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string Company
        { 
            get 
            {
                var attributes = assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false); 
                
                return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute)attributes[0]).Company; 
            } 
        }

        public string Copyright
        {
            get
            {
                var attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string GetCustom<T>(string name) where T : Attribute
        {
            var attributes = assembly.GetCustomAttributes(typeof(T), false);

            return attributes.Length == 0 ? "" : ((T)attributes[0]).GetPropertyValue<string>(name);
        }

        public Version VersionRaw 
        {
            get
            {
                var assemblyName = new AssemblyName(assembly.FullName);

                if (assemblyName == null)
                {
                    return null;
                }

                var version = assemblyName.Version;

                if (version == null)
                {
                    return null;
                }

                return version;
            }
        }

        public string Version
        {
            get
            {
                var assemblyName = new AssemblyName(assembly.FullName);

                if (assemblyName == null)
                {
                    return string.Empty;
                }

                var version = assemblyName.Version;

                if (version == null)
                {
                    return string.Empty;
                }

                return version.ToString();
            }
        }
    }
}
