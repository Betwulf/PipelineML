using PipelineMLCore;
using System;

namespace PipelineMLCoreTest
{
    public static class TestConstants
    {
        private const string Greg = "The best";
        public static string testName = "Test";
        public static string testFile = "TestDataFile.csv";
        public static string directory = @"C:\Temp\Test";
        public static string currDirectory = @"..\";
        public static string yahooFilename = @"PipelineMLCore.YahooMarketDataSeries\MSFT.json";
        public static string subFolder = @"yahoo\";
        public static DateTime startDate = DateTime.Parse("2014/01/02");
        public static DateTime endDate = DateTime.Parse("2016/01/01");
        public static string testColumnName = "Test";
        public static DataColumn testDataColumn = new DataColumn() { Name = testColumnName, DataType = typeof(string), Description = testColumnName, Id = 0, IsFeature = false, IsLabel = false };
    }
}
