namespace Move_Files
{
    partial class FormCSVMerge
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
            this.textBoxCSVFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnChooseCSVFolder = new System.Windows.Forms.Button();
            this.btnChooseOutFolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxOutFile = new System.Windows.Forms.TextBox();
            this.btnMerge = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.FolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // textBoxCSVFolder
            // 
            this.textBoxCSVFolder.Location = new System.Drawing.Point(83, 26);
            this.textBoxCSVFolder.Name = "textBoxCSVFolder";
            this.textBoxCSVFolder.Size = new System.Drawing.Size(280, 20);
            this.textBoxCSVFolder.TabIndex = 7;
            this.textBoxCSVFolder.TextChanged += new System.EventHandler(this.textBoxCSVFolder_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "CSV Dir : ";
            // 
            // btnChooseCSVFolder
            // 
            this.btnChooseCSVFolder.AutoSize = true;
            this.btnChooseCSVFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseCSVFolder.Location = new System.Drawing.Point(386, 24);
            this.btnChooseCSVFolder.Name = "btnChooseCSVFolder";
            this.btnChooseCSVFolder.Size = new System.Drawing.Size(29, 23);
            this.btnChooseCSVFolder.TabIndex = 8;
            this.btnChooseCSVFolder.Text = "...";
            this.btnChooseCSVFolder.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnChooseCSVFolder.UseVisualStyleBackColor = true;
            this.btnChooseCSVFolder.Click += new System.EventHandler(this.btnChooseCSVFolder_Click);
            // 
            // btnChooseOutFolder
            // 
            this.btnChooseOutFolder.AutoSize = true;
            this.btnChooseOutFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseOutFolder.Location = new System.Drawing.Point(386, 62);
            this.btnChooseOutFolder.Name = "btnChooseOutFolder";
            this.btnChooseOutFolder.Size = new System.Drawing.Size(29, 23);
            this.btnChooseOutFolder.TabIndex = 8;
            this.btnChooseOutFolder.Text = "...";
            this.btnChooseOutFolder.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnChooseOutFolder.UseVisualStyleBackColor = true;
            this.btnChooseOutFolder.Click += new System.EventHandler(this.btnChooseOutFolder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Output Dir : ";
            // 
            // textBoxOutFile
            // 
            this.textBoxOutFile.Location = new System.Drawing.Point(83, 64);
            this.textBoxOutFile.Name = "textBoxOutFile";
            this.textBoxOutFile.Size = new System.Drawing.Size(280, 20);
            this.textBoxOutFile.TabIndex = 7;
            this.textBoxOutFile.TextChanged += new System.EventHandler(this.textBoxOutFile_TextChanged);
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(83, 100);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(75, 23);
            this.btnMerge.TabIndex = 9;
            this.btnMerge.Text = "&Merge";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(173, 100);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormCSVMerge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 140);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnMerge);
            this.Controls.Add(this.textBoxOutFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxCSVFolder);
            this.Controls.Add(this.btnChooseOutFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnChooseCSVFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormCSVMerge";
            this.Text = "CSV Merge";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxCSVFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnChooseCSVFolder;
        private System.Windows.Forms.Button btnChooseOutFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxOutFile;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.FolderBrowserDialog FolderDialog;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
    }
}