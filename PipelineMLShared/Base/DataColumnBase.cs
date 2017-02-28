using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DataColumnBase
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [TypeConverter(typeof(TypeTypeConverter))] 
        public Type DataType { get; set; }

        public string Description { get; set; }

        public bool IsFeature { get; set; }

        public bool IsLabel { get; set; }

        public bool IsTraining { get; set; }

        public bool IsScore { get; set; }

        public bool IsScoreProbability { get; set; }

        public bool IsCategory { get; set; }

    }
}
