// file:	TsvParserHost.cs
//
// summary:	Implements the tsv parser host class

using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Utils;

namespace Utils
{
    /// <summary>   A Template Tsv parser host. </summary>
    ///
    /// <remarks>   Ken, 10/1/2020. </remarks>
    ///
    /// <typeparam name="TRecord">  Type of the record. </typeparam>

    public class TsvParserHost<TRecord> : ITsvParserHost<TRecord>
    {
        /// <summary>   The CSV parser. </summary>
        private TextFieldParser tsvParser;
        private Stack<object> parentItemRecords;
        private bool hasItemConverter;
        private int currentLevel;
        private object lastRecord;
        private List<TRecord> topLevelRecords;

        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Ken, 10/3/2020. </remarks>

        public TsvParserHost()
        {
            parentItemRecords = new Stack<object>();
        }

        public virtual List<TRecord> ParseFile(string path, Dictionary<string, string> textReplacements = null, EntityTypeConfigRelationshipManager relationshipManager = null)
        {
            var type = typeof(TRecord);

            topLevelRecords = new List<TRecord>();

            hasItemConverter = type.GetProperties().Any(p => p.HasCustomAttribute<TsvParserChildRecursionAttribute>());

            using (var tsvParser = new TextFieldParser(path))
            {
                tsvParser.CommentTokens = new string[] { "#" };
                tsvParser.SetDelimiters(new string[] { "\t" });
                tsvParser.HasFieldsEnclosedInQuotes = true;
                tsvParser.TrimWhiteSpace = true;

                this.tsvParser = tsvParser;

                tsvParser.ReadLine();

                while (!tsvParser.EndOfData)
                {
                    var fields = tsvParser.ReadFields().ToList();
                    var record = Activator.CreateInstance<TRecord>();

                    if (relationshipManager != null)
                    {
                        var getFirstColumnValue = relationshipManager.GetFirstColumnValue;

                        fields.Insert(0, getFirstColumnValue());
                    }

                    AddRecord(fields.ToArray(), record, topLevelRecords, textReplacements, relationshipManager: relationshipManager);
                }
            }

            return topLevelRecords;
        }

        /// <summary>   Parse file. </summary>
        ///
        /// <remarks>   Ken, 10/1/2020. </remarks>
        ///
        /// <param name="path">             Full pathname of the file. </param>
        /// <param name="textReplacements"> The text replacements. </param>
        ///
        /// <returns>   A List&lt;TRecord&gt; </returns>

        public virtual List<TRecord> ParseFile(string path, Dictionary<string, string> textReplacements, Func<string> getFirstColumnValue)
        {
            var type = typeof(TRecord);

            topLevelRecords = new List<TRecord>();

            hasItemConverter = type.GetProperties().Any(p => p.HasCustomAttribute<TsvParserChildRecursionAttribute>());

            using (var tsvParser = new TextFieldParser(path))
            {
                tsvParser.CommentTokens = new string[] { "#" };
                tsvParser.SetDelimiters(new string[] { "\t" });
                tsvParser.HasFieldsEnclosedInQuotes = true;
                tsvParser.TrimWhiteSpace = true;

                this.tsvParser = tsvParser;

                tsvParser.ReadLine();

                while (!tsvParser.EndOfData)
                {
                    var fields = tsvParser.ReadFields().ToList();
                    var record = Activator.CreateInstance<TRecord>();

                    if (getFirstColumnValue != null)
                    {
                        fields.Insert(0, getFirstColumnValue());
                    }

                    AddRecord(fields.ToArray(), record, topLevelRecords, textReplacements);
                }
            }

            return topLevelRecords;
        }

        protected virtual object GetNonScalarValue(PropertyInfo property, string fieldValue, EntityTypeConfigRelationshipManager relationshipManager = null)
        {
            throw new NotImplementedException("GetNonScalarValue was not implemented by inheriting class");
        }

        /// <summary>   Adds a record. </summary>
        ///
        /// <remarks>   Ken, 10/1/2020. </remarks>
        ///
        /// <exception cref="NotSupportedException">    Thrown when the requested operation is not
        ///                                             supported. </exception>
        ///
        /// <param name="fields">           The fields. </param>
        /// <param name="record">           The record. </param>
        /// <param name="records">          The records. </param>
        /// <param name="textReplacements"> The text replacements. </param>
        /// <param name="startColumnIndex"> (Optional) The start column index. </param>

