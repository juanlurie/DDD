using System;
using Iris.Reflection;
using Newtonsoft.Json.Serialization;

namespace Iris.Serialization.Json
{
    public class MessageContractResolver : DefaultContractResolver
    {
        private readonly ITypeMapper typeMapper;

        public MessageContractResolver(ITypeMapper typeMapper)
            : base(true)
        {
            this.typeMapper = typeMapper;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var mappedTypeFor = typeMapper.GetMappedTypeFor(objectType);

            if (mappedTypeFor == null)
                return base.CreateObjectContract(objectType);

            var jsonContract = base.CreateObjectContract(mappedTypeFor);
            jsonContract.DefaultCreator = () => typeMapper.CreateInstance(mappedTypeFor);

            return jsonContract;
        }
    }
}
