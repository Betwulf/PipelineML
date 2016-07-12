namespace PipelineMLCore
{
    public interface IDataTransform
    {
        string Name { get; set; }

        void Configure(string jsonConfig);

        IRawDataset Transform(IRawDataset datasetIn);
    }
}