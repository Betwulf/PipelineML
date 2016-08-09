using System;

namespace PipelineMLCore
{
    public interface IDatasetGenerator : IPipelinePart
    {
        IDatasetDescriptor DatasetDescription { get; set; }

        IDataset Generate(Action<string> updateMessage);
    }
}
