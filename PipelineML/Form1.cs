using PipelineMLCore;
using System;
using System.IO;
using System.Windows.Forms;

namespace PipelineML
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }


        private void btnLaunchEditor_Click(object sender, EventArgs e)
        {
            var frmEdit = new frmEditPipelineDefinition();
            frmEdit.Show(this);
        }

    }
}
