﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineMLCore;

namespace PipelineMLCoreTest
{
    [TestClass]
    public class TestDataTransforms
    {
        [TestMethod]
        public void TestAddColumn()
        {
            var dt = new DataTransformAddColumn();
            var dtcfg = new DataTransformConfigAddColumn();
            dtcfg.Code = TestConstants.testCode;
            dtcfg.Name = TestConstants.testName;
            dtcfg.NewColumn = TestConstants.testDataColumnNew;
            dt.Configure(string.Empty, dtcfg.ToJSON());
            IDataset dsIn = TestConstants.GetIDataset();
            var result = dt.Transform(dsIn);
            Assert.IsTrue(result.Table.Columns.Count == 2);
            Assert.IsTrue((int)result.Table.Rows[0][TestConstants.testColumnNameNew] == 2);
        }
    }
}
