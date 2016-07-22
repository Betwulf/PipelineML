using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public interface IDatasetGeneratorResults
    {
        IDatasetGenerator FromDatasetGenerator { get; set; }

        IDataset SampleResults { get; set; }

        IDatasetDescriptor DatasetDescriptor { get; set; }

        DateTime StartTime { get; set; }

        DateTime StopTime { get; set; }

    }
}
