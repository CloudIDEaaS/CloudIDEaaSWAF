using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utils.HttpServer
{
    public class Server : BaseThreadedService, IDisposable
    {
        private string filePath;
        private HttpListener listener;
        private Task listenTask;
        private int requestCount;
        private int pageViews;
        private IManagedLockObject lockObject;
        private CancellationTokenSource cancellationTokenSource;
        private bool runServer;
        public Dictionary<string, string> MimeTypes { get; private set; }
        public event OnPrepareResponseEventHandler OnPrepareResponse;

        public Server(string url, string filePath, int minimumThreads = 1, int maximumThreads = 10, int maximumAsynThreads = 5) 
        {
            this.filePath = filePath;

            lockObject = LockManager.CreateObject();
            cancellationTokenSource = new CancellationTokenSource();
            runServer = true;
            MimeTypes = new Dictionary<string, string>();

            ThreadPool.SetMinThreads(minimumThreads, maximumAsynThreads);
            ThreadPool.SetMaxThreads(maximumThreads, maximumAsynThreads);

            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();

            this.Start();

            Console.WriteLine("Listening for connections on {0}", url);
        }

        public void Dispose()
        {
            using (lockObject.Lock())
            {
                runServer = false;
            }

            cancellationTokenSource.Cancel();
            this.Stop();
        }

        public async override void DoWork(bool stopping)
        {
            var context = await listener.GetContextAsync();

            ThreadPool.QueueUserWorkItem(async (o) => await HandleRequest(context));
        }

        public async Task HandleRequest(HttpListenerContext context)
        {
            bool runServer = true;

            using (lockObject.Lock())
            {
                runServer = this.runServer;
            }

            // While a user hasn't visited the `shutdown` url, keep on handling requests

            while (runServer)
            {
                var request = context.Request;
                var response = context.Response;
                var handled = false;
                string fileName;
                string path;

                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(request.Url.ToString());
                Console.WriteLine(request.HttpMethod);
                Console.WriteLine(request.UserHostName);
                Console.WriteLine(request.UserAgent);
                Console.WriteLine();

                if ((request.HttpMethod == "POST") && (request.Url.AbsolutePath == "/shutdown"))
                {
                    Console.WriteLine("Shutdown requested");
                    runServer = false;
                }

                if (request.Url.AbsolutePath != "/favicon.ico")
                {
                    pageViews += 1;
                }

                if (request.Url.AbsolutePath == "/")
                {
                    path = request.Url.AbsolutePath + "index.html";
                }
                else
                {
                    path = request.Url.AbsolutePath;
                }

                fileName = Path.Combine(this.filePath, HttpUtility.UrlDecode(path).RemoveStartIfMatches("/").BackSlashes());

                var fileInfo = new FileInfo(fileName);

                if (!fileInfo.Exists)
                {
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    byte[] data = Encoding.UTF8.GetBytes("File not found");
                    var eventArgs = new OnPrepareResponseEventArgs(request, response, fileInfo, null!);

                    if (OnPrepareResponse != null)
                    {
                        OnPrepareResponse(this, eventArgs);

                        if (eventArgs.Handled)
                        {
                            handled = true;
                        }
                    }

                    if (!handled)
                    {
                        response.OutputStream.WriteAsync(data);
                        handled = true;
                    }
                }

                if (!handled)
                {
                    using (var reader = new StreamReader(fileName))
                    {
                        byte[] data = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                        var extension = fileInfo.Extension;
                        string mimeType;

                        using (lockObject.Lock())
                        {
                            if (MimeTypes.ContainsKey(extension))
                            {
                                mimeType = MimeTypes[extension];
                            }
                            else
                            {
                                mimeType = fileInfo.GetMimeType();

                                MimeTypes.Add(extension, mimeType);
                            }
                        }

                        response.ContentType = mimeType;
                        response.ContentEncoding = Encoding.UTF8;
                        response.ContentLength64 = data.LongLength;

                        var eventArgs = new OnPrepareResponseEventArgs(request, response, fileInfo, null!);

                        if (OnPrepareResponse != null)
                        {
                            OnPrepareResponse(this, eventArgs);

                            if (eventArgs.Handled)
                            {
                                handled = true;
                            }
                        }

                        if (!handled)
                        {
                            await response.OutputStream.WriteAsync(data, 0, data.Length);
                        }
                    }
                }

                response.Close();

                var task = listener.GetContextAsync();

                if (!task.Wait(30_000, cancellationTokenSource.Token))
                {
                    return;
                }

                context = task.Result;
            }
        }
    }
}
