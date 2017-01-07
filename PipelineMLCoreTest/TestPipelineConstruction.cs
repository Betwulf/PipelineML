using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using PipelineMLCore;
using System.Linq;

namespace PipelineMLCoreTest
{
    [TestClass]
    public class TestPipelineConstruction
    {
        [TestMethod]
        public void TestPipelineDefinition()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IStorage>().To<StorageFile>();

            string testname = TestConstants.testName;
            string testfile = TestConstants.testFile;

            var pi = new PipelineInstance();
            pi.Configure(kernel);

            var dsgcfg = new DatasetConfigCSVFile
            {
                Name = testname,
                Filepath = testfile
            };
            pi.DatasetGenerator = new DatasetGeneratorCSVFile();
            pi.DatasetGenerator.Configure(kernel, dsgcfg.ToJSON());
            var predtcfg = new DataTransformConfigColumns { Name = TestConstants.testName };
            predtcfg.ColumnNames.Add(TestConstants.testDataColumn);
            var predt = new DataTransformRemoveColumns();
            predt.Configure(kernel, predtcfg.ToJSON());
            pi.PreprocessDataTransforms.Add(predt);

            // convert to definition
            var pd = pi.CreateDefinition();

            // convert back
            var pi2 = pd.CreateInstance();

            // Prove before and after are same
            Assert.IsInstanceOfType(pi2.DatasetGenerator, pi.DatasetGenerator.GetType());
            Assert.IsInstanceOfType(pi2.PreprocessDataTransforms.First(), pi.PreprocessDataTransforms.First().GetType());
        }
    }
}
