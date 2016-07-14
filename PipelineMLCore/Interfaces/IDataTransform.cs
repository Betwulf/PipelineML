namespace PipelineMLCore
{
    public interface IDataTransform : IPipelinePart
    {
        string Name { get; set; }

        IRawDataset Transform(IRawDataset datasetIn);
    }
}