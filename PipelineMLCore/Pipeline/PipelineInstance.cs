using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class PipelineInstance
    {
        public PipelineDefinition Definition { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IRawDatasetGenerator DatasetGenerator { get; set; }

        public Queue<IDataTransform> PreprocessDataTransforms { get; set; }

        public List<IMachineLearningProcess> MLList { get; set; }

        public Queue<IDataTransform> PostprocessDataTransforms { get; set; }

        public ITradesimulator TradeSim { get; set; }

        public IEvaluator Evaluator { get; set; }

    }
}
