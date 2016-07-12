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
            dsg.DatasetDescription = new DatasetDescriptor();
            dsg.Config.Name = "Yahoo Market Data";
            dsg.Config.Symbols.AddRange(stocks);
            dsg.Config.StartDate = DateTime.Parse("1/1/2010");
            dsg.Config.EndDate = DateTime.Parse("1/1/2016");
            var dsgCfg = dsg.Config.ToJSON();

            var transformCfg = GetDataTransformConfig();

            var pd = new PipelineDefinition();
            pd.Name = "Test";
            pd.DatasetGenerator = new TypeDefinition();
            pd.DatasetGenerator.ClassConfig = dsgCfg;
            pd.DatasetGenerator.ClassType = dsg.GetType();
            var tdpreprocess = new TypeDefinition() { ClassConfig = transformCfg, ClassType = typeof(DataTransformRemoveColumns) };
            pd.PreprocessDataTransforms.Enqueue(tdpreprocess);
            var jsonPD = dsg.Config.ToJSON();

            IDataTransform remove = (IDataTransform)Activator.CreateInstance(tdpreprocess.ClassType);

            remove.Configure(tdpreprocess.ClassConfig);
            prpGrid.SelectedObject = remove;

            //File.WriteAllText(pdJsonFilename, jsonPD);

        }

        string GetDataTransformConfig()
        {
            var remove = new DataTransformRemoveColumns();
            remove.Config.ColumnNames.Add(new PipelineMLCore.DataColumn() { DataType = typeof(System.String), Description = "",
                Id = 1, IsFeature = true, IsLabel = false, Name = "ID" });
            remove.Config.Name = "Remove ID";
            var configString = remove.Config.ToJSON();
            return configString;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            string configJson = File.ReadAllText(pdJsonFilename);
            RawDatasetConfigYahooMarketData yahooConfig = ConfigBase.FromJSON(configJson, typeof(RawDatasetConfigYahooMarketData)) as RawDatasetConfigYahooMarketData;
            prpGrid.SelectedObject = yahooConfig;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            RawDatasetConfigYahooMarketData pd = prpGrid.SelectedObject as RawDatasetConfigYahooMarketData;
            var jsonPD = pd.ToJSON();
            File.WriteAllText(pdJsonFilename, jsonPD);

        }
    }
}
