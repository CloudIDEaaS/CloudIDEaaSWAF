using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Utils
{
    public static class AspExtensions
    {
        public static string ToHttpString(this HttpRequest request)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(request.Method))
            {
                sb.Append(request.Method);
                sb.Append(' ');
            }
            else
            {
                sb.Append("GET");
                sb.Append(' ');
            }

            GetRequestUrl(sb, request, includeQueryString: true);
            
            if (!string.IsNullOrEmpty(request.Protocol))
            {
                sb.Append(' ');
                sb.Append(request.Protocol);
            }
            
            if (!string.IsNullOrEmpty(request.ContentType))
            {
                sb.Append(' ');
                sb.Append(request.ContentType);
            }
            
            return sb.ToString();
        }

        public static string ToHttpString(this HttpResponse response)
        {
            var sb = new StringBuilder();
            var reasonPhrase = EnumUtils.GetValue<HttpStatusCode>(response.StatusCode).ToString();

            sb.Append(response.StatusCode);

            var responseString = response.CallInternalMethod<string>("DebuggerToString");

            var resolvedReasonPhrase = ResolveReasonPhrase(response, reasonPhrase);

            if (!string.IsNullOrEmpty(resolvedReasonPhrase))
            {
                sb.Append(' ');
                sb.Append(resolvedReasonPhrase);
            }
            
            if (!string.IsNullOrEmpty(response.ContentType))
            {
                sb.Append(' ');
                sb.Append(response.ContentType);
            }

            return sb.ToString();
        }

        private static string? ResolveReasonPhrase(HttpResponse response, string? reasonPhrase)
        {
            return response.HttpContext.Features.Get<IHttpResponseFeature>()?.ReasonPhrase ?? reasonPhrase;
        }

        private static void GetRequestUrl(StringBuilder sb, HttpRequest request, bool includeQueryString)
        {
            // The URL might be missing because the context was manually created in a test, e.g. new DefaultHttpContext()
            if (string.IsNullOrEmpty(request.Scheme) &&
                !request.Host.HasValue &&
                !request.PathBase.HasValue &&
                !request.Path.HasValue &&
                !request.QueryString.HasValue)
            {
                sb.Append("(unspecified)");
                return;
            }

            // If some parts of the URL are provided then default the significant parts to avoid a werid output.
            var scheme = request.Scheme;
            if (string.IsNullOrEmpty(scheme))
            {
                scheme = "(unspecified)";
            }
            var host = request.Host.Value;
            if (string.IsNullOrEmpty(host))
            {
                host = "(unspecified)";
            }

            sb.Append(CultureInfo.InvariantCulture, $"{scheme}://{host}{request.PathBase.Value}{request.Path.Value}{(includeQueryString ? request.QueryString.Value : string.Empty)}");
        }

        public static string HashPassword(this string password, ref string saltText)
        {
            var salt = new byte[128 / 8];

            if (saltText != null)
            {
                salt = Convert.FromBase64String(saltText);
            }
            else
            {
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);

                    saltText = Convert.ToBase64String(salt);
                }
            }

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8));
        }
    }
}
