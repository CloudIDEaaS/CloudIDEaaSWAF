// file:	IO\JsonExtension.cs
//
// summary:	Implements the JSON extension class

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NamedPipeWrapper.IO
{
    /// <summary>   A JSON extensions. </summary>
    ///
    /// <remarks>   CloudIDEaaS, 2/9/2021. </remarks>

    public static class JsonExtensions
    {
        /// <summary>   A TextWriter extension method that writes a JSON. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/9/2021. </remarks>
        ///
        /// <param name="writer">   The writer to act on. </param>
        /// <param name="text">     The text. </param>

        public static void WriteJson(this TextWriter writer, string text)
        {
            //text = text.Replace("'", "\"").Replace("\r\n", "");

            writer.WriteLine(text);
        }

        /// <summary>   An object extension method that converts this  to a JSON text. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/9/2021. </remarks>
        ///
        /// <param name="obj">          The obj to act on. </param>
        /// <param name="formatting">   (Optional) The formatting. </param>
        ///
        /// <returns>   The given data converted to a string. </returns>

        public static string ToJsonText(this object obj, Formatting formatting = Formatting.None)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Formatting = formatting
            };

            settings.Converters.Add(new KeyValuePairConverter());

            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>   A TextWriter extension method that writes a JSON. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/9/2021. </remarks>
        ///
        /// <param name="writer">           The writer to act on. </param>
        /// <param name="obj">              The obj to act on. </param>
        /// <param name="formatting">       (Optional) The formatting. </param>
        /// <param name="namingStrategy">   (Optional) The naming strategy. </param>

        public static void WriteJson(this TextWriter writer, dynamic obj, Formatting formatting = Formatting.None, NamingStrategy namingStrategy = null)
        {
            var serializer = new JsonSerializer();
            var builder = new StringBuilder();

            serializer.Converters.Add(new KeyValuePairConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;

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

        /// <summary>   Reads a JSON. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/9/2021. </remarks>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="reader">   The reader to act on. </param>
        ///
        /// <returns>   The JSON. </returns>

        public static T ReadJson<T>(this TextReader reader)
        {
            var json = string.Empty;
            var text = reader.ReadToEnd();

            return ReadJson<T>(text);
        }

        /// <summary>   Reads a JSON. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/9/2021. </remarks>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="json"> The JSON. </param>
        ///
        /// <returns>   The JSON. </returns>

        public static T ReadJson<T>(string json)
        {
            var settings = new JsonSerializerSettings();

            settings.Converters.Add(new KeyValuePairConverter());
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;

            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>   Query if 'json' is valid JSON. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/9/2021. </remarks>
        ///
        /// <param name="json"> The JSON. </param>
        ///
        /// <returns>   True if valid json, false if not. </returns>

        public static bool IsValidJson(string json)
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
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>   Reads a JSON. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/9/2021. </remarks>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="reader">   The reader to act on. </param>
        /// <param name="settings"> Options for controlling the operation. </param>
        ///
        /// <returns>   The JSON. </returns>

        public static T ReadJson<T>(this TextReader reader, JsonSerializerSettings settings)
        {
            var json = string.Empty;
            var text = reader.ReadToEnd();

            return ReadJson<T>(text, settings);
        }

        /// <summary>   Reads a JSON. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/9/2021. </remarks>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="json">     The JSON. </param>
        /// <param name="settings"> Options for controlling the operation. </param>
        ///
        /// <returns>   The JSON. </returns>

        public static T ReadJson<T>(string json, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>   Reads a JSON. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/9/2021. </remarks>
        ///
        /// <param name="json"> The JSON. </param>
        ///
        /// <returns>   The JSON. </returns>

        public static object ReadJson(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }

        /// <summary>   An object extension method that JSON select. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/9/2021. </remarks>
        ///
        /// <param name="obj">          The obj to act on. </param>
        /// <param name="tokenPath">    Full pathname of the token file. </param>
        ///
        /// <returns>   An object. </returns>

        public static object JsonSelect(this object obj, string tokenPath)
        {
            var jObject = JObject.FromObject(obj);
            var token = jObject.SelectToken(tokenPath);
            string json;

            if (token != null)
            {
                json = token.ToString();

                return ReadJson<object>(json);
            }
            else
            {
                return null;
            }
        }
    }
}