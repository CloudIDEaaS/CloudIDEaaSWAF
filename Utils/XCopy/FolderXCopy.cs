using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZetaLongPaths;
using FileAttributes = ZetaLongPaths.Native.FileAttributes;

namespace Utils.XCopy
{
    public sealed class FolderXCopy
    {
        private bool _VERBOSE;

        private static bool _isFirstLog;

        private static string _scriptFile;

        private static string scriptFileName
        {
            get
            {
                return Path.GetFileName(FolderXCopy._scriptFile);
            }
        }

        private static string scriptFilePath
        {
            get
            {
                return FolderXCopy._scriptFile;
            }
        }

        private static string scriptFolderPath
        {
            get
            {
                return Path.GetDirectoryName(FolderXCopy._scriptFile).TrimEnd(new char[] { '\\' });
            }
        }

        static FolderXCopy()
        {
            FolderXCopy._isFirstLog = true;
            FolderXCopy._scriptFile = (new StackTrace(new StackFrame(true))).GetFrame(0).GetFileName();
        }

        public FolderXCopy()
        {
        }

        private string CheckCreateFolder(string folderPath)
        {
            if ((folderPath == null ? false : !ZlpIOHelper.DirectoryExists(folderPath)))
            {
                this.verboseLog("Creating folder '{0}'.", new object[] { folderPath });
                ZlpSimpleFileAccessProtector.Protect(() => ZlpIOHelper.CreateDirectory(folderPath), null);
            }
            return folderPath;
        }

        public FolderXCopyResult Copy(string sourceFolderPath, string destinationFolderPath, FolderXCopyOptions options)
        {
            this._VERBOSE = options.VerboseLogging;
            sourceFolderPath = ZlpPathHelper.GetFullPath(sourceFolderPath);
            destinationFolderPath = ZlpPathHelper.GetFullPath(destinationFolderPath);
            this.dumpOptions(sourceFolderPath, destinationFolderPath, options);
            FolderXCopyResult folderXCopyResult = new FolderXCopyResult();
            DateTime now = DateTime.Now;
            this.CopyFolderTree(sourceFolderPath, destinationFolderPath, options, folderXCopyResult);
            this.dumpResult(sourceFolderPath, destinationFolderPath, now, folderXCopyResult);
            return folderXCopyResult;
        }

        private void CopyFile(string sourceFilePath, string destinationFilePath, FolderXCopyOptions options)
        {
            string directoryPathNameFromFilePath = ZlpPathHelper.GetDirectoryPathNameFromFilePath(destinationFilePath);
            if (!ZlpIOHelper.DirectoryExists(directoryPathNameFromFilePath))
            {
                this.verboseLog("Creating folder '{0}'.", new object[] { directoryPathNameFromFilePath });
                ZlpSimpleFileAccessProtector.Protect(() => ZlpIOHelper.CreateDirectory(directoryPathNameFromFilePath), null);
            }
            this.verboseLog("Copying file from '{0}' to '{1}'.", new object[] { sourceFilePath, destinationFilePath });
            ZlpSimpleFileAccessProtector.Protect(() => ZlpIOHelper.CopyFile(sourceFilePath, destinationFilePath, options.OverwriteExistingFiles), null);
        }

