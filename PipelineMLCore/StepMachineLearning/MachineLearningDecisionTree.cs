using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using AForge;
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
            public int LabelValueCount { get; set; }
            public double[][] inputs { get; set; }
            public int[] labels { get; set; }
            public List<DecisionVariable> DecisionVariables { get; set; }
        }

        public string FriendlyName { get { return "Decision Tree"; } }

        public string Description { get { return "Uses a simple decision Tree algorithm"; } }


        private MachineLearningConfigDecisionTree ConfigInternal { get { return Config as MachineLearningConfigDecisionTree; } }


        public MachineLearningDecisionTree()
        {
            Config = new MachineLearningConfigDecisionTree();
        }

        public new void Configure(string rootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<MachineLearningConfigDecisionTree>(jsonConfig);
            Name = Config.Name;
        }




        public new IMachineLearningResults TrainML(DatasetBase datasetIn)
        {
            DateTime startTime = DateTime.Now;
            DatasetML mlData = PrepareDataset(datasetIn);
            TrainingData trainingData = GetTrainingData(mlData);
            // define columns for decision tree

            // Create the discrete Decision tree
            var tree = new DecisionTree(trainingData.DecisionVariables, trainingData.LabelValueCount);

            // Create the C4.5 learning algorithm
            C45Learning c45 = new C45Learning(tree);

            // Learn the decision tree using C4.5
            double TrainingError = c45.Run(trainingData.inputs, trainingData.labels);
            Console.WriteLine($"TrainingError: {TrainingError}");

            // Now that we have trained our decision tree, let's score it
            MachineLearningResults results = ScoreTestData(mlData, tree);
            results.StartTime = startTime;
            results.TrainingError = TrainingError;
            return results;
        }



        


        protected TrainingData GetTrainingData(DatasetML mlData)
        {
            // get column that is the label
            DataColumnML labelCol = mlData.Descriptor.ColumnDescriptions.Find(x => x.IsLabel == true);

            // find the column that has the generated ml input data
            string mlColName = nameof(DataColumnML.IsMLData);

            // create training data
            var trainingData = new TrainingData();
            trainingData.LabelValueCount = labelCol.ColumnSet.Count();
            var trainingRows = mlData.Table.Select($"{nameof(DataColumnBase.IsTraining)} = true");
            int trainingRowCount = trainingRows.Count();
            trainingData.inputs = new double[trainingRowCount][];
            trainingData.labels = new int[trainingRowCount];
            for (int i = 0; i < trainingRowCount; i++)
            {
                // Map label to ml data
                trainingData.labels[i] = labelCol.ColumnMap[trainingRows[i][labelCol.Name]];
                trainingData.inputs[i] = (double[])trainingRows[i][mlColName];
            }

            // create decisionVariables
            trainingData.DecisionVariables = new List<DecisionVariable>();
            foreach (var col in mlData.Descriptor.ColumnDescriptions)
            {
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


        protected DatasetML PrepareDataset(DatasetBase datasetIn)
        {
            if (datasetIn.Descriptor.ColumnDescriptions.Find(x => x.IsTraining == true) == null)
            { throw new PipelineException("Cannot find Training rows in dataset. Please set training data before machine learning", datasetIn, this); }

            // preprocess columns
            int numberOfFeatures = 0;
            bool foundLabelColumn = false; // use to ensure there is only one label column
            DatasetML mlData = new DatasetML(new DatasetDescriptorML() { ColumnDescriptions = new List<DataColumnML>(), Name = datasetIn.Name });
            foreach (var col in datasetIn.Descriptor.ColumnDescriptions)
            {
                var newCol = new DataColumnML(col);
                mlData.Descriptor.ColumnDescriptions.Add(newCol);

                if (newCol.IsFeature || newCol.IsLabel)
                {
                    newCol.ColumnSet = (from row in datasetIn.Table.AsEnumerable()
                                        select row[col.Name]).Distinct();
                    newCol.MinRange = 0.0;
                    if ((col.DataType != typeof(double)) || (col.IsCategory))
                    {
                        newCol.MaxRange = newCol.ColumnSet.Count();
                    }
                    else
                    {
                        // the column is a double and max range is what it is, no mapping needed
                        // the double multiplier is to give the tree room for unexpected higher double values in non-training data
                        newCol.MaxRange = ((double)newCol.ColumnSet.Max()) * 2.0;
                    }

                    // generate a map of existing values to numerical categories
                    newCol.ColumnMap = new Dictionary<object, int>();
                    for (int i = 0; i < newCol.ColumnSet.Count(); i++)
                    {
                        newCol.ColumnMap.Add(newCol.ColumnSet.ElementAt(i), i);
                    }
                    if (col.IsLabel)
                    {
                        if (foundLabelColumn == true)
                            throw new PipelineException($"Column {col.Name} : Dataset cannot have more than one label column", datasetIn, this);
                        if (col.IsFeature)
                            throw new PipelineException($"Column {col.Name} cannot be both a feature and a label column", datasetIn, this);
                        if (newCol.MaxRange > 10)
                            throw new PipelineException($"Trying to predict {newCol.MaxRange} number of possible outcomes is too many. Reduce the distinct values of your label column {col.Name}.", datasetIn, this);
                        foundLabelColumn = true;
                    }
                    else if (col.IsFeature)
                    { numberOfFeatures++; }
                }
            }
            mlData.NumberOfFeatures = numberOfFeatures;
            // Add a column to store the ml data for the entire row
            var MLDataColumn = new DataColumnML() { Name = nameof(DataColumnML.IsMLData), Description = nameof(DataColumnML.IsMLData), DataType = typeof(double[]), IsMLData = true };
            mlData.Descriptor.ColumnDescriptions.Add(MLDataColumn);
            datasetIn.Table.Columns.Add(MLDataColumn.Name, MLDataColumn.DataType);

            // filter only the columns that are features or labels to be sent to the ml algorithm
            foreach (DataRow row in datasetIn.Table.Rows)
            {
                row[MLDataColumn.Name] = new double[numberOfFeatures];
                int colnum = 0;
                foreach (var col in mlData.Descriptor.ColumnDescriptions)
                {
                    // check if we have to categorize the input for ml
                    if (col.IsLabel || !col.IsFeature)
                    {
                        // Don't increment colnum, only features are input columns
                    }
                    else if ((col.DataType != typeof(double)) || (col.IsCategory))
                    {
                        // if it isn't already a double, convert it
                        ((double[])row[MLDataColumn.Name])[colnum] = col.ColumnMap[row[col.Name]];
                        colnum++;
                    }
                    else
                    {
                        // Then the data is a double already and is not indicated to be a category, so take it in as is.
                        ((double[])row[MLDataColumn.Name])[colnum] = (double)row[col.Name];
                        colnum++;
                    }
                }
            }
            mlData.Table = datasetIn.Table;
            return mlData;
        }

        


        protected MachineLearningResults ScoreTestData(DatasetML mlData, DecisionTree tree)
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
                if (ConfigInternal.IncludeTrainingDataInTestingData || (bool)row[trainingColName])
                {
                    int score = tree.Compute((double[])row[mlColName]);
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
                throw new PipelineException($"found zero scores.", results.DatasetWithScores, this);

            results.Error = scoreCounter;
            results.Error = (results.Error - correct) / results.Error;
            results.FromMLProcess = this;
            results.StopTime = DateTime.Now;
            return results;
        }
    }
}
