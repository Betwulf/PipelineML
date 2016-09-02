namespace PipelineML
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.prpGrid = new System.Windows.Forms.PropertyGrid();
            this.btnLaunchEditor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // prpGrid
            // 
            this.prpGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prpGrid.Location = new System.Drawing.Point(210, 12);
            this.prpGrid.Name = "prpGrid";
            this.prpGrid.Size = new System.Drawing.Size(537, 431);
            this.prpGrid.TabIndex = 2;
            // 
            // btnLaunchEditor
            // 
            this.btnLaunchEditor.Location = new System.Drawing.Point(12, 12);
            this.btnLaunchEditor.Name = "btnLaunchEditor";
            this.btnLaunchEditor.Size = new System.Drawing.Size(107, 31);
            this.btnLaunchEditor.TabIndex = 4;
            this.btnLaunchEditor.Text = "Edit...";
            this.btnLaunchEditor.UseVisualStyleBackColor = true;
            this.btnLaunchEditor.Click += new System.EventHandler(this.btnLaunchEditor_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 455);
            this.Controls.Add(this.btnLaunchEditor);
            this.Controls.Add(this.prpGrid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "Pipeline ML";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PropertyGrid prpGrid;
        private System.Windows.Forms.Button btnLaunchEditor;
    }
}

