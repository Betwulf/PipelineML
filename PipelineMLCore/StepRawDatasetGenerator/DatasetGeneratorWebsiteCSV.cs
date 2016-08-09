﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PipelineMLCore.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DatasetGeneratorWebsiteCSV : IDatasetGenerator, ISearchableClass
    {
        public string Name { get; set; }

        public IDatasetDescriptor DatasetDescription { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        private DatasetConfigWebsiteCSV ConfigInternal { get { return Config as DatasetConfigWebsiteCSV; } }

        private JsonRepository<YahooMarketDataSeries> Cache;

        public string FriendlyName { get { return "Website CSV File"; } }

        public string Description { get { return "Will request URL and expect a CSV file response. CSV File MUST have column headers."; } }

        public DatasetGeneratorWebsiteCSV()
        {
            Config = new DatasetConfigWebsiteCSV();

        }

        public void Configure(string RootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DatasetConfigWebsiteCSV>(jsonConfig);
            Name = Config.Name;
        }

        public IDataset Generate(Action<string> updateMessage)
        {
            string jsonString;
            DatasetDescription = new DatasetDescriptor();
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
            col.ToList().ForEach(x => {
                DatasetDescription.ColumnNames.Add(new DataColumn() { Id = x.Ordinal, Name = x.ColumnName, DataType = x.DataType, Description = x.ColumnName, IsFeature = false, IsLabel = false });
            });

            foreach (var item in tableData.Skip(1)) { dt.Rows.Add(item.Split(",".ToCharArray())); }

            var dsb = new DatasetBase(DatasetDescription);
            dsb.Table = dt;
            return dsb;
        }
    }
}