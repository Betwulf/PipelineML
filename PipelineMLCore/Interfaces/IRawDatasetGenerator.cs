using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public interface IRawDatasetGenerator
    {
        string Name { get; set; }

        void Configure(string jsonConfig);

        IRawDatasetDescriptor DatasetDescription { get; set; }

        IRawDataset Generate();
    }
}
