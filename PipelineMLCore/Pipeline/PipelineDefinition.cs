using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PipelineMLCore
{
    public class PipelineDefinition : ConfigBase
    {

        public string RootFolder { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TypeDefinition DatasetGenerator { get; set; }
        
        public Queue<TypeDefinition> PreprocessDataTransforms { get; set; }

        public List<TypeDefinition> MLList { get; set; }

        public Queue<TypeDefinition> PostprocessDataTransforms { get; set; }

        public TypeDefinition TradeSim { get; set; }

        public TypeDefinition Evaluator { get; set; }

        public PipelineDefinition()
        {
            PreprocessDataTransforms = new Queue<TypeDefinition>();
            MLList = new List<TypeDefinition>();
            PostprocessDataTransforms = new Queue<TypeDefinition>();

        }

        public PipelineInstance CreateInstance()
        {
            var pi = new PipelineInstance();
            pi.DatasetGenerator = Activator.CreateInstance(DatasetGenerator.ClassType) as IRawDatasetGenerator;
            pi.DatasetGenerator.Configure(DatasetGenerator.ClassConfig);
            return pi;
        }
    }
}