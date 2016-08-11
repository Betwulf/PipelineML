using System;
using System.ComponentModel;

namespace PipelineMLCore
{
    public class DataTransformConfigAddColumn : ConfigBase
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DataColumnBase NewColumn { get; set; }

        public string Code { get; set; }

        public DataTransformConfigAddColumn()
        {
            NewColumn = new DataColumnBase();
        }
    }
}
