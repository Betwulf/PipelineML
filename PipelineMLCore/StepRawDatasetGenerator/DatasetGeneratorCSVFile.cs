using Newtonsoft.Json;
using PipelineMLShared.Base;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;


namespace PipelineMLCore
{
    public class DatasetGeneratorCSVFile : IDatasetGenerator, ISearchableClass
    {
        public string Name { get; set; }

        public IDatasetDescriptor DatasetDescription { get; set; }

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

        public IDataset Generate(Action<string> updateMessage)
        {
            string CSVString;
            DatasetDescription = new DatasetDescriptor();
            DataTable dt = new DataTable();


            if (!File.Exists(ConfigInternal.Filepath)) { return null; }
            {
                CSVString = File.ReadAllText(ConfigInternal.Filepath);
            }
            if (CSVString == null) return null;

            string[] tableData = CSVString.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var col = from cl in tableData[0].Split(",".ToCharArray())
                      select new System.Data.DataColumn(cl);
            dt.Columns.AddRange(col.ToArray());
            col.ToList().ForEach(x =>
            {
                DatasetDescription.ColumnNames.Add(new DataColumn() { Id = x.Ordinal, Name = x.ColumnName, DataType = x.DataType, Description = x.ColumnName, IsFeature = false, IsLabel = false });
            });

            foreach (var item in tableData.Skip(1)) { dt.Rows.Add(item.Split(",".ToCharArray())); }

            var dsb = new DatasetBase(DatasetDescription);
            dsb.Table = dt;
            return dsb;
        }

    }
}
