using Newtonsoft.Json;
using System;

namespace PipelineMLCore
{
    public class ConfigBase : INamed
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ConfigBase()
        {
            Id = Guid.NewGuid();
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static T FromJSON<T>(string json)
            where T : ConfigBase
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
