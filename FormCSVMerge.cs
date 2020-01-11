using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Move_Files
{
    public partial class FormCSVMerge : Form
    {
        private string SourceCSVFolder;
        private string OutCSVFile;

        public FormCSVMerge()
        {
            InitializeComponent();
        }

        private void btnChooseCSVFolder_Click(object sender, EventArgs e)
        {
            FolderDialog.Description = "Select Source CSV Folder";

            if (FolderDialog.ShowDialog() == DialogResult.OK)
            {
                SourceCSVFolder = FolderDialog.SelectedPath;
                textBoxCSVFolder.Text = SourceCSVFolder;
            }
        }

        private void btnChooseOutFolder_Click(object sender, EventArgs e)
        {
            SaveFileDialog.Title = "Select output file name";
            if (Directory.Exists(SourceCSVFolder))
            {
                SaveFileDialog.InitialDirectory = SourceCSVFolder; 
            }

            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                OutCSVFile = SaveFileDialog.FileName;
                textBoxOutFile.Text = OutCSVFile;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(SourceCSVFolder))
            {
                MessageBox.Show("Source CSV folder does not exist.");
                return;
            }
            if (File.Exists(OutCSVFile))
            {
                if (MessageBox.Show("Output file already exist. Do you want to replace it?") != DialogResult.OK)
                {
                    return;
                }
            }
            StringBuilder outContent = new StringBuilder();
            int filecount = 0;
            foreach (string csvfile in Directory.GetFiles(SourceCSVFolder, "*.CSV", SearchOption.TopDirectoryOnly))
            {
                string filecontent = File.ReadAllText(csvfile);
                outContent.AppendLine();
                outContent.Append(filecontent.Trim());
                filecount++;
            }
            File.WriteAllText(OutCSVFile, outContent.ToString());
            MessageBox.Show($"{filecount} files merged.","Done!");
        }

        private void textBoxCSVFolder_TextChanged(object sender, EventArgs e)
        {
            SourceCSVFolder = textBoxCSVFolder.Text;
        }

        private void textBoxOutFile_TextChanged(object sender, EventArgs e)
        {
            OutCSVFile = textBoxOutFile.Text;
        }
    }
}
