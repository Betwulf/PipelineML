﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class MachineLearningConfigDecisionTree : ConfigBase
    {
        public MachineLearningConfigDecisionTree()
        {
            IncludeTrainingDataInTestingData = false;
        }

        public bool IncludeTrainingDataInTestingData { get; set; }
    }
}
