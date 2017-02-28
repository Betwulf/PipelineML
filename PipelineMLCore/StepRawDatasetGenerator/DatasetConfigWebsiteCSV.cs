using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineMLCore
{
    public class DatasetConfigWebsiteCSV : ConfigBase
    {
        [Required]
        [Url]
        public string URL { get; set; }


        /// <summary>
        /// Seed the config with default / sample data
        /// </summary>
        public DatasetConfigWebsiteCSV()
        {
            Name = "Website CSV";
            URL = "http://forge.scilab.org/index.php/p/rdataset/source/file/368b19abcb4292c56e4f21079f750eb76b325907/csv/datasets/Titanic.csv";
        }
    }
}
