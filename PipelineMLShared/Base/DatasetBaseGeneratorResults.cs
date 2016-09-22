using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DatasetBaseGeneratorResults : ResultsBase, IDatasetBaseGeneratorResults
    {
        public DatasetBaseGeneratorResults()
        {
            Log = new StringBuilder();
        }
        public IDatasetGenerator FromDatasetGenerator { get; set; }

        public DatasetBase SampleResults { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime StopTime { get; set; }

        public int RowCount { get; set; }


        public void LogUpdateResults(Action<string> updateMessage)
        {
            Action<string> updateAll = x => { updateMessage(x); Log.Append(x); };
            updateAll($"DatasetGenerator {FromDatasetGenerator.Name}({FromDatasetGenerator.GetType()}) - Results");
            updateAll($"Time Elapsed: {(StartTime-StopTime)}");
            updateAll($"Row Count: {RowCount}");
            updateAll($"Columns:");

            foreach (var col in SampleResults.Descriptor.ColumnDescriptions)
            {
                updateAll(GetColumnDetails(col));
            }



            updateAll($"Sample Data --------------- ");

            updateAll(string.Join(", ", SampleResults.Descriptor.ColumnDescriptions.Select(GetColumnName).ToArray()));
            for (int r = 0; r < Math.Min(100, SampleResults.Table.Rows.Count); r++)
            {
                updateAll($"Data: {string.Join(", ", SampleResults.Table.Rows[r].ItemArray)}");
            }

            updateAll($"");
        }

    }
}
