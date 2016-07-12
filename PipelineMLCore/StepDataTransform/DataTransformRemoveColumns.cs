using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DataTransformRemoveColumns : IDataTransform
    {
        public string Name { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DataTransformConfigColumns Config { get; set; }

        public DataTransformRemoveColumns()
        {
            Config = new DataTransformConfigColumns();
        }

        public void Configure(string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DataTransformConfigColumns>(jsonConfig);
            Name = Config.Name;
        }

        public IRawDataset Transform(IRawDataset datasetIn)
        {
            foreach (var col in Config.ColumnNames)
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
