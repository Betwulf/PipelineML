﻿namespace PipelineMLCore
{
    public interface IEvaluator : IPipelinePart
    {
        IEvaluatorResults Evaluate(IMachineLearningResults mlResults, DatasetBase datasetIn);
    }
}