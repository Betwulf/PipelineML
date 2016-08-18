using System.Collections.Generic;

namespace PipelineMLCore
{
    public class PipelineResults
    {
        public DatasetBaseGeneratorResults DataSetGeneratorResult { get; set; }

        public List<IDataTransformResults> PreprocessTransformOutput { get; set; }

        public List<IMachineLearningResults> MachineLearningResults { get; set; }

        public List<IDataTransformResults> PostprocessTransformOutput { get; set; }

        public List<IEvaluatorResults> EvaluatorResults { get; set; }


    }
}