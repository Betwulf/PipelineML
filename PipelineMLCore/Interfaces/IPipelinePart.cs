using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public interface IPipelinePart
    {
        string Name { get; set; }


        void Configure(string jsonConfig);

        ConfigBase Config { get; set; }

    }
}
