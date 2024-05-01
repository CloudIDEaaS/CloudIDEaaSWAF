using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class FixtureClosure
    {
        public IIntegrationTestFixtureBase Fixture { get; private set; }
        public Action Action { get; set; }

        public FixtureClosure(IIntegrationTestFixtureBase fixture)
        {
            this.Fixture = fixture;
        }
    }
}
