// file:	ConfigObject.cs
//
// summary:	Implements the configuration object class

using Newtonsoft.Json;
using System;

namespace Utils
{
    /// <summary>   A string enum converter. </summary>
    ///
    /// <remarks>   CloudIDEaaS, 3/16/2021. </remarks>

    public class StringEnumConverter : JsonConverter
    {
        /// <summary>   Determines whether this instance can convert the specified object type. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 3/16/2021. </remarks>
        ///
        /// <param name="objectType">   Type of the object. </param>
        ///
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Enum))
            {
                return true;
            }

            return false;
        }

        /// <summary>   Reads the JSON representation of the object. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 3/16/2021. </remarks>
        ///
        /// <param name="reader">           The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from. </param>
        /// <param name="objectType">       Type of the object. </param>
        /// <param name="existingValue">    The existing value of object being read. </param>
        /// <param name="serializer">       The calling serializer. </param>
        ///
        /// <returns>   The object value. </returns>

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        /// <summary>   Writes the JSON representation of the object. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 3/16/2021. </remarks>
        ///
        /// <param name="writer">       The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to. </param>
        /// <param name="value">        The value. </param>
        /// <param name="serializer">   The calling serializer. </param>

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var enumValue = (Enum)value;
            var enumString = enumValue.ToString();

            serializer.Serialize(writer, enumString);
        }
    }
}