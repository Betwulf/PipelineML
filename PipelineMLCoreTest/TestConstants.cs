using PipelineMLCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static string testTableName = "TestTable";
        public static string testColumnName = "TestColumn";
        public static string testColumnNameNew = "TestColumnNew";
        public static PipelineMLCore.DataColumnBase testDataColumn = new PipelineMLCore.DataColumnBase() { Name = testColumnName, DataType = typeof(int), Description = testColumnName, Id = 0, IsFeature = false, IsLabel = false };
        public static PipelineMLCore.DataColumnBase testDataColumnNew = new PipelineMLCore.DataColumnBase() { Name = testColumnNameNew, DataType = typeof(int), Description = testColumnNameNew, Id = 1, IsFeature = false, IsLabel = false };
        public static DataTable testDataTable = new DataTable(testTableName);
        public static string testCode = @"var tableOut = new DataTable(); 
                                    tableOut.Columns.Add(" + "\"" + testColumnNameNew + "\"" + @", typeof(int));
                                    for (int i = 0; i < datasetIn.Table.Rows.Count; i++)
                                    {
                                        tableOut.Rows.Add(new object[] { 2 });
                                    }

                                    return tableOut;";

        public static IDataset GetIDataset()
        {
            var dsd = new DatasetDescriptor();
            dsd.ColumnNames.Add(testDataColumn);
            dsd.Name = testTableName;
            var dbb = new DatasetBase(dsd);
            dbb.Table.Rows.Add(new object[] { 1 });
            dbb.Table.Rows.Add(new object[] { 1 });
            return dbb;
        }
    }
}
