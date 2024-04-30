using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BitmapedTextures;

namespace EdgeDetection
{
    public delegate double ImageScanDelegate(ScanStrategy s, int a, int b, int c, int d);
    public delegate void RefreshPictureBoxDelegate(PictureBox p, Graphics g, Label l, double d);

    public partial class frmTest : Form
    {
        BitmapAsTexture bmptex = null;
        Bitmap bitmap = null;
        Bitmap edgeboundaryX = null;
        Bitmap edgeboundaryY = null;
        Bitmap edgeboundaryXY = null;
        ImageScanDelegate del = null;

        public frmTest()
        {
            InitializeComponent();

            GetSupportedStrategies();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ReSetImage(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bmptex == null)
            {
                return;
            }

            Graphics g2 = Graphics.FromImage(edgeboundaryX);
            Graphics g3 = Graphics.FromImage(edgeboundaryY);
            Graphics g4 = Graphics.FromImage(edgeboundaryXY);

            int edge = trackBar1.Value;
            int tol = trackBar2.Value;
            int width = bitmap.Width;
            int height = bitmap.Height;

            Color edgeColor = Color.White;
            ColorSubtracter sub = comboBox2.SelectedItem as ColorSubtracter;

            del = new ImageScanDelegate(ImageScan);

            del.BeginInvoke(new HScan(bmptex, g2, edgeColor, sub), width, height, edge, tol, new AsyncCallback(Refresh), new object[] { p2, g2, l2 });
            //del.BeginInvoke(new VScan(bmptex, g3, edgeColor, sub), width, height, edge, tol, new AsyncCallback(Refresh), new object[] { p3, g3, l3 });
            // del.BeginInvoke(new InclinedScan(60, bmptex, g4, edgeColor, sub), width, height, edge, tol, new AsyncCallback(Refresh), new object[] { p4, g4, l4 });
         }

        private double ImageScan(ScanStrategy strategy, int width, int height, int maxedges, int tolerance)
        {
            return strategy.Scan(width, height, maxedges, tolerance);
        }

        private void Refresh(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                object[] obj = result.AsyncState as object[];
                double d = del.EndInvoke(result);

                PictureBox p = obj[0] as PictureBox;
                Graphics g = obj[1] as Graphics;
                Label l = obj[2] as Label;

                p.Invoke(new RefreshPictureBoxDelegate(RefreshPictureBox), p, g, l, d);
            }
        }

        private void RefreshPictureBox(PictureBox p, Graphics g, Label l, double d)
        {
            l.Text = string.Format("Scan{0} = {1}", p.Name[1], d.ToString());
            g.Dispose();
            p.Refresh();
        }

        private void GetSupportedStrategies()
        {
            comboBox2.Items.AddRange(new object[] { new EuclidSubtractor(), new HueSubtractor(), new LuminoSubtractor(), new RedSubtractor(), new GreenSubtractor(),
                                                    new BlueSubtractor(), new BrightnessSubtractor(), new AvgSubtractor() });

            comboBox2.SelectedIndex = 0;
        }

        private void ReSetImage(string path)
        {
            if (bitmap != null)
            {
                bmptex.Dispose();
                bitmap.Dispose();
                bitmap = null;
            }

            if (edgeboundaryX != null)
            {
                edgeboundaryX.Dispose();
                edgeboundaryX = null;
            }

            if (edgeboundaryY != null)
            {
                edgeboundaryY.Dispose();
                edgeboundaryY = null;
            }

            bitmap = new Bitmap(path);
            pictureBox1.Image = bitmap;

            edgeboundaryX = new Bitmap(bitmap.Width, bitmap.Height);
            p2.Image = edgeboundaryX;

            edgeboundaryY = new Bitmap(bitmap.Width, bitmap.Height);
            p3.Image = edgeboundaryY;

            edgeboundaryXY = new Bitmap(bitmap.Width, bitmap.Height);
            p4.Image = edgeboundaryXY;

            bmptex = new BitmapAsTexture(bitmap);

            l2.Text = string.Empty;
            l3.Text = string.Empty;
            l4.Text = string.Empty;
        }
    }
}
