using System.Collections.Generic;

namespace PipelineMLCore
{
    public class DataTransformConfigColumns : ConfigBase
    {
        public List<DataColumnBase> ColumnNames { get; set; }


        public DataTransformConfigColumns()
        {
            ColumnNames = new List<DataColumnBase>();
        }
    }
}
