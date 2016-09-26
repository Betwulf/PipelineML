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
            string dir = TestConstants.directory;
            string filename = TestConstants.yahooFilename;
            string subfolder = TestConstants.subFolder;
            var cfg = new DatasetConfigYahooMarketData();
            cfg.StartDate = TestConstants.startDate;
            cfg.EndDate = TestConstants.endDate;
            cfg.SubFolder = subfolder;
            cfg.Symbols = new List<string>() { TestConstants.yahooStock };
            cfg.Name = TestConstants.testName;

            var dgy = new DatasetGeneratorYahoo();
            dgy.Configure(dir, cfg.ToJSON());
            var result = dgy.Generate(Console.WriteLine);

            string filepath = Path.Combine(dir, subfolder, filename);
            Console.WriteLine(filepath);
            Assert.IsTrue(result.Table.Columns.Count == 9);
            Assert.IsTrue(Directory.Exists(dir));
            Assert.IsTrue(File.Exists(filepath));
        }


        [TestMethod]
        public void TestDatasetConfigCSVFile()
        {
            var cfg = new DatasetConfigCSVFile();
            cfg.Name = TestConstants.testName;
            cfg.Filepath = TestConstants.testFile;

            var dgy = new DatasetGeneratorCSVFile();
            dgy.Configure(TestConstants.currDirectory, cfg.ToJSON());
            var result = dgy.Generate(Console.WriteLine);

            Assert.IsTrue(result.Table.Columns.Count == 2);
            Assert.IsTrue(result.Table.Rows.Count == 2);
            Assert.IsTrue(result.Table.Columns[0].ColumnName == TestConstants.testColumnName);
        }
    }
}
