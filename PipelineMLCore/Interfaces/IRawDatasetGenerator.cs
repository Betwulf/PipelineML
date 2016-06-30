using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLInterfaces
{
    public interface IRawDatasetGenerator
    {
        IRawDatasetDescriptor DatasetDescription { get; set; }

        public int MyProperty { get; set; }

        IRawDataset Generate();
    }
}
