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
                double trainingError = 1.0;
                int i;
                for (i = 0; i < ConfigInternal.MaxTrainingIterations; i++)
                {
                    trainingError = teacher.RunEpoch(mlData.inputs, mlData.labels);
                    trainingError = trainingError / mlData.inputs.Length;
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

        /// <summary>
        /// Returns the squared error
        /// </summary>
        /// <param name="output"></param>
        /// <param name="desiredOutput"></param>
        /// <returns>the squared error</returns>
        protected double GetError(double[] output, double[] desiredOutput)
        {
            double error = 0;
            
            if (output.Length != desiredOutput.Length)
            {
                throw new IndexOutOfRangeException($"Output Length: {output.Length} does not match desired output length: {desiredOutput.Length}");
            }
            for (int i = 0; i < output.Length; i++)
            {
                // error of the neuron
                double currentError = desiredOutput[i] - output[i];
                // square the error and sum it
                error += (currentError * currentError);
            }
            // return squared error of the last layer divided by 2
            return error / 2.0;
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
            double runningError = 0;
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
                    double rowError = GetError(scores, labels);
                    row[probabilityColName] = rowError;
                    runningError += rowError;
                }
            }
            if (!foundData)
                throw new PipelineException($"found zero scores.", results.DatasetWithScores, this, results.GetLoggedUpdateMessage(updateMessage));

            results.Error = runningError / results.DatasetWithScores.Table.Rows.Count;
            results.FromMLProcess = this;
            results.StopTime = DateTime.Now;
            return results;
        }

    }
}
