using System;
using System.Data;

namespace PipelineMLCore
{
    public class DatasetScored : DatasetML
    {
        public DatasetScored(DatasetML mlData) : base(mlData.Descriptor)
        {
            Table = mlData.Table;
            Table.Columns.Add(nameof(DataColumnBase.IsScore), typeof(int));
            Table.Columns.Add(nameof(DataColumnBase.IsScoreProbability), typeof(double));
            Descriptor.ColumnDescriptions.Add(new DataColumnML() { Name = nameof(DataColumnBase.IsScore), DataType = typeof(int), IsScore = true });
            Descriptor.ColumnDescriptions.Add(new DataColumnML() { Name = nameof(DataColumnBase.IsScoreProbability), DataType = typeof(double), IsScoreProbability = true });
        }
    }
}
