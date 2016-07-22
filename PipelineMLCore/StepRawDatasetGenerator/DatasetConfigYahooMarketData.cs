using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Design;

namespace PipelineMLCore
{
    public class DatasetConfigYahooMarketData : ConfigBase
    {
        /// <summary>
        /// Subfolder for Yahoo Market Data to be cached to disk
        /// </summary>
        public string SubFolder { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
"System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
typeof(UITypeEditor))]
        [TypeConverter(typeof(CsvConverter))]
        public List<string> Symbols { get; set; }

        /// <summary>
        /// Seed the config with default / sample data
        /// </summary>
        public DatasetConfigYahooMarketData()
        {
            Symbols = new List<string>();
            Name = "Yahoo Market Data";
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
