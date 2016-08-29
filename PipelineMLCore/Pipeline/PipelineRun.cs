using System;
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
            var dataset = Instance.DatasetGenerator.Generate(updateMessage);
            results.DataSetGeneratorResult.SampleResults = dataset.GenerateSample();
            results.DataSetGeneratorResult.StopTime = DateTime.Now;
            return results;
        }
    }
}
