using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class ProtectedDataExtensions
    {
        public static byte[] CreateAppUserProtectedData(this object jsonObject)
        {
            var json = JsonExtensions.ToJsonText(jsonObject);
            var bytes = Encoding.UTF8.GetBytes(json);
            var entropyBytes = (Assembly.GetEntryAssembly().FullName + " " + Environment.UserName).GetHashData();
            var encryptedBytes = ProtectedData.Protect(bytes, entropyBytes, DataProtectionScope.LocalMachine);

            return encryptedBytes.ToZip();
        }

        public static T GetAppUserProtectedData<T>(this byte[] bytes)
        {
            var entropyBytes = (Assembly.GetEntryAssembly().FullName + " " + Environment.UserName).GetHashData();
            var decryptedBytes = ProtectedData.Unprotect(bytes, entropyBytes, DataProtectionScope.LocalMachine).FromZip();

            return JsonExtensions.ReadJson<T>(decryptedBytes);
        }
    }
}
