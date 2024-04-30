using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using Microsoft.IdentityModel.Tokens;
using TidyManaged;
using System.Security.Permissions;
using System.IO;
using CERTENROLLLib;
using EncodingType = CERTENROLLLib.EncodingType;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Security;
using System.Linq.Dynamic;

namespace Utils
{
    public static class SecurityExtensions
    {
        [DllImport("dllmain.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SecureZeroMem(IntPtr ptr, uint cnt);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct CREDUI_INFO
        {
            public int cbSize;
            public IntPtr hwndParent;
            public string pszMessageText;
            public string pszCaptionText;
            public IntPtr hbmBanner;
        }

        [DllImport("credui.dll", CharSet = CharSet.Auto)]
        private static extern bool CredUnPackAuthenticationBuffer(int dwFlags, IntPtr pAuthBuffer, uint cbAuthBuffer, StringBuilder pszUserName, ref int pcchMaxUserName, StringBuilder pszDomainName, ref int pcchMaxDomainame, StringBuilder pszPassword, ref int pcchMaxPassword);
        [DllImport("credui.dll", CharSet = CharSet.Auto)]
        private static extern int CredUIPromptForWindowsCredentials(ref CREDUI_INFO notUsedHere, int authError, ref uint authPackage, IntPtr InAuthBuffer,uint InAuthBufferSize, out IntPtr refOutAuthBuffer, out uint refOutAuthBufferSize, ref bool fSave, int flags);

        public static CsrDetailsWithPrivateKey AddPrivateKey(this CsrDetails csrDetails, SecureString privateKey)
        {
            var csrDetailsWithPrivateKey = new CsrDetailsWithPrivateKey();

            csrDetails.CopyTo(csrDetailsWithPrivateKey);

            csrDetailsWithPrivateKey.PrivateKey = privateKey;

            return csrDetailsWithPrivateKey;
        }

        public static CsrDetailsWithUserId AddUserId(this CsrDetails csrDetails, string userId)
        {
            var csrDetailsWithUserId = new CsrDetailsWithUserId();

            csrDetails.CopyTo(csrDetailsWithUserId);

            csrDetailsWithUserId.UserId = userId;

            return csrDetailsWithUserId;
        }

        public static X509Certificate2 CreateSelfSignedCertificate(this CsrDetails csrDetails)
        {
            CX509PrivateKey privateKey;

            return csrDetails.CreateSelfSignedCertificate(out privateKey);
        }
        public static X509Certificate2 CreateSelfSignedCertificate(this CsrDetails csrDetails, out CX509PrivateKey privateKey)
        {
            return csrDetails.CreateSelfSignedCertificate(2048, out privateKey);
        }

        public static X509Certificate2 CreateSelfSignedCertificate(this CsrDetails csrDetails, int keySize, out CX509PrivateKey privateKey)
        {
            return csrDetails.CreateSelfSignedCertificate(null, (int)keySize, out privateKey);
        }

        public static X509Certificate2 CreateSelfSignedCertificate(this CsrDetails csrDetails, SecureString privateKeyString, out CX509PrivateKey privateKey)
        {
            return csrDetails.CreateSelfSignedCertificate(privateKeyString, 2048, out privateKey);
        }

        public static bool RemoveSelfSignedCertificate(this CsrDetails csrDetails)
        {
            X509Certificate2Collection certCollection;

            using (var certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                var distinguishedName = string.Format("CN={0}, O={1}, OU={2}, L={3}, S={4}, C={5}, E={6}", csrDetails.CommonName, csrDetails.Organization, csrDetails.OrganizationUnit, csrDetails.Locality, csrDetails.StateOrProvince, csrDetails.Country, csrDetails.Email);

                certStore.Open(OpenFlags.ReadWrite);
                certCollection = certStore.Certificates.Find(X509FindType.FindByIssuerDistinguishedName, distinguishedName, false);

                if (certCollection.Count > 0)
                {
                    foreach (var cert in certCollection)
                    {
                        certStore.Remove(cert);
                    }

                    certStore.Close();
                    return true;
                }

                certStore.Close();
            }

            return false;
        }

        public static X509Certificate2 FindExistingSelfSignedCertificate(this CsrDetails csrDetails)
        {
            X509Certificate2Collection certCollection;

            using (var certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                var distinguishedName = string.Format("CN={0}, O={1}, OU={2}, L={3}, S={4}, C={5}, E={6}", csrDetails.CommonName, csrDetails.Organization, csrDetails.OrganizationUnit, csrDetails.Locality, csrDetails.StateOrProvince, csrDetails.Country, csrDetails.Email);

                certStore.Open(OpenFlags.ReadOnly);
                certCollection = certStore.Certificates.Find(X509FindType.FindByIssuerDistinguishedName, distinguishedName, false);
                certStore.Close();

                if (certCollection.Count == 1)
                {
                    return certCollection[0];
                }
            }

            return null;
        }

        public static X509Certificate2 CreateSelfSignedCertificate(this CsrDetails csrDetails, SecureString privateKeyString, int keySize, out CX509PrivateKey privateKey)
        {
            // create a new private key for the certificate
            privateKey = new CX509PrivateKey();
            CX509CertificateRequestCertificate certRequest;
            CX509Enrollment enroll;
            string csr;
            string base64encoded;
            string decodedName;

            privateKey.ProviderName = "Microsoft RSA SChannel Cryptographic Provider";
            privateKey.Description = csrDetails.Description;
            privateKey.MachineContext = true;
            privateKey.Length = keySize;
            privateKey.SecurityDescriptor = "D:PAI(A;;0xd01f01ff;;;SY)(A;;0xd01f01ff;;;BA)(A;;0x80120089;;;NS)";
            privateKey.KeySpec = X509KeySpec.XCN_AT_KEYEXCHANGE; // use is not limited
            privateKey.ExportPolicy = X509PrivateKeyExportFlags.XCN_NCRYPT_ALLOW_EXPORT_FLAG;

            if (privateKeyString != null)
            {
                certRequest = new CX509CertificateRequestCertificate();

                privateKey.Import("PRIVATEBLOB", privateKeyString.Unsecure());
            }
            else
            {
                privateKey.Create();
            }

            // Use the stronger SHA512 hashing algorithm
            var hashobj = new CObjectId();
            hashobj.InitializeFromAlgorithmName(ObjectIdGroupId.XCN_CRYPT_HASH_ALG_OID_GROUP_ID, ObjectIdPublicKeyFlags.XCN_CRYPT_OID_INFO_PUBKEY_ANY, AlgorithmFlags.AlgorithmFlagsNone, "SHA512");

            // add extended key usage if you want - look at MSDN for a list of possible OIDs
            var oid = new CObjectId();
            oid.InitializeFromValue("1.3.6.1.5.5.7.3.1"); // SSL server

            var oidlist = new CObjectIds
            {
                oid
            };
            
            var eku = new CX509ExtensionEnhancedKeyUsage();
            eku.InitializeEncode(oidlist);

            var alternativeNames = new CAlternativeNames();
            var san = new CX509ExtensionAlternativeNames();

            foreach (var name in csrDetails.AlternativeNames)
            {
                var rfc822Name = new CAlternativeName();

                rfc822Name.InitializeFromString(AlternativeNameType.XCN_CERT_ALT_NAME_RFC822_NAME, name);

                alternativeNames.Add(rfc822Name);
            }

            san.InitializeEncode(alternativeNames);

            var dn = new CX500DistinguishedName();
            // The following characters are not allowed in any of the CSR fields:
            // [! @ # $ % ^ * ( ) ~ ? > < & / \\ , . ' ']
            // 

            decodedName = string.Format("CN={0}, O={1}, OU={2}, L={3}, S={4}, C={5}, E={6}", csrDetails.CommonName, csrDetails.Organization, csrDetails.OrganizationUnit, csrDetails.Locality, csrDetails.StateOrProvince, csrDetails.Country, csrDetails.Email);

            dn.Encode(decodedName, X500NameFlags.XCN_CERT_NAME_STR_NONE);

            // Create the self signing request
            certRequest = new CX509CertificateRequestCertificate();
            certRequest.InitializeFromPrivateKey(X509CertificateEnrollmentContext.ContextMachine, privateKey, "");
            certRequest.Subject = dn;
            certRequest.Issuer = dn; // the issuer and the subject are the same

            if (csrDetails.NotBefore.HasValue)
            {
                certRequest.NotBefore = csrDetails.NotBefore.Value.ToUniversalTime();
            }
            else
            {
                certRequest.NotBefore = DateTime.UtcNow;
            }

            if (csrDetails.NotAfter.HasValue)
            {
                certRequest.NotAfter = csrDetails.NotAfter.Value.ToUniversalTime();
            }
            else
            {
                certRequest.NotAfter = DateTime.UtcNow.AddYears(10);
            }

            certRequest.X509Extensions.Add((CX509Extension)eku); 
            certRequest.X509Extensions.Add((CX509Extension)san);
            certRequest.HashAlgorithm = hashobj; // Specify the hashing algorithm

            if (csrDetails is CsrDetailsWithUserId csrDetailsWithUserId)
            {
                certRequest.SerialNumber = csrDetailsWithUserId.UserId;
                certRequest.Encode(); // encode the certificate

                // Do the final enrollment process
                enroll = new CX509Enrollment();

                enroll.InitializeFromRequest(certRequest); // load the certificate

                enroll.CertificateFriendlyName = csrDetailsWithUserId.UserId;
            }
            else
            {
                certRequest.SerialNumber = csrDetails.SerialNumber;
                certRequest.Encode(); // encode the certificate

                // Do the final enrollment process
                enroll = new CX509Enrollment();

                enroll.InitializeFromRequest(certRequest); // load the certificate

                enroll.CertificateFriendlyName = csrDetails.CommonName;
            }

            csr = enroll.CreateRequest(EncodingType.XCN_CRYPT_STRING_BASE64HEADER); // Output the request in base64
                                                 // and install it back as the response
            enroll.InstallResponse(InstallResponseRestrictionFlags.AllowUntrustedCertificate, csr, EncodingType.XCN_CRYPT_STRING_BASE64HEADER, ""); // no password
            base64encoded = enroll.CreatePFX("", PFXExportOptions.PFXExportChainWithRoot);

            // instantiate the target class with the PKCS#12 data (and the empty password)
            return new X509Certificate2(Convert.FromBase64String(base64encoded), "", X509KeyStorageFlags.Exportable);
        }

        public static byte[] RSAEncrypt(this RSAParameters RSAKeyInfo, byte[] DataToEncrypt, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {

                    //Import the RSA Key information. This only needs
                    //to include the public key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Encrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                }
                return encryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public static byte[] RSADecrypt(this RSAParameters RSAKeyInfo, byte[] DataToDecrypt, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                //Create a new instance of RSACryptoServiceProvider.
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    //Import the RSA Key information. This needs
                    //to include the private key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Decrypt the passed byte array and specify OAEP padding.  
                    //OAEP padding is only available on Microsoft Windows XP or
                    //later.  
                    decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                }
                return decryptedData;
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());

                return null;
            }
        }

        public static bool IsUserAdministrator()
        {
            //bool value to hold our return value
            bool isAdmin;
            WindowsIdentity user = null;
            try
            {
                //get the currently logged in user
                user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }
            catch (Exception ex)
            {
                isAdmin = false;
            }
            finally
            {
                if (user != null)
                    user.Dispose();
            }

            return isAdmin;
        }

        public static byte[] GetSha1(this string input)
        {
            var shaHash = SHA1.Create();

            return shaHash.ComputeHash(Encoding.UTF8.GetBytes(input));
        }

        public static string GetSha1Hash(this string input)
        {
            var shaHash = SHA1.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = shaHash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static string GetSha256Hash(this string input)
        {
            var shaHash = SHA256.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = shaHash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static byte[] Pfx2Snk(byte[] pfxData, string pfxPassword)
        {
            // load .pfx
            var cert = new X509Certificate2(pfxData, pfxPassword, X509KeyStorageFlags.Exportable);

            // create .snk
            var privateKey = (RSACryptoServiceProvider)cert.PrivateKey;

            return privateKey.ExportCspBlob(true);
        }

        public static byte[] ToSnk(this X509Certificate2 cert)
        {
            var privateKey = (RSACryptoServiceProvider)cert.PrivateKey;

            return privateKey.ExportCspBlob(true);
        }

        public static string Protect(this string str, string additionalEntropy)
        {
            var bytes = ProtectedData.Protect(ASCIIEncoding.ASCII.GetBytes(str), ASCIIEncoding.ASCII.GetBytes(additionalEntropy), DataProtectionScope.CurrentUser);

            return bytes.ToBase64();
        }

        public static string Unprotect(this string str, string additionalEntropy)
        {
            var bytes = ProtectedData.Unprotect(str.FromBase64(), ASCIIEncoding.ASCII.GetBytes(additionalEntropy), DataProtectionScope.CurrentUser);

            return ASCIIEncoding.ASCII.GetString(bytes);
        }

        public static NetworkCredential GetCredentials(string serverName, string message)
        {
            NetworkCredential networkCredential = null;
            var credUIInfo = new CREDUI_INFO();
            var outCredBuffer = new IntPtr();
            var usernameBuilder = new StringBuilder(100);
            var passwordBuilder = new StringBuilder(100);
            var domainBuilder = new StringBuilder(100);
            var maxUserName = 100;
            var maxDomain = 100;
            var maxPassword = 100;
            int result;
            uint authPackage = 0;
            uint outCredSize;
            bool save = false;

            credUIInfo.pszCaptionText = "Please enter the credentails for " + serverName;
            credUIInfo.pszMessageText = message;
            credUIInfo.cbSize = Marshal.SizeOf(credUIInfo);

            result = CredUIPromptForWindowsCredentials(ref credUIInfo, 0, ref authPackage, IntPtr.Zero, 0, out outCredBuffer, out outCredSize, ref save, 0x00002);

            try
            {
                if (result == 0)
                {
                    try
                    {
                        if (CredUnPackAuthenticationBuffer(0, outCredBuffer, outCredSize, usernameBuilder, ref maxUserName, domainBuilder, ref maxDomain, passwordBuilder, ref maxPassword))
                        {
                            networkCredential = new NetworkCredential(usernameBuilder.ToString(), passwordBuilder.ToString(), domainBuilder.ToString());
                        }
                    }
                    finally
                    {
                        passwordBuilder.Clear();
                    }
                }
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(outCredBuffer);
            }

            return networkCredential;
        }
    }
}
