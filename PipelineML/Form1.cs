using PipelineMLCore;
using System;
using System.IO;
using System.Windows.Forms;

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
            string[] stocks = { "SCTY", "SPY", "GOOG", "NFLX" };
            var dsg = new DatasetGeneratorYahoo { DatasetDescription = new DatasetDescriptor() };
            var dsgconfig = dsg.Config as DatasetConfigYahooMarketData;
            dsgconfig.Name = "Yahoo Market Data";
            dsgconfig.Symbols.AddRange(stocks);
            dsgconfig.StartDate = DateTime.Parse("1/1/2010");
            dsgconfig.EndDate = DateTime.Parse("1/1/2016");
            var dsgCfg = dsg.Config.ToJSON();

            var transform = GetDataTransformConfig();

            var pd = new PipelineDefinition();
            pd.Name = "Test";
            pd.DatasetGenerator = TypeDefinition.Create(dsg);
            var tdpreprocess = TypeDefinition.Create(transform);
            pd.PreprocessDataTransforms.Add(tdpreprocess);
            var jsonPD = dsg.Config.ToJSON();

            IDataTransform remove = (IDataTransform)Activator.CreateInstance(tdpreprocess.ClassType);

            remove.Configure(string.Empty, tdpreprocess.ClassConfig);
            prpGrid.SelectedObject = dsgconfig;

            //File.WriteAllText(pdJsonFilename, jsonPD);

        }

        IDataTransform GetDataTransformConfig()
        {
            var remove = new DataTransformRemoveColumns();
            var dtConfig = remove.Config as DataTransformConfigColumns;
            dtConfig.ColumnNames.Add(new PipelineMLCore.DataColumn()
            {
                DataType = typeof(System.String),
                Description = "",
                Id = 1,
                IsFeature = true,
                IsLabel = false,
                Name = "ID"
            });
            dtConfig.Name = "Remove ID";
            return remove;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            string configJson = File.ReadAllText(pdJsonFilename);
            DatasetConfigYahooMarketData yahooConfig = ConfigBase.FromJSON<DatasetConfigYahooMarketData>(configJson);
            prpGrid.SelectedObject = yahooConfig;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DatasetConfigYahooMarketData pd = prpGrid.SelectedObject as DatasetConfigYahooMarketData;
            var jsonPD = pd.ToJSON();
            File.WriteAllText(pdJsonFilename, jsonPD);

        }

        private void btnLaunchEditor_Click(object sender, EventArgs e)
        {
            var frmEdit = new frmEditPipelineDefinition();
            frmEdit.Show(this);
        }

    }
}
