// file:	BusinessModel.cs
//
// summary:	Implements the business model class

using System;
using System.Collections.Generic;
using System.Linq;
#if USES_CORE
using System.Linq.Dynamic.Core;
#else
using System.Linq.Dynamic;
#endif
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace Utils
{
    /// <summary>   A data Model for the business. </summary>
    ///
    /// <remarks>   Ken, 10/1/2020. </remarks>

    public class TsvEntityParserHost<TEntity> : TsvParserHost<TEntity>  where TEntity : class
    {
        /// <summary>   Gets or sets the records. </summary>
        ///
        /// <value> The records. </value>

        public List<TEntity> Records { get; private set; }

        /// <summary>   Gets or sets the top level object. </summary>
        ///
        /// <value> The top lfevel object. </value>

        public TEntity TopLevelObject { get; set; }

        /// <summary>   Parse file. </summary>
        ///
        /// <remarks>   Ken, 10/1/2020. </remarks>
        ///
        /// <param name="path">             Full pathname of the file. </param>
        /// <param name="textReplacements"> The text replacements. </param>
        ///
        /// <returns>   A List&lt;TRecord&gt; </returns>

        public override List<TEntity> ParseFile(string path, Dictionary<string, string> textReplacements = null, EntityTypeConfigRelationshipManager relationshipManager = null)
        {
            var allObjects = new List<TEntity>();

            this.Records = base.ParseFile(path, textReplacements, relationshipManager);

            if (relationshipManager != null)
            {
                relationshipManager.Entities.Add(typeof(TEntity).Name, this.Records.AsQueryable<TEntity>());
            }
            
            return Records;
        }

#if USES_CORE
        protected override object GetNonScalarValue(PropertyInfo property, string fieldValue, EntityTypeConfigRelationshipManager relationshipManager = null)
        {
            if (!property.PropertyType.IsCollectionType())
            {
                var relatedEntity = fieldValue.RegexGet(@"^(?<entity>.*?)\.", "entity");
                var predicate = fieldValue.RegexRemove(@"^.*?\.");
                var value = relationshipManager.Entities[relatedEntity].AsQueryable().Single(predicate);

                return value;
            }

            return null;
        }

        protected override bool IsRelatedKey(string fieldValue, EntityTypeConfigRelationshipManager relationshipManager)
        {
            var pattern = @"^(?<entity>.*?)\.(?<foreignkey>.*)$";

            if (Regex.IsMatch(fieldValue, pattern))
            {
                var relatedEntity = fieldValue.RegexGet(pattern, "entity");

                if (relationshipManager.Entities.ContainsKey(relatedEntity))
                {
                    return true;
                }
            }

            return false;
        }

        protected override object GetRelatedKeyValue(object record, string fieldValue, EntityTypeConfigRelationshipManager relationshipManager)
        {
            var pattern = @"^(?<entity>.*?)\.(?<foreignkey>.*)$";

            if (Regex.IsMatch(fieldValue, pattern))
            {
                var relatedEntityName = fieldValue.RegexGet(pattern, "entity");
                var keyName = fieldValue.RegexGet(pattern, "foreignkey");
                var relatedEntityProperty = record.GetType().GetProperty(relatedEntityName);

                if (relatedEntityProperty == null)
                {
                    relatedEntityProperty = record.GetType().GetProperties().SingleOrDefault(p => !p.PropertyType.IsScalar() && p.Name.Contains(relatedEntityName));
                }

                var relatedEntity = relatedEntityProperty.GetValue(record);
                var keyProperty = relatedEntity.GetType().GetProperty(keyName);
                var keyValue = keyProperty.GetValue(relatedEntity);

                relatedEntityProperty.SetValue(record, null);

                return keyValue;
            }

            return null;
        }
#endif
    }
}
