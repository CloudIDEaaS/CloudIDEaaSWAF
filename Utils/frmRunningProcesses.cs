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
    public partial class frmRunningProcesses : Form
    {
        private bool loaded;
        private Bitmap warningIcon;

        public string Path { get; set; }
        public List<Process> Processes { get; set; }
        public Dictionary<int, ProcessAdvancedInfoEx> AdvancedInfo { get; }

        public frmRunningProcesses()
        {
            this.AdvancedInfo = new Dictionary<int, ProcessAdvancedInfoEx>();

            InitializeComponent();
        }

        private void frmRunningProcesses_Load(object sender, EventArgs e)
        {
            var wait = this.Wait();

            Task.Run(() =>
            {
                FileSystemInfo fileSystemInfo;

                if (Directory.Exists(this.Path))
                {
                    fileSystemInfo = new DirectoryInfo(this.Path);

                    foreach (var process in this.Processes)
                    {
                        try
                        {
                            var advancedInfo = process.GetAdvancedInfo();

                            if (advancedInfo != null)
                            {
                                var advancedInfoEx = new ProcessAdvancedInfoEx(advancedInfo, process);
                                var isConflicting = false;

                                if (advancedInfo.CommandLine != null && advancedInfo.CommandLine.Any(c => c.RemoveQuotes().AsCaseless() == this.Path))
                                {
                                    advancedInfoEx.Conflict = string.Format("Command line conflicts with '{0}", this.Path);
                                    isConflicting = true;
                                }

                                AddProcessToList(process, isConflicting);

                                this.AdvancedInfo.Add(process.Id, advancedInfoEx);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                else if (File.Exists(this.Path))
                {
                    fileSystemInfo = new FileInfo(this.Path);

                    try
                    {
                        foreach (var process in this.Processes)
                        {
                            var advancedInfo = process.GetAdvancedInfo();

                            if (advancedInfo != null)
                            {
                                var advancedInfoEx = new ProcessAdvancedInfoEx(advancedInfo, process);
                                var isConflicting = false;

                                if (advancedInfo.CommandLine != null && advancedInfo.CommandLine.Any(c => c.RemoveQuotes().AsCaseless() == this.Path))
                                {
                                    advancedInfoEx.Conflict = string.Format("Command line conflicts with '{0}", this.Path);
                                    isConflicting = true;
                                }

                                AddProcessToList(process, isConflicting);

                                this.AdvancedInfo.Add(process.Id, advancedInfoEx);
                            }
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
                    throw new ArgumentException("Invalid path", "Path");
                }

                this.Invoke(() =>
                {
                    this.Text = "Running Processes";
                    this.Flash();

                    if (listViewProcesses.SelectedItems.Count > 0)
                    {
                        cmdCloseSelected.Enabled = true;
                    }

                    wait.Dispose();

                    loaded = true;
                });
            });
        }

        private void AddProcessToList(Process process, bool isConflicting)
        {
            var platformProcess = ProcessExtensions.GetAllProcesses().OrderBy(p => p.ProcessName).Where(p => !p.Path.IsNullOrEmpty() && p.Process.Id == process.Id).SingleOrDefault();
            var name = process.ProcessName;
            var file = new FileInfo(platformProcess.Path);
            Bitmap icon;

            this.Invoke(() =>
            {
                ListViewItem item;
                string id = process.Id.ToString();

                try
                {
                    icon = file.GetLargeIcon<Bitmap>();
                }
                catch
                {
                    icon = typeof(frmRunningProcesses).ReadResource<Bitmap>(@"Images\ProcessIcon16.png");
                }

                if (isConflicting)
                {
                    Bitmap bitmap;

                    if (warningIcon == null)
                    {
                        warningIcon = typeof(frmRunningProcesses).ReadResource<Bitmap>(@"Images\WarningIcon.png");
                    }

                    warningIcon.MakeTransparent();

                    using (var graphics = Graphics.FromImage(icon))
                    {
                        graphics.DrawImage(warningIcon, new PointF(6, 6));
                    }
                }

                if (!imageList.Images.ContainsKey(id))
                {
                    imageList.Images.Add(id, icon);
                }

                item = listViewProcesses.Items.Add(process.ProcessName, id);
                item.Tag = process;

                if (isConflicting)
                {
                    item.Checked = true;
                }
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

        private void frmRunningProcesses_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void listViewProcesses_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var item = e.Item;
            var process = (Process)item.Tag;

            if (this.AdvancedInfo.ContainsKey(process.Id))
            {
                var advancedInfoEx = this.AdvancedInfo[process.Id];

                propertyGrid.Title = process.ProcessName;
                propertyGrid.PropertyGrid.SelectedObject = advancedInfoEx;
            }
        }
    }
}
