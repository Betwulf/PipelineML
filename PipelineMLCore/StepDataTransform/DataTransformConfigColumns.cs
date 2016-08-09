using System.Collections.Generic;

namespace PipelineMLCore
{
    public class DataTransformConfigColumns : ConfigBase
    {
        public List<DataColumn> ColumnNames { get; set; }


        public DataTransformConfigColumns()
        {
            ColumnNames = new List<DataColumn>();
        }
    }
}
