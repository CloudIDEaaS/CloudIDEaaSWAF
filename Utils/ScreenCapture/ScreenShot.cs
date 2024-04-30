using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Utils.Controls.ScreenCapture
{
    public class ScreenShot
    {
        public static bool saveToClipboard = false;

        public static Bitmap CaptureImage(bool showCursor, Size curSize, Point curPos, Point sourcePoint, Point destinationPoint, Rectangle selectionRectangle)
        {
            Bitmap bitmap = new Bitmap(selectionRectangle.Width, selectionRectangle.Height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(sourcePoint, destinationPoint, selectionRectangle.Size);

                if (showCursor)
                {
                    Rectangle cursorBounds = new Rectangle(curPos, curSize);
                    Cursors.Default.Draw(graphics, cursorBounds);
                }
            }

            if (saveToClipboard)
            {
                Clipboard.SetImage(bitmap);
            }

            return bitmap;
        }
    }
}