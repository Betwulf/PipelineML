using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PipelineMLCore;
using System.IO;

namespace PipelineML
{
    public partial class frmMain : Form
    {
        private string pdJsonFilename;

        public frmMain()
        {
            InitializeComponent();
            pdJsonFilename = "pd.json";
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            string[] stocks = new string[] { "SCTY", "SPY", "GOOG", "NFLX" };
            var dsg = new RawDatasetGeneratorYahoo();
            dsg.Config.Name = "Yahoo Market Data";
            dsg.Config.Symbols.AddRange(stocks);
            dsg.Config.StartDate = DateTime.Parse("1/1/2010");
            dsg.Config.EndDate = DateTime.Parse("1/1/2016");
            var cfg = dsg.Config.ToJSON();

            var pd = new PipelineDefinition();
            pd.DatasetGenerator = new TypeDefinition();
            pd.DatasetGenerator.ClassConfig = cfg;
            pd.DatasetGenerator.ClassType = dsg.GetType();
            var jsonPD = pd.ToJSON();

            File.WriteAllText(pdJsonFilename, jsonPD);

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            string pdJson = File.ReadAllText(pdJsonFilename);
            PipelineDefinition pd = ConfigBase.FromJSON(pdJson, typeof(PipelineDefinition)) as PipelineDefinition;
            var pi = pd.CreateInstance();
            prpGrid.SelectedObject = pi.DatasetGenerator;
        }
    }
}
