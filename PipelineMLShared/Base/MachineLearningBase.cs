using System;
using System.ComponentModel;

namespace PipelineMLCore
{
    public abstract class MachineLearningBase : IMachineLearningProcess
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        public string Name { get { return Config.Name; } }

        public void Configure(string rootDirectory, string jsonConfig)
        {
            throw new NotImplementedException();
        }

        public IMachineLearningResults TrainML(DatasetBase datasetIn)
        {
            throw new NotImplementedException();
        }
    }
}
