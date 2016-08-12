using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore.StepDataTransform
{
    public class DataTranformSetTraining : IDataTransform, ISearchableClass
    {
        public string Name { get; set; }

        public string FriendlyName { get { return "Set Training Data Transform"; } }

        public string Description { get { return "Designates rows of data that will be used for machine learning training purposes"; } }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConfigBase Config { get; set; }

        private DataTransformConfigSetTraining ConfigInternal { get { return Config as DataTransformConfigSetTraining; } }


        public DataTranformSetTraining()
        {
            Config = new DataTransformConfigSetTraining();
        }

        public void Configure(string rootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<DataTransformConfigSetTraining>(jsonConfig);
            Name = Config.Name;
        }



        public IDataset Transform(IDataset datasetIn)
        {
            // Create new column
            var trainingColumn = new DataColumnBase()
                { Name = "IsTraining", DataType = typeof(bool), Description = "Training Flag",
                IsFeature = false, IsLabel = false, IsScore = false, IsScoreProbability = false,
                IsTraining = true, Id = -1 };
            datasetIn.Descriptor.ColumnDescriptions.Add(trainingColumn);
            datasetIn.Table.Columns.Add(trainingColumn.Name, trainingColumn.DataType);
            Random rnd = new Random(datasetIn.Table.Rows.Count*DateTime.Now.Millisecond);

            // create training values
            for (int i = 0; i < datasetIn.Table.Rows.Count; i++)
            {
                var row = datasetIn.Table.Rows[i];
                row[trainingColumn.Name] = (rnd.NextDouble() < ConfigInternal.PercentOfTrainingData) ? true : false;
            }
            return datasetIn;
        }

    }
}
