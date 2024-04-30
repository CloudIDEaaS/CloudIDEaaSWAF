using BTreeIndex.Collections.Generic.BTree;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Utils.BTreeIndex.FullText;
using Utils.MemoryMappedFiles;

namespace WebSecurity.KestrelWAF
{
    public class WAFDataFileCache : IWAFDataFileCache
    {
        private IManagedLockObject lockObject;
        private string dataFilesPath;
        private DirectoryInfo rawDataFilesDirectory;
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment environment;
        private readonly IServiceProvider serviceProvider;
        private readonly ActionQueueService actionQueueService;
        private readonly ILogger<WAFDataFileCache> logger;
        private readonly Dictionary<string, MappedFileInfo> mappedFiles;
        private readonly WeakDictionary<string, FullTextIndex<string>> fileIndexes;

        public WAFDataFileCache(IConfiguration configuration, IHostEnvironment environment, ActionQueueService actionQueueService, IServiceProvider serviceProvider, ILogger<WAFDataFileCache> logger)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.serviceProvider = serviceProvider;
            this.actionQueueService = actionQueueService;
            this.logger = logger;
            this.mappedFiles = new Dictionary<string, MappedFileInfo>();
            this.dataFilesPath = configuration["WAFDataFilesPath"]!;
            this.fileIndexes = new WeakDictionary<string, FullTextIndex<string>>();

            if (!actionQueueService.IsRunning)
            {
                actionQueueService.Start();
            }

            this.lockObject = LockManager.CreateObject();

            CacheFiles(dataFilesPath);
        }

        private void CacheFiles(string dataFilesPath)
        {
            actionQueueService.Run(() =>
            {
                using (lockObject.Lock())
                {
                    var directory = new DirectoryInfo(dataFilesPath);
                    var files = directory.GetFiles();
                    var fileCount = files.Length;
                    var fileIndex = 0;

                    rawDataFilesDirectory = new DirectoryInfo(dataFilesPath.RemoveEndIfMatches(@"\") + "Raw");

                    foreach (var file in files)
                    {
                        if (!rawDataFilesDirectory.HasFile(file.Name))
                        {
                            using (var configInputStream = File.OpenRead(file.FullName))
                            {
                                var properties = new JavaProperties();
                                var rawFilePath = Path.Combine(rawDataFilesDirectory.FullName, file.Name);
                                List<string> lines;
                                List<DictionaryEntry> entries;

                                Console.WriteLine("Caching {0} of {1} waf data files.", fileIndex + 1, fileCount);

                                properties.Load(configInputStream, file.Name);

                                entries = properties.Cast<DictionaryEntry>().ToList();
                                lines = entries.Select(p => p.GetLine()).OrderBy(l => l).ToList()!;

                                File.WriteAllText(rawFilePath, lines.ToMultiLineList());
                            }
                        }

                        fileIndex++;
                    }

                    files = rawDataFilesDirectory.GetFiles();

                    Debug.Assert(fileCount == files.Length);

                    foreach (var file in files)
                    {
                        var mappedFileInfo = file.MapFile();

                        mappedFiles.Add(file.Name, mappedFileInfo);
                    }
                }
            });
        }

        public IDisposable IndexFile(string fileName)
        {
            var disposable = this.CreateDisposable(fileIndexes.Cull);

            using (lockObject.Lock())
            {
                if (!fileIndexes.ContainsKey(fileName))
                {
                    var index = new FullTextIndex<string>();
                    MappedFileInfo mappedFileInfo;
                    string content;

                    Debug.Assert(File.Exists(fileName), $"File {fileName} does not exist");

                    if (!mappedFiles.ContainsKey(fileName))
                    {
                        throw new IOException($"File {fileName} exists but is not cached");
                    }

                    mappedFileInfo = mappedFiles[fileName];
                    content = mappedFileInfo.MapFileGetContent<string>();

                    index.AddText(content, fileName);

                    fileIndexes.Add(fileName, index);
                }
            }

            return disposable;
        }

        public bool PartialMatchFromFile(string fileName, string searchText)
        {
            using (lockObject.Lock())
            {
                if (fileIndexes.ContainsKey(fileName))
                {
                    var index = fileIndexes[fileName];

                    return index.ContainsKey(searchText);
                }
                else
                {
                    MappedFileInfo mappedFileInfo;
                    string content;
                    var fullName = Path.Combine(rawDataFilesDirectory.FullName, fileName);    

                    Debug.Assert(File.Exists(fullName), $"File {fileName} does not exist");

                    if (!mappedFiles.ContainsKey(fileName))
                    {
                        throw new IOException($"File {fileName} exists but is not cached");
                    }

                    mappedFileInfo = mappedFiles[fileName];
                    content = mappedFileInfo.MapFileGetContent<string>();

                    if (content.Contains(searchText))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
