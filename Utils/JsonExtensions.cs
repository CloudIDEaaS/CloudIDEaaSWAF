using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using System.Windows.Input;
using System.Diagnostics;

namespace Utils
{
#if !UTILS_INTERNAL
    public static class JsonExtensions
#else
    internal static class JsonExtensions
#endif
    {
        public static void ChangeTo(this JToken jToken, string newValue)
        {
            var owner = jToken.Parent.Parent;
            var property = (JProperty)jToken.Parent;

            property.Remove();
            owner.Add(new JProperty(property.Name, newValue));
        }

        public static Dictionary<string, string> GetAllJsonPropertyValues(string content)
        {
            var jObject = (JObject)JsonExtensions.ReadJson(content);
            var result = new Dictionary<string, string>();
            Action<JObject> recurseProperties = null;

            recurseProperties = new Action<JObject>(j =>
            {
                foreach (var property in j.Properties())
                {
                    var value = property.Value;

                    if (value.Type == JTokenType.Object)
                    {
                        recurseProperties((JObject)value);
                    }
                    else
                    {
                        result.Add(property.Name, value.ToString());
                    }
                }
            });

            recurseProperties(jObject);

            return result;
        }


        public static List<string> GetAllJsonValues(string content)
        {
            var jObject = (JObject)JsonExtensions.ReadJson(content);
            var result = new List<string>();
            Action<JObject> recurseProperties = null;

            recurseProperties = new Action<JObject>(j =>
            {
                foreach (var property in j.Properties())
                {
                    var value = property.Value;

                    if (value.Type == JTokenType.Object)
                    {
                        recurseProperties((JObject)value);
                    }
                    else
                    {
                        result.Add(value.ToString());
                    }
                }
            });

            recurseProperties(jObject);

            return result;
        }

        public static List<string> GetAllJsonPropertyNames(string content)
        {
            var jObject = (JObject)JsonExtensions.ReadJson(content);
            var result = new List<string>();
            Action<JObject> recurseProperties = null;

            recurseProperties = new Action<JObject>(j =>
            {
                foreach (var property in j.Properties())
                {
                    var value = property.Value;
                    var name = property.Name;

                    if (value.Type == JTokenType.Object)
                    {
                        recurseProperties((JObject)value);
                    }
                    else
                    {
                        result.Add(name.ToString());
                    }
                }
            });

            recurseProperties(jObject);

            return result;
        }
        public static void WriteJson(this TextWriter writer, string text)
        {
            //text = text.Replace("'", "\"").Replace("\r\n", "");

            writer.WriteLine(text);
        }

        public static string ToJsonText(this object obj, Formatting formatting = Formatting.None, NamingStrategy namingStrategy = null)
        {
            var serializer = new JsonSerializer();
            var builder = new StringBuilder();

            serializer.Converters.Add(new KeyValuePairConverter());
            serializer.Converters.Add(new StringEnumConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            serializer.Formatting = formatting;

            if (namingStrategy != null)
            {
                serializer.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = namingStrategy
                };
            }

            using (var stringWriter = new StringWriter(builder))
            {
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    serializer.Serialize(jsonWriter, obj);
                }
            }

            return builder.ToString();
        }

        public static void WriteJson(this TextWriter writer, object obj, Formatting formatting = Formatting.None, NamingStrategy namingStrategy = null)
        {
            var serializer = new JsonSerializer();
            var builder = new StringBuilder();

            serializer.Converters.Add(new StringEnumConverter());
            serializer.Converters.Add(new KeyValuePairConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            if (formatting != Formatting.None)
            {
                serializer.Formatting = formatting;
            }

            if (namingStrategy != null)
            {
                serializer.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = namingStrategy
                };
            }

            using (var stringWriter = new StringWriter(builder))
            {
                using (var jsonWriter = new JsonTextWriter(stringWriter))
                {
                    serializer.Serialize(jsonWriter, obj);
                }
            }

            writer.WriteLine(builder.ToString());
        }

        public static T Get<T>(this KeyValuePair<string, object>[] arguments, string name)
        {
            var value = arguments.SingleOrDefault(a => a.Key == name).Value;

            if (value is JObject)
            {
                return ((JObject)arguments.SingleOrDefault(a => a.Key == name).Value).ToObject<T>();
            }
            else if (value is string)
            {
                var valueString = (string) value;

                switch (typeof(T).Name)
                {
                    case "Guid":
                        return (T)(object) Guid.Parse(valueString);
                }

                return (T) System.Convert.ChangeType((string) value, typeof(T));
            }
            else
            {
                switch (typeof(T).Name)
                {
                    case "Int16":
                        return (T)(object) System.Convert.ToInt16(value);
                }

                return (T)value;
            }
        }

        public static string Get(this KeyValuePair<string, object>[] arguments, string name)
        {
            return (string)arguments.SingleOrDefault(a => a.Key == name).Value;
        }

        public static void AddArg(this CommandPacket commandPacket, string name, object value)
        {
            if (commandPacket.Arguments == null)
            {
                commandPacket.Arguments = new KeyValuePair<string, object>[] { new KeyValuePair<string, object>(name, value) };
            }
            else
            {
                var arguments = commandPacket.Arguments;

                Array.Resize(ref arguments, arguments.Length + 1);

                arguments.SetValue(new KeyValuePair<string, object>(name, value), arguments.Length - 1);

                commandPacket.Arguments = arguments;
            }
        }

        public static T Convert<T>(object obj)
        {
            var json = obj.ToJsonText();

            return ReadJson<T>(json);
        }

        public static T LoadJson<T>(string jsonPath, NamingStrategy namingStrategy = null, IJsonTransactionLog transactionLog = null)
        {
            using (var reader = File.OpenText(jsonPath))
            {
                return reader.ReadJson<T>(namingStrategy);
            }
        }

