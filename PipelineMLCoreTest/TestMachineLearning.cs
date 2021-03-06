﻿using System;
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
    public class TestMachineLearning
    {
        [TestMethod]
        public void TestDecisionTreeWithTitanicData()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IStorage>().ToMethod(context => new StorageFile(@"C:\Temp\"));

            // config dataset
            var cfg = new DatasetConfigCSVFile();
            cfg.Name = TestConstants.testName;
            cfg.Filepath = TestConstants.titanicFile;

            // generate dataset
            var dgy = new DatasetGeneratorCSVFile();
            dgy.Configure(kernel, cfg.ToJSON());
            var rawdata = dgy.Generate(Console.WriteLine);

            // Remove unused columns
            var dtcfg = new DataTransformConfigColumns();
            dtcfg.ColumnNames = TestConstants.GetTitanicColumnsToRemove();
            var dt = new DataTransformRemoveColumns();
            dt.Configure(kernel, dtcfg.ToJSON());
            var columnsRemoved = dt.Transform(rawdata, Console.WriteLine);

            // Set label
            var dtcfg2 = new DataTransformConfigColumns();
            dtcfg2.ColumnNames = TestConstants.GetTitanicColumnToLabel();
            var dt2 = new DataTransformSetLabel();
            dt2.Configure(kernel, dtcfg2.ToJSON());
            var labelAdded = dt2.Transform(columnsRemoved, Console.WriteLine);

            // remove feature flag from name and passenger
            var dtcfg3 = new DataTransformConfigColumns();
            dtcfg3.ColumnNames = TestConstants.GetTitanicColumnsToIgnore();
            var dt3 = new DataTransformRemoveFeature();
            dt3.Configure(kernel, dtcfg3.ToJSON());
            var columnsIgnored = dt3.Transform(labelAdded, Console.WriteLine);

            // remove rows with nulls
            var dtcfg4 = new DataTransformConfigColumns();
            dtcfg4.ColumnNames = TestConstants.GetTitanicColumnWithNullValues();
            var dt4 = new DataTransformRemoveNullRows();
            dt4.Configure(kernel, dtcfg4.ToJSON());
            var nullRowsRemoved = dt4.Transform(columnsIgnored, Console.WriteLine);


            // convertdatacolumn from string to num
            var dtcfg5 = new DataTransformConfigColumns();
            dtcfg5.ColumnNames = TestConstants.GetTitanicColumnWithNullValues();
            var dt5 = new DataTransformConvertColumnDataType();
            dt5.Configure(kernel, dtcfg5.ToJSON());
            var columnConverted = dt5.Transform(nullRowsRemoved, Console.WriteLine);
            

            // set Training data
            var dtcfg6 = new DataTransformConfigSetTraining();
            dtcfg6.PercentOfTrainingData = 0.8;
            var dt6 = new DataTransformSetTraining();
            dt6.Configure(kernel, dtcfg6.ToJSON());
            var trainingDataSet = dt6.Transform(columnConverted, Console.WriteLine);


            // Create Decision Tree
            var treecfg = new MachineLearningConfigDecisionTree();
            treecfg.Name = TestConstants.testName;
            treecfg.IncludeTrainingDataInTestingData = false;
            var tree = new MachineLearningDecisionTree();
            tree.Configure(kernel, treecfg.ToJSON());

            // Train Tree
            var mlResults = tree.TrainML(trainingDataSet, Console.WriteLine);
            Assert.IsTrue(mlResults.Error < 0.4);
            Assert.IsNotNull(tree);
        }
    }
}
