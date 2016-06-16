using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLInterfaces
{
    public class DataColumn
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Type DataType { get; set; }

        public string Description { get; set; }

        public bool IsFeature { get; set; }

        public bool IsLabel { get; set; }
    }
}
