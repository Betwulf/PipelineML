using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class MachineLearningDecisionTree : IMachineLearningProcess, ISearchableClass
    {
        public string Name { get; set; }

        public string FriendlyName { get { return "Add Column Data Transform"; } }

        public string Description { get { return "Run custom C# code to generate a new column of data"; } }


        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        private MachineLearningConfigDecisionTree ConfigInternal { get { return Config as MachineLearningConfigDecisionTree; } }


        public MachineLearningDecisionTree()
        {
            Config = new MachineLearningConfigDecisionTree();
        }

        public void Configure(string rootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DataTransformConfigColumns>(jsonConfig);
            Name = Config.Name;
        }

        public IMachineLearningResults TrainML(IDataset datasetIn)
        {



            throw new NotImplementedException();
        }
    }
}
