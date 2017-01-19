using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using PipelineMLCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        public class SearchableClassViewModel
        {
            public string FriendlyName { get; set; }

            public string Description { get; set; }

            public Type ClassType { get; set; }

        }


        private List<SearchableClassViewModel> GetPartsList(Type partInterface)
        {
            var TypeList = SearchClasses.SearchForClassesThatImplementInterface(partInterface);
            List<SearchableClassViewModel> partsList = new List<SearchableClassViewModel>();
            foreach (var item in TypeList)
            {
                ISearchableClass iSearchable = Activator.CreateInstance(item) as ISearchableClass;
                partsList.Add(new SearchableClassViewModel() { FriendlyName = iSearchable.FriendlyName, Description = iSearchable.Description, ClassType = item });
            }
            return partsList;
        }

        [TestMethod]
        public void TestSearchableAssemblies()
        {
            var x = new PipelineDefinition();
            Dictionary<string, List<SearchableClassViewModel>> _pipelineParts = null;
            _pipelineParts = new Dictionary<string, List<SearchableClassViewModel>>();
            _pipelineParts.Add(typeof(IDatasetGenerator).ToString(), GetPartsList(typeof(IDatasetGenerator)));
            _pipelineParts.Add(typeof(IDataTransform).ToString(), GetPartsList(typeof(IDataTransform)));
            _pipelineParts.Add(typeof(IMachineLearningProcess).ToString(), GetPartsList(typeof(IMachineLearningProcess)));
            _pipelineParts.Add(typeof(IEvaluator).ToString(), GetPartsList(typeof(IEvaluator)));
            Assert.IsTrue(_pipelineParts[typeof(IDatasetGenerator).ToString()].Count > 0);

        }
    }
}
