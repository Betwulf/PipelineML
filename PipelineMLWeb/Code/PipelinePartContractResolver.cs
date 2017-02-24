using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PipelineMLWeb
{
    public class PipelinePartContractResolver : DefaultContractResolver
    {

        public PipelinePartContractResolver()
        {
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);

            // only serializer properties that are not Id
            properties = properties.Where(p => p.PropertyName != "Id").ToList();

            return properties;
        }
    }
}