using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DatasetBaseGeneratorResults : IDatasetBaseGeneratorResults
    {
        public IDatasetGenerator FromDatasetGenerator { get; set; }

        public DatasetBase SampleResults { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime StopTime { get; set; }

    }
}
