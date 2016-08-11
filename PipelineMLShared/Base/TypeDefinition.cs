using System;

namespace PipelineMLCore
{
    public class TypeDefinition
    {
        public Type ClassType { get; set; }

        public string ClassConfig { get; set; }

        private TypeDefinition(Type type, string config)
        {
            ClassType = type;
            ClassConfig = config;
        }
        public TypeDefinition()
        {

        }

        public static TypeDefinition Create(IPipelinePart part)
        {
            return new TypeDefinition(part.GetType(), part.Config.ToJSON());
        }
    }
}
