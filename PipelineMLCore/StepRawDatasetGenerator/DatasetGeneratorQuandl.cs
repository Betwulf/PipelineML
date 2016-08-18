using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace PipelineMLCore
{
    public class DatasetGeneratorQuandl : IDatasetGenerator, ISearchableClass
    {
        public string Name { get; set; }

        public DatasetDescriptorBase DatasetDescription { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        public string FriendlyName { get { return "Quandl Data Generator"; } }
        public string Description { get { return "Will call and cache security information from Quandl"; } }


        public DatasetGeneratorQuandl()
        {
            Config = new DatasetConfigQuandlMarketData();
        }

        public void Configure(string rootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DatasetConfigQuandlMarketData>(jsonConfig);
            Name = Config.Name;
        }

        public override string ToString()
        {
            return Name;
        }

        public DatasetBase Generate(Action<string> updateMessage)
        {
            return null;
        }

    }
}
