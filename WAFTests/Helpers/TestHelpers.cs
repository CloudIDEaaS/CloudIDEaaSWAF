
using System.Management.Automation.Runspaces;

namespace WebSecurity.StartupTests.Helpers;

public static partial class TestHelpers
{
    public static void RunPreFixtureTestSetup()
    {
        //TestingDnsProvider.OnGetHostName += (sender, e) =>
        //{
        //    if (e.RequestorType == typeof(SessionService))
        //    {
        //        e.HostName = TEST_AUTOMATION_SERVER_HOSTNAME;
        //    }
        //    else
        //    {
        //        DebugUtils.Break();
        //    }
        //};

        //TestingDnsProvider.OnGetHostEntry += (sender, e) =>
        //{
        //    if (e.RequestorType == typeof(SessionService))
        //    {
        //        e.IPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
        //    }
        //    else
        //    {
        //        DebugUtils.Break();
        //    }
        //};
    }

    //public static InitialSessionState GetInitialSessionState()
    //{
    //    var initialSessionState = InitialSessionState.CreateDefault();

    //    return initialSessionState;
    //}

    //public static HydraDevOpsContext GetSqlLiteHydraDevOpsContext(IHostEnvironment environment, IConfiguration configuration)
    //{
    //    var connection = new SqliteConnection("DataSource=:memory:");
    //    var options = new DbContextOptionsBuilder<HydraDevOpsContext>().UseSqlite(connection).Options;
    //    HydraDevOpsContext devOpsContext;

    //    connection.Open();
    //    devOpsContext = new HydraDevOpsContext(options, environment, configuration);

    //    devOpsContext.Database.Migrate();

    //    return devOpsContext;
    //}
}
