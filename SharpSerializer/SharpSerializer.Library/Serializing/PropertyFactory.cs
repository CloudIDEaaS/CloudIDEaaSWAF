#region Copyright © 2010 Pawel Idzikowski [idzikowski@sharpserializer.com]

//  ***********************************************************************
//  Project: sharpSerializer
//  Web: http://www.sharpserializer.com
//  
//  This software is provided 'as-is', without any express or implied warranty.
//  In no event will the author(s) be held liable for any damages arising from
//  the use of this software.
//  
//  Permission is granted to anyone to use this software for any purpose,
//  including commercial applications, and to alter it and redistribute it
//  freely, subject to the following restrictions:
//  
//      1. The origin of this software must not be misrepresented; you must not
//        claim that you wrote the original software. If you use this software
//        in a product, an acknowledgment in the product documentation would be
//        appreciated but is not required.
//  
//      2. Altered source versions must be plainly marked as such, and must not
//        be misrepresented as being the original software.
//  
//      3. This notice may not be removed or altered from any source distribution.
//  
//  ***********************************************************************

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Polenter.Serialization.Advanced;
using Polenter.Serialization.Core;
using Polenter.Serialization.Advanced.Xml;

namespace Polenter.Serialization.Serializing
{
    /// <summary>
    ///   Decomposes object to a Property and its Subproperties
    /// </summary>
    public sealed class PropertyFactory
    {
        private readonly object[] _emptyObjectArray = new object[0];
        private readonly PropertyProvider _propertyProvider;
        private bool _skipErrors;
        private int _depth;
        private int _currentDepth;
        private IComplexValueConverter converter;
        private bool ignoreReadOnlyProperties;

        /// <summary>
        /// Contains reference targets.
        /// </summary>
        private readonly Dictionary<object, ReferenceTargetProperty> _propertyCache =
            new Dictionary<object, ReferenceTargetProperty>();

        /// <summary>
        /// It will be incremented as neccessary
        /// </summary>
        private int _currentReferenceId = 1;

        /// <summary>
        /// </summary>
        /// <param name = "propertyProvider">provides all important properties of the decomposed object</param>
        public PropertyFactory(PropertyProvider propertyProvider, IComplexValueConverter converter, int depth = 1, bool skipErrors = false, bool ignoreReadOnlyProperties = false)
        {
            _propertyProvider = propertyProvider;
            _skipErrors = skipErrors;
            _depth = depth;
            this.ignoreReadOnlyProperties = ignoreReadOnlyProperties;

            this.converter = converter;
        }

        /// <summary>
        /// </summary>
        /// <param name = "name"></param>
        /// <param name = "value"></param>
        /// <returns>NullProperty if the value is null</returns>
        public Property CreateProperty(string name, object value)
        {
            if (value == null) return new NullProperty(name);

            // If value type is recognized, it will be taken from typeinfo cache
            TypeInfo typeInfo = TypeInfo.GetTypeInfo(value);

            if (converter != null)
            {
                var convertedType = converter.ConvertToType(typeInfo.Type.FullName);

                if (convertedType != null && convertedType.FullName != typeInfo.Type.FullName)
                {
                    var convertedObject = Tools.CreateInstance(convertedType);
                    converter.Initialize(value, convertedObject);

                    typeInfo = TypeInfo.GetTypeInfo(convertedObject);

                    value = convertedObject;
                }
            }

            // Is it simple type
            Property property = createSimpleProperty(name, typeInfo, value);
            if (property != null)
            {
                // It is simple type
                return property;
            }

            // From now it can only be an instance of ReferenceTargetProperty
            ReferenceTargetProperty referenceTarget = createReferenceTargetInstance(name, typeInfo);

            // Search in Cache
            ReferenceTargetProperty cachedTarget;
            if (_propertyCache.TryGetValue(value, out cachedTarget))
            {
                // Value was already referenced
                // Its reference will be used
                cachedTarget.Reference.Count++;
                referenceTarget.MakeFlatCopyFrom(cachedTarget);
                return referenceTarget;
            }

            // Target was not found in cache
            // it must be created

            // Adding property to cache
            referenceTarget.Reference = new ReferenceInfo();
            referenceTarget.Reference.Id = _currentReferenceId++;
            _propertyCache.Add(value, referenceTarget);

            // Parsing the property
            var handled = fillSingleDimensionalArrayProperty(referenceTarget as SingleDimensionalArrayProperty, typeInfo, value);
            handled = handled || fillMultiDimensionalArrayProperty(referenceTarget as MultiDimensionalArrayProperty, typeInfo, value);
            handled = handled || fillDictionaryProperty(referenceTarget as DictionaryProperty, typeInfo, value);
            handled = handled || fillCollectionProperty(referenceTarget as CollectionProperty, typeInfo, value);
            handled = handled || fillComplexProperty(referenceTarget as ComplexProperty, typeInfo, value);

            if (!handled)
                throw new InvalidOperationException(string.Format("Property cannot be filled. Property: {0}",
                                                                  referenceTarget));
           
            return referenceTarget;
        }

