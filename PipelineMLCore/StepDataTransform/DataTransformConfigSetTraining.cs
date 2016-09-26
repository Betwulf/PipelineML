using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DataTransformConfigSetTraining : ConfigBase
    {
        public double PercentOfTrainingData { get; set; }

        public int RandomSeed { get; set; }

        public DataTransformConfigSetTraining()
        {
            RandomSeed = 0;
            PercentOfTrainingData = .5;
        }
    }
}
