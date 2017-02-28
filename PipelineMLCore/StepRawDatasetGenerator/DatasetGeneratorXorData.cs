using Newtonsoft.Json;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;

namespace PipelineMLCore
{
    public class DatasetGeneratorXorData : IDatasetGenerator, ISearchableClass
    {
        public Guid Id { get { return Config.Id; } }

        public string Name { get { return Config.Name; } }

        public DatasetDescriptorBase DatasetDescription { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        private DatasetConfigBlank ConfigInternal { get { return Config as DatasetConfigBlank; } }

        public string FriendlyName { get { return "XOR Data"; } }

        public string Description { get { return "Simple 4 rows of XOR data, preconfigured."; } }

        public DatasetGeneratorXorData()
        {
            Config = new DatasetConfigBlank();
            Config.Name = "XOR Data";
        }

        public void Configure(IKernel kernel, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DatasetConfigBlank>(jsonConfig);
        }

        public DatasetBase Generate(Action<string> updateMessage)
        {
            return XorData.GetXorData();
        }
        
    }
}
