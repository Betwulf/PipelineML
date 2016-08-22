﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DataTransformSetLabel : IDataTransform, ISearchableClass
    {
        public string Name { get; set; }

        public string FriendlyName { get { return "Set Label Data Transform"; } }

        public string Description { get { return "Designates the column that will be used to train the output of the machine learning algorithm"; } }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        private DataTransformConfigColumns ConfigInternal { get { return Config as DataTransformConfigColumns; } }


        public DataTransformSetLabel()
        {
            Config = new DataTransformConfigColumns();
        }

        public void Configure(string rootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DataTransformConfigColumns>(jsonConfig);
            Name = Config.Name;
        }

        public DatasetBase Transform(DatasetBase datasetIn, Action<string> updateMessage)
        {
            if (ConfigInternal.ColumnNames.Count > 1)
                throw new PipelineException("Don't set more than one label column please", datasetIn, this);
            foreach (var col in ConfigInternal.ColumnNames)
            {
                var found = datasetIn.Descriptor.ColumnDescriptions.First(x => x.Name == col.Name);
                found.IsLabel = true;
                found.IsFeature = false;
            }
            return datasetIn;

        }
    }
}