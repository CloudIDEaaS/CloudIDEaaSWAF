using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utils;
using WAFWebSample.Data;
using WebSecurity;
using WebSecurity.KestrelWAF;
using WebSecurity.Models;

namespace WAFWebSample.WebApi.Providers.WAF.Models
{
    public class CrsGlobal : IGlobal
    {
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment environment;
        private readonly IServiceProvider serviceProvider;
        private readonly ActionQueueService actionQueueService;
        private readonly IWAFDataFileCache wafDataFileCache;
        private readonly ILoggerFactory? loggerFactory;
        private readonly ILogger logger;
        private WAFGlobalSettings? globalSettings;
        private bool needsAdd;

        public CrsGlobal(IConfiguration configuration, IHostEnvironment environment, ActionQueueService actionQueueService, IWAFDataFileCache wafDataFileCache, IServiceProvider serviceProvider)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.serviceProvider = serviceProvider;
            this.actionQueueService = actionQueueService;
            this.wafDataFileCache = wafDataFileCache;

            if (!actionQueueService.IsRunning)
            {
                actionQueueService.Start();
            }

            loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            logger = loggerFactory.CreateLogger(nameof(CrsGlobal));

            using (var scope = serviceProvider.CreateScope()) 
            {
                var hydraDbContext = scope.ServiceProvider.GetService<WAFWebSampleDbContext>();
				var wafGlobalSettings = hydraDbContext.WAFGlobalSettings;

                globalSettings = wafGlobalSettings.OrderByDescending(s => s.CrsSetupVersion).FirstOrDefault();

				if (globalSettings == null) 
				{
					needsAdd = true;
				}
            }
        }

        public string? CrsSetupVersion
        {
            get
            {
                var version = new SemanticVersion(globalSettings.CrsSetupVersion!);

                return version.CompactVersion;
            }

            set
            {
                if (this.globalSettings == null)
                {
                    this.globalSettings = new WAFGlobalSettings();
                }

                this.globalSettings.CrsSetupVersion = value;
            }
        }

        public long? MaxFileSize
		{
			get
			{
				if (this.globalSettings != null)
				{
					return this.globalSettings.MaxFileSize;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.globalSettings == null)
				{
					this.globalSettings = new WAFGlobalSettings();
				}

                this.globalSettings.MaxFileSize = value;
            }
        }

        public int? MaxNumArgs
		{
			get
			{
				if (this.globalSettings != null)
				{
					return this.globalSettings.MaxNumArgs;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.globalSettings == null)
				{
					this.globalSettings = new WAFGlobalSettings();
				}

                this.globalSettings.MaxNumArgs = value;
            }
        }

        public int? TotalArgLength
		{
			get
			{
				if (this.globalSettings != null)
				{
					return this.globalSettings.TotalArgLength;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.globalSettings == null)
				{
					this.globalSettings = new WAFGlobalSettings();
				}

                this.globalSettings.TotalArgLength = value;
            }
        }

        public bool? EnableDefaultCollections
        {
            get
            {
                if (this.globalSettings != null)
                {
                    return this.globalSettings.EnableDefaultCollections;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.globalSettings == null)
                {
                    this.globalSettings = new WAFGlobalSettings();
                }

                this.globalSettings.EnableDefaultCollections = value;
            }
        }

        public object? this[string indexOrRegex]
        {
            get
            {
                return this.GetValue<object>(indexOrRegex);
            }

            set
            {
                this.SetValue(indexOrRegex, value);
            }
        }

        public bool PartialMatchFromFile(string filename, string searchText)
        {
            return wafDataFileCache.PartialMatchFromFile(filename, searchText);
        }

        public IDisposable IndexFile(string filename)
        {
            return wafDataFileCache.IndexFile(filename);
        }

        public void Save()
        {
            actionQueueService.Run(() =>
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var hydraDbContext = scope.ServiceProvider.GetService<WAFWebSampleDbContext>();
                    var wafGlobalSettings = hydraDbContext.WAFGlobalSettings;

                    if (needsAdd)
                    {
                        wafGlobalSettings.Add(globalSettings);
                    }

                    hydraDbContext.SaveChanges();
                }
            });
        }
    }
}
