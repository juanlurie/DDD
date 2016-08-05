using System;

namespace Iris.Serialization
{
    public interface ISerializeObjects
    {
        T DeserializeObject<T>(string value);
        object DeserializeObject(string value, Type type);
        object DeserializeObject<T>(string value, Type type);
        string SerializeObject(object value);
        string GetContentType();
    }
}