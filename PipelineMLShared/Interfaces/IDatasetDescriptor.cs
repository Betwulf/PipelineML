using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public interface IDatasetDescriptor
    {
        string Name { get; set; }

        List<DataColumnBase> ColumnDescriptions { get; set; }


    }
}
