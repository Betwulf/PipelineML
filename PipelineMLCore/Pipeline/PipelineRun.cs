using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;

namespace PipelineMLCore
{
    public class PipelineRun
    {
        private PipelineInstance Instance { get; }

        public PipelineRun(PipelineInstance pi)
        {
            Instance = pi;
        }

        public PipelineResults Run(Action<string> updateMessage)
        {
            var results = new PipelineResults();
            DatasetBase dataset = null;

            // Run DatasetGenerator
            try
            {
                results.DataSetGeneratorResult = new DatasetBaseGeneratorResults();
                results.DataSetGeneratorResult.StartTime = DateTime.Now;
                results.DataSetGeneratorResult.FromDatasetGenerator = Instance.DatasetGenerator;
                dataset = Instance.DatasetGenerator.Generate(results.DataSetGeneratorResult.GetLoggedUpdateMessage(updateMessage));
                results.DataSetGeneratorResult.SampleResults = dataset.GenerateSample();
                results.DataSetGeneratorResult.StopTime = DateTime.Now;
                results.DataSetGeneratorResult.RowCount = dataset.Table.Rows.Count;
                results.DataSetGeneratorResult.LogUpdateResults(updateMessage);
            }
            catch (Exception ex)
            {
                updateMessage($"Failure running Dataset Generator: {ex.Message}");
                Log.Logger.Error("Failure running Dataset Generator: {Message}", ex.Message);
            }

            // Run Preprocessors
            results.PreprocessTransformOutput = new List<IDataTransformResults>();
            foreach (var dt in Instance.PreprocessDataTransforms)
            {
                var presults = new DataTransformResults();
                presults.StartTime = DateTime.Now;
                presults.FromDataTransform = dt;
                dataset = dt.Transform(dataset, presults.GetLoggedUpdateMessage(updateMessage));
                presults.SampleResults = dataset.GenerateSample();
                presults.StopTime = DateTime.Now;
                presults.RowCount = dataset.Table.Rows.Count;
                presults.LogUpdateResults(updateMessage);
                results.PreprocessTransformOutput.Add(presults);
            }

            // Run ML
            results.MachineLearningResults = new List<IMachineLearningResults>();
            foreach (var ml in Instance.MLList)
            {
                DatasetBase copiedDataset = dataset.Copy();
                var mlout = ml.TrainML(copiedDataset, updateMessage);
                mlout.LogUpdateResults(updateMessage);
                results.MachineLearningResults.Add(mlout);
            }


            return results;
        }
    }
}
