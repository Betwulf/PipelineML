using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
