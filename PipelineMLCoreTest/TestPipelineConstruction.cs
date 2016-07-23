using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PipelineMLCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PipelineMLCoreTest
{
    [TestClass]
    public class TestPipelineConstruction
    {
        [TestMethod]
        public void TestPipelineDefinition()
        {
            string testname = TestConstants.testName;
            string testfile = TestConstants.testFile;
            var pi = new PipelineInstance();
            var dsgcfg = new DatasetConfigCSVFile();
            dsgcfg.Name = testname;
            dsgcfg.Filepath = testfile;
            pi.DatasetGenerator = new DatasetGeneratorCSVFile();
            pi.DatasetGenerator.Configure(TestConstants.currDirectory, dsgcfg.ToJSON());
            var predtcfg = new DataTransformConfigColumns();
            predtcfg.Name = TestConstants.testName;
            predtcfg.ColumnNames.Add(TestConstants.testDataColumn);
            var predt = new DataTransformRemoveColumns();
            predt.Configure(predtcfg.ToJSON());
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
