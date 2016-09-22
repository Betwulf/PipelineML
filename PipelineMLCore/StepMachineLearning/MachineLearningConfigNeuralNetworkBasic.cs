using Accord.Neuro;
using Accord.Neuro.Learning;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class MachineLearningConfigNeuralNetworkBasic : ConfigBase
    {
        public MachineLearningConfigNeuralNetworkBasic()
        {
            ActivationFunction = typeof(SigmoidFunction);
            ActivationFunctionAlpha = 2.0;
            TrainUntilError = 0.01;
            HiddenLayerNeurons = 4;
            LearningAlgorithm = typeof(BackPropagationLearning);
            LearningRate = 0.2;
            Momentum = 0.1;
            MinTrainingIterations = 100;
            MaxTrainingIterations = 10000;
        }

        /// <summary>
        /// Stop training early if you are better than this error threshold
        /// </summary>
        public double TrainUntilError { get; set; }


        /// <summary>
        /// The maximum number of training iterations, may iterate fewer times depending on error achieved
        /// </summary>
        public int MaxTrainingIterations { get; set; }


        /// <summary>
        /// The maximum number of training iterations, may iterate fewer times depending on error achieved
        /// </summary>
        public int MinTrainingIterations { get; set; }


        /// <summary>
        /// Whether or not to include training data in the results output
        /// </summary>
        public bool IncludeTrainingDataInTestingData { get; set; }

        /// <summary>
        /// The activation Function to use for all neurons
        /// </summary>
        [TypeConverter(typeof(ActivationFunctionTypeConverter))]
        public Type ActivationFunction { get; set; }

        /// <summary>
        /// The Alpha Value for the Activation Function (if needed)
        /// </summary>
        public double ActivationFunctionAlpha { get; set; }

        /// <summary>
        /// The number of neurons in the hidden layer
        /// </summary>
        public int HiddenLayerNeurons { get; set; }


        /// <summary>
        /// Choose Learning Algorithms wisely...
        /// </summary>
        [TypeConverter(typeof(NeuralNetworkTeacherTypeConverter))]
        public Type LearningAlgorithm { get; set; }

        private double learningRate;
        /// <summary>
        /// Choose a value between [0-1] exclusive of the extents
        /// </summary>
        public double LearningRate
        {
            get { return learningRate; }
            set { learningRate = value.RangeLimit(0, 1); }
        }

        private double momentum;
        public double Momentum
        {
            get { return momentum; }
            set { momentum = value.RangeLimit(0, 1); }
        }

    }
}
