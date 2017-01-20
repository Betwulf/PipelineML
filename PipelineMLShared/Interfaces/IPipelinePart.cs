using Ninject;
using System;

namespace PipelineMLCore
{
    public interface IPipelinePart : INamed
    {
        void Configure(IKernel kernel, string jsonConfig);

        ConfigBase Config { get; set; }

        Guid Id { get; set; }
    }
}
