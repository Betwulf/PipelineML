using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DatasetDescriptorBase
    {
        public string Name { get; set; }

        public List<DataColumnBase> ColumnDescriptions { get; set; }


        public DatasetDescriptorBase()
        {
            ColumnDescriptions = new List<DataColumnBase>();
        }

    }
}
