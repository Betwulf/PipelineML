using Newtonsoft.Json;
using Ninject;
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
        public Guid Id { get { return Config.Id; } }

        public string Name { get { return Config.Name; } }

        public string FriendlyName { get { return "Remove rows with Null"; } }

        public string Description { get { return "Will remove data rows where selected columns have null values"; } }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        private DataTransformConfigColumns ConfigInternal { get { return Config as DataTransformConfigColumns; } }


        public DataTransformRemoveNullRows()
        {
            Config = new DataTransformConfigColumns();
            Config.Name = "Remove null data rows";
        }

        public void Configure(IKernel kernel, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DataTransformConfigColumns>(jsonConfig);
        }

        public DatasetBase Transform(DatasetBase datasetIn, Action<string> updateMessage)
        {
            for (int i = datasetIn.Table.Rows.Count-1; i >= 0; i--)
            {
                bool deleteRow = false;
                foreach (var col in ConfigInternal.ColumnNames)
                {
                    if (datasetIn.Table.Rows[i][col.Name] == null || datasetIn.Table.Rows[i][col.Name].ToString() == "")
                        deleteRow = true;
                }
                if (deleteRow) datasetIn.Table.Rows.RemoveAt(i);

            }
            return datasetIn;
        }
    }
}
