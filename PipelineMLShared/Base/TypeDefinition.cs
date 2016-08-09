using System;

namespace PipelineMLCore
{
    public class TypeDefinition
    {
        public Type ClassType { get; }

        public string ClassConfig { get; }

        private TypeDefinition(Type type, string config)
        {
            ClassType = type;
            ClassConfig = config;
        }

        public static TypeDefinition Create(IPipelinePart part)
        {
            return new TypeDefinition(part.GetType(), part.Config.ToJSON());
        }
    }
}
