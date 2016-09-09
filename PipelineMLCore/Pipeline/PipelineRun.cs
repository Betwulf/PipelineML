﻿using System;
using System.Collections.Generic;
using System.Linq;

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

            // Run DatasetGenerator
            results.DataSetGeneratorResult = new DatasetBaseGeneratorResults();
            results.DataSetGeneratorResult.StartTime = DateTime.Now;
            results.DataSetGeneratorResult.FromDatasetGenerator = Instance.DatasetGenerator;
            var dataset = Instance.DatasetGenerator.Generate(results.DataSetGeneratorResult.GetLoggedUpdateMessage(updateMessage));
            results.DataSetGeneratorResult.SampleResults = dataset.GenerateSample();
            results.DataSetGeneratorResult.StopTime = DateTime.Now;
            results.DataSetGeneratorResult.RowCount = dataset.Table.Rows.Count;
            results.DataSetGeneratorResult.LogUpdateResults(updateMessage);

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
                var mlout = ml.TrainML(dataset, updateMessage);
                mlout.LogUpdateResults(updateMessage);
                results.MachineLearningResults.Add(mlout);
            }

            return results;
        }
    }
}
