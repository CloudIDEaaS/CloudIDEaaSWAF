using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.IO;

namespace Utils
{
    public class TfsAgent
    {
        private string teamProjectCollectionUrl;
        private TfsTeamProjectCollection teamFoundationServices;
        private VersionControlServer versionControlServer;
        public bool Connected { get; set; }

        public TfsAgent(string teamProjectCollectionUrl)
        {
            try
            {
                this.teamProjectCollectionUrl = teamProjectCollectionUrl;

                teamFoundationServices = new TfsTeamProjectCollection(new Uri(teamProjectCollectionUrl), System.Net.CredentialCache.DefaultCredentials);

                teamFoundationServices.EnsureAuthenticated();

                versionControlServer = (VersionControlServer)teamFoundationServices.GetService(typeof(VersionControlServer));

                this.Connected = true;
            }
            catch (Exception ex)
            {
            }
        }

        public void CheckoutFiles(string rootPath, string pattern, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                var workspace = versionControlServer.GetWorkspace(rootPath);

                workspace.Refresh();

                foreach (var file in Directory.GetFiles(rootPath, pattern, searchOption))
                {
                    workspace.PendEdit(file);
                }
            }
            catch (Exception ex)
            {
                DebugUtils.Break();
            }
        }

        public void CheckoutFiles(string rootPath, List<string> files)
        {
            try
            {
                Workspace workspace;

                versionControlServer = (VersionControlServer)teamFoundationServices.GetService(typeof(VersionControlServer));

                workspace = versionControlServer.GetWorkspace(rootPath);
                workspace.Refresh();

                foreach (var file in files)
                {
                    workspace.PendEdit(file);
                }

                workspace.Refresh();
            }
            catch (Exception ex)
            {
                DebugUtils.Break();
            }
        }

        public void CheckoutFiles(string rootPath)
        {
            try
            {
                Workspace workspace;

                versionControlServer = (VersionControlServer)teamFoundationServices.GetService(typeof(VersionControlServer));

                workspace = versionControlServer.GetWorkspace(rootPath);
                workspace.Refresh();

                workspace.PendEdit(rootPath, RecursionType.Full);
            }
            catch (Exception ex)
            {
                DebugUtils.Break();
            }
        }

        public void CheckinFiles(string rootPath, string pattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            try
            {
                var pendingChanges = new List<PendingChange>();
                var workspace = versionControlServer.GetWorkspace(rootPath);

                workspace.Refresh();

                foreach (var file in Directory.GetFiles(rootPath, pattern, searchOption))
                {
                    var pendingChange = workspace.GetPendingChangesEnumerable().Single(p => p.FileName == file);

                    pendingChanges.Add(pendingChange);
                }

                workspace.CheckIn(pendingChanges.ToArray(), string.Format("Auto check in by {0} on {1}", Environment.UserName, DateTime.Now.ToDateTimeText()));
            }
            catch (Exception ex)
            {
                DebugUtils.Break();
            }
        }

        public void CheckinFiles(string rootPath, List<string> files)
        {
            try
            {
                Workspace workspace;
                IEnumerable<PendingChange> pendingChanges;

                workspace = versionControlServer.GetWorkspace(rootPath);
                workspace.Refresh();

                pendingChanges = workspace.GetPendingChangesEnumerable().Where(p => files.Any(f => f == p.LocalItem));

                if (pendingChanges.Count() > 0)
                {
                    workspace.CheckIn(pendingChanges.ToArray(), string.Format("Auto check in by {0} on {1}", Environment.UserName, DateTime.Now.ToDateTimeText()));
                }
            }
            catch (Exception ex)
            {
                DebugUtils.Break();
            }
        }

        public bool HasPendingChanges(string rootPath, List<string> files)
        {
            try
            {
                Workspace workspace;
                IEnumerable<PendingChange> pendingChanges;

                workspace = versionControlServer.GetWorkspace(rootPath);
                workspace.Refresh();

                pendingChanges = workspace.GetPendingChangesEnumerable().Where(p => files.Any(f => f == p.LocalItem));

                return pendingChanges.Count() > 0;
            }
            catch (Exception ex)
            {
                DebugUtils.Break();
            }

            return false;
        }

        public void CheckinFiles(string rootPath)
        {
            try
            {
                Workspace workspace;
                IEnumerable<PendingChange> pendingChanges;

                workspace = versionControlServer.GetWorkspace(rootPath);
                workspace.Refresh();

                pendingChanges = workspace.GetPendingChangesEnumerable().ToList();

                if (pendingChanges.Count() > 0)
                {
                    workspace.CheckIn(pendingChanges.ToArray(), string.Format("Auto check in by {0} on {1}", Environment.UserName, DateTime.Now.ToDateTimeText()));
                }
            }
            catch (Exception ex)
            {
                DebugUtils.Break();
            }
        }

        public void CheckoutFile(string rootPath, string file)
        {
            try
            {
                Workspace workspace;

                versionControlServer = (VersionControlServer)teamFoundationServices.GetService(typeof(VersionControlServer));

                workspace = versionControlServer.GetWorkspace(rootPath);
                workspace.Refresh();

                workspace.PendEdit(file);
                workspace.Refresh();
            }
            catch (Exception ex)
            {
                DebugUtils.Break();
            }
        }

        public void CheckinFile(string rootPath, string file)
        {
            try
            {
                Workspace workspace;
                PendingChange pendingChange;
                List<PendingChange> pendingChanges;

                workspace = versionControlServer.GetWorkspace(rootPath);
                workspace.Refresh();

                pendingChanges = workspace.GetPendingChangesEnumerable().ToList();
                pendingChange = pendingChanges.Single(p => p.LocalItem == file);

                workspace.CheckIn(new PendingChange[] { pendingChange }, string.Format("Auto check in of {0} by {1} on {2}", file, Environment.UserName, DateTime.Now.ToDateTimeText()));
            }
            catch (Exception ex)
            {
                DebugUtils.Break();
            }
        }
    }
}
