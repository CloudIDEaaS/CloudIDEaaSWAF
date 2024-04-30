using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;
using Utils.Controls.ScreenCapture;
using static Utils.ControlExtensions;

namespace Utils
{
    public partial class ctrlImageSearchBar : FlowLayoutPanel, IMessageFilter
    {
        private const string QUERY_PARAMETER = "?q=";  // Required
        private const string MKT_PARAMETER = "&mkt=";  // Strongly suggested
        private const string COUNT_PARAMETER = "&count=";
        private const string OFFSET_PARAMETER = "&offset=";
        private const string ID_PARAMETER = "&id=";
        private const string SAFE_SEARCH_PARAMETER = "&safeSearch=";
        private const string ASPECT_PARAMETER = "&aspect=";
        private const string COLOR_PARAMETER = "&color=";
        private const string FRESHNESS_PARAMETER = "&freshness=";
        private const string HEIGHT_PARAMETER = "&height=";
        private const string WIDTH_PARAMETER = "&width=";
        private const string IMAGE_CONTENT_PARAMETER = "&imageContent=";
        private const string IMAGE_TYPE_PARAMETER = "&imageType=";
        private const string LICENSE_PARAMETER = "&license=";
        private const string MAX_FILE_SIZE_PARAMETER = "&maxFileSize=";
        private const string MIN_FILE_SIZE_PARAMETER = "&minFileSize=";
        private const string MAX_HEIGHT_PARAMETER = "&maxHeight=";
        private const string MIN_HEIGHT_PARAMETER = "&minHeight=";
        private const string MAX_WIDTH_PARAMETER = "&maxWidth=";
        private const string MIN_WIDTH_PARAMETER = "&minWidth=";
        private const string SIZE_PARAMETER = "&size=";
        private static string subscriptionKey = "fedc9dab990f4114a8c85181a8b5f1cd";
        private static string baseUri = "https://api.bing.microsoft.com/v7.0/images/search";
        private string clientIdHeader = null;
        private IManagedLockObject lockObject;
        private long nextOffset;
        private Queue<ImageResult> displayQueue;
        private List<ImageResult> relatedContentList;
        public List<PictureBox> SelectedImages { get; }
        private Dictionary<PictureBox, NativeWindow> nativeWindows;
        private string tempFolder;
        private DirectoryInfo tempDirectory;
        private List<ImageResult> imageResults;
        private ManualResetEvent resetEventCleanTemp;
        public event EventHandlerT<ImageResult> ImageFromLocal;
        public event EventHandlerT<ImageResult> ImageSelected;
        public event EventHandlerT<ImageResult> ImageDeselected;
        public event EventHandlerT<ImageResult> ImageDeleted;
        public event EventHandlerT<ImageResult> ImageChanged;

        public ctrlImageSearchBar()
        {
            lockObject = LockManager.CreateObject();
            this.SelectedImages = new List<PictureBox>();

            Application.AddMessageFilter(this);

            imageResults = new List<ImageResult>();
            displayQueue = new Queue<ImageResult>();
            relatedContentList = new List<ImageResult>();
            nativeWindows = new Dictionary<PictureBox, NativeWindow>();
            this.SelectedImages = new List<PictureBox>();

            tempFolder = Path.Combine(Path.GetTempPath(), "BingWebSearch-Publisher");
            tempDirectory = new DirectoryInfo(tempFolder);
            resetEventCleanTemp = new ManualResetEvent(false);

            if (!tempDirectory.Exists)
            {
                tempDirectory.Create();
            }

            InitializeComponent();

            System.Threading.Tasks.Task.Run(() =>
            {
                CleanTempFolders();
            });
        }

        public List<PictureBox> Images
        {
            get
            {
                return this.Controls.Cast<PictureBox>().Where(p => p.Tag.CastTo<ImageResult>().RemainInList || this.SelectedImages.Any(s => s == p)).ToList();
            }
        }

        public void AddImage(ImageResult imageResult)
        {
            using (lockObject.Lock())
            {
                displayQueue.Enqueue(imageResult);
            }

            timerImageHandler.Start();

            using (lockObject.Lock())
            {
                while (displayQueue.Count > 0)
                {
                    Application.DoEvents();
                }
            }

            timerImageHandler.Stop();
        }

