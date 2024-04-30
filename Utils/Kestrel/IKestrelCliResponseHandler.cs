using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace Utils.Kestrel;

public interface IKestrelCliResponseHandler
{
    int AllowedPort { set; }
    bool Handles(string absolutePath, string contentType, ref PathString subPath);
    bool UseDefaultHandler(string absolutePath, string contentType, ref PathString subPath);
    bool PrepareResponse(KestrelStaticFileResponseContext context);
}