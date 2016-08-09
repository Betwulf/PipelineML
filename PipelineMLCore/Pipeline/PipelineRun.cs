using System.Collections.Generic;

namespace PipelineMLCore
{
    public class PipelineRun
    {
        private PipelineInstance Instance { get; }

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
