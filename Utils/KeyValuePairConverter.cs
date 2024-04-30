using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if !NETCOREAPP3_1
//////using System.Web.Script.Serialization;
#endif

namespace Utils
{
    public class KeyValuePairConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(KeyValuePair<string, object>[]) || objectType == typeof(KeyValuePair<string, string>[]))
            {
                return true;
            }

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var array = (JArray)serializer.Deserialize<object>(reader);
            var keyValuePairs = array.Children().SelectMany(o => o.Children()).Cast<JProperty>().Select(p => new KeyValuePair<string, object>(p.Name, p.Value.ToObject<object>()));

            return keyValuePairs.ToArray();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var keyValuePairs = (KeyValuePair<string, object>[])value;
            var array = new JArray(keyValuePairs.Select(p =>
            {
                var obj = new JObject();

                obj.Add(p.Key, p.Value == null ? null : JToken.FromObject(p.Value));

                return obj;
            }));

            serializer.Serialize(writer, array);
        }
    }

#if !NETCOREAPP3_1
    //////public class KeyValuePairConverterJavascript : JavaScriptConverter
    //////{
    //////    public override IEnumerable<Type> SupportedTypes
    //////    {
    //////        get
    //////        {
    //////            return new List<Type> { typeof(KeyValuePair<string, object>), typeof(string[][]) };
    //////        }
    //////    }

    //////    public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
    //////    {
    //////        var keyValuePairs = (KeyValuePair<string, object>)obj;
    //////        var dictionary = new Dictionary<string, object>();

    //////        dictionary.Add(keyValuePairs.Key, keyValuePairs.Value);

    //////        return dictionary;
    //////    }

    //////    public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
    //////    {
    //////        return dictionary.SingleOrDefault();
    //////    }
    //////}
#endif
}
