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

        void Configure(string RootDirectory, string jsonConfig);

        ConfigBase Config { get; set; }

        IRawDatasetDescriptor DatasetDescription { get; set; }

        IRawDataset Generate(Action<string> updateMessage);
    }
}
