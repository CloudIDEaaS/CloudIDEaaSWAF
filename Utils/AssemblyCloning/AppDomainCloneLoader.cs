using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace Utils
{
    internal class AppDomainCloneLoader : IAssemblyCloneLoader
    {
        private static AppDomain domain;
        private List<FileInfo> assemblyTempFiles;
        private bool useTempAssemblies;

        internal AppDomainCloneLoader(bool useTempAssemblies = false) 
        {
            var setup = AppDomain.CurrentDomain.SetupInformation;

            assemblyTempFiles = new List<FileInfo>();

            this.useTempAssemblies = useTempAssemblies;
            domain = AppDomain.CreateDomain("AssemblyLoaderDomain", AppDomain.CurrentDomain.Evidence, setup);
        }

        public _Assembly LoadAssemblyClone(string assemblyPath)
        {
            var file = new FileInfo(assemblyPath);

            Debug.Assert(file.Exists);

            if (useTempAssemblies)
            {
                var fileTemp = ToTemp(file);
                assemblyTempFiles.Add(fileTemp);

                return AssemblyLoader.LoadClone(fileTemp.FullName, domain);
            }
            else
            {
                return AssemblyLoader.LoadClone(file.FullName, domain);
            }
        }

        private FileInfo ToTemp(FileInfo file)
        {
            var fileNoExt = Path.GetFileNameWithoutExtension(file.Name);
            var tempFile = new FileInfo(Path.Combine(Path.GetDirectoryName(file.FullName), string.Format("{0}_{1}.dll", fileNoExt, Guid.NewGuid().ToString().Replace("-", ""))));

            file.CopyTo(tempFile.FullName, true);

            return tempFile;
        }

        public void Dispose()
        {
            foreach (var file in assemblyTempFiles)
            {
                file.Delete();
            }

            AppDomain.Unload(domain);
        }
    }
}