        public void ShowCurrentImages(List<ImageResult> images)
        {
            resetEventCleanTemp.WaitOne();

            foreach (var pictureBox in this.Controls.Cast<PictureBox>())
            {
                var nativeWindow = nativeWindows[pictureBox];

                nativeWindow.Dispose();
            }

            this.Controls.Clear();

            foreach (var imageResult in images)
            {
                if (!imageResult.ThumbnailFileName.IsNullOrEmpty())
                {
                    displayQueue.Enqueue(imageResult);
                }
            }

            timerImageHandler.Start();

            using (lockObject.Lock())
            {
                while (displayQueue.Count > 0)
                {
                    Application.DoEvents();
                }
            }

            timerImageHandler.Stop();
        }

        public void DoImageSearch(string searchString)
        {
            this.Controls.Cast<PictureBox>().Where(p => !p.Tag.CastTo<ImageResult>().RemainInList && !this.SelectedImages.Any(s => s == p)).ForEach(p =>
            {
                var nativeWindow = nativeWindows[p];

                this.Controls.Remove(p);

                nativeWindow.Dispose();
            });

            timerImageHandler.Start();

            RunImageSearchAsync(searchString).Wait();

            using (lockObject.Lock())
            {
                while (displayQueue.Count > 0)
                {
                    this.DoEventsSleep(100);
                }
            }

            timerImageHandler.Stop();
        }

