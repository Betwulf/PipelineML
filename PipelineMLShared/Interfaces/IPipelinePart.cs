using Ninject;

namespace PipelineMLCore
{
    public interface IPipelinePart : INamed
    {
        void Configure(IKernel kernel, string jsonConfig);

        ConfigBase Config { get; set; }
    }
}
