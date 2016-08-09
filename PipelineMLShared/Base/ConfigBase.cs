using Newtonsoft.Json;

namespace PipelineMLCore
{
    public class ConfigBase : INamed
    {
        public string Name { get; set; }

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
