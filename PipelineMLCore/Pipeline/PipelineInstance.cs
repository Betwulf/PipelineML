using Ninject;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;

namespace PipelineMLCore
{
    /// <summary>
    /// An instance pipeline as defined by a PipelineDefinition - see that class for more info.
    /// </summary>
    public class PipelineInstance : ConfigBase
    {
        private IKernel _kernel;

        public string InstanceID { get; set; }

        public PipelineInstance(IKernel kernel)
        {
            _kernel = kernel;
            InstanceID = Path.GetTempFileName();
            PreprocessDataTransforms = new List<IDataTransform>();
            MLList = new List<IMachineLearningProcess>();
            PostprocessDataTransforms = new List<IDataTransform>();
            Evaluators = new List<IEvaluator>();
        }

        public PipelineDefinition CreateDefinition()
        {
            var pd = new PipelineDefinition(_kernel)
            {
                // Base params
                Name = Name,
                DatasetGenerator = TypeDefinition.Create(DatasetGenerator)
            };

            // Preprocessors
            PreprocessDataTransforms
                .Select(TypeDefinition.Create)
                .ToList()
                .ForEach(pd.PreprocessDataTransforms.Add);

            // ML
            MLList
                .Select(TypeDefinition.Create)
                .ToList()
                .ForEach(pd.MLList.Add);

            // Postprocessors
            PostprocessDataTransforms
                .Select(TypeDefinition.Create)
                .ToList()
                .ForEach(pd.PostprocessDataTransforms.Add);

            // Evaluators
            Evaluators
                .Select(TypeDefinition.Create)
                .ToList()
                .ForEach(pd.Evaluators.Add);

            return pd;

        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IDatasetGenerator DatasetGenerator { get; set; }

        [TypeConverter(typeof(CollectionEditor))]
        public List<IDataTransform> PreprocessDataTransforms { get; set; }

        [TypeConverter(typeof(CollectionEditor))]
        public List<IMachineLearningProcess> MLList { get; set; }

        [TypeConverter(typeof(CollectionEditor))]
        public List<IDataTransform> PostprocessDataTransforms { get; set; }

        [TypeConverter(typeof(CollectionEditor))]
        public List<IEvaluator> Evaluators { get; set; }

    }
}
