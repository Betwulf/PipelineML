using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public interface IEvaluatorResults
    {
        IEvaluator FromEvaluator { get; set; }

        DateTime StartTime { get; set; }

        DateTime StopTime { get; set; }

    }
}
