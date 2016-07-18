using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore.Base
{
    public class RawDatasetBase : IRawDataset
    {
        public IRawDatasetDescriptor Descriptor { get; set; }

        public string Name { get; set; }

        public DataTable Table { get; set; }

    }
}
