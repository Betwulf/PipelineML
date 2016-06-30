using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLInterfaces
{
    public interface IRawDatasetDescriptor
    {
        string Name { get; set; }

        List<DataColumn> ColumnNames { get; set; }


    }
}
