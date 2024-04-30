using Aspose.Words.Saving;
using HtmlAgilityPack;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Win32;
using Moq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Windows.Forms;
using NUglify;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using NUglify.Html;
using HtmlElement = System.Windows.Forms.HtmlElement;
using HtmlNode = HtmlAgilityPack.HtmlNode;
using System.Net.Http;
using System.Xml;
using TidyManaged;
using System.Diagnostics;

namespace Utils
{
    public static class WebExtensions
    {
        static WebExtensions()
        {
            var process = Process.GetCurrentProcess();
            var directory = new DirectoryInfo(Path.GetDirectoryName(process.MainModule.FileName));
            var assembly = Assembly.GetExecutingAssembly();
            var location = Path.GetDirectoryName(assembly.Location);
            var existingFile = new FileInfo(Path.Combine(directory.FullName, "libtidy.dll"));
            FileInfo file;

            if (process.Is64Bit())
            {
                file = directory.FindFile("tidy64.dll");
            }
            else
            {
                file = directory.FindFile("tidy32.dll");
            }

            if (file == null)
            {
                directory = new DirectoryInfo(location);

                existingFile = new FileInfo(Path.Combine(directory.FullName, "libtidy.dll"));

                if (process.Is64Bit())
                {
                    file = directory.FindFile("tidy64.dll");
                }
                else
                {
                    file = directory.FindFile("tidy32.dll");
                }
            }

            if (file == null)
            {
                DebugUtils.Break();
            }

            if (existingFile.Exists)
            {
                try
                {
                    existingFile.Delete();
                }
                catch
                {
                    return;
                }
            }

            file.CopyTo(existingFile.FullName);
        }

        public static bool IsSuccess(this HttpStatusCode statusCode)
        {
            return ((int)statusCode).IsBetween(200, 299);
        }

        public static HtmlDocument DownloadPage(string url, int timeoutMilliseconds = 10_000)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            Stream responseStream;
            var document = new HtmlDocument();
            string source;

            request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = Assembly.GetExecutingAssembly().FullName;
            request.Timeout = timeoutMilliseconds;

            using (response = (HttpWebResponse)request.GetResponse())
            {
                using (responseStream = response.GetResponseStream())
                {
                    source = responseStream.ReadToEnd().ToText();
                }
            }

            document.LoadHtml(source);

            return document;
        }

        public static void RegisterIEEmulation(this Assembly assembly, int preferredVersionValue = 10000)
        {
            var processName = Path.GetFileName(assembly.Location);
            var emulationKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
            var versionValue = 0;

            try
            {
                versionValue = (int)emulationKey.GetValue(processName);
            }
            catch
            {
            }

            if (versionValue != preferredVersionValue)
            {
                versionValue = preferredVersionValue;
                emulationKey.SetValue(processName, versionValue, Microsoft.Win32.RegistryValueKind.DWord);
            }
        }

        public static bool TestConnectivity(this Uri uri, TimeSpan timeout)
        {
            try
            {
                var request = WebRequest.Create(uri.ToString());
                HttpWebResponse response;
                string contentResponse;
                Stream stream;

                request.Method = "HEAD";
                request.Timeout = (int)timeout.TotalMilliseconds;

                response = (HttpWebResponse) request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    request = WebRequest.Create(uri.ToString());
                    request.Timeout = (int)(timeout.TotalMilliseconds + 5000);

                    request.Method = "GET";
                    response = (HttpWebResponse)request.GetResponse();
                    stream = response.GetResponseStream();

                    contentResponse = stream.ReadToEnd().ToText();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool TestConnectivity(this Uri uri, TimeSpan timeout, out string status)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri.ToString());
                HttpWebResponse response;
                string contentResponse;
                Stream stream;

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                request.Method = "HEAD";
                request.Timeout = (int)timeout.TotalMilliseconds;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36";

                using (response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        request = (HttpWebRequest)WebRequest.Create(uri.ToString());
                        request.Timeout = (int)(timeout.TotalMilliseconds + 5000);

                        request.Method = "GET";

                        using (response = (HttpWebResponse)request.GetResponse())
                        {
                            using (stream = response.GetResponseStream())
                            {
                                contentResponse = stream.ReadToEnd().ToText();
                                status = "Success";
                            }
                        }

                        return true;
                    }
                    else
                    {
                        status = response.StatusDescription;
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(uri.ToString());
                    HttpWebResponse response;
                    string contentResponse;
                    Stream stream;

                    request.Method = "GET";
                    request.Timeout = (int)timeout.TotalMilliseconds;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36";

                    using (response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            request = (HttpWebRequest)WebRequest.Create(uri.ToString());
                            request.Timeout = (int)(timeout.TotalMilliseconds + 5000);

                            request.Method = "GET";

                            using (response = (HttpWebResponse)request.GetResponse())
                            {
                                using (stream = response.GetResponseStream())
                                {
                                    contentResponse = stream.ReadToEnd().ToText();
                                    status = "Success";
                                }
                            }

                            return true;
                        }
                        else
                        {
                            status = response.StatusDescription;
                            return false;
                        }
                    }
                }
                catch (Exception ex2)
                {
                    status = ex.Message;
                    return false;
                }

