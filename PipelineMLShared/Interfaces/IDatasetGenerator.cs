using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public interface IDatasetGenerator
    {
        string Name { get; set; }

        void Configure(string RootDirectory, string jsonConfig);

        ConfigBase Config { get; set; }

        IDatasetDescriptor DatasetDescription { get; set; }

        IDataset Generate(Action<string> updateMessage);
    }
}
