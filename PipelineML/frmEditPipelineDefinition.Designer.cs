namespace PipelineML
{
    partial class frmEditPipelineDefinition
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditPipelineDefinition));
            this.lblPreProcessTransforms = new System.Windows.Forms.Label();
            this.flwDatasetGen = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDatasetGen = new System.Windows.Forms.Label();
            this.btnCreateDatasetGen = new System.Windows.Forms.Button();
            this.btnEditDatasetGen = new System.Windows.Forms.Button();
            this.flwPreprocess = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddPreprocessTransform = new System.Windows.Forms.Button();
            this.btnEditPreprocessTransform = new System.Windows.Forms.Button();
            this.flwMain = new System.Windows.Forms.FlowLayoutPanel();
            this.flwML = new System.Windows.Forms.FlowLayoutPanel();
            this.lblML = new System.Windows.Forms.Label();
            this.btnAddML = new System.Windows.Forms.Button();
            this.btnEditML = new System.Windows.Forms.Button();
            this.flwPostprocess = new System.Windows.Forms.FlowLayoutPanel();
            this.lblPostprocess = new System.Windows.Forms.Label();
            this.flwEvaluate = new System.Windows.Forms.FlowLayoutPanel();
            this.lblEvaluate = new System.Windows.Forms.Label();
            this.prpGrid = new System.Windows.Forms.PropertyGrid();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblRoot = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtRoot = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.flwDatasetGen.SuspendLayout();
            this.flwPreprocess.SuspendLayout();
            this.flwMain.SuspendLayout();
            this.flwML.SuspendLayout();
            this.flwPostprocess.SuspendLayout();
            this.flwEvaluate.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPreProcessTransforms
            // 
            this.lblPreProcessTransforms.AutoSize = true;
            this.lblPreProcessTransforms.Location = new System.Drawing.Point(3, 0);
            this.lblPreProcessTransforms.Name = "lblPreProcessTransforms";
            this.lblPreProcessTransforms.Size = new System.Drawing.Size(127, 13);
            this.lblPreProcessTransforms.TabIndex = 1;
            this.lblPreProcessTransforms.Text = "2) Preprocess Transforms";
            // 
            // flwDatasetGen
            // 
            this.flwDatasetGen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flwDatasetGen.Controls.Add(this.lblDatasetGen);
            this.flwDatasetGen.Controls.Add(this.btnCreateDatasetGen);
            this.flwDatasetGen.Controls.Add(this.btnEditDatasetGen);
            this.flwDatasetGen.Location = new System.Drawing.Point(3, 3);
            this.flwDatasetGen.Name = "flwDatasetGen";
            this.flwDatasetGen.Size = new System.Drawing.Size(189, 104);
            this.flwDatasetGen.TabIndex = 2;
            // 
            // lblDatasetGen
            // 
            this.lblDatasetGen.AutoSize = true;
            this.lblDatasetGen.Location = new System.Drawing.Point(3, 0);
            this.lblDatasetGen.Name = "lblDatasetGen";
            this.lblDatasetGen.Size = new System.Drawing.Size(106, 13);
            this.lblDatasetGen.TabIndex = 1;
            this.lblDatasetGen.Text = "1) Dataset Generator";
            // 
            // btnCreateDatasetGen
            // 
            this.btnCreateDatasetGen.Location = new System.Drawing.Point(3, 16);
            this.btnCreateDatasetGen.Name = "btnCreateDatasetGen";
            this.btnCreateDatasetGen.Size = new System.Drawing.Size(99, 32);
            this.btnCreateDatasetGen.TabIndex = 2;
            this.btnCreateDatasetGen.Text = "Create...";
            this.btnCreateDatasetGen.UseVisualStyleBackColor = true;
            this.btnCreateDatasetGen.Click += new System.EventHandler(this.btnCreateDatasetGen_Click);
            // 
            // btnEditDatasetGen
            // 
            this.btnEditDatasetGen.Location = new System.Drawing.Point(3, 54);
            this.btnEditDatasetGen.Name = "btnEditDatasetGen";
            this.btnEditDatasetGen.Size = new System.Drawing.Size(99, 32);
            this.btnEditDatasetGen.TabIndex = 3;
            this.btnEditDatasetGen.Text = "Edit...";
            this.btnEditDatasetGen.UseVisualStyleBackColor = true;
            this.btnEditDatasetGen.Click += new System.EventHandler(this.btnEditDatasetGen_Click);
            // 
            // flwPreprocess
            // 
            this.flwPreprocess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flwPreprocess.Controls.Add(this.lblPreProcessTransforms);
            this.flwPreprocess.Controls.Add(this.btnAddPreprocessTransform);
            this.flwPreprocess.Controls.Add(this.btnEditPreprocessTransform);
            this.flwPreprocess.Location = new System.Drawing.Point(198, 3);
            this.flwPreprocess.Name = "flwPreprocess";
            this.flwPreprocess.Size = new System.Drawing.Size(189, 104);
            this.flwPreprocess.TabIndex = 3;
            // 
            // btnAddPreprocessTransform
            // 
            this.btnAddPreprocessTransform.Location = new System.Drawing.Point(3, 16);
            this.btnAddPreprocessTransform.Name = "btnAddPreprocessTransform";
            this.btnAddPreprocessTransform.Size = new System.Drawing.Size(99, 32);
            this.btnAddPreprocessTransform.TabIndex = 4;
            this.btnAddPreprocessTransform.Text = "Add...";
            this.btnAddPreprocessTransform.UseVisualStyleBackColor = true;
            this.btnAddPreprocessTransform.Click += new System.EventHandler(this.btnAddPreprocessTransform_Click);
            // 
            // btnEditPreprocessTransform
            // 
            this.btnEditPreprocessTransform.Location = new System.Drawing.Point(3, 54);
            this.btnEditPreprocessTransform.Name = "btnEditPreprocessTransform";
            this.btnEditPreprocessTransform.Size = new System.Drawing.Size(99, 32);
            this.btnEditPreprocessTransform.TabIndex = 5;
            this.btnEditPreprocessTransform.Text = "Edit...";
            this.btnEditPreprocessTransform.UseVisualStyleBackColor = true;
            this.btnEditPreprocessTransform.Click += new System.EventHandler(this.btnEditPreprocessTransform_Click);
            // 
            // flwMain
            // 
            this.flwMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flwMain.Controls.Add(this.flwDatasetGen);
            this.flwMain.Controls.Add(this.flwPreprocess);
            this.flwMain.Controls.Add(this.flwML);
            this.flwMain.Controls.Add(this.flwPostprocess);
            this.flwMain.Controls.Add(this.flwEvaluate);
            this.flwMain.Location = new System.Drawing.Point(12, 88);
            this.flwMain.Name = "flwMain";
            this.flwMain.Size = new System.Drawing.Size(404, 661);
            this.flwMain.TabIndex = 4;
            // 
            // flwML
            // 
            this.flwML.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flwML.Controls.Add(this.lblML);
            this.flwML.Controls.Add(this.btnAddML);
            this.flwML.Controls.Add(this.btnEditML);
            this.flwML.Location = new System.Drawing.Point(3, 113);
            this.flwML.Name = "flwML";
            this.flwML.Size = new System.Drawing.Size(189, 104);
            this.flwML.TabIndex = 4;
            // 
            // lblML
            // 
            this.lblML.AutoSize = true;
            this.lblML.Location = new System.Drawing.Point(3, 0);
            this.lblML.Name = "lblML";
            this.lblML.Size = new System.Drawing.Size(123, 13);
            this.lblML.TabIndex = 1;
            this.lblML.Text = "3) Machine Learning List";
            // 
            // btnAddML
            // 
            this.btnAddML.Location = new System.Drawing.Point(3, 16);
            this.btnAddML.Name = "btnAddML";
            this.btnAddML.Size = new System.Drawing.Size(99, 32);
            this.btnAddML.TabIndex = 6;
            this.btnAddML.Text = "Add...";
            this.btnAddML.UseVisualStyleBackColor = true;
            this.btnAddML.Click += new System.EventHandler(this.btnAddML_Click);
            // 
            // btnEditML
            // 
            this.btnEditML.Location = new System.Drawing.Point(3, 54);
            this.btnEditML.Name = "btnEditML";
            this.btnEditML.Size = new System.Drawing.Size(99, 32);
            this.btnEditML.TabIndex = 7;
            this.btnEditML.Text = "Edit...";
            this.btnEditML.UseVisualStyleBackColor = true;
            this.btnEditML.Click += new System.EventHandler(this.btnEditML_Click);
            // 
            // flwPostprocess
            // 
            this.flwPostprocess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flwPostprocess.Controls.Add(this.lblPostprocess);
            this.flwPostprocess.Location = new System.Drawing.Point(198, 113);
            this.flwPostprocess.Name = "flwPostprocess";
            this.flwPostprocess.Size = new System.Drawing.Size(189, 104);
            this.flwPostprocess.TabIndex = 5;
            // 
            // lblPostprocess
            // 
            this.lblPostprocess.AutoSize = true;
            this.lblPostprocess.Location = new System.Drawing.Point(3, 0);
            this.lblPostprocess.Name = "lblPostprocess";
            this.lblPostprocess.Size = new System.Drawing.Size(132, 13);
            this.lblPostprocess.TabIndex = 1;
            this.lblPostprocess.Text = "4) Postprocess Transforms";
            // 
            // flwEvaluate
            // 
            this.flwEvaluate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flwEvaluate.Controls.Add(this.lblEvaluate);
            this.flwEvaluate.Location = new System.Drawing.Point(3, 223);
            this.flwEvaluate.Name = "flwEvaluate";
            this.flwEvaluate.Size = new System.Drawing.Size(189, 104);
            this.flwEvaluate.TabIndex = 7;
            // 
            // lblEvaluate
            // 
            this.lblEvaluate.AutoSize = true;
            this.lblEvaluate.Location = new System.Drawing.Point(3, 0);
            this.lblEvaluate.Name = "lblEvaluate";
            this.lblEvaluate.Size = new System.Drawing.Size(69, 13);
            this.lblEvaluate.TabIndex = 1;
            this.lblEvaluate.Text = "5) Evaluators";
            // 
            // prpGrid
            // 
            this.prpGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prpGrid.Location = new System.Drawing.Point(422, 88);
            this.prpGrid.Name = "prpGrid";
            this.prpGrid.Size = new System.Drawing.Size(408, 661);
            this.prpGrid.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(117, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(99, 32);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save...";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(99, 32);
            this.btnOpen.TabIndex = 7;
            this.btnOpen.Text = "Open...";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(35, 67);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 8;
            this.lblName.Text = "Name:";
            // 
            // lblRoot
            // 
            this.lblRoot.AutoSize = true;
            this.lblRoot.Location = new System.Drawing.Point(230, 67);
            this.lblRoot.Name = "lblRoot";
            this.lblRoot.Size = new System.Drawing.Size(33, 13);
            this.lblRoot.TabIndex = 9;
            this.lblRoot.Text = "Root:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(77, 63);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(150, 20);
            this.txtName.TabIndex = 10;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // txtRoot
            // 
            this.txtRoot.Location = new System.Drawing.Point(266, 62);
            this.txtRoot.Name = "txtRoot";
            this.txtRoot.Size = new System.Drawing.Size(150, 20);
            this.txtRoot.TabIndex = 11;
            this.txtRoot.Text = "C:\\";
            this.txtRoot.TextChanged += new System.EventHandler(this.txtRoot_TextChanged);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(222, 12);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(99, 32);
            this.btnRun.TabIndex = 12;
            this.btnRun.Text = "Run...";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // frmEditPipelineDefinition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 761);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.txtRoot);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblRoot);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.prpGrid);
            this.Controls.Add(this.flwMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(650, 600);
            this.Name = "frmEditPipelineDefinition";
            this.Text = "Edit Pipeline Definition";
            this.flwDatasetGen.ResumeLayout(false);
            this.flwDatasetGen.PerformLayout();
            this.flwPreprocess.ResumeLayout(false);
            this.flwPreprocess.PerformLayout();
            this.flwMain.ResumeLayout(false);
            this.flwML.ResumeLayout(false);
            this.flwML.PerformLayout();
            this.flwPostprocess.ResumeLayout(false);
            this.flwPostprocess.PerformLayout();
            this.flwEvaluate.ResumeLayout(false);
            this.flwEvaluate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPreProcessTransforms;
        private System.Windows.Forms.FlowLayoutPanel flwDatasetGen;
        private System.Windows.Forms.Label lblDatasetGen;
        private System.Windows.Forms.FlowLayoutPanel flwPreprocess;
        private System.Windows.Forms.FlowLayoutPanel flwMain;
        private System.Windows.Forms.Button btnCreateDatasetGen;
        private System.Windows.Forms.Button btnEditDatasetGen;
        private System.Windows.Forms.PropertyGrid prpGrid;
        private System.Windows.Forms.FlowLayoutPanel flwML;
        private System.Windows.Forms.Label lblML;
        private System.Windows.Forms.FlowLayoutPanel flwPostprocess;
        private System.Windows.Forms.Label lblPostprocess;
        private System.Windows.Forms.Button btnAddPreprocessTransform;
        private System.Windows.Forms.Button btnEditPreprocessTransform;
        private System.Windows.Forms.FlowLayoutPanel flwEvaluate;
        private System.Windows.Forms.Label lblEvaluate;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblRoot;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtRoot;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnAddML;
        private System.Windows.Forms.Button btnEditML;
    }
}