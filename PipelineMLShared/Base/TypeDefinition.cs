using System;

namespace PipelineMLCore
{
    public class TypeDefinition
    {
        public Type ClassType { get; set; }

        public string ClassConfig { get; set; }

        public string Guid { get; set; }

        private TypeDefinition(Type type, string config, string guid)
        {
            ClassType = type;
            ClassConfig = config;
            Guid = guid;
        }
        public TypeDefinition()
        {

        }

        public static TypeDefinition Create(IPipelinePart part)
        {
            return new TypeDefinition(part.GetType(), part.Config.ToJSON(), part.Id.ToString());
        }
    }
}
