namespace PipelineMLCore
{
    public interface IDataTransform : IPipelinePart
    {
        IRawDataset Transform(IRawDataset datasetIn);
    }
}