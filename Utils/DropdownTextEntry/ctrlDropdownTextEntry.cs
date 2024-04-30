using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils.DropdownTextEntry;
using static Utils.ControlExtensions;

namespace Utils.DropdownTextEntry
{
    public partial class ctrlDropdownTextEntry : UserControl
    {
        private frmDropdownText frmDropdownText;
        private bool entered;
        private bool noFocus;
        private bool noDropdown;

        public override string Text { get => textBox.Text; set => textBox.Text = value; }

        [Browsable(true)]
        public new event EventHandler TextChanged
        {
            add
            {
                textBox.TextChanged += value;
            }

            remove
            {
                textBox.TextChanged -= value;
            }
        }

        public ctrlDropdownTextEntry()
        {
            InitializeComponent();
        }

        public bool ReadOnly
        {
            get
            {
                return textBox.ReadOnly;
            }

            set
            {
                textBox.ReadOnly = value;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            var designMode = this.InDesignMode();

            if (designMode)
            {
                return;
            }    

            this.DelayInvoke(1, () =>
            {
                frmDropdownText = new frmDropdownText(this.ParentForm);

                frmDropdownText.ReadOnly = this.ReadOnly;
                frmDropdownText.Shown += frmDropdownText_Shown;
                frmDropdownText.FormClosing += frmDropdownText_FormClosing;
                frmDropdownText.VisibleChanged += frmDropdownText_VisibleChanged;
                frmDropdownText.OnTabEventHandler += frmDropdownText_OnTabEventHandler;
            });

            base.OnHandleCreated(e);
        }

        private void frmDropdownText_OnTabEventHandler(object sender, TabEventArgs e)
        {
            noFocus = true;

            frmDropdownText.Hide();

            this.DelayInvoke(2, () =>
            {
                this.SelectNext(e.Forward);
            });
        }

        private void frmDropdownText_VisibleChanged(object sender, EventArgs e)
        {
            var designMode = this.InDesignMode();

            if (designMode)
            {
                return;
            }

            if (!frmDropdownText.Visible)
            {
                textBox.Text = frmDropdownText.Text.RemoveText("\t");
                entered = false;

                if (noFocus)
                {
                    noFocus = false;
                    return;
                }

                this.DelayInvoke(1, () =>
                {
                    textBox.GetParentForm().Activate();

                    noDropdown = true;
                    textBox.Focus();
                    noDropdown = false;
                });
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (frmDropdownText != null)
            {
                frmDropdownText.Close();
                frmDropdownText.Dispose();
            }

            base.OnHandleDestroyed(e);
        }

        private void frmDropdownText_FormClosing(object sender, FormClosingEventArgs e)
        {
            var designMode = this.InDesignMode();

            if (designMode)
            {
                return;
            }

            textBox.Text = frmDropdownText.Text.RemoveText("\t");
            entered = false;
        }

        private void frmDropdownText_Shown(object sender, EventArgs e)
        {
            var point = this.PointToScreen(new Point(0, this.Height + 3));
            point = this.ParentForm.PointToClient(point);

            frmDropdownText.SetWindowPos(new Rectangle(point, new Size()), ControlExtensions.SetWindowPosFlags.IgnoreResize);
        }

        private void ctrlDropdownTextEntry_Enter(object sender, EventArgs e)
        {
            var designMode = this.InDesignMode();
            List<ctrlDropdownTextEntry> textEntries;

            if (noDropdown)
            {
                return;
            }

            if (designMode)
            {
                return;
            }

            textEntries = this.GetParentForm().GetAllControls().OfType<ctrlDropdownTextEntry>().Where(c => c != this).ToList();
            textEntries.ForEach(t => t.Hide());

            entered = true;
            HideOrShow();
        }

        private void HideOrShow()
        {
            if (frmDropdownText.Visible)
            {
                frmDropdownText.Hide();
            }
            else
            {
                Show();
            }
        }

        private new void Show()
        {
            if (frmDropdownText == null)
            {
                this.DoEvents();
            }

            Debug.WriteLine("Showing for " + this.Name);

            if (frmDropdownText == null)
            {

            }

            frmDropdownText.Show();

            frmDropdownText.Text = textBox.Text;

            this.DelayInvoke(1, () =>
            {
                frmDropdownText.SetFocus();
            });
        }

        private new void Hide()
        {
            var designMode = this.InDesignMode();

            if (designMode)
            {
                return;
            }

            if (frmDropdownText != null && frmDropdownText.Visible)
            {
                frmDropdownText.Hide();
            }
        }

        private void ctrlDropdownTextEntry_Leave(object sender, EventArgs e)
        {
            var designMode = this.InDesignMode();

            if (designMode)
            {
                return;
            }

            if (frmDropdownText != null)
            {
                frmDropdownText.Hide();
            }
        }

        private void textBox_Click(object sender, EventArgs e)
        {
            var designMode = this.InDesignMode();

            if (designMode)
            {
                return;
            }

            if (entered)
            {
                entered = false;
                return;
            }

            HideOrShow();
        }

        private void textBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                Show();
            }
        }
    }
}
