namespace Move_Files
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            this.LabelClose = new System.Windows.Forms.Label();
            this.FolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxSourceTIF = new System.Windows.Forms.TextBox();
            this.textBoxSourceXML = new System.Windows.Forms.TextBox();
            this.btnChooseSourceXML = new System.Windows.Forms.Button();
            this.btnChooseSourceTIF = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxDestTIF = new System.Windows.Forms.TextBox();
            this.textBoxDestXML = new System.Windows.Forms.TextBox();
            this.btnChooseDestXML = new System.Windows.Forms.Button();
            this.btnChooseDestTIF = new System.Windows.Forms.Button();
            this.btnMove = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.Status = new System.Windows.Forms.ToolStripLabel();
            this.ErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.BGWorker = new System.ComponentModel.BackgroundWorker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnInvertPaths = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.LabelXMLProgress = new System.Windows.Forms.Label();
            this.LabelTIFProgress = new System.Windows.Forms.Label();
            this.LabelMoveSize = new System.Windows.Forms.Label();
            this.LabelHelp = new System.Windows.Forms.Label();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.cbCreateXMLLog = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbCreateTIFLog = new System.Windows.Forms.CheckBox();
            this.ToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelClose
            // 
            this.LabelClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelClose.AutoSize = true;
            this.LabelClose.BackColor = System.Drawing.Color.Transparent;
            this.LabelClose.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelClose.ForeColor = System.Drawing.Color.Black;
            this.LabelClose.Location = new System.Drawing.Point(451, 0);
            this.LabelClose.Name = "LabelClose";
            this.LabelClose.Size = new System.Drawing.Size(18, 19);
            this.LabelClose.TabIndex = 0;
            this.LabelClose.Text = "X";
            this.LabelClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTip.SetToolTip(this.LabelClose, "Close the program");
            this.LabelClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LabelClose_MouseClick);
            this.LabelClose.MouseEnter += new System.EventHandler(this.LabelClose_MouseEnter);
            this.LabelClose.MouseLeave += new System.EventHandler(this.LabelClose_MouseLeave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Move XML and TIF Files";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "XML Folder : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "TIF Folder : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Move From :";
            // 
            // textBoxSourceTIF
            // 
            this.textBoxSourceTIF.Location = new System.Drawing.Point(98, 61);
            this.textBoxSourceTIF.Name = "textBoxSourceTIF";
            this.textBoxSourceTIF.Size = new System.Drawing.Size(280, 20);
            this.textBoxSourceTIF.TabIndex = 4;
            this.textBoxSourceTIF.TextChanged += new System.EventHandler(this.textBoxSourceTIF_TextChanged);
            // 
            // textBoxSourceXML
            // 
            this.textBoxSourceXML.Location = new System.Drawing.Point(98, 31);
            this.textBoxSourceXML.Name = "textBoxSourceXML";
            this.textBoxSourceXML.Size = new System.Drawing.Size(280, 20);
            this.textBoxSourceXML.TabIndex = 4;
            this.textBoxSourceXML.TextChanged += new System.EventHandler(this.textBoxSourceXML_TextChanged);
            // 
            // btnChooseSourceXML
            // 
            this.btnChooseSourceXML.AutoSize = true;
            this.btnChooseSourceXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseSourceXML.Location = new System.Drawing.Point(401, 29);
            this.btnChooseSourceXML.Name = "btnChooseSourceXML";
            this.btnChooseSourceXML.Size = new System.Drawing.Size(29, 23);
            this.btnChooseSourceXML.TabIndex = 5;
            this.btnChooseSourceXML.Text = "...";
            this.btnChooseSourceXML.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ToolTip.SetToolTip(this.btnChooseSourceXML, "Select source XML folder");
            this.btnChooseSourceXML.UseVisualStyleBackColor = true;
            this.btnChooseSourceXML.Click += new System.EventHandler(this.btnChooseSourceXML_Click);
            // 
            // btnChooseSourceTIF
            // 
            this.btnChooseSourceTIF.AutoSize = true;
            this.btnChooseSourceTIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseSourceTIF.Location = new System.Drawing.Point(401, 58);
            this.btnChooseSourceTIF.Name = "btnChooseSourceTIF";
            this.btnChooseSourceTIF.Size = new System.Drawing.Size(29, 23);
            this.btnChooseSourceTIF.TabIndex = 6;
            this.btnChooseSourceTIF.Text = "...";
            this.btnChooseSourceTIF.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ToolTip.SetToolTip(this.btnChooseSourceTIF, "Select source TIF folder");
            this.btnChooseSourceTIF.UseVisualStyleBackColor = true;
            this.btnChooseSourceTIF.Click += new System.EventHandler(this.btnChooseSourceTIF_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "XML Folder : ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "TIF Folder : ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Move To :";
            // 
            // textBoxDestTIF
            // 
            this.textBoxDestTIF.Location = new System.Drawing.Point(98, 152);
            this.textBoxDestTIF.Name = "textBoxDestTIF";
            this.textBoxDestTIF.Size = new System.Drawing.Size(280, 20);
            this.textBoxDestTIF.TabIndex = 4;
            this.textBoxDestTIF.TextChanged += new System.EventHandler(this.textBoxDestTIF_TextChanged);
            // 
            // textBoxDestXML
            // 
            this.textBoxDestXML.Location = new System.Drawing.Point(98, 122);
            this.textBoxDestXML.Name = "textBoxDestXML";
            this.textBoxDestXML.Size = new System.Drawing.Size(280, 20);
            this.textBoxDestXML.TabIndex = 4;
            this.textBoxDestXML.TextChanged += new System.EventHandler(this.textBoxDestXML_TextChanged);
            // 
            // btnChooseDestXML
            // 
            this.btnChooseDestXML.AutoSize = true;
            this.btnChooseDestXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseDestXML.Location = new System.Drawing.Point(401, 120);
            this.btnChooseDestXML.Name = "btnChooseDestXML";
            this.btnChooseDestXML.Size = new System.Drawing.Size(29, 23);
            this.btnChooseDestXML.TabIndex = 5;
            this.btnChooseDestXML.Text = "...";
            this.btnChooseDestXML.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ToolTip.SetToolTip(this.btnChooseDestXML, "Select target XML folder");
            this.btnChooseDestXML.UseVisualStyleBackColor = true;
            this.btnChooseDestXML.Click += new System.EventHandler(this.btnChooseDestXML_Click);
            // 
            // btnChooseDestTIF
            // 
            this.btnChooseDestTIF.AutoSize = true;
            this.btnChooseDestTIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseDestTIF.Location = new System.Drawing.Point(401, 149);
            this.btnChooseDestTIF.Name = "btnChooseDestTIF";
            this.btnChooseDestTIF.Size = new System.Drawing.Size(29, 23);
            this.btnChooseDestTIF.TabIndex = 6;
            this.btnChooseDestTIF.Text = "...";
            this.btnChooseDestTIF.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.ToolTip.SetToolTip(this.btnChooseDestTIF, "Select target TIF folder");
            this.btnChooseDestTIF.UseVisualStyleBackColor = true;
            this.btnChooseDestTIF.Click += new System.EventHandler(this.btnChooseDestTIF_Click);
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(98, 211);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(75, 23);
            this.btnMove.TabIndex = 7;
            this.btnMove.Text = "&Move";
            this.ToolTip.SetToolTip(this.btnMove, "Start Moving Files");
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(179, 211);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "&Reset";
            this.ToolTip.SetToolTip(this.btnReset, "Clear Path fields");
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // ToolStrip
            // 
            this.ToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgressBar,
            this.Status});
            this.ToolStrip.Location = new System.Drawing.Point(0, 390);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.ToolStrip.Size = new System.Drawing.Size(469, 25);
            this.ToolStrip.TabIndex = 8;
            this.ToolStrip.Text = "toolStrip1";
            // 
            // ProgressBar
            // 
            this.ProgressBar.ForeColor = System.Drawing.Color.Lime;
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.ProgressBar.Size = new System.Drawing.Size(100, 22);
            this.ProgressBar.Step = 1;
            this.ProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // Status
            // 
            this.Status.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(39, 22);
            this.Status.Text = "Ready";
            // 
            // ErrorProvider
            // 
            this.ErrorProvider.ContainerControl = this;
            // 
            // BGWorker
            // 
            this.BGWorker.WorkerReportsProgress = true;
            this.BGWorker.WorkerSupportsCancellation = true;
            this.BGWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BGWorker_DoWork);
            this.BGWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BGWorker_ProgressChanged);
            this.BGWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BGWorker_RunWorkerCompleted);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(341, 211);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.ToolTip.SetToolTip(this.btnCancel, "Cancel copying");
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbCreateTIFLog);
            this.panel1.Controls.Add(this.cbCreateXMLLog);
            this.panel1.Controls.Add(this.btnInvertPaths);
            this.panel1.Controls.Add(this.textBoxSourceXML);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnReset);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.btnMove);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnChooseDestTIF);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnChooseSourceTIF);
            this.panel1.Controls.Add(this.textBoxSourceTIF);
            this.panel1.Controls.Add(this.btnChooseDestXML);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.btnChooseSourceXML);
            this.panel1.Controls.Add(this.textBoxDestTIF);
            this.panel1.Controls.Add(this.textBoxDestXML);
            this.panel1.Location = new System.Drawing.Point(5, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(458, 246);
            this.panel1.TabIndex = 10;
            // 
            // btnInvertPaths
            // 
            this.btnInvertPaths.Location = new System.Drawing.Point(260, 211);
            this.btnInvertPaths.Name = "btnInvertPaths";
            this.btnInvertPaths.Size = new System.Drawing.Size(75, 23);
            this.btnInvertPaths.TabIndex = 10;
            this.btnInvertPaths.Text = "&Invert Paths";
            this.ToolTip.SetToolTip(this.btnInvertPaths, "Invert source and Destination paths");
            this.btnInvertPaths.UseVisualStyleBackColor = true;
            this.btnInvertPaths.Click += new System.EventHandler(this.btnInvertPaths_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 284);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Progress : ";
            // 
            // LabelXMLProgress
            // 
            this.LabelXMLProgress.AutoSize = true;
            this.LabelXMLProgress.Location = new System.Drawing.Point(32, 311);
            this.LabelXMLProgress.Name = "LabelXMLProgress";
            this.LabelXMLProgress.Size = new System.Drawing.Size(73, 13);
            this.LabelXMLProgress.TabIndex = 13;
            this.LabelXMLProgress.Text = "XML Progress";
            // 
            // LabelTIFProgress
            // 
            this.LabelTIFProgress.AutoSize = true;
            this.LabelTIFProgress.Location = new System.Drawing.Point(32, 337);
            this.LabelTIFProgress.Name = "LabelTIFProgress";
            this.LabelTIFProgress.Size = new System.Drawing.Size(67, 13);
            this.LabelTIFProgress.TabIndex = 13;
            this.LabelTIFProgress.Text = "TIF Progress";
            // 
            // LabelMoveSize
            // 
            this.LabelMoveSize.AutoSize = true;
            this.LabelMoveSize.Location = new System.Drawing.Point(32, 363);
            this.LabelMoveSize.Name = "LabelMoveSize";
            this.LabelMoveSize.Size = new System.Drawing.Size(40, 13);
            this.LabelMoveSize.TabIndex = 14;
            this.LabelMoveSize.Text = "Moved";
            // 
            // LabelHelp
            // 
            this.LabelHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelHelp.AutoSize = true;
            this.LabelHelp.BackColor = System.Drawing.Color.Transparent;
            this.LabelHelp.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelHelp.ForeColor = System.Drawing.Color.Black;
            this.LabelHelp.Location = new System.Drawing.Point(435, 0);
            this.LabelHelp.Name = "LabelHelp";
            this.LabelHelp.Size = new System.Drawing.Size(14, 17);
            this.LabelHelp.TabIndex = 15;
            this.LabelHelp.Text = "?";
            this.LabelHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTip.SetToolTip(this.LabelHelp, "About");
            this.LabelHelp.Click += new System.EventHandler(this.LabelHelp_Click);
            this.LabelHelp.MouseEnter += new System.EventHandler(this.LabelHelp_MouseEnter);
            this.LabelHelp.MouseLeave += new System.EventHandler(this.LabelHelp_MouseLeave);
            // 
            // cbCreateXMLLog
            // 
            this.cbCreateXMLLog.AutoSize = true;
            this.cbCreateXMLLog.Location = new System.Drawing.Point(179, 187);
            this.cbCreateXMLLog.Name = "cbCreateXMLLog";
            this.cbCreateXMLLog.Size = new System.Drawing.Size(72, 17);
            this.cbCreateXMLLog.TabIndex = 11;
            this.cbCreateXMLLog.Text = "XML Files";
            this.cbCreateXMLLog.UseVisualStyleBackColor = true;
            this.cbCreateXMLLog.CheckedChanged += new System.EventHandler(this.cbCreateXMLLog_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(95, 187);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Create Logs for";
            // 
            // cbCreateTIFLog
            // 
            this.cbCreateTIFLog.AutoSize = true;
            this.cbCreateTIFLog.Location = new System.Drawing.Point(254, 187);
            this.cbCreateTIFLog.Name = "cbCreateTIFLog";
            this.cbCreateTIFLog.Size = new System.Drawing.Size(66, 17);
            this.cbCreateTIFLog.TabIndex = 11;
            this.cbCreateTIFLog.Text = "TIF Files";
            this.cbCreateTIFLog.UseVisualStyleBackColor = true;
            this.cbCreateTIFLog.CheckedChanged += new System.EventHandler(this.cbCreateTIFLog_CheckedChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 415);
            this.ControlBox = false;
            this.Controls.Add(this.LabelHelp);
            this.Controls.Add(this.LabelMoveSize);
            this.Controls.Add(this.LabelTIFProgress);
            this.Controls.Add(this.LabelXMLProgress);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ToolStrip);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LabelClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelClose;
        private System.Windows.Forms.FolderBrowserDialog FolderDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxSourceTIF;
        private System.Windows.Forms.TextBox textBoxSourceXML;
        private System.Windows.Forms.Button btnChooseSourceXML;
        private System.Windows.Forms.Button btnChooseSourceTIF;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxDestTIF;
        private System.Windows.Forms.TextBox textBoxDestXML;
        private System.Windows.Forms.Button btnChooseDestXML;
        private System.Windows.Forms.Button btnChooseDestTIF;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ToolStrip ToolStrip;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private System.Windows.Forms.ToolStripLabel Status;
        private System.Windows.Forms.ErrorProvider ErrorProvider;
        private System.ComponentModel.BackgroundWorker BGWorker;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LabelTIFProgress;
        private System.Windows.Forms.Label LabelXMLProgress;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label LabelMoveSize;
        private System.Windows.Forms.Label LabelHelp;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.Button btnInvertPaths;
        private System.Windows.Forms.CheckBox cbCreateTIFLog;
        private System.Windows.Forms.CheckBox cbCreateXMLLog;
        private System.Windows.Forms.Label label9;
    }
}

