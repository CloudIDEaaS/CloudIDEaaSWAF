using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utils
{
    public class UriData
    {
        public Uri Uri { get; }
        public string MediaType { get; }
        public bool Base64 { get; }
        public string DataString { get; }
        public byte[] Data { get; }
        public string Extension { get; }

        public UriData(Uri uri, string mediaType, bool base64, string data, string extension)
        {
            Uri = uri;
            MediaType = mediaType;
            Base64 = base64;
            DataString = data;
            Extension = extension;

            if (base64)
            {
                this.Data = data.FromBase64();
            }
            else
            {
                this.Data = data.ToArray();
            }
        }
    }

    public static class UriExtensions
    {
        public static bool IsData(this Uri uri)
        {
            return uri.ToString().StartsWith("data:");
        }

        public static bool DomainMatches(this Uri uri, string url)
        {
            var uriMatch = new Uri(url);
            var domain = uri.GetDomain();
            var domainMatch = uriMatch.GetDomain();

            return domain.AsCaseless() == domainMatch;
        }

        public static string GetDomain(this Uri uri)
        {
            var host = uri.Host;
            var parts = host.Split(".").ToList();

            if (parts.Count > 2)
            {
                parts.RemoveAt(0);
            }

            return string.Join(".", parts);
        }

        public static string GetBaseUrl(this Uri uri)
        {
            var host = uri.Host;
            var scheme = uri.Scheme;
            var port = uri.Port;

            if (port == 80)
            {
                return string.Format("{0}://{1}", scheme, host);
            }
            else
            {
                return string.Format("{0}://{1}:{2}", scheme, host, port);
            }
        }

        public static UriData GetData(this Uri uri)
        {
            var regex = new Regex("data:(?<mediaType>.*?)(?<base64>;base64)?,(?<data>.*)");
            var match = regex.Match(uri.ToString());
            var mediaType = match.GetGroupValue("mediaType");
            var base64 = match.GetGroupValue("base64").Length > 0;
            var data = match.GetGroupValue("data");
            var extension = IOExtensions.GetDefaultExtension(mediaType);
            var uriData = new UriData(uri, mediaType, base64, data, extension);

            return uriData;
        }

        public static Uri ChangeScheme(this Uri uri, string scheme)
        {
            var uriBuilder = new UriBuilder(uri.ToString())
            {
                Scheme = scheme,
            };

            return uriBuilder.Uri;
        }

        public static Uri ToHttps(this Uri uri)
        {
            var uriBuilder = new UriBuilder(uri.ToString())
            {
                Scheme = Uri.UriSchemeHttps,
                Port = -1
            };

            return uriBuilder.Uri;
        }

        public static Uri ToHttp(this Uri uri)
        {
            var uriBuilder = new UriBuilder(uri.ToString())
            {
                Scheme = Uri.UriSchemeHttp,
                Port = -1
            };

            return uriBuilder.Uri;
        }

        public static Uri MakeAbsolute(this Uri uri, Uri baseUri)
        {
            if (uri.IsAbsoluteUri)
            {
                return uri;
            }
            else
            {
                return new Uri(baseUri, uri);
            }
        }
    }
}