                status = ex.Message;
                return false;
            }
        }

        public static string Unescape(this Uri uri)
        {
            return UnescapeUri(uri.AbsoluteUri);
        }

        public static string EscapeUri(string url)
        {
            return Uri.EscapeDataString(url);
        }

        public static string UnescapeUri(string url)
        {
            return Uri.UnescapeDataString(url);
        }

        public static dynamic JsonDecode(string json)
        {
            return Json.Decode(json);
        }

        public static string JsonEncode(object value)
        {
            return Json.Encode(value);
        }
        
        public static string InsertParagraph(string html, string paragraph, int index = 0)
        {
            var document = new HtmlDocument();
            HtmlNode rootNode;
            HtmlNode newParagraphNode;
            HtmlNode firstParagraphNode;

            document.LoadHtml(html);
            rootNode = document.DocumentNode;

            newParagraphNode = HtmlNode.CreateNode(paragraph);

            firstParagraphNode = rootNode.SelectNodes("//p").ElementAt(index);
            rootNode.InsertBefore(newParagraphNode, firstParagraphNode);

            return rootNode.InnerHtml;
        }

        public static string EmboldenKeywords(string html, List<string> keywords)
        {
            var document = new HtmlDocument();
            var x = 0;
            HtmlNode rootNode;

            foreach (var keyword in keywords)
            {
                html = html.Replace(keyword, keyword.EncloseWithTag("strong"));
            }

            document.LoadHtml(html);
            rootNode = document.DocumentNode;

            return rootNode.InnerHtml;
        }

        public static string StripToInternal(string htmlCandidate)
        {
            var document = new HtmlDocument();
            var x = 0;
            HtmlNode rootNode;
            HtmlNode candidateRootNode;

            document.LoadHtml(htmlCandidate);
            rootNode = document.DocumentNode;

            candidateRootNode = rootNode.SelectSingleNode("html");

            if (candidateRootNode != null)
            {
                rootNode = candidateRootNode;
            }

            candidateRootNode = rootNode.SelectSingleNode("body");

            if (candidateRootNode != null)
            {
                rootNode = candidateRootNode;
            }

            candidateRootNode = rootNode.SelectSingleNode("article");

            if (candidateRootNode != null)
            {
                rootNode = candidateRootNode;
            }

            return rootNode.InnerHtml;
        }

        public static IEnumerable<HtmlElement> GetAncestorsAndSelf(this HtmlElement htmlElement)
        {
            var parent = htmlElement.Parent;

            yield return htmlElement;

            while (parent != null)
            {
                yield return parent;
                parent = parent.Parent;
            }
        }

        public static HtmlElement GetPreviousSibling(this HtmlElement htmlElement)
        {
            var parent = htmlElement.Parent;
            var index = 0;

            foreach (var child in parent.Children.Cast<HtmlElement>())
            {
                if (child.DomElement == htmlElement.DomElement)
                {
                    break;
                }

                index++;
            }

            if (index > 0)
            {
                return (HtmlElement) parent.Children[index - 1];
            }

            return null;
        }

        public static HtmlElement GetNextSibling(this HtmlElement htmlElement)
        {
            var parent = htmlElement.Parent;
            var index = 0;

            foreach (var child in parent.Children.Cast<HtmlElement>())
            {
                if (child.DomElement == htmlElement.DomElement)
                {
                    break;
                }

                index++;
            }

            if (index < parent.Children.Count - 1)
            {
                return (HtmlElement)parent.Children[index + 1];
            }

            return null;
        }

        public static IEnumerable<KeyValuePair<string, T>> GetJsonMembers<T>(this object obj)
        {
            var dynamicJsonObject = (DynamicJsonObject)obj;
            
            foreach (var memberName in dynamicJsonObject.GetDynamicMemberNames())
            {
                var value = dynamicJsonObject.GetDynamicMember<T>(memberName);

                yield return new KeyValuePair<string, T>(memberName, value);
            }
        }

        public static T GetDynamicMember<T>(this object obj, string memberName)
        {
            CallSite<Func<CallSite, object, object>> callsite = null;

            try
            {
                var binder = Binder.GetMember(CSharpBinderFlags.None, memberName, obj.GetType(), new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) });

                callsite = CallSite<Func<CallSite, object, object>>.Create(binder);
            }
            catch (Exception ex)
            {
                return default(T);
            }

            return (T) callsite.Target(callsite, obj);
        }

        public static dynamic CallRestServiceGet(string url, out string rawResults, KeyValuePair<string, string>[] requestProperties, params KeyValuePair<string, object>[] parms)
        {
            HttpWebRequest request;
            string fullUrl;

            if (parms.Length > 0)
            {
                var queryParms = string.Join("&", parms.Select(p => string.Format("{0}={1}", p.Key, p.Value)));

                fullUrl = string.Format("{0}?{1}", url, queryParms);
            }
            else
            {
                fullUrl = url;
            }

            request = (HttpWebRequest) HttpWebRequest.Create(fullUrl);
            request.Method = "GET";
            request.Headers = new WebHeaderCollection();
            request.SetHeaders(requestProperties);

            using (var resp = request.GetResponse())
            {
                var results = new StreamReader(resp.GetResponseStream()).ReadToEnd();

                rawResults = results;

                return JsonDecode(results);
            }
        }

        public static dynamic CallRestServiceGet(string url, out string rawResults, params KeyValuePair<string, object>[] parms)
        {
            WebRequest request;
            string fullUrl;

            if (parms.Length > 0)
            {
                var queryParms = string.Join("&", parms.Select(p => string.Format("{0}={1}", p.Key, p.Value)));

                fullUrl = string.Format("{0}?{1}", url, queryParms);
            }
            else
            {
                fullUrl = url;
            }

            request = HttpWebRequest.Create(fullUrl);
            request.Method = "GET";

            using (var resp = request.GetResponse())
            {
                var results = new StreamReader(resp.GetResponseStream()).ReadToEnd();

                rawResults = results;

                return JsonDecode(results);
            }
        }

        public static void SetHeaders(this HttpWebRequest request, KeyValuePair<string, string>[] requestProperties)
        {
            foreach (var requestProperty in requestProperties)
            {
                switch (requestProperty.Key)
                {
                    case "Content-Type":
                        request.ContentType = requestProperty.Value;
                        break;
                    case "Accept":
                        request.Accept = requestProperty.Value;
                        break;
                    default:
                        request.Headers.Add(requestProperty.Key, requestProperty.Value);
                        break;
                }
            }
        }

        public static void SetHeaders(this HttpRequestMessage request, KeyValuePair<string, string>[] requestProperties)
        {
            foreach (var requestProperty in requestProperties)
            {
                switch (requestProperty.Key)
                {
                    default:
                        request.Headers.Add(requestProperty.Key, requestProperty.Value);
                        break;
                }
            }
        }

        public static dynamic CallRestServiceGet(string url, KeyValuePair<string, string>[] requestProperties, params KeyValuePair<string, object>[] parms)
        {
            HttpWebRequest request;
            string fullUrl;

            if (parms.Length > 0)
            {
                var queryParms = string.Join("&", parms.Select(p => string.Format("{0}={1}", p.Key, p.Value)));

                fullUrl = string.Format("{0}?{1}", url, queryParms);
            }
            else
            {
                fullUrl = url;
            }

            request = (HttpWebRequest) HttpWebRequest.Create(fullUrl);
            request.Method = "GET";
            request.Headers = new WebHeaderCollection();
            request.SetHeaders(requestProperties);

            using (var resp = request.GetResponse())
            {
                var results = new StreamReader(resp.GetResponseStream()).ReadToEnd();

                return JsonDecode(results);
            }
        }

        public static T CallRestServiceGet<T>(string url, KeyValuePair<string, string>[] requestProperties, params KeyValuePair<string, object>[] parms)
        {
            HttpWebRequest request;
            string fullUrl;

            if (parms.Length > 0)
            {
                var queryParms = string.Join("&", parms.Select(p => string.Format("{0}={1}", p.Key, p.Value)));

                fullUrl = string.Format("{0}?{1}", url, queryParms);
            }
            else
            {
                fullUrl = url;
            }

            request = (HttpWebRequest)HttpWebRequest.Create(fullUrl);
            request.Method = "GET";
            request.Headers = new WebHeaderCollection();
            request.SetHeaders(requestProperties);

            using (var resp = request.GetResponse())
            {
                var results = new StreamReader(resp.GetResponseStream()).ReadToEnd();

                return JsonExtensions.ReadJson<T>(results);
            }
        }

        public static string CallRestServicePostNew(string url, KeyValuePair<string, string>[] requestProperties, params KeyValuePair<string, string>[] bodyParms)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Content = new FormUrlEncodedContent(bodyParms),
            };

            SetHeaders(request, requestProperties);

            using (var response = client.SendAsync(request).Result)
            {
                response.EnsureSuccessStatusCode();
                var body = response.Content.ReadAsStringAsync().Result;

                return body;
            }
        }

        public static dynamic CallRestServicePost(string url, KeyValuePair<string, string>[] requestProperties, params KeyValuePair<string, object>[] bodyParms)
        {
            HttpWebRequest request;
            Stream requestStream;
            StreamWriter streamWriter;
            string body;

            request = (HttpWebRequest) HttpWebRequest.Create(url);
            request.Method = "POST";
            request.Headers = new WebHeaderCollection();
            request.SetHeaders(requestProperties);

            using (requestStream = request.GetRequestStream())
            {
                streamWriter = new StreamWriter(requestStream);

                body = string.Join("&", bodyParms.Select(p => string.Format("{0}={1}", p.Key, p.Value.ToString().UriEncode())));

                streamWriter.Write(ASCIIEncoding.ASCII.GetBytes(body));
                streamWriter.Flush();
            }

            using (var resp = request.GetResponse())
            {
                var results = new StreamReader(resp.GetResponseStream()).ReadToEnd();

                return JsonDecode(results);
            }
        }

        public static dynamic CallRestServiceGet(string url, params KeyValuePair<string, object>[] parms)
        {
            WebRequest request;
            string fullUrl;

            if (parms.Length > 0)
            {
                var queryParms = string.Join("&", parms.Select(p => string.Format("{0}={1}", p.Key, p.Value)));

                fullUrl = string.Format("{0}?{1}", url, queryParms);
            }
            else
            {
                fullUrl = url;
            }

            request = HttpWebRequest.Create(fullUrl);
            request.Method = "GET";

            using (var resp = request.GetResponse())
            {
                var results = new StreamReader(resp.GetResponseStream()).ReadToEnd();

                return JsonDecode(results);
            }
        }

        public static HtmlNode NextElement(this HtmlNode htmlNode)
        {
            var nextSibling = htmlNode.NextSibling;

            while (nextSibling != null)
            {
                if (nextSibling.NodeType == HtmlNodeType.Element)
                {
                    return nextSibling;
                }

                nextSibling = nextSibling.NextSibling;
            }

            return null;
        }

        public static IEnumerable<HtmlNode> AllSiblings(this HtmlNode htmlNode)
        {
            var sibling = htmlNode.PreviousSibling;

            while (sibling != null)
            {
                yield return sibling;

                sibling = sibling.PreviousSibling;
            }
            
            sibling = htmlNode.NextSibling;

            while (sibling != null)
            {
                yield return sibling;

                sibling = sibling.NextSibling;
            }
        }

        public static IEnumerable<HtmlNode> AllNextSiblings(this HtmlNode htmlNode)
        {
            var sibling = htmlNode.NextSibling;

            while (sibling != null)
            {
                yield return sibling;

                sibling = sibling.NextSibling;
            }
        }

        public static IEnumerable<HtmlNode> SelectElements(this HtmlNode htmlNode, string xpath)
        {
            var nodes = htmlNode.SelectNodes(xpath);

            if (nodes != null)
            {
                return nodes;
            }

            return new List<HtmlNode>();
        }

        public static IEnumerable<HtmlNode> PreviousSiblings(this HtmlNode htmlNode)
        {
            var sibling = htmlNode.PreviousSibling;

            while (sibling != null)
            {
                yield return sibling;

                sibling = sibling.PreviousSibling;
            }
        }

        public static IEnumerable<HtmlNode> FollowingSiblings(this HtmlNode htmlNode)
        {
            var sibling = htmlNode.NextSibling;

            while (sibling != null)
            {
                yield return sibling;

                sibling = sibling.NextSibling;
            }
        }

        public static bool CompareHtmlStructure(string html1, string html2)
        {
            var document1 = Document.FromString(html1);
            var document2 = Document.FromString(html2);

            document1.ShowWarnings = false;
            document1.Quiet = true;
            document1.OutputXhtml = true;
            document1.CleanAndRepair();

            document2.ShowWarnings = false;
            document2.Quiet = true;
            document2.OutputXhtml = true;
            document2.CleanAndRepair();

            html1 = document1.Save();
            html2 = document2.Save();

            return html1 == html2;
        }
        
        public static HtmlDocument Compress(this HtmlDocument document)
        {
            using (var stream = new MemoryStream())
            {
                Aspose.Words.Document asposeDocument;

                document.Save(stream);
                stream.Rewind();

                asposeDocument = new Aspose.Words.Document(stream);
                asposeDocument.Cleanup(new Aspose.Words.CleanupOptions
                {
                    UnusedBuiltinStyles = true,
                });

                asposeDocument.Save(stream, Aspose.Words.SaveFormat.Html);

                stream.Rewind();

                document = new HtmlDocument();
                document.Load(stream);

                return document;
            }
        }

        public static string CompressToString(this HtmlDocument document, Action<string> generalLogger, Action<string> errorLogger)
        {
            using (var stream = new MemoryStream())
            {
                var builder = new StringBuilder();
                var htmlSettings = new HtmlSettings();
                UglifyResult result;
                string html;

                document.Save(stream);
                stream.Rewind();

                html = stream.ToText();

                htmlSettings.DecodeEntityCharacters = false;
                htmlSettings.RemoveOptionalTags = false;

                result = Uglify.Html(html, htmlSettings);

                if (result.HasErrors)
                {
                    generalLogger(string.Format("Html has {0} errors", result.Errors.Count));

                    Thread.Sleep(2000);

                    foreach (var error in result.Errors)
                    {
                        generalLogger(error.ToString());
                        errorLogger(error.ToString());
                    }
                }

                return result.Code;
            }
        }

        public static bool DownloadPage(this Uri uri, out HtmlDocument document)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            Stream responseStream;

            request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = "GET";
            request.Headers.Clear();

            request.UserAgent = Assembly.GetExecutingAssembly().FullName;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                responseStream = response.GetResponseStream();

                using (var reader = new StreamReader(responseStream))
                {
                    var source = reader.ReadToEnd();

                    document = new HtmlDocument();
                    document.LoadHtml(source);
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    throw new HttpException((int)response.StatusCode, response.StatusDescription);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<(bool, Exception, HtmlDocument)> DownloadPageAsync(this Uri uri)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            Stream responseStream;
            HtmlDocument document;

            request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = "GET";
            request.Headers.Clear();

            request.UserAgent = Assembly.GetExecutingAssembly().FullName;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                responseStream = response.GetResponseStream();

                using (var reader = new StreamReader(responseStream))
                {
                    var source = await reader.ReadToEndAsync();

                    document = new HtmlDocument();
                    document.LoadHtml(source);
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return (response.StatusCode == HttpStatusCode.OK, null, document);
                }
                else
                {
                    throw new HttpException((int) response.StatusCode, response.StatusDescription);
                }
            }
            catch (Exception ex)
            {
                document = null;
                return (false, ex, document);
            }
        }
    }
}
