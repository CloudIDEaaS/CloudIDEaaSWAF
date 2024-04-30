using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utils.XCopy
{
    public class FolderXCopyOptions
    {
        private readonly List<string> _excludeRegexSubStrings = new List<string>();
        private readonly List<string> _excludeSubStrings = new List<string>();
        private readonly List<string> _includeSubStrings = new List<string>();

        public bool AlwaysMatchFolderIncludes
        {
            get;
            set;
        }

        public bool CopyEmptyFolders
        {
            get;
            set;
        }

        public bool CopyHiddenAndSystemFiles
        {
            get;
            set;
        }

        public bool CopyOnlyIfSourceIsNewer
        {
            get;
            set;
        }

        public List<string> ExcludeRegexSubStrings
        {
            get
            {
                return this._excludeRegexSubStrings;
            }
        }

        public List<string> ExcludeSubStrings
        {
            get
            {
                return this._excludeSubStrings;
            }
        }

        public string FilesPattern
        {
            get;
            set;
        }

        public string FoldersPattern
        {
            get;
            set;
        }

        public List<string> IncludeSubStrings
        {
            get
            {
                return this._includeSubStrings;
            }
        }

        public bool OverwriteExistingFiles
        {
            get;
            set;
        }

        public bool RecurseFolders
        {
            get;
            set;
        }

        public bool VerboseLogging
        {
            get;
            set;
        }

        public Func<string, bool, bool> WantExcludeFunc
        {
            get;
            set;
        }

        public Func<string, bool, bool> WantIncludeFunc
        {
            get;
            set;
        }

        public FolderXCopyOptions()
        {
            this.FilesPattern = "*.*";
            this.FoldersPattern = "*";
        }

        public FolderXCopyOptions AddExcludeRegexSubStrings(params string[] items)
        {
            this.ExcludeRegexSubStrings.AddRange(items);
            return this;
        }

        public FolderXCopyOptions AddExcludeSubStrings(params string[] items)
        {
            this.ExcludeSubStrings.AddRange(items);
            return this;
        }

        public FolderXCopyOptions AddExcludeWildcardSubStrings(params string[] items)
        {
            if (items != null)
            {
                List<string> strs = new List<string>();
                string[] strArrays = items;
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    strs.Add(FolderXCopyOptions.convertWildcardToRegex(strArrays[i]));
                }
                this.ExcludeRegexSubStrings.AddRange(strs.ToArray());
            }
            return this;
        }

        public FolderXCopyOptions AddIncludeSubStrings(params string[] items)
        {
            this.IncludeSubStrings.AddRange(items);
            return this;
        }

        private static string convertWildcardToRegex(string pattern)
        {
            string str = string.Concat("^", Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", "."), "$");
            return str;
        }

        public FolderXCopyOptions SetAlwaysMatchFolderIncludes(bool value)
        {
            this.AlwaysMatchFolderIncludes = value;
            return this;
        }

        public FolderXCopyOptions SetCopyEmptyFolders(bool value)
        {
            this.CopyEmptyFolders = value;
            return this;
        }

        public FolderXCopyOptions SetCopyHiddenAndSystemFiles(bool value)
        {
            this.CopyHiddenAndSystemFiles = value;
            return this;
        }

        public FolderXCopyOptions SetCopyOnlyIfSourceIsNewer(bool value)
        {
            this.CopyOnlyIfSourceIsNewer = value;
            return this;
        }

        public FolderXCopyOptions SetExcludeCheckDelegate(Func<string, bool, bool> wantExclude)
        {
            this.WantExcludeFunc = wantExclude;
            return this;
        }

        public FolderXCopyOptions SetIncludeCheckDelegate(Func<string, bool, bool> wantInclude)
        {
            this.WantIncludeFunc = wantInclude;
            return this;
        }

        public FolderXCopyOptions SetOverwriteExistingFiles(bool value)
        {
            this.OverwriteExistingFiles = value;
            return this;
        }

        public FolderXCopyOptions SetRecurseFolders(bool value)
        {
            this.RecurseFolders = value;
            return this;
        }
    }
}