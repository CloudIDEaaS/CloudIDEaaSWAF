using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Utils
{
    public class ImageResult
    {
        public string ThumbnailUrl { get; set; }
        public string EncodingFormat { get; set; }
        public string ThumbnailFileName { get; set; }
        public string FileName { get; set; }
        public string ThumbnailImageBase64 { get; set; }
        public string ImageBase64 { get; set; }
        public ImageFormat ImageFormat { get; set; }
        public string ContentUrl { get; set; }
        public string HostPageDomainFriendlyName { get; set; }
        public string HostPageDisplayUrl { get; set; }
        public Exception DownloadException { get; set; }
        public Color AccentColor { get; set; }
        public float AspectRatio { get; set; }
        public string AspectRatioText { get; set; }
        public bool IsMainImage { get; set; }
        public int FollowedParagraphIndex { get; set; }
        public string InstanceId { get; set; }
        public bool IsSelected { get; set; }
        public bool RemainInList { get; set; }
        public string SearchString { get; set; }

        public ImageResult()
        {
            this.FollowedParagraphIndex = -1;
        }

        public void Save(Bitmap image)
        {
            byte[] bytes;

            using (var stream = new MemoryStream())
            {
                image.Save(stream, this.ImageFormat);

                bytes = stream.ToArray();
                this.ThumbnailImageBase64 = bytes.ToBase64();
            }

            using (var stream = File.OpenWrite(this.FileName))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(bytes);
                writer.Flush();
            }
        }

        public void Pack()
        {
            if (File.Exists(this.ThumbnailFileName) && this.ThumbnailImageBase64.IsNullOrEmpty())
            {
                using (var stream = File.OpenRead(this.ThumbnailFileName))
                using (var reader = new BinaryReader(stream))
                {
                    this.ThumbnailImageBase64 = reader.ReadBytes((int)stream.Length).ToBase64();
                }
            }

            if (File.Exists(this.FileName) && this.ImageBase64.IsNullOrEmpty())
            {
                using (var stream = File.OpenRead(this.FileName))
                using (var reader = new BinaryReader(stream))
                {
                    this.ImageBase64 = reader.ReadBytes((int)stream.Length).ToBase64();
                }
            }
        }

        public void Repack()
        {
            if (File.Exists(this.ThumbnailFileName))
            {
                using (var stream = File.OpenRead(this.ThumbnailFileName))
                using (var reader = new BinaryReader(stream))
                {
                    this.ThumbnailImageBase64 = reader.ReadBytes((int)stream.Length).ToBase64();
                }
            }

            if (File.Exists(this.FileName))
            {
                using (var stream = File.OpenRead(this.FileName))
                using (var reader = new BinaryReader(stream))
                {
                    this.ImageBase64 = reader.ReadBytes((int)stream.Length).ToBase64();
                }
            }
        }
    }
}
