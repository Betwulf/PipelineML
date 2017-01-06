using Ninject;
using PipelineMLCore;
using System;
using System.IO;
using System.Windows.Forms;

namespace PipelineML
{
    public partial class frmMain : Form
    {
        private IKernel _kernel;

        public frmMain(IKernel kernel)
        {
            _kernel = kernel;
            InitializeComponent();
        }


        private void btnLaunchEditor_Click(object sender, EventArgs e)
        {
            var frmEdit = new frmEditPipelineDefinition(_kernel);
            frmEdit.Show(this);
        }

    }
}
