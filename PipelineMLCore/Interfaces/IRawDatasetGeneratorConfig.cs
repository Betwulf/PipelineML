using System;

namespace PipelineMLCore
{
    public interface IRawDatasetGeneratorConfig
    {
        string Name { get; set; }

        Type RawDatasetGeneratorType { get; set; }


    }
}