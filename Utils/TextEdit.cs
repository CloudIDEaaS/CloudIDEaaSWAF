using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Drawing;
using System.Drawing.Text;
using System.Diagnostics;
using Clipboard = System.Windows.Forms.Clipboard;
using Utils.TextEditElements;

namespace Utils
{
    public partial class TextEdit : Control
    {
        private Caret caret;
        private TEXTMETRIC tm;
        private int nCharX;
        private int nCharY;
        private ushort nWindowX;
        private ushort nWindowY;
        private int nWindowCharsX;
        private int nWindowCharsY;
        private int nCaretPosX;
        private int nCaretPosY;
        private StringBuilder builder;
        private bool isValidText;
        private string oldText;
        private LinkedList<Word> words;
        public const int TEXT_X = 0;
        public const int TEXT_Y = 1;
        private int xLocation;
        private int charLocation;
        public Color BorderColor { get; set; }

        public TextEdit(string text = null)
        {
            builder = new StringBuilder();
            words = new LinkedList<Word>();

            isValidText = true;

            if (text == null)
            {
                builder = new StringBuilder();
            }
            else
            {
                builder = new StringBuilder(text);
            }

            InitializeComponent();

            SetStyle(ControlStyles.Selectable, true);
            this.Cursor = Cursors.IBeam;
        }

        public override string Text
        {
            get
            {
                return builder.ToString();
            }
        }

        protected override void WndProc(ref Message m)
        {
            var message = (Utils.ControlExtensions.WindowsMessage)m.Msg;
            Graphics graphics;
            Keys key;
            System.Drawing.Point point;

            switch (message)
            {
                case ControlExtensions.WindowsMessage.CREATE:

                    using (graphics = this.CreateGraphics())
                    {
                        tm = graphics.GetTextMetrics(this.Font);
                    }

                    nCharX = tm.tmAveCharWidth; 
                    nCharY = tm.tmHeight;
                    this.BackColor = System.Drawing.SystemColors.Window;

                    break;

                case ControlExtensions.WindowsMessage.SIZE:

                    var loHigh = m.LParam.ToLowHiWord();

                    nWindowX = loHigh.Low;
                    nWindowCharsX = Math.Max(1, nWindowX / nCharX); 

                    nWindowY = loHigh.High;
                    nWindowCharsY = Math.Max(1, nWindowY/nCharY);

                    if (caret != null)
                    {
                        caret.Position = new System.Drawing.Point(0, 0);
                    }

                    break;

                case ControlExtensions.WindowsMessage.KEYDOWN:

                    key = (Keys)m.WParam;

                    switch (key)
                    {
                        case Keys.Home:
                            nCaretPosX = 0; 
                            break;
                        case Keys.End:
                            nCaretPosX = nWindowCharsX - 1; 
                            break;
                        case Keys.PageUp:
                            nCaretPosY = 0; 
                            break;
                        case Keys.PageDown:
                            nCaretPosY = nWindowCharsY - 1; 
                            break;
                        case Keys.Left:
                            {
                                if (charLocation > 0)
                                {
                                    Character character;

                                    point = caret.Position;

                                    caret.Hide();
                                    this.Invalidate(caret.Rect, true);
                                    this.Update();

                                    nCaretPosX = Math.Max(nCaretPosX - 1, 0);
                                    charLocation--;

                                    character = this.AllChars[charLocation];
                                    xLocation = character.Rect.Right;

                                    point.X = xLocation + TEXT_X;

                                    caret.Position = point;
                                    caret.Show();
                                }
                            }
                            break;
                        case Keys.Right:
                            {
                                if (charLocation < this.AllChars.Count)
                                {
                                    Character character;

                                    point = caret.Position;

                                    caret.Hide();
                                    this.Invalidate(caret.Rect, true);
                                    this.Update();

                                    nCaretPosX = Math.Max(nCaretPosX - 1, 0);
                                    charLocation++;

                                    character = this.AllChars[charLocation];
                                    xLocation = character.Rect.Right;

                                    point.X = xLocation + TEXT_X;

                                    caret.Position = point;
                                    caret.Show();
                                }
                            }
                            break;
                        case Keys.Up:
                            nCaretPosY = Math.Max(nCaretPosY - 1, 0); 
                            break;
                        case Keys.Down:
                            nCaretPosY = Math.Min(nCaretPosY + 1, nWindowCharsY - 1); 
                            break;
                        case Keys.V:
                            if (Keys.ControlKey.IsPressed())
                            {
                                var newText = Clipboard.GetText();

                                builder.Append(newText);

                                nCaretPosY += newText.Length;
                            }
                            break;
                        case Keys.Delete:

                            if (builder.Length > 0)
                            {
                                var lastWord = this.words.Last.Value;

                                oldText = string.Empty;

                                builder.RemoveEnd(1);

                                if (lastWord.Characters.Count > 0)
                                {
                                    lastWord.Characters.RemoveLast();
                                }
                                else
                                {
                                    this.words.RemoveLast();
                                    lastWord = this.words.Last.Value;

                                    lastWord.Characters.RemoveLast();
                                }

                                charLocation = this.AllChars.Count;
                                xLocation = this.words.Last().Rect.Right;

                                point = caret.Position;

                                caret.Hide();
                                this.Invalidate(caret.Rect, true);
                                this.Update();

                                using (graphics = this.CreateGraphics())
                                {
                                    CalculateElements(graphics, builder);

                                    point = DrawText(graphics);
                                }

                                caret.Position = point;
                                caret.Show();
                            }

                            return;
                    }

                    m.Result = IntPtr.Zero;
                    break;

                case ControlExtensions.WindowsMessage.CHAR:

                    var ch = (char)m.WParam;

                    key = ch.ToKey();

                    switch (key)
                    {
                        case Keys.Back:

                            this.SendMessage(ControlExtensions.WindowsMessage.KEYDOWN, (int) Keys.Delete, 1L);
                            break;

                        case Keys.Tab:

                                do 
                                {
                                    this.SendMessage(ControlExtensions.WindowsMessage.CHAR, ' ', 1L); 
                                } 
                                while (nCaretPosX % 4 != 0); 

                            break;

                        case Keys.Return:
                   
                            nCaretPosX = 0;

                            if (++nCaretPosY == nWindowCharsY)
                            {
                                nCaretPosY = 0;
                            }

                            break;

                        case Keys.Escape:
                        case Keys.LineFeed:

                            this.Beep(ControlExtensions.BeepType.OK);
                            break;

                        default:

                            oldText = this.Text;

                            builder.Append(key.ToAscii());

                            caret.Hide();

                            using (graphics = this.CreateGraphics())
                            {
                                CalculateElements(graphics, builder);

                                point = DrawText(graphics, false);
                            }

                            caret.Position = point;
                            caret.Show();

                            charLocation = this.AllChars.Count;
                            xLocation = this.words.Last().Rect.Right;

                            m.Result = IntPtr.Zero;

                            break;
                    }

                    break;

                case ControlExtensions.WindowsMessage.ERASEBKGND:

                    break;

                case ControlExtensions.WindowsMessage.SETFOCUS:

                    caret = new Caret(this, 0, this.Height - 2);
                    caret.Position = new System.Drawing.Point(2, 2);
                    caret.Show();

                    break;

                case ControlExtensions.WindowsMessage.DESTROY:
                    break;

                case ControlExtensions.WindowsMessage.KILLFOCUS:

                    if (caret != null)
                    {
                        caret.Dispose();
                    }

                    break;
            }

            base.WndProc(ref m);
        }

