using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class MachineLearningResults : IMachineLearningResults
    {
        public IMachineLearningProcess FromMLProcess { get; set; }

        public DatasetScored DatasetWithScores { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime StopTime { get; set; }

    }
}
