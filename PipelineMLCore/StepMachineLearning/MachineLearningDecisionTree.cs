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


        protected MachineLearningDataDecisionTree PrepareDataset(IDataset datasetIn)
        {
            MachineLearningDataDecisionTree mlData = new MachineLearningDataDecisionTree(datasetIn);
            if (datasetIn.Descriptor.ColumnDescriptions.Find(x => x.IsTraining == true) == null)
            {
                throw new PipelineException("Cannot find Training rows in dataset. Please set training data before machine learning", datasetIn, this);
            }
            var trainingRows = datasetIn.Table.Select($"{nameof(DataColumnBase.IsTraining)} = true");
            var testingRows = datasetIn.Table.Select($"{nameof(DataColumnBase.IsTraining)} = false");
            DataTable trainingData = new DataTable();
            DataTable testingData = new DataTable();
            int labelCount = 0;
            foreach (var col in datasetIn.Descriptor.ColumnDescriptions)
            {
                // if it is not a feature or a label, ignore it
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
                    mlData.TrainingLabel = new int[trainingRows.Length];
                    mlData.TestingLabel = new int[testingRows.Length];
                    for (int i = 0; i < trainingRows.Length; i++)
                    {
                        mlData.TrainingLabel[i] = labelMap[trainingRows[i][col.Name]];
                    }
                    for (int i = 0; i < testingRows.Length; i++)
                    {
                        mlData.TestingLabel[i] = labelMap[testingRows[i][col.Name]];
                    }
                }
                else if (col.IsFeature)
                {
                    trainingData.Columns.Add(col.Name, typeof(double));
                    testingData.Columns.Add(col.Name, typeof(double));
                    if (trainingData.Rows.Count == 0) // initialize rows if no data exists yet
                    {
                        foreach (var row in trainingRows) { trainingData.Rows.Add(); }
                        foreach (var row in testingRows) { testingData.Rows.Add(); }
                    }
                    // deal with column conversion to double
                    if ((col.DataType != typeof(double)) || (col.IsCategory))
                    {
                        // if it isn't already a double, convert it
                        var columnSet = (from row in datasetIn.Table.AsEnumerable()
                                         select row[col.Name]).Distinct();
                        var columnRange = columnSet.Count();
                        // generate a map of existing values to numerical categories
                        var columnMap = new Dictionary<object, int>();
                        for (int i = 0; i < columnRange; i++)
                        {
                            columnMap.Add(columnSet.ElementAt(i), i);
                        }
                        // replace existing data with normalized categorical data
                        for (int i = 0; i < trainingRows.Length; i++)
                        {
                            trainingData.Rows[i][col.Name] = columnMap[trainingRows[i][col.Name]];
                        }
                        for (int i = 0; i < testingRows.Length; i++)
                        {
                            testingData.Rows[i][col.Name] = columnMap[testingRows[i][col.Name]];
                        }
                        mlData.DecisionVariables.Add(new DecisionVariable(col.Name, new IntRange() { Min = 0, Max = columnRange }));
                    }
                    else
                    {
                        double maxDouble = 0;
                        // we have a column that is a double already, leave it as is
                        for (int i = 0; i < trainingRows.Length; i++)
                        {
                            trainingData.Rows[i][col.Name] = trainingRows[i][col.Name];
                            maxDouble = maxDouble < (double)trainingData.Rows[i][col.Name] ? (double)trainingData.Rows[i][col.Name] : maxDouble;
                        }
                        for (int i = 0; i < testingRows.Length; i++)
                        {
                            testingData.Rows[i][col.Name] = testingRows[i][col.Name];
                            maxDouble = maxDouble < (double)testingData.Rows[i][col.Name] ? (double)testingData.Rows[i][col.Name] : maxDouble;
                        }
                        mlData.DecisionVariables.Add(new DecisionVariable(col.Name, new DoubleRange() { Min = 0, Max = maxDouble*2.0 })); 
                        // the double multiplier is to give the tree room for unexpected higher double values in non-training data
                    }
                }
            }
            // generate inputs 
            var trainingDataEnumerable = trainingData.AsEnumerable();
            mlData.TrainingInputs = trainingDataEnumerable.Select(x => x.ItemArray.OfType<double>().ToArray()).ToArray();
            var testingDataEnumerable = testingData.AsEnumerable();
            mlData.TestingInputs = testingDataEnumerable.Select(x => x.ItemArray.OfType<double>().ToArray()).ToArray();
            return mlData;
        }


        public new IMachineLearningResults TrainML(IDataset datasetIn)
        {
            DateTime startTime = DateTime.Now;
            var data = PrepareDataset(datasetIn);
            // define columns for decision tree

            // Create the discrete Decision tree
            var tree = new DecisionTree(data.DecisionVariables, data.LabelCategoryCount);


            // Create the C4.5 learning algorithm
            C45Learning c45 = new C45Learning(tree);

            // Learn the decision tree using C4.5
            double TrainingError = c45.Run(data.TrainingInputs, data.TrainingLabel);
            Console.WriteLine($"TrainingError: {TrainingError}");

            // Now that we have trained our decision tree, let's score it
            var results = ScoreTestData(data, tree);
            results.StartTime = startTime;
            results.TrainingError = TrainingError;
            return results;
        }

        protected MachineLearningResults ScoreTestData(MachineLearningDataDecisionTree data, DecisionTree tree)
        {
            var results = new MachineLearningResults();
            int[] scores = new int[data.TestingInputs.Length];
            for (int i = 0; i < data.TestingInputs.Length; i++)
            {
                scores[i] = tree.Compute(data.TestingInputs[i]);
                if (scores[i] == -1)
                {
                    Console.WriteLine("WTF"); 
                }
            }
            var scoredData = data.Source.Table.Clone();
            scoredData.Columns.Add(nameof(DataColumnBase.IsScore), typeof(int));
            scoredData.Columns.Add(nameof(DataColumnBase.IsScoreProbability), typeof(int));
            data.Source.Descriptor.ColumnDescriptions.Add(new DataColumnBase() { Name = nameof(DataColumnBase.IsScore), DataType = typeof(int), IsScore = true });
            data.Source.Descriptor.ColumnDescriptions.Add(new DataColumnBase() { Name = nameof(DataColumnBase.IsScoreProbability), DataType = typeof(int), IsScoreProbability = true });
            int scoreCounter = 0;
            int rowCounter = 0;
            int correct = 0;
            foreach (DataRow row in data.Source.Table.Rows)
            {
                scoredData.ImportRow(row);
                if ((bool)row[nameof(DataColumnBase.IsTraining)] == false)
                {
                    scoredData.Rows[rowCounter][nameof(DataColumnBase.IsScore)] = scores[scoreCounter];
                    string labelColumnName = data.Source.Descriptor.ColumnDescriptions.First(x => x.IsLabel).Name;
                    if (scores[scoreCounter]== (int)scoredData.Rows[rowCounter][labelColumnName])
                    {
                        correct++;
                    }
                    scoreCounter++;
                }
                rowCounter++;
            }
            if (scoreCounter == 0)
                throw new PipelineException($"found zero scores.", data.Source, this);
            results.Error = scoreCounter;
            results.Error = (results.Error - correct) / results.Error;
            results.DatasetWithScores = new DatasetScored(data.Source.Descriptor);
            results.DatasetWithScores.Table = scoredData;
            results.FromMLProcess = this;
            results.StopTime = DateTime.Now;
            return results;
        }
    }
}
