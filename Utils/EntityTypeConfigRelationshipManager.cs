using System;
using System.Collections.Generic;
#if USES_CORE
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
#endif
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Dynamic;
using Utils;
using System.Reflection;

namespace Utils
{
    public class EntityTypeConfigRelationshipManager
    {
        public Func<string> GetFirstColumnValue { get; set; }
        public Dictionary<string, IQueryable> Entities { get; }
        public Dictionary<object, Action> Callbacks { get; }

        public EntityTypeConfigRelationshipManager(Func<string> getFirstColumnValue)
        {
            this.GetFirstColumnValue = getFirstColumnValue;
            this.Entities = new Dictionary<string, IQueryable>();
            this.Callbacks = new Dictionary<object, Action>();
        }

#if USES_CORE
        public object GetEntity(string entityName, string whereClause)
        {
            var entityList = this.Entities[entityName];
            var entity = entityList.AsQueryable().Single(whereClause);

            return entity;
        }
#endif
        public object ShadowRecord(object record)
        {
            var type = record.GetType();
            var properties = type.GetProperties();
            var allProperties = new Dictionary<string, Type>();
            var newPropertyValues = new Dictionary<string, object>();
            object newRecord = null;
            Type newType = null;
             
            foreach (var property in properties)
            {
                var value = property.GetValue(record);

                if (!property.PropertyType.IsScalar())
                {
                    var idProperty = value.GetType().GetProperties().First();
                    var id = idProperty.GetValue(value);

                    allProperties.Add(idProperty.Name, idProperty.PropertyType);
                    newPropertyValues.Add(idProperty.Name, id);
                }
                else
                {
                    allProperties.Add(property.Name, property.PropertyType);
                }
            }

#if USES_CORE
            newType = TypeShadow.CreateShadowType(type, allProperties);
#endif
            newRecord = Activator.CreateInstance(newType);

            foreach (var property in properties.Where(p => p.PropertyType.IsScalar()))
            {
                var value = property.GetValue(record);
                var newProperty = newType.GetProperty(property.Name);

                newProperty.SetValue(newRecord, value);
            }

            foreach (var propertyValue in newPropertyValues)
            {
                var name = propertyValue.Key;
                var value = propertyValue.Value;
                var newProperty = newType.GetProperty(name);

                newProperty.SetValue(newRecord, value);
            }

            return newRecord;
        }
    }
}
