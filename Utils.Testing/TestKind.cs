using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    [Flags]
    public enum TestKind
    {
        None,
        UnitTests = 1 << 1,
        IntegrationTests = 1 << 2,
        EndToEndTests = 1 << 3,
        PerformanceTests = 1 << 4,
        StressTests = 1 << 5,
        SmokeTests = 1 << 6,
        StartupTests = 1 << 7
    }
}
