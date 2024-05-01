using WAFWebSample.WebApi.Providers.WAF.Models;
using Utils;
using WebSecurity.Enums;
using WebSecurity.Interfaces;
using WebSecurity.KestrelWAF;
using WebSecurity.Models;

namespace WAFWebSample.WebApi.Providers.WAF;

public class KestrelWAFGlobalRepository : ICrsRepository<IGlobal, IGlobal>
{
    private readonly ILogger<KestrelWAFGlobalRepository> logger;
    private readonly IConfiguration configuration;
    private readonly IHostEnvironment environment;
    private readonly IServiceProvider serviceProvider;
    private readonly ActionQueueService actionQueueService;
    private readonly IWAFDataFileCache wafDataFileCache;
    private CrsGlobal global;

    public KestrelWAFGlobalRepository(IConfiguration configuration, IHostEnvironment environment, IServiceProvider serviceProvider, ActionQueueService actionQueueService, IWAFDataFileCache wafDataFileCache, ILogger<KestrelWAFGlobalRepository> logger)
    {
        this.logger = logger;
        this.configuration = configuration;
        this.environment = environment;
        this.serviceProvider = serviceProvider;
        this.actionQueueService = actionQueueService;
        this.wafDataFileCache = wafDataFileCache;
    }

    public void Add(IGlobal entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(IGlobal entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IGlobal> GetAll()
    {
        throw new NotImplementedException();
    }

    public IGlobal GetById(IdType idType, int id)
    {
        throw new NotImplementedException();
    }

    public IGlobal GetSettings()
    {
        if (this.global == null)
        {
            this.global = new CrsGlobal(configuration, environment, actionQueueService, wafDataFileCache, serviceProvider);
        }

        return this.global;
    }

    public void Update(IGlobal entity)
    {
        throw new NotImplementedException();
    }
}
