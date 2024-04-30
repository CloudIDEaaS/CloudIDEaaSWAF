using System.Runtime.Serialization;

namespace Utils
{
    [Serializable]
    public class HttpException : Exception
    {
        private int statusCode;
        private string statusDescription;

        public HttpException()
        {
        }

        public HttpException(string? message) : base(message)
        {
        }

        public HttpException(int statusCode, string statusDescription)
        {
            this.statusCode = statusCode;
            this.statusDescription = statusDescription;
        }

        public HttpException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}