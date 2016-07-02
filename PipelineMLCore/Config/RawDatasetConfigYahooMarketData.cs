﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Design;

namespace PipelineMLCore
{
    public class RawDatasetConfigYahooMarketData : ConfigBase
    {

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor," +
"System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
typeof(UITypeEditor))]
        [TypeConverter(typeof(CsvConverter))]
        public List<string> Symbols { get; set; }

        public RawDatasetConfigYahooMarketData()
        {
            Symbols = new List<string>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
