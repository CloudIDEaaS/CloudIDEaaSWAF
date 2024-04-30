using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    [DefaultProperty("Url")]
    public partial class ctrlEditHyperlink : UserControl
    {
        private bool editingUrlLink;
        private string defaultLabelUrlText;
        private TextEdit textEditUrl;
        private string url;
        private bool isValidUrl;

        [Browsable(true)]
        public event EventHandler TextChanged;

        public ctrlEditHyperlink()
        {
            InitializeComponent();

            defaultLabelUrlText = linkLabelUrl.Text;
        }

        public string Url 
        {
            get
            {
                if (linkLabelUrl == null || linkLabelUrl.Text == "[None]")
                {
                    return null;
                }

                return linkLabelUrl.Text;
            }

            set
            {
                if (value.IsNullOrEmpty())
                {
                    value = "[None]";
                }

                if (linkLabelUrl != null)
                {
                    linkLabelUrl.Text = value;
                    timerUrlValidation.Start();
                }
            }
        }

        private void linkLabelUrl_MouseHover(object sender, EventArgs e)
        {
            textEditUrl = linkLabelUrl.Parent.GetTextEdit();

            if (Keys.ControlKey.IsPressed())
            {
                if (textEditUrl != null)
                {
                    textEditUrl.Visible = false;
                }
            }
            else
            {
                if (textEditUrl != null)
                {
                    textEditUrl.Visible = true;
                }
                else
                {
                    ShowUrlTextEdit();
                }
            }
        }

        private void linkLabelUrl_MouseClick(object sender, MouseEventArgs e)
        {
            if (Keys.ControlKey.IsPressed())
            {
                if (linkLabelUrl.Text != defaultLabelUrlText)
                {
                    linkLabelUrl.LinkVisited = true;

                    Process.Start(linkLabelUrl.Text);
                }
            }
            else
            {
                ShowUrlTextEdit();
            }
        }

        private void ShowUrlTextEdit()
        {
            var destroying = false;
            var loading = true;
            Action<bool> destroyEdit;
            Rectangle linkRect;
            Rectangle invalid;

            if (this.editingUrlLink)
            {
                return;
            }

            if (linkLabelUrl.Text == defaultLabelUrlText)
            {
                textEditUrl = new TextEdit();
            }
            else
            {
                textEditUrl = new TextEdit(linkLabelUrl.Text);
            }

            linkRect = linkLabelUrl.Bounds;

            textEditUrl.Font = new Font(this.Font, FontStyle.Regular);
            textEditUrl.ForeColor = ColorTranslator.FromHtml("#0003fe");
            textEditUrl.BackColor = SystemColors.Control;

            lblUrlNote.FadeOut(100);
            this.DoEventsSleep(100);

            if (textEditUrl == null)
            {
                DebugUtils.Break();
            }

            textEditUrl.CreateControl();
            textEditUrl.SetAsChildOf(this);

            linkRect.Width = this.Width;
            linkRect.Height = this.Height;

            textEditUrl.SetWindowPos(linkRect, ControlExtensions.SetWindowPosFlags.ShowWindow);
            textEditUrl.BringToFront();
            textEditUrl.Focus();

            textEditUrl.Width = this.Width;
            textEditUrl.Height = this.Height;

            this.editingUrlLink = true;

            invalid = textEditUrl.Bounds;
            invalid.Inflate(6, 6);

            this.Invalidate(invalid);

            loading = false;

            destroyEdit = (restoreOnEmpty) =>
            {
                if (destroying || loading)
                {
                    return;
                }

                destroying = true;

                textEditUrl = linkLabelUrl.Parent.GetTextEdit();

                if (textEditUrl != null)
                {
                    if (textEditUrl.Text.IsNullOrEmpty() && restoreOnEmpty)
                    {
                        linkLabelUrl.Text = defaultLabelUrlText;
                    }
                    else
                    {
                        url = textEditUrl.Text;

                        linkLabelUrl.Text = url;
                    }
                }

                // Guess destroy doesn't always mean destroy

                while (textEditUrl != null)
                {
                    textEditUrl.SetAsChildOf(null);
                    textEditUrl.Destroy();

                    textEditUrl = linkLabelUrl.GetTextEdit();
                }

                timerUrlValidation.Start();

                this.editingUrlLink = false;

                linkLabelUrl.Parent.Invalidate();
                linkLabelUrl.Parent.GetParentForm().Refresh();
            };

            textEditUrl.MouseHover += (sender2, e2) =>
            {
                if (Keys.ControlKey.IsPressed())
                {
                    textEditUrl.Visible = false;
                }
                else
                {
                    textEditUrl.Visible = true;
                }
            };

            textEditUrl.MouseLeave += (sender2, e2) =>
            {
                var hwndFocus = ControlExtensions.GetFocus();

                if (textEditUrl.Handle != hwndFocus /* && objectPropertyGrid.Handle != hwndFocus */)
                {
                    destroyEdit(true);
                }
            };

            textEditUrl.KeyDown += (sender2, e2) =>
            {
                if (e2.KeyCode == Keys.Enter)
                {
                    destroyEdit(false);
                }
                else if (e2.KeyCode == Keys.Escape)
                {
                    destroyEdit(true);
                }
                else if (e2.KeyCode == Keys.Tab)
                {
                    destroyEdit(true);

                    this.DelayInvoke(1, () =>
                    {
                        this.SelectNext();
                    });
                }
                else
                {
                    if (!timerUrlValidation.Enabled)
                    {
                        timerUrlValidation.Start();
                    }

                    TextChanged?.Invoke(this, EventArgs.Empty);
                }
            };
        }

        private void linkLabelUrl_MouseLeave(object sender, EventArgs e)
        {
            if (textEditUrl == null)
            {
                editingUrlLink = false;
            }
        }

        private void linkLabelUrl_Paint(object sender, PaintEventArgs e)
        {
            PaintUrlIndicators(e.Graphics, e.ClipRectangle);
        }

        private void PaintUrlIndicators(Graphics graphics, Rectangle rectangle)
        {
            if (!isValidUrl)
            {
                graphics.DrawErrorSquiggly(linkLabelUrl.Text, linkLabelUrl.Font, rectangle);
            }
        }

        private void timerUrlValidation_Tick(object sender, EventArgs e)
        {
            if (textEditUrl != null && textEditUrl.IsHandleCreated)
            {
                var url = textEditUrl.Text;

                if (url.IsValidUrl())
                {
                    textEditUrl.IsValidText = true;
                    isValidUrl = true;

                    // ShowLinkProperties(textEditUrl);
                }
                else
                {
                    textEditUrl.IsValidText = false;
                    isValidUrl = false;

                    // HideLinkProperties(textEditUrl);
                }
            }
            else if (linkLabelUrl.Text != defaultLabelUrlText)
            {
                var url = linkLabelUrl.Text;

                if (lblUrlNote.Visible)
                {
                    lblUrlNote.Visible = false;
                }

                if (url.IsValidUrl())
                {
                    isValidUrl = true;
                }
                else
                {
                    isValidUrl = false;

                    using (var graphics = linkLabelUrl.CreateGraphics())
                    {
                        PaintUrlIndicators(graphics, linkLabelUrl.Bounds);
                    }
                }
            }
            else
            {
                isValidUrl = true;
                linkLabelUrl.Refresh();
            }

            timerUrlValidation.Stop();
        }

        private void linkLabelUrl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ShowUrlTextEdit();
            }
        }

        private void toolTip_Popup(object sender, PopupEventArgs e)
        {
            toolTip.SetToolTip(lblUrlNote, this.Url);
            toolTip.SetToolTip(linkLabelUrl, this.Url);
        }
    }
}
