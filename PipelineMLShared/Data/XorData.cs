using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class XorData
    {
        public static DatasetBase GetXorData()
        {
            var descriptor = new DatasetDescriptorBase();
            descriptor.ColumnDescriptions.Add(new DataColumnBase() { DataType = typeof(double), Name = "a", Description = "First Input", IsFeature = true, IsTraining = true, Id = 0 });
            descriptor.ColumnDescriptions.Add(new DataColumnBase() { DataType = typeof(double), Name = "b", Description = "Second Input", IsFeature = true, IsTraining = true, Id = 0 });
            descriptor.ColumnDescriptions.Add(new DataColumnBase() { DataType = typeof(int), Name = "out", Description = "Output", IsLabel=true, IsTraining = true, Id = 0 });
            var DatasetBaseXOR = new DatasetBase(descriptor);
            DatasetBaseXOR.Table.Rows.Add(new object[] { 0.0, 0.0, 0 });
            DatasetBaseXOR.Table.Rows.Add(new object[] { 0.0, 1.0, 1 });
            DatasetBaseXOR.Table.Rows.Add(new object[] { 1.0, 0.0, 1 });
            DatasetBaseXOR.Table.Rows.Add(new object[] { 1.0, 1.0, 0 });

            // Create Training column
            var trainingColumn = new DataColumnBase()
            {
                Name = nameof(DataColumnBase.IsTraining),
                DataType = typeof(bool),
                Description = "Training Flag",
                IsFeature = false,
                IsLabel = false,
                IsScore = false,
                IsScoreProbability = false,
                IsTraining = true,
                Id = -1
            };
            DatasetBaseXOR.Descriptor.ColumnDescriptions.Add(trainingColumn);
            DatasetBaseXOR.Table.Columns.Add(trainingColumn.Name, trainingColumn.DataType);

            // create training values
            for (int r = 0; r < DatasetBaseXOR.Table.Rows.Count; r++)
            {
                var row = DatasetBaseXOR.Table.Rows[r];
                row[trainingColumn.Name] = true;
            }

            return DatasetBaseXOR;
        }

    }
}
