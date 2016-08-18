using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DatasetDescriptorML
    {
        public string Name { get; set; }

        public List<DataColumnML> ColumnDescriptions { get; set; }


        public DatasetDescriptorML()
        {
            ColumnDescriptions = new List<DataColumnML>();
        }

    }
}
