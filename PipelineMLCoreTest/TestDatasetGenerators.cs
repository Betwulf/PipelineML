using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineMLCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PipelineMLCoreTest
{
    [TestClass]
    public class TestDatasetGenerators
    {
        [TestMethod]
        public void TestDatasetConfigYahooMarketData()
        {
            string directory = @"C:\Temp\Test";
            string filename = @"PipelineMLCore.YahooMarketDataSeries\MSFT.json";
            string subfolder = @"yahoo\";
            var cfg = new DatasetConfigYahooMarketData();
            cfg.StartDate = DateTime.Parse("2014/01/02");
            cfg.EndDate = DateTime.Parse("2016/01/01");
            cfg.SubFolder = subfolder;
            cfg.Symbols = new List<string>() { "MSFT" };
            cfg.Name = "Test";

            var dgy = new DatasetGeneratorYahoo();
            dgy.Configure(directory, cfg.ToJSON());
            var result = dgy.Generate(Console.WriteLine);

            string filepath = Path.Combine(directory, subfolder, filename);
            Console.WriteLine(filepath);
            Assert.IsTrue(result.Table.Columns.Count == 9);
            Assert.IsTrue(Directory.Exists(directory));
            Assert.IsTrue(File.Exists(filepath));
        }


        [TestMethod]
        public void TestDatasetConfigCSVFile()
        {
            var cfg = new DatasetConfigCSVFile();
            cfg.Name = "Test";
            cfg.Filepath = "TestDataFile.csv";

            var dgy = new DatasetGeneratorCSVFile();
            dgy.Configure(@"..\", cfg.ToJSON());
            var result = dgy.Generate(Console.WriteLine);

            Assert.IsTrue(result.Table.Columns.Count == 2);
            Assert.IsTrue(result.Table.Rows.Count == 2);
            Assert.IsTrue(result.Table.Columns[0].ColumnName == "Test");
        }
    }
}
