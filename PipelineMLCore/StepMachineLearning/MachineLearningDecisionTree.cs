using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace PipelineMLCore
{
    public class MachineLearningDecisionTree : MachineLearningBase, ISearchableClass
    {
        public string FriendlyName { get { return "Decision Tree"; } }

        public string Description { get { return "Uses a simple decision Tree algorithm"; } }


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

        protected MachineLearningData PrepareDataset(IDataset datasetIn)
        {
            MachineLearningData mlData = new MachineLearningData(datasetIn);
            int labelCount = 0;
            foreach (var col in datasetIn.Descriptor.ColumnDescriptions)
            {
                // if it is not a feature or a label, ignore it
                if (col.IsFeature || col.IsLabel)
                {
                    // test to make sure only one label column exists
                    if (col.IsLabel)
                    {
                        if (++labelCount > 1)
                            throw new PipelineException($"Column {col.Name} : Dataset cannot have more than one label column", datasetIn, this);
                        if (col.IsFeature)
                            throw new PipelineException($"Column {col.Name} cannot be both a feature and a label column", datasetIn, this);

                        // Now convert label column into an array of int, and determine the number of states it can have
                        var labelSet = (from row in datasetIn.Table.AsEnumerable()
                                        select row[col.Name]).Distinct();
                        mlData.LabelCategoryCount = labelSet.Count();
                        if (mlData.LabelCategoryCount > 10)
                            throw new PipelineException($"Trying to predict {mlData.LabelCategoryCount} number of possible outcomes is too many. Reduce the distinct values of your label column {col.Name}.", datasetIn, this);

                        // Create a map to change the values of the label to a machine readable val
                        var labelMap = new Dictionary<object, int>();
                        for (int i = 0; i < mlData.LabelCategoryCount; i++)
                        {
                            labelMap.Add(labelSet.ElementAt(i), i);
                        }
                        for (int i = 0; i < datasetIn.Table.Rows.Count; i++)
                        {
                            datasetIn.Table.Rows[i][col.Name] = labelMap[datasetIn.Table.Rows[i][col.Name]];
                        }
                        

                        //mlData.Label = (from row in datasetIn.Table.AsEnumerable() select row.Field<int>(col.Name)).ToArray();
                    }



                }
            }
            // generate inputs 
            var tableEnumerable = datasetIn.Table.AsEnumerable();
            mlData.Inputs = tableEnumerable.Select(x => x.ItemArray.OfType<double>().ToArray()).ToArray();
            return mlData;
        }

        public IMachineLearningResults TrainML(IDataset datasetIn)
        {
            // Work in progress...
            /*
            // define columns for decision tree
            DecisionVariable[] variables =
           {
                new DecisionVariable(PClassColumn, 4),
                new DecisionVariable(AgeColumn, new DoubleRange() { Min = 0, Max = 120.0 } ),
                new DecisionVariable(SibSpColumn, new IntRange() { Min = 0, Max = 8 } ),
                new DecisionVariable(ParchColumn, new IntRange() { Min = 0, Max = 8 } ),
                new DecisionVariable(FareColumn, new DoubleRange() { Min = 0, Max = 1000.0 } ),
                new DecisionVariable(GenderColumn, DecisionVariableKind.Discrete),
                new DecisionVariable(EmbarkedNumberColumn, new IntRange() { Min = 0, Max = 2 } ),
            };

            // Create the discrete Decision tree
            var tree = new DecisionTree(variables, 2);

            checkArgs(inputs, outputs, tree);

            // Create the C4.5 learning algorithm
            C45Learning c45 = new C45Learning(tree);

            // Learn the decision tree using C4.5
            double error = c45.Run(inputs, outputs);
            */

            throw new NotImplementedException();
        }
    }
}
