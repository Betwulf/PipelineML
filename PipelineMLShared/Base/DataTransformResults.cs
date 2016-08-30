using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DataTransformResults : ResultsBase, IDataTransformResults
    {
        public IDataTransform FromDataTransform { get; set; }

        public DatasetBase SampleResults { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime StopTime { get; set; }


        public DataTransformResults()
        {
            Log = new StringBuilder();
        }

        public int RowCount { get; set; }



        public void LogUpdateResults(Action<string> updateMessage)
        {
            Action<string> updateAll = GetLoggedUpdateMessage(updateMessage);
            updateAll($"DataTransform {FromDataTransform.Name}({FromDataTransform.GetType()}) - Results");
            updateAll($"Time Elapsed: {(StartTime - StopTime)}");
            updateAll($"Row Count: {RowCount}");
            updateAll($"Columns:");

            foreach (var col in SampleResults.Descriptor.ColumnDescriptions)
            {
                updateAll(GetColumnDetails(col));
            }



            updateAll($"Sample Data --------------- ");

            updateAll(string.Join(", ", SampleResults.Descriptor.ColumnDescriptions.Select(GetColumnName).ToArray()));
            for (int i = 0; i < 10; i++)
            {
                updateAll($"Data: {string.Join(", ", SampleResults.Table.Rows[i].ItemArray)}");
            }

            updateAll($"");
        }

    }
}
