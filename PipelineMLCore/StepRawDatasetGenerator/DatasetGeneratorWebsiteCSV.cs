using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;

namespace PipelineMLCore
{
    public class DatasetGeneratorWebsiteCSV : IDatasetGenerator, ISearchableClass
    {
        public string Name { get { return Config.Name; } }

        public DatasetDescriptorBase DatasetDescription { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        private DatasetConfigWebsiteCSV ConfigInternal { get { return Config as DatasetConfigWebsiteCSV; } }


        public string FriendlyName { get { return "Website CSV File"; } }

        public string Description { get { return "Will request URL and expect a CSV file response. CSV File MUST have column headers."; } }

        public DatasetGeneratorWebsiteCSV()
        {
            Config = new DatasetConfigWebsiteCSV();

        }

        public void Configure(string rootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DatasetConfigWebsiteCSV>(jsonConfig);
        }

        public DatasetBase Generate(Action<string> updateMessage)
        {
            string jsonString;
            DatasetDescription = new DatasetDescriptorBase();
            DataTable dt = new DataTable();

            using (var client = new WebClient())
            {
                jsonString = client.DownloadString(ConfigInternal.URL);
            }
            if (jsonString == null) return null;


            string[] tableData = jsonString.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var col = from cl in tableData[0].Split(",".ToCharArray())
                      select new System.Data.DataColumn(cl);
            dt.Columns.AddRange(col.ToArray());
            col.ToList().ForEach(x =>
            {
                DatasetDescription.ColumnDescriptions.Add(new DataColumnBase() { Id = x.Ordinal, Name = x.ColumnName, DataType = x.DataType, Description = x.ColumnName, IsFeature = false, IsLabel = false });
            });

            foreach (var item in tableData.Skip(1)) { dt.Rows.Add(item.Split(",".ToCharArray())); }

            var dsb = new DatasetBase(DatasetDescription);
            dsb.Table = dt;
            return dsb;
        }
    }
}
