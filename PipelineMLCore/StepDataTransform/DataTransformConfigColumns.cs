using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PipelineMLCore
{
    public class DataTransformConfigColumns : ConfigBase
    {
        [Required]
        public List<DataColumnBase> ColumnNames { get; set; }


        public DataTransformConfigColumns()
        {
            ColumnNames = new List<DataColumnBase>();
        }
    }
}
