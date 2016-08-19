using PipelineMLCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCoreTest
{
    public static class TestConstants
    {
        public static string testName = "Test";
        public static string testFile = "TestDataFile.csv";
        public static string titanicFile = "titanic.csv";
        public static string directory = @"C:\Temp\Test";
        public static string currDirectory = @"..\";
        public static string yahooFilename = @"PipelineMLCore.YahooMarketDataSeries\MSFT.json";
        public static string subFolder = @"yahoo\";
        public static DateTime startDate = DateTime.Parse("2014/01/02");
        public static DateTime endDate = DateTime.Parse("2016/01/01");
        public static string testTableName = "TestTable";
        public static string testColumnName = "TestColumn";
        public static string testColumnNameNew = "TestColumnNew";
        public static DataColumnBase testDataColumn = new DataColumnBase() { Name = testColumnName, DataType = typeof(int), Description = testColumnName, Id = 0, IsFeature = false, IsLabel = false };
        public static DataColumnBase testDataColumnNew = new DataColumnBase() { Name = testColumnNameNew, DataType = typeof(int), Description = testColumnNameNew, Id = 1, IsFeature = false, IsLabel = false };
        public static DataTable testDataTable = new DataTable(testTableName);
        public static string testCode = @"var tableOut = new DataTable(); 
                                    tableOut.Columns.Add(" + "\"" + testColumnNameNew + "\"" + @", typeof(int));
                                    for (int i = 0; i < datasetIn.Table.Rows.Count; i++)
                                    {
                                        tableOut.Rows.Add(new object[] { 2 });
                                    }

                                    return tableOut;";

        public static DatasetBase GetDatasetBase()
        {
            var dsd = new DatasetDescriptorBase();
            dsd.ColumnDescriptions.Add(testDataColumn);
            dsd.Name = testTableName;
            var dbb = new DatasetBase(dsd);
            dbb.Table.Rows.Add(new object[] { 1 });
            dbb.Table.Rows.Add(new object[] { 1 });
            return dbb;
        }

        public static List<DataColumnBase> GetTitanicColumnsToRemove()
        {
            var lst = new List<DataColumnBase>();
            lst.Add(new DataColumnBase() { Name = "Cabin", DataType = typeof(string), Description = testColumnName, Id = 10, IsFeature = true, IsLabel = false });
            lst.Add(new DataColumnBase() { Name = "Ticket", DataType = typeof(string), Description = testColumnName, Id = 8, IsFeature = true, IsLabel = false });

            return lst;
        }


        public static List<DataColumnBase> GetTitanicColumnToLabel()
        {
            var lst = new List<DataColumnBase>();
            lst.Add(new DataColumnBase() { Name = "Survived", DataType = typeof(string), Description = testColumnName, Id = 1, IsFeature = false, IsLabel = true });

            return lst;
        }



        public static List<DataColumnBase> GetTitanicColumnsToIgnore()
        {
            var lst = new List<DataColumnBase>();
            lst.Add(new DataColumnBase() { Name = "PassengerId", DataType = typeof(string), Description = testColumnName, Id = 0, IsFeature = false, IsLabel = true });
            lst.Add(new DataColumnBase() { Name = "Name", DataType = typeof(string), Description = testColumnName, Id = 3, IsFeature = false, IsLabel = true });

            return lst;
        }



        public static List<DataColumnBase> GetTitanicColumnWithNullValues()
        {
            var lst = new List<DataColumnBase>();
            lst.Add(new DataColumnBase() { Name = "Age", DataType = typeof(double), Description = testColumnName, Id = 0, IsFeature = true, IsLabel = false, IsCategory = false });

            return lst;
        }
    }
}
