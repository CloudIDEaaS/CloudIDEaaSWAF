using System.Reflection;
using System.Windows.Forms;
using Utils;
using WebSecurity.StartupTests;

namespace WAFWebSample.StartupTests
{
    public static class StartupTests
    {
        public static void RunStartupTests()
        {
            try
            {
                var assembly = Assembly.GetEntryAssembly();
                var startupTestsAssemblyFile = Path.Combine(Path.GetDirectoryName(assembly.Location), @"StartupTests\Hydra.StartupTests.dll");
                var startupTestsAssembly = Assembly.LoadFrom(startupTestsAssemblyFile);
                var inProcessTestRunner = startupTestsAssembly.GetType("Hydra.StartupTests.InProcessTestRunner");
                KeyValuePair<Enum, object>[] pairs;

                ControlExtensions.ShowConsoleInSecondaryMonitor(FormWindowState.Normal);

                pairs =
                [
                    new KeyValuePair<Enum, object>(EnumSwitches.None, "version")
                ];

                inProcessTestRunner.CallPublicStaticMethod("RunTests", pairs);

                pairs =
                [
                    new KeyValuePair<Enum, object>(EnumSwitches.None, "help")
                ];

                inProcessTestRunner.CallPublicStaticMethod("RunTests", pairs);

                pairs =
                [
                    new KeyValuePair<Enum, object>(EnumSwitches.RUN_STARTUP_TESTS, null!)
                ];

                inProcessTestRunner.CallPublicStaticMethod("RunTests", pairs);
            }
            catch (Exception ex)
            {
                var hwndConsole = ControlExtensions.GetConsoleWindow();

                Console.WriteLine("Loading Test Runner Failed. Exception: {0}", ex.Message);
                ControlExtensions.Flash(hwndConsole, FlashWindowFlags.FLASHW_ALL | FlashWindowFlags.FLASHW_TIMERNOFG, 0, 1000);
                return;
            }
        }
    }
}
