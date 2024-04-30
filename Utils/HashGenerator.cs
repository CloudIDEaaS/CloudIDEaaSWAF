using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Reflection;
using System.Xml.Serialization;

namespace Utils
{
    public static class HashGenerator
    {
        private static readonly Object locker = new Object();

        public static String GenerateKey(Object sourceObject)
        {
            String hashString;

            //Catch unuseful parameter values
            if (sourceObject == null)
            {
                throw new ArgumentNullException("Null as parameter is not allowed");
            }
            else
            {
                //We determine if the passed object is really serializable.
                try
                {
                    //Now we begin to do the real work.
                    hashString = ComputeHash(ObjectToByteArray(sourceObject));
                    return hashString;
                }
                catch (AmbiguousMatchException ame)
                {
                    throw new ApplicationException("Could not definitely decide if object is serializable. Message:" + ame.Message);
                }
            }
        }

        public static byte[] GetMD5(this FileInfo file)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                try
                {
                    byte[] fileContents = File.ReadAllBytes(file.FullName);
                    byte[] result = md5.ComputeHash(fileContents);

                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public static byte[] GetSHA1(this FileInfo file)
        {
            byte[] fileContents = File.ReadAllBytes(file.FullName);
            byte[] result = fileContents.GetSHA1();

            return result;
        }

        public static byte[] GetSHA1(this Stream stream)
        {
            return stream.ToArray().GetSHA1();
        }

        public static byte[] GetSHA1(this byte[] fileContents)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                try
                {
                    byte[] result = sha1.ComputeHash(fileContents);

                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public static string GetSHA1HexString(this FileInfo file)
        {
            var builder = new StringBuilder();
            var result = file.GetSHA1();

            for (int i = 0; i < result.Length; i++)
            {
                builder.Append(result[i].ToString("X2"));
            }

            return builder.ToString();
        }

        public static string GetSHA1HexString(this byte[] fileContents)
        {
            var builder = new StringBuilder();
            var result = fileContents.GetSHA1();

            for (int i = 0; i < result.Length; i++)
            {
                builder.Append(result[i].ToString("X2"));
            }

            return builder.ToString();
        }

        public static string GetSHA1HexString(this Stream stream)
        {
            var builder = new StringBuilder();
            var result = stream.GetSHA1();

            for (int i = 0; i < result.Length; i++)
            {
                builder.Append(result[i].ToString("X2"));
            }

            return builder.ToString();
        }

        public static string GetMD5HexString(this FileInfo file)
        {
            var builder = new StringBuilder();
            var result = file.GetMD5();

            for (int i = 0; i < result.Length; i++)
            {
                builder.Append(result[i].ToString("X2"));
            }

            return builder.ToString();
        }

        public static int GenerateHashInt(Object sourceObject)
        {
            int hashInt;

            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                //Catch unuseful parameter values
                if (sourceObject == null)
                {
                    throw new ArgumentNullException("Null as parameter is not allowed");
                }
                else
                {
                    //We determine if the passed object is really serializable.
                    try
                    {
                        //Now we begin to do the real work.

                        var array = sourceObject.ToByteArray();
                        var hashBytes = sha1.ComputeHash(sourceObject.ToByteArray());
                        var hashInt1 = BitConverter.ToInt32(hashBytes, 0);
                        var hashInt2 = BitConverter.ToInt32(hashBytes, 4);
                        var hashInt3 = BitConverter.ToInt32(hashBytes, 8);
                        var hashInt4 = BitConverter.ToInt32(hashBytes, 12);

                        hashInt = (((hashInt1 & hashInt2) ^ hashInt3) | hashInt4);

                        return hashInt;
                    }
                    catch (AmbiguousMatchException ame)
                    {
                        throw new ApplicationException("Could not definitely decide if object is serializable. Message:" + ame.Message);
                    }
                }
            }
        }

        private static string ComputeHash(byte[] objectAsBytes)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                try
                {
                    byte[] result = md5.ComputeHash(objectAsBytes);

                    // Build the final string by converting each byte
                    // into hex and appending it to a StringBuilder
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < result.Length; i++)
                    {
                        sb.Append(result[i].ToString("X2"));
                    }

                    // And return it
                    return sb.ToString();
                }
                catch (ArgumentNullException ane)
                {
                    //If something occurred during serialization, 
                    //this method is called with a null argument. 
                    Console.WriteLine("Hash has not been generated.");
                    return null;
                }
            }
        }

        private static byte[] ObjectToByteArray(Object objectToSerialize)
        {
            MemoryStream fs = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(objectToSerialize.GetType(), "MD5Hash");
            try
            {
                //Here's the core functionality! One Line!
                //To be thread-safe we lock the object
                lock (locker)
                {
                    serializer.Serialize(fs, objectToSerialize);
                }

                return fs.ToArray();
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Error occurred during serialization. Message: " +
                se.Message);
                return null;
            }
            finally
            {
                fs.Close();
            }
        }
    }
}
