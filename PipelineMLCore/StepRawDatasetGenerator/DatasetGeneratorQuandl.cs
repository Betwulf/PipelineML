using Newtonsoft.Json;
using Ninject;
using System;
using System.ComponentModel;

namespace PipelineMLCore
{
    public class DatasetGeneratorQuandl : IDatasetGenerator, ISearchableClass
    {
        public Guid Id { get { return Config.Id; } }

        public string Name { get { return Config.Name; } }

        public DatasetDescriptorBase DatasetDescription { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        public string FriendlyName { get { return "Quandl Data Generator"; } }
        public string Description { get { return "Will call and cache security information from Quandl"; } }


        public DatasetGeneratorQuandl()
        {
            Config = new DatasetConfigQuandlMarketData();
        }

        public void Configure(IKernel kernel, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DatasetConfigQuandlMarketData>(jsonConfig);
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
