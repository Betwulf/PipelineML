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
        public double MaxError { get; set; }

        [TypeConverter(typeof(ActivationFunctionTypeConverter))]
        public Type ActivationFunction { get; set; }

        public int HiddenLayerNeurons { get; set; }

        [TypeConverter(typeof(NeuralNetworkTeacherTypeConverter))]
        public Type LearningAlgorithm { get; set; }

    }
}
