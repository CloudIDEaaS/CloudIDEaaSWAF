using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using static Utils.ControlExtensions;

namespace Utils
{
    public partial class frmEmojisPalette : Form
    {
        private ManualResetEvent emojiResetEvent;
        private List<Emoji> emojis;
        private Dictionary<string, List<Emoji>> emojisByCategory;
        private IManagedLockObject lockObject;
        private PictureBox hoverImage;
        public event EventHandlerT<Emoji> OnSelected;

        public frmEmojisPalette()
        {
            var hydraSolutionPath = Path.GetFullPath(Environment.ExpandEnvironmentVariables("%HYDRASOLUTIONPATH%"));
            var emojisFile = Path.Combine(hydraSolutionPath, @"Utils\Emojis\Emojis.html");
            var document = new HtmlAgilityPack.HtmlDocument();

            emojiResetEvent = new ManualResetEvent(false);
            lockObject = LockManager.CreateObject();

            InitializeComponent();

            document.Load(emojisFile);
            emojis = new List<Emoji>();
            emojisByCategory = new Dictionary<string, List<Emoji>>();

            Task.Run(() =>
            {
                try
                {
                    foreach (var node in document.DocumentNode.SelectElements("//tr[./td]"))
                    {
                        var headerNode = node.PreviousSiblings().First(n => n.Name == "tr" && n.ChildNodes.Any(n2 => n2.Name == "th" && n2.GetAttributes().Any(a => a.Name == "class" && a.Value == "bighead"))).ChildNodes.Single();
                        var category = headerNode.SelectSingleNode("a").InnerText.HtmlDecode();
                        var name = node.SelectSingleNode($"td[@class='name']").InnerText;
                        var chars = node.SelectSingleNode($"td[@class='chars']").InnerText;
                        var firstImgaNode = node.SelectElements($"td/img[@class='imga']").FirstOrDefault();

                        if (firstImgaNode != null)
                        {
                            var src = node.SelectElements($"td/img[@class='imga']").First().GetAttributeValue<string>("src", null);
                            var imageBytes = src.RemoveStart("data:image/png;base64,").FromBase64();
                            var emoji = new Emoji();
                            Image image;

                            emoji.Name = name;
                            emoji.Chars = chars;
                            emoji.Src = src;
                            emoji.Category = category;

                            using (var stream = imageBytes.ToMemory())
                            {
                                image = Bitmap.FromStream(stream);

                                emoji.Image = image;
                            }

                            using (this.lockObject.Lock())
                            {
                                emojis.Add(emoji);
                            }
                        }
                    }

                    var groupings = emojis.GroupBy(e2 => e2.Category);

                    foreach (var grouping in groupings)
                    {
                        emojisByCategory.AddToDictionaryListCreateIfNotExist(grouping.Key, grouping);
                    }
                }
                catch (Exception ex)
                {
                    DebugUtils.Break();
                }

                emojiResetEvent.Set();
            });
        }

        private void frmEmojisPalette_Load(object sender, EventArgs e)
        {
            this.DelayInvoke(100, () =>
            {
                emojiResetEvent.WaitOne();

                foreach (var key in emojisByCategory.Keys)
                {
                    var tabPage = new TabPage(key);
                    var panel = new Panel();
                    var flowPanel = new FlowLayoutPanel();

                    flowPanel.FlowDirection = FlowDirection.LeftToRight;
                    flowPanel.WrapContents = true;
                    flowPanel.Tag = key;
                    flowPanel.Location = new Point(0, 0);
                    flowPanel.Size = tabPage.Size;
                    flowPanel.AutoSize = true;

                    panel.Dock = DockStyle.Fill;

                    panel.Controls.Add(flowPanel);
                    tabPage.Controls.Add(panel);

                    panel.AutoScroll = true;

                    panel.Resize += (s, e2) =>
                    {
                        flowPanel.MaximumSize = new Size(panel.Width - 25, panel.Height + 1000);
                    };

                    tabPage.VisibleChanged += (s, e2) =>
                    {
                        var visibleTabPage = (TabPage)s;
                        var visiblePanel = (Panel)visibleTabPage.Controls[0];
                        var visibleFlowPanel = (FlowLayoutPanel)visiblePanel.Controls[0];

                        if (visibleFlowPanel.Visible && visibleFlowPanel.Controls.Count == 0)
                        {
                            var visibleKey = (string) visibleFlowPanel.Tag;
                            var visibleEmojis = emojisByCategory[key];

                            foreach (var emoji in visibleEmojis)
                            {
                                var pictureBox = new PictureBox();
                                var image = emoji.Image;

                                image = image.ResizeImage(32, 32);

                                pictureBox.Image = emoji.Image;
                                pictureBox.Size = image.Size;
                                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBox.Tag = emoji;

                                pictureBox.MouseEnter += (sender2, e3) =>
                                {
                                    if (hoverImage != null)
                                    {
                                        var oldHoverImage = hoverImage;

                                        hoverImage = null;

                                        oldHoverImage.Refresh();
                                        oldHoverImage.Parent.Refresh();
                                    }

                                    hoverImage = (PictureBox)sender2;
                                    DrawHover(hoverImage);
                               };

                                visibleFlowPanel.Controls.Add(pictureBox);

                                pictureBox.Visible = true;

                                pictureBox.GetMessages(PictureBoxMsgProc, PictureBoxPostMsgProc);
                            }
                        }
                    };

                    tabControl.TabPages.Add(tabPage);
                }
            });
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
                        var emoji = (Emoji)pictureBox.Tag;

                        OnSelected.Raise(this, emoji);

                        this.Hide();
                    }
                    break;

                case WindowsMessage.LBUTTONUP:
                    {
                        var emoji = (Emoji)pictureBox.Tag;

                        OnSelected.Raise(this, emoji);

                        this.Hide();
                    }

                    break;
            }

            return true;
        }

        private void PictureBoxPostMsgProc(Message message)
        {
            var hwnd = message.HWnd;
            var msg = (WindowsMessage)message.Msg;

            if (msg == WindowsMessage.PAINT)
            {
                var pictureBox = (PictureBox)FromHandle(hwnd);

                if (hoverImage == pictureBox)
                {
                    DrawHover(pictureBox);
                }
            }
        }

        private static void DrawHover(PictureBox pictureBox)
        {
            using (var graphics = pictureBox.CreateGraphics())
            {
                var pen = new Pen(Color.Gray, 1);
                var rect = pictureBox.ClientRectangle;

                rect.Inflate(-2, -2);

                graphics.DrawRoundedRectangle(pen, rect, 3);

                pen.Dispose();
            }
        }

        private void frmEmojisPalette_Deactivate(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
