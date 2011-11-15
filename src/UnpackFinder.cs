using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Mindstep.UnpackFinder.Properties;
using Mindstep.UnpackFinder;

namespace Mindstep.UnpackFinder
{
    public partial class UnpackFinder : Form
    {
        string[] items;

        public UnpackFinder()
        {
            InitializeComponent();
        }

        private void buttonBrowseFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxFolder.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBoxFolder.Text))
            {
                buttonScan.Enabled = false;
                buttonBrowseFolder.Enabled = false;
                textBoxFolder.Enabled = false;
                toolStripStatusLabelStatus.Text = "Searching...";
                Settings.Default.savedPath = textBoxFolder.Text;
                Settings.Default.Save();

                backgroundWorkerScanner.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("The specified directory could not be read.", "Invalid directory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listViewDirectories_ItemActivate(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(listViewDirectories.FocusedItem.Text);
        }

        private void backgroundWorkerScanner_DoWork(object sender, DoWorkEventArgs e)
        {
            items = UnpackScanner.getUnpackedWarez(textBoxFolder.Text);
        }

        private void backgroundWorkerScanner_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            listViewDirectories.Items.Clear();
            foreach (string directory in items)
            {
                listViewDirectories.Items.Add(directory);
            }
            listViewDirectories.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            buttonScan.Enabled = true;
            buttonBrowseFolder.Enabled = true;
            textBoxFolder.Enabled = true;
            toolStripStatusLabelStatus.Text = string.Format("Found {0} item(s).", items.Count());
        }
    }
}