        public static void SaveJson(string jsonPath, object obj, Formatting formatting = Formatting.None, NamingStrategy namingStrategy = null, IJsonTransactionLog transactionLog = null)
        {
            if (transactionLog != null)
            {
                using (transactionLog.CreateTransaction(jsonPath, JsonExtensions.ToJsonText(obj)))
                using (var stream = File.OpenWrite(jsonPath))
                using (var writer = new StreamWriter(stream))
                {
                    stream.SetLength(0);

                    writer.WriteJson((object)obj, formatting, namingStrategy);

                    writer.Flush();
                }
            }
            else
            {
                using (var stream = File.OpenWrite(jsonPath))
                using (var writer = new StreamWriter(stream))
                {
                    stream.SetLength(0);

                    writer.WriteJson((object)obj, formatting, namingStrategy);

                    writer.Flush();
                }
            }
        }

        public static T ReadJson<T>(this TextReader reader, NamingStrategy namingStrategy = null)
        {
            var json = string.Empty;
            var text = reader.ReadToEnd();

            return ReadJson<T>(text, namingStrategy);
        }

        public static T ReadJson<T>(string json, NamingStrategy namingStrategy = null)
        {
            var settings = new JsonSerializerSettings();

            settings.Converters.Add(new StringEnumConverter());
            settings.Converters.Add(new KeyValuePairConverter());
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;

            if (namingStrategy != null)
            {
                settings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = namingStrategy
                };
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(json, settings);
            }
            catch
            {
                return JsonConvert.DeserializeObject<T>(json.RemoveBOM(), settings);
            }
        }

        public static string ToJsonDynamic(this object obj, bool prettyPrint = true)
        {
            //////var jsonObject = (DynamicJsonConverter.DynamicJsonObject)obj;

            //////return jsonObject.ToString();
            ///

            throw new NotImplementedException();
        }

        public static void WriteJsonCommand(this TextWriter writer, CommandPacket commandPacket, Action<string> textWriteCallback = null)
        {
            if (commandPacket.Arguments == null)
            {
                commandPacket.Arguments = new KeyValuePair<string, object>[0];
            }

            writer.WriteJsonCommand(commandPacket, Environment.NewLine, textWriteCallback);
        }

        public static void WriteJsonCommand<T>(this TextWriter writer, CommandPacket<T> commandPacket, Action<string> textWriteCallback = null)
        {
            writer.WriteJsonCommand(commandPacket, Environment.NewLine, textWriteCallback);
        }

        public static void WriteJsonCommand(this TextWriter writer, CommandPacket commandPacket, string lineTerminator, Action<string> textWriteCallback = null)
        {
            var json = commandPacket.ToJsonText();

            if (textWriteCallback != null)
            {
                textWriteCallback(json);
            }

            writer.WriteJson(json);

            if (!lineTerminator.IsNullOrEmpty())
            {
                writer.WriteLine(lineTerminator);
            }

            writer.Flush();
        }

        public static void WriteJsonCommand<T>(this TextWriter writer, CommandPacket<T> commandPacket, string lineTerminator, Action<string> textWriteCallback = null)
        {
            var json = commandPacket.ToJsonText();

            if (textWriteCallback != null)
            {
                textWriteCallback(json);
            }

            writer.WriteJson(json);

            if (!lineTerminator.IsNullOrEmpty())
            {
                writer.WriteLine(lineTerminator);
            }

            writer.Flush();
        }

        public static CommandPacket ReadJsonCommand(this TextReader reader, Action<string> textReadCallback = null)
        {
            var jsonText = reader.ReadUntil(Environment.NewLine.Repeat(2), true);

            if (textReadCallback != null)
            {
                textReadCallback(jsonText);
            }

            return ReadJson<CommandPacket>(jsonText);
        }

        public static CommandPacket<T> ReadJsonCommand<T>(this TextReader reader, Action<string> textReadCallback = null)
        {
            var jsonText = reader.ReadUntil(Environment.NewLine.Repeat(2), true);

            if (textReadCallback != null)
            {
                textReadCallback(jsonText);
            }

            return ReadJson<CommandPacket<T>>(jsonText);
        }
        public static bool IsValidJson(string json)
        {
            return IsValidJson(json, out _);
        }

        public static bool IsValidJson(string json, out Exception exception)
        {
            json = json.Trim();

            exception = null;

            try
            {
                if (json.StartsWith("{") && json.EndsWith("}"))
                {
                    JToken.Parse(json);
                }
                else if (json.StartsWith("[") && json.EndsWith("]"))
                {
                    JArray.Parse(json);
                }
                else
                {
                    exception = new JsonException("JSON does not have matching braces or brackets");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }
        }

        public static Exception GetJsonExceptions(string json)
        {
            json = json.Trim();

            try
            {
                if (json.StartsWith("{") && json.EndsWith("}"))
                {
                    JToken.Parse(json);
                }
                else if (json.StartsWith("[") && json.EndsWith("]"))
                {
                    JArray.Parse(json);
                }
                else
                {
                    return new FormatException("json does not start with a { or [");
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static T ReadJson<T>(this TextReader reader, JsonSerializerSettings settings)
        {
            var json = string.Empty;
            var text = reader.ReadToEnd();

            return ReadJson<T>(text, settings);
        }

        public static T ReadJson<T>(string json, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public static object ReadJson(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }

        public static object JsonSelect(this object obj, string tokenPath)
        {
            var jObject = JObject.FromObject(obj);
            var token = jObject.SelectToken(tokenPath);
            string json;

            if (token != null)
            {
                json = token.ToString();

                if (IsValidJson(json))
                {
                    return ReadJson<object>(json);
                }
                else
                {
                    return token;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
