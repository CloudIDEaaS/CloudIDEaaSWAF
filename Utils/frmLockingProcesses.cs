using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils.ProcessHelpers;

namespace Utils
{
    public partial class frmLockingProcesses : Form
    {
        private bool loaded;

        public string LockedPath { get; set; }

        public frmLockingProcesses()
        {
            InitializeComponent();
        }


        private void frmLockingProcesses_Load(object sender, EventArgs e)
        {
            var wait = this.Wait();

            Task.Run(() =>
            {
                FileSystemInfo fileSystemInfo;

                if (Directory.Exists(this.LockedPath))
                {
                    fileSystemInfo = new DirectoryInfo(this.LockedPath);

                    var processes = Process.GetProcesses();

                    foreach (var process in processes)
                    {
                        try
                        {
                            if (process.GetOpenDirectories().Any(f => f.LocalPath != null && f.LocalPath.AsCaseless().StartsWith(this.LockedPath)))
                            {
                                AddProcessToList(process);
                            }
                            else if (process.GetOpenFiles().Any(f => f.LocalPath != null && f.LocalPath.AsCaseless().StartsWith(this.LockedPath)))
                            {
                                AddProcessToList(process);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                else if (File.Exists(this.LockedPath))
                {
                    fileSystemInfo = new FileInfo(this.LockedPath);

                    try
                    {
                        var processes = fileSystemInfo.FindLockingProcesses();

                        foreach (var process in processes)
                        {
                            AddProcessToList(process);
                        }
                    }
                    catch (Exception ex)
                    {
                        wait.Dispose();
                        throw;
                    }

                }
                else
                {
                    DebugUtils.Break();
                    throw new ArgumentException("Invalid path", "LockedPath");
                }

                this.Invoke(() =>
                {
                    this.Text = "Locking Processes";
                    this.Flash();

                    if (listViewProcesses.Items.Count > 0)
                    {
                        cmdCloseSelected.Enabled = true;
                    }

                    wait.Dispose();

                    loaded = true;
                });
            });
        }

        private void AddProcessToList(Process process)
        {
            var platformProcess = ProcessExtensions.GetAllProcesses().OrderBy(p => p.ProcessName).Where(p => !p.Path.IsNullOrEmpty() && p.Process.Id == process.Id).SingleOrDefault();
            var name = process.ProcessName;
            var file = new FileInfo(platformProcess.Path);
            Bitmap icon;

            this.Invoke(() =>
            {
                ListViewItem item;

                try
                {
                    icon = file.GetLargeIcon<Bitmap>();
                }
                catch
                {
                    icon = typeof(frmLockingProcesses).ReadResource<Bitmap>(@"Images\ProcessIcon16.png");
                }

                if (!imageList.Images.ContainsKey(process.ProcessName))
                {
                    imageList.Images.Add(process.ProcessName, icon);
                }

                item = listViewProcesses.Items.Add(process.ProcessName, process.ProcessName);
                item.Tag = process;

                item.Checked = true;
            });
        }

        private void listViewProcesses_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!loaded)
            {
                return;
            }

            if (listViewProcesses.CheckedItems.Count > 0)
            {
                cmdCloseSelected.Enabled = true;
            }
            else
            {
                cmdCloseSelected.Enabled = false;
            }
        }

        private void cmdCloseSelected_Click(object sender, EventArgs e)
        {
            var erroredProcesses = new Dictionary<Process, Exception>();

            foreach (var item in listViewProcesses.CheckedItems.Cast<ListViewItem>().ToList())
            {
                var process = (Process)item.Tag;

                try
                {
                    process.Kill();
                    listViewProcesses.Items.Remove(item);
                }
                catch (Exception ex)
                {
                    erroredProcesses.Add(process, ex);
                    item.Checked = false;
                }
            }

            if (erroredProcesses.Count > 0)
            {
                var builder = new StringBuilder();

                builder.AppendLine("The following processes could not be closed:\r\n");

                foreach (var pair in erroredProcesses)
                {
                    var process = pair.Key;
                    var exception = pair.Value;

                    builder.AppendLineFormat("{0}, error: {1}", process.ProcessName, exception.Message);
                }

                MessageBox.Show(builder.ToString());

                return;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }

            this.Close();
        }

        private void frmLockingProcesses_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Cursor = Cursors.Default;
        }
    }
}
