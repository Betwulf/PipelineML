using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public interface DatasetBaseGeneratorResults
    {
        IDatasetGenerator FromDatasetGenerator { get; set; }

        DatasetBase SampleResults { get; set; }

        DatasetDescriptorBase DatasetDescriptor { get; set; }

        DateTime StartTime { get; set; }

        DateTime StopTime { get; set; }

    }
}
