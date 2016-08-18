using System;

namespace PipelineMLCore
{
    public interface IDataTransform : IPipelinePart
    {
        DatasetBase Transform(DatasetBase datasetIn, Action<string> updateMessage);
    }
}