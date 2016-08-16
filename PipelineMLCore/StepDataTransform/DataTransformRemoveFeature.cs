using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;

namespace PipelineMLCore
{
    public class DataTransformRemoveFeature : IDataTransform, ISearchableClass
    {
        public string Name { get; set; }

        public string FriendlyName { get { return "Remove Feature Flag Data Transform"; } }

        public string Description { get { return "Will remove feature flag from selected columns, bypassing machine learning"; } }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        private DataTransformConfigColumns ConfigInternal { get { return Config as DataTransformConfigColumns; } }


        public DataTransformRemoveFeature()
        {
            Config = new DataTransformConfigColumns();
        }

        public void Configure(string rootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DataTransformConfigColumns>(jsonConfig);
            Name = Config.Name;
        }

        public IDataset Transform(IDataset datasetIn, Action<string> updateMessage)
        {
            foreach (var col in ConfigInternal.ColumnNames)
            {
                var found = datasetIn.Descriptor.ColumnDescriptions.First(x => x.Name == col.Name);
                found.IsLabel = false;
                found.IsFeature = false;
                found.IsCategory = false;
            }
            return datasetIn;
        }
    }
}
