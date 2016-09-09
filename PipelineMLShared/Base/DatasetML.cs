using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DatasetML : DatasetBase
    {
        public double[][] inputs { get; set; }

        public double[][] labels { get; set; }

        public new DatasetDescriptorML Descriptor { get; set; }

        public int NumberOfFeatures { get; set; }

        public int NumberOfLabels { get; set; }

        public DatasetML(DatasetDescriptorML descriptor) : base(null)
        {
            Descriptor = descriptor;
        }

    }
}
