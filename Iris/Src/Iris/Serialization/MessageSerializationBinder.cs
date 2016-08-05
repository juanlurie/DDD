﻿using System;
using System.Runtime.Serialization;
using Iris.Reflection;

namespace Iris.Serialization
{
    public class MessageSerializationBinder : SerializationBinder
    {
        private readonly ITypeMapper typeMapper;

        public MessageSerializationBinder(ITypeMapper typeMapper)
        {
            this.typeMapper = typeMapper;
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            var mappedType = typeMapper.GetMappedTypeFor(serializedType) ?? serializedType;

            assemblyName = null;
            typeName = mappedType.AssemblyQualifiedName;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            throw new NotImplementedException();
        }
    }
}