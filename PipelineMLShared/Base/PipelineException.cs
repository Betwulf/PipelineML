﻿using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class PipelineException : Exception
    {
        public DatasetBase CurrentDataset { get; set; }

        public IPipelinePart CurrentPipelinePart { get; set; }

        public PipelineException(string message, DatasetBase currentDataset, IPipelinePart currentPart) : base(message)
        {
            Log.Logger.Error("{message} {currentDataset} {currentPart}", message, currentDataset, currentPart);
            CurrentDataset = currentDataset;
            CurrentPipelinePart = currentPart;
        }
    }
}
