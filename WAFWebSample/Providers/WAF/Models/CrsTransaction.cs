using Microsoft.EntityFrameworkCore;
using Utils;
using WAFWebSample.Data;
using WebSecurity.Models;

namespace WAFWebSample.WebApi.Providers.WAF.Models
{
    public class CrsTransaction : ITransaction
    {
        private IConfiguration configuration;
        private IHostEnvironment environment;
        private WAFWebSampleDbContext hydraDbContext;
        private IServiceProvider serviceProvider;
        private IHttpContextAccessor httpContextAccessor;
        private HttpContext? httpContext;
        private ActionQueueService actionQueueService;
        private ILoggerFactory? loggerFactory;
        private ILogger logger;
        private WAFTransactionSettings? transactionSettings;
        private DbSet<WAFTransactionSettings> wafTransactionSettings;
        private bool needsAdd;

        public CrsTransaction(IConfiguration configuration, IHostEnvironment environment, WAFWebSampleDbContext hydraDbContext, IHttpContextAccessor httpContextAccessor, ActionQueueService actionQueueService, IServiceProvider serviceProvider)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.hydraDbContext = hydraDbContext;
            this.serviceProvider = serviceProvider;
            this.httpContextAccessor = httpContextAccessor;
            this.httpContext = httpContextAccessor.HttpContext;
            this.actionQueueService = actionQueueService;

            if (!actionQueueService.IsRunning)
            {
                actionQueueService.Start();
            }

            loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            logger = loggerFactory.CreateLogger(nameof(CrsTransaction));

            this.ConnectionId = httpContext.Connection.Id;

            wafTransactionSettings = hydraDbContext.WAFTransactionSettings;

            transactionSettings = wafTransactionSettings.Where(s => s.ConnectionId == this.ConnectionId).OrderByDescending(s => s.UpdateDate).FirstOrDefault();

            if (transactionSettings == null)
            {
                needsAdd = true;
            }
        }

        public string? ConnectionId 
        { 
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.ConnectionId;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

                    if (needsAdd)
                    {
                        wafTransactionSettings.Add(transactionSettings);
                    }
                }

