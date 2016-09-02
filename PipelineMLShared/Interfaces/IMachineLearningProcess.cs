using System;

namespace PipelineMLCore
{
    public interface IMachineLearningProcess : IPipelinePart
    {
        IMachineLearningResults TrainML(DatasetBase datasetIn, Action<string> updateMessage);
    }
}