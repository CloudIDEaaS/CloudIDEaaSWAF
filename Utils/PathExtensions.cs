using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utils
{
    public static class PathExtensions
    {
        public static string EnvironmentPathCombine(string enviromentVariableNoPercents, params string[] paths)
        {
            var environmentVariableExpanded = Path.GetFullPath(Environment.ExpandEnvironmentVariables(enviromentVariableNoPercents.SurroundWith("%")));
            var possibleRelativePath = Path.Combine(new string[] { environmentVariableExpanded }.Concat(paths).ToArray());
            var path = Path.GetFullPath(possibleRelativePath);

            return path;
        }

        public static string GetPathFromFolder(this FileInfo file, string folder, bool includeFolderName = false)
        {
            var path = file.FullName;

            if (includeFolderName)
            {
                path = path.RemoveStart(path.IndexOf(folder));
            }
            else
            {
                path = path.RemoveStart(path.IndexOf(folder) + folder.Prepend(@"\").Length);
            }

            return path;
        }

        public static string GetPathFromFolder(this DirectoryInfo directory, string folder, bool includeFolderName = false)
        {
            var path = directory.FullName;

            if (includeFolderName)
            {
                path = path.RemoveStart(path.IndexOf(folder));
            }
            else
            {
                var index = path.IndexOf(folder);

                if (index + folder.Prepend(@"\").Length >= path.Length)
                {
                    return string.Empty;
                }
                else
                {
                    path = path.RemoveStart(index + folder.Prepend(@"\").Length);
                }
            }

            return path;
        }

        public static string GetSubPathFromFolder(this DirectoryInfo directory, DirectoryInfo rootDirectoryToTruncate, bool includeStartingSlash = false)
        {
            var folder = directory.FullName;
            var toTruncate = rootDirectoryToTruncate.FullName;
            var subPath = folder.Right(folder.Length - toTruncate.Length);

            if (includeStartingSlash)
            {
                if (subPath.StartsWith(@"\"))
                {
                    return subPath;
                }
                else
                {
                    return @"\" + subPath;
                }
            }
            else
            {
                if (subPath.StartsWith(@"\"))
                {
                    return subPath.RemoveStart(1);
                }
                else
                {
                    return subPath;
                }
            }
        }


        public static bool PathStartsWith(string path, string startingPart)
        {
            if (path == startingPart)
            {
                return true;
            }
            else if (path.StartsWith(startingPart + @"\"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static IEnumerable<DirectoryInfo> GetAncestors(this DirectoryInfo directory, bool includeSelf = true)
        {
            var list = new List<DirectoryInfo>();
            var parent = directory.Parent;

            if (includeSelf)
            {
                list.Add(directory);
            }

            while (parent != null)
            {
                list.Add(parent);

                parent = parent.Parent;
            }

            return list;
        }

        public static string GetParentPathToFolder(this DirectoryInfo directory, string folder, bool includeFolderName = false)
        {
            Func<DirectoryInfo, string> recurse = null;

            recurse = (d) =>
            {
                var parent = d.Parent;

                while (parent != null)
                {
                    if (parent.Name == folder)
                    {
                        if (includeFolderName)
                        {
                            return parent.FullName;
                        }
                        else
                        {
                            return parent.Parent.FullName;
                        }
                    }

                    parent = parent.Parent;
                }

                return null;
            };

            var subPath = recurse(directory);

            return subPath;
        }

        public static string GetSubPathToFolder(this DirectoryInfo directory, string folder, bool includeFolderName = false)
        {
            Func<DirectoryInfo, string> recurse = null;

            recurse = (d) =>
            {
                foreach (var subDirectory in d.GetDirectories())
                {
                    if (subDirectory.Name == folder)
                    {
                        if (includeFolderName)
                        {
                            return subDirectory.FullName.RemoveStart(directory.FullName.Append(@"\"));
                        }
                        else
                        {
                            return subDirectory.FullName.RemoveStart(directory.FullName.Append(@"\"));
                        }
                    }

                    var path = recurse(subDirectory);

                    if (path != null)
                    {
                        return path;
                    }
                }

                return null;
            };

            var subPath = recurse(directory);

            return subPath;
        }

        public static string GetSubPathToFile(this DirectoryInfo directory, string fileName, bool includeFileName = false)
        {
            Func<DirectoryInfo, string> recurse = null;

            fileName = Path.GetFileName(fileName);

            recurse = (d) =>
            {
                foreach (var file in d.GetFiles(fileName))
                {
                    if (file.Name.AsCaseless() == fileName)
                    {
                        if (includeFileName)
                        {
                            return file.FullName.RemoveStart(directory.FullName.Append(@"\"));
                        }
                        else
                        {
                            return d.FullName.RemoveStart(directory.FullName.Append(@"\"));
                        }
                    }
                }

                foreach (var subDirectory in d.GetDirectories())
                {
                    var path = recurse(subDirectory);
                             
                    if (path != null)
                    {
                        return path;
                    }
                }

                return null;
            };

            var subPath = recurse(directory);

            return subPath;
        }

        public static FileInfo GetFile(this DirectoryInfo directory, string fileName)
        {
            Func<DirectoryInfo, string> recurse = null;

            recurse = (d) =>
            {
                foreach (var file in d.GetFiles(Path.GetFileName(fileName)))
                {
                    if (file.Name.AsCaseless() == fileName)
                    {
                        return file.FullName;
                    }
                }

                foreach (var subDirectory in d.GetDirectories())
                {
                    var path = recurse(subDirectory);

                    if (path != null)
                    {
                        return path;
                    }
                }

                return null;
            };

            var filePath = recurse(directory);

            return new FileInfo(filePath);
        }

        public static string[] GetParts(this DirectoryInfo directory)
        {
            var parts = new List<string>();

            while (directory != null)
            {
                parts.Add(directory.Name);

                directory = directory.Parent;
            }

            return parts.AsEnumerable().Reverse().ToArray<string>();
        }

        public static bool PathIsValid(string path)
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (path.StartsWith(drive.Name))
                {
                    var removedDrivePath = path.RegexRemove("^" + drive.Name.Replace(@"\", @"\\").Replace(":", @"\:"));

                    var validPath = !removedDrivePath.Split('\\').Any(p =>
                    {
                        var invalid = p.Any(c => Path.GetInvalidFileNameChars().Contains(c));

                        return invalid;
                    });

                    return validPath;
                }
            }

            return false;
        }

        public static bool PathIsValidAndExists(string path)
        {
            return PathIsValid(path) && (Directory.Exists(path) || File.Exists(path));
        }
    }
}
