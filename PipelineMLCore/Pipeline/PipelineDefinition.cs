using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PipelineMLCore
{
    /// <summary>
    /// Serializable template that can create a Pipeline Instance. Should contain all the types and configuration 
    /// that define a pipeline.
    /// 1) Dataset Generator
    /// 2) Preprocess Data Transforms
    /// 3) Machine Learning Algorithim collection (all can run in parallel on the preprocessed data)
    /// 4) Postprocess Data Tranforms
    /// 5) Evaluators - this will compare the effectiveness of the various machine learning algorithms relative to each other / or absolute.
    /// 6) Trade Simulation - Step to specifically go run a simulation with the trained machine learning algorithm
    /// </summary>
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