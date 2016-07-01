using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class PipelineRun
    {
        private PipelineInstance Instance { get; set; }

        public PipelineRun(PipelineInstance pi)
        {
            Instance = pi;
        }

        public List<PipelineResults> Run()
        {
            return null;
        }
    }
}
