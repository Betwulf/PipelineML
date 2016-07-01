using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class RawDatasetGeneratorYahoo : IRawDatasetGenerator
    {
        public IRawDatasetDescriptor DatasetDescription { get; set; }

        public RawDatasetConfigYahooMarketData Config { get; set; }

        
        public RawDatasetGeneratorYahoo()
        {
            Config = new RawDatasetConfigYahooMarketData();
        }

        public void Configure(string json)
        {
            Config = JsonConvert.DeserializeObject<RawDatasetConfigYahooMarketData>(json);
        }



        public IRawDataset Generate()
        {
            return null;
        }

    }
}
