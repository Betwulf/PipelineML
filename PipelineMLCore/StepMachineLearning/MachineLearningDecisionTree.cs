using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord;
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
        protected struct TrainingData
        {

            public List<DecisionVariable> DecisionVariables { get; set; }
        }

        public string FriendlyName { get { return "Decision Tree"; } }

        public string Description { get { return "Uses a simple decision Tree algorithm with a c45 learning algorithm"; } }


        private MachineLearningConfigDecisionTree ConfigInternal { get { return Config as MachineLearningConfigDecisionTree; } }


        public MachineLearningDecisionTree()
        {
            Config = new MachineLearningConfigDecisionTree();
        }

        public new void Configure(string rootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<MachineLearningConfigDecisionTree>(jsonConfig);
        }




        public new IMachineLearningResults TrainML(DatasetBase datasetIn, Action<string> updateMessage)
        {
            DateTime startTime = DateTime.Now;
            DatasetML mlData = PrepareDataset(datasetIn, updateMessage);
            TrainingData trainingData = GetTrainingData(mlData, updateMessage);
            // define columns for decision tree

            // Create the discrete Decision tree
            var tree = new DecisionTree(trainingData.DecisionVariables, trainingData.LabelValueCount);

            // Create the C4.5 learning algorithm
            C45Learning c45 = new C45Learning(tree);

            // Learn the decision tree using C4.5
            double TrainingError = c45.Run(trainingData.inputs, trainingData.labels);
            updateMessage($"TrainingError: {TrainingError}"); // or throw?

            // Now that we have trained our decision tree, let's score it
            MachineLearningResults results = ScoreTestData(mlData, tree, updateMessage);
            results.StartTime = startTime;
            results.TrainingError = TrainingError;
            return results;
        }



        


        protected TrainingData GetTrainingData(DatasetML mlData, Action<string> updateMessage)
        {
            // get column that is the label
            DataColumnML labelCol = mlData.Descriptor.ColumnDescriptions.Find(x => x.IsLabel == true);

            // create training data
            var trainingData = new TrainingData();
            var trainingRows = mlData.Table.Select($"{nameof(DataColumnBase.IsTraining)} = true");
            int trainingRowCount = trainingRows.Count();

            // create decisionVariables
            trainingData.DecisionVariables = new List<DecisionVariable>();
            bool foundLabelColumn = false;
            foreach (var col in mlData.Descriptor.ColumnDescriptions)
            {
                if (foundLabelColumn == true)
                    throw new PipelineException($"Column {col.Name} : Dataset cannot have more than one label column", mlData, this, updateMessage);
                if (col.IsLabel) foundLabelColumn = true;
                if (col.IsFeature && (col.DataType != typeof(double) || col.IsCategory))
                {
                    trainingData.DecisionVariables.Add(new DecisionVariable(col.Name, new IntRange() { Min = (int)col.MinRange, Max = (int)col.MaxRange }));
                }
                else if (col.IsFeature)
                {
                    trainingData.DecisionVariables.Add(new DecisionVariable(col.Name, new DoubleRange() { Min = (int)col.MinRange, Max = (int)col.MaxRange }));
                }
            }
            return trainingData;
        }



        protected MachineLearningResults ScoreTestData(DatasetML mlData, DecisionTree tree, Action<string> updateMessage)
        {
            var results = new MachineLearningResults();
            results.DatasetWithScores = new DatasetScored(mlData);

            // get column that is the label
            DataColumnML labelCol = results.DatasetWithScores.Descriptor.ColumnDescriptions.Find(x => x.IsLabel == true);
            // find the column that has the generated ml input data
            string mlColName = nameof(DataColumnML.IsMLData);
            string scoreColName = nameof(DataColumnBase.IsScore);
            string trainingColName = nameof(DataColumnBase.IsTraining);
            string scoreProbabilityName = nameof(DataColumnBase.IsScoreProbability);
            int correct = 0;
            int scoreCounter = 0;
            foreach (DataRow row in results.DatasetWithScores.Table.Rows)
            {
                if (ConfigInternal.IncludeTrainingDataInTestingData || !(bool)row[trainingColName])
                {
                    int score = tree.Decide((double[])row[mlColName]);
                    row[scoreColName] = score;
                    row[scoreProbabilityName] = 1.0; // decisiontree gives no probability? :(

                    if (score == -1) { Console.WriteLine("WTF"); }
                    if ((int)row[labelCol.Name] == score)
                    {
                        correct++;
                    }
                    scoreCounter++;
                }
            }
            if (scoreCounter == 0)
                throw new PipelineException($"found zero scores.", results.DatasetWithScores, this, updateMessage);

            results.Error = scoreCounter;
            results.Error = (results.Error - correct) / results.Error;
            results.FromMLProcess = this;
            results.StopTime = DateTime.Now;
            return results;
        }
    }
}