        private void AddRecord(string[] fields, object record, IList records, Dictionary<string, string> textReplacements = null, int startColumnIndex = 0, EntityTypeConfigRelationshipManager relationshipManager = null)
        {
            var type = record.GetType();
            var properties = type.GetProperties().Where(p => !p.HasCustomAttribute<JsonIgnoreAttribute>()).OrderBy(p => p.HasCustomAttribute<JsonPropertyAttribute>() ? p.GetCustomAttribute<JsonPropertyAttribute>().Order : 0);
            var index = 0;
            var order = 0;

            foreach (var property in properties)
            {
                JsonPropertyAttribute jsonPropertyAttribute = null;
                TsvParserChildRecursionAttribute childRecursionAttribute = null;
                var hasOrder = false;

                if (property.HasCustomAttribute<JsonPropertyAttribute>())
                {
                    jsonPropertyAttribute = property.GetCustomAttribute<JsonPropertyAttribute>();
                    hasOrder = true;
                    order = jsonPropertyAttribute.Order;
                }

                if (property.HasCustomAttribute<TsvParserChildRecursionAttribute>())
                {
                    childRecursionAttribute = property.GetCustomAttribute<TsvParserChildRecursionAttribute>();
                }

                if (fields.Length > order + startColumnIndex)
                {
                    var fieldValue = fields[order + startColumnIndex];

                    if (textReplacements != null)
                    {
                        fieldValue = textReplacements.DoTextReplacements(fieldValue);
                    }

                    if (childRecursionAttribute != null)
                    {
                        var parms = childRecursionAttribute.ChildRecursionParameters.Cast<string>().ToList();
                        var itemType = childRecursionAttribute.ItemType;
                        var recursionFieldCount = childRecursionAttribute.ItemType.GetProperties().Where(p => !p.HasCustomAttribute<TsvParserChildRecursionAttribute>()).Count();

                        if (parms.Contains("--IndentRecurseColumns") && parms.Contains("--RecursionChildProperty"))
                        {
                            var childPropertyIndex = parms.IndexOf("--RecursionChildProperty") + 1;
                            var recursionChildProperty = parms[childPropertyIndex];
                            var thisLevel = fields.TakeWhile(f => f.IsNullOrEmpty()).Count();
                            object parentRecord = null;
                            IList list;

                            if (thisLevel > currentLevel)
                            {
                                parentItemRecords.Push(lastRecord); 
                                parentRecord = lastRecord;
                            }
                            else if (thisLevel < currentLevel)
                            {
                                for (var x = 0; x < currentLevel - thisLevel; x+= recursionFieldCount)
                                {
                                    if (parentItemRecords.Count > 0)
                                    {
                                        parentItemRecords.Pop();
                                    }
                                }

                                if (parentItemRecords.Count > 0)
                                {
                                    parentRecord = parentItemRecords.Peek();
                                }
                            }
                            else if (thisLevel == currentLevel && parentItemRecords.Count > 0)
                            {
                                parentRecord = parentItemRecords.Peek();
                            }

                            if (parentRecord != null)
                            {
                                var itemRecord = Activator.CreateInstance(childRecursionAttribute.ItemType);

                                if (parentRecord.GetType() == typeof(TRecord))
                                {
                                    list = (IList)property.GetValue(parentRecord);

                                    if (list == null)
                                    {
                                        list = (IList)Activator.CreateInstance(property.PropertyType);

                                        property.SetValue(parentRecord, list);
                                    }
                                }
                                else
                                {
                                    var listProperty = parentRecord.GetProperty(recursionChildProperty);

                                    list = (IList)listProperty.GetValue(parentRecord);

                                    if (list == null)
                                    {
                                        list = (IList)Activator.CreateInstance(listProperty.PropertyType);

                                        listProperty.SetValue(parentRecord, list);
                                    }
                                }

                                AddRecord(fields, itemRecord, list, textReplacements, thisLevel);

                                currentLevel = thisLevel;

                                return;
                            }

                            currentLevel = thisLevel;
                        }
                    }

                    if (!fieldValue.IsNullOrEmpty())
                    {
                        if (textReplacements != null)
                        {
                            fieldValue = textReplacements.DoTextReplacements(fieldValue);
                        }

                        if (relationshipManager != null && !property.PropertyType.IsScalar())
                        {
                            property.SetValue(record, this.GetNonScalarValue(property, fieldValue, relationshipManager));
                        }
                        else if (relationshipManager != null && this.IsRelatedKey(fieldValue, relationshipManager))
                        {
                            property.SetValue(record, this.GetRelatedKeyValue(record, fieldValue, relationshipManager));
                        }
                        else if (property.PropertyType == typeof(byte[]))
                        {
                            property.SetValue(record, fieldValue.FromBase64());
                        }
                        else
                        {
                            property.SetValue(record, TypeDescriptor.GetConverter(property.PropertyType).ConvertFromString(fieldValue));
                        }
                    }

                    index++;

                    if (!hasOrder)
                    {
                        order++;
                    }
                }
            }

            records.Add(record);
            lastRecord = record;
        }

        protected virtual bool IsRelatedKey(string fieldValue, EntityTypeConfigRelationshipManager relationshipManager)
        {
            return false;
        }

        protected virtual object GetRelatedKeyValue(object record, string fieldValue, EntityTypeConfigRelationshipManager relationshipManager)
        {
            throw new NotImplementedException("GetRelatedKeyValue was not implemented by inheriting class");
        }
    }
}