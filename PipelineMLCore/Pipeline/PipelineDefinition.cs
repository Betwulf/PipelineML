using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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

        public PipelineDefinition()
        {
            Id = Guid.NewGuid();
            PreprocessDataTransforms = new List<TypeDefinition>();
            MLList = new List<TypeDefinition>();
            PostprocessDataTransforms = new List<TypeDefinition>();
            Evaluators = new List<TypeDefinition>();
        }

        public void Configure(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IPipelinePart CreateInstanceOf(int column, TypeDefinition typeDef)
        {
            if (typeDef == null) return null;
            switch (column)
            {
                case 0:
                    // hydrate DatasetGenerator
                    var datasetGenerator = Activator.CreateInstance(typeDef.ClassType) as IDatasetGenerator;
                    datasetGenerator.Configure(_kernel, typeDef.ClassConfig);
                    return datasetGenerator;
                case 1:
                case 3:
                    // hydrate data transform
                    IDataTransform dt = Activator.CreateInstance(typeDef.ClassType) as IDataTransform;
                    dt.Configure(_kernel, typeDef.ClassConfig);
                    return dt;
                case 2:
                    IMachineLearningProcess ml = Activator.CreateInstance(typeDef.ClassType) as IMachineLearningProcess;
                    ml.Configure(_kernel, typeDef.ClassConfig);
                    return ml;
                case 4:
                    IEvaluator eval = Activator.CreateInstance(typeDef.ClassType) as IEvaluator;
                    eval.Configure(_kernel, typeDef.ClassConfig);
                    return eval;
                default:
                    return null;
            }
        }


        public IPipelinePart CreateInstanceOf(int column, Type classType, string Guid)
        {
            switch (column)
            {
                case 0:
                    // hydrate DatasetGenerator
                    if (DatasetGenerator != null && DatasetGenerator.ClassType == classType)
                    {
                        var partDG = CreateInstanceOf(column, DatasetGenerator);
                        if (partDG.Id.ToString() == Guid)
                            return partDG;
                    }
                    return null;
                case 1:
                    // hydrate preprocess data transforms
                    var resultPre = PreprocessDataTransforms.FirstOrDefault(x => x.ClassType == classType && x.Guid == Guid);
                    if (resultPre != null)
                    {
                        var partPre = CreateInstanceOf(column, resultPre);
                        if (partPre.Id.ToString() == Guid)
                            return partPre;
                    }
                    return null;
                case 2:
                    // hydrate ml MLList
                    var resultML = MLList.FirstOrDefault(x => x.ClassType == classType && x.Guid == Guid);
                    if (resultML != null)
                    {
                        var partML = CreateInstanceOf(column, resultML);
                        if (partML.Id.ToString() == Guid)
                            return partML;
                    }
                    return null;
                case 3:
                    // hydrate preprocess data transforms
                    var resultPost = PostprocessDataTransforms.FirstOrDefault(x => x.ClassType == classType && x.Guid == Guid);
                    if (resultPost != null)
                    {
                        var partPost = CreateInstanceOf(column, resultPost);
                        if (partPost.Id.ToString() == Guid)
                            return partPost;
                    }
                    return null;
                case 4:
                    var resultEval = Evaluators.FirstOrDefault(x => x.ClassType == classType && x.Guid == Guid);
                    if (resultEval != null)
                    {
                        var partEval = CreateInstanceOf(column, resultEval);
                        if (partEval.Id.ToString() == Guid)
                            return partEval;
                    }
                    return null;
                default:
                    return null;
            }
        }


        public PipelineInstance CreateInstance()
        {
            var pi = new PipelineInstance()
            {
                Name = Name
            };

            pi.Configure(_kernel);

            // hydrate dataset generator
            pi.DatasetGenerator = CreateInstanceOf(0, DatasetGenerator) as IDatasetGenerator;

            // hydrate preprocess data transforms
            foreach (var item in PreprocessDataTransforms)
            {
                IDataTransform dt = CreateInstanceOf(1, item) as IDataTransform;
                pi.PreprocessDataTransforms.Add(dt);
            }

            // hydrate ml
            foreach (var item in MLList)
            {
                IMachineLearningProcess ml = CreateInstanceOf(2,item) as IMachineLearningProcess;
                pi.MLList.Add(ml);
            }

            // hydrate postprocess data transforms
            foreach (var item in PostprocessDataTransforms)
            {
                IDataTransform dt = CreateInstanceOf(3, item) as IDataTransform;
                pi.PostprocessDataTransforms.Add(dt);
            }

            //hydrate Evaluators
            foreach (var item in Evaluators)
            {
                IEvaluator eval = CreateInstanceOf(4, item) as IEvaluator;
                pi.Evaluators.Add(eval);
            }

            return pi;
        }
    }
}