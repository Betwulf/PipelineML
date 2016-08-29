using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public interface IDatasetBaseGeneratorResults
    {
        IDatasetGenerator FromDatasetGenerator { get; set; }

        DatasetBase SampleResults { get; set; }

        DateTime StartTime { get; set; }

        DateTime StopTime { get; set; }

    }
}
