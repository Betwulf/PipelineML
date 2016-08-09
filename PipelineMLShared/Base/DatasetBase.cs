using PipelineMLCore;
using System.Data;

namespace PipelineMLShared.Base
{
    public class DatasetBase : IDataset
    {
        public IDatasetDescriptor Descriptor { get; set; }

        public string Name => Table?.TableName;

        public DataTable Table { get; set; }

        public DatasetBase(IDatasetDescriptor descriptor)
        {
            Descriptor = descriptor;
            Table = new DataTable(Descriptor.Name);
            Descriptor.ColumnNames.ForEach(x => Table.Columns.Add(x.Name, x.DataType));
        }

    }
}
