using PipelineMLCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PipelineML
{
    public partial class frmRunPipeline : Form
    {
        public frmRunPipeline(PipelineInstance pi)
        {
            run = new PipelineRun(pi);
            Results = new PipelineResults();
            InitializeComponent();
        }

        public PipelineRun run { get; set; }

        public PipelineResults Results { get; set; }
        

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            run.Run(Console.WriteLine);
        }
    }
}
