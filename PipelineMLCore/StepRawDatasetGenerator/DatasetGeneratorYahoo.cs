using Serilog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace PipelineMLCore
{
    public class DatasetGeneratorYahoo : IDatasetGenerator, ISearchableClass
    {
        public string Name { get { return Config.Name; } }

        public DatasetDescriptorBase DatasetDescription { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        private DatasetConfigYahooMarketData ConfigInternal { get { return Config as DatasetConfigYahooMarketData; } }

        private JsonRepository<YahooMarketDataSeries> cache;

        public string FriendlyName { get { return "Yahoo Data Generator"; } }

        public string Description { get { return "Will call and cache security information from Yahoo"; } }

        /// <summary>
        /// The max number of months Yahoo will let you request at a time
        /// </summary>
        private const int MonthsAtATime = 14;

        public DatasetGeneratorYahoo()
        {
            Config = new DatasetConfigYahooMarketData();
        }

        public void Configure(string rootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DatasetConfigYahooMarketData>(jsonConfig);
            string fullPath = Path.Combine(rootDirectory, ConfigInternal.SubFolder);
            cache = new JsonRepository<YahooMarketDataSeries>(fullPath);
        }

        public override string ToString()
        {
            return Name;
        }

        public DatasetBase Generate(Action<string> updateMessage)
        {
            DatasetDescription = new DatasetDescriptorBase();
            DatasetDescription.Name = "Yahoo Financial Market Data";
            DatasetDescription.ColumnDescriptions.Add(new DataColumnBase() { Id = 1, Name = "Ticker", DataType = typeof(string), Description = "The stock ticker", IsFeature = false, IsLabel = false });
            DatasetDescription.ColumnDescriptions.Add(new DataColumnBase() { Id = 2, Name = "Date", DataType = typeof(DateTime), Description = "Date of the prices", IsFeature = false, IsLabel = false });
            DatasetDescription.ColumnDescriptions.Add(new DataColumnBase() { Id = 3, Name = "AdjustedClose", DataType = typeof(decimal), Description = "Adjusted Close", IsFeature = true, IsLabel = false });
            DatasetDescription.ColumnDescriptions.Add(new DataColumnBase() { Id = 4, Name = "Close", DataType = typeof(decimal), Description = "Close", IsFeature = true, IsLabel = false });
            DatasetDescription.ColumnDescriptions.Add(new DataColumnBase() { Id = 5, Name = "High", DataType = typeof(decimal), Description = "High", IsFeature = true, IsLabel = false });
            DatasetDescription.ColumnDescriptions.Add(new DataColumnBase() { Id = 6, Name = "Open", DataType = typeof(decimal), Description = "Open", IsFeature = true, IsLabel = false });
            DatasetDescription.ColumnDescriptions.Add(new DataColumnBase() { Id = 7, Name = "Low", DataType = typeof(decimal), Description = "Low", IsFeature = true, IsLabel = false });
            DatasetDescription.ColumnDescriptions.Add(new DataColumnBase() { Id = 8, Name = "Volume", DataType = typeof(int), Description = "Volume", IsFeature = true, IsLabel = false });
            DatasetDescription.ColumnDescriptions.Add(new DataColumnBase() { Id = 9, Name = "Source", DataType = typeof(string), Description = "Source of the Data", IsFeature = false, IsLabel = false });
            var ds = new DatasetBase(DatasetDescription);

            var dates = GetTimeSegmentsFromConfig();
            var allData = new List<YahooMarketData>();
            foreach (var ticker in ConfigInternal.Symbols)
            {
                var tickerData = new List<YahooMarketData>();
                for (int i = 1; i < dates.Count; i++)
                {
                    try
                    {
                        var newData = GetYahooMarketData(dates[i - 1], dates[i], ticker, updateMessage);
                        tickerData.AddRange(newData);
                    }
                    catch (Exception ex)
                    {
                        updateMessage(ex.Message);
                    }
                }
                if (tickerData.Count > 0)
                {
                    AddToCache(tickerData, ticker, updateMessage);
                    allData.AddRange(tickerData);
                }
            }
            return ConvertListToDataTable(allData);
        }


        private async void AddToCache(List<YahooMarketData> tickerData, string ticker, Action<string> updateMessage)
        {
            var cachedData = cache.GetById(ticker);
            var minDate = tickerData.Min(x => x.PriceDate);
            var maxDate = tickerData.Max(x => x.PriceDate);
            // check if there is cached data
            if (cachedData == null || cachedData.MarketDataList.Min(x => x.PriceDate.Year == 1))
            {
                // then save what we have!
                var newCachedData = new YahooMarketDataSeries() { Ticker = ticker, MarketDataList = tickerData };
                await cache.CreateAsync(newCachedData, updateMessage);
                return;
            }
            var minCacheDate = cachedData.MarketDataList.Min(x => x.PriceDate);
            var maxCacheDate = cachedData.MarketDataList.Max(x => x.PriceDate);
            // First check to see if cache already has it all. if so then exit
            if (minCacheDate < minDate && maxCacheDate > maxDate) return;

            // now see where the overlap is, first if new data is in the future
            List<YahooMarketData> historicalData = null;
            List<YahooMarketData> futureData = null;
            if (maxDate > maxCacheDate)
            {
                historicalData = cachedData.MarketDataList;
                futureData = tickerData;
            }
            else if (minDate < minCacheDate)
            {
                historicalData = tickerData;
                futureData = cachedData.MarketDataList;
            }

            // Now that we have the old and new dataset figured out, adjust market data
            if (historicalData != null)
            {
                historicalData.Sort();
                futureData.Sort();
                var lastMarketData = historicalData.Last();
                var overlap = futureData.Find(x => x.PriceDate == lastMarketData.PriceDate);

                if (overlap == null)
                { throw new ArgumentException("AddToCache - requires overlapped records to correctly calc adjustment"); }
                if (overlap.AdjustedClose != lastMarketData.AdjustedClose)
                {
                    // then adjustment is needed
                    decimal overlapAdjustValue = overlap.Close / overlap.AdjustedClose;
                    foreach (var item in historicalData)
                    {
                        item.AdjustedClose = item.AdjustedClose / overlapAdjustValue;
                    }
                }
                // remove overlap(s)
                futureData.RemoveAll(x => x.PriceDate <= lastMarketData.PriceDate);
                historicalData.AddRange(futureData);
                cachedData.MarketDataList = historicalData;
                await cache.UpdateAsync(ticker, cachedData, updateMessage);
            }
        }


        private List<YahooMarketData> GetYahooMarketData(DateTime startdate, DateTime enddate, string ticker, Action<string> updateMessage)
        {
            string startDateString = GetYahooDate(startdate);
            string endDateString = GetYahooDate(enddate);

            var cachedData = cache.GetById(ticker);
            if (cachedData != null)
            {
                // then let's try to get data from Cache - check range
                var minDate = cachedData.MarketDataList.Min(x => x.PriceDate);
                var maxDate = cachedData.MarketDataList.Max(x => x.PriceDate);
                if (startdate >= minDate && enddate <= maxDate)
                {
                    // Then cache contains the date range! 
                    // TODO: I could try to check for overlaps between a subset of the cache values and 
                    // what the requested date range is... but not now.
                    updateMessage($"Yahoo found cached data for {ticker} between {startdate} and {enddate}");
                    return cachedData.MarketDataList;
                }
            }

            StringBuilder theWebAddress = new StringBuilder();
            theWebAddress.Append("http://query.yahooapis.com/v1/public/yql?");

            string yquery = "select * from yahoo.finance.historicaldata where symbol = '" + ticker + "' and startDate = '" + startDateString + "' and endDate = '" + endDateString + "'";
            theWebAddress.Append("q=" + System.Web.HttpUtility.UrlEncode(yquery));
            theWebAddress.Append("&format=json");
            theWebAddress.Append("&diagnostics=true");
            theWebAddress.Append("&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");
            theWebAddress.Append("&callback=");

            YahooWait(updateMessage);
            using (var client = new WebClient())
            {
                updateMessage($"Yahoo Call to get historical data for {ticker} between {startdate} and {enddate}");
                var json = client.DownloadString(theWebAddress.ToString());
                JObject dataObject = JObject.Parse(json);
                int queryCount = (int)dataObject["query"]["count"];
                if (queryCount == 0)
                {
                    string yahooWarning = (string)dataObject["query"]["diagnostics"]["warning"];
                    Log.Logger.Warning("Yahoo {warning}", yahooWarning);
                    throw new Exception($"Yahoo Warning: {yahooWarning}");
                }
                else
                {
                    JArray jsonArray = (JArray)dataObject["query"]["results"]["quote"];
                    string smallJson = jsonArray.ToString();

                    List<YahooMarketData> newList = JsonConvert.DeserializeObject<List<YahooMarketData>>(smallJson);
                    newList.ForEach(x => x.Source = "Yahoo");
                    if (newList.Any(x => x.PriceDate.Year == 1))
                    {
                        Log.Logger.Warning("Yahoo {ticker} - {warning}", ticker, "Found Bad data returned from Yahoo. Failing the call. Try again.");
                        throw new Exception($"Yahoo Warning: Found Bad data for {ticker} returned from Yahoo. Failing the call. Try again.");
                    }
                    return newList;
                }
            }
        }





        private DatasetBase ConvertListToDataTable(List<YahooMarketData> list)
        {
            var dt = new DatasetBase(DatasetDescription);
            list.ForEach(x =>
            {
                var row = dt.Table.NewRow();
                row["Ticker"] = x.Ticker;
                row["Date"] = x.PriceDate;
                row["AdjustedClose"] = x.AdjustedClose;
                row["Close"] = x.Close;
                row["High"] = x.High;
                row["Open"] = x.Open;
                row["Low"] = x.Low;
                row["Volume"] = x.Volume;
                row["Source"] = x.Source;
                dt.Table.Rows.Add(row);
            });
            return dt;
        }



        private List<DateTime> GetTimeSegmentsFromConfig()
        {
            var list = new List<DateTime>();
            list.Add(ConfigInternal.StartDate);
            while (list.Last().AddMonths(MonthsAtATime) < ConfigInternal.EndDate)
            {
                list.Add(list.Last().AddMonths(MonthsAtATime));
            }
            list.Add(ConfigInternal.EndDate);
            return list;
        }





        private void YahooWait(Action<string> updateMessage)
        {
            // Wait so Yahoo doesn't get mad
            var rnd = new Random();
            var delayTime = rnd.Next(3000, 6000);
            updateMessage($"Yahoo requires a delay for: {delayTime}");
            Thread.Sleep(delayTime);
        }


        private string GetYahooDate(DateTime date)
        {
            string aMonth = date.Month.ToString().PadLeft(2, '0');
            string aDay = date.Day.ToString().PadLeft(2, '0');
            return date.Year + "-" + aMonth + "-" + aDay;
        }

    }
}
