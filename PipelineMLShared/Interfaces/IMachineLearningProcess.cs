namespace PipelineMLCore
{
    public interface IMachineLearningProcess : IPipelinePart
    {
        IMachineLearningResults TrainML(IDataset datasetIn);
    }
}