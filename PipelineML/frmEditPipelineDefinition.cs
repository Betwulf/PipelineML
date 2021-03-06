﻿using Ninject;
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
        private IKernel _kernel;

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
        

        public frmEditPipelineDefinition(IKernel kernel)
        {
            _kernel = kernel;
            PipelineDef = new PipelineDefinition();
            PipelineDef.Configure(kernel);
            PipelineInst = new PipelineInstance();
            PipelineInst.Configure(kernel);
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
                PipelineDef.Configure(_kernel);
                PipelineInst = PipelineDef.CreateInstance();
            }
            txtName.Text = PipelineDef.Name;
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


        private void btnAddPostProcessTransform_Click(object sender, EventArgs e)
        {
            var searchForm = new frmSearchForClass();
            searchForm.Initialize(typeof(IDataTransform));
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                IDataTransform postProcTransform = (IDataTransform)Activator.CreateInstance(searchForm.SelectedType);
                PipelineInst.PostprocessDataTransforms.Add(postProcTransform);
                prpGrid.SelectedObject = postProcTransform.Config;
            }
        }

        private void btnEditPostProcessTransform_Click(object sender, EventArgs e)
        {
            var wrap = new WrapCollection<IDataTransform>(PipelineInst.PostprocessDataTransforms);
            prpGrid.SelectedObject = wrap;
        }



        private void btnRun_Click(object sender, EventArgs e)
        {
            // Go through the hydration routine to ensure proper config proliferation... 
            var tempPipelineDef = PipelineInst.CreateDefinition();
            var newPipelineInstance = tempPipelineDef.CreateInstance();
            var frmRun = new frmRunPipeline(newPipelineInstance);
            frmRun.WindowState = FormWindowState.Maximized;
            frmRun.ShowDialog();
        }

        private void btnAddML_Click(object sender, EventArgs e)
        {
            var searchForm = new frmSearchForClass();
            searchForm.Initialize(typeof(IMachineLearningProcess));
            if (searchForm.ShowDialog() == DialogResult.OK)
            {
                IMachineLearningProcess mlProcess = (IMachineLearningProcess)Activator.CreateInstance(searchForm.SelectedType);
                PipelineInst.MLList.Add(mlProcess);
                prpGrid.SelectedObject = mlProcess.Config;
            }
        }

        private void btnEditML_Click(object sender, EventArgs e)
        {
            var wrap = new WrapCollection<IMachineLearningProcess>(PipelineInst.MLList);
            prpGrid.SelectedObject = wrap;
        }

    }
}
