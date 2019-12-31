using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Move_Files
{
    public partial class FormMain : Form
    {
        private string SourceXMLFolder;
        private string SourceTIFFolder;

        private string DestXMLFolder;
        private string DestTIFFolder;

        private MySettings settings;

        private string CopiedXMLLog;
        private string CopiedTIFLog;
        private string ErrorXMLLog;
        private string ErrorTIFLog;

        private int copiedXMLFiles = 0, errorXMLFiles = 0;
        private int copiedTIFFiles = 0, errorTIFFiles = 0;
        private int totalXMLFiles = 0, totalTIFFiles = 0;

        private long totalXMLSize = 0, totalTIFSize = 0;
        private long totalFileSize, totalCopiedSize = 0;

        private string dateString;
        private string LogPath;

        private System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

        private bool bEnableXMLLog, bEnableTIFLog;
        private bool bMoveXMLOnly, bMoveTIFOnly, bMoveBoth;
        private bool bWindowMoving;

        //private bool bPrefetchFiles;

        int Sx, Sy, Ex, Ey;

        public FormMain()
        {
            InitializeComponent();
            LabelClose.BackColor = Color.Transparent;
            LabelClose.ForeColor = Color.Black;

            ReadSettings();
            LabelXMLProgress.Text = string.Empty;
            LabelTIFProgress.Text = string.Empty;
            LabelMoveSize.Text = string.Empty;

            comboFilesToMove.SelectedIndex = 0;
            bMoveXMLOnly = false;
            bMoveTIFOnly = false;
            bMoveBoth = true;

            dateString = DateTime.Now.ToString("yyMMdd");
            LogPath = Directory.CreateDirectory(Path.GetFullPath("./MoveFilesLogs/")).FullName;

        }

        private void ReadSettings()
        {
            settings = new MySettings(Path.GetFullPath("./MoveFilesSettings.XML"));

            //if (settings.ReadSetting("prefetch") == "enable")
            //{
            //    bPrefetchFiles = true;

            //}
            //else
            //{
            //    bPrefetchFiles = false;
            //}
            //cbPrefetch.Checked = bPrefetchFiles;

            SourceXMLFolder = settings.ReadSetting("sourcexmlpath");
            SourceTIFFolder = settings.ReadSetting("sourcetifpath");

            DestXMLFolder = settings.ReadSetting("destxmlpath");
            DestTIFFolder = settings.ReadSetting("desttifpath");

            textBoxSourceXML.Text = SourceXMLFolder;
            textBoxSourceTIF.Text = SourceTIFFolder;
            textBoxDestXML.Text = DestXMLFolder;
            textBoxDestTIF.Text = DestTIFFolder;

            string tempset;

            tempset = settings.ReadSetting("xmllog");
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

        private void btnChooseSourceXML_Click(object sender, EventArgs e)
        {
            FolderDialog.Description = "Select Source XML Folder";

            if (FolderDialog.ShowDialog() == DialogResult.OK)
            {
                SourceXMLFolder = FolderDialog.SelectedPath;
                textBoxSourceXML.Text = SourceXMLFolder;
                ErrorProvider.SetError(textBoxSourceXML, string.Empty);
            }
        }

        private void btnChooseSourceTIF_Click(object sender, EventArgs e)
        {
            FolderDialog.Description = "Select Source TIF Folder";

            if (FolderDialog.ShowDialog() == DialogResult.OK)
            {
                SourceTIFFolder = FolderDialog.SelectedPath;
                textBoxSourceTIF.Text = SourceTIFFolder;
                ErrorProvider.SetError(textBoxSourceTIF, string.Empty);
            }
        }

        private void btnChooseDestXML_Click(object sender, EventArgs e)
        {
            FolderDialog.Description = "Select Dest XML Folder";

            if (FolderDialog.ShowDialog() == DialogResult.OK)
            {
                DestXMLFolder = FolderDialog.SelectedPath;
                textBoxDestXML.Text = DestXMLFolder;
                ErrorProvider.SetError(textBoxDestXML, string.Empty);
            }
        }

        private void btnChooseDestTIF_Click(object sender, EventArgs e)
        {
            FolderDialog.Description = "Select Dest TIF Folder";

            if (FolderDialog.ShowDialog() == DialogResult.OK)
            {
                DestTIFFolder = FolderDialog.SelectedPath;
                textBoxDestTIF.Text = DestTIFFolder;
                ErrorProvider.SetError(textBoxDestTIF, string.Empty);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            textBoxSourceXML.Clear();
            textBoxSourceTIF.Clear();
            textBoxDestXML.Clear();
            textBoxDestTIF.Clear();
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            if (rbVIL.Checked)
            {
                CopiedXMLLog = Path.Combine(LogPath, $"C_copiedxml_{dateString}.log");
                CopiedTIFLog = Path.Combine(LogPath, $"C_copiedtif_{dateString}.log");
                ErrorXMLLog = Path.Combine(LogPath, $"C_errorxml_{dateString}.log");
                ErrorTIFLog = Path.Combine(LogPath, $"C_errortif_{dateString}.log");
            }
            else if (rbFCR.Checked)
            {
                CopiedXMLLog = Path.Combine(LogPath, $"F_copiedxml_{dateString}.log");
                CopiedTIFLog = Path.Combine(LogPath, $"F_copiedtif_{dateString}.log");
                ErrorXMLLog = Path.Combine(LogPath, $"F_errorxml_{dateString}.log");
                ErrorTIFLog = Path.Combine(LogPath, $"F_errortif_{dateString}.log");
            }
            else
            {
                MessageBox.Show("You must select either VIL or FCR", "Select Process", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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

            settings.WriteSetting("sourcexmlpath", SourceXMLFolder);
            settings.WriteSetting("sourcetifpath", SourceTIFFolder);
            settings.WriteSetting("destxmlpath", DestXMLFolder);
            settings.WriteSetting("desttifpath", DestTIFFolder);


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

                //if (!bPrefetchFiles)
                //{  
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
                totalFileSize = totalXMLSize + totalTIFSize;
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
                //}
                //else
                //{
                //    System.Collections.Generic.IEnumerable<FileInfo> xmlfiles = sourceXMLInfo.EnumerateFiles("*.XML", SearchOption.TopDirectoryOnly);
                //    totalXMLFiles = xmlfiles.Count();
                //    if (totalXMLFiles == 0)
                //    {
                //        MessageBox.Show("No XML files found.");
                //        Status.Text = "No XML files found.";
                //        return;
                //    }
                //    totalXMLSize = xmlfiles.Sum(f => f.Length);
                //    totalTIFSize=sourceXMLInfo.EnumerateFiles("*.XML",SearchOption.TopDirectoryOnly) sourceTIFInfo.EnumerateFiles(Path.GetFileNameWithoutExtension(xmlfile.Name) + "*.TIF")
                //    totalFileSize = totalXMLSize;
                //    foreach (FileInfo xmlfile in xmlfiles)
                //    {
                //        if (BGWorker.CancellationPending)
                //        {
                //            e.Cancel = true;
                //            BGWorker.ReportProgress(0);
                //            return;
                //        }
                //        try
                //        {
                //            Status.Text = "Moving " + xmlfile.Name;
                //            xmlfile.CopyTo(Path.Combine(DestXMLFolder, xmlfile.Name), true);
                //            if (bEnableXMLLog)
                //                File.AppendAllText(CopiedXMLLog, Environment.NewLine + Path.GetFileNameWithoutExtension(xmlfile.Name));
                //            copiedXMLFiles++;
                //            xmlfile.Delete();
                //        }
                //        catch
                //        {
                //            Status.Text = "Error Moving " + xmlfile.Name;
                //            errorXMLFiles++;
                //            if (bEnableXMLLog)
                //                File.AppendAllText(ErrorXMLLog, Environment.NewLine + Path.GetFileNameWithoutExtension(xmlfile.Name));
                //        }
                //        finally
                //        {
                //            totalCopiedSize += xmlfile.Length;
                //            BGWorker.ReportProgress((int)(totalCopiedSize / (float)totalFileSize * 100));
                //        }
                //        foreach (FileInfo tiffile in sourceTIFInfo.GetFiles(Path.GetFileNameWithoutExtension(xmlfile.Name) + "*.TIF"))
                //        {
                //            if (BGWorker.CancellationPending)
                //            {
                //                e.Cancel = true;
                //                BGWorker.ReportProgress(0);
                //            }
                //            try
                //            {
                //                Status.Text = "Moving " + tiffile.Name;
                //                File.Copy(tiffile.FullName, Path.Combine(DestTIFFolder, tiffile.Name), true);
                //                if (bEnableTIFLog)
                //                    File.AppendAllText(CopiedTIFLog, Environment.NewLine + Path.GetFileNameWithoutExtension(tiffile.Name));
                //                copiedTIFFiles++;
                //                tiffile.Delete();
                //            }
                //            catch
                //            {
                //                Status.Text = "Moving " + tiffile.Name;
                //                if (bEnableTIFLog)
                //                    File.AppendAllText(ErrorTIFLog, Environment.NewLine + Path.GetFileNameWithoutExtension(tiffile.Name));
                //                errorTIFFiles++;
                //            }
                //            finally
                //            {
                //                totalCopiedSize += tiffile.Length;
                //                BGWorker.ReportProgress((int)(totalCopiedSize / (float)totalFileSize * 100));
                //            }
                //        }
                //    }
                //}
            }
            else if (bMoveXMLOnly)
            {
                //if (!bPrefetchFiles)
                //{
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
                //}
                //else
                //{                    
                //    System.Collections.Generic.IEnumerable<FileInfo> xmlfiles = new DirectoryInfo(SourceXMLFolder).EnumerateFiles("*.XML", SearchOption.TopDirectoryOnly);
                //    totalXMLFiles = xmlfiles.Count();
                //    if (totalXMLFiles == 0)
                //    {
                //        MessageBox.Show("No XML files found.");
                //        Status.Text = "No XML files found.";
                //        return;
                //    }
                //    totalXMLSize = xmlfiles.Sum(f => f.Length);
                //    totalFileSize = totalXMLSize;
                //    foreach (FileInfo xmlfile in xmlfiles)
                //    {
                //        if (BGWorker.CancellationPending)
                //        {
                //            e.Cancel = true;
                //            BGWorker.ReportProgress(0);
                //            return;
                //        }
                //        try
                //        {
                //            Status.Text = "Moving " + xmlfile.Name;
                //            xmlfile.CopyTo(Path.Combine(DestXMLFolder, xmlfile.Name), true);
                //            if (bEnableXMLLog)
                //                File.AppendAllText(CopiedXMLLog, Environment.NewLine + Path.GetFileNameWithoutExtension(xmlfile.Name));
                //            copiedXMLFiles++;
                //            xmlfile.Delete();
                //        }
                //        catch
                //        {
                //            Status.Text = "Error Moving " + xmlfile.Name;
                //            errorXMLFiles++;
                //            if (bEnableXMLLog)
                //                File.AppendAllText(ErrorXMLLog, Environment.NewLine + Path.GetFileNameWithoutExtension(xmlfile.Name));
                //        }
                //        finally
                //        {
                //            totalCopiedSize += xmlfile.Length;
                //            BGWorker.ReportProgress((int)(totalCopiedSize / (float)totalFileSize * 100));
                //        }
                //    }
                //}
            }
            else if (bMoveTIFOnly)
            {
                //if (!bPrefetchFiles)
                //{
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
                //}
                //else
                //{
                //    System.Collections.Generic.IEnumerable<FileInfo> tiffiles = new DirectoryInfo(SourceTIFFolder).EnumerateFiles("*.TIF", SearchOption.TopDirectoryOnly);
                //    totalTIFFiles = tiffiles.Count();
                //    if (totalTIFFiles == 0)
                //    {
                //        MessageBox.Show("No TIF files found.");
                //        Status.Text = "No TIF files found.";
                //        return;
                //    }
                //    totalTIFSize = tiffiles.Sum(f => f.Length);
                //    totalFileSize = totalTIFSize;
                //    foreach (FileInfo tiffile in tiffiles)
                //    {
                //        if (BGWorker.CancellationPending)
                //        {
                //            e.Cancel = true;
                //            BGWorker.ReportProgress(0);
                //            return;
                //        }
                //        try
                //        {
                //            Status.Text = "Moving " + tiffile.Name;
                //            tiffile.CopyTo(Path.Combine(DestTIFFolder, tiffile.Name), true);
                //            if (bEnableTIFLog)
                //                File.AppendAllText(CopiedTIFLog, Environment.NewLine + Path.GetFileNameWithoutExtension(tiffile.Name));
                //            copiedTIFFiles++;
                //            tiffile.Delete();
                //        }
                //        catch
                //        {
                //            Status.Text = "Error Moving " + tiffile.Name;
                //            errorTIFFiles++;
                //            if (bEnableTIFLog)
                //                File.AppendAllText(ErrorTIFLog, Environment.NewLine + Path.GetFileNameWithoutExtension(tiffile.Name));
                //        }
                //        finally
                //        {
                //            totalCopiedSize += tiffile.Length;
                //            BGWorker.ReportProgress((int)(totalCopiedSize / (float)totalFileSize * 100));
                //        }
                //    }
                //}
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
            System.Text.StringBuilder HelpText = new System.Text.StringBuilder();
            HelpText.AppendLine("This program will move all XML files from source XML folder");
            HelpText.AppendLine("to target XML folder and their corresponding TIF files from");
            HelpText.AppendLine("source TIF folder to target TIF folder.");
            HelpText.AppendLine("Tool will also create logs for Moved files in Log folder");
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
                    textBoxSourceXML.Enabled = true;
                    textBoxDestXML.Enabled = true;
                    btnChooseSourceXML.Enabled = true;
                    btnChooseDestXML.Enabled = true;

                    textBoxSourceTIF.Enabled = true;
                    textBoxDestTIF.Enabled = true;
                    btnChooseSourceTIF.Enabled = true;
                    btnChooseDestTIF.Enabled = true;

                    bMoveXMLOnly = false;
                    bMoveTIFOnly = false;
                    bMoveBoth = true;

                    break;
                case "XML only":
                    textBoxSourceXML.Enabled = true;
                    textBoxDestXML.Enabled = true;
                    btnChooseSourceXML.Enabled = true;
                    btnChooseDestXML.Enabled = true;

                    textBoxSourceTIF.Enabled = false;
                    textBoxDestTIF.Enabled = false;
                    btnChooseSourceTIF.Enabled = false;
                    btnChooseDestTIF.Enabled = false;

                    bMoveXMLOnly = true;
                    bMoveTIFOnly = false;
                    bMoveBoth = false;

                    MessageBox.Show("Only XML files will be moved.\nTIFs wont move.");
                    break;
                case "TIF only":
                    textBoxSourceXML.Enabled = false;
                    textBoxDestXML.Enabled = false;
                    btnChooseSourceXML.Enabled = false;
                    btnChooseDestXML.Enabled = false;

                    textBoxSourceTIF.Enabled = true;
                    textBoxDestTIF.Enabled = true;
                    btnChooseSourceTIF.Enabled = true;
                    btnChooseDestTIF.Enabled = true;

                    bMoveXMLOnly = false;
                    bMoveTIFOnly = true;
                    bMoveBoth = false;

                    MessageBox.Show("All TIF files will be moved.\nXMLs wont move.", "Confirm?", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                default:
                    textBoxSourceXML.Enabled = true;
                    textBoxDestXML.Enabled = true;
                    btnChooseSourceXML.Enabled = true;
                    btnChooseDestXML.Enabled = true;

                    textBoxSourceTIF.Enabled = true;
                    textBoxDestTIF.Enabled = true;
                    btnChooseSourceTIF.Enabled = true;
                    btnChooseDestTIF.Enabled = true;

                    bMoveXMLOnly = false;
                    bMoveTIFOnly = false;
                    bMoveBoth = true;

                    break;
            }
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

        //private void cbPrefetch_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (((CheckBox)sender).Checked)
        //    {
        //        settings.WriteSetting("prefetch", "enable");
        //        bPrefetchFiles = true;
        //    }
        //    else
        //    {
        //        settings.WriteSetting("prefetch", "disable");
        //        bPrefetchFiles = false;
        //    }
        //}

        private void btnInvertPaths_Click(object sender, EventArgs e)
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
            LabelXMLProgress.Text = $"{copiedXMLFiles} of {totalXMLFiles} XML files copied. {errorXMLFiles} errors.";
            LabelTIFProgress.Text = $"{copiedTIFFiles} of {totalTIFFiles} TIF files copied. {errorTIFFiles} errors.";
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
                if(totalXMLFiles==0 && totalTIFFiles == 0)
                {
                    LabelXMLProgress.Text = string.Empty;
                    LabelTIFProgress.Text = string.Empty;
                    LabelMoveSize.Text = string.Empty;
                }
                else
                {
                    LabelMoveSize.Text = $"{(float)totalCopiedSize / (1024 * 1024)} MB moved in {timer.Elapsed.TotalSeconds} seconds at {((float)totalCopiedSize / (1024 * 1024 * timer.Elapsed.TotalSeconds)).ToString("00.00")} MBps";
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
