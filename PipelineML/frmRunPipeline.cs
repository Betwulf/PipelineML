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
using System.Threading;


namespace PipelineML
{
    public partial class frmRunPipeline : Form
    {
        private readonly SynchronizationContext synchronizationContext;

        public frmRunPipeline(PipelineInstance pi)
        {
            synchronizationContext = SynchronizationContext.Current;
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
            btnRun.Enabled = false;
            Task.Run(() => { run.Run(UpdateTextout); });
        }



        private void UpdateTextout(string output)
        {
            Console.WriteLine("UpdateUI: " + output);
            synchronizationContext.Post(new SendOrPostCallback(p =>
            {
                txtRunOutput.AppendText(p.ToString() + Environment.NewLine);
            }), output);
        }

    }
}
