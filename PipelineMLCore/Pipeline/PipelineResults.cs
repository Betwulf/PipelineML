using System;
using System.Collections.Generic;

namespace PipelineMLCore
{
    public class PipelineResults
    {
        public PipelineResults()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public DateTime RunDate { get; set; }

        public IDatasetBaseGeneratorResults DataSetGeneratorResult { get; set; }

        public List<IDataTransformResults> PreprocessTransformOutput { get; set; }

        public List<IMachineLearningResults> MachineLearningResults { get; set; }

        public List<IDataTransformResults> PostprocessTransformOutput { get; set; }

        public List<IEvaluatorResults> EvaluatorResults { get; set; }


    }
}