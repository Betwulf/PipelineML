using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Data;
using Ninject;

namespace PipelineMLCore
{
    public class DataTransformConvertColumnDataType : IDataTransform, ISearchableClass
    {
        public Guid Id { get { return Config.Id; } }

        public string Name { get { return Config.Name; } }

        public string FriendlyName { get { return "Convert Column DataType Data Transform"; } }

        public string Description { get { return "Will try to convert the column into a different datatype"; } }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        private DataTransformConfigColumns ConfigInternal { get { return Config as DataTransformConfigColumns; } }


        public DataTransformConvertColumnDataType()
        {
            Config = new DataTransformConfigColumns();
            Config.Name = "Convert column type";
        }

        public void Configure(IKernel kernel, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DataTransformConfigColumns>(jsonConfig);
        }

        public DatasetBase Transform(DatasetBase datasetIn, Action<string> updateMessage)
        {
            foreach (var col in ConfigInternal.ColumnNames)
            {
                string tempName = Path.GetRandomFileName();
                var found = datasetIn.Descriptor.ColumnDescriptions.First(x => x.Name == col.Name);
                datasetIn.Table.Columns[found.Name].ColumnName = tempName;
                datasetIn.Descriptor.ColumnDescriptions.Remove(found);

                datasetIn.Table.Columns.Add(new DataColumn(col.Name, col.DataType));
                foreach (DataRow row in datasetIn.Table.Rows)
                {
                    row[col.Name] = Convert.ChangeType(row[tempName], col.DataType);
                }
                datasetIn.Table.Columns.Remove(tempName);
                datasetIn.Descriptor.ColumnDescriptions.Add(found);
                found.DataType = col.DataType;
            }
            return datasetIn;
        }
    }
}
