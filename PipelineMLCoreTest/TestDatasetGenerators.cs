using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineMLCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ninject;

namespace PipelineMLCoreTest
{
    [TestClass]
    public class TestDatasetGenerators
    {
        [TestMethod]
        public void TestDatasetConfigYahooMarketData()
        {
            // ninject config for test
            IKernel kernel = new StandardKernel();
            kernel.Bind<IStorage>().To<StorageFile>();
            

            string filename = TestConstants.yahooFilename;
            string subfolder = TestConstants.subFolder;
            var cfg = new DatasetConfigYahooMarketData();
            cfg.StartDate = TestConstants.startDate;
            cfg.EndDate = TestConstants.endDate;
            cfg.SubFolder = subfolder;
            cfg.Symbols = new List<string>() { TestConstants.yahooStock };
            cfg.Name = TestConstants.testName;

            
            var dgy = new DatasetGeneratorYahoo();
            dgy.Configure(kernel, cfg.ToJSON());
            var result = dgy.Generate(Console.WriteLine);

            Assert.IsTrue(result.Table.Columns.Count == 9);
        }


        [TestMethod]
        public void TestDatasetConfigCSVFile()
        {
            // ninject config for test
            IKernel kernel = new StandardKernel();
            kernel.Bind<IStorage>().To<StorageFile>();

            var cfg = new DatasetConfigCSVFile();
            cfg.Name = TestConstants.testName;
            cfg.Filepath = TestConstants.testFile;

            var dgy = new DatasetGeneratorCSVFile();
            dgy.Configure(kernel, cfg.ToJSON());
            var result = dgy.Generate(Console.WriteLine);

            Assert.IsTrue(result.Table.Columns.Count == 2);
            Assert.IsTrue(result.Table.Rows.Count == 2);
            Assert.IsTrue(result.Table.Columns[0].ColumnName == TestConstants.testColumnName);
        }
    }
}
