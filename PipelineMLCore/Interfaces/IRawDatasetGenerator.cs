using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public interface IRawDatasetGenerator
    {
        IRawDatasetDescriptor DatasetDescription { get; set; }

        ConfigBase Config { get; set; }

        void Configure(string jsonConfig);

        IRawDataset Generate();
    }
}
