using Utils;

namespace WebSecurity.StartupTests
{
    public static class Switches
    {
        [CommandLineSwitch("Runs the following unit tests (default): { UnitTests }", true, 2, fallbackDescription: "Runs unit tests (default)")]
        public const string RUN_UNIT_TESTS = "RunUnitTests";
        [CommandLineSwitch("Runs the following integration tests: { IntegrationTests }   The above switch is required to run integration tests.", false, 2, fallbackDescription: "Runs integration tests. This switch is required to run integration tests.")]
        public const string RUN_INTEGRATION_TESTS = "RunIntegrationTests";
        [CommandLineSwitch("Runs the following startup tests: { StartupTests }   The above switch is required to run startup tests.", false, 2, fallbackDescription: "Runs startup tests. This switch is required to run startup tests.")]
        public const string RUN_STARTUP_TESTS = "RunStartupTests";
        [CommandLineSwitch("Specifies database kind, options: SqlLite (default), SqlLiteSingleThread, SqlLiteFile, SqlLiteNewFile, SqlLiteNewSingleThreadFile, EFInMemory, ServiceFile", true, 2)]
        public const string DATABASE_KIND = "DatabaseKind";
        [CommandLineSwitch("Runs a specific test, format [FullNamespace.Type.Method]", true, 2)]
        public const string TEST_TO_RUN = "TestToRun";
        [CommandLineSwitch("Loads Logging Trace Listener UI", true, 2)]
        public const string LOAD_LOGGING_TRACE_LISTENER = "LoadLoggingTraceListener";
        [CommandLineSwitch("Specifies Run from InProcessTestRunner.  Do not pass this switch, its used internally", true, 2)]
        public const string IN_PROCESS = "InProcess";
        [CommandLineSwitch("Runs a specific test, format [FullNamespace.Type.Method]", true, 2)]
        public const string NO_HEADLESS_BROWSER = "NoHeadlessBrowser";
        public static string UnitTests { get; set; }
        public static string IntegrationTests { get; set; }
        public static string StartupTests { get; set; }
    }

    public enum EnumSwitches
    {
        None = 0,
        RUN_UNIT_TESTS,
        RUN_INTEGRATION_TESTS,
        RUN_STARTUP_TESTS,
        DATABASE_KIND,
        TEST_TO_RUN,
        LOAD_LOGGING_TRACE_LISTENER
    }
}
