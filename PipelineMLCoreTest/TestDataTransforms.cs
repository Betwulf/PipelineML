using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using PipelineMLCore;

namespace PipelineMLCoreTest
{
    [TestClass]
    public class TestDataTransforms
    {
        [TestMethod]
        public void TestAddColumn()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IStorage>().ToMethod(context => new StorageFile(@"C:\Temp\"));

            var dt = new DataTransformAddColumn();
            var dtcfg = new DataTransformConfigAddColumn();
            dtcfg.Code = TestConstants.testCode;
            dtcfg.Name = TestConstants.testName;
            dtcfg.NewColumn = TestConstants.testDataColumnNew;
            dt.Configure(kernel, dtcfg.ToJSON());
            DatasetBase dsIn = TestConstants.GetDatasetBase();
            var result = dt.Transform(dsIn, System.Console.WriteLine);
            Assert.IsTrue(result.Table.Columns.Count == 2);
            Assert.IsTrue((int)result.Table.Rows[0][TestConstants.testColumnNameNew] == 2);
        }

        [TestMethod]
        public void TestCodeStarter()
        {
            var dt = new DataTransformAddColumn();
            var str = dt.CodeStarter;
            Assert.IsNotNull(str);
        }
    }
}
