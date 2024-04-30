using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class ConfigurationExtensions
    {
        public static JToken ToJson(this IConfiguration config)
        {
            var obj = new JObject();

            foreach (var child in config.GetChildren())
            {
                obj.Add(child.Key, ToJson(child));
            }

            if (!obj.HasValues && config is IConfigurationSection section)
            {
                return new JValue(section.Value);
            }

            return obj;
        }

        public static TOption Get<TOption>(this IConfiguration configuration, string name) where TOption : class, new()
        {
            var settings = configuration.GetSection(name);
            var obj = new TOption();

            settings.GetChildren().ForEach(c =>
            {
                var property = obj.GetPublicProperties().SingleOrDefault(p => p.Name == c.Key);

                if (property != null)
                {
                    if (property.PropertyType.Implements<IList>())
                    {
                        var list = obj.GetPropertyValue<IList>(property.Name);

                        foreach (var section in c.GetChildren())
                        {
                            list.Add(section.Value);
                        }
                    }
                    else
                    {
                        if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                        {
                            var dateTime = DateTime.Parse(c.Value);

                            obj.SetPropertyValue(property.Name, dateTime);
                        }
                        else if (property.PropertyType == typeof(bool))
                        {
                            obj.SetPropertyValue(property.Name, bool.Parse(c.Value.ToLower()));
                        }
                        else if (property.PropertyType == typeof(int))
                        {
                            obj.SetPropertyValue(property.Name, int.Parse(c.Value));
                        }
                        else if (property.PropertyType == typeof(SecureString))
                        {
                            obj.SetPropertyValue(property.Name, c.Value.ToSecureString());
                        }
                        else
                        {
                            obj.SetPropertyValue(property.Name, c.Value);
                        }
                    }
                }
            });

            return obj;
        }
    }
}
