using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using MockExtensions = Utils.Testing.MockExtensions;
using WAFWebSample.Data;

namespace WebSecurity.StartupTests.Helpers;

[Flags]
public enum MockTypes
{
    HydraDBContext = 1 << 2,
}

public static partial class TestHelpers
{
    public static Mocks SetupMocks(IServiceCollection services, MockTypes mockTypes)
    {
        var mocks = new Mocks();

        if (mockTypes.HasFlag(MockTypes.HydraDBContext))
        {
            var hydraDbContextMock = new Mock<WAFWebSampleDbContext>();

            hydraDbContextMock.SetupGet(m => m.WAFGlobalSettings).Returns(() =>
            {
                var globalSettings = new List<WAFGlobalSettings>();
                DbSet<WAFGlobalSettings> dbSet;

                globalSettings.Add(new WAFGlobalSettings
                {
                   WAFGlobalSettingsId = Guid.NewGuid(), 
                   CreationDate = DateTime.Now,
                   UpdateDate = DateTime.Now,
                   EnableDefaultCollections = true,
                   CrsSetupVersion = "004.002.0001-dev"
                });

                dbSet = MockExtensions.ToDbSet(globalSettings);

                return dbSet;
            });

            hydraDbContextMock.SetupGet(m => m.WAFTransactionSettings).Returns(() =>
            {
                var transactionSettings = new List<WAFTransactionSettings>();
                DbSet<WAFTransactionSettings> dbSet;

                dbSet = MockExtensions.ToDbSet(transactionSettings);

                return dbSet;
            });

            mocks.WAFWebSampleDbContext = hydraDbContextMock.Object;
            services.AddTransient(s => hydraDbContextMock.Object);
        }

        //ServicesClient.Contracts.AutomationServerRegistration registration = null;
        //var serverRegistrations = new List<ServicesClient.Contracts.AutomationServerRegistration>();
        //var serverPortRangeStart = int.Parse(appSettings.AutomationServerPortRangeStart.ToString());
        //var serverPortRangeMax = int.Parse(appSettings.AutomationServerPortRangeMax.ToString());
        //var diagnosticsPortRangeStart = int.Parse(appSettings.AutomationDiagnosticsPortRangeStart.ToString());
        //var diagnosticsPortRangeMax = int.Parse(appSettings.AutomationDiagnosticsPortRangeMax.ToString());
        //var automationSessionLastActivityTimeoutMinutes = int.Parse(appSettings.AutomationSessionTimeoutMinutes.ToString());
        //var automationSessionCommandLastActivityTimeoutMinutes = int.Parse(appSettings.AutomationSessionCommandLastActivityTimeoutMinutes.ToString());
        //var automationSessionCommandLastActivityCheckInterval = int.Parse(appSettings.AutomationSessionCommandLastActivityCheckInterval.ToString());
        //var installsRelativeLocation = int.Parse(appSettings.InstallsRelativeLocation.ToString());
        //var automationSessionCheckInterval = int.Parse(appSettings.AutomationSessionCheckInterval.ToString());
        //var serviceStartEvent = new ManualResetEvent(false);
        //var devOpsClient = new Mock<IHydraDevOpsClient>();
        //var httpSession = new Mock<ISession>();
        //var configuration = new Mock<IConfiguration>();
        //var environment = new Mock<IHostEnvironment>();
        //var applicationLifetime = new Mock<IApplicationLifetime>();
        //var elasticSearchEngine = new Mock<IElasticSearchEngine>();
        //var dnsProviderFactory = new TestingDnsProviderFactory();
        //var sessionDictionary = new Dictionary<string, object>();
        //var clientCookie = Guid.NewGuid();
        //var userId = UserConfiguration.TestUserId;
        //var socketFactory = new SocketFactory();
        //var runspaceFactoryMock = new Mock<IRunspaceFactory>();
        //SessionService sessionService = null;
        //SessionClientFactory sessionClientFactory = null;
        //IAutomationSessionClient sessionClient = null;
        //IRunspaceFactory runspaceFactory = null;
        //ClaimsPrincipal mockUser;
        //UserLogin userLogin = null;

        //configuration.SetupGet(m => m[It.Is<string>(s => s == "AutomationSessionLastActivityTimeoutMinutes")]).Returns(automationSessionLastActivityTimeoutMinutes.ToString());
        //configuration.SetupGet(m => m[It.Is<string>(s => s == "AutomationSessionCommandLastActivityTimeoutMinutes")]).Returns(automationSessionCommandLastActivityTimeoutMinutes.ToString());
        //configuration.SetupGet(m => m[It.Is<string>(s => s == "AutomationSessionCommandLastActivityCheckInterval")]).Returns(automationSessionCommandLastActivityCheckInterval.ToString());

        //configuration.SetupGet(m => m[It.Is<string>(s => s == "InstallsRelativeLocation")]).Returns(installsRelativeLocation.ToString());
        //configuration.SetupGet(m => m[It.Is<string>(s => s == "AutomationSessionCheckInterval")]).Returns(automationSessionLastActivityTimeoutMinutes.ToString());
        //configuration.SetupGet(m => m[It.Is<string>(s => s == "AutomationConnectionCheckInterval")]).Returns(automationSessionCheckInterval.ToString());
        //configuration.SetupGet(m => m[It.Is<string>(s => s == "Development:HydraDevOps.Services:DevOpsOrganizationName")]).Returns(string.Empty);
        //configuration.SetupGet(m => m[It.Is<string>(s => s == "Development:HydraDevOps.Services:DevOpsServicePrincipalUserName")]).Returns(string.Empty);
        //configuration.SetupGet(m => m[It.Is<string>(s => s == "Development:HydraDevOps.Services:DevOpsServicePrincipalPassword")]).Returns(string.Empty);
        //configuration.SetupGet(m => m[It.Is<string>(s => s == "Development:HydraDevOps.Services:DevOpsServicePrincipalTenant")]).Returns(string.Empty);
        //configuration.SetupGet(m => m[It.Is<string>(s => s == "Development:HydraDevOps.Services:DevOpsPersonalAccessToken")]).Returns(string.Empty);

        //environment.SetupGet(m => m.EnvironmentName).Returns("Development");

        //mockUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        //{
        //    new Claim(ClaimTypes.Name, "MockUser"),
        //    new Claim("UserId", userId.ToString()),
        //    new Claim(ClaimTypes.Role, "Users"),

        //}, "Mock"));

        //mocks.AuthenticatedUser = mockUser;

        //if (mockTypes.HasFlag(MockTypes.RunspaceFactory))
        //{
        //    var runspaceMock = new Mock<IRunspace>();
        //    var pipelineMock = new Mock<IPipeline>();

        //    runspaceMock.Setup(m => m.CreatePipeline(It.IsAny<string>())).Returns(() =>
        //    {
        //        return pipelineMock.Object;
        //    });

        //    runspaceFactoryMock.Setup(m => m.CreateRunspace(It.IsAny<InitialSessionState>())).Returns(runspaceMock.Object);
        //}
        //else
        //{
        //    runspaceFactoryMock.Setup(m => m.CreateRunspace(It.IsAny<InitialSessionState>())).Returns(new Utils.Wrappers.Implementations.Runspace(System.Management.Automation.Runspaces.RunspaceFactory.CreateRunspace()));
        //}

        //if (mockTypes.HasFlag(MockTypes.DevOpsClient))
        //{
        //    var devOpsClientFactory = new Mock<IHydraDevOpsClientFactory>();

        //    devOpsClientFactory.Setup(m => m.CreateClient()).Returns(() =>
        //    {
        //        ClientExtensions.OnGetHttpClientInfo += (sender, e) =>
        //        {
        //            e.Logger = new ConsoleLogger<HydraDevOpsClientFactory>();
        //            e.Configuration = new ServicesClientConfig { BaseUrl = configuration.Object["HydraDevOpsServicesBaseUrl"] };
        //            e.HttpClient = new HttpClient(new HttpEventMessageHandler());
        //            e.HttpClient.BaseAddress = new Uri("http://not_a_real_address");
        //        };

        //        return devOpsClient.Object;
        //    });

        //    mocks.DevOpsClientFactory = devOpsClientFactory.Object;

        //    devOpsClient.Setup(m => m.RegisterSessionServerAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid?>(), It.IsAny<DateTime?>())).Returns((string hostName, string ipAddress, string searchIndex, Guid serverRegistrationId, DateTime serverStartTime) =>
        //    {
        //        var ipRegistrations = serverRegistrations.Where(r => r.IpAddress == ipAddress && r.ShutdownTime == null && r.PingError == null);
        //        var ipRegistered = ipRegistrations.Any();
        //        var maxServerPortRegistration = serverPortRangeStart;
        //        var maxDiagnosticsPortRegistration = diagnosticsPortRangeStart;

        //        if (ipRegistered)
        //        {
        //            maxServerPortRegistration = NumberExtensions.ScopeRange(ipRegistrations.Max(r => r.ServerPortNumber) + 1, serverPortRangeMax);
        //            maxDiagnosticsPortRegistration = NumberExtensions.ScopeRange(ipRegistrations.Max(r => r.DiagnosticsPortNumber) + 1, diagnosticsPortRangeMax);
        //        }

        //        registration = new ServicesClient.Contracts.AutomationServerRegistration
        //        {
        //            AutomationServerRegistrationId = serverRegistrationId,
        //            IpAddress = ipAddress,
        //            RegistrationTime = DateTime.UtcNow,
        //            LastActivityTime = DateTime.UtcNow,
        //            ServerStartTime = serverStartTime,
        //            ServerPortNumber = maxServerPortRegistration,
        //            ServerPortRangeStart = serverPortRangeStart,
        //            DiagnosticsPortNumber = maxDiagnosticsPortRegistration,
        //            DiagnosticsPortRangeStart = diagnosticsPortRangeStart
        //        };

        //        serverRegistrations.Add(registration);

        //        return Task.FromResult<ServicesClient.Contracts.AutomationServerRegistration>(registration);
        //    });
        //}

        //if (mockTypes.HasFlag(MockTypes.DevOpsDBContext))
        //{
        //    var devOpsContext = new Mock<HydraDevOpsContext>();
        //    var registrations = new List<Entities.Models.AutomationServerRegistration>();
        //    var sessions = new List<Entities.Models.AutomationSession>();
        //    var commandExecutions = new List<Entities.Models.AutomationSessionCommandExecution>();
        //    var userLogins = new List<Entities.Models.UserLogin>();
        //    var users = new List<Entities.Models.User>();
        //    var registrationsCount = 0;
        //    var sessionsCount = 0;
        //    var usersCount = 0;
        //    var userLogingsCount = 0;

        //    devOpsContext.Setup(m => m.SaveChanges()).Returns(() =>
        //    {
        //        var count = 0;
        //        var dbContext = devOpsContext.Object;
        //        var serverRegistrationsDbSet = dbContext.AutomationServerRegistrations;
        //        var serverSessionsDbSet = dbContext.AutomationSessions;

        //        count += Math.Max(registrationsCount, registrations.Count);
        //        count += Math.Max(sessionsCount, sessions.Count);
        //        count += Math.Max(usersCount, users.Count);
        //        count += Math.Max(userLogingsCount, userLogins.Count);

        //        return count;
        //    });

        //    mocks.DevOpsContext = devOpsContext.Object;

        //    devOpsContext.SetupGet(m => m.AutomationServerRegistrations).Returns(() =>
        //    {
        //        var serverRegistrationsDbSetMock = new Mock<DbSet<Entities.Models.AutomationServerRegistration>>();

        //        if (registrations.Count == 0)
        //        {
        //            registrations.AddRange(serverRegistrations.Select(r =>
        //            {
        //                return new Entities.Models.AutomationServerRegistration
        //                {
        //                    AutomationServerRegistrationId = Guid.NewGuid(),
        //                    IPAddress = r.IpAddress,
        //                    RegistrationTime = r.RegistrationTime,
        //                    LastActivityTime = r.LastActivityTime,
        //                    ServerStartTime = r.ServerStartTime,
        //                    ServerPortNumber = r.ServerPortNumber,
        //                    ServerPortRangeStart = r.ServerPortRangeStart,
        //                    DiagnosticsPortNumber = r.DiagnosticsPortNumber,
        //                    DiagnosticsPortRangeStart = r.DiagnosticsPortRangeStart,
        //                    AutomationSessions = new List<AutomationSession>()
        //                };
        //            }));
        //        }

        //        serverRegistrationsDbSetMock.Setup(registrations);

        //        return serverRegistrationsDbSetMock.Object;
        //    });

        //    devOpsContext.SetupGet(m => m.AutomationSessions).Returns(() =>
        //    {
        //        var serverSessionsDbSetMock = new Mock<DbSet<Entities.Models.AutomationSession>>();

        //        serverSessionsDbSetMock.Setup(sessions);

        //        return serverSessionsDbSetMock.Object;
        //    });

        //    devOpsContext.SetupGet(m => m.AutomationSessionCommandExecutions).Returns(() =>
        //    {
        //        var commandExecutionsDbSetMock = new Mock<DbSet<Entities.Models.AutomationSessionCommandExecution>>();

        //        commandExecutionsDbSetMock.Setup(commandExecutions);

        //        return commandExecutionsDbSetMock.Object;
        //    });

        //    devOpsContext.SetupGet(m => m.Users).Returns(() =>
        //    {
        //        var usersDbSetMock = new Mock<DbSet<Entities.Models.User>>();

        //        if (users.Count == 0)
        //        {
        //            users.Add(new Entities.Models.User
        //            {
        //                UserId = userId,
        //                FirstName = "TestUser",
        //                LastName = "TestUser",
        //                UserName = "TestUser"
        //            });
        //        }

        //        usersDbSetMock.Setup(users);

        //        return usersDbSetMock.Object;
        //    });

        //    devOpsContext.SetupGet(m => m.UserLogins).Returns(() =>
        //    {
        //        var userLoginsDbSetMock = new Mock<DbSet<Entities.Models.UserLogin>>();
        //        var user = users.Single();

        //        if (userLogins.Count == 0)
        //        {
        //            userLogins.Add(new Entities.Models.UserLogin
        //            {
        //                UserLoginId = Guid.NewGuid(),
        //                AttemptUserName = user.UserName,
        //                Attempt = DateTime.Now,
        //                User = user,
        //                IsLoggedIn = true,
        //            });
        //        }

        //        userLogin = userLogins.Single();

        //        userLoginsDbSetMock.Setup(userLogins);

        //        return userLoginsDbSetMock.Object;
        //    });
        //}

        //if (mockTypes.HasFlag(MockTypes.SessionService))
        //{
        //    var sessionServiceLogger = new ConsoleLogger<SessionService>();
        //    var diagnosticsLogger = new ConsoleLogger<SessionDiagnosticsService>();
        //    int wait;

        //    sessionService = SessionService.StartService(null, mocks.DevOpsClientFactory, environment.Object, dnsProviderFactory, elasticSearchEngine.Object, socketFactory, runspaceFactory, serviceProvider, configuration.Object, sessionServiceLogger, diagnosticsLogger);

        //    sessionService.Started += (sender, e) =>
        //    {
        //        serviceStartEvent.Set();
        //    };

        //    if (Debugger.IsAttached)
        //    {
        //        wait = System.Threading.Timeout.Infinite;
        //    }
        //    else
        //    {
        //        wait = 10000;
        //    }

        //    if (!serviceStartEvent.WaitOne(wait))
        //    {
        //        throw new XunitException("Timeout waiting for service to start after 10 seconds.");
        //    }

        //    Thread.Sleep(100);
        //}

        //if (mockTypes.HasFlag(MockTypes.DevOpsControllerWithSessions))
        //{
        //    HydraDevOpsController devOpsController;
        //    var sessionClientFactoryLogger = new ConsoleLogger<SessionClientFactory>();
        //    var devOpsControllerLogger = new ConsoleLogger<HydraDevOpsController>();
        //    var httpContext = new DefaultHttpContext();
        //    var controllerContext = new ControllerContext();
        //    var devOpsClientFactory = new Mock<IHydraDevOpsClientFactory>();

        //    devOpsClientFactory.Setup(m => m.CreateClient()).Returns(() =>
        //    {
        //        ClientExtensions.OnGetHttpClientInfo += (sender, e) =>
        //        {
        //            e.Logger = new ConsoleLogger<HydraDevOpsClientFactory>();
        //            e.Configuration = new ServicesClientConfig { BaseUrl = configuration.Object["HydraDevOpsServicesBaseUrl"] };
        //            e.HttpClient = new HttpClient(new HttpEventMessageHandler());
        //            e.HttpClient.BaseAddress = new Uri("http://not_a_real_address");
        //        };

        //        return devOpsClient.Object;
        //    });

        //    mocks.DevOpsClientFactory = devOpsClientFactory.Object;

        //    sessionClientFactory = new SessionClientFactory(environment.Object, sessionClientFactoryLogger, configuration.Object, elasticSearchEngine.Object, socketFactory, devOpsClientFactory.Object, serviceProvider, applicationLifetime.Object);
        //    devOpsController = new HydraDevOpsController(null, null, null, null, null, sessionClientFactory, null, devOpsControllerLogger, null, mocks.DevOpsContext, null);

        //    httpContext.User = mockUser;

        //    devOpsController.ControllerContext = controllerContext;
        //    devOpsController.ControllerContext.HttpContext = httpContext;

        //    httpContext.Response.Body = new MemoryStream();

        //    mocks.DevOpsController = devOpsController;
        //}
        //else if (mockTypes.HasFlag(MockTypes.CreateSession))
        //{
        //    var sessionClientFactoryLogger = new ConsoleLogger<SessionClientFactory>();
        //    var devOpsClientFactory = new Mock<IHydraDevOpsClientFactory>();

        //    devOpsClientFactory.Setup(m => m.CreateClient()).Returns(() =>
        //    {
        //        ClientExtensions.OnGetHttpClientInfo += (sender, e) =>
        //        {
        //            e.Logger = new ConsoleLogger<HydraDevOpsClientFactory>();
        //            e.Configuration = new ServicesClientConfig { BaseUrl = configuration.Object["HydraDevOpsServicesBaseUrl"] };
        //            e.HttpClient = new HttpClient(new HttpEventMessageHandler());
        //            e.HttpClient.BaseAddress = new Uri("http://not_a_real_address");
        //        };

        //        return devOpsClient.Object;
        //    });

        //    mocks.DevOpsClientFactory = devOpsClientFactory.Object;


        //    sessionClientFactory = new SessionClientFactory(environment.Object, sessionClientFactoryLogger, configuration.Object, elasticSearchEngine.Object, socketFactory, devOpsClientFactory.Object, serviceProvider, applicationLifetime.Object);
        //    //////sessionClient = sessionClientFactory.CreateSession(mockUser, userLogin, clientCookie);

        //    //////sessionClientFactory.SetClientCookieAndEndPoint(sessionClient, clientCookie, null);

        //    if (registration == null)
        //    {
        //        throw new XunitException("Service started but registration took too long.");
        //    }
        //}

        //mocks.DevOpsClient = devOpsClient.Object;
        //mocks.SessionService = sessionService;
        //mocks.SessionClientFactory = sessionClientFactory;
        //mocks.SessionClient = sessionClient;
        //mocks.ServerRegistration = registration;
        //mocks.ClientCookie = clientCookie;

        return (Mocks) mocks;
    }
}
