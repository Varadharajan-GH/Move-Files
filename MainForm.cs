using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Move_Files
{
    public partial class FormMain : Form
    {
        private string SourceXMLFolder, SourceTIFFolder;

        private string DestXMLFolder, DestTIFFolder;

        private string CopiedXMLLog, CopiedTIFLog;
        private string ErrorXMLLog, ErrorTIFLog;

        private int copiedXMLFiles = 0, errorXMLFiles = 0;
        private int copiedTIFFiles = 0, errorTIFFiles = 0;
        private int totalXMLFiles = 0, totalTIFFiles = 0;

        private long totalXMLSize = 0, totalTIFSize = 0;
        private long totalFileSize, totalCopiedSize = 0;

        private readonly string DateString, LogPath;

        private readonly System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
        private MySettings settings;

        private bool bEnableXMLLog, bEnableTIFLog;
        private bool bMoveXMLOnly, bMoveTIFOnly, bMoveBoth;
        private bool bWindowMoving;

        private string CurrentProcess;

        private int Sx, Sy, Ex, Ey;

        private IProcessModel processModel;
        private VILModel vilModel;
        private FCRModel fcrModel;
        private ValidationModel validationModel;

        public FormMain()
        {
            InitializeComponent();
            LabelClose.BackColor = Color.Transparent;
            LabelClose.ForeColor = Color.Black;

            vilModel = new VILModel(rbVIL);
            fcrModel = new FCRModel(rbFCR);
            validationModel = new ValidationModel(rbValidation);

            processModel = vilModel;
            processModel.GetRadioButton().Checked = true;
            CurrentProcess = processModel.GetProcessName();

            ReadSettings();

            LabelXMLProgress.Text = string.Empty;
            LabelTIFProgress.Text = string.Empty;
            LabelMoveSize.Text = string.Empty;

            comboFilesToMove.SelectedIndex = 0;
            bMoveXMLOnly = false;
            bMoveTIFOnly = false;
            bMoveBoth = true;

            DateString = DateTime.Now.ToString("yyMMdd");
            LogPath = Directory.CreateDirectory(Path.GetFullPath("./MoveFilesLogs/")).FullName;

        }

        private void ReadSettings()
        {
            settings = new MySettings(Path.GetFullPath("./MoveFilesSettings.XML"));

            ReadFolderSettings();
            UpdateFolderBoxes();

            string tempset = settings.ReadSetting("xmllog");
            if (!string.IsNullOrWhiteSpace(tempset))
            {
                if (tempset == "enable")
                {
                    bEnableXMLLog = true;
                    cbCreateXMLLog.Checked = bEnableXMLLog;
                }
                else
                {
                    bEnableXMLLog = false;
                    cbCreateXMLLog.Checked = bEnableXMLLog;
                }
            }
            else
            {
                bEnableXMLLog = false;
                cbCreateXMLLog.Checked = bEnableXMLLog;
            }

            tempset = settings.ReadSetting("tiflog");
            if (!string.IsNullOrWhiteSpace(tempset))
            {
                if (tempset == "enable")
                {
                    bEnableTIFLog = true;
                    cbCreateTIFLog.Checked = bEnableTIFLog;
                }
                else
                {
                    bEnableTIFLog = false;
                    cbCreateTIFLog.Checked = bEnableTIFLog;
                }
            }
            else
            {
                bEnableTIFLog = false;
                cbCreateTIFLog.Checked = bEnableTIFLog;
            }
        }
        private void ReadFolderSettings()
        {
            if (processModel == null)
            {
                MessageBox.Show("Select Process");
                return;
            }
            else
            {
                SourceXMLFolder = settings.ReadSetting(processModel.GetSourceXMLPathKey());
                SourceTIFFolder = settings.ReadSetting(processModel.GetSourceTIFPathKey());

                DestXMLFolder = settings.ReadSetting(processModel.GetDestXMLPathKey());
                DestTIFFolder = settings.ReadSetting(processModel.GetDestTIFPathKey());
            }


            //if (CurrentProcess == "VIL")
            //{
            //    SourceXMLFolder = settings.ReadSetting("vilsourcexmlpath");
            //    SourceTIFFolder = settings.ReadSetting("vilsourcetifpath");

            //    DestXMLFolder = settings.ReadSetting("vildestxmlpath");
            //    DestTIFFolder = settings.ReadSetting("vildesttifpath");
            //}
            //else if (CurrentProcess == "FCR")
            //{
            //    SourceXMLFolder = settings.ReadSetting("fcrsourcexmlpath");
            //    SourceTIFFolder = settings.ReadSetting("fcrsourcetifpath");

            //    DestXMLFolder = settings.ReadSetting("fcrdestxmlpath");
            //    DestTIFFolder = settings.ReadSetting("fcrdesttifpath");
            //}
            //else
            //{
            //    MessageBox.Show("Select Process");
            //}
        }
        private void UpdateFolderBoxes()
        {
            textBoxSourceXML.Text = SourceXMLFolder;
            textBoxSourceTIF.Text = SourceTIFFolder;
            textBoxDestXML.Text = DestXMLFolder;
            textBoxDestTIF.Text = DestTIFFolder;
        }

        private void LabelClose_MouseClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void LabelClose_MouseEnter(object sender, EventArgs e)
        {
            LabelClose.BackColor = Color.Red;
            LabelClose.ForeColor = Color.White;
        }

        private void LabelClose_MouseLeave(object sender, EventArgs e)
        {
            LabelClose.BackColor = Color.Transparent;
            LabelClose.ForeColor = Color.Black;
        }

        private void ChooseSourceXML_Click(object sender, EventArgs e)
        {
            FolderDialog.Description = "Select Source XML Folder";

            if (FolderDialog.ShowDialog() == DialogResult.OK)
            {
                SourceXMLFolder = FolderDialog.SelectedPath;
                textBoxSourceXML.Text = SourceXMLFolder;
                ErrorProvider.SetError(textBoxSourceXML, string.Empty);
            }
        }

        private void ChooseSourceTIF_Click(object sender, EventArgs e)
        {
            FolderDialog.Description = "Select Source TIF Folder";

            if (FolderDialog.ShowDialog() == DialogResult.OK)
            {
                SourceTIFFolder = FolderDialog.SelectedPath;
                textBoxSourceTIF.Text = SourceTIFFolder;
                ErrorProvider.SetError(textBoxSourceTIF, string.Empty);
            }
        }

        private void ChooseDestXML_Click(object sender, EventArgs e)
        {
            FolderDialog.Description = "Select Dest XML Folder";

            if (FolderDialog.ShowDialog() == DialogResult.OK)
            {
                DestXMLFolder = FolderDialog.SelectedPath;
                textBoxDestXML.Text = DestXMLFolder;
                ErrorProvider.SetError(textBoxDestXML, string.Empty);
            }
        }

        private void ChooseDestTIF_Click(object sender, EventArgs e)
        {
            FolderDialog.Description = "Select Dest TIF Folder";

            if (FolderDialog.ShowDialog() == DialogResult.OK)
            {
                DestTIFFolder = FolderDialog.SelectedPath;
                textBoxDestTIF.Text = DestTIFFolder;
                ErrorProvider.SetError(textBoxDestTIF, string.Empty);
            }
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            textBoxSourceXML.Clear();
            textBoxSourceTIF.Clear();
            textBoxDestXML.Clear();
            textBoxDestTIF.Clear();
        }

        private void Move_Click(object sender, EventArgs e)
        {
            if (processModel == null)
            {
                MessageBox.Show("You must select either VIL or FCR", "Select Process", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                CopiedXMLLog = Path.Combine(LogPath, $"{processModel.GetCopiedXMLLogPrefix()}_{DateString}.log");
                CopiedTIFLog = Path.Combine(LogPath, $"{processModel.GetCopiedTIFLogPrefix()}_{DateString}.log");
                ErrorXMLLog = Path.Combine(LogPath, $"{processModel.GetErrorXMLLogPrefix()}_{DateString}.log");
                ErrorTIFLog = Path.Combine(LogPath, $"{processModel.GetErrorTIFLogPrefix()}_{DateString}.log");
            }
            //if (CurrentProcess=="VIL")
            //{
            //    CopiedXMLLog = Path.Combine(LogPath, $"C_copiedxml_{DateString}.log");
            //    CopiedTIFLog = Path.Combine(LogPath, $"C_copiedtif_{DateString}.log");
            //    ErrorXMLLog = Path.Combine(LogPath, $"C_errorxml_{DateString}.log");
            //    ErrorTIFLog = Path.Combine(LogPath, $"C_errortif_{DateString}.log");
            //}
            //else if (CurrentProcess == "FCR")
            //{
            //    CopiedXMLLog = Path.Combine(LogPath, $"F_copiedxml_{DateString}.log");
            //    CopiedTIFLog = Path.Combine(LogPath, $"F_copiedtif_{DateString}.log");
            //    ErrorXMLLog = Path.Combine(LogPath, $"F_errorxml_{DateString}.log");
            //    ErrorTIFLog = Path.Combine(LogPath, $"F_errortif_{DateString}.log");
            //}
            //else
            //{
            //    MessageBox.Show("You must select either VIL or FCR", "Select Process", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            if (!Directory.Exists(textBoxSourceXML.Text))
            {
                ErrorProvider.SetError(textBoxSourceXML, "Source XML Folder does not exist");
                return;
            }
            else
            {
                ErrorProvider.SetError(textBoxSourceXML, string.Empty);
            }
            if (!Directory.Exists(textBoxSourceTIF.Text))
            {
                ErrorProvider.SetError(textBoxSourceTIF, "Source TIF Folder does not exist");
                return;
            }
            else
            {
                ErrorProvider.SetError(textBoxSourceTIF, string.Empty);
            }
            if (!Directory.Exists(textBoxDestXML.Text))
            {
                ErrorProvider.SetError(textBoxDestXML, "Target XML Folder does not exist");
                return;
            }
            else
            {
                ErrorProvider.SetError(textBoxDestXML, string.Empty);
            }
            if (!Directory.Exists(textBoxDestTIF.Text))
            {
                ErrorProvider.SetError(textBoxDestTIF, "Target TIF Folder does not exist");
                return;
            }
            else
            {
                ErrorProvider.SetError(textBoxDestTIF, string.Empty);
            }

            if (SourceXMLFolder == DestXMLFolder)
            {
                MessageBox.Show("Source and target XML folders are same");
                return;
            }

            if (SourceTIFFolder == DestTIFFolder)
            {
                MessageBox.Show("Source and target TIF folders are same");
                return;
            }

            if (processModel == null)
            {
                MessageBox.Show("Unable to write settings. No process selected.");
            }
            else
            {
                settings.WriteSetting(processModel.GetSourceXMLPathKey(), SourceXMLFolder);
                settings.WriteSetting(processModel.GetSourceTIFPathKey(), SourceTIFFolder);
                settings.WriteSetting(processModel.GetDestXMLPathKey(), DestXMLFolder);
                settings.WriteSetting(processModel.GetDestTIFPathKey(), DestTIFFolder);
            }

            //if (CurrentProcess == "VIL")
            //{
            //    settings.WriteSetting("vilsourcexmlpath", SourceXMLFolder);
            //    settings.WriteSetting("vilsourcetifpath", SourceTIFFolder);
            //    settings.WriteSetting("vildestxmlpath", DestXMLFolder);
            //    settings.WriteSetting("vildesttifpath", DestTIFFolder);
            //}
            //else if (CurrentProcess == "FCR")
            //{
            //    settings.WriteSetting("fcrsourcexmlpath", SourceXMLFolder);
            //    settings.WriteSetting("fcrsourcetifpath", SourceTIFFolder);
            //    settings.WriteSetting("fcrdestxmlpath", DestXMLFolder);
            //    settings.WriteSetting("fcrdesttifpath", DestTIFFolder);
            //} 

            if (!BGWorker.IsBusy)
            {
                copiedXMLFiles = errorXMLFiles = 0;
                copiedTIFFiles = errorTIFFiles = 0;
                totalXMLFiles = totalTIFFiles = 0;
                totalXMLSize = totalTIFSize = 0;

                totalFileSize = totalCopiedSize = 0;
                Status.Text = "Moving";
                BGWorker.RunWorkerAsync();
            }
        }

        private void BGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            timer.Restart();
            if (bMoveBoth)
            {
                DirectoryInfo sourceXMLInfo = new DirectoryInfo(SourceXMLFolder);
                DirectoryInfo sourceTIFInfo = new DirectoryInfo(SourceTIFFolder);

                foreach (FileInfo xmlfile in sourceXMLInfo.GetFiles("*.XML"))
                {
                    totalXMLFiles++;
                    totalXMLSize += xmlfile.Length;

                    foreach (FileInfo tiffile in sourceTIFInfo.GetFiles(Path.GetFileNameWithoutExtension(xmlfile.Name) + "*.TIF"))
                    {
                        totalTIFFiles++;
                        totalTIFSize += tiffile.Length;
                    }
                }
                if (totalXMLFiles == 0)
                {
                    MessageBox.Show("No XML files found.");
                    Status.Text = "No XML files found.";
                    return;
                }
                if (totalTIFFiles == 0)
                {
                    if (MessageBox.Show("No corresponding TIF files found. Move only XMLs?", "Confirm Move!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                totalFileSize = totalXMLSize + totalTIFSize;
                foreach (FileInfo xmlfile in sourceXMLInfo.GetFiles("*.XML"))
                {
                    if (BGWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        BGWorker.ReportProgress(0);
                        return;
                    }
                    try
                    {
                        Status.Text = "Moving " + xmlfile.Name;
                        File.Copy(xmlfile.FullName, Path.Combine(DestXMLFolder, xmlfile.Name), true);
                        if (bEnableXMLLog)
                            File.AppendAllText(CopiedXMLLog, Environment.NewLine + Path.GetFileNameWithoutExtension(xmlfile.Name));
                        copiedXMLFiles++;
                        xmlfile.Delete();
                    }
                    catch
                    {
                        Status.Text = "Error Moving " + xmlfile.Name;
                        errorXMLFiles++;
                        if (bEnableXMLLog)
                            File.AppendAllText(ErrorXMLLog, Environment.NewLine + Path.GetFileNameWithoutExtension(xmlfile.Name));
                    }
                    finally
                    {
                        totalCopiedSize += xmlfile.Length;
                        BGWorker.ReportProgress((int)(totalCopiedSize / (float)totalFileSize * 100));
                    }

                    foreach (FileInfo tiffile in sourceTIFInfo.GetFiles(Path.GetFileNameWithoutExtension(xmlfile.Name) + "*.TIF"))
                    {
                        if (BGWorker.CancellationPending)
                        {
                            e.Cancel = true;
                            BGWorker.ReportProgress(0);
                        }
                        try
                        {
                            Status.Text = "Moving " + tiffile.Name;
                            File.Copy(tiffile.FullName, Path.Combine(DestTIFFolder, tiffile.Name), true);
                            if (bEnableTIFLog)
                                File.AppendAllText(CopiedTIFLog, Environment.NewLine + Path.GetFileNameWithoutExtension(tiffile.Name));
                            copiedTIFFiles++;
                            tiffile.Delete();
                        }
                        catch
                        {
                            Status.Text = "Moving " + tiffile.Name;
                            if (bEnableTIFLog)
                                File.AppendAllText(ErrorTIFLog, Environment.NewLine + Path.GetFileNameWithoutExtension(tiffile.Name));
                            errorTIFFiles++;
                        }
                        finally
                        {
                            totalCopiedSize += tiffile.Length;
                            BGWorker.ReportProgress((int)(totalCopiedSize / (float)totalFileSize * 100));
                        }
                    }
                }
            }
            else if (bMoveXMLOnly)
            {
                DirectoryInfo sourceXMLInfo = new DirectoryInfo(SourceXMLFolder);

                foreach (FileInfo xmlfile in sourceXMLInfo.GetFiles("*.XML"))
                {
                    totalXMLFiles++;
                    totalXMLSize += xmlfile.Length;
                }
                totalFileSize = totalXMLSize;
                if (totalXMLFiles == 0)
                {
                    MessageBox.Show("No XML files found.");
                    Status.Text = "No XML files found.";
                    return;
                }
                foreach (FileInfo xmlfile in sourceXMLInfo.GetFiles("*.XML"))
                {
                    if (BGWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        BGWorker.ReportProgress(0);
                        return;
                    }
                    try
                    {
                        Status.Text = "Moving " + xmlfile.Name;
                        File.Copy(xmlfile.FullName, Path.Combine(DestXMLFolder, xmlfile.Name), true);
                        if (bEnableXMLLog)
                            File.AppendAllText(CopiedXMLLog, Environment.NewLine + Path.GetFileNameWithoutExtension(xmlfile.Name));
                        copiedXMLFiles++;
                        xmlfile.Delete();
                    }
                    catch
                    {
                        Status.Text = "Error Moving " + xmlfile.Name;
                        errorXMLFiles++;
                        if (bEnableXMLLog)
                            File.AppendAllText(ErrorXMLLog, Environment.NewLine + Path.GetFileNameWithoutExtension(xmlfile.Name));
                    }
                    finally
                    {
                        totalCopiedSize += xmlfile.Length;
                        BGWorker.ReportProgress((int)(totalCopiedSize / (float)totalFileSize * 100));
                    }
                }
            }
            else if (bMoveTIFOnly)
            {
                DirectoryInfo sourceTIFInfo = new DirectoryInfo(SourceTIFFolder);

                foreach (FileInfo TIFfile in sourceTIFInfo.GetFiles("*.TIF"))
                {
                    totalTIFFiles++;
                    totalTIFSize += TIFfile.Length;
                }
                totalFileSize = totalTIFSize;
                if (totalTIFFiles == 0)
                {
                    MessageBox.Show("No TIF files found.");
                    Status.Text = "No TIF files found.";
                    return;
                }
                foreach (FileInfo tiffile in sourceTIFInfo.GetFiles("*.TIF"))
                {
                    if (BGWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        BGWorker.ReportProgress(0);
                        return;
                    }
                    try
                    {
                        Status.Text = "Moving " + tiffile.Name;
                        File.Copy(tiffile.FullName, Path.Combine(DestTIFFolder, tiffile.Name), true);
                        if (bEnableTIFLog)
                            File.AppendAllText(CopiedTIFLog, Environment.NewLine + Path.GetFileNameWithoutExtension(tiffile.Name));
                        copiedTIFFiles++;
                        tiffile.Delete();
                    }
                    catch
                    {
                        Status.Text = "Error Moving " + tiffile.Name;
                        errorTIFFiles++;
                        if (bEnableTIFLog)
                            File.AppendAllText(ErrorTIFLog, Environment.NewLine + Path.GetFileNameWithoutExtension(tiffile.Name));
                    }
                    finally
                    {
                        totalCopiedSize += tiffile.Length;
                        BGWorker.ReportProgress((int)(totalCopiedSize / (float)totalFileSize * 100));
                    }
                }
            }
            else
            {
                timer.Stop();
                MessageBox.Show("Select what to move.");
                return;
            }
        }

        private void textBoxSourceXML_TextChanged(object sender, EventArgs e)
        {
            SourceXMLFolder = textBoxSourceXML.Text;
        }

        private void textBoxSourceTIF_TextChanged(object sender, EventArgs e)
        {
            SourceTIFFolder = textBoxSourceTIF.Text;
        }

        private void textBoxDestXML_TextChanged(object sender, EventArgs e)
        {
            DestXMLFolder = textBoxDestXML.Text;
        }

        private void textBoxDestTIF_TextChanged(object sender, EventArgs e)
        {
            DestTIFFolder = textBoxDestTIF.Text;
        }

        private void LabelHelp_Click(object sender, EventArgs e)
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            DateTime buildDate = new DateTime(2000, 1, 1)
                                    .AddDays(version.Build)
                                    .AddSeconds(version.Revision * 2);

            System.Text.StringBuilder HelpText = new System.Text.StringBuilder();
            HelpText.AppendLine("   Move Files");
            HelpText.AppendLine($"   Version : {version}");
            HelpText.AppendLine($"   Build Date : {buildDate}");
            HelpText.AppendLine();
            HelpText.AppendLine("   This program will move all XML files from source XML folder");
            HelpText.AppendLine("to target XML folder and their corresponding TIF files from");
            HelpText.AppendLine("source TIF folder to target TIF folder.");
            HelpText.AppendLine();
            HelpText.AppendLine("   Tool will also create logs for Moved files in Log folder");
            MessageBox.Show(HelpText.ToString(), "Move Files", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LabelHelp_MouseEnter(object sender, EventArgs e)
        {
            LabelHelp.BackColor = Color.LightGray;
            LabelHelp.ForeColor = Color.White;
        }

        private void LabelHelp_MouseLeave(object sender, EventArgs e)
        {
            LabelHelp.BackColor = Color.Transparent;
            LabelHelp.ForeColor = Color.Black;
        }

        private void panelTitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (bWindowMoving && (e.Button == MouseButtons.Left))
            {
                Ex = e.X;
                Ey = e.Y;
                Left += Ex - Sx;
                Top += Ey - Sy;
            }
        }

        private void panelTitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (bWindowMoving && (e.Button == MouseButtons.Left))
            {
                Ex = e.X;
                Ey = e.Y;
                Left += Ex - Sx;
                Top += Ey - Sy;
                bWindowMoving = false;
            }
        }

        private void ProcessChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                if (((RadioButton)sender) == vilModel.processRadio)
                {
                    processModel = vilModel;
                }
                else if (((RadioButton)sender) == fcrModel.processRadio)
                {
                    processModel = fcrModel;
                }
                else if (((RadioButton)sender) == validationModel.processRadio)
                {
                    processModel = validationModel;
                }
                CurrentProcess = processModel.GetProcessName();
                ReadFolderSettings();
                UpdateFolderBoxes();
            }



            //if (((RadioButton)sender).Checked)
            //{
            //    RadioButton rb = (RadioButton)sender;
            //    CurrentProcess = rb.Text;
            //    ReadFolderSettings();
            //    UpdateFolderBoxes();
            //}
        }

        private void cbCreateXMLLog_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                settings.WriteSetting("xmllog", "enable");
                bEnableXMLLog = true;
            }
            else
            {
                settings.WriteSetting("xmllog", "disable");
                bEnableXMLLog = false;
            }

        }

        private void cbCreateTIFLog_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                settings.WriteSetting("tiflog", "enable");
                bEnableTIFLog = true;
            }
            else
            {
                settings.WriteSetting("tiflog", "disable");
                bEnableTIFLog = false;
            }

        }

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bWindowMoving = true;
                Sx = e.X;
                Sy = e.Y;
            }
        }

        private void comboFilesToMove_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboFilesToMove.SelectedItem)
            {
                case "Both XML and TIF":
                    EnableXMLControls();
                    EnableTIFControls();

                    bMoveXMLOnly = false;
                    bMoveTIFOnly = false;
                    bMoveBoth = true;
                    break;
                case "XML only":
                    EnableXMLControls();
                    DisableTIFControls();

                    bMoveXMLOnly = true;
                    bMoveTIFOnly = false;
                    bMoveBoth = false;

                    MessageBox.Show("Only XML files will be moved.\nTIFs wont move.");
                    break;
                case "TIF only":
                    DisableXMLControls();
                    EnableTIFControls();

                    bMoveXMLOnly = false;
                    bMoveTIFOnly = true;
                    bMoveBoth = false;

                    MessageBox.Show("All TIF files will be moved.\nXMLs wont move.", "Confirm?", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                default:
                    EnableXMLControls();
                    EnableTIFControls();

                    bMoveXMLOnly = false;
                    bMoveTIFOnly = false;
                    bMoveBoth = true;

                    break;
            }
        }

        private void DisableXMLControls()
        {
            textBoxSourceXML.Enabled = false;
            textBoxDestXML.Enabled = false;
            btnChooseSourceXML.Enabled = false;
            btnChooseDestXML.Enabled = false;
        }
        private void DisableTIFControls()
        {
            textBoxSourceTIF.Enabled = false;
            textBoxDestTIF.Enabled = false;
            btnChooseSourceTIF.Enabled = false;
            btnChooseDestTIF.Enabled = false;
        }
        private void EnableTIFControls()
        {
            textBoxSourceTIF.Enabled = true;
            textBoxDestTIF.Enabled = true;
            btnChooseSourceTIF.Enabled = true;
            btnChooseDestTIF.Enabled = true;
        }
        private void EnableXMLControls()
        {
            textBoxSourceXML.Enabled = true;
            textBoxDestXML.Enabled = true;
            btnChooseSourceXML.Enabled = true;
            btnChooseDestXML.Enabled = true;
        }
        private void LabelMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void LabelMinimize_MouseEnter(object sender, EventArgs e)
        {
            LabelMinimize.BackColor = Color.LightGray;
            LabelMinimize.ForeColor = Color.White;
        }

        private void LabelMinimize_MouseLeave(object sender, EventArgs e)
        {
            LabelMinimize.BackColor = Color.Transparent;
            LabelMinimize.ForeColor = Color.Black;
        }

        private void InvertPaths_Click(object sender, EventArgs e)
        {
            string temp;
            temp = SourceXMLFolder;
            textBoxSourceXML.Text = DestXMLFolder;
            textBoxDestXML.Text = temp;

            temp = SourceTIFFolder;
            textBoxSourceTIF.Text = DestTIFFolder;
            textBoxDestTIF.Text = temp;
        }

        private void BGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            LabelXMLProgress.Text = $"{copiedXMLFiles} of {totalXMLFiles} XML files moved. {errorXMLFiles} errors.";
            LabelTIFProgress.Text = $"{copiedTIFFiles} of {totalTIFFiles} TIF files moved. {errorTIFFiles} errors.";
            LabelMoveSize.Text = $"Moved {(float)totalCopiedSize / (1024 * 1024)} MB of {(float)totalFileSize / (1024 * 1024)} MB at {((float)totalCopiedSize / (1024 * 1024 * timer.Elapsed.TotalSeconds)).ToString("00.00") } MBps";
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void BGWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Status.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                Status.Text = "Error: " + e.Error.Message;
            }
            else
            {
                Status.Text = "Done!";
                ProgressBar.Value = 0;
                if (totalXMLFiles == 0 && totalTIFFiles == 0)
                {
                    LabelXMLProgress.Text = string.Empty;
                    LabelTIFProgress.Text = string.Empty;
                    LabelMoveSize.Text = string.Empty;
                }
                else
                {
                    LabelMoveSize.Text = $"{(float)totalCopiedSize / (1024 * 1024)} MB moved in {timer.Elapsed.TotalSeconds} seconds at {((float)totalCopiedSize / (1024 * 1024 * timer.Elapsed.TotalSeconds)).ToString("00.00")} MBps";
                    MessageBox.Show($"Task complete for {processModel.GetProcessName()} process.");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (BGWorker.WorkerSupportsCancellation)
            {
                BGWorker.CancelAsync();
            }
        }

    }
}
