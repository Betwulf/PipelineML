using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class PipelineException : Exception
    {
        public IDataset CurrentDataset { get; set; }

        public IPipelinePart CurrentPipelinePart { get; set; }

        public PipelineException(string message, IDataset currentDataset, IPipelinePart currentPart) : base(message)
        {
            CurrentDataset = currentDataset;
            CurrentPipelinePart = currentPart;
        }
    }
}
