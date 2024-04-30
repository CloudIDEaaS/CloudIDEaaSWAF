using System;
using Polenter.Serialization.Advanced.Serializing;

namespace Polenter.Serialization.Advanced.Xml
{
    public interface IComplexValueConverter : ITypeNameConverter
    {
        object ConvertFromType(Type type);
        void Initialize(object originalObject, object convertedObject);
    }
}