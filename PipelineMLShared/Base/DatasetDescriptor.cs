using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DatasetDescriptor : IRawDatasetDescriptor
    {
        public string Name { get; set; }

        public List<DataColumn> ColumnNames { get; set; }

        public DatasetDescriptor()
        {
            ColumnNames = new List<DataColumn>();
        }

    }
}
