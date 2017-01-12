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
            var now = DateTime.Now;
            results.RunDate = now;
            DatasetBase dataset = null;

            // Run DatasetGenerator
            try
            {
                results.DataSetGeneratorResult = new DatasetBaseGeneratorResults();
                results.DataSetGeneratorResult.StartTime = now;
                results.DataSetGeneratorResult.FromDatasetGenerator = Instance.DatasetGenerator;
                dataset = Instance.DatasetGenerator.Generate(results.DataSetGeneratorResult.GetLoggedUpdateMessage(updateMessage));
                results.DataSetGeneratorResult.SampleResults = dataset.GenerateSample();
                results.DataSetGeneratorResult.StopTime = DateTime.Now;
                results.DataSetGeneratorResult.RowCount = dataset.Table.Rows.Count;
                results.DataSetGeneratorResult.LogUpdateResults(updateMessage);
            }
            catch (Exception ex)
            {
                // TODO: need to get errors into the PipelineResults
                updateMessage($"Failure running Dataset Generator: {ex.Message}");
                Log.Logger.Error("Failure running Dataset Generator: {Message}", ex.Message);
                return results;
            }

            // Run Preprocessors
            results.PreprocessTransformOutput = new List<IDataTransformResults>();
            foreach (var dt in Instance.PreprocessDataTransforms)
            {
                try
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
                catch (Exception ex)
                {
                    updateMessage($"Failure running Preprocessor: {dt.Name} - {ex.Message}");
                    Log.Logger.Error("Failure running Preprocessor: {Name} - {Message}", dt.Name, ex.Message);
                    return results;
                }
            }

            // Run ML
            results.MachineLearningResults = new List<IMachineLearningResults>();
            foreach (var ml in Instance.MLList)
            {
                try
                {
                    DatasetBase copiedDataset = dataset.Copy();
                    var mlout = ml.TrainML(copiedDataset, updateMessage);
                    mlout.LogUpdateResults(updateMessage);
                    results.MachineLearningResults.Add(mlout);
                }
                catch (Exception ex)
                {
                    updateMessage($"Failure running ML: {ml.Name} - {ex.Message}");
                    Log.Logger.Error("Failure running ML: {Name} - {Message}", ml.Name, ex.Message);
                    return results;
                }
            }


            // Run Postprocessors
            results.PostprocessTransformOutput = new List<IDataTransformResults>();
            foreach (var dt in Instance.PostprocessDataTransforms)
            {
                try
                {
                    var presults = new DataTransformResults();
                    presults.StartTime = DateTime.Now;
                    presults.FromDataTransform = dt;
                    dataset = dt.Transform(dataset, presults.GetLoggedUpdateMessage(updateMessage));
                    presults.SampleResults = dataset.GenerateSample();
                    presults.StopTime = DateTime.Now;
                    presults.RowCount = dataset.Table.Rows.Count;
                    presults.LogUpdateResults(updateMessage);
                    results.PostprocessTransformOutput.Add(presults);
                }
                catch (Exception ex)
                {
                    updateMessage($"Failure running Postprocessor: {dt.Name} - {ex.Message}");
                    Log.Logger.Error("Failure running Postprocessor: {Name} - {Message}", dt.Name, ex.Message);
                    return results;
                }

            }



            return results;
        }
    }
}
