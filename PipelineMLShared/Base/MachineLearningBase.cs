﻿using System;
using System.ComponentModel;

namespace PipelineMLCore
{
    public class MachineLearningBase : IMachineLearningProcess
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        public string Name { get; set; }


        public void Configure(string rootDirectory, string jsonConfig)
        {
            throw new NotImplementedException();
        }

        public IMachineLearningResults TrainML(IDataset datasetIn)
        {
            throw new NotImplementedException();
        }
    }
}