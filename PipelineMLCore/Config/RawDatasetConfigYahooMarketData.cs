using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class RawDatasetConfigYahooMarketData : ConfigBase
    {

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<string> Symbols { get; set; }

        public RawDatasetConfigYahooMarketData()
        {
            Symbols = new List<string>();
        }
    }
}