        private void CopyFolderTree(string sourceFolderPath, string destinationFolderPath, FolderXCopyOptions options, FolderXCopyResult result)
        {
            string str;
            string str1;
            this.verboseLog("");
            this.verboseLog("**************");
            this.verboseLog("Copying folder tree '{0}' to '{1}'.", new object[] { sourceFolderPath, destinationFolderPath });
            string str2 = destinationFolderPath;
            if (options.CopyEmptyFolders)
            {
                this.CheckCreateFolder(str2);
            }
            string[] files = FolderXCopy.getFiles(sourceFolderPath, options.FilesPattern);
            this.verboseLog("Got {0} files in source folder '{1}' with pattern '{2}'.", new object[] { (int)files.Length, sourceFolderPath, options.FilesPattern });
            if (files != null)
            {
                string[] strArrays = files;
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    string str3 = strArrays[i];
                    string str4 = FolderXCopy.PathHelperCombine(str2, ZlpPathHelper.GetFileNameFromFilePath(str3));
                    if (!FolderXCopy.OnProgressFile(new ZlpFileInfo(str3), new ZlpFileInfo(str4), options, out str))
                    {
                        FolderXCopyResult skippedFileCount = result;
                        skippedFileCount.SkippedFileCount = skippedFileCount.SkippedFileCount + 1;
                        this.verboseLog("NOT copying file '{0}' to '{1}', reason '{2}'.", new object[] { str3, str4, str });
                    }
                    else
                    {
                        FolderXCopyResult copiedFileCount = result;
                        copiedFileCount.CopiedFileCount = copiedFileCount.CopiedFileCount + 1;
                        this.verboseLog("COPYING file '{0}' to '{1}', reason '{2}'.", new object[] { str3, str4, str });
                        this.CheckCreateFolder(str2);
                        this.CopyFile(str3, str4, options);
                    }
                }
            }
            if (!options.RecurseFolders)
            {
                this.verboseLog("NOT recursing folders.");
            }
            else
            {
                this.verboseLog("RECURSING folders.");
                ZlpDirectoryInfo[] directories = ZlpIOHelper.GetDirectories(sourceFolderPath, options.FoldersPattern);
                this.verboseLog("Got {0} child folders in source folder '{1}' with pattern '{2}'.", new object[] { (int)directories.Length, sourceFolderPath, options.FoldersPattern });
                if (directories != null)
                {
                    ZlpDirectoryInfo[] zlpDirectoryInfoArray = directories;
                    for (int j = 0; j < (int)zlpDirectoryInfoArray.Length; j++)
                    {
                        ZlpDirectoryInfo zlpDirectoryInfo = zlpDirectoryInfoArray[j];
                        ZlpDirectoryInfo zlpDirectoryInfo1 = zlpDirectoryInfo;
                        if ((!FolderXCopy.isFolderEmpty(zlpDirectoryInfo1) ? true : options.CopyEmptyFolders))
                        {
                            string str5 = zlpDirectoryInfo.FullName.Substring(sourceFolderPath.Length);
                            ZlpDirectoryInfo zlpDirectoryInfo2 = new ZlpDirectoryInfo(FolderXCopy.PathHelperCombine(str2, str5));
                            if (!FolderXCopy.OnProgressFolder(zlpDirectoryInfo1, zlpDirectoryInfo2, options, out str1))
                            {
                                FolderXCopyResult skippedFolderCount = result;
                                skippedFolderCount.SkippedFolderCount = skippedFolderCount.SkippedFolderCount + 1;
                                this.verboseLog("NOT recursing into folder '{0}' (destination folder '{1}', reason '{2}').", new object[] { zlpDirectoryInfo1.FullName, zlpDirectoryInfo2.FullName, str1 });
                            }
                            else
                            {
                                FolderXCopyResult copiedFolderCount = result;
                                copiedFolderCount.CopiedFolderCount = copiedFolderCount.CopiedFolderCount + 1;
                                this.verboseLog("Recursing into folder '{0}' (destination folder '{1}', reason '{2}').", new object[] { zlpDirectoryInfo1.FullName, zlpDirectoryInfo2, str1 });
                                this.CopyFolderTree(zlpDirectoryInfo1.FullName, zlpDirectoryInfo2.FullName, options, result);
                            }
                        }
                    }
                }
            }
        }

        private static bool doesStringContainRegexSubString(string searchIn, List<string> subStrings, out string reason)
        {
            bool flag;
            if (string.IsNullOrEmpty(searchIn))
            {
                reason = "Nothing to search in.";
                flag = false;
            }
            else if (subStrings.Count > 0)
            {
                foreach (string subString in subStrings)
                {
                    if ((new Regex(subString, RegexOptions.IgnoreCase)).IsMatch(searchIn))
                    {
                        reason = string.Format("Regex match with '{0}'.", subString);
                        flag = true;
                        return flag;
                    }
                }
                reason = "No regex match.";
                flag = false;
            }
            else
            {
                reason = "Nothing present => ALL.";
                flag = true;
            }
            return flag;
        }

