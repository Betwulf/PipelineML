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
    public partial class frmSearchForClass : Form
    {
        public frmSearchForClass()
        {
            InitializeComponent();
        }

        public List<Type> TypeList { get; set; }


        public string SelectedItemText
        {
            get
            {
                if (lvwClasses.SelectedItems.Count > 0)
                    return lvwClasses.SelectedItems[0].Text;
                else return "";
            }
        }

        public Type SelectedType
        {
            get
            {
                if (lvwClasses.SelectedItems.Count > 0)
                {
                    foreach (Type iDataSourceType in TypeList)
                    {
                        if (lvwClasses.SelectedItems[0].Tag == iDataSourceType as object)
                        {
                            return iDataSourceType;
                        }
                    }
                }

                // otherwise
                return null;
            }
        }

        private void frmSearchForClass_Load(object sender, EventArgs e)
        {

            lvwClasses.Columns.Clear();
            if (lvwClasses.View == View.Details)
            {
                lvwClasses.Columns.Add("Name");
                lvwClasses.Columns.Add("Description");
                lvwClasses.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            UpdateListView();
        }

        public void Initialize(Type interfaceType)
        {
            TypeList = SearchClasses.SearchForClassesThatImplementInterface(interfaceType);
        }

        protected void UpdateListView()
        {
            try
            {
                lvwClasses.SelectedItems.Clear();
                lvwClasses.Items.Clear();

                if (TypeList != null)
                {
                    foreach (Type iDataSourceType in TypeList)
                    {
                        ISearchableClass iSearchable = Activator.CreateInstance(iDataSourceType) as ISearchableClass;
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = iSearchable.FriendlyName;
                        lvi.Tag = iDataSourceType;
                        lvi.ToolTipText = iSearchable.Description;
                        lvi.SubItems.Add(iSearchable.Description);
                        lvwClasses.Items.Add(lvi);
                    }
                }
                if (lvwClasses.Items.Count > 0)
                {
                    lvwClasses.SelectedIndices.Add(0);
                }
                lvwClasses.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void lvwClasses_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }
    }
}
