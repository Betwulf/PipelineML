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
    public class MachineLearningDecisionTree : MachineLearningBase, ISearchableClass, IMachineLearningProcess
    {
        // Nested Class to handle Decision Tree Variables
        protected struct TrainingDecisionVariables
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

        public void Configure(string rootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<MachineLearningConfigDecisionTree>(jsonConfig);
        }




        public IMachineLearningResults TrainML(DatasetBase datasetIn, Action<string> updateMessage)
        {
            var results = new MachineLearningResults();
            results.StartTime = DateTime.Now;
            var internalUpdate = results.GetLoggedUpdateMessage(updateMessage);
            DatasetML mlData = PrepareDataset(datasetIn, internalUpdate, this);
            TrainingDecisionVariables trainingData = GetDecisionTreeTrainingData(mlData, internalUpdate);
            // define columns for decision tree
            int LabelValueCount = (from col in mlData.Descriptor.ColumnDescriptions where col.IsLabel == true select col.ColumnSet).FirstOrDefault().Count();



            // Create the discrete Decision tree
            var tree = new DecisionTree(trainingData.DecisionVariables, LabelValueCount);

            // Create the C4.5 learning algorithm
            C45Learning c45 = new C45Learning(tree);

            // Learn the decision tree using C4.5
            int[] outputs = mlData.labels.Select(x => (int)x[0]).ToArray();
            double TrainingError = c45.Run(mlData.inputs, outputs);
            internalUpdate($"Decision Tree Trained. TrainingError: {TrainingError}");

            // Now that we have trained our decision tree, let's score it
            results = ScoreTestData(mlData, tree, results, internalUpdate);
            results.TrainingError = TrainingError;
            return results;
        }



        


        protected TrainingDecisionVariables GetDecisionTreeTrainingData(DatasetML mlData, Action<string> updateMessage)
        {
            if (mlData.NumberOfLabels != 1)
            {
                throw new PipelineException($"Need exactly 1 label, not {mlData.NumberOfLabels} label(s).", mlData, this, updateMessage);
            }

            // get column that is the label
            DataColumnML labelCol = mlData.Descriptor.ColumnDescriptions.Find(x => x.IsLabel == true);
            if (labelCol.DataType != typeof(int))
            {
                throw new PipelineException($"Label for a decision Tree must be an int, not {labelCol.DataType} .", mlData, this, updateMessage);
            }


            // create training data
            var trainingData = new TrainingDecisionVariables();
            var trainingRows = mlData.Table.Select($"{nameof(DataColumnBase.IsTraining)} = true");
            int trainingRowCount = trainingRows.Count();

            // create decisionVariables
            trainingData.DecisionVariables = new List<DecisionVariable>();
            bool foundLabelColumn = false;
            foreach (var col in mlData.Descriptor.ColumnDescriptions)
            {
                if (col.IsLabel)
                {
                    if (foundLabelColumn == true)
                        throw new PipelineException($"Column {col.Name} : Dataset cannot have more than one label column", mlData, this, updateMessage);
                    foundLabelColumn = true;
                }
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



        protected MachineLearningResults ScoreTestData(DatasetML mlData, DecisionTree tree, MachineLearningResults results, Action<string> updateMessage)
        {
            results.DatasetWithScores = new DatasetScored(mlData);

            // get column that is the label
            DataColumnML labelCol = results.DatasetWithScores.Descriptor.ColumnDescriptions.Find(x => x.IsLabel == true);
            // find the column that has the generated ml input data
            string inputColName = nameof(DataColumnML.IsMLInputData);
            string labelColName = nameof(DataColumnML.IsMLLabelData);
            string scoreColName = nameof(DataColumnBase.IsScore);
            string trainingColName = nameof(DataColumnBase.IsTraining);
            string probabilityColName = nameof(DataColumnBase.IsScoreProbability);
            double sumMeanSquaredError = 0;
            double sumError = 0;
            int rowCount = 0;
            foreach (DataRow row in results.DatasetWithScores.Table.Rows)
            {
                if (ConfigInternal.IncludeTrainingDataInTestingData || !(bool)row[trainingColName])
                {
                    int score = tree.Decide((double[])row[inputColName]);
                    double[] doubleScore = new double[1];
                    doubleScore[0] = score.To<double>();
                    row[scoreColName] = doubleScore;


                    var labels = (double[])row[labelColName];
                    double[] errorVector = GetErrorVector(doubleScore, labels);
                    sumError += errorVector.Sum(x => Math.Abs(x));
                    var meanSquareError = errorVector.MeanSquareError();
                    row[probabilityColName] = 1 - errorVector.Sum(x => Math.Abs(x));
                    sumMeanSquaredError += meanSquareError;
                    rowCount++;
                }
            }
            if (rowCount == 0)
                throw new PipelineException($"found zero scores.", results.DatasetWithScores, this, results.GetLoggedUpdateMessage(updateMessage));

            results.Error = sumError / rowCount / mlData.NumberOfLabels;
            results.MeanSquareError = sumMeanSquaredError / rowCount;
            results.RootMeanSquareDeviation = Math.Sqrt(results.MeanSquareError);
            results.FromMLProcess = this;
            results.StopTime = DateTime.Now;
            return results;
        }
    }
}
