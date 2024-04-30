using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    internal static class IOExtensions
    {
        public static void CreateIfNotExists(this DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                directory.Create();
            }
        }

        public static byte[] ReadUntil(this Stream stream, string terminator, bool excludeTerminator = false, int limit = -1)
        {
            var terminatorBytes = ASCIIEncoding.ASCII.GetBytes(terminator);

            return stream.ReadUntil(terminatorBytes, excludeTerminator);
        }

        public static byte[] ReadUntil(this Stream stream, byte[] terminator, bool excludeTerminator = false, int limit = -1)
        {
            var data = new byte[0];
            var terminatorIndex = 0;
            var counter = 0;

            while (true)
            {
                var size = 1;
                var buffer = new byte[size];

                stream.Read(buffer, 0, size);

                data = data.Append(buffer);

                counter++;

                if (counter == limit)
                {
                    return data;
                }

                if (buffer[0] == terminator[terminatorIndex])
                {
                    terminatorIndex++;

                    if (terminatorIndex == terminator.Length)
                    {
                        if (excludeTerminator)
                        {
                            data = data.TrimRight(terminator.Length);
                        }

                        return data;
                    }
                }
                else
                {
                    terminatorIndex = 0;
                }
            }
        }

        public static byte[] ReadUntil(this BinaryReader reader, string terminator, bool excludeTerminator = false, int limit = -1)
        {
            var terminatorBytes = ASCIIEncoding.ASCII.GetBytes(terminator);

            return reader.ReadUntil(terminatorBytes, excludeTerminator);
        }

        public static byte[] ReadUntil(this BinaryReader reader, byte[] terminator, bool excludeTerminator = false, int limit = -1)
        {
            var data = new byte[0];
            var terminatorIndex = 0;
            var counter = 0;

            while (true)
            {
                var size = 1;
                var buffer = new byte[size];

                reader.Read(buffer, 0, size);

                data = data.Append(buffer);

                counter++;

                if (counter == limit)
                {
                    return data;
                }

                if (buffer[0] == terminator[terminatorIndex])
                {
                    terminatorIndex++;

                    if (terminatorIndex == terminator.Length)
                    {
                        if (excludeTerminator)
                        {
                            data = data.TrimRight(terminator.Length);
                        }

                        return data;
                    }
                }
                else
                {
                    terminatorIndex = 0;
                }
            }
        }

        public static string ReadUntil(this TextReader reader, string terminator, bool excludeTerminator = false, int limit = -1)
        {
            var terminatorBytes = ASCIIEncoding.ASCII.GetBytes(terminator);

            return reader.ReadUntil(terminatorBytes, excludeTerminator);
        }

        public static byte[] ToArray(this string text)
        {
            var bytes = System.Text.UTF8Encoding.UTF8.GetBytes(text);

            return bytes;
        }

        public static string ReadUntil(this TextReader reader, byte[] terminator, bool excludeTerminator = false, int limit = -1)
        {
            var data = new StringBuilder();
            var terminatorIndex = 0;
            var counter = 0;

            while (true)
            {
                var size = 1;
                var buffer = new char[size];

                reader.Read(buffer, 0, size);

                data = data.Append(buffer);

                counter++;

                if (counter == limit)
                {
                    return data.ToString();
                }

                if (buffer[0] == terminator[terminatorIndex])
                {
                    terminatorIndex++;

                    if (terminatorIndex == terminator.Length)
                    {
                        if (excludeTerminator)
                        {
                            data.RemoveEnd(terminator.Length);
                        }

                        return data.ToString();
                    }
                }
                else
                {
                    terminatorIndex = 0;
                }
            }
        }
    }
}
