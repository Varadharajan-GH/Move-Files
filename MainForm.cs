using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        private string ErrorOnUndo;

        private int movedXMLFiles = 0, errorXMLFiles = 0;
        private int movedTIFFiles = 0, errorTIFFiles = 0;
        private int totalXMLFiles = 0, totalTIFFiles = 0;

        private long totalXMLSize = 0, totalTIFSize = 0;
        private long totalFileSize, totalCopiedSize = 0;

        private readonly string DateString, LogPath;

        private readonly System.Diagnostics.Stopwatch timer;
        private MySettings settings;

        private bool bEnableXMLLog, bEnableTIFLog;
        private bool bMoveXMLOnly, bMoveTIFOnly, bMoveBoth;
        private bool bWindowMoving;
        private bool bFilterAccession, bFilterItem;

        private int Sx, Sy, Ex, Ey;

        private IProcessModel processModel;
        private readonly VILModel vilModel;
        private readonly FCRModel fcrModel;
        private readonly ValidationModel validationModel;

        private List<string> SourceFiles;
        private List<string> DestFiles;

        private List<string> ListValid;
        private List<string> ListInvalid;

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
            LogPath = Directory.CreateDirectory(Path.GetFullPath("./MoveFilesLogs/" + DateString)).FullName;
            btnMove.Enabled = false;
            ValidateLogin();
        }

        private void ValidateLogin()
        {
            string result = "";

            if (ShowInputDialog(ref result) == DialogResult.OK)
            {
                //string passwordHash = BCrypt.Net.BCrypt.HashPassword(result);
                string storedPass;
                try
                {
                    storedPass = File.ReadAllText("pph.db").Trim();
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("pph.db file missing.");
                    Environment.Exit(0);
                    throw;
                }
                catch
                {
                    MessageBox.Show("Could not read pph.db.");
                    Environment.Exit(0);
                    throw;
                }
                if (BCrypt.Net.BCrypt.Verify(result, storedPass))
                {
                    DateTime nistDateTime;
                    try
                    {
                        nistDateTime = GetNistTime();
                    }
                    catch
                    {
                        MessageBox.Show("Connection error. Make sure you are connected to internet.");
                        Environment.Exit(0);
                        throw;
                    }
                    DateTime expireDateTime = new DateTime(2020, 3, 27);

                    if (expireDateTime.Subtract(nistDateTime).Days > 0)
                    {
                        btnMove.Enabled = true;
                    }
                    else
                    {
                        btnMove.Enabled = false;
                        MessageBox.Show("Your free trail period expired. Contact Developer.");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    btnMove.Enabled = false;
                    MessageBox.Show("Wrong password.");
                    Environment.Exit(0);
                }
            }
            else
            {
                Environment.Exit(0);
            }
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
        private List<ItemDetail> GetItemDetails(string xmlFolder, string tifFolder)
        {
            List<ItemDetail> itemDetailList = new List<ItemDetail>();

            foreach (string xmlfile in Directory.GetFiles(xmlFolder, "*.XML", SearchOption.TopDirectoryOnly))
            {
                ItemDetail itemDetail = new ItemDetail
                {
                    ItemName = Path.GetFileNameWithoutExtension(xmlfile),
                    XmlPath = xmlfile,
                    XmlSize = new FileInfo(xmlfile).Length
                };

                List<string> fList = new List<string>();
                long totSize = 0;
                foreach (string tiffile in Directory.GetFiles(tifFolder, itemDetail.ItemName + "*.TIF", SearchOption.TopDirectoryOnly))
                {
                    fList.Add(tiffile);
                    totSize += new FileInfo(tiffile).Length;
                }
                itemDetail.TifDetail = new FilesDetail(fList, totSize);
                itemDetailList.Add(itemDetail);
            }
            return itemDetailList;
        }
        private FilesDetail GetFilesDetail(string dirName, string pattern)
        {
            List<string> fileList = new List<string>();
            long totalSize = 0;

            foreach (string file in Directory.GetFiles(dirName, pattern, SearchOption.TopDirectoryOnly))
            {
                fileList.Add(file);
                totalSize += new FileInfo(file).Length;
            }
            return new FilesDetail(fileList, totalSize);
        }
        private static DialogResult ShowInputDialog(ref string input)
        {
            Size size = new Size(200, 70);
            Form inputBox = new Form
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                ControlBox = false,
                ClientSize = size,
                Text = "Enter Password"
            };

            TextBox textBox = new TextBox
            {
                Size = new Size(size.Width - 10, 23),
                Location = new Point(5, 5),
                Text = input,
                PasswordChar = '*'
            };
            inputBox.Controls.Add(textBox);

            Button okButton = new Button
            {
                DialogResult = DialogResult.OK,
                Name = "okButton",
                Size = new Size(75, 23),
                Text = "&OK",
                Location = new Point(size.Width - 80 - 80, 39)
            };
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button
            {
                DialogResult = DialogResult.Cancel,
                Name = "cancelButton",
                Size = new Size(75, 23),
                Text = "&Cancel",
                Location = new Point(size.Width - 80, 39)
            };
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;
            inputBox.StartPosition = FormStartPosition.CenterParent;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }
        public static DateTime GetNistTime()
        {

            try
            {
                System.Net.HttpWebRequest myHttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("http://www.google.com");
                System.Net.WebResponse response = myHttpWebRequest.GetResponse();
                string todaysDates = response.Headers["date"];
                return DateTime.ParseExact(todaysDates,
                                           "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                           System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat,
                                           System.Globalization.DateTimeStyles.AssumeUniversal);
            }
            catch
            {
                throw;
            }
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
            HelpText.AppendLine();
            HelpText.AppendLine();
            HelpText.AppendLine("Reach developer at varadhamca.1887@gmail.com");
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

        private void CSVMerge_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormCSVMerge frmCSVMerge = new FormCSVMerge();
            frmCSVMerge.Show();
        }

        private void RBFilterAccession_CheckedChanged(object sender, EventArgs e)
        {
            bFilterAccession = rbFilterAccession.Checked;
        }

        private void RBFilterItem_CheckedChanged(object sender, EventArgs e)
        {
            bFilterItem = rbFilterItem.Checked;
        }

        private void CBFilterAccession_CheckedChanged(object sender, EventArgs e)
        {            
            textBoxFilter.Enabled = chkFilter.Checked;
            rbFilterAccession.Enabled = chkFilter.Checked;
            rbFilterItem.Enabled = chkFilter.Checked;
            rbFilterAccession.Checked = false;
            rbFilterItem.Checked = false;
            bFilterAccession = false;
            bFilterItem = false;
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
                CopiedXMLLog = Path.Combine(LogPath, $"{processModel.GetCopiedXMLLogPrefix()}.log");
                CopiedTIFLog = Path.Combine(LogPath, $"{processModel.GetCopiedTIFLogPrefix()}.log");
                ErrorXMLLog = Path.Combine(LogPath, $"{processModel.GetErrorXMLLogPrefix()}.log");
                ErrorTIFLog = Path.Combine(LogPath, $"{processModel.GetErrorTIFLogPrefix()}.log");
                ErrorOnUndo = Path.Combine(LogPath, $"ErrorOnUndo.log");
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

            if (chkFilter.Checked)
            {
                if (rbFilterAccession.Checked)
                {
                    bFilterAccession = true;
                    ListValid = new List<string>();
                    ListInvalid = new List<string>();
                    foreach (string line in textBoxFilter.Lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string nLine = Regex.Replace(line, "[^a-zA-Z0-9]", string.Empty).Trim();
                        if (nLine.Length == 5)
                        {
                            ListValid.Add(nLine);
                        }
                        else
                        {
                            ListInvalid.Add(line);
                            Console.WriteLine(line + "," + nLine);
                        }
                    }
                    if (ListValid.Count <= 0)
                    {
                        MessageBox.Show("No valid Accession filter entries found. No files will be moved.");
                        return;
                    }
                }
                else if (rbFilterItem.Checked)
                {
                    bFilterItem = true;
                    ListValid = new List<string>();
                    ListInvalid = new List<string>();
                    foreach (string line in textBoxFilter.Lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string nLine = Regex.Replace(line, "[^a-zA-Z0-9]", string.Empty).Trim();
                        if (nLine.Length == 9)
                        {
                            ListValid.Add(nLine);
                        }
                        else
                        {
                            ListInvalid.Add(line);
                            Console.WriteLine(line + "," + nLine);
                        }
                    }
                    if (ListValid.Count <= 0)
                    {
                        MessageBox.Show("No valid Item filter entries found. No files will be moved.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Select what to filter. Accession/Item.");
                    return;
                }
                if (ListInvalid.Count > 0)
                {
                    MessageBox.Show("Invalid entries found and will not be processed");                    
                }
                textBoxFilter.Text = string.Join(Environment.NewLine, ListInvalid.ToArray());
            }

            if (!BGWorker.IsBusy)
            {
                movedXMLFiles = errorXMLFiles = 0;
                movedTIFFiles = errorTIFFiles = 0;
                totalXMLFiles = totalTIFFiles = 0;
                totalXMLSize = totalTIFSize = 0;

                totalFileSize = totalCopiedSize = 0;
                Status.Text = "Moving";
                SourceFiles = new List<string>();
                DestFiles = new List<string>();
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
        private void Undo_Click(object sender, EventArgs e)
        {
            if (SourceFiles.Count <= 0)
            {
                MessageBox.Show("Unable to Perform Undo. \n Source files empty.");
                return;
            }
            if (DestFiles.Count <= 0)
            {
                MessageBox.Show("Unable to Perform Undo. \n Target files empty.");
                return;
            }
            if (SourceFiles.Count != DestFiles.Count)
            {
                MessageBox.Show("Unable to Perform Undo. \n Source files and Target files count mismatch.");
                return;
            }
            LabelXMLProgress.Text = "Reverting last move.";
            LabelMoveSize.Text = "";

            for (int i = 0; i < DestFiles.Count; i++)
            {
                LabelTIFProgress.Text = $"Moving file {i + 1}  of {DestFiles.Count} ";
                Status.Text = $"Moving {Path.GetFileNameWithoutExtension(DestFiles[i])}";
                try
                {
                    File.Move(DestFiles[i], SourceFiles[i]);
                }
                catch (Exception)
                {
                    File.AppendAllText(ErrorOnUndo, Environment.NewLine + $"{DestFiles[i]} => {SourceFiles[i]}");
                }
            }
            LabelTIFProgress.Text = $"{DestFiles.Count} files moved.";
            Status.Text = "Done";
            DestFiles = new List<string>();
            SourceFiles = new List<string>();
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
                List<ItemDetail> itemDetialList = GetItemDetails(SourceXMLFolder, SourceTIFFolder);

                if (itemDetialList.Count == 0)
                {
                    MessageBox.Show("No XML files found.");
                    Status.Text = "No XML files found.";
                    return;
                }

                List<ItemDetail> newItemDetialList = new List<ItemDetail>();

                if (chkFilter.Checked)
                {
                    if (bFilterAccession)
                    {
                        foreach (ItemDetail item in itemDetialList)
                        {
                            if (ListValid.Contains(item.ItemName.Substring(0, 5)))
                            {
                                newItemDetialList.Add(item);
                            }
                        }
                        itemDetialList = newItemDetialList;
                    }
                    else if (bFilterItem)
                    {
                        foreach (ItemDetail item in itemDetialList)
                        {
                            if (ListValid.Contains(item.ItemName.Substring(0, 9)))
                            {
                                newItemDetialList.Add(item);
                            }
                        }
                        itemDetialList = newItemDetialList;
                    }
                }

                totalXMLFiles = itemDetialList.Count;
                totalXMLSize = itemDetialList.Sum(f => f.XmlSize);
                totalTIFFiles = itemDetialList.Sum(f => f.TifDetail.filesList.Count);
                totalTIFSize = itemDetialList.Sum(f => f.TifDetail.totalSize);

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
                        return;
                    }
                    if (percentFilesToMove > 100.0 || percentFilesToMove < 0.0)
                    {
                        MessageBox.Show("Files percent should be between 0 and 100");
                        return;
                    }
                    numFilesToMove = (int)Math.Round(percentFilesToMove / 100.0 * totalXMLFiles);
                    itemDetialList = itemDetialList.Take(numFilesToMove).ToList();
                    totalXMLFiles = itemDetialList.Count;
                    totalXMLSize = itemDetialList.Sum(f => f.XmlSize);
                    totalTIFFiles = itemDetialList.Sum(f => f.TifDetail.filesList.Count);
                    totalTIFSize = itemDetialList.Sum(f => f.TifDetail.totalSize);
                    totalFileSize = totalXMLSize + totalTIFSize;
                }
                else if (rbMoveCount.Checked)
                {
                    if (!int.TryParse(textBoxMoveCount.Text, out numFilesToMove))
                    {
                        MessageBox.Show("Files to Move count is Invalid");
                        return;
                    }
                    if (numFilesToMove < 0)
                    {
                        MessageBox.Show("Files to Move count should not be negative;");
                        return;
                    }
                    itemDetialList = itemDetialList.Take(numFilesToMove).ToList();
                    totalXMLFiles = itemDetialList.Count;
                    totalXMLSize = itemDetialList.Sum(f => f.XmlSize);
                    totalTIFFiles = itemDetialList.Sum(f => f.TifDetail.filesList.Count);
                    totalTIFSize = itemDetialList.Sum(f => f.TifDetail.totalSize);
                    totalFileSize = totalXMLSize + totalTIFSize;
                }

                if (itemDetialList.Count == 0)
                {
                    MessageBox.Show("No files to move.");
                    return;
                }
                if (bEnableXMLLog) File.AppendAllText(CopiedXMLLog, Environment.NewLine + "===========");
                if (bEnableTIFLog) File.AppendAllText(CopiedTIFLog, Environment.NewLine + "===========");
                foreach (ItemDetail itemDetail in itemDetialList)
                {
                    if (BGWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        BGWorker.ReportProgress(0);
                        return;
                    }
                    try
                    {
                        Status.Text = "Moving XML" + itemDetail.ItemName;
                        File.Move(itemDetail.XmlPath, Path.Combine(DestXMLFolder, itemDetail.ItemName + ".XML"));
                        //File.Copy(itemDetail.XmlPath, Path.Combine(DestXMLFolder, itemDetail.ItemName + ".XML"), true);
                        SourceFiles.Add(itemDetail.XmlPath);
                        DestFiles.Add(Path.Combine(DestXMLFolder, itemDetail.ItemName + ".XML"));
                        if (bEnableXMLLog) File.AppendAllText(CopiedXMLLog, Environment.NewLine + itemDetail.ItemName);
                        movedXMLFiles++;
                        //File.Delete(itemDetail.XmlPath);
                    }
                    catch
                    {
                        Status.Text = "Error Moving " + itemDetail.ItemName;
                        errorXMLFiles++;
                        if (bEnableXMLLog) File.AppendAllText(ErrorXMLLog, Environment.NewLine + itemDetail.ItemName);
                    }
                    finally
                    {
                        totalCopiedSize += itemDetail.XmlSize;
                        BGWorker.ReportProgress((int)(totalCopiedSize / (float)totalFileSize * 100));
                    }

                    foreach (string tiffile in itemDetail.TifDetail.filesList)
                    {
                        //if (BGWorker.CancellationPending)
                        //{
                        //    e.Cancel = true;
                        //    BGWorker.ReportProgress(0);
                        //}
                        try
                        {
                            Status.Text = "Moving " + Path.GetFileName(tiffile);
                            totalCopiedSize += new FileInfo(tiffile).Length;
                            File.Move(tiffile, Path.Combine(DestTIFFolder, Path.GetFileNameWithoutExtension(tiffile) + ".TIF"));
                            //File.Copy(tiffile, Path.Combine(DestTIFFolder, Path.GetFileNameWithoutExtension(tiffile) + ".TIF"), true);
                            SourceFiles.Add(tiffile);
                            DestFiles.Add(Path.Combine(DestTIFFolder, Path.GetFileNameWithoutExtension(tiffile) + ".TIF"));
                            if (bEnableTIFLog) File.AppendAllText(CopiedTIFLog, Environment.NewLine + Path.GetFileNameWithoutExtension(tiffile));
                            movedTIFFiles++;
                            //File.Delete(tiffile);
                        }
                        catch
                        {
                            Status.Text = "Moving " + Path.GetFileNameWithoutExtension(tiffile);
                            totalCopiedSize += new FileInfo(tiffile).Length;
                            if (bEnableTIFLog)
                                File.AppendAllText(ErrorTIFLog, Environment.NewLine + Path.GetFileNameWithoutExtension(tiffile));
                            errorTIFFiles++;
                        }
                        finally
                        {
                            BGWorker.ReportProgress((int)(totalCopiedSize / (float)totalFileSize * 100));
                        }
                    }
                }
            }
            else if (bMoveXMLOnly)
            {
                FilesDetail xmlDetial = GetFilesDetail(SourceXMLFolder, "*.XML");

                if (xmlDetial.filesList.Count == 0)
                {
                    MessageBox.Show("No XML files found.");
                    Status.Text = "No XML files found.";
                    return;
                }
                totalXMLFiles = xmlDetial.filesList.Count;
                totalXMLSize = xmlDetial.totalSize;
                totalTIFFiles = 0;
                totalTIFSize = 0;

                totalFileSize = totalXMLSize;

                int numFilesToMove;
                if (rbMovePercent.Checked)
                {
                    if (!double.TryParse(textBoxMovePercent.Text, out double percentFilesToMove))
                    {
                        MessageBox.Show("Files percent to Move is Invalid");
                        return;
                    }
                    if (percentFilesToMove > 100.0 || percentFilesToMove < 0.0)
                    {
                        MessageBox.Show("Files percent should be between 0 and 100");
                        return;
                    }
                    numFilesToMove = (int)Math.Round(percentFilesToMove / 100.0 * totalXMLFiles);
                    xmlDetial.filesList = xmlDetial.filesList.Take(numFilesToMove).ToList();
                    totalXMLFiles = xmlDetial.filesList.Count;
                    totalXMLSize = xmlDetial.totalSize;
                    totalFileSize = totalXMLSize;
                }
                else if (rbMoveCount.Checked)
                {
                    if (!int.TryParse(textBoxMoveCount.Text, out numFilesToMove))
                    {
                        MessageBox.Show("Files to Move count is Invalid");
                        return;
                    }
                    if (numFilesToMove < 0)
                    {
                        MessageBox.Show("Files to Move count should not be negative;");
                        return;
                    }
                    xmlDetial.filesList = xmlDetial.filesList.Take(numFilesToMove).ToList();
                    totalXMLFiles = xmlDetial.filesList.Count;
                    totalXMLSize = xmlDetial.totalSize;
                    totalFileSize = totalXMLSize;
                }

                if (xmlDetial.filesList.Count == 0)
                {
                    MessageBox.Show("No files to move.");
                    return;
                }
                if (bEnableXMLLog) File.AppendAllText(CopiedXMLLog, Environment.NewLine + "===========");
                foreach (string xmlfile in xmlDetial.filesList)
                {
                    if (BGWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        BGWorker.ReportProgress(0);
                        return;
                    }
                    try
                    {
                        Status.Text = "Moving XML" + Path.GetFileName(xmlfile);
                        totalCopiedSize += new FileInfo(xmlfile).Length;
                        File.Move(xmlfile, Path.Combine(DestXMLFolder, Path.GetFileName(xmlfile)));
                        //File.Copy(xmlfile, Path.Combine(DestXMLFolder, Path.GetFileName(xmlfile)), true);
                        SourceFiles.Add(xmlfile);
                        DestFiles.Add(Path.Combine(DestXMLFolder, Path.GetFileName(xmlfile)));
                        if (bEnableXMLLog) File.AppendAllText(CopiedXMLLog, Environment.NewLine + Path.GetFileNameWithoutExtension(xmlfile));
                        movedXMLFiles++;
                        //File.Delete(xmlfile);
                    }
                    catch
                    {
                        Status.Text = "Error Moving " + Path.GetFileName(xmlfile);
                        totalCopiedSize += new FileInfo(xmlfile).Length;
                        errorXMLFiles++;
                        if (bEnableXMLLog)
                            File.AppendAllText(ErrorXMLLog, Environment.NewLine + Path.GetFileNameWithoutExtension(xmlfile));
                    }
                    finally
                    {
                        BGWorker.ReportProgress((int)(totalCopiedSize / (float)totalFileSize * 100));
                    }
                }
            }
            else if (bMoveTIFOnly)
            {
                FilesDetail tifDetial = GetFilesDetail(SourceTIFFolder, "*.TIF");

                if (tifDetial.filesList.Count == 0)
                {
                    MessageBox.Show("No TIF files found.");
                    Status.Text = "No TIF files found.";
                    return;
                }
                totalTIFFiles = tifDetial.filesList.Count;
                totalTIFSize = tifDetial.totalSize;
                totalXMLFiles = 0;
                totalXMLSize = 0;

                totalFileSize = totalTIFSize;

                int numFilesToMove;
                if (rbMovePercent.Checked)
                {
                    if (!double.TryParse(textBoxMovePercent.Text, out double percentFilesToMove))
                    {
                        MessageBox.Show("Files percent to Move is Invalid");
                        return;
                    }
                    if (percentFilesToMove > 100.0 || percentFilesToMove < 0.0)
                    {
                        MessageBox.Show("Files percent should be between 0 and 100");
                        return;
                    }
                    numFilesToMove = (int)Math.Round(percentFilesToMove / 100.0 * totalTIFFiles);
                    tifDetial.filesList = tifDetial.filesList.Take(numFilesToMove).ToList();
                    totalTIFFiles = tifDetial.filesList.Count;
                    totalTIFSize = tifDetial.totalSize;
                    totalFileSize = totalTIFSize;
                }
                else if (rbMoveCount.Checked)
                {
                    if (!int.TryParse(textBoxMoveCount.Text, out numFilesToMove))
                    {
                        MessageBox.Show("Files to Move count is Invalid");
                        return;
                    }
                    if (numFilesToMove < 0)
                    {
                        MessageBox.Show("Files to Move count should not be negative;");
                        return;
                    }
                    tifDetial.filesList = tifDetial.filesList.Take(numFilesToMove).ToList();
                    totalTIFFiles = tifDetial.filesList.Count;
                    totalTIFSize = tifDetial.totalSize;
                    totalFileSize = totalTIFSize;
                }

                if (tifDetial.filesList.Count == 0)
                {
                    MessageBox.Show("No files to move.");
                    return;
                }
                if (bEnableTIFLog) File.AppendAllText(CopiedTIFLog, Environment.NewLine + "===========");
                foreach (string tiffile in tifDetial.filesList)
                {
                    if (BGWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        BGWorker.ReportProgress(0);
                        return;
                    }
                    try
                    {
                        Status.Text = "Moving " + Path.GetFileName(tiffile);
                        totalCopiedSize += new FileInfo(tiffile).Length;
                        File.Move(tiffile, Path.Combine(DestTIFFolder, Path.GetFileName(tiffile)));
                        //File.Copy(tiffile, Path.Combine(DestTIFFolder, Path.GetFileName(tiffile)), true);
                        SourceFiles.Add(tiffile);
                        DestFiles.Add(Path.Combine(DestTIFFolder, Path.GetFileName(tiffile)));
                        if (bEnableTIFLog) File.AppendAllText(CopiedTIFLog, Environment.NewLine + Path.GetFileNameWithoutExtension(tiffile));
                        movedTIFFiles++;
                        //File.Delete(tiffile);
                    }
                    catch
                    {
                        Status.Text = "Error Moving " + Path.GetFileName(tiffile);
                        totalCopiedSize += new FileInfo(tiffile).Length;
                        errorTIFFiles++;
                        if (bEnableTIFLog)
                            File.AppendAllText(ErrorTIFLog, Environment.NewLine + Path.GetFileNameWithoutExtension(tiffile));
                    }
                    finally
                    {
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
            LabelXMLProgress.Text = $"{movedXMLFiles} of {totalXMLFiles} XML files moved. {errorXMLFiles} errors.";
            LabelTIFProgress.Text = $"{movedTIFFiles} of {totalTIFFiles} TIF files moved. {errorTIFFiles} errors.";
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
                Status.Text = "Done";
                ProgressBar.Value = 0;
                if (errorTIFFiles != 0 || errorXMLFiles != 0)
                {
                    MessageBox.Show("Some errors occured while moving files. Review error logs and verify moved files.");
                }
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