        private static bool doesStringContainSubString(string searchIn, List<string> subStrings, out string reason)
        {
            bool flag;
            if (string.IsNullOrEmpty(searchIn))
            {
                reason = "Nothing to search in.";
                flag = false;
            }
            else if (subStrings.Count > 0)
            {
                foreach (string subString in subStrings)
                {
                    if (searchIn.IndexOf(subString, StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        reason = string.Format("Substring match with '{0}'.", subString);
                        flag = true;
                        return flag;
                    }
                }
                reason = "No substring match.";
                flag = false;
            }
            else
            {
                reason = "Nothing present => ALL.";
                flag = true;
            }
            return flag;
        }

        private static bool doesStringNotContainSubString(string searchIn, List<string> subStrings, out string reason)
        {
            return !FolderXCopy.doesStringContainSubString(searchIn, subStrings, out reason);
        }

        private void dumpOptions(string sourceFolderPath, string destinationFolderPath, FolderXCopyOptions options)
        {
            this.verboseLog("+++++++++++++++++");
            this.verboseLog("Starting XCOPY folder tree.");
            this.verboseLog("\tSource = '{0}' (exists: {1}).", new object[] { sourceFolderPath, Directory.Exists(sourceFolderPath) });
            this.verboseLog("\tDestination = '{0}' (exists: {1}).", new object[] { destinationFolderPath, Directory.Exists(destinationFolderPath) });
            this.verboseLog("\tXCOPY options:");
            this.verboseLog("\t\tVerbose logging = '{0}'.", new object[] { options.VerboseLogging });
            this.verboseLog("\t\tRecurse folders = '{0}'.", new object[] { options.RecurseFolders });
            this.verboseLog("\t\tCopy empty folders = '{0}'.", new object[] { options.CopyEmptyFolders });
            this.verboseLog("\t\tCopy hidden and system files = '{0}'.", new object[] { options.CopyHiddenAndSystemFiles });
            this.verboseLog("\t\tOverwrite existing files = '{0}'.", new object[] { options.OverwriteExistingFiles });
            this.verboseLog("\t\tCopy only if source is newer = '{0}'.", new object[] { options.CopyOnlyIfSourceIsNewer });
            this.verboseLog("\t\tFiles pattern = '{0}'.", new object[] { options.FilesPattern });
            this.verboseLog("\t\tFolders pattern = '{0}'.", new object[] { options.FoldersPattern });
            this.verboseLog("\t\tAlways match folder includes = '{0}'.", new object[] { options.AlwaysMatchFolderIncludes });
            this.verboseLog("\t\tExclude substrings ({0}):", new object[] { options.ExcludeSubStrings.Count });
            for (int i = 0; i < options.ExcludeSubStrings.Count; i++)
            {
                this.verboseLog("\t\t\tExclude substring = '{0}'.", new object[] { options.ExcludeSubStrings[i] });
            }
            this.verboseLog("\t\tExclude regex substrings ({0}):", new object[] { options.ExcludeRegexSubStrings.Count });
            for (int j = 0; j < options.ExcludeRegexSubStrings.Count; j++)
            {
                this.verboseLog("\t\t\tExclude regex substring = '{0}'.", new object[] { options.ExcludeRegexSubStrings[j] });
            }
            this.verboseLog("\t\tInclude substrings ({0}):", new object[] { options.IncludeSubStrings.Count });
            for (int k = 0; k < options.IncludeSubStrings.Count; k++)
            {
                this.verboseLog("\t\t\tInclude substring = '{0}'.", new object[] { options.IncludeSubStrings[k] });
            }
            this.verboseLog("-----------------");
        }

        private void dumpResult(string sourceFolderPath, string destinationFolderPath, DateTime start, FolderXCopyResult result)
        {
            TimeSpan now = DateTime.Now - start;
            this.verboseLog("-----------------");
            this.verboseLog("\tXCOPY result:");
            this.verboseLog("\t\tCopied folder count = '{0}'.", new object[] { result.CopiedFolderCount });
            this.verboseLog("\t\tCopied file count = '{0}'.", new object[] { result.CopiedFileCount });
            this.verboseLog("\t\tSkipped folder count = '{0}'.", new object[] { result.SkippedFolderCount });
            this.verboseLog("\t\tSkipped file count = '{0}'.", new object[] { result.SkippedFileCount });
            this.verboseLog("Finished XCOPY folder tree in {0} from '{1}' to '{2}'.", new object[] { FolderXCopy.formatTimeSpan(now), sourceFolderPath, destinationFolderPath });
            this.verboseLog("+++++++++++++++++");
        }

        private static string formatTimeSpan(TimeSpan ts)
        {
            int seconds;
            string str;
            string str1;
            double totalSeconds = ts.TotalSeconds;
            if (totalSeconds < 60)
            {
                if (ts.Seconds == 1)
                {
                    str1 = "one second ago";
                }
                else
                {
                    seconds = ts.Seconds;
                    str1 = string.Concat(seconds.ToString(), " seconds ago");
                }
                str = str1;
            }
            else if (totalSeconds < 120)
            {
                str = "a minute ago";
            }
            else if (totalSeconds < 2700)
            {
                seconds = ts.Minutes;
                str = string.Concat(seconds.ToString(), " minutes ago");
            }
            else if (totalSeconds < 5400)
            {
                str = "an hour ago";
            }
            else if (totalSeconds < 86400)
            {
                seconds = ts.Hours;
                str = string.Concat(seconds.ToString(), " hours ago");
            }
            else if (totalSeconds < 172800)
            {
                str = "yesterday";
            }
            else if (totalSeconds < 2592000)
            {
                seconds = ts.Days;
                str = string.Concat(seconds.ToString(), " days ago");
            }
            else if (totalSeconds >= 31104000)
            {
                int num = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                str = (num <= 1 ? "one year ago" : string.Concat(num.ToString(), " years ago"));
            }
            else
            {
                int num1 = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                str = (num1 <= 1 ? "one month ago" : string.Concat(num1.ToString(), " months ago"));
            }
            return str;
        }

        private static string[] getFiles(string path, string searchPattern)
        {
            return FolderXCopy.getFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        private static string[] getFiles(string path, string searchPattern, SearchOption searchOption)
        {
            Action<ZlpFileInfo> action = null;
            string[] strArrays = searchPattern.Split(new char[] { ';', ',', '|' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> strs = new List<string>();
            string[] strArrays1 = strArrays;
            for (int i = 0; i < (int)strArrays1.Length; i++)
            {
                string str = strArrays1[i];
                List<ZlpFileInfo> zlpFileInfos = new List<ZlpFileInfo>(ZlpIOHelper.GetFiles(path, str.Trim(), searchOption));
                Action<ZlpFileInfo> action1 = action;
                if (action1 == null)
                {
                    Action<ZlpFileInfo> action2 = (ZlpFileInfo x) => strs.Add(x.FullName);
                    Action<ZlpFileInfo> action3 = action2;
                    action = action2;
                    action1 = action3;
                }
                zlpFileInfos.ForEach(action1);
            }
            strs.Sort();
            return strs.ToArray();
        }

        private static bool IsFileOneNewerThanFileTwo(string one, string two)
        {
            bool fileLastWriteTime;
            if ((string.IsNullOrEmpty(one) ? true : !ZlpIOHelper.FileExists(one)))
            {
                fileLastWriteTime = false;
            }
            else if ((string.IsNullOrEmpty(two) ? false : ZlpIOHelper.FileExists(two)))
            {
                DateTime dateTime = ZlpIOHelper.GetFileLastWriteTime(one);
                fileLastWriteTime = dateTime > ZlpIOHelper.GetFileLastWriteTime(two);
            }
            else
            {
                fileLastWriteTime = true;
            }
            return fileLastWriteTime;
        }

        private static bool IsFileOneNewerThanFileTwo(ZlpFileInfo one, ZlpFileInfo two)
        {
            bool flag;
            if (one != null)
            {
                flag = (two != null ? FolderXCopy.IsFileOneNewerThanFileTwo(one.FullName, two.FullName) : true);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        private static bool isFolderEmpty(ZlpDirectoryInfo folderPath)
        {
            return folderPath.GetFiles().Length == 0;
        }

        private static bool IsSystemOrHiddenFile(ZlpFileInfo filePath)
        {
            bool flag;
            if ((filePath == null ? false : filePath.Exists))
            {
                FileAttributes attributes = filePath.Attributes;
                flag = (((int)(attributes & FileAttributes.Hidden) != 0 ? false : (int)(attributes & FileAttributes.System) == 0) ? false : true);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        private static bool IsSystemOrHiddenFolder(ZlpDirectoryInfo folderPath)
        {
            bool flag;
            if ((folderPath == null ? false : folderPath.Exists))
            {
                FileAttributes attributes = folderPath.Attributes;
                flag = (((int)(attributes & FileAttributes.Hidden) != 0 ? false : (int)(attributes & FileAttributes.System) == 0) ? false : true);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        private static void log()
        {
            FolderXCopy.log(string.Empty);
        }

        private static void log(string text)
        {
            FolderXCopy.log(text, new object[0]);
        }

        private static void log(string text, params object[] args)
        {
            try
            {
                Console.WriteLine(text, args);
                string str = Path.ChangeExtension(FolderXCopy.scriptFilePath, ".log");
                if ((!FolderXCopy._isFirstLog ? false : File.Exists(str)))
                {
                    File.Delete(str);
                }
                FolderXCopy._isFirstLog = false;
                if (!string.IsNullOrEmpty(text))
                {
                    string str1 = string.Format(string.Concat("[{0}] {1}", Environment.NewLine), DateTime.Now, string.Format(text, args));
                    File.AppendAllText(str, str1);
                    string environmentVariable = Environment.GetEnvironmentVariable("CENTRAL_LOGFILEPATH");
                    if (!string.IsNullOrEmpty(environmentVariable))
                    {
                        File.AppendAllText(environmentVariable, str1);
                    }
                }
            }
            catch (Exception exception)
            {
            }
        }

        private static void log(object o)
        {
            FolderXCopy.log((o == null ? string.Empty : o.ToString()));
        }

        private static bool OnProgressFile(ZlpFileInfo sourceFilePath, ZlpFileInfo destinationFilePath, FolderXCopyOptions options, out string reason)
        {
            bool flag;
            string empty = string.Empty;
            if ((options.CopyHiddenAndSystemFiles ? false : FolderXCopy.IsSystemOrHiddenFile(sourceFilePath)))
            {
                reason = "File Not copying system file.";
                flag = false;
            }
            else if ((!options.CopyOnlyIfSourceIsNewer ? false : !FolderXCopy.IsFileOneNewerThanFileTwo(sourceFilePath, destinationFilePath)))
            {
                reason = "File Not copying older file.";
                flag = false;
            }
            else if ((options.OverwriteExistingFiles ? false : destinationFilePath.Exists))
            {
                reason = "File Not overwriting destination file.";
                flag = false;
            }
            else if ((options.WantExcludeFunc == null ? false : options.WantExcludeFunc(sourceFilePath.FullName, true)))
            {
                reason = string.Format("File exclude function match.", Array.Empty<object>());
                flag = false;
            }
            else if ((options.ExcludeSubStrings.Count <= 0 ? false : FolderXCopy.doesStringContainSubString(sourceFilePath.FullName, options.ExcludeSubStrings, out empty)))
            {
                reason = string.Format("File Substring exclude match. '{0}'.", empty);
                flag = false;
            }
            else if ((options.ExcludeRegexSubStrings.Count <= 0 ? false : FolderXCopy.doesStringContainRegexSubString(sourceFilePath.FullName, options.ExcludeRegexSubStrings, out empty)))
            {
                reason = string.Format("File Substring regex exclude match. '{0}'.", empty);
                flag = false;
            }
            else if ((options.WantIncludeFunc == null ? false : options.WantIncludeFunc(sourceFilePath.FullName, true)))
            {
                reason = string.Format("File include function match.", Array.Empty<object>());
                flag = true;
            }
            else if ((options.IncludeSubStrings.Count <= 0 ? false : !FolderXCopy.doesStringContainSubString(sourceFilePath.FullName, options.IncludeSubStrings, out empty)))
            {
                reason = string.Format("File Substring include nothing matches. '{0}'.", empty);
                flag = false;
            }
            else
            {
                reason = string.Format("File Substring include match. '{0}'.", empty);
                flag = true;
            }
            return flag;
        }

        private static bool OnProgressFolder(ZlpDirectoryInfo sourceFolderPath, ZlpDirectoryInfo destinationFolderPath, FolderXCopyOptions options, out string reason)
        {
            bool flag;
            string empty = string.Empty;
            string str = string.Concat(sourceFolderPath.FullName.TrimEnd(new char[] { '\\' }), "\\");
            if ((options.WantExcludeFunc == null ? false : options.WantExcludeFunc(str, false)))
            {
                reason = string.Format("Folder exclude function match.", Array.Empty<object>());
                flag = false;
            }
            else if ((options.ExcludeSubStrings.Count <= 0 ? false : FolderXCopy.doesStringContainSubString(str, options.ExcludeSubStrings, out empty)))
            {
                reason = string.Format("Folder Substring exclude match. '{0}'.", empty);
                flag = false;
            }
            else if ((options.ExcludeRegexSubStrings.Count <= 0 ? false : FolderXCopy.doesStringContainRegexSubString(str, options.ExcludeRegexSubStrings, out empty)))
            {
                reason = string.Format("Folder Substring regex exclude match. '{0}'.", empty);
                flag = false;
            }
            else if ((options.WantIncludeFunc == null ? false : options.WantIncludeFunc(str, false)))
            {
                reason = string.Format("Folder include function match.", Array.Empty<object>());
                flag = true;
            }
            else if (options.AlwaysMatchFolderIncludes)
            {
                reason = string.Format("Options always match folder includes is active.", Array.Empty<object>());
                flag = true;
            }
            else if ((options.IncludeSubStrings.Count <= 0 ? false : !FolderXCopy.doesStringContainSubString(str, options.IncludeSubStrings, out empty)))
            {
                reason = string.Format("Folder Substring include nothing matches. '{0}'.", empty);
                flag = false;
            }
            else
            {
                reason = string.Format("Folder Substring include match. '{0}'.", empty);
                flag = true;
            }
            return flag;
        }

        private static string PathHelperCombine(string path1, string path2)
        {
            string str;
            if (string.IsNullOrEmpty(path1))
            {
                str = path2;
            }
            else if (!string.IsNullOrEmpty(path2))
            {
                path1 = path1.TrimEnd(new char[] { '\\', '/' }).Replace('/', '\\');
                path2 = path2.TrimStart(new char[] { '\\', '/' }).Replace('/', '\\');
                str = string.Concat(path1, "\\", path2);
            }
            else
            {
                str = path1;
            }
            return str;
        }

        private void verboseLog(string text, params object[] args)
        {
            this.verboseLog(string.Format(text, args));
        }

        private void verboseLog(string text)
        {
            if (this._VERBOSE)
            {
                FolderXCopy.log(string.Format("[VERBOSE {0}] {1}", DateTime.Now, text).Trim());
            }
        }
    }
}