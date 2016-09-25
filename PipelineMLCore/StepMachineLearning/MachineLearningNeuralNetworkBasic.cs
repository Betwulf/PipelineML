using Accord.Neuro;
using Accord.Neuro.Learning;
using AForge;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace PipelineMLCore
{
    public class MachineLearningNeuralNetworkBasic : MachineLearningBase, ISearchableClass, IMachineLearningProcess
    {

        public string FriendlyName { get { return "Basic Neural Network"; } }

        public string Description { get { return "Uses a Basic Neural Network algorithm"; } }


        private MachineLearningConfigNeuralNetworkBasic ConfigInternal { get { return Config as MachineLearningConfigNeuralNetworkBasic; } }


        public MachineLearningNeuralNetworkBasic()
        {
            Config = new MachineLearningConfigNeuralNetworkBasic();
        }

        public void Configure(string rootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<MachineLearningConfigNeuralNetworkBasic>(jsonConfig);
        }


        //             typeof(SigmoidFunction), typeof(BipolarSigmoidFunction), typeof(ThresholdFunction)


        public IMachineLearningResults TrainML(DatasetBase datasetIn, Action<string> updateMessage)
        {
            // Prepare the dataset and result set
            var results = new MachineLearningResults();
            results.StartTime = DateTime.Now;
            var internalUpdate = results.GetLoggedUpdateMessage(updateMessage);
            DatasetML mlData = PrepareDataset(datasetIn, internalUpdate, this);

            IActivationFunction activationFn = null;

            // figure out Activation Function
            if (ConfigInternal.ActivationFunction == typeof(SigmoidFunction))
                activationFn = new SigmoidFunction(ConfigInternal.ActivationFunctionAlpha);
            else if (ConfigInternal.ActivationFunction == typeof(BipolarSigmoidFunction))
                activationFn = new BipolarSigmoidFunction(ConfigInternal.ActivationFunctionAlpha);
            else if (ConfigInternal.ActivationFunction == typeof(ThresholdFunction))
                activationFn = new ThresholdFunction();
            else
                throw new PipelineException($"ActivationFunction Type unknown: {ConfigInternal.ActivationFunction.Name}", datasetIn, this, updateMessage);

            // Create Neural Network
            ActivationNetwork network = new ActivationNetwork(activationFn, mlData.NumberOfFeatures, ConfigInternal.HiddenLayerNeurons, mlData.NumberOfLabels);

            // Instance the training algo
            if (ConfigInternal.LearningAlgorithm == typeof(BackPropagationLearning))
            {
                BackPropagationLearning teacher = new BackPropagationLearning(network);
                teacher.LearningRate = ConfigInternal.LearningRate;
                teacher.Momentum = ConfigInternal.Momentum;
                double trainingError = 1.0;
                int i;
                for (i = 0; i < ConfigInternal.MaxTrainingIterations; i++)
                {
                    trainingError = teacher.RunEpoch(mlData.inputs, mlData.labels);
                    if (trainingError < ConfigInternal.TrainUntilError && i > ConfigInternal.MinTrainingIterations)
                        break;
                }
                results.TrainingIterations = i;
                results.TrainingError = trainingError;
            }
            else if (ConfigInternal.LearningAlgorithm == typeof(ResilientBackpropagationLearning))
            {
                ResilientBackpropagationLearning teacher = new ResilientBackpropagationLearning(network);
                teacher.LearningRate = ConfigInternal.LearningRate;
                double trainingError = 1.0;
                int i;
                for (i = 0; i < ConfigInternal.MaxTrainingIterations; i++)
                {
                    trainingError = teacher.RunEpoch(mlData.inputs, mlData.labels);
                    if (trainingError < ConfigInternal.TrainUntilError && i > ConfigInternal.MinTrainingIterations)
                        break;
                }
                results.TrainingIterations = i;
                results.TrainingError = trainingError;
            }
            else if (ConfigInternal.LearningAlgorithm == typeof(EvolutionaryLearning))
            {
                EvolutionaryLearning teacher = new EvolutionaryLearning(network, ConfigInternal.Population);
                double trainingError = 1.0;
                int i;
                for (i = 0; i < ConfigInternal.MaxTrainingIterations; i++)
                {
                    trainingError = teacher.RunEpoch(mlData.inputs, mlData.labels);
                    if (trainingError < ConfigInternal.TrainUntilError && i > ConfigInternal.MinTrainingIterations)
                        break;
                }
                results.TrainingIterations = i;
                results.TrainingError = trainingError;
            }
            else
                throw new PipelineException($"Neural Teacher Type unknown: {ConfigInternal.LearningAlgorithm.Name}", datasetIn, this, updateMessage);

            // Score non training data and return
            results = ScoreTestData(mlData, network, results, internalUpdate);
            return results;
        }


        protected MachineLearningResults ScoreTestData(DatasetML mlData, ActivationNetwork network, MachineLearningResults results, Action<string> updateMessage)
        {
            results.DatasetWithScores = new DatasetScored(mlData);

            // find the column that has the generated ml input data
            string inputColName = nameof(DataColumnML.IsMLInputData);
            string labelColName = nameof(DataColumnML.IsMLLabelData);
            string scoreColName = nameof(DataColumnBase.IsScore);
            string trainingColName = nameof(DataColumnBase.IsTraining);
            string probabilityColName = nameof(DataColumnBase.IsScoreProbability);
            double sumMeanSquaredError = 0;
            double sumError = 0;
            int rowCount = 0;
            bool foundData = false;
            foreach (DataRow row in results.DatasetWithScores.Table.Rows)
            {
                // if this is not a training row, let's calculate it's output
                if (ConfigInternal.IncludeTrainingDataInTestingData || !(bool)row[trainingColName])
                {
                    foundData = true;
                    double[] scores = network.Compute((double[])row[inputColName]);
                    row[scoreColName] = scores;

                    // Assess correctness of output versus expected
                    var labels = (double[])row[labelColName];
                    double[] errorVector = GetErrorVector(scores, labels);
                    sumError += errorVector.Sum(x => Math.Abs(x));
                    var meanSquareError = errorVector.MeanSquareError();
                    row[probabilityColName] = 1 - errorVector.Sum(x => Math.Abs(x));
                    sumMeanSquaredError += meanSquareError;
                    rowCount++;
                }
            }
            if (!foundData)
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
