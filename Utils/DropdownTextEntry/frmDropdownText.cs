using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Utils.ControlExtensions;

namespace Utils.DropdownTextEntry
{
    public partial class frmDropdownText : ChildPopupForm
    {
        public event OnTabEventHandler OnTabEventHandler;
        public event EventHandler DocumentTextChanged;

        public bool ReadOnly { get; internal set; }

        public frmDropdownText(Control control) : base(control, true, false, false)
        {
            InitializeComponent();
        }

        public frmDropdownText(Control control, bool asControl = false, bool allowMove = false, bool autoSetPosition = true) : base(control, asControl, allowMove, autoSetPosition)
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == (int) WindowsMessage.PAINT)
            {
                var graphics = this.CreateGraphics();
                var rect = this.ClientRectangle;

                rect.Inflate(-1, -1);

                graphics.DrawRectangle(Pens.Black, rect);

                m.Result = new IntPtr(1);
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (this.Visible)
            {
                textBox.SetFocus();
            }

            base.OnVisibleChanged(e);
        }

        public override string Text 
        {
            get
            {
                if (textBox != null)
                {
                    return textBox.Text;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (textBox != null)
                {
                    textBox.Text = value;
                }
            }
        }

        private void frmDropdownText_Load(object sender, EventArgs e)
        {
            if (this.ReadOnly)
            {
#if !NOHUNSPELL
                nHunspellTextBoxExtender.SetSpellCheckEnabled(this.textBox, false);
#endif
                textBox.ReadOnly = true;
            }
            else
            {
#if !NOHUNSPELL
                nHunspellTextBoxExtender.SetSpellCheckEnabled(this.textBox, true);
#endif
            }

            textBox.SetFocus();
        }

        private void textBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                var tabEventArgs = new TabEventArgs(!e.Modifiers.HasFlag(Keys.Shift));

                OnTabEventHandler?.Invoke(this, tabEventArgs);

                if (!tabEventArgs.SkipHide)
                {
                    this.Hide();
                }
            }
            else if (Keys.ControlKey.IsPressed() && e.KeyCode == Keys.Up)
            {
                this.Hide();
            }
        }

        public void ScrollToEnd()
        {
            textBox.SelectionStart = textBox.Text.Length;

            textBox.ScrollToCaret();
        }

        private void cmdCollapse_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            DocumentTextChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
