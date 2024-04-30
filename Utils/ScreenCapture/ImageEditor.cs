using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Utils.Controls.ScreenCapture
{
    public partial class ImageEditor : Form
    {
        public ImageEditor(Image image)
        {
            OriginalImage = (Bitmap) image;
            CroppedImage = OriginalImage.Clone() as Bitmap;

            InitializeComponent();

            MakeScaledImage();
        }

        // Exit the program.
        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // The original image.
        private Bitmap OriginalImage;

        // The currently cropped image.
        private Bitmap CroppedImage;

        // The currently scaled cropped image.
        private Bitmap ScaledImage;

        // The cropped image with the selection rectangle.
        private Bitmap DisplayImage;
        private Graphics DisplayGraphics;

        // The current scale.
        private float ImageScale = 1f;

        // Let the user select an area.
        private bool Drawing = false;
        private Point StartPoint, EndPoint;

        public Bitmap Image { get; private set; }

        // Open a file.
        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            if (ofdPicture.ShowDialog() == DialogResult.OK)
            {
                OriginalImage = LoadBitmapUnlocked(ofdPicture.FileName);
                CroppedImage = OriginalImage.Clone() as Bitmap;

                MakeScaledImage();
            }
        }

        // Make the scaled cropped image.
        private void MakeScaledImage()
        {
            int wid = (int)(ImageScale * (CroppedImage.Width));
            int hgt = (int)(ImageScale * (CroppedImage.Height));
            
            ScaledImage = new Bitmap(wid, hgt);

            using (Graphics gr = Graphics.FromImage(ScaledImage))
            {
                Rectangle src_rect = new Rectangle(0, 0,
                    CroppedImage.Width, CroppedImage.Height);
                Rectangle dest_rect = new Rectangle(0, 0, wid, hgt);
                gr.PixelOffsetMode = PixelOffsetMode.Half;
                gr.InterpolationMode = InterpolationMode.NearestNeighbor;
                gr.DrawImage(CroppedImage, dest_rect, src_rect,
                    GraphicsUnit.Pixel);
            }

            DisplayImage = ScaledImage.Clone() as Bitmap;
            if (DisplayGraphics != null) DisplayGraphics.Dispose();
            DisplayGraphics = Graphics.FromImage(DisplayImage);

            picCropped.Image = DisplayImage;
            picCropped.Visible = true;
        }

        private void picCropped_MouseDown(object sender, MouseEventArgs e)
        {
            Drawing = true;

            StartPoint = RoundPoint(e.Location);

            // Draw the area selected.
            DrawSelectionBox(StartPoint);
        }

        private void picCropped_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Drawing) return;

            // Draw the area selected.
            DrawSelectionBox(RoundPoint(e.Location));
        }

        private void picCropped_MouseUp(object sender, MouseEventArgs e)
        {
            if (!Drawing) return;
            Drawing = false;

            // Crop.
            // Get the selected area's dimensions.
            int x = (int)(Math.Min(StartPoint.X, EndPoint.X) / ImageScale);
            int y = (int)(Math.Min(StartPoint.Y, EndPoint.Y) / ImageScale);
            int width = (int)(Math.Abs(StartPoint.X - EndPoint.X) / ImageScale);
            int height = (int)(Math.Abs(StartPoint.Y - EndPoint.Y) / ImageScale);

            if ((width == 0) || (height == 0))
            {
                MessageBox.Show("Width and height must be greater than 0.");
                return;
            }

            Rectangle source_rect = new Rectangle(x, y, width, height);
            Rectangle dest_rect = new Rectangle(0, 0, width, height);

            // Copy that part of the image to a new bitmap.
            Bitmap new_image = new Bitmap(width, height);
            using (Graphics gr = Graphics.FromImage(new_image))
            {
                gr.DrawImage(CroppedImage, dest_rect, source_rect,
                    GraphicsUnit.Pixel);
            }
            CroppedImage = new_image;

            // Display the new scaled image.
            MakeScaledImage();
        }

        // Round the point to the nearest unscaled pixel location.
        private Point RoundPoint(Point point)
        {
            int x = (int)(ImageScale * (int)(point.X / ImageScale));
            int y = (int)(ImageScale * (int)(point.Y / ImageScale));
            return new Point(x, y);
        }

        // Draw the area selected.
        private void DrawSelectionBox(Point end_point)
        {
            // Save the end point.
            EndPoint = end_point;
            if (EndPoint.X < 0) EndPoint.X = 0;
            if (EndPoint.X >= ScaledImage.Width) EndPoint.X = ScaledImage.Width - 1;
            if (EndPoint.Y < 0) EndPoint.Y = 0;
            if (EndPoint.Y >= ScaledImage.Height) EndPoint.Y = ScaledImage.Height - 1;

            // Reset the image.
            DisplayGraphics.DrawImageUnscaled(ScaledImage, 0, 0);

            // Draw the selection area.
            int x = Math.Min(StartPoint.X, EndPoint.X);
            int y = Math.Min(StartPoint.Y, EndPoint.Y);
            int width = Math.Abs(StartPoint.X - EndPoint.X);
            int height = Math.Abs(StartPoint.Y - EndPoint.Y);
            DisplayGraphics.DrawRectangle(Pens.Red, x, y, width, height);
            picCropped.Refresh();
        }

        // Display the original image.
        private void mnuPictureReset_Click(object sender, EventArgs e)
        {
            CroppedImage = OriginalImage.Clone() as Bitmap;
            MakeScaledImage();
        }

        // Save the current file.
        private void mnuFileSave_Click(object sender, EventArgs e)
        {
            if (sfdPicture.ShowDialog() == DialogResult.OK)
            {
                SaveBitmapUsingExtension(CroppedImage, sfdPicture.FileName);
            }
        }

        // Save the file with the appropriate format.
        // Throw a NotSupportedException if the file
        // has an unknown extension.
        public void SaveBitmapUsingExtension(Bitmap bm, string filename)
        {
            string extension = Path.GetExtension(filename);
            switch (extension.ToLower())
            {
                case ".bmp":
                    bm.Save(filename, ImageFormat.Bmp);
                    break;
                case ".exif":
                    bm.Save(filename, ImageFormat.Exif);
                    break;
                case ".gif":
                    bm.Save(filename, ImageFormat.Gif);
                    break;
                case ".jpg":
                case ".jpeg":
                    bm.Save(filename, ImageFormat.Jpeg);
                    break;
                case ".png":
                    bm.Save(filename, ImageFormat.Png);
                    break;
                case ".tif":
                case ".tiff":
                    bm.Save(filename, ImageFormat.Tiff);
                    break;
                default:
                    throw new NotSupportedException(
                        "Unknown file extension " + extension);
            }
        }

        // Load the image into a Bitmap, clone it, and
        // set the PictureBox's Image property to the Bitmap.
        private Bitmap LoadBitmapUnlocked(string file_name)
        {
            using (Bitmap bm = new Bitmap(file_name))
            {
                Bitmap new_bitmap = new Bitmap(bm.Width, bm.Height);
                using (Graphics gr = Graphics.FromImage(new_bitmap))
                {
                    gr.DrawImage(bm, 0, 0);
                }
                return new_bitmap;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Image = this.DisplayImage;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Image = null;
            this.Close();
        }

        private void ImageEditor_Load(object sender, EventArgs e)
        {
            this.Flash();
        }

        // Change the scale.
        private void mnuScale_Click(object sender, EventArgs e)
        {
            // Get the scale percentage.
            ToolStripMenuItem mnu = sender as ToolStripMenuItem;
            int percent = int.Parse(mnu.Text.Replace("&", "").Replace("%", ""));
            ImageScale = percent / 100f;

            // Check the selected menu item.
            mnuScale50.Checked = false;
            mnuScale100.Checked = false;
            mnuScale200.Checked = false;
            mnuScale300.Checked = false;
            mnuScale400.Checked = false;
            mnuScale500.Checked = false;
            mnuScale1000.Checked = false;
            mnu.Checked = true;

            MakeScaledImage();
        }
    }
}
