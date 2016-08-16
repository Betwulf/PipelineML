using System;

namespace PipelineMLCore
{
    public interface IDataTransform : IPipelinePart
    {
        IDataset Transform(IDataset datasetIn, Action<string> updateMessage);
    }
}