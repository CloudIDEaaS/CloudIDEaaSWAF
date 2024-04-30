using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;

namespace Utils
{
    public static class InterprocessVariables
    {
        public class Variable
        {
            public string Name;
            public string Value;
        }

        public static void SetVariable(string domain, string variableName, string value)
        {
            var path = Path.GetTempPath();

            SetVariable(path, domain, variableName, value);
        }

        public static string GetVariable(string domain, string variableName)
        {
            var path = Path.GetTempPath();

            return GetVariable(path, domain, variableName);
        }

        public static void SetVariable(string path, string domain, string variableName, string value)
        {
            string json;
            int variableSize;
            Mutex mutex;
            bool mutexCreated;
            var directory = new DirectoryInfo(Path.Combine(path, domain));
            var file = new FileInfo(Path.Combine(directory.FullName, "variable.tmp"));
            var variable = new Variable
            {
                Name = variableName,
                Value = value
            };

            json = JsonExtensions.ToJsonText(variable);
            variableSize = json.Length;

            mutex = new Mutex(false, string.Format(@"Global\{0}+Mutex", domain), out mutexCreated);
            mutex.WaitOne();

            if (!directory.Exists)
            {
                directory.Create();
            }
            else if (file.Exists)
            {
                file.Delete();
            }

            using (var mappedFile = MemoryMappedFile.CreateFromFile(file.FullName, FileMode.OpenOrCreate, domain, variableSize))
            {
                using (var stream = mappedFile.CreateViewStream(0, variableSize))
                {
                    var bytes = ASCIIEncoding.ASCII.GetBytes(json);

                    stream.Write(bytes, 0, bytes.Length);
                }
            }

            mutex.ReleaseMutex();
        }

        public static string GetVariable(string path, string domain, string variableName)
        {
            string value;

            try
            {
                var mutex = Mutex.OpenExisting(string.Format(@"Global\{0}+Mutex", domain));
                var directory = new DirectoryInfo(Path.Combine(path, domain));
                var file = Path.Combine(directory.FullName, "variable.tmp");

                if (mutex.WaitOne(10000))
                {
                    try
                    {
                        using (var mappedFile = MemoryMappedFile.CreateFromFile(file, FileMode.OpenOrCreate, domain, 0))
                        {
                            using (var accessor = mappedFile.CreateViewAccessor(0, 0))
                            {
                                Variable variable;
                                byte b;
                                var builder = new StringBuilder();
                                var x = 0;

                                while (!JsonExtensions.IsValidJson(builder.ToString()))
                                {
                                    b = accessor.ReadByte(x);
                                    builder.Append(Convert.ToChar(b));

                                    x++;
                                }

                                variable = JsonExtensions.ReadJson<Variable>(builder.ToString());
                                Debug.Assert(variable.Name == variableName);

                                value = variable.Value;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        value = null;
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
                else
                {
                    value = null;
                }
            }
            catch (Exception ex)
            {
                value = null;
            }
            finally
            {
            }

            return value;
        }

        public static void SetVariable(FileInfo file, string domain, string variableName, string value)
        {
            string json;
            int variableSize;
            Mutex mutex;
            bool mutexCreated;
            var variable = new Variable
            {
                Name = variableName,
                Value = value
            };

            json = JsonExtensions.ToJsonText(variable);
            variableSize = json.Length;

            mutex = new Mutex(false, string.Format(@"Global\{0}+Mutex", domain), out mutexCreated);
            mutex.WaitOne();

            if (file.Exists)
            {
                file.Delete();
            }

            using (var mappedFile = MemoryMappedFile.CreateFromFile(file.FullName, FileMode.OpenOrCreate, domain, variableSize))
            {
                using (var stream = mappedFile.CreateViewStream(0, variableSize))
                {
                    var bytes = ASCIIEncoding.ASCII.GetBytes(json);

                    stream.Write(bytes, 0, bytes.Length);
                }
            }

            mutex.ReleaseMutex();
        }

        public static string GetVariable(FileInfo file, string domain, string variableName)
        {
            string value;

            try
            {
                var mutex = Mutex.OpenExisting(string.Format(@"Global\{0}+Mutex", domain));

                if (mutex.WaitOne(10000))
                {
                    try
                    {
                        using (var mappedFile = MemoryMappedFile.CreateFromFile(file.FullName, FileMode.OpenOrCreate, domain, 0))
                        {
                            using (var accessor = mappedFile.CreateViewAccessor(0, 0))
                            {
                                Variable variable;
                                byte b;
                                var builder = new StringBuilder();
                                var x = 0;

                                while (!JsonExtensions.IsValidJson(builder.ToString()))
                                {
                                    b = accessor.ReadByte(x);
                                    builder.Append(Convert.ToChar(b));

                                    x++;
                                }

                                variable = JsonExtensions.ReadJson<Variable>(builder.ToString());
                                Debug.Assert(variable.Name == variableName);

                                value = variable.Value;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        value = null;
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
                else
                {
                    value = null;
                }
            }
            catch (Exception ex)
            {
                value = null;
            }
            finally
            {
            }

            return value;
        }

        public static string OpenAndGetVariable(FileInfo file, string domain, string variableName)
        {
            string value;
            bool mutexCreated;

            try
            {
                var mutex = new Mutex(false, string.Format(@"Global\{0}+Mutex", domain), out mutexCreated);
                mutex.WaitOne();

                try
                {
                    using (var mappedFile = MemoryMappedFile.CreateFromFile(file.FullName, FileMode.OpenOrCreate, domain, 0))
                    {
                        using (var accessor = mappedFile.CreateViewAccessor(0, 0))
                        {
                            Variable variable;
                            byte b;
                            var builder = new StringBuilder();
                            var x = 0;

                            while (!JsonExtensions.IsValidJson(builder.ToString()))
                            {
                                b = accessor.ReadByte(x);
                                builder.Append(Convert.ToChar(b));

                                x++;
                            }

                            variable = JsonExtensions.ReadJson<Variable>(builder.ToString());
                            Debug.Assert(variable.Name == variableName);

                            value = variable.Value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    value = null;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
            catch (Exception ex)
            {
                value = null;
            }
            finally
            {
            }

            return value;
        }
    }
}
