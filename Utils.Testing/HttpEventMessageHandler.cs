using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    public delegate void SendAsyncEventHandler(object sender, SendAsyncEventArgs e);

    public class SendAsyncEventArgs : EventArgs
    {
        public Task<HttpResponseMessage> ResponseTask { get; set; }
        public HttpRequestMessage Request { get; }
        public CancellationToken CancellationToken { get; }

        public SendAsyncEventArgs(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.Request = request;
            this.CancellationToken = cancellationToken;
        }
    }

    public class HttpEventMessageHandler : HttpMessageHandler
    {
        public static event SendAsyncEventHandler OnSendAsync;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var eventArgs = new SendAsyncEventArgs(request, cancellationToken);

            HttpEventMessageHandler.OnSendAsync(this, eventArgs);

            return eventArgs.ResponseTask;
        }
    }
}