                this.transactionSettings.ConnectionId = value;
            }
        }

        public int? InboundAnomalyScoreThreshold
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.InboundAnomalyScoreThreshold;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.InboundAnomalyScoreThreshold = value;
            }
        }

        public int? OutboundAnomalyScoreThreshold
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.OutboundAnomalyScoreThreshold;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.OutboundAnomalyScoreThreshold = value;
            }
        }

        public int? ReportingLevel
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.ReportingLevel;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.ReportingLevel = value;
            }
        }

        public bool? EarlyBlocking
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.EarlyBlocking;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.EarlyBlocking = value;
            }
        }

        public int? BlockingParanoiaLevel
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.BlockingParanoiaLevel;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.BlockingParanoiaLevel = value;
            }
        }

        public int? DetectionParanoiaLevel
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.DetectionParanoiaLevel;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.DetectionParanoiaLevel = value;
			}
		}

        public int? _922100Charset
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings._922100Charset;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings._922100Charset = value;
			}
		}

        public int? ArgLength
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.ArgLength;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.ArgLength = value;
			}
		}

        public int? ArgNameLength
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.ArgNameLength;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.ArgNameLength = value;
			}
		}

        public int? BlockingAnomalyScore
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.BlockingAnomalyScore;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.BlockingAnomalyScore = value;
			}
		}

        public int? BlockingInboundAnomalyScore
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.BlockingInboundAnomalyScore;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.BlockingInboundAnomalyScore = value;
			}
		}

        public int? BlockingOutboundAnomalyScore
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.BlockingOutboundAnomalyScore;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.BlockingOutboundAnomalyScore = value;
			}
		}

        public int? CombinedFileSizes
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.CombinedFileSizes;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.CombinedFileSizes = value;
			}
		}

        public string? ContentType
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.ContentType;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.ContentType = value;
			}
		}

        public string? ContentTypeCharset
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.ContentTypeCharset;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.ContentTypeCharset = value;
			}
		}

        public int? CriticalAnomalyScore
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.CriticalAnomalyScore;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.CriticalAnomalyScore = value;
			}
		}

        public int? DetectionAnomalyScore
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.DetectionAnomalyScore;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.DetectionAnomalyScore = value;
			}
		}

        public int? DetectionInboundAnomalyScore
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.DetectionInboundAnomalyScore;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.DetectionInboundAnomalyScore = value;
			}
		}

        public int? DetectionOutboundAnomalyScore
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.DetectionOutboundAnomalyScore;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.DetectionOutboundAnomalyScore = value;
			}
		}

        public int? ErrorAnomalyScore
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.ErrorAnomalyScore;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.ErrorAnomalyScore = value;
			}
		}

        public string? Extension
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.Extension;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.Extension = value;
			}
		}

        public int? NoticeAnomalyScore
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.NoticeAnomalyScore;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.NoticeAnomalyScore = value;
			}
		}

        public int? SamplingPercentage
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.SamplingPercentage;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

                    if (needsAdd)
                    {
                        wafTransactionSettings.Add(transactionSettings);
                    }
                }

                this.transactionSettings.SamplingPercentage = value;
            }
        }

        public int? SamplingRnd100
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.SamplingRnd100;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.SamplingRnd100 = value;
			}
		}

        public int? WarningAnomalyScore
		{
			get
			{
				if (this.transactionSettings != null)
				{
					return this.transactionSettings.WarningAnomalyScore;
				}
				else
				{
					return null;
				}
			}

			set
			{
				if (this.transactionSettings == null)
				{
					this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.WarningAnomalyScore = value;
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

        public ICollection<string>? AllowedMethods
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.AllowedMethods.SplitNullToEmpty(" ").ToList();
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.AllowedMethods = value?.Join(" ");
            }
        }

        public ICollection<string>? AllowedRequestContentType
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.AllowedRequestContentType.SplitNullToEmpty(" ").ToList();
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.AllowedRequestContentType = value?.Join(" ");
            }
        }

        public ICollection<string>? AllowedRequestContentTypeCharset
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.AllowedRequestContentTypeCharset.SplitNullToEmpty(" ").ToList();
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.AllowedRequestContentTypeCharset = value?.Join(" ");
            }
        }

        public ICollection<string>? AllowedHttpVersions
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.AllowedHttpVersions.SplitNullToEmpty(" ").ToList();
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.AllowedHttpVersions = value?.Join(" ");
            }
        }

        public ICollection<string>? RestrictedExtensions
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.RestrictedExtensions.SplitNullToEmpty(" ").ToList();
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.RestrictedExtensions = value?.Join(" ");
            }
        }

        public ICollection<string>? RestrictedHeadersBasic
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.RestrictedHeadersBasic.SplitNullToEmpty(" ").ToList();
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.RestrictedHeadersBasic = value?.Join(" ");
            }
        }

        public ICollection<string>? RestrictedHeadersExtended
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.RestrictedHeadersExtended.SplitNullToEmpty(" ");
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.RestrictedHeadersExtended = value?.Join(" ");
            }
        }

        public bool? EnforceBodyprocUrlencoded
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.EnforceBodyprocUrlencoded;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.EnforceBodyprocUrlencoded = value;
            }
        }

        public bool? CrsValidateUtf8Encoding
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.CrsValidateUtf8Encoding;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.CrsValidateUtf8Encoding = value;
            }
        }

        public int? InboundAnomalyScorePl1
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.InboundAnomalyScorePl1;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.InboundAnomalyScorePl1 = value;
            }
        }

        public int? InboundAnomalyScorePl2
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.InboundAnomalyScorePl2;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.InboundAnomalyScorePl2 = value;
            }
        }

        public int? InboundAnomalyScorePl3
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.InboundAnomalyScorePl3;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.InboundAnomalyScorePl3 = value;
            }
        }

        public int? InboundAnomalyScorePl4
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.InboundAnomalyScorePl4;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.InboundAnomalyScorePl4 = value;
            }
        }

        public int? SqlInjectionScore
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.SqlInjectionScore;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.SqlInjectionScore = value;
            }
        }

        public int? XssScore
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.XssScore;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.XssScore = value;
            }
        }

        public int? RfiScore
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.RfiScore;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.RfiScore = value;
            }
        }

        public int? LfiScore
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.LfiScore;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.LfiScore = value;
            }
        }

        public int? RceScore
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.RceScore;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.RceScore = value;
            }
        }

        public int? PhpInjectionScore
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.PhpInjectionScore;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.PhpInjectionScore = value;
            }
        }

        public int? HttpViolationScore
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.HttpViolationScore;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.HttpViolationScore = value;
            }
        }

        public int? SessionFixationScore
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.SessionFixationScore;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.SessionFixationScore = value;
            }
        }

        public int? OutboundAnomalyScorePl1
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.OutboundAnomalyScorePl1;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.OutboundAnomalyScorePl1 = value;
            }
        }

        public int? OutboundAnomalyScorePl2
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.OutboundAnomalyScorePl2;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.OutboundAnomalyScorePl2 = value;
            }
        }

        public int? OutboundAnomalyScorePl3
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.OutboundAnomalyScorePl3;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.OutboundAnomalyScorePl3 = value;
            }
        }

        public int? OutboundAnomalyScorePl4
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.OutboundAnomalyScorePl4;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.OutboundAnomalyScorePl4 = value;
            }
        }

        public int? AnomalyScore
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings.AnomalyScore;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings.AnomalyScore = value;
            }
        }

        public string? _932260MatchedVarName
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings._932260MatchedVarName;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings._932260MatchedVarName = value;
            }
        }

        public string? _932200MatchedVarName
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings._932200MatchedVarName;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings._932200MatchedVarName = value;
            }
        }

        public string? _932205MatchedVarName
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings._932205MatchedVarName;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings._932205MatchedVarName = value;
            }
        }

        public string? _932206MatchedVarName
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings._932206MatchedVarName;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings._932206MatchedVarName = value;
            }
        }

        public string? _932240MatchedVarName
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings._932240MatchedVarName;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings._932240MatchedVarName = value;
            }
        }

        public string? _933120Tx0
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings._933120Tx0;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings._933120Tx0 = value;
            }
        }

        public string? _933151Tx0
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings._933151Tx0;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings._933151Tx0 = value;
            }
        }

        public string? _942130MatchedVarName
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings._942130MatchedVarName;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings._942130MatchedVarName = value;
            }
        }

        public string? _942131MatchedVarName
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings._942131MatchedVarName;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings._942131MatchedVarName = value;
            }
        }

        public string? _942521MatchedVarName
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings._942521MatchedVarName;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings._942521MatchedVarName = value;
            }
        }

        public string? _943110MatchedVarName
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings._943110MatchedVarName;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings._943110MatchedVarName = value;
            }
        }

        public string? _943120MatchedVarName
        {
            get
            {
                if (this.transactionSettings != null)
                {
                    return this.transactionSettings._943120MatchedVarName;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (this.transactionSettings == null)
                {
                    this.transactionSettings = new WAFTransactionSettings();

					if (needsAdd)
					{
						wafTransactionSettings.Add(transactionSettings);
					}
				}

				this.transactionSettings._943120MatchedVarName = value;
            }
        }

        public void Save()
        {
            actionQueueService.Run(() =>
            {
                hydraDbContext.SaveChanges();
            });
        }
    }
}
