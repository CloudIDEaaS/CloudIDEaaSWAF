using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public interface IIntegrationTestFixtureCoordinator
    {
        List<IIntegrationTestFixtureBase> TestFixtures { get; }
        Task<HttpResponseMessage> RouteRequest(HttpRequestMessage request, CancellationToken cancellationToken);
        void Wait(IIntegrationTestFixtureBase integrationTestFixture, Action action);
        void SetShutdownOrder(params IIntegrationTestFixtureBase[] fixtures);
        ManualResetEvent SignalStart();
    }
}