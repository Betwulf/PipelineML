using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DataTransformRemoveColumns : IDataTransform, ISearchableClass
    {
        public string Name { get; set; }

        public string FriendlyName { get { return "Remove Columns Data Transform"; } }

        public string Description { get { return "Will remove selected columns from the dataset"; } }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        private DataTransformConfigColumns ConfigInternal { get { return Config as DataTransformConfigColumns; } }


        public DataTransformRemoveColumns()
        {
            Config = new DataTransformConfigColumns();
        }

        public void Configure(string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DataTransformConfigColumns>(jsonConfig);
            Name = Config.Name;
        }

        public IDataset Transform(IDataset datasetIn)
        {
            foreach (var col in ConfigInternal.ColumnNames)
            {
                datasetIn.Descriptor.ColumnNames.RemoveAll(x => x.Name == col.Name);
                for (int i = datasetIn.Table.Columns.Count-1; i >= 0; i--)
                {
                    var tableCol = datasetIn.Table.Columns[i];
                    if (tableCol != null && tableCol.ColumnName == col.Name)
                    {
                        datasetIn.Table.Columns.Remove(tableCol);
                    }
                }
                
            }
            return datasetIn;
        }
    }
}