        private List<Character> AllChars
        {
            get
            {
                return this.words.SelectMany(w => w.Characters).ToList();
            }
        }

        private void CalculateElements(Graphics graphics, StringBuilder builder)
        {
            var text = builder.ToString();

            if (text.Length > 0)
            {
                var words = builder.ToString().SplitToWords(false);
                var index = 0;
                var length = words.Length;
                var spaceSize = new System.Drawing.Size();
                var y = TEXT_Y;
                var xChar = TEXT_X;
                var xWord = TEXT_X;
                Rectangle wordRect;
                Rectangle charRect;
                Character character;
                Word word;

                this.words.Clear();

                foreach (var wordText in words)
                {
                    var charsList = new LinkedList<Character>();
                    var wordSize = graphics.GetTextExtentPoint(this.Font, wordText);

                    wordRect = new Rectangle(new System.Drawing.Point(xWord, y), wordSize);
                    xWord += wordSize.Width;

                    foreach (var ch in wordText)
                    {
                        var charSize = graphics.GetTextExtentPoint(this.Font, ch.ToString());

                        charRect = new Rectangle(new System.Drawing.Point(xChar, y), charSize);
                        xChar += charSize.Width;

                        character = new Character(ch, charRect);
                        charsList.AddLast(character);
                    }

                    if (index < length - 1)
                    {
                        var ch = ' ';

                        if (spaceSize.IsEmpty)
                        {
                            spaceSize = graphics.GetTextExtentPoint(this.Font, ch.ToString());
                        }

                        charRect = new Rectangle(new System.Drawing.Point(xChar, y), spaceSize);
                        xChar += spaceSize.Width;

                        character = new Character(ch, charRect);
                        charsList.AddLast(character);
                    }

                    word = new Word(wordText, wordRect, charsList);
                    this.words.AddLast(word);

                    index++;
                }
            }
        }

        public bool IsValidText
        {
            get
            {
                return isValidText;
            }

            set
            {
                var oldValue = isValidText;

                isValidText = value;

                if (oldValue != isValidText)
                {
                    oldText = string.Empty;

                    using (var graphics = this.CreateGraphics())
                    {
                        CalculateElements(graphics, builder);

                        DrawText(graphics, true);
                    }
                }
            }
        }

        private System.Drawing.Point DrawText(Graphics graphics, bool eraseBkgd = true)
        {
            var brush = new SolidBrush(this.BackColor);
            var fontBrush = new SolidBrush(this.ForeColor);
            var size = graphics.GetTextExtentPoint(this.Font, this.Text);
            var rect = new Rectangle(TEXT_X, TEXT_Y, (int) size.Width, (int) size.Height);
            var point = new System.Drawing.Point((int)rect.Right + 1, (int)rect.Top);
            var clientRect = this.ClientRectangle;
            System.Drawing.Rectangle clipRect;
            System.Drawing.SizeF oldSize;

            if (!eraseBkgd && oldText != null && oldText.Length > 0 && this.Text.Length > oldText.Length)
            {
                oldSize = graphics.GetTextExtentPoint(this.Font, oldText);
                clipRect = new Rectangle((int) oldSize.Width - 3, 0, (int) (clientRect.Width - oldSize.Width), clientRect.Height);

                graphics.SetClip(clipRect);
            }

            if (eraseBkgd)
            {
                graphics.FillRectangle(brush, clientRect);
            }

            graphics.DrawString(this.Text, this.Font, fontBrush, rect.Location);

            if (eraseBkgd)
            {
                clientRect.Height -= 1;

                graphics.DrawEdge(clientRect, EdgeStyle.EdgeBump, BorderFlags.Rect);
            }

            if (!isValidText)
            {
                graphics.DrawErrorSquiggly(this.Text, this.Font, rect);
            }

            brush.Dispose();

            graphics.ResetClip();

            return point;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;

            CalculateElements(graphics, builder);

            DrawText(graphics);
        }
    }
}
