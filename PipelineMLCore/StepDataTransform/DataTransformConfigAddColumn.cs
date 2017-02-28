using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PipelineMLCore
{
    public class DataTransformConfigAddColumn : ConfigBase
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DataColumnBase NewColumn { get; set; }

        [Required]
        public string Code { get; set; }

        public DataTransformConfigAddColumn()
        {
            NewColumn = new DataColumnBase();
        }
    }
}
