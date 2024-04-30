using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting.Server;
using System.Net;
using System.Security.Authentication;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using CERTENROLLLib;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.SqlServer.Dac.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;

namespace Utils.Kestrel
{
    public static class KestrelExtensions
    {
        private static IManagedLockObject lockObject;
        private static Queue<KestrelOptionsInfo> optionsQueue;
        private static List<KestrelOptionsInfo> listeners;
        private static Thread dispatchThread;
        private static ManualResetEvent stopListeningResetEvent;
        private static CancellationTokenSource cancellationTokenSource;
        private static IServiceProvider serviceProvider;
        private static IServiceCollection? services;
        private static KestrelOptionsService service;

        public static void UsePorts(this IWebHostBuilder hostBuilder, params int[] ports)
        {
            var assembly = System.Reflection.Assembly.GetEntryAssembly();
            var location = Path.GetDirectoryName(assembly.Location);
            var launchSettingsFile = Path.Combine(location, @"Properties\launchsettings.json");

            if (File.Exists(launchSettingsFile))
            {
                using (var reader = new StreamReader(launchSettingsFile))
                {
                    var settings = reader.ReadJson<LaunchSettings>();
                    var launchSettingUrl = settings.profiles.https.applicationUrl;
                    var uri = new Uri(launchSettingUrl);

                    hostBuilder.UseKestrel((o) => HandleKestrelOptions(o, uri.Port, ports));
                }
            }
            else
            {
                hostBuilder.UseKestrel((o) => HandleKestrelOptions(o, -1, ports));
            }
        }

        private static void HandleKestrelOptions(KestrelServerOptions options, int launchSettingsPort, int[] ports)
        {
            var oneTimeTimer = new OneTimeTimer(TimeSpan.FromSeconds(5));

            serviceProvider = options.ApplicationServices;

            if (services == null)
            {
                services = serviceProvider.GetService<IServiceCollection>();
                service = (KestrelOptionsService)serviceProvider.GetService<IKestrelOptionsService>();
                services.AddSingleton(p => service.AddProvider(p));
            }

            oneTimeTimer.Start(() =>
            {
                foreach (var port in ports)
                {
                    if (launchSettingsPort != -1 && launchSettingsPort == port)
                    {
                        throw new ArgumentException("Cannot pass port that is equal to launchsettings port. Kestrel handles these automatically");
                    }

                    Console.WriteLine($"Instruction to listen on local host, port {port}");

                    if (optionsQueue == null)
                    {
                        lockObject = LockManager.CreateObject();
                        stopListeningResetEvent = new ManualResetEvent(false);
                        cancellationTokenSource = new CancellationTokenSource();

                        optionsQueue = new Queue<KestrelOptionsInfo>();
                        listeners = new List<KestrelOptionsInfo>();

                        dispatchThread = new Thread(DispatchThreadProc);

                        dispatchThread.IsBackground  = true;
                        dispatchThread.Priority = ThreadPriority.Lowest;
                        dispatchThread.Name = "Kestrel dispatch thread";

                        dispatchThread.Start();
                    }

                    using (lockObject.Lock())
                    {
                        optionsQueue.Enqueue(new KestrelOptionsInfo(options, port));
                    }
                }
            });
        }

        private static void DispatchThreadProc()
        {
            KestrelOptionsInfo kestrelOptionsInfo = null;

            using (lockObject.Lock())
            {
                if (optionsQueue.Count > 0)
                {
                    kestrelOptionsInfo = optionsQueue.Dequeue();
                    listeners.Add(kestrelOptionsInfo);
                }
            }

            if (kestrelOptionsInfo != null)
            {
                var options = kestrelOptionsInfo.Options;
                var port = kestrelOptionsInfo.Port;
                var listenThread = new Thread(() => ListenThreadProc(kestrelOptionsInfo));

                listenThread.IsBackground  = true;
                listenThread.Priority = ThreadPriority.Lowest;
                listenThread.Name = $"Kestrel listen thread for port {port}";

                kestrelOptionsInfo.ListenThread = listenThread;

                listenThread.Start();
            }
        }

