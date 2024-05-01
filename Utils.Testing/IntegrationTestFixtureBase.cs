using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace Utils
{
    public abstract class IntegrationTestFixtureBase<TStartup> : IDisposable, IIntegrationTestFixtureBase
    {
        public TestServer Server { get; }
        public HttpClient Client { get; }
        public ServiceProvider ServiceProvider { get; private set; }
        public string BasePath { get; }

        protected abstract void ConfigureClient(HttpClient client);
        protected abstract IIntegrationTestFixtureCoordinator GetCoordinatorInstance(IServiceCollection services);
        protected abstract IHost GetHost();
        public abstract ManualResetEvent WaitForStart();
        private CancellationTokenSource traceCancellationTokenSource;
        private IManagedLockObject lockObject;

        public static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
        {
            var projectName = startupAssembly.GetName().Name;
            var applicationBasePath = AppContext.BaseDirectory;
            var directoryInfo = new DirectoryInfo(applicationBasePath);

            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));

                if (projectDirectoryInfo.Exists)
                {
                    if (new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj")).Exists)
                    {
                        return Path.Combine(projectDirectoryInfo.FullName, projectName);
                    }
                }
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }

        public bool CanHandle(HttpRequestMessage request)
        {
            var baseAddress = this.Client.BaseAddress.AbsoluteUri;
            var requestAddress = request.RequestUri.AbsoluteUri;

            return requestAddress.StartsWith(baseAddress);
        }

        public Task<HttpResponseMessage> HandleSendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var baseAddress = this.Client.BaseAddress.AbsoluteUri;
            var requestAddress = request.RequestUri.AbsoluteUri;
            var path = requestAddress.RightMinusLengthOf(baseAddress);

            Debug.Assert(requestAddress.StartsWith(baseAddress));

            using (lockObject.Lock())
            {
                this.Client.DefaultRequestHeaders.Clear();

                if (request.Method.Method == "POST")
                {
                    var content = request.Content;

                    foreach (var header in request.Headers)
                    {
                        this.Client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }

                    return this.Client.PostAsync(path, request.Content);
                }
                else
                {
                    var sendRequest = new HttpRequestMessage(HttpMethod.Get, path);

                    foreach (var header in request.Headers)
                    {
                        sendRequest.Headers.Add(header.Key, header.Value);
                    }

                    return this.Client.SendAsync(sendRequest);
                }
            }
        }

        protected IntegrationTestFixtureBase(string relativeTargetProjectParentDir)
        {
            var host = this.GetHost();

            // Create instance of test server
            this.Server = host.GetTestServer();
            this.Client = this.Server.CreateClient();

            this.ConfigureClient(this.Client);
        }

        public IntegrationTestFixtureBase() : this(Path.Combine(""))
        {
        }

        public virtual void Dispose()
        {
            Client.Dispose();
            Server.Dispose();

            traceCancellationTokenSource.Cancel();
        }

        protected virtual void InitializeServices(IServiceCollection services)
        {
            var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;

            var manager = new ApplicationPartManager
            {
                ApplicationParts =
                {
                    new AssemblyPart(startupAssembly)
                },
                FeatureProviders =
                {
                    new ControllerFeatureProvider(),
                    new ViewComponentFeatureProvider()
                }
            };

            services.AddSingleton(manager);
            services.AddSingleton<HttpMessageHandler, HttpTestServerMessageHandler>();

            if (services.Cast<ServiceDescriptor>().Any(d => d.ServiceType == typeof(IIntegrationTestFixtureCoordinator)))
            {
                var descriptors = services.Cast<ServiceDescriptor>().Where(d => d.ServiceType == typeof(IIntegrationTestFixtureCoordinator));

                DebugUtils.Break();
            }
            else
            {
                var coordinatorInstance = this.GetCoordinatorInstance(services);

                services.AddSingleton(s => coordinatorInstance);
            }

            this.ServiceProvider = services.BuildServiceProvider();
        }
    }
}
