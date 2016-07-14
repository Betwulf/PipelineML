using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class RawDatasetGeneratorYahoo : IRawDatasetGenerator, ISearchableClass
    {
        public string Name { get; set; }

        public IRawDatasetDescriptor DatasetDescription { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        public string FriendlyName { get { return "Yahoo Data Generator"; } }
        public string Description { get { return "Will call and cache security information from Yahoo"; } }


        public RawDatasetGeneratorYahoo()
        {
            Config = new RawDatasetConfigYahooMarketData();
        }

        public void Configure(string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<RawDatasetConfigYahooMarketData>(jsonConfig);
            Name = Config.Name;
        }

        public override string ToString()
        {
            return Name;
        }

        public IRawDataset Generate()
        {
            return null;
        }

    }
}
