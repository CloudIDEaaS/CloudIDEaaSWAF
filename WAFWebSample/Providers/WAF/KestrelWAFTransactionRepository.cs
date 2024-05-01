using WAFWebSample.WebApi.Providers.WAF.Models;
using Utils;
using WebSecurity.Enums;
using WebSecurity.Interfaces;
using WebSecurity.Models;
using WAFWebSample.Data;

namespace WAFWebSample.WebApi.Providers.WAF;

public class KestrelWAFTransactionRepository : ICrsRepository<ITransaction, ITransaction>
{
    private readonly ILogger<KestrelWAFTransactionRepository> logger;
    private readonly WAFWebSampleDbContext dbContext;
    private readonly IConfiguration configuration;
    private readonly IHostEnvironment environment;
    private readonly IServiceProvider serviceProvider;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ActionQueueService actionQueueService;
    private CrsTransaction transaction;
    private ICrsRepository<IGlobal, IGlobal> globalRepository;

    public KestrelWAFTransactionRepository(IConfiguration configuration, IHostEnvironment environment, WAFWebSampleDbContext dbContext, IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor, ICrsRepository<IGlobal, IGlobal> globalRepository, ActionQueueService actionQueueService, ILogger<KestrelWAFTransactionRepository> logger)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.configuration = configuration;
        this.environment = environment;
        this.serviceProvider = serviceProvider;
        this.httpContextAccessor = httpContextAccessor;
        this.actionQueueService = actionQueueService;
    }

    public void Add(ITransaction entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(ITransaction entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ITransaction> GetAll()
    {
        throw new NotImplementedException();
    }

    public ITransaction GetById(IdType idType, int id)
    {
        throw new NotImplementedException();
    }

    public ITransaction GetSettings()
    {
        if (transaction == null)
        {
            this.transaction = new CrsTransaction(configuration, environment, dbContext, httpContextAccessor, actionQueueService, serviceProvider);
        }

        return transaction;
    }

    public void Update(ITransaction entity)
    {
        throw new NotImplementedException();
    }
}
