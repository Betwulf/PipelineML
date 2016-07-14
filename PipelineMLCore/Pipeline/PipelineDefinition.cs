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
        
        public List<TypeDefinition> PreprocessDataTransforms { get; set; }

        public List<TypeDefinition> MLList { get; set; }

        public List<TypeDefinition> PostprocessDataTransforms { get; set; }

        public TypeDefinition TradeSim { get; set; }

        public TypeDefinition Evaluator { get; set; }

        public PipelineDefinition()
        {
            PreprocessDataTransforms = new List<TypeDefinition>();
            MLList = new List<TypeDefinition>();
            PostprocessDataTransforms = new List<TypeDefinition>();

        }

        public PipelineInstance CreateInstance()
        {
            var pi = new PipelineInstance();

            // hydrate params
            pi.Name = Name;
            pi.RootFolder = RootFolder;


            // hydrate dataset generator
            if (DatasetGenerator != null)
            {
                pi.DatasetGenerator = Activator.CreateInstance(DatasetGenerator.ClassType) as IRawDatasetGenerator;
                pi.DatasetGenerator.Configure(DatasetGenerator.ClassConfig);
            }

            // hydrate preprocess data transforms
            foreach (var item in PreprocessDataTransforms)
            {
                IDataTransform dt = Activator.CreateInstance(item.ClassType) as IDataTransform;
                dt.Configure(item.ClassConfig);
                pi.PreprocessDataTransforms.Add(dt);
            }

            // hydrate ml
            foreach (var item in MLList)
            {
                IMachineLearningProcess ml = Activator.CreateInstance(item.ClassType) as IMachineLearningProcess;
                ml.Configure(item.ClassConfig);
                pi.MLList.Add(ml);
            }


            return pi;
        }
    }
}