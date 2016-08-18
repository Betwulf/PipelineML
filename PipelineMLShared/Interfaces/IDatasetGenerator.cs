using System;

namespace PipelineMLCore
{
    public interface IDatasetGenerator : IPipelinePart
    {
        DatasetDescriptorBase DatasetDescription { get; set; }

        DatasetBase Generate(Action<string> updateMessage);
    }
}
