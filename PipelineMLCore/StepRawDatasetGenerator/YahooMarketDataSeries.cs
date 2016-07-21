using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;


namespace PipelineMLCore
{
    public class YahooMarketDataSeries : INamed
    {
        public string Name
        {
            get { return Ticker; }
            set { Ticker = value; }
        }

        [JsonProperty(PropertyName = "marketDataList")]
        public List<YahooMarketData> MarketDataList { get; set; }

        [JsonProperty(PropertyName = "ticker")]
        public string Ticker { get; set; }

        
    }
}
