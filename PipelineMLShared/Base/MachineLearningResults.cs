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

        public double MeanSquareError { get; set; }

        public double RootMeanSquareDeviation { get; set; }

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
            updateAll($"MeanSquareError: {MeanSquareError}");
            updateAll($"RootMeanSquareDeviation: {RootMeanSquareDeviation}");
            updateAll($"Columns:");

            foreach (var col in DatasetWithScores.Descriptor.ColumnDescriptions)
            {
                updateAll(GetColumnDetails(col));
            }



            updateAll($"Dataset With Scores --------------- ");

            updateAll(string.Join(", ", DatasetWithScores.Descriptor.ColumnDescriptions.Select(GetColumnName).ToArray()));
            for (int r = 0; r < Math.Min(20, DatasetWithScores.Table.Rows.Count); r++)
            {
                List<string> StringList = new List<string>();
                for (int c = 0; c < DatasetWithScores.Table.Columns.Count; c++)
                {
                    if (DatasetWithScores.Table.Columns[c].DataType == typeof(double[]))
                    {
                        if (DatasetWithScores.Table.Rows[r][c].GetType() == typeof(DBNull))
                            StringList.Add("NULL");
                        else
                            StringList.Add(((double[])DatasetWithScores.Table.Rows[r][c]).ToExpandedString());
                    }
                    else
                    {
                        if (DatasetWithScores.Table.Rows[r][c].GetType() == typeof(DBNull))
                            StringList.Add("NULL");
                        else
                            StringList.Add(DatasetWithScores.Table.Rows[r][c].ToString());
                    }

                }
                updateAll($"{string.Join(", ", StringList.ToArray())}");
            }

            updateAll($"");
        }
    }
}
