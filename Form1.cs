using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Move_Files
{
    public partial class FormMain : Form
    {
        string SourceXMLFolder;
        string SourceTIFFolder;

        string DestXMLFolder;
        string DestTIFFolder;

        MySettings settings;

        private string CopiedXMLLog;
        private string CopiedTIFLog;
        private string ErrorXMLLog;
        private string ErrorTIFLog;

        int copiedXMLFiles = 0, errorXMLFiles = 0;
        int copiedTIFFiles = 0, errorTIFFiles = 0;
        int totalXMLFiles = 0, totalTIFFiles = 0;
        long totalXMLSize = 0, totalTIFSize = 0;

        long totalFileSize;
        long totalCopiedSize = 0;

        bool enableXMLLog;
        bool enableTIFLog;

        public FormMain()
        {
            InitializeComponent();
            LabelClose.BackColor = Color.Transparent;
            LabelClose.ForeColor = Color.Black;

            ReadSettings();
            LabelXMLProgress.Text = string.Empty;
            LabelTIFProgress.Text = string.Empty;

            string timestring = DateTime.Now.ToString("yyMMddHHmmss");
            string LogPath = Directory.CreateDirectory(Path.GetFullPath("./MoveFilesLogs/")).FullName;
            CopiedXMLLog = Path.Combine(LogPath, $"copiedxml-{timestring}.log");
            CopiedTIFLog = Path.Combine(LogPath, $"copiedtif-{timestring}.log");
            ErrorXMLLog = Path.Combine(LogPath, $"errorxml-{timestring}.log");
            ErrorTIFLog = Path.Combine(LogPath, $"errortif-{timestring}.log");
        }

        private void ReadSettings()
        {
            settings = new MySettings(Path.GetFullPath("./MoveFilesSettings.XML"));

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
                    enableXMLLog = true;
                    cbCreateXMLLog.Checked = enableXMLLog;
                }
                else
                {
                    enableXMLLog = false;
                    cbCreateXMLLog.Checked = enableXMLLog;
                }
                    
            }
            else
            {
                enableXMLLog = false;
                cbCreateXMLLog.Checked = enableXMLLog;
            }

            tempset = settings.ReadSetting("tiflog");
            if (!string.IsNullOrWhiteSpace(tempset))
            {
                if (tempset == "enable")
                {
                    enableTIFLog = true;
                    cbCreateTIFLog.Checked = enableTIFLog;
                }
                else
                {
                    enableTIFLog = false;
                    cbCreateTIFLog.Checked = enableTIFLog;
                }

            }
            else
            {
                enableTIFLog = false;
                cbCreateTIFLog.Checked = enableTIFLog;
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
                    Status.Text="Moving " + xmlfile.Name;
                    File.Copy(xmlfile.FullName, Path.Combine(DestXMLFolder, xmlfile.Name), true);
                    if(enableXMLLog)
                        File.AppendAllText(CopiedXMLLog, Environment.NewLine + xmlfile.Name);
                    copiedXMLFiles++;
                    xmlfile.Delete();
                }
                catch
                {
                    Status.Text = "Error Moving " + xmlfile.Name;
                    errorXMLFiles++;
                    if (enableXMLLog)
                        File.AppendAllText(ErrorXMLLog, Environment.NewLine + xmlfile.Name);
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
                        if (enableTIFLog)
                            File.AppendAllText(CopiedTIFLog, Environment.NewLine + tiffile.Name);
                        copiedTIFFiles++;
                        tiffile.Delete();
                    }
                    catch
                    {
                        Status.Text = "Moving " + tiffile.Name;
                        if (enableTIFLog)
                            File.AppendAllText(ErrorTIFLog, Environment.NewLine + tiffile.Name);
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
            LabelHelp.BackColor = Color.Red;
            LabelHelp.ForeColor = Color.White;
        }

        private void LabelHelp_MouseLeave(object sender, EventArgs e)
        {
            LabelHelp.BackColor = Color.Transparent;
            LabelHelp.ForeColor = Color.Black;
        }

        private void cbCreateXMLLog_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                settings.WriteSetting("xmllog", "enable");
                enableXMLLog = true;
            }
            else
            {
                settings.WriteSetting("xmllog", "disable");
                enableXMLLog = false;
            }
                
        }

        private void cbCreateTIFLog_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                settings.WriteSetting("tiflog", "enable");
                enableTIFLog = true;
            }
            else
            {
                settings.WriteSetting("tiflog", "disable");
                enableTIFLog = false;
            }
                
        }

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
            LabelMoveSize.Text = $"Moved {(float)totalCopiedSize / (1024 * 1024)} MB of {(float)totalFileSize / (1024 * 1024)} MB";
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
