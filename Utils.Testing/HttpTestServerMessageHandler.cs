using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public class HttpTestServerMessageHandler : HttpMessageHandler
    {
        private IServiceProvider serviceProvider;
        private IIntegrationTestFixtureCoordinator testFixtureCoordinator;

        public HttpTestServerMessageHandler(IServiceProvider serviceProvider, IIntegrationTestFixtureCoordinator testFixtureCoordinator)
        {
            this.serviceProvider = serviceProvider;
            this.testFixtureCoordinator = testFixtureCoordinator;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return this.testFixtureCoordinator.RouteRequest(request, cancellationToken);
        }
    }
}
