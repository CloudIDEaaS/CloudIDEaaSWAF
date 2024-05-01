using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public interface IIntegrationTestFixtureBase
    {
        HttpClient Client { get; }
        TestServer Server { get; }
        bool CanHandle(HttpRequestMessage request);
        Task<HttpResponseMessage> HandleSendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }
}