        private static ReferenceTargetProperty createReferenceTargetInstance(string name, TypeInfo typeInfo)
        {
            // Is it array?
            if (typeInfo.IsArray)
            {
                if (typeInfo.ArrayDimensionCount < 2)
                {
                    // 1D-Array
                    return new SingleDimensionalArrayProperty(name, typeInfo.Type);
                }
                // MultiD-Array
                return new MultiDimensionalArrayProperty(name, typeInfo.Type);
            }

            if (typeInfo.IsDictionary)
            {
                return new DictionaryProperty(name, typeInfo.Type);
            }
            if (typeInfo.IsCollection)
            {
                return new CollectionProperty(name, typeInfo.Type);
            }
            if (typeInfo.IsEnumerable)
            {
                // Actually it would be enough to check if the typeinfo.IsEnumerable is true...
                return new CollectionProperty(name, typeInfo.Type);
            }

            // If nothing was recognized, a complex type will be created
            return new ComplexProperty(name, typeInfo.Type);
        }

        private bool fillComplexProperty(ComplexProperty property, TypeInfo typeInfo, object value)
        {
            if (property == null)
                return false;

            // Parsing properties
            parseProperties(property, typeInfo, value);

            return true;
        }

        private void parseProperties(ComplexProperty property, TypeInfo typeInfo, object value)
        {
            if (_currentDepth < _depth)
            {
                IList<PropertyInfo> propertyInfos = _propertyProvider.GetProperties(typeInfo);

                _currentDepth++;

                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if (_skipErrors)
                    {
                        try
                        {
                            object subValue = propertyInfo.GetValue(value, _emptyObjectArray);

                            Property subProperty = CreateProperty(propertyInfo.Name, subValue);

                            property.Properties.Add(subProperty);
                        }
                        catch { }
                    }
                    else
                    {
                        object subValue = propertyInfo.GetValue(value, _emptyObjectArray);

                        Property subProperty = CreateProperty(propertyInfo.Name, subValue);

                        property.Properties.Add(subProperty);
                    }
                }

                _currentDepth--;
            }
        }

        private bool fillCollectionProperty(CollectionProperty property, TypeInfo info, object value)
        {
            if (property == null)
                return false;

            // Parsing properties
            parseProperties(property, info, value);

            // Parse Items
            parseCollectionItems(property, info, value);

            return true;
        }

        private void parseCollectionItems(CollectionProperty property, TypeInfo info, object value)
        {
            property.ElementType = info.ElementType;

            var collection = (ICollection) value;

            foreach (object item in collection)
            {
                Property itemProperty = CreateProperty(item.GetType().Name, item);

                property.Items.Add(itemProperty);
            }
        }

        private bool fillDictionaryProperty(DictionaryProperty property, TypeInfo info, object value)
        {
            if (property == null)
                return false;

            // Properties
            parseProperties(property, info, value);

            // Items
            parseDictionaryItems(property, info, value);

            return true;
        }

        private void parseDictionaryItems(DictionaryProperty property, TypeInfo info, object value)
        {
            property.KeyType = info.KeyType;
            property.ValueType = info.ElementType;

            var dictionary = (IDictionary) value;

            foreach (DictionaryEntry entry in dictionary)
            {
                Property keyProperty = CreateProperty(entry.Key.GetType().Name, entry.Key);

                Property valueProperty = CreateProperty(entry.Value.GetType().Name, entry.Value);

                property.Items.Add(new KeyValueItem(keyProperty, valueProperty));
            }
        }

        private bool fillMultiDimensionalArrayProperty(MultiDimensionalArrayProperty property, TypeInfo info, object value)
        {
            if (property == null)
                return false;
            property.ElementType = info.ElementType;

            var analyzer = new ArrayAnalyzer(value);

            // DimensionInfos
            property.DimensionInfos = analyzer.ArrayInfo.DimensionInfos;

            // Items
            foreach (var indexSet in analyzer.GetIndexes())
            {
                object subValue = ((Array) value).GetValue(indexSet);
                Property itemProperty = CreateProperty(null, subValue);

                property.Items.Add(new MultiDimensionalArrayItem(indexSet, itemProperty));
            }
            return true;
        }

        private bool fillSingleDimensionalArrayProperty(SingleDimensionalArrayProperty property, TypeInfo info, object value)
        {
            if (property == null)
                return false;

            property.ElementType = info.ElementType;

            var analyzer = new ArrayAnalyzer(value);

            // Dimensionen
            DimensionInfo dimensionInfo = analyzer.ArrayInfo.DimensionInfos[0];
            property.LowerBound = dimensionInfo.LowerBound;

            // Items
            foreach (object item in analyzer.GetValues())
            {
                Property itemProperty = CreateProperty(null, item);

                property.Items.Add(itemProperty);
            }

            return true;
        }

        private static Property createSimpleProperty(string name, TypeInfo typeInfo, object value)
        {
            if (!typeInfo.IsSimple) return null;
            var result = new SimpleProperty(name, typeInfo.Type);
            result.Value = value;
            return result;
        }
    }
}