﻿namespace PipelineML
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
            this.btnTest = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.prpGrid = new System.Windows.Forms.PropertyGrid();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLaunchEditor = new System.Windows.Forms.Button();
            this.lblTest = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(13, 13);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(107, 31);
            this.btnTest.TabIndex = 0;
            this.btnTest.Text = "button1";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(13, 50);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(107, 31);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
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
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(13, 87);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(107, 31);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLaunchEditor
            // 
            this.btnLaunchEditor.Location = new System.Drawing.Point(13, 124);
            this.btnLaunchEditor.Name = "btnLaunchEditor";
            this.btnLaunchEditor.Size = new System.Drawing.Size(107, 31);
            this.btnLaunchEditor.TabIndex = 4;
            this.btnLaunchEditor.Text = "Edit...";
            this.btnLaunchEditor.UseVisualStyleBackColor = true;
            this.btnLaunchEditor.Click += new System.EventHandler(this.btnLaunchEditor_Click);
            // 
            // lblTest
            // 
            this.lblTest.AutoSize = true;
            this.lblTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTest.Location = new System.Drawing.Point(127, 17);
            this.lblTest.Name = "lblTest";
            this.lblTest.Size = new System.Drawing.Size(40, 20);
            this.lblTest.TabIndex = 5;
            this.lblTest.Text = "Test";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 455);
            this.Controls.Add(this.lblTest);
            this.Controls.Add(this.btnLaunchEditor);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.prpGrid);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnTest);
            this.Name = "frmMain";
            this.Text = "Pipeline ML";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.PropertyGrid prpGrid;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLaunchEditor;
        private System.Windows.Forms.Label lblTest;
    }
}

