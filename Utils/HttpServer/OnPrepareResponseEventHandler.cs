using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utils.HttpServer
{
    public delegate void OnPrepareResponseEventHandler(object sender, OnPrepareResponseEventArgs e);

    public class OnPrepareResponseEventArgs : EventArgs
    {
        public bool Handled { get; set; } = false;
        public HttpListenerRequest Request { get; }
        public HttpListenerResponse Response { get; }
        public FileInfo File { get; }
        public byte[] FileData { get; }

        public OnPrepareResponseEventArgs(HttpListenerRequest request, HttpListenerResponse response, FileInfo file, byte[] fileData)
        {
            this.Request = request;
            this.Response = response;
            this.File = file;
            this.FileData = fileData;
        }
    }
}
