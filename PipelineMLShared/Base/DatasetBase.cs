using System;
using System.Collections.Generic;
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
            int MaxSampleCount = Math.Min(100,Table.Rows.Count);
            List<int> shuffledRowIndexes = new List<int>();
            var rnd = new Random();
            for (int i = 0; i < MaxSampleCount; i++)
            {
                int randomNum = rnd.Next(0, Table.Rows.Count);
                if (!shuffledRowIndexes.Contains(randomNum)) { shuffledRowIndexes.Add(randomNum); }
                else { i--; }
            }
            var sample = new DatasetBase(Descriptor);
            sample.Table = Table.Clone();

            for (int i = 0; i < MaxSampleCount; i++)
            {
                sample.Table.Rows.Add(Table.Rows[shuffledRowIndexes[i]].ItemArray);
            }
            return sample;
        }

        public DatasetBase Copy()
        {
            DatasetDescriptorBase copyDescriptor = new DatasetDescriptorBase();
            copyDescriptor.Name = Descriptor.Name;
            foreach (var col in Descriptor.ColumnDescriptions)
            {
                copyDescriptor.ColumnDescriptions.Add(new DataColumnBase() { Id = col.Id, Name = col.Name, DataType = col.DataType, Description = col.Description, IsCategory = col.IsCategory, IsFeature = col.IsFeature, IsLabel = col.IsLabel, IsTraining = col.IsTraining, IsScore = col.IsScore, IsScoreProbability = col.IsScoreProbability });
            }
            DatasetBase copyDataset = new DatasetBase(copyDescriptor);
            for (int r = 0; r < Table.Rows.Count; r++)
            {
                copyDataset.Table.Rows.Add(Table.Rows[r].ItemArray);
            }
            return copyDataset;
        }
    }
}
