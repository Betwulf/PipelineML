namespace PipelineMLCore
{
    public interface IMachineLearningProcess : IPipelinePart
    {
        IMachineLearningResults TrainML(DatasetBase datasetIn);
    }
}