﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Move_Files
{
    public partial class FormMain : Form
    {
        #region Declaration
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

        private readonly System.Diagnostics.Stopwatch timer;
        private MySettings settings;

        private bool bEnableXMLLog, bEnableTIFLog;
        private bool bMoveXMLOnly, bMoveTIFOnly, bMoveBoth;
        private bool bWindowMoving;

        private int Sx, Sy, Ex, Ey;

        private IProcessModel processModel;
        private readonly VILModel vilModel;
        private readonly FCRModel fcrModel;
        private readonly ValidationModel validationModel;
        #endregion Declaration

        public FormMain()
        {
            InitializeComponent();

            vilModel = new VILModel(rbVIL);
            fcrModel = new FCRModel(rbFCR);
            validationModel = new ValidationModel(rbValidation);
            processModel = vilModel;
            processModel.GetRadioButton().Checked = true;
            timer = new System.Diagnostics.Stopwatch();

            ReadSettings();

            comboFilesToMove.SelectedIndex = 0;
            bMoveXMLOnly = false;
            bMoveTIFOnly = false;
            bMoveBoth = true;

            DateString = DateTime.Now.ToString("yyMMdd");
            LogPath = Directory.CreateDirectory(Path.GetFullPath("./MoveFilesLogs/")).FullName;
        }

        #region Helper Functions
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
        }
        private void UpdateFolderBoxes()
        {
            textBoxSourceXML.Text = SourceXMLFolder;
            textBoxSourceTIF.Text = SourceTIFFolder;
            textBoxDestXML.Text = DestXMLFolder;
            textBoxDestTIF.Text = DestTIFFolder;
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
        #endregion Helper Functions

        #region Window Controls
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
        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                bWindowMoving = true;
                Sx = e.X;
                Sy = e.Y;
            }
        }
        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (bWindowMoving && (e.Button == MouseButtons.Left))
            {
                Ex = e.X;
                Ey = e.Y;
                Left += Ex - Sx;
                Top += Ey - Sy;
            }
        }
        private void TitleBar_MouseUp(object sender, MouseEventArgs e)
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
        #endregion Window Controls

        #region Folder Dialog
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
        #endregion Folder Dialog

        #region Tool Control
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
        private void Reset_Click(object sender, EventArgs e)
        {
            textBoxSourceXML.Clear();
            textBoxSourceTIF.Clear();
            textBoxDestXML.Clear();
            textBoxDestTIF.Clear();
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
        private void Cancel_Click(object sender, EventArgs e)
        {
            if (BGWorker.WorkerSupportsCancellation)
            {
                BGWorker.CancelAsync();
            }
        }
        #endregion Tool Control

        #region BG Worker
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

                int numFilesToMove;
                if (rbMovePercent.Checked)
                {
                    if (!double.TryParse(textBoxMovePercent.Text, out double percentFilesToMove))
                    {
                        MessageBox.Show("Files percent to Move is Invalid");
                        ErrorProvider.SetError(textBoxMovePercent, "Enter numeric value only");
                        return;
                    }
                    else
                    {
                        if (percentFilesToMove > 100.0)
                        {
                            MessageBox.Show("Files percent should not exceed 100");
                            ErrorProvider.SetError(textBoxMovePercent, "Enter numeric value only");
                            return;
                        }
                        numFilesToMove = (int)Math.Round(percentFilesToMove / 100.0 * totalXMLFiles);
                    }
                }
                else if (rbMoveCount.Checked)
                {
                    if (!int.TryParse(textBoxMoveCount.Text, out numFilesToMove))
                    {
                        MessageBox.Show("Files to Move count is Invalid");
                        ErrorProvider.SetError(textBoxMoveCount, "Enter numeric value only");
                        return;
                    }
                }
                else
                {
                    numFilesToMove = totalXMLFiles;
                }
                foreach (FileInfo xmlfile in sourceXMLInfo.GetFiles("*.XML"))
                {
                    if (BGWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        BGWorker.ReportProgress(0);
                        return;
                    }
                    if (copiedXMLFiles >= numFilesToMove) return;
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

                int numFilesToMove;
                if (rbMovePercent.Checked)
                {
                    if (!double.TryParse(textBoxMoveCount.Text, out double percentFilesToMove))
                    {
                        MessageBox.Show("Files percent to Move is Invalid");
                        ErrorProvider.SetError(textBoxMovePercent, "Enter numeric value only");
                        return;
                    }
                    else
                    {
                        if (percentFilesToMove > 100.0)
                        {
                            MessageBox.Show("Files percent should not exceed 100");
                            ErrorProvider.SetError(textBoxMovePercent, "Enter numeric value only");
                            return;
                        }
                        numFilesToMove = (int)Math.Round(percentFilesToMove / 100.0 * totalXMLFiles);
                    }
                }
                else if (rbMoveCount.Checked)
                {
                    if (!int.TryParse(textBoxMoveCount.Text, out numFilesToMove))
                    {
                        MessageBox.Show("Files to Move count is Invalid");
                        ErrorProvider.SetError(textBoxMoveCount, "Enter numeric value only");
                        return;
                    }
                }
                else
                {
                    numFilesToMove = totalXMLFiles;
                }

                foreach (FileInfo xmlfile in sourceXMLInfo.GetFiles("*.XML"))
                {
                    if (BGWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        BGWorker.ReportProgress(0);
                        return;
                    }
                    if (copiedXMLFiles >= numFilesToMove) return;
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

                int numFilesToMove;
                if (rbMovePercent.Checked)
                {
                    if (!double.TryParse(textBoxMoveCount.Text, out double percentFilesToMove))
                    {
                        MessageBox.Show("Files percent to Move is Invalid");
                        ErrorProvider.SetError(textBoxMovePercent, "Enter numeric value only");
                        return;
                    }
                    else
                    {
                        if (percentFilesToMove > 100.0)
                        {
                            MessageBox.Show("Files percent should not exceed 100");
                            ErrorProvider.SetError(textBoxMovePercent, "Enter numeric value only");
                            return;
                        }
                        numFilesToMove = (int)Math.Round(percentFilesToMove / 100.0 * totalXMLFiles);
                    }
                }
                else if (rbMoveCount.Checked)
                {
                    if (!int.TryParse(textBoxMoveCount.Text, out numFilesToMove))
                    {
                        MessageBox.Show("Files to Move count is Invalid");
                        ErrorProvider.SetError(textBoxMoveCount, "Enter numeric value only");
                        return;
                    }
                }
                else
                {
                    numFilesToMove = totalXMLFiles;
                }

                foreach (FileInfo tiffile in sourceTIFInfo.GetFiles("*.TIF"))
                {
                    if (BGWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        BGWorker.ReportProgress(0);
                        return;
                    }
                    if (copiedXMLFiles >= numFilesToMove) return;
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
        #endregion BG Worker

        #region Events
        private void TextBoxSourceXML_TextChanged(object sender, EventArgs e)
        {
            SourceXMLFolder = textBoxSourceXML.Text;
        }
        private void TextBoxSourceTIF_TextChanged(object sender, EventArgs e)
        {
            SourceTIFFolder = textBoxSourceTIF.Text;
        }
        private void TextBoxDestXML_TextChanged(object sender, EventArgs e)
        {
            DestXMLFolder = textBoxDestXML.Text;
        }
        private void TextBoxDestTIF_TextChanged(object sender, EventArgs e)
        {
            DestTIFFolder = textBoxDestTIF.Text;
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
                ReadFolderSettings();
                UpdateFolderBoxes();
            }
        }
        private void CreateXMLLog_CheckedChanged(object sender, EventArgs e)
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
        private void CreateTIFLog_CheckedChanged(object sender, EventArgs e)
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
        private void ComboFilesToMove_SelectedIndexChanged(object sender, EventArgs e)
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
        #endregion Events
    }
}