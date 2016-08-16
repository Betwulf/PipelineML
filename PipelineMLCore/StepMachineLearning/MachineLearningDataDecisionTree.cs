using Accord.MachineLearning.DecisionTrees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class MachineLearningDataDecisionTree : MachineLearningData
    {
        public List<DecisionVariable> DecisionVariables { get; set; }

        public MachineLearningDataDecisionTree(IDataset datasetIn) : base(datasetIn)
        {
            DecisionVariables = new List<DecisionVariable>();
        }
    }
}