        private static void ListenThreadProc(KestrelOptionsInfo kestrelOptionsInfo)
        {
            var options = kestrelOptionsInfo.Options;
            var port = kestrelOptionsInfo.Port;
            var thread = kestrelOptionsInfo.ListenThread;
            var server = serviceProvider.GetService<IServer>();
            object addressBindContext;
            Func<ListenOptions, CancellationToken, Task> createBinding;
            IPEndPoint ipEndpoint;
            ListenOptions listenOptions;
            Task bindingTask;
            CsrDetails csrDetails = null;

            addressBindContext = server.GetPrivatePropertyValue<object>("AddressBindContext");

            while (addressBindContext == null)
            {
                Thread.Sleep(1000);

                addressBindContext = server.GetPrivatePropertyValue<object>("AddressBindContext");
            }

            createBinding = addressBindContext.GetPropertyValue<Func<ListenOptions, CancellationToken, Task>>("CreateBinding");
            ipEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listenOptions = TypeExtensions.ActivatePrivate<ListenOptions>(ipEndpoint);

            Console.WriteLine($"Attempting listen on {port}");

            listenOptions.SetPropertyValue(nameof(listenOptions.KestrelServerOptions), kestrelOptionsInfo.Options);

            csrDetails = (CsrDetails)serviceProvider.GetService(typeof(CsrDetails));

            HandleListenOptions(listenOptions, kestrelOptionsInfo, csrDetails, port);

            bindingTask = createBinding(listenOptions, cancellationTokenSource.Token);

            kestrelOptionsInfo.BindingTask = bindingTask;

            while (!stopListeningResetEvent.WaitOne(100))
            {
                Thread.Sleep(100);
            }

            Console.WriteLine($"Stopping listen on {port}");
        }

        public static IApplicationBuilder UseKestrelStaticFiles(this IApplicationBuilder app, KestrelSpaOptions options)
        {
            ArgumentNullException.ThrowIfNull(app);

            return app.UseMiddleware<KestrelStaticFileMiddleware>(options);
        }

        public static void UseKestrelSpa(this IApplicationBuilder app, Action<IKestrelSpaBuilder> configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration, "configuration");
            var options = new KestrelSpaOptions();
            var defaultSpaBuilder = new KestrelDefaultSpaBuilder(app, options);

            configuration(defaultSpaBuilder);

            options.Value = new StaticFileOptions
            {
                RequestPath = defaultSpaBuilder.Options.SourcePath.ForwardSlashes()
            };

            app.UseKestrelStaticFiles(options);
        }

        public static void UseKestrelCliServer(this IKestrelSpaBuilder spaBuilder, int allowedPort)
        {
            ArgumentNullException.ThrowIfNull(spaBuilder, "spaBuilder");

            if (string.IsNullOrEmpty(spaBuilder.Options.SourcePath))
            {
                throw new InvalidOperationException("To use UseKestrelCliServer, you must supply a non-empty value for the SourcePath property of SpaOptions when calling UseSpa.");
            }

            KestrelCliMiddleware.Attach(spaBuilder, allowedPort);
        }

        private static void HandleListenOptions(ListenOptions listenOptions, KestrelOptionsInfo kestrelOptionsInfo, CsrDetails csrDetails, int port)
        {
            var connectionHandler = new KestrelConnectionHandler(kestrelOptionsInfo, csrDetails);

            listenOptions.Protocols = HttpProtocols.Http1;

            if (port == 443 || csrDetails != null)
            {
                if (csrDetails != null)
                {
                    Console.WriteLine($"Found cert details for port {port}");

                    listenOptions.UseHttps(o =>
                    {
                        CX509PrivateKey privateKey;
                        CsrDetailsWithPrivateKey csrDetailsWithPrivateKey;
                        var certificate = csrDetails.FindExistingSelfSignedCertificate();

                        if (certificate == null)
                        {
                            if (csrDetails.IsOfType<CsrDetailsWithPrivateKey>())
                            {
                                csrDetailsWithPrivateKey = (CsrDetailsWithPrivateKey)csrDetails;
                                certificate = csrDetails.CreateSelfSignedCertificate(csrDetailsWithPrivateKey.PrivateKey, out privateKey);
                            }
                            else
                            {
                                certificate = csrDetails.CreateSelfSignedCertificate(out privateKey);
                            }
                        }

                        o.ServerCertificate = certificate;
                        o.SslProtocols = SslProtocols.Tls12;
                    });
                }
                else
                {
                    throw new InvalidCredentialException("Port 443 requires CsrDetails added a service");
                }
            }

            service.Listeners.Add(port, kestrelOptionsInfo);

            listenOptions.Use((d) =>
            {
                var handlerDelegate = (ConnectionDelegate)connectionHandler.OnConnectedAsync;

                connectionHandler.Next = d;

                return handlerDelegate;
            });
        }
    }

    public class EnvironmentVariables
    {
        public string ASPNETCORE_ENVIRONMENT { get; set; }
    }

    public class Http
    {
        public string commandName { get; set; }
        public bool dotnetRunMessages { get; set; }
        public bool launchBrowser { get; set; }
        public string launchUrl { get; set; }
        public string applicationUrl { get; set; }
        public EnvironmentVariables environmentVariables { get; set; }
    }

    public class Https
    {
        public string commandName { get; set; }
        public bool dotnetRunMessages { get; set; }
        public bool launchBrowser { get; set; }
        public string launchUrl { get; set; }
        public string applicationUrl { get; set; }
        public EnvironmentVariables environmentVariables { get; set; }
    }

    public class Profiles
    {
        public Http http { get; set; }
        public Https https { get; set; }
    }

    public class LaunchSettings
    {
        [JsonProperty("$schema")]
        public string schema { get; set; }
        public Profiles profiles { get; set; }
    }
}
