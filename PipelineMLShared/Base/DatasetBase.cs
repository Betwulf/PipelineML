using System.Data;

namespace PipelineMLCore
{
    public class DatasetBase
    {
        public DatasetDescriptorBase Descriptor { get; set; }

        public string Name => Table?.TableName;

        public DataTable Table { get; set; }

        public DatasetBase(DatasetDescriptorBase descriptor)
        {
            if (descriptor != null)
            {
                Descriptor = descriptor;
                Table = new DataTable(Descriptor.Name);
                Descriptor.ColumnDescriptions.ForEach(x => Table.Columns.Add(x.Name, x.DataType));
            }
        }

        public DatasetBase GenerateSample()
        {
            var sample = new DatasetBase(Descriptor);
            sample.Table = Table.Clone();
            for (int i = 0; i < 100; i++)
            {
                sample.Table.Rows.Add(Table.Rows[i].ItemArray);
            }
            return sample;
        }
    }
}
