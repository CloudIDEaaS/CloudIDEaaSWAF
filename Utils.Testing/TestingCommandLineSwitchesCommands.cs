namespace Utils
{
    public static partial class Switches
    {
        [CommandLineSwitch("Runs the following unit tests (default): { UnitTests }", true, 2)]
        public const string RUN_UNIT_TESTS = "RunUnitTests";
        [CommandLineSwitch("Runs the following integration tests: { IntegrationTests }   This switch is required to run integration tests.", false, 2)]
        public const string RUN_INTEGRATION_TESTS = "RunIntegrationTests";
        [CommandLineSwitch("Specifies database kind, options: SqlLite (default), SqlLiteSingleThread, SqlLiteFile, SqlLiteNewFile, SqlLiteNewSingleThreadFile, EFInMemory, ServiceFile", true, 2)]
        public const string DATABASE_KIND = "DatabaseKind";
        [CommandLineSwitch("Runs a specific test, format [FullNamespace.Type.Method]", true, 2)]
        public const string TEST_TO_RUN = "TestToRun";
        [CommandLineSwitch("Loads Logging Trace Listener UI", true, 2)]
        public const string LOAD_LOGGING_TRACE_LISTENER = "LoadLoggingTraceListener";
        public static string UnitTests { get; set; }
        public static string IntegrationTests { get; set; }
    }
}
