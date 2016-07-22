using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace PipelineMLCore
{
    public class DatasetConfigCSVFile : ConfigBase
    {
        [EditorAttribute(typeof(FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Filepath { get; set; }

        public DatasetConfigCSVFile()
        {
            Name = "CSV File";
        }
    }
}
