using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace PipelineMLCore
{
    /// <summary>
    /// Used internally for the Yahoo Data Generator to gather and cache market data for future requests
    /// </summary>
    public class YahooMarketData : IComparable<YahooMarketData>, INamed
    {
        [JsonProperty(PropertyName = "Symbol")]
        public string Ticker { get; set; }

        [JsonProperty(PropertyName = "Date")]
        public DateTime PriceDate { get; set; }

        [JsonProperty(PropertyName = "Adj_Close")]
        public decimal AdjustedClose { get; set; }

        public decimal Close { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public int Volume { get; set; }

        public int CompareTo(YahooMarketData other)
        {
            if (other == null) return 1;
            return PriceDate.CompareTo(other.PriceDate);
        }

        public string Source { get; set; }

        public string Name
        {
            get { return Ticker; }
            set { Ticker = value; }
        }
    }

    public class MarketDataReverseComparer : IComparer<YahooMarketData>
    {
        public int Compare(YahooMarketData x, YahooMarketData y)
        {
            if (x == null) return 1;
            return -x.PriceDate.CompareTo(y.PriceDate);
        }
    }
}
