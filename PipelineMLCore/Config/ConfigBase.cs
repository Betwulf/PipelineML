using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class ConfigBase : IConfig
    {
        public string Name { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static ConfigBase FromJSON(string json, Type t)
        {
            return JsonConvert.DeserializeObject(json, t) as ConfigBase;
        }
    }
}
