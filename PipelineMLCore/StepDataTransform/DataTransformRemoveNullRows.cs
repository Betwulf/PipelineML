using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DataTransformRemoveNullRows : IDataTransform, ISearchableClass
    {
        public string Name { get; set; }

        public string FriendlyName { get { return "Remove rows with Null"; } }

        public string Description { get { return "Will remove data rows where selected columns have null values"; } }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        private DataTransformConfigColumns ConfigInternal { get { return Config as DataTransformConfigColumns; } }


        public DataTransformRemoveNullRows()
        {
            Config = new DataTransformConfigColumns();
        }

        public void Configure(string rootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DataTransformConfigColumns>(jsonConfig);
            Name = Config.Name;
        }

        public DatasetBase Transform(DatasetBase datasetIn, Action<string> updateMessage)
        {
            for (int i = datasetIn.Table.Rows.Count-1; i >= 0; i--)
            {
                bool deleteRow = false;
                foreach (var col in ConfigInternal.ColumnNames)
                {
                    if (datasetIn.Table.Rows[i][col.Name] == null)
                        deleteRow = true;
                }
                if (deleteRow) datasetIn.Table.Rows.RemoveAt(i);

            }
            return datasetIn;
        }
    }
}
