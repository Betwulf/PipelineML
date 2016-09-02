using Accord.Neuro;
using AForge;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace PipelineMLCore
{
    public class MachineLearningNeuralNetworkBasic : MachineLearningBase, ISearchableClass
    {

        public string FriendlyName { get { return "Basic Neural Network"; } }

        public string Description { get { return "Uses a Basic Neural Network algorithm"; } }


        private MachineLearningConfigNeuralNetworkBasic ConfigInternal { get { return Config as MachineLearningConfigNeuralNetworkBasic; } }


        public MachineLearningNeuralNetworkBasic()
        {
            Config = new MachineLearningConfigNeuralNetworkBasic();
        }

        public new void Configure(string rootDirectory, string jsonConfig)
        {
            Config = JsonConvert.DeserializeObject<MachineLearningConfigNeuralNetworkBasic>(jsonConfig);
        }




        public new IMachineLearningResults TrainML(DatasetBase datasetIn, Action<string> updateMessage)
        {
            // ActivationNetwork network = new ActivationNetwork();
            return null;
        }
    }
}
