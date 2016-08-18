using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;


namespace PipelineMLCore
{
    public class DatasetGeneratorCSVFile : IDatasetGenerator, ISearchableClass
    {
        public string Name { get; set; }

        public DatasetDescriptorBase DatasetDescription { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        private DatasetConfigCSVFile ConfigInternal { get { return Config as DatasetConfigCSVFile; } }

        public string FriendlyName { get { return "CSV File"; } }

        public string Description { get { return "Will search on disk for a CSV file. CSV File MUST have column headers."; } }

        public DatasetGeneratorCSVFile()
        {
            Config = new DatasetConfigCSVFile();

        }

        public void Configure(string rootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DatasetConfigCSVFile>(jsonConfig);
            Name = Config.Name;
        }

        public DatasetBase Generate(Action<string> updateMessage)
        {
            string CSVString;
            DatasetDescription = new DatasetDescriptorBase();
            DataTable dt = new DataTable();


            if (!File.Exists(ConfigInternal.Filepath)) { return null; }
            {
                CSVString = File.ReadAllText(ConfigInternal.Filepath);
            }
            if (CSVString == null) return null;

            string[] tableData = CSVString.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var col = from cl in tableData[0].Split(",".ToCharArray())
                      select new DataColumn(cl);
            dt.Columns.AddRange(col.ToArray());
            col.ToList().ForEach(x =>
            {
                DatasetDescription.ColumnDescriptions.Add(new DataColumnBase() { Id = x.Ordinal, Name = x.ColumnName,
                    DataType = x.DataType, Description = x.ColumnName, IsFeature = true, IsLabel = false,
                    IsCategory = false, IsScore = false, IsScoreProbability = false, IsTraining = false });
            });

            foreach (var item in tableData.Skip(1))
            {
                var pieces = LineSplitter(item);
                dt.Rows.Add(pieces.ToArray());
            }

            // try to recognize and convert numerical columns to int and double where applicable
            // TODO: This is SUPER INEFFICIENT, several copies of data in memory please fix
            foreach (var column in DatasetDescription.ColumnDescriptions)
            {
                bool IsInt = true;
                bool IsDouble = true;
                double d;
                int i;
                for (int rownum = 0; rownum < dt.Rows.Count; rownum++)
                {
                    IsDouble = (double.TryParse(dt.Rows[rownum][column.Name].ToString(), out d)) && IsDouble;
                    IsInt = (int.TryParse(dt.Rows[rownum][column.Name].ToString(), out i)) && IsInt;
                }
                if (IsInt)
                {
                    DataTable clonedTable = dt.Clone();
                    clonedTable.Columns[column.Name].DataType = typeof(int);
                    foreach (DataRow row in dt.Rows)
                    {
                        clonedTable.ImportRow(row);
                    }
                    dt = clonedTable;
                    DatasetDescription.ColumnDescriptions.First(x => x.Name == column.Name).DataType = typeof(int);
                }
                else if (IsDouble)
                {
                    DataTable clonedTable = dt.Clone();
                    clonedTable.Columns[column.Name].DataType = typeof(double);
                    foreach (DataRow row in dt.Rows)
                    {
                        clonedTable.ImportRow(row);
                    }
                    dt = clonedTable;
                    DatasetDescription.ColumnDescriptions.First(x => x.Name == column.Name).DataType = typeof(double);
                }
            }

            var dsb = new DatasetBase(DatasetDescription);
            dsb.Table = dt;
            return dsb;
        }

        protected IEnumerable<string> LineSplitter(string line)
        {
            int fieldStart = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ',')
                {
                    yield return line.Substring(field‌​Start, i - fieldStart); fieldStart = i + 1;
                }
                if (line[i] == '"')
                    for (i++; line[i] != '"'; i++)
                    {
                    }
            } yield return line.Substring(field‌​Start, line.Length - fieldStart);
        }


    }
}
