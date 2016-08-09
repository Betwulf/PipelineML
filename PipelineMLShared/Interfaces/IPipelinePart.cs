namespace PipelineMLCore
{
    public interface IPipelinePart : INamed
    {
        void Configure(string rootDirectory, string jsonConfig);

        ConfigBase Config { get; set; }
    }
}
