using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class MachineLearningResults : ResultsBase, IMachineLearningResults
    {
        public IMachineLearningProcess FromMLProcess { get; set; }

        public DatasetScored DatasetWithScores { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime StopTime { get; set; }

        public double Error { get; set; }

        public double TrainingError { get; set; }

        public int TrainingIterations { get; set; }

        public MachineLearningResults()
        {
            Log = new StringBuilder();
        }

        public void LogUpdateResults(Action<string> updateMessage)
        {
            Action<string> updateAll = GetLoggedUpdateMessage(updateMessage);
            updateAll($"Machine Learning Results - {FromMLProcess.Name}({FromMLProcess.GetType()})");
            updateAll($"Time Elapsed: {(StartTime - StopTime)}");
            updateAll($"Training Iterations: {TrainingIterations}");
            updateAll($"Training Error: {TrainingError}");
            updateAll($"Error: {Error}");
            updateAll($"Columns:");

            foreach (var col in DatasetWithScores.Descriptor.ColumnDescriptions)
            {
                updateAll(GetColumnDetails(col));
            }



            updateAll($"Dataset With Scores --------------- ");

            updateAll(string.Join(", ", DatasetWithScores.Descriptor.ColumnDescriptions.Select(GetColumnName).ToArray()));
            for (int i = 0; i < 10; i++)
            {
                updateAll($"Data: {string.Join(", ", DatasetWithScores.Table.Rows[i].ItemArray)}");
            }

            updateAll($"");
        }
    }
}
