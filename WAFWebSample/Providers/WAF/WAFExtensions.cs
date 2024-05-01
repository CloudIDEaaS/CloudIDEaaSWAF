using WAFWebSample.WebApi.Providers.WAF.Models;
using Utils;
using WebSecurity.Models;
using WebSecurity;

namespace WAFWebSample.WebApi.Providers.WAF
{
    public static class WAFExtensions
    {
        public static T? GetValue<T>(this IStorageBase storageBase, string indexOrRegex)
        {
            var type = storageBase.GetType().GetInterfaces().Where(i => i.GetInterfaces().Any(t => t == typeof(IStorageBase))).Single();
            var property = type.GetProperty(indexOrRegex);

            if (property == null)
            {
                property = type.GetProperties().SingleOrDefault(p => p.HasCustomAttribute<OwaspNameAttribute>() && p.GetCustomAttribute<OwaspNameAttribute>().Name == indexOrRegex);
            }

            if (property == null)
            {
                DebugUtils.Break();
            }

            property = type.GetProperty(property.Name);

            if (property == null)
            {
                DebugUtils.Break();
            }

            return (T?) property.GetValue(storageBase);
        }

        public static void SetValue<T>(this IStorageBase storageBase, string indexOrRegex, T? value)
        {
            var type = storageBase.GetType().GetInterfaces().Where(i => i.GetInterfaces().Any(t => t == typeof(IStorageBase))).Single();
            var property = type.GetProperty(indexOrRegex);

            if (property == null)
            {
                property = type.GetProperties().SingleOrDefault(p => p.HasCustomAttribute<OwaspNameAttribute>() && p.GetCustomAttribute<OwaspNameAttribute>().Name == indexOrRegex);
            }

            if (property == null)
            {
                DebugUtils.Break();
            }

            property = type.GetProperty(property.Name);

            if (property == null)
            {
                DebugUtils.Break();
            }

            if (property.PropertyType == typeof(ICollection<string>))
            {
                var typedValue = value.ToString().SplitNullToEmpty(" ").ToList();

                property.SetValue(storageBase, typedValue);
            }
            else if (property.PropertyType != typeof(string))
            {
                var propertyType = property.PropertyType.GetAnyInnerType();
                object? typedValue;

                if (propertyType == typeof(bool) && value.IsOneOf("1", "0"))
                {
                    typedValue = value.ToString() == "1";
                }
                else
                {
                    typedValue = Convert.ChangeType(value, propertyType);
                }

                property.SetValue(storageBase, typedValue);
            }
            else
            {
                property.SetValue(storageBase, value);
            }

            storageBase.Save();
        }
    }
}