        private void timerImageHandler_Tick(object sender, EventArgs e)
        {
            using (lockObject.Lock())
            {
                while (displayQueue.Count > 0)
                {
                    var imageResult = displayQueue.Dequeue();
                    var isFromLocal = false;
                    Image image;
                    PictureBox pictureBox;

                    if (imageResult.ThumbnailImageBase64 == null)
                    {
                        if (imageResult.FileName.IsNullOrEmpty())
                        {
                            continue;
                        }

                        try
                        {
                            image = imageResult.GetImage();
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    else
                    {
                        isFromLocal = true;

                        try
                        {
                            image = imageResult.GetImage();
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    if (this.FlowDirection == FlowDirection.TopDown)
                    {
                        pictureBox = new PictureBox
                        {
                            Image = image,
                            Tag = imageResult,
                            Size = new Size(this.Width - 4, this.Width - 4),
                            Padding = new Padding(0),
                            Margin = new Padding(2),
                            SizeMode = PictureBoxSizeMode.StretchImage
                        };
                    }
                    else
                    {
                        pictureBox = new PictureBox
                        {
                            Image = image,
                            Tag = imageResult,
                            Size = new Size(this.Height - 4, this.Height - 4),
                            Padding = new Padding(0),
                            Margin = new Padding(2),
                            SizeMode = PictureBoxSizeMode.StretchImage
                        };
                    }

                    this.Controls.Add(pictureBox);

                    if (isFromLocal)
                    {
                        ImageFromLocal.Raise(this, imageResult);

                        if (imageResult.IsSelected)
                        {
                            this.SelectedImages.Add(pictureBox);
                        }
                    }

                    if (this.FlowDirection == FlowDirection.TopDown)
                    {
                        pictureBox.Width = this.Width - 4;
                    }
                    else
                    {
                        pictureBox.Height = this.Height - 4;
                    }

                    var nativeWindow = pictureBox.GetMessages(PictureBoxMsgProc, PictureBoxPostMsgProc);

                    nativeWindows.Add(pictureBox, nativeWindow);
                }
            }
        }

        public bool PreFilterMessage(ref Message m)
        {
            var msg = (WindowsMessage)m.Msg;

            switch (msg)
            {
                case WindowsMessage.KEYDOWN:
                    {
                        var vk = (VirtualKeyShort)m.WParam;

                        if (vk == VirtualKeyShort.DELETE)
                        {
                            var focus = this.GetFocus();

                            if (focus != null && this.SelectedImages.Count > 0 && this.GetAllControls().Any(c => c.Handle == focus.Handle))
                            {
                                DeleteImages();

                                return true;
                            }
                        }
                    }
                    break;
            }

            return false;
        }

        private void DeleteImages()
        {
            if (MessageBox.Show(this, "Are you sure you want to delete the image(s) selected?", "Delete image(s)?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (var pictureBox in this.SelectedImages.ToList())
                {
                    var imageResult = (ImageResult)pictureBox.Tag;
                    var nativeWindow = nativeWindows[pictureBox];

                    ImageDeleted.Raise(this, imageResult);

                    this.SelectedImages.Remove(pictureBox);
                    this.Controls.Remove(pictureBox);

                    nativeWindow.Dispose();
                }
            }
        }

        private bool PictureBoxMsgProc(Message message)
        {
            var hwnd = message.HWnd;
            var pictureBox = (PictureBox)FromHandle(hwnd);
            var msg = (WindowsMessage)message.Msg;
            var flags = (MouseKeyFlags)message.WParam;

            switch (msg)
            {

                case WindowsMessage.LBUTTONDBLCLK:
                    {
                        var imageResult = (ImageResult)pictureBox.Tag;
                        var imageEditor = new ImageEditor(imageResult.GetImage());

                        if (imageEditor.ShowDialog() == DialogResult.OK)
                        {
                            if (pictureBox.Image != null)
                            {
                                pictureBox.Image.Dispose();
                                pictureBox.Image = null;

                                GC.Collect();
                            }

                            imageResult.Save(imageEditor.Image);

                            pictureBox.Image = imageEditor.Image;

                            ImageChanged.Raise(this, imageResult);

                            pictureBox.Refresh();
                        }
                    }
                    break;

                case WindowsMessage.LBUTTONDOWN:
                    {
                        var refresh = false;

                        SetFocus(pictureBox.Handle);

                        if (this.SelectedImages.Contains(pictureBox))
                        {
                            var imageResult = (ImageResult)pictureBox.Tag;

                            this.SelectedImages.Remove(pictureBox);
                            ImageDeselected.Raise(this, imageResult);

                            refresh = true;
                        }
                        else if (flags.HasFlag(MouseKeyFlags.MK_SHIFT))
                        {
                            if (FlowDirection == FlowDirection.TopDown)
                            {
                                var index = this.Controls.IndexOf(pictureBox);
                                var minTop = this.SelectedImages.Min(i2 => i2.Top);
                                var maxTop = this.SelectedImages.Max(i2 => i2.Top);
                                var hasSelection = false;

                                if (minTop != 0)
                                {
                                    var minSelect = this.SelectedImages.SingleOrDefault(i => i.Top == minTop);

                                    if (minSelect != null)
                                    {
                                        var indexMin = this.Controls.IndexOf(minSelect);

                                        if (indexMin < index)
                                        {
                                            for (var x = indexMin; x <= index; x++)
                                            {
                                                var selectedImage = (PictureBox)this.Controls[x];

                                                if (!this.SelectedImages.Contains(selectedImage))
                                                {
                                                    this.SelectedImages.Add(selectedImage);
                                                    refresh = true;
                                                }
                                            }

                                            hasSelection = true;
                                        }
                                    }
                                }

                                if (!hasSelection && maxTop != 0)
                                {
                                    var maxSelect = this.SelectedImages.SingleOrDefault(i => i.Top == maxTop);

                                    if (maxSelect != null)
                                    {
                                        var indexMax = this.Controls.IndexOf(maxSelect);

                                        if (indexMax > index)
                                        {
                                            for (var x = index; x <= indexMax; x++)
                                            {
                                                var selectedImage = (PictureBox)this.Controls[x];

                                                if (!this.SelectedImages.Contains(selectedImage))
                                                {
                                                    this.SelectedImages.Add(selectedImage);
                                                    refresh = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var index = this.Controls.IndexOf(pictureBox);
                                var minLeft = this.SelectedImages.Min(i2 => i2.Left);
                                var maxLeft = this.SelectedImages.Max(i2 => i2.Left);
                                var hasSelection = false;

                                if (minLeft != 0)
                                {
                                    var minSelect = this.SelectedImages.SingleOrDefault(i => i.Left == minLeft);

                                    if (minSelect != null)
                                    {
                                        var indexMin = this.Controls.IndexOf(minSelect);

                                        if (indexMin < index)
                                        {
                                            for (var x = indexMin; x <= index; x++)
                                            {
                                                var selectedImage = (PictureBox)this.Controls[x];

                                                if (!this.SelectedImages.Contains(selectedImage))
                                                {
                                                    this.SelectedImages.Add(selectedImage);
                                                    refresh = true;
                                                }
                                            }

                                            hasSelection = true;
                                        }
                                    }
                                }

                                if (!hasSelection && maxLeft != 0)
                                {
                                    var maxSelect = this.SelectedImages.SingleOrDefault(i => i.Left == maxLeft);

                                    if (maxSelect != null)
                                    {
                                        var indexMax = this.Controls.IndexOf(maxSelect);

                                        if (indexMax > index)
                                        {
                                            for (var x = index; x <= indexMax; x++)
                                            {
                                                var selectedImage = (PictureBox)this.Controls[x];

                                                if (!this.SelectedImages.Contains(selectedImage))
                                                {
                                                    this.SelectedImages.Add(selectedImage);
                                                    refresh = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (flags.HasFlag(MouseKeyFlags.MK_CONTROL))
                        {
                            this.SelectedImages.Add(pictureBox);
                            refresh = true;
                        }
                        else
                        {
                            var imageResult = (ImageResult)pictureBox.Tag;

                            this.SelectedImages.Clear();

                            ImageFromLocal.Raise(this, imageResult);

                            this.SelectedImages.Add(pictureBox);
                            ImageSelected.Raise(this, imageResult);

                            refresh = true;
                        }

                        if (refresh)
                        {
                            this.Refresh();
                        }
                        break;
                    }
                case WindowsMessage.RBUTTONDOWN:
                    {
                        var refresh = false;
                        var hiLo = message.LParam.ToLowHiWord();
                        var x = hiLo.Low;
                        var y = hiLo.High;
                        ImageResult imageResult;

                        SetFocus(pictureBox.Handle);

                        if (this.SelectedImages.Contains(pictureBox))
                        {
                            imageResult = (ImageResult)pictureBox.Tag;
                            refresh = true;
                        }
                        else
                        {
                            imageResult = (ImageResult)pictureBox.Tag;

                            this.SelectedImages.Clear();

                            this.SelectedImages.Add(pictureBox);
                            ImageSelected.Raise(this, imageResult);

                            refresh = true;
                        }

                        contextMenuStrip.Show(pictureBox, new Point(x, y));

                        if (refresh)
                        {
                            this.Refresh();
                        }
                     
                        break;
                    }
            }

            return true;
        }

        private void mnuSaveImage_Click(object sender, System.EventArgs e)
        {
            var pictureBox = this.SelectedImages.Single();
            var imageResult = (ImageResult)pictureBox.Tag;

            sfdPicture.InitialDirectory = Path.GetDirectoryName(imageResult.FileName);

            if (sfdPicture.ShowDialog() == DialogResult.OK)
            {
                var image = imageResult.GetImage();
                var file = new FileInfo(sfdPicture.FileName);
                var format = IOExtensions.GetImageFormat(file.Extension);

                if (format == ImageFormat.Bmp)
                {
                    format = ImageFormat.Jpeg;
                }

                image.Save(sfdPicture.FileName, format);
            }
        }

        private void mnuOpenImageLocation_Click(object sender, System.EventArgs e)
        {
            var pictureBox = this.SelectedImages.Single();
            var imageResult = (ImageResult)pictureBox.Tag;
            var file = new FileInfo(imageResult.FileName);

            file.OpenFileLocation();
        }

        private void mnuChangeImage_Click(object sender, System.EventArgs e)
        {
            var pictureBox = this.SelectedImages.Single();
            var imageResult = (ImageResult)pictureBox.Tag;

            ofdPicture.Title = "Replace " + Path.GetFileName(imageResult.FileName);

            ofdPicture.InitialDirectory = Path.GetDirectoryName(imageResult.FileName);
            ofdPicture.FileName = "*" + Path.GetExtension(imageResult.FileName);

            if (ofdPicture.ShowDialog() == DialogResult.OK)
            {
                var fileInfo = new FileInfo(ofdPicture.FileName);

                if (fileInfo.Exists)
                {
                    Image image;

                    pictureBox.Image.Dispose();
                    pictureBox.Image = null;

                    GC.Collect();

                    using (var stream = File.OpenRead(fileInfo.FullName))
                    using (var reader = new BinaryReader(stream))
                    {
                        var bytes = reader.ReadBytes((int) stream.Length);

                        imageResult.ImageBase64 = bytes.ToBase64();
                    }

                    imageResult.FileName = fileInfo.FullName;

                    ImageChanged.Raise(this, imageResult);

                    image = imageResult.GetImage();

                    pictureBox.Image = image;
                }
                else
                {
                    DebugUtils.Break();
                }
            }
        }

        private void mnuDeleteImage_Click(object sender, EventArgs e)
        {
            DeleteImages();
        }

        private void PictureBoxPostMsgProc(Message message)
        {
            var hwnd = message.HWnd;
            var msg = (WindowsMessage)message.Msg;

            if (msg == WindowsMessage.PAINT)
            {
                var pictureBox = (PictureBox)FromHandle(hwnd);

                if (this.SelectedImages.Any(i => i == pictureBox))
                {
                    using (var graphics = pictureBox.CreateGraphics())
                    {
                        var pen = new Pen(Color.Red, 3);
                        var rect = pictureBox.ClientRectangle;

                        rect.Inflate(-2, -2);

                        graphics.DrawRectangle(pen, rect);

                        pen.Dispose();
                    }
                }
            }
        }

        private async Task<HttpResponseMessage> MakeRequestAsync(string queryString)
        {
            var client = new HttpClient();

            // Request headers. The subscription key is the only required header but you should
            // include User-Agent (especially for mobile), X-MSEdge-ClientID, X-Search-Location
            // and X-MSEdge-ClientIP (especially for local aware queries).

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            return (await client.GetAsync(baseUri + queryString).ConfigureAwait(false));
        }

        private async System.Threading.Tasks.Task RunImageSearchAsync(string searchString)
        {
            try
            {
                // Remember to encode the q query parameter.

                var queryString = QUERY_PARAMETER + Uri.EscapeDataString(searchString);
                queryString += MKT_PARAMETER + "en-us";
                queryString += COUNT_PARAMETER + "10";

                HttpResponseMessage response = await MakeRequestAsync(queryString).ConfigureAwait(false);

                clientIdHeader = response.Headers.GetValues("X-MSEdge-ClientID").FirstOrDefault();

                // This example uses dictionaries instead of objects to access the response data.

                var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Dictionary<string, object> searchResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);

                if (response.IsSuccessStatusCode)
                {
                    SaveImages(searchResponse, searchString);
                }
                else
                {
                    PrintErrors(response.Headers, searchResponse);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void SaveImages(Dictionary<string, object> response, string searchString)
        {
            nextOffset = (long)response["nextOffset"];

            var images = response["value"] as Newtonsoft.Json.Linq.JToken;

            foreach (Newtonsoft.Json.Linq.JToken image in images)
            {
                var thumbnailUrl = (string)image["thumbnailUrl"];
                var contentUrl = (string)image["contentUrl"];
                var contentUri = new Uri(contentUrl);
                var lastSegment = contentUri.Segments.Last();
                var encodingFormat = (string)image["encodingFormat"];
                string fileName;
                string suggestedFileName = null;
                string suggestedThumbnailFileName = null;
                var imageResult = new ImageResult
                {
                    ThumbnailUrl = thumbnailUrl,
                    EncodingFormat = encodingFormat,
                    ContentUrl = contentUrl,
                    SearchString = searchString,
                    HostPageDomainFriendlyName = (string)image["hostPageDomainFriendlyName"],
                    HostPageDisplayUrl = (string)image["hostPageDisplayUrl"],
                    AccentColor = ColorTranslator.FromHtml("#" + (string)image["accentColor"])
                };

                if (lastSegment.Contains("."))
                {
                    var invalids = System.IO.Path.GetInvalidFileNameChars();

                    lastSegment = string.Join("_", lastSegment.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
                    suggestedFileName = lastSegment.RegexReplace(@"%\w{2}", "-");

                    suggestedThumbnailFileName = Path.GetFileNameWithoutExtension(suggestedFileName) + "-Thumbnail" + Path.GetExtension(suggestedFileName);
                }

                try
                {
                    if (DownloadFile(new Uri(thumbnailUrl), "." + encodingFormat, suggestedThumbnailFileName, out fileName))
                    {
                        imageResult.ThumbnailFileName = fileName;
                    }

                    if (DownloadFile(new Uri(contentUrl), "." + encodingFormat, suggestedFileName, out fileName))
                    {
                        imageResult.FileName = fileName;
                    }
                }
                catch (Exception ex)
                {
                    imageResult.DownloadException = ex;
                }

                using (lockObject.Lock())
                {
                    if (!imageResult.FileName.IsNullOrEmpty())
                    {
                        imageResults.Add(imageResult);
                        displayQueue.Enqueue(imageResult);
                    }
                }

                // If you want to get additional insights about the image, capture the
                // image token that you use when calling Visual Search API.

                var insightsToken = (string)image["imageInsightsToken"];
            }
        }

        private void CleanTempFolders()
        {
            var tempPath = Path.GetTempPath();
            var directory = new DirectoryInfo(tempFolder);

            if (!directory.Exists)
            {
                resetEventCleanTemp.Set();
                return;
            }

            directory.ForceDeleteFiles();

            directory = new DirectoryInfo(tempPath);

            foreach (var file in directory.GetFiles())
            {
                if (file.Name.RegexIsMatch(@"(?im)^TempImage[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]"))
                {
                    try
                    {
                        if (file.IsReadOnly)
                        {
                            file.MakeWritable();
                        }

                        file.Delete();
                    }
                    catch
                    {
                    }
                }
            }

            resetEventCleanTemp.Set();
        }

        public bool DownloadFile(Uri uri, string extension, string suggestedFileName, out string fileName)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            Stream responseStream;

            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(uri);
                request.Method = "GET";
                request.Headers.Clear();

                request.UserAgent = Assembly.GetExecutingAssembly().FullName;

                response = (HttpWebResponse)request.GetResponse();

                responseStream = response.GetResponseStream();

                using (responseStream)
                {
                    FileInfo file;
                    var bytes = responseStream.ReadToEnd();

                    if (suggestedFileName.IsNullOrEmpty())
                    {
                        var tempFilePrefix = GetTempPrefix();

                        fileName = tempFilePrefix + extension;
                    }
                    else
                    {
                        var tempPath = GetTempPath();

                        fileName = Path.Combine(tempPath, suggestedFileName);
                    }

                    file = new FileInfo(fileName);

                    if (!file.Directory.Exists)
                    {
                        file.Directory.Create();
                    }

                    using (var fileStream = File.OpenWrite(fileName))
                    using (var writer = new BinaryWriter(fileStream))
                    {
                        writer.Write(bytes);
                        writer.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                fileName = null;
                return false;
            }

            return true;
        }

        public string GetTempPrefix()
        {
            var tempFilePrefix = Path.Combine(Path.GetTempPath(), "BingWebSearch-Publisher", Guid.NewGuid().ToString());

            return tempFilePrefix;
        }

        public string GetTempPath()
        {
            var tempFilePrefix = Path.Combine(Path.GetTempPath(), "BingWebSearch-Publisher");

            return tempFilePrefix;
        }

        private void PrintErrors(HttpResponseHeaders headers, Dictionary<String, object> response)
        {
            Console.WriteLine("The response contains the following errors:\n");

            object value;

            if (response.TryGetValue("error", out value))  // typically 401, 403
            {
                PrintError(response["error"] as Newtonsoft.Json.Linq.JToken);
            }
            else if (response.TryGetValue("errors", out value))
            {
                // Bing API error

                foreach (Newtonsoft.Json.Linq.JToken error in response["errors"] as Newtonsoft.Json.Linq.JToken)
                {
                    PrintError(error);
                }

                // Included only when HTTP status code is 400; not included with 401 or 403.

                IEnumerable<string> headerValues;
                if (headers.TryGetValues("BingAPIs-TraceId", out headerValues))
                {
                    Console.WriteLine("\nTrace ID: " + headerValues.FirstOrDefault());
                }
            }

        }

        private void PrintError(Newtonsoft.Json.Linq.JToken error)
        {
            string value = null;

            Console.WriteLine("Code: " + error["code"]);
            Console.WriteLine("Message: " + error["message"]);

            if ((value = (string)error["parameter"]) != null)
            {
                Console.WriteLine("Parameter: " + value);
            }

            if ((value = (string)error["value"]) != null)
            {
                Console.WriteLine("Value: " + value);
            }
        }

        public bool CleanTempWaitOne()
        {
            return resetEventCleanTemp.WaitOne();
        }

        public IDisposable ImageSearchLock()
        {
            return lockObject.Lock();
        }
    }
}
