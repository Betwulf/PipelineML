using Ninject;
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
        private IKernel _kernel;

        public TypeDefinition DatasetGenerator { get; set; }

        public ICollection<TypeDefinition> PreprocessDataTransforms { get; set; }

        public ICollection<TypeDefinition> MLList { get; set; }

        public ICollection<TypeDefinition> PostprocessDataTransforms { get; set; }

        public ICollection<TypeDefinition> Evaluators { get; set; }

        public PipelineDefinition(IKernel kernel)
        {
            _kernel = kernel;
            PreprocessDataTransforms = new List<TypeDefinition>();
            MLList = new List<TypeDefinition>();
            PostprocessDataTransforms = new List<TypeDefinition>();
            Evaluators = new List<TypeDefinition>();
        }

        public PipelineInstance CreateInstance()
        {
            var pi = new PipelineInstance(_kernel)
            {
                Name = Name
            };

            // hydrate dataset generator
            if (DatasetGenerator != null)
            {
                pi.DatasetGenerator = Activator.CreateInstance(DatasetGenerator.ClassType) as IDatasetGenerator;
                pi.DatasetGenerator.Configure(_kernel, DatasetGenerator.ClassConfig);
            }

            // hydrate preprocess data transforms
            foreach (var item in PreprocessDataTransforms)
            {
                IDataTransform dt = Activator.CreateInstance(item.ClassType) as IDataTransform;
                dt.Configure(_kernel, item.ClassConfig);
                pi.PreprocessDataTransforms.Add(dt);
            }

            // hydrate ml
            foreach (var item in MLList)
            {
                IMachineLearningProcess ml = Activator.CreateInstance(item.ClassType) as IMachineLearningProcess;
                ml.Configure(_kernel, item.ClassConfig);
                pi.MLList.Add(ml);
            }

            // hydrate postprocess data transforms
            foreach (var item in PostprocessDataTransforms)
            {
                IDataTransform dt = Activator.CreateInstance(item.ClassType) as IDataTransform;
                dt.Configure(_kernel, item.ClassConfig);
                pi.PostprocessDataTransforms.Add(dt);
            }

            //hydrate Evaluators
            foreach (var item in Evaluators)
            {
                IEvaluator eval = Activator.CreateInstance(item.ClassType) as IEvaluator;
                eval.Configure(_kernel, item.ClassConfig);
                pi.Evaluators.Add(eval);
            }

            return pi;
        }
    }
}