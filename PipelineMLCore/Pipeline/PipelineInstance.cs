using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    /// <summary>
    /// An instance pipeline as defined by a PipelineDefinition - see that class for more info.
    /// </summary>
    public class PipelineInstance : ConfigBase
    {
        public string InstanceID { get; set; }

        public PipelineInstance()
        {
            InstanceID = Path.GetRandomFileName();
            PreprocessDataTransforms = new List<IDataTransform>();
            MLList = new List<IMachineLearningProcess>();
            PostprocessDataTransforms = new List<IDataTransform>();
        }

        public PipelineDefinition CreateDefinition()
        {
            var pd = new PipelineDefinition();

            // Base params
            pd.Name = Name;
            pd.RootDirectory = RootDirectory;

            // Dataset Gen
            pd.DatasetGenerator = new TypeDefinition();
            pd.DatasetGenerator.ClassConfig = DatasetGenerator.Config.ToJSON();
            pd.DatasetGenerator.ClassType = DatasetGenerator.GetType();

            //Preprocessors
            foreach (var item in PreprocessDataTransforms)
            {
                var dtrans = new TypeDefinition() { ClassConfig = item.Config.ToJSON(), ClassType = item.GetType() };
                pd.PreprocessDataTransforms.Add(dtrans);
            }

            // ML
            foreach (var item in MLList)
            {
                var ml = new TypeDefinition() { ClassConfig = item.Config.ToJSON(), ClassType = item.GetType() };
                pd.MLList.Add(ml);
            }

            return pd;

        }

        public string RootDirectory { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IRawDatasetGenerator DatasetGenerator { get; set; }

        public List<IDataTransform> PreprocessDataTransforms { get; set; }

        public List<IMachineLearningProcess> MLList { get; set; }

        public List<IDataTransform> PostprocessDataTransforms { get; set; }

        public ITradesimulator TradeSim { get; set; }

        public IEvaluator Evaluator { get; set; }

    }
}
