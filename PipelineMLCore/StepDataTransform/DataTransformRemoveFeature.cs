using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using Ninject;

namespace PipelineMLCore
{
    public class DataTransformRemoveFeature : IDataTransform, ISearchableClass
    {
        public string Name { get { return Config.Name; } }

        public string FriendlyName { get { return "Remove Feature Flag Data Transform"; } }

        public string Description { get { return "Will remove feature flag from selected columns, bypassing machine learning"; } }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        private DataTransformConfigColumns ConfigInternal { get { return Config as DataTransformConfigColumns; } }


        public DataTransformRemoveFeature()
        {
            Config = new DataTransformConfigColumns();
        }

        public void Configure(IKernel kernel, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DataTransformConfigColumns>(jsonConfig);
        }

        public DatasetBase Transform(DatasetBase datasetIn, Action<string> updateMessage)
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
