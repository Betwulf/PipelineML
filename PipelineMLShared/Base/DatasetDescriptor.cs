using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DatasetDescriptor : IDatasetDescriptor
    {
        public string Name { get; set; }

        public List<DataColumnBase> ColumnDescriptions { get; set; }

        public DatasetDescriptor()
        {
            ColumnDescriptions = new List<DataColumnBase>();
        }

    }
}
