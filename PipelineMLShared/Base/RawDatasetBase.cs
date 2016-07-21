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

        public string Name { get { return Table?.TableName ; } }

        public DataTable Table { get; set; }

        public RawDatasetBase(IRawDatasetDescriptor descriptor)
        {
            Descriptor = descriptor;
            Table = new DataTable(Descriptor.Name);
            Descriptor.ColumnNames.ForEach(x => Table.Columns.Add(x.Name, x.DataType));
        }

    }
}
