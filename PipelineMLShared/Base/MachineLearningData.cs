using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class MachineLearningData
    {
        public IDataset Source { get; set; }

        public DatasetDescriptor InputDescriptor { get; set; }

        public string Name { get; set; }


        public double[][] TrainingInputs { get; set; }

        public int[] TrainingLabel { get; set; }


        public double[][] TestingInputs { get; set; }

        public int[] TestingLabel { get; set; }

        /// <summary>
        /// number of possible values in the label series
        /// </summary>
        public int LabelCategoryCount { get; set; }

        public MachineLearningData(IDataset datasetIn)
        {
            Name = datasetIn.Name;
            Source = datasetIn;
            InputDescriptor = new DatasetDescriptor();
        }
    }
}
