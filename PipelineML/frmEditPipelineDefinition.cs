using PipelineMLCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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
        public class WrapCollection<T>
        {
            public WrapCollection(List<T> collection)
            {
                Collection = collection;
            }
            [TypeConverter(typeof(CollectionEditor))]
            public List<T> Collection { get; set; }
        }

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
                PipelineDef = ConfigBase.FromJSON<PipelineDefinition>(configJson);
                PipelineInst = PipelineDef.CreateInstance();
            }
            txtName.Text = PipelineDef.Name;
            txtRoot.Text = PipelineDef.RootDirectory;
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
            searchForm.Initialize(typeof(IDatasetGenerator));
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                IDatasetGenerator datagen = (IDatasetGenerator)Activator.CreateInstance(searchForm.SelectedType);
                PipelineInst.DatasetGenerator = datagen;
                prpGrid.SelectedObject = datagen.Config;
            }
        }

        private void btnEditDatasetGen_Click(object sender, EventArgs e)
        {
            prpGrid.SelectedObject = PipelineInst.DatasetGenerator?.Config;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            PipelineInst.Name = txtName.Text;
        }

        private void txtRoot_TextChanged(object sender, EventArgs e)
        {
            PipelineInst.RootDirectory = txtRoot.Text;
        }

        private void btnAddPreprocessTransform_Click(object sender, EventArgs e)
        {
            var searchForm = new frmSearchForClass();
            searchForm.Initialize(typeof(IDataTransform));
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                IDataTransform preprocTransform = (IDataTransform)Activator.CreateInstance(searchForm.SelectedType);
                PipelineInst.PreprocessDataTransforms.Add(preprocTransform);
                prpGrid.SelectedObject = preprocTransform.Config;
            }
        }

        private void btnEditPreprocessTransform_Click(object sender, EventArgs e)
        {
            var wrap = new WrapCollection<IDataTransform>(PipelineInst.PreprocessDataTransforms);
            prpGrid.SelectedObject = wrap;
        }



        private void btnRun_Click(object sender, EventArgs e)
        {
            var frmRun = new frmRunPipeline(PipelineInst);
            frmRun.ShowDialog();
        }
    }
}
