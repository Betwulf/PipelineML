using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace PipelineMLInterfaces
{
    public interface IRawDataset
    {
        string Name { get; set; }

        IRawDatasetDescriptor Descriptor { get; set; }

        DataTable Table { get; set; }
    }
}
