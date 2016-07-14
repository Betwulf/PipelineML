using PipelineMLCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PipelineML
{
    public partial class frmEditPipelineDefinition : Form
    {
        public string DefaultLoadLocation { get; set; }

        public string DefaultFileExtension { get; set; }


        public PipelineDefinition PipelineDef { get; set; }

        public PipelineInstance PipelineInst { get; set; }
        

        public frmEditPipelineDefinition()
        {
            PipelineDef = new PipelineDefinition();
            PipelineInst = new PipelineInstance();
            InitializeComponent();
            DefaultLoadLocation = @"C:\";
            DefaultFileExtension = "JSON files (*.json)|*.json|All files (*.*)|*.*";
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            string filename;
            if (GetOpenFilenameFromUser(out filename))
            {
                string configJson = File.ReadAllText(filename);
                PipelineDef = PipelineDefinition.FromJSON(configJson, typeof(PipelineDefinition)) as PipelineDefinition;
                PipelineInst = PipelineDef.CreateInstance();
            }
            txtName.Text = PipelineDef.Name;
            txtRoot.Text = PipelineDef.RootFolder;
        }
        public bool GetSaveFilenameFromUser(out string filename)
        {
            filename = null;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = DefaultLoadLocation;
            saveFileDialog1.Filter = DefaultFileExtension;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = saveFileDialog1.FileName;
                return true;
            }
            return false;
        }
        public bool GetOpenFilenameFromUser(out string filename)
        {
            filename = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = DefaultLoadLocation;
            openFileDialog1.Filter = DefaultFileExtension;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                return true;
            }
            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string filename;
            if (GetSaveFilenameFromUser(out filename))
            {
                string configJson = PipelineInst.CreateDefinition().ToJSON();
                File.WriteAllText(filename, configJson);
            }
        }

        private void btnCreateDatasetGen_Click(object sender, EventArgs e)
        {
            var searchForm = new frmSearchForClass();
            searchForm.Initialize(typeof(IRawDatasetGenerator));
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                IRawDatasetGenerator datagen = (IRawDatasetGenerator)Activator.CreateInstance(searchForm.SelectedType);
                PipelineInst.DatasetGenerator = datagen;
                prpGrid.SelectedObject = datagen.Config;
            }
        }

        private void btnEditDatasetGen_Click(object sender, EventArgs e)
        {
            prpGrid.SelectedObject = PipelineInst.DatasetGenerator.Config;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            PipelineInst.Name = txtName.Text;
        }

        private void txtRoot_TextChanged(object sender, EventArgs e)
        {
            PipelineInst.RootFolder = txtRoot.Text;
        }
    }
}
