// file:	ctrlEditor.cs
//
// summary:	Implements the control editor class

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GemBox.Document;
using Utils;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Utils.RichEdit;
using Utils.TextObjectModel;
using System.Threading;
using TextObjectModel;
#if !NOHUNSPELL
using NHunspellExtender;
#endif
using System.Drawing.Imaging;

namespace Utils
{
    /// <summary>   Editor for Control. </summary>
    ///
    /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>

    public partial class ctrlEditor : UserControl, IMessageFilter
    {
        /// <summary>   True to loading. </summary>
        private bool loading;
        private Dictionary<string, EmbeddedEmoji> embeddedEmojis;

        /// <summary>   Gets or sets a value indicating whether the tab next. </summary>
        ///
        /// <value> True if tab next, false if not. </value>

        public bool TabNext { get; set; }

        /// <summary>   Gets or sets a value indicating whether the tab previous. </summary>
        ///
        /// <value> True if tab previous, false if not. </value>

        public bool TabPrevious { get; set; }

        /// <summary>   Gets or sets a value indicating whether this  has changes. </summary>
        ///
        /// <value> True if this  has changes, false if not. </value>

        public bool HasChanges { get; set; }


        /// <summary>   The shortcut buttons. </summary>
        private Dictionary<Keys, ToolStripButton> shortcutButtons;
        private IRichEditOle richEditOle;
        private IOleTextDocument textDocument;
        private List<LinkData> linkDataList;
        private ManualResetEvent emojiResetEvent;
        private TextDocument document;
        private frmEmojisPalette frmEmojisPalette;
        private const int S_OK = 0;
        public event OnImageSelectedHandler OnImageSelected;
        public event OnLinkSelectedHandler OnLinkSelected;
        public event OnLinkChangedHandler OnLinkChanged;
        public event EventHandler OnSelectionChanged;
        public event EventHandler DocumentTextChanged;
        public event EventHandler DocumentLeave;
        public event OnTextSelectedHandler OnTextSelected;

        [Flags]
        enum EmfToWmfBitsFlags
        {
            EmfToWmfBitsFlagsDefault = 0x00000000,
            EmfToWmfBitsFlagsEmbedEmf = 0x00000001,
            EmfToWmfBitsFlagsIncludePlaceable = 0x00000002,
            EmfToWmfBitsFlagsNoXORClip = 0x00000004
        }

        const int MM_ISOTROPIC = 7;
        const int MM_ANISOTROPIC = 8;

        [DllImport("gdiplus.dll")]
        private static extern uint GdipEmfToWmfBits(IntPtr _hEmf, uint _bufferSize,
            byte[] _buffer, int _mappingMode, EmfToWmfBitsFlags _flags);
        [DllImport("gdi32.dll")]
        private static extern IntPtr SetMetaFileBitsEx(uint _bufferSize,
            byte[] _buffer);
        [DllImport("gdi32.dll")]
        private static extern IntPtr CopyMetaFile(IntPtr hWmf,
            string filename);
        [DllImport("gdi32.dll")]
        private static extern bool DeleteMetaFile(IntPtr hWmf);
        [DllImport("gdi32.dll")]
        private static extern bool DeleteEnhMetaFile(IntPtr hEmf);
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>

        public ctrlEditor()
        {
            var regex = new Regex(@"Ctrl\+(?<key>.{1})$");

            InitializeComponent();

            document = TextDocument.FromRichTextBox(richTextBox, true);
            frmEmojisPalette = new frmEmojisPalette();

            frmEmojisPalette.CreateControl();
            embeddedEmojis = new Dictionary<string, EmbeddedEmoji>();

            shortcutButtons = new Dictionary<Keys, ToolStripButton>();
            linkDataList = new List<LinkData>();

            foreach (var toolStripButton in toolStrip.Items.OfType<ToolStripButton>().Where(b => b.Tag != null))
            {
                var tag = (string)toolStripButton.Tag;

                if (regex.IsMatch(tag))
                {
                    var keyText = regex.GetGroupValue(tag, "key");
                    var key = (Keys) EnumUtils.GetValues<Keys>().Single(k => k.ToAscii().AsCaseless() == keyText);

                    shortcutButtons.Add(key, toolStripButton);
                }
            }

            ComponentInfo.SetLicense("DN-2022Oct27-ExAJJ1rqwZEFOjfKzhSiGwbbVXlMagUCD2aly1BBtm0/006lykvqazgKxg7QwBi3dtZoooFaxasoKmviJ9ffH0CZd7w==A");

            Application.AddMessageFilter(this);
        }

        private void GetOleInterfaces()
        {
            var ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(IntPtr)));
            IntPtr result;
            IntPtr textDocumentPtr;

            Marshal.WriteIntPtr(ptr, IntPtr.Zero);
            result = richTextBox.SendMessage(ControlExtensions.WindowsMessage.EM_GETOLEINTERFACE, IntPtr.Zero, ptr);

            if (result == IntPtr.Zero)
            {
                DebugUtils.Break();
            }
            else
            {
                var pRichEdit = Marshal.ReadIntPtr(ptr);

                richEditOle = (IRichEditOle)Marshal.GetObjectForIUnknown(pRichEdit);

                if (richEditOle == null)
                {
                    DebugUtils.Break();
                }

                // IID_ITextDocument
                var guid = new Guid("8CC497C0-A1DF-11CE-8098-00AA0047BE5D");
                Marshal.QueryInterface(pRichEdit, ref guid, out textDocumentPtr);

                // Wrap it in the C# interface for IRichEditOle
                textDocument = (IOleTextDocument)Marshal.GetTypedObjectForIUnknown(textDocumentPtr, typeof(IOleTextDocument));

                if (textDocument == null)
                {
                    DebugUtils.Break();
                }
            }

            Marshal.FreeCoTaskMem(ptr);
        }

        /// <summary>   Event handler. Called by menuItemOpen for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void menuItemOpen_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                AddExtension = true,
                Filter =
                    "All Documents (*.docx;*.docm;*.doc;*.dotx;*.dotm;*.dot;*.htm;*.html;*.rtf;*.txt)|*.docx;*.docm;*.dotx;*.dotm;*.doc;*.dot;*.htm;*.html;*.rtf;*.txt|" +
                    "Word Documents (*.docx)|*.docx|" +
                    "Word Macro-Enabled Documents (*.docm)|*.docm|" +
                    "Word 97-2003 Documents (*.doc)|*.doc|" +
                    "Word Templates (*.dotx)|*.dotx|" +
                    "Word Macro-Enabled Templates (*.dotm)|*.dotm|" +
                    "Word 97-2003 Templates (*.dot)|*.dot|" +
                    "Web Pages (*.htm;*.html)|*.htm;*.html|" +
                    "Rich Text Format (*.rtf)|*.rtf|" +
                    "Text Files (*.txt)|*.txt"
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                using (var stream = new MemoryStream())
                {
                    // Convert input file to RTF stream.
                    DocumentModel.Load(dialog.FileName).Save(stream, SaveOptions.RtfDefault);

                    stream.Position = 0;

                    // Load RTF stream into RichTextBox.
                    this.richTextBox.LoadFile(stream, RichTextBoxStreamType.RichText);
                }
        }

        /// <summary>   Loads a file. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="fileName"> Filename of the file. </param>
        /// <param name="fileType"> Type of the file. </param>

        public void LoadFile(string fileName, RichTextBoxStreamType fileType)
        {
            this.richTextBox.LoadFile(fileName, fileType);
        }

        /// <summary>   Clears this  to its blank/initial state. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>

        public void Clear()
        {
            this.richTextBox.Clear();
        }

        /// <summary>   Gets the text. </summary>
        ///
        /// <value> The text. </value>

        public override string Text
        {
            get
            {
                var text = richTextBox.Text;
                var builder = new StringBuilder(text);

                if (embeddedEmojis.Count > 0)
                {
                    var ole = new RichEditOle(richTextBox);
                    var richEditOle = ole.IRichEditOle;
                    var count = richEditOle.GetObjectCount();
                    var objects = new Dictionary<string, REOBJECT>();
                    int hr;

                    for (var x = 0; x < count; x++)
                    {
                        var reObject = new REOBJECT();

                        hr = richEditOle.GetObject(x, reObject, GETOBJECTOPTIONS.REO_GETOBJ_ALL_INTERFACES);

                        if (hr == S_OK)
                        {
                            var characterPoint = reObject.cp;
                            var range = document.Range(characterPoint, 1);

                            var selectedRtf = range.Rtf;
                            string pattern;

                            pattern = @"(?s)\{(?<image>\\pict\{.*?\r\n.*?\})";

                            if (Regex.IsMatch(selectedRtf, pattern))
                            {
                                var match = Regex.Match(selectedRtf, pattern);
                                var identifier = match.GetGroupValue("image");

                                objects.Add(identifier, reObject);
                            }
                        }
                    }

                    foreach (var pair in embeddedEmojis.Reverse())
                    {
                        var embeddedEmoji = pair.Value;
                        var emoji = embeddedEmoji.Emoji;
                        var characters = emoji.Chars;
                        var identifier = pair.Key;

                        if (objects.ContainsKey(identifier))
                        {
                            var reObjectFound = objects[identifier];
                            var characterPoint = reObjectFound.cp;

                            builder.Insert(characterPoint, emoji.Chars);
                        }
                    }
                }

                return builder.ToString();
            }

            set
            {
                richTextBox.Text = value;
            }
        }

        public string Rtf
        {
            get
            {
                if (embeddedEmojis.Count > 0)
                {
                    var ole = new RichEditOle(richTextBox);
                    var richEditOle = ole.IRichEditOle;
                    var count = richEditOle.GetObjectCount();
                    var objects = new Dictionary<string, global::TextObjectModel.TextRange>();
                    int hr;

                    for (var x = 0; x < count; x++)
                    {
                        var reObject = new REOBJECT();

                        hr = richEditOle.GetObject(x, reObject, GETOBJECTOPTIONS.REO_GETOBJ_ALL_INTERFACES);

                        if (hr == S_OK)
                        {
                            var characterPoint = reObject.cp;
                            var range = document.Range(characterPoint, 1);

                            var selectedRtf = range.Rtf;
                            string pattern;

                            pattern = @"(?s)\{(?<image>\\pict\{.*?\r\n.*?\})";

                            if (Regex.IsMatch(selectedRtf, pattern))
                            {
                                var match = Regex.Match(selectedRtf, pattern);
                                var identifier = match.GetGroupValue("image");

                                objects.Add(identifier, range);
                            }
                        }
                    }

                    foreach (var pair in embeddedEmojis.Reverse())
                    {
                        var embeddedEmoji = pair.Value;
                        var emoji = embeddedEmoji.Emoji;
                        var characters = emoji.Chars;
                        var identifier = pair.Key;

                        if (objects.ContainsKey(identifier))
                        {
                            var textRange = (global::TextObjectModel.TextRange) objects[identifier];

                            textRange.Text = characters;
                        }
                    }
                }

                return richTextBox.Rtf;
            }

            set
            {
                richTextBox.Rtf = value;
            }
        }

        public ToolStrip ToolStrip
        {
            get
            {
                return toolStrip;
            }
        }

        public RichTextBox RichTextBox
        {
            get
            {
                return richTextBox;
            }
        }

        public void AppendFormattedText(string text, System.Drawing.Color textColour, Font font, System.Windows.Forms.HorizontalAlignment alignment = System.Windows.Forms.HorizontalAlignment.Left)
        {
            var rtb = this.richTextBox;
            int start = rtb.TextLength;
            rtb.AppendText(text);
            int end = rtb.TextLength; // now longer by length of appended text

            // Select text that was appended
            rtb.Select(start, end - start);

            #region Apply Formatting
            rtb.SelectionColor = textColour;
            rtb.SelectionAlignment = alignment;
            rtb.SelectionFont = font;
            #endregion

            // Unselect text
            rtb.SelectionLength = 0;
        }
        /// <summary>   Gets or sets the rich text. </summary>
        ///
        /// <value> The rich text. </value>

        public string RichText 
        {
            get
            {
                using (var stream = new MemoryStream())
                {
                    // Save RichTextBox content to RTF stream.
                    this.richTextBox.SaveFile(stream, RichTextBoxStreamType.RichText);

                    stream.Position = 0;

                    return stream.ToText();
                }
            }

            set
            {
                if (!value.IsNullWhiteSpaceOrEmpty())
                {
                    using (var stream = value.ToStream())
                    {
                        // Convert input file to RTF stream.

                        stream.Position = 0;

                        // Load RTF stream into RichTextBox.

                        this.loading = true;

                        using (this.CreateDisposable((sender, e) => this.loading = false))
                        {
                            this.richTextBox.LoadFile(stream, RichTextBoxStreamType.RichText);  
                        }
                    }
                }
            }
        }

        /// <summary>   Event handler. Called by btnSave for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnSave_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog()
            {
                AddExtension = true,
                Filter =
                    "Word Document (*.docx)|*.docx|" +
                    "Word Macro-Enabled Document (*.docm)|*.docm|" +    
                    "Word Template (*.dotx)|*.dotx|" +
                    "Word Macro-Enabled Template (*.dotm)|*.dotm|" +
                    "PDF (*.pdf)|*.pdf|" +
                    "XPS Document (*.xps)|*.xps|" +
                    "Web Page (*.htm;*.html)|*.htm;*.html|" +
                    "Single File Web Page (*.mht;*.mhtml)|*.mht;*.mhtml|" +
                    "Rich Text Format (*.rtf)|*.rtf|" +
                    "Plain Text (*.txt)|*.txt|" +
                    "Image (*.png;*.jpg;*.jpeg;*.gif;*.bmp;*.tif;*.tiff;*.wdp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp;*.tif;*.tiff;*.wdp"
            };

            if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                using (var stream = new MemoryStream())
                {
                    // Save RichTextBox content to RTF stream.
                    this.richTextBox.SaveFile(stream, RichTextBoxStreamType.RichText);

                    stream.Position = 0;

                    // Convert RTF stream to output format.
                    DocumentModel.Load(stream, LoadOptions.RtfDefault).Save(dialog.FileName);
                    Process.Start(dialog.FileName);
                }
            }
        }

        public void SaveAsRtf(string fileName)
        {
            this.richTextBox.SaveFile(fileName, RichTextBoxStreamType.RichText);
        }

        public void AppendText(string text)
        {
            richTextBox.AppendText(text);
        }

        /// <summary>   Event handler. Called by btnGemBoxCut for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnGemBoxCut_Click(object sender, EventArgs e)
        {
            this.DoGemBoxCopy();

            // Clear selection.
            this.richTextBox.SelectedRtf = string.Empty;
        }

        /// <summary>   Event handler. Called by btnGemBoxCopy for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnGemBoxCopy_Click(object sender, EventArgs e)
        {
            this.DoGemBoxCopy();
        }

        /// <summary>   Event handler. Called by btnGemBoxPastePrepend for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnGemBoxPastePrepend_Click(object sender, EventArgs e)
        {
            this.DoGemBoxPaste(true);
        }

        /// <summary>   Event handler. Called by btnGemBoxPasteAppend for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnGemBoxPasteAppend_Click(object sender, EventArgs e)
        {
            this.DoGemBoxPaste(false);
        }

        public void DoPaste()
        {
            this.DoGemBoxPaste(false);
        }

        /// <summary>   Event handler. Called by btnUndo for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (this.richTextBox.CanUndo)
                this.richTextBox.Undo();
        }

        /// <summary>   Event handler. Called by btnRedo for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnRedo_Click(object sender, EventArgs e)
        {
            if (this.richTextBox.CanRedo)
                this.richTextBox.Redo();
        }

        /// <summary>   Event handler. Called by btnCut for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnCut_Click(object sender, EventArgs e)
        {
            this.richTextBox.Cut();
        }

        /// <summary>   Event handler. Called by btnCopy for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnCopy_Click(object sender, EventArgs e)
        {
            this.richTextBox.Copy();
        }

        /// <summary>   Event handler. Called by btnPaste for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                this.richTextBox.Paste(DataFormats.GetFormat(DataFormats.Text));
            }
            else
            {
                this.richTextBox.Paste();
            }
        }

        /// <summary>   Event handler. Called by btnBold for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnBold_Click(object sender, EventArgs e)
        {
            this.richTextBox.SelectionFont = new Font(this.richTextBox.SelectionFont.FontFamily, this.richTextBox.SelectionFont.Size, this.ToggleFontStyle(this.richTextBox.SelectionFont.Style, FontStyle.Bold));
        }

        /// <summary>   Event handler. Called by btnItalic for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnItalic_Click(object sender, EventArgs e)
        {
            this.richTextBox.SelectionFont = new Font(this.richTextBox.SelectionFont.FontFamily, this.richTextBox.SelectionFont.Size, this.ToggleFontStyle(this.richTextBox.SelectionFont.Style, FontStyle.Italic));
        }

        /// <summary>   Event handler. Called by btnUnderline for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnUnderline_Click(object sender, EventArgs e)
        {
            this.richTextBox.SelectionFont = new Font(this.richTextBox.SelectionFont.FontFamily, this.richTextBox.SelectionFont.Size, this.ToggleFontStyle(this.richTextBox.SelectionFont.Style, FontStyle.Underline));
        }

        /// <summary>   Event handler. Called by btnIncreaseFontSize for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnIncreaseFontSize_Click(object sender, EventArgs e)
        {
            this.richTextBox.SelectionFont = new Font(this.richTextBox.SelectionFont.FontFamily, this.richTextBox.SelectionFont.Size + 1, this.richTextBox.SelectionFont.Style);
        }

        /// <summary>   Event handler. Called by btnDecreaseFontSize for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnDecreaseFontSize_Click(object sender, EventArgs e)
        {
            this.richTextBox.SelectionFont = new Font(this.richTextBox.SelectionFont.FontFamily, this.richTextBox.SelectionFont.Size - 1, this.richTextBox.SelectionFont.Style);
        }

        /// <summary>   Event handler. Called by btnToggleBullets for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnToggleBullets_Click(object sender, EventArgs e)
        {
            this.richTextBox.SelectionBullet = !this.richTextBox.SelectionBullet;
        }

        /// <summary>   Event handler. Called by btnDecreaseIndentation for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnDecreaseIndentation_Click(object sender, EventArgs e)
        {
            this.richTextBox.SelectionIndent -= 10;
        }

        /// <summary>   Event handler. Called by btnIncreaseIndentation for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnIncreaseIndentation_Click(object sender, EventArgs e)
        {
            this.richTextBox.SelectionIndent += 10;
        }

        /// <summary>   Event handler. Called by btnAlignLeft for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnAlignLeft_Click(object sender, EventArgs e)
        {
            this.richTextBox.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Left;
        }

        /// <summary>   Event handler. Called by btnAlignCenter for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnAlignCenter_Click(object sender, EventArgs e)
        {
            this.richTextBox.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Center;
        }

        /// <summary>   Event handler. Called by btnAlignRight for click events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void btnAlignRight_Click(object sender, EventArgs e)
        {
            this.richTextBox.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Right;
        }

        /// <summary>   Executes the gem box copy operation. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>.0

        private void DoGemBoxCopy()
        {
            using (var stream = new MemoryStream())
            {
                // Save RichTextBox selection to RTF stream.
                var writer = new StreamWriter(stream);
                writer.Write(this.richTextBox.SelectedRtf);
                writer.Flush();

                stream.Position = 0;

                // Save RTF stream to clipboard.
                DocumentModel.Load(stream, LoadOptions.RtfDefault).Content.SaveToClipboard();
            }
        }

        /// <summary>   Executes the gem box paste operation. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="prepend">  True to prepend. </param>

        private void DoGemBoxPaste(bool prepend)
        {
            using (var stream = new MemoryStream())
            {
                // Save RichTextBox content to RTF stream.
                var writer = new StreamWriter(stream);
                writer.Write(this.richTextBox.SelectedRtf);
                writer.Flush();

                stream.Position = 0;

                // Load document from RTF stream and prepend or append clipboard content to it.
                var document = DocumentModel.Load(stream, LoadOptions.RtfDefault);
                var content = document.Content;
                (prepend ? content.Start : content.End).LoadFromClipboard();

                stream.Position = 0;

                // Save document to RTF stream.
                document.Save(stream, SaveOptions.RtfDefault);

                stream.Position = 0;

                // Load RTF stream into RichTextBox.
                var reader = new StreamReader(stream);
                this.richTextBox.SelectedRtf = reader.ReadToEnd();
            }
        }

        /// <summary>   Toggle font style. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="item">     The item. </param>
        /// <param name="toggle">   The toggle. </param>
        ///
        /// <returns>   A FontStyle. </returns>

        public FontStyle ToggleFontStyle(FontStyle item, FontStyle toggle)
        {
            return item ^ toggle;
        }

        /// <summary>   Sets default focus. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>

        public void SetDefaultFocus()
        {
            richTextBox.SetFocus();
        }

        /// <summary>   Event handler. Called by ctrlEditor for key down events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Key event information. </param>

        private void ctrlEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                if (e.Modifiers.HasFlag(Keys.Shift))
                {
                    this.TabPrevious = true;
                }
                else
                {
                    this.TabNext = true;
                }
            }
        }

        /// <summary>   Event handler. Called by richTextBox for text changed events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                this.HasChanges = true;
                DocumentTextChanged?.Invoke(sender, e);
            }
        }

        /// <summary>   Filters out a message before it is dispatched. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="m">    [in,out] The message to be dispatched. You cannot modify this message. </param>
        ///
        /// <returns>
        /// true to filter the message and stop it from being dispatched; false to allow the message to
        /// continue to the next filter or control.
        /// </returns>

        public bool PreFilterMessage(ref Message m)
        {
            var message = (Utils.ControlExtensions.WindowsMessage)m.Msg;
            var hwnd = m.HWnd;

            try
            {
                if (this.IsDisposed || this.Handle == (IntPtr)0x0)
                {
                    return false;
                }

                if (hwnd != this.Handle && !this.GetAllControls().Any(c => c.Handle == hwnd))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            if (message == ControlExtensions.WindowsMessage.KEYDOWN)
            {
                var key = (Keys)m.WParam;

                if (Keys.ControlKey.IsPressed())
                {
                    if (shortcutButtons.ContainsKey(key))
                    {
                        var button = shortcutButtons[key];

                        if (key == Keys.V)
                        {
                            using (var image = Clipboard.GetImage())
                            {
                                if (image != null)
                                {
                                    var rtf = GetImageRtf(image);

                                    richTextBox.SelectedRtf = rtf;

                                    return true;
                                }
                            }
                        }

                        button.PerformClick();

                        return true;
                    }
                }
            }

            return false;
        }

        public string GetImageRtf(Image image)
        {
            Metafile metafile = null;
            float dpiX; float dpiY;
            string proto;

            using (Graphics g = Graphics.FromImage(image))
            {
                IntPtr hDC = g.GetHdc();

                metafile = new Metafile(hDC, EmfType.EmfOnly);
                g.ReleaseHdc(hDC);
            }

            using (Graphics g = Graphics.FromImage(metafile))
            {
                g.DrawImage(image, 0, 0);

                dpiX = g.DpiX;
                dpiY = g.DpiY;
            }

            IntPtr _hEmf = metafile.GetHenhmetafile();
            uint _bufferSize = GdipEmfToWmfBits(_hEmf, 0, null, MM_ANISOTROPIC,
            EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
            byte[] _buffer = new byte[_bufferSize];
            GdipEmfToWmfBits(_hEmf, _bufferSize, _buffer, MM_ANISOTROPIC,
                                        EmfToWmfBitsFlags.EmfToWmfBitsFlagsDefault);
            IntPtr hmf = SetMetaFileBitsEx(_bufferSize, _buffer);
            string tempfile = Path.GetTempFileName();
            IntPtr hmf2 = CopyMetaFile(hmf, tempfile);
            
            DeleteMetaFile(hmf);
            DeleteMetaFile(hmf2);
            DeleteEnhMetaFile(_hEmf);

            using (var stream = new MemoryStream())
            { 
                byte[] data = File.ReadAllBytes(tempfile);
                File.Delete(tempfile);
                int count = data.Length;
                stream.Write(data, 0, count);

                proto = @"{\rtf1{\pict\wmetafile8\picw" + (int)image.Width
                                  + @"\pich" + (int)image.Height
                                  + @"\picwgoal" + (int)(((float)image.Width / dpiX) * 1440)
                                  + @"\pichgoal" + (int)(((float)image.Height / dpiY) * 1440)
                                  + " "
                                  + BitConverter.ToString(stream.ToArray()).Replace("-", "")
                                  + "}}";
            }

            return proto;
        }
    
        /// <summary>   Event handler. Called by frmEditor for form closing events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Form closing event information. </param>

        protected override void OnHandleDestroyed(EventArgs e)
        {
            Application.RemoveMessageFilter(this);

            base.OnHandleDestroyed(e);
        }

        /// <summary>   Event handler. Called by frmEditor for load events. </summary>
        ///
        /// <remarks>   CloudIDEaaS, 2/23/2021. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Event information. </param>

        private void frmEditor_Load(object sender, EventArgs e)
        {
            GetOleInterfaces();
        }

        private void richTextBox_SelectionChanged(object sender, EventArgs e)
        {
            if (richTextBox.SelectionType == RichTextBoxSelectionTypes.Object)
            {
                var selectedRtf = richTextBox.SelectedRtf;
                string pattern;

                pattern = @"(?s)\{(?<image>\\pict\\.*?)\r\n.*?\}";

                if (Regex.IsMatch(selectedRtf, pattern))
                {
                    var match = Regex.Match(selectedRtf, pattern);
                    var identifier = match.GetGroupValue("image");

                    if (OnImageSelected != null)
                    {
                        OnImageSelected(this, new OnImageSelectedEventArgs(identifier, selectedRtf));
                    }
                }

                return;
            }
            else
            {
                string selectedWord;
                var selectionStart = richTextBox.SelectionStart;
                var selectionLength = richTextBox.SelectionLength;
                int startPosition;
                int endPosition;
                string selectedRtf;

                OnTextSelected?.Invoke(this, new OnTextSelectedEventArgs(richTextBox.SelectedText, selectionStart, selectionLength));

                selectedWord = GetWordUnderCaret(selectionStart, out startPosition, out endPosition);

                if (!selectedWord.IsNullOrEmpty())
                {
                    if (selectedWord.IsValidEmailAddress() || selectedWord.IsValidUrl())
                    {
                        var newLinkData = new LinkData(selectedWord, startPosition, endPosition);

                        selectedRtf = richTextBox.SelectedRtf;

                        if (this.linkDataList.Any(d => d.Word != newLinkData.Word && d.Intersects(newLinkData)))
                        {
                            var linkDataIntersections = this.linkDataList.Where(d => d.Intersects(newLinkData)).ToList();

                            foreach (var linkData in linkDataIntersections)
                            {
                                var linkChangedEventArgs = new OnLinkChangedEventArgs(newLinkData.Word, linkData.Word);

                                OnLinkChanged(this, linkChangedEventArgs);

                                if (linkChangedEventArgs.DeleteOld)
                                {
                                    this.linkDataList.Remove(linkData);
                                }
                            }
                        }

                        if (!linkDataList.Any(d => d.Word == newLinkData.Word))
                        {
                            linkDataList.Add(newLinkData);
                        }

                        if (OnLinkSelected != null)
                        {
                            OnLinkSelected(this, new OnLinkSelectedEventArgs(selectedWord, selectedRtf, selectedWord));
                        }
                    }

                    return;
                }
            }

            OnSelectionChanged?.Invoke(this, e);
        }

        private string GetWordUnderCaret(int position, out int startPosition, out int endPosition)
        {
            string text = richTextBox.Text;

            for (startPosition = position; startPosition >= 0; startPosition--)
            {
                if (text.Length > startPosition)
                {
                    char ch = text[startPosition];

                    if (!char.IsLetterOrDigit(ch) && (ch.NotOneOf('_', '-', '.', '-', '@', '&', '=', '/', ':')))
                    {
                        break;
                    }
                }
            }

            startPosition++;

            for (endPosition = position; endPosition < text.Length; endPosition++)
            {
                char ch = text[endPosition];

                if (!char.IsLetterOrDigit(ch) && (ch.NotOneOf('_', '-', '.', '-', '@', '&', '=', '/', ':')))
                {
                    break;
                }
            }
            
            endPosition--;

            if (startPosition > endPosition)
            {
                return "";
            }

            return text.Substring(startPosition, endPosition - startPosition + 1);
        }

        private void richTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            var selectedWord = richTextBox.SelectedRtf;
            var selectionStart = richTextBox.GetCharIndexFromPosition(e.Location);
            var selectionLength = richTextBox.SelectionLength;
            int startPosition;
            int endPosition;
            string selectedRtf;

            selectedWord = GetWordUnderCaret(selectionStart, out startPosition, out endPosition);

            if (!selectedWord.IsNullOrEmpty())
            {
                if (selectedWord.IsValidEmailAddress() || selectedWord.IsValidUrl())
                {
                    selectedRtf = richTextBox.SelectedRtf;

                    if (OnLinkSelected != null)
                    {
                        OnLinkSelected(this, new OnLinkSelectedEventArgs(selectedWord, selectedRtf, selectedWord));
                    }

                    return;
                }
            }
        }

        private void richTextBox_Leave(object sender, EventArgs e)
        {
            DocumentLeave?.Invoke(sender, e);
        }

        #region Imports and structs

        // It makes no difference if we use PARAFORMAT or
        // PARAFORMAT2 here, so I have opted for PARAFORMAT2.
        [StructLayout(LayoutKind.Sequential)]
        public struct PARAFORMAT
        {
            public int cbSize;
            public uint dwMask;
            public short wNumbering;
            public short wReserved;
            public int dxStartIndent;
            public int dxRightIndent;
            public int dxOffset;
            public short wAlignment;
            public short cTabCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public int[] rgxTabs;

            // PARAFORMAT2 from here onwards.
            public int dySpaceBefore;
            public int dySpaceAfter;
            public int dyLineSpacing;
            public short sStyle;
            public byte bLineSpacingRule;
            public byte bOutlineLevel;
            public short wShadingWeight;
            public short wShadingStyle;
            public short wNumberingStart;
            public short wNumberingStyle;
            public short wNumberingTab;
            public short wBorderSpace;
            public short wBorderWidth;
            public short wBorders;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CHARFORMAT
        {
            public int cbSize;
            public UInt32 dwMask;
            public UInt32 dwEffects;
            public Int32 yHeight;
            public Int32 yOffset;
            public Int32 crTextColor;
            public byte bCharSet;
            public byte bPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szFaceName;

            // CHARFORMAT2 from here onwards.
            public short wWeight;
            public short sSpacing;
            public Int32 crBackColor;
            public uint lcid;
            public uint dwReserved;
            public short sStyle;
            public short wKerning;
            public byte bUnderlineType;
            public byte bAnimation;
            public byte bRevAuthor;
            public byte bReserved1;
        }

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(HandleRef hWnd,
            int msg,
            int wParam,
            int lParam);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(HandleRef hWnd,
            int msg,
            int wParam,
            ref PARAFORMAT lp);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int SendMessage(HandleRef hWnd,
            int msg,
            int wParam,
            ref CHARFORMAT lp);

        private const int EM_SETEVENTMASK = 1073;
        private const int WM_SETREDRAW = 11;

        [DllImport("User32.dll", CharSet = CharSet.Auto, PreserveSig = false)]
        public static extern IRichEditOle SendMessage(IntPtr hWnd, int message, int wParam);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern bool GetClientRect(IntPtr hWnd, [In, Out] ref Rectangle rect);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern bool GetWindowRect(IntPtr hWnd, [In, Out] ref Rectangle rect);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("ole32.dll")]
        static extern int OleSetContainedObject([MarshalAs(UnmanagedType.IUnknown)]
            object pUnk, bool fContained);

        [DllImport("ole32.dll")]
        static extern int OleLoadPicturePath(
            [MarshalAs(UnmanagedType.LPWStr)] string lpszPicturePath,
            [MarshalAs(UnmanagedType.IUnknown)][In] object pIUnknown,
            uint dwReserved,
            uint clrReserved,
            ref Guid riid,
            [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

        [DllImport("ole32.dll")]
        static extern int OleCreateFromFile([In] ref Guid rclsid,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszFileName, [In] ref Guid riid,
            uint renderopt, ref FORMATETC pFormatEtc, IOleClientSite pClientSite,
            IStorage pStg, [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

        [DllImport("ole32.dll")]
        static extern int OleCreateFromData(IDataObject pSrcDataObj,
            [In] ref Guid riid, uint renderopt, ref FORMATETC pFormatEtc,
            IOleClientSite pClientSite, IStorage pStg,
            [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

        [DllImport("ole32.dll")]
        static extern int OleCreateStaticFromData([MarshalAs(UnmanagedType.Interface)] IDataObject pSrcDataObj,
            [In] ref Guid riid, uint renderopt, ref FORMATETC pFormatEtc,
            IOleClientSite pClientSite, IStorage pStg,
            [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

        [DllImport("ole32.dll")]
        static extern int OleCreateLinkFromData([MarshalAs(UnmanagedType.Interface)] IDataObject pSrcDataObj,
            [In] ref Guid riid, uint renderopt, ref FORMATETC pFormatEtc,
            IOleClientSite pClientSite, IStorage pStg,
            [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

        #endregion

        public void InsertOleObject(IOleObject oleObj)
        {
            RichEditOle ole = new RichEditOle(richTextBox);
            ole.InsertOleObject(oleObj);
        }

        public void InsertControl(Control control)
        {
            RichEditOle ole = new RichEditOle(richTextBox);
            ole.InsertControl(control);
        }

        public REOBJECT InsertDataObject(DataObject mdo, uint uniqueId)
        {
            RichEditOle ole = new RichEditOle(richTextBox);
            return ole.InsertDataObject(mdo, uniqueId);
        }

        public void UpdateObjects()
        {
            RichEditOle ole = new RichEditOle(richTextBox);
            ole.UpdateObjects();
        }

        public REOBJECT InsertImage(Image image, uint uniqueId)
        {
            DataObject mdo = new DataObject();

            mdo.SetImage(image);

            return this.InsertDataObject(mdo, uniqueId);
        }

        public void InsertImage(string imageFile, uint uniqueId)
        {
            DataObject mdo = new DataObject();

            mdo.SetImage(imageFile);

            this.InsertDataObject(mdo, uniqueId);
        }

        public void InsertImageFromFile(string strFilename)
        {
            RichEditOle ole = new RichEditOle(richTextBox);
            ole.InsertImageFromFile(strFilename);
        }

        public void InsertActiveX(string strProgID)
        {
            Type t = Type.GetTypeFromProgID(strProgID);
            if (t == null)
                return;

            object o = System.Activator.CreateInstance(t);

            bool b = (o is IOleObject);

            if (b)
                this.InsertOleObject((IOleObject)o);
        }

        // RichEditOle wrapper and helper
        public class RichEditOle
        {
            public const int WM_USER = 0x0400;
            public const int EM_GETOLEINTERFACE = WM_USER + 60;

            private RichTextBox _richEdit;
            private IRichEditOle _RichEditOle;

            public RichEditOle(RichTextBox richEdit)
            {
                this._richEdit = richEdit;
            }

            public IRichEditOle IRichEditOle
            {
                get
                {
                    if (this._RichEditOle == null)
                    {
                        this._RichEditOle = SendMessage(this._richEdit.Handle, EM_GETOLEINTERFACE, 0);
                    }

                    return this._RichEditOle;
                }
            }


            [DllImport("ole32.dll", EntryPoint = "CreateILockBytesOnHGlobal", ExactSpelling = true, PreserveSig = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            static extern int CreateILockBytesOnHGlobal(IntPtr /* HGLOBAL */ hGlobal, bool fDeleteOnRelease, [MarshalAs(UnmanagedType.Interface)] out object /* ILockBytes** */ ppLkbyt);
            [DllImport("ole32.dll")]
            static extern int StgCreateDocfileOnILockBytes(ILockBytes plkbyt, uint grfMode, uint reserved, out IStorage ppstgOpen);

            public void InsertControl(Control control)
            {
                if (control == null)
                    return;

                Guid guid = Marshal.GenerateGuidForType(control.GetType());

                //-----------------------
                object pLockBytes;
                CreateILockBytesOnHGlobal(IntPtr.Zero, true, out pLockBytes);

                IStorage pStorage;
                StgCreateDocfileOnILockBytes((ILockBytes) pLockBytes, (uint)(STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE | STGM.STGM_READWRITE), 0, out pStorage);

                IOleClientSite pOleClientSite;
                this.IRichEditOle.GetClientSite(out pOleClientSite);
                //-----------------------

                //-----------------------
                REOBJECT reoObject = new REOBJECT();

                reoObject.cp = this._richEdit.TextLength;

                reoObject.clsid = guid;
                reoObject.pstg = pStorage;
                reoObject.poleobj = Marshal.GetIUnknownForObject(control);
                reoObject.polesite = pOleClientSite;
                reoObject.dvAspect = (uint)(DVASPECT.DVASPECT_CONTENT);
                reoObject.dwFlags = (uint)(REOOBJECTFLAGS.REO_BELOWBASELINE);
                reoObject.dwUser = 1;

                this.IRichEditOle.InsertObject(reoObject);
                //-----------------------

                //-----------------------
                Marshal.ReleaseComObject(pLockBytes);
                Marshal.ReleaseComObject(pOleClientSite);
                Marshal.ReleaseComObject(pStorage);
                //-----------------------
            }

            public bool InsertImageFromFile(string strFilename)
            {
                //-----------------------
                object pLockBytes;
                CreateILockBytesOnHGlobal(IntPtr.Zero, true, out pLockBytes);

                IStorage pStorage;
                StgCreateDocfileOnILockBytes((ILockBytes) pLockBytes, (uint)(STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE | STGM.STGM_READWRITE), 0, out pStorage);

                IOleClientSite pOleClientSite;
                this.IRichEditOle.GetClientSite(out pOleClientSite);
                //-----------------------


                //-----------------------
                FORMATETC formatEtc = new FORMATETC();

                formatEtc.cfFormat = 0;
                formatEtc.ptd = IntPtr.Zero;
                formatEtc.dwAspect = DVASPECT.DVASPECT_CONTENT;
                formatEtc.lindex = -1;
                formatEtc.tymed = TYMED.TYMED_NULL;

                Guid IID_IOleObject = new Guid("{00000112-0000-0000-C000-000000000046}");
                Guid CLSID_NULL = new Guid("{00000000-0000-0000-0000-000000000000}");

                object pOleObjectOut;

                // I don't sure, but it appears that this function only loads from bitmap
                // You can also try OleCreateFromData, OleLoadPictureIndirect, etc.
                int hr = OleCreateFromFile(ref CLSID_NULL, strFilename, ref IID_IOleObject, (uint)OLERENDER.OLERENDER_DRAW, ref formatEtc, pOleClientSite, pStorage, out pOleObjectOut);

                if (pOleObjectOut == null)
                {
                    Marshal.ReleaseComObject(pLockBytes);
                    Marshal.ReleaseComObject(pOleClientSite);
                    Marshal.ReleaseComObject(pStorage);

                    return false;
                }

                IOleObject pOleObject = (IOleObject)pOleObjectOut;
                //-----------------------


                //-----------------------
                Guid guid = new Guid();

                //guid = Marshal.GenerateGuidForType(pOleObject.GetType());
                pOleObject.GetUserClassID(ref guid);
                //-----------------------

                //-----------------------
                OleSetContainedObject(pOleObject, true);

                REOBJECT reoObject = new REOBJECT();

                reoObject.cp = this._richEdit.TextLength;

                reoObject.clsid = guid;
                reoObject.pstg = pStorage;
                reoObject.poleobj = Marshal.GetIUnknownForObject(pOleObject);
                reoObject.polesite = pOleClientSite;
                reoObject.dvAspect = (uint)(DVASPECT.DVASPECT_CONTENT);
                reoObject.dwFlags = (uint)(REOOBJECTFLAGS.REO_BELOWBASELINE);
                reoObject.dwUser = 0;

                this.IRichEditOle.InsertObject(reoObject);
                //-----------------------

                //-----------------------
                Marshal.ReleaseComObject(pLockBytes);
                Marshal.ReleaseComObject(pOleClientSite);
                Marshal.ReleaseComObject(pStorage);
                Marshal.ReleaseComObject(pOleObject);
                //-----------------------

                return true;
            }

            public REOBJECT InsertDataObject(DataObject mdo, uint uniqueId)
            {
                if (mdo == null)
                    return null;

                //-----------------------
                object pLockBytes;
                int sc = CreateILockBytesOnHGlobal(IntPtr.Zero, true, out pLockBytes);

                IStorage pStorage;
                sc = StgCreateDocfileOnILockBytes((ILockBytes) pLockBytes, (uint)(STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE | STGM.STGM_READWRITE), 0, out pStorage);

                IOleClientSite pOleClientSite;
                this.IRichEditOle.GetClientSite(out pOleClientSite);
                //-----------------------

                Guid guid = Marshal.GenerateGuidForType(mdo.GetType());

                Guid IID_IOleObject = new Guid("{00000112-0000-0000-C000-000000000046}");
                Guid IID_IDataObject = new Guid("{0000010e-0000-0000-C000-000000000046}");
                Guid IID_IUnknown = new Guid("{00000000-0000-0000-C000-000000000046}");

                object pOleObject;

                int hr = OleCreateStaticFromData(mdo, ref IID_IOleObject, (uint)OLERENDER.OLERENDER_FORMAT, ref mdo.mpFormatetc, pOleClientSite, pStorage, out pOleObject);

                if (pOleObject == null)
                    return null;
                //-----------------------


                //-----------------------
                OleSetContainedObject(pOleObject, true);

                REOBJECT reoObject = new REOBJECT();

                reoObject.cp = -1;

                reoObject.clsid = guid;
                reoObject.pstg = pStorage;
                reoObject.poleobj = Marshal.GetIUnknownForObject(pOleObject);
                reoObject.polesite = pOleClientSite;
                reoObject.dvAspect = (uint)(DVASPECT.DVASPECT_CONTENT);
                reoObject.dwFlags = (uint)(REOOBJECTFLAGS.REO_BELOWBASELINE);
                reoObject.dwUser = uniqueId;

                this.IRichEditOle.InsertObject(reoObject);
                //-----------------------

                //-----------------------
                Marshal.ReleaseComObject(pLockBytes);
                Marshal.ReleaseComObject(pOleClientSite);
                Marshal.ReleaseComObject(pStorage);
                Marshal.ReleaseComObject(pOleObject);
                //-----------------------

                return reoObject;
            }

            public void InsertOleObject(IOleObject oleObject)
            {
                if (oleObject == null)
                    return;

                //-----------------------
                object pLockBytes;
                CreateILockBytesOnHGlobal(IntPtr.Zero, true, out pLockBytes);

                IStorage pStorage;
                StgCreateDocfileOnILockBytes((ILockBytes) pLockBytes, (uint)(STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE | STGM.STGM_READWRITE), 0, out pStorage);

                IOleClientSite pOleClientSite;
                this.IRichEditOle.GetClientSite(out pOleClientSite);
                //-----------------------

                //-----------------------
                Guid guid = new Guid();

                oleObject.GetUserClassID(ref guid);
                //-----------------------

                //-----------------------
                OleSetContainedObject(oleObject, true);

                REOBJECT reoObject = new REOBJECT();

                reoObject.cp = this._richEdit.TextLength;

                reoObject.clsid = guid;
                reoObject.pstg = pStorage;
                reoObject.poleobj = Marshal.GetIUnknownForObject(oleObject);
                reoObject.polesite = pOleClientSite;
                reoObject.dvAspect = (uint)DVASPECT.DVASPECT_CONTENT;
                reoObject.dwFlags = (uint)REOOBJECTFLAGS.REO_BELOWBASELINE;

                this.IRichEditOle.InsertObject(reoObject);
                //-----------------------

                //-----------------------
                Marshal.ReleaseComObject(pLockBytes);
                Marshal.ReleaseComObject(pOleClientSite);
                Marshal.ReleaseComObject(pStorage);
                //-----------------------
            }

            public void UpdateObjects()
            {
                int k = this.IRichEditOle.GetObjectCount();

                for (int i = 0; i < k; i++)
                {
                    REOBJECT reoObject = new REOBJECT();

                    this.IRichEditOle.GetObject(i, reoObject, GETOBJECTOPTIONS.REO_GETOBJ_ALL_INTERFACES);

                    if (reoObject.dwUser == 1)
                    {
                        Point pt = this._richEdit.GetPositionFromCharIndex(reoObject.cp);
                        Rectangle rect = new Rectangle(pt, reoObject.sizel);

                        this._richEdit.Invalidate(rect, false); // repaint
                    }
                }
            }
        }

        private void btnEmoticons_DropDownOpening(object sender, EventArgs e)
        {
            InsertEmoji();
        }

        private void btnEmoticons_ButtonClick(object sender, EventArgs e)
        {
            InsertEmoji();
        }

        private void InsertEmoji()
        {
            var rect = this.RectangleToScreen(richTextBox.Bounds);
            var frmEmojisPalette = new frmEmojisPalette();

            frmEmojisPalette.OnSelected += (s, e2) =>
            {
                var emoji = e2.Value;
                var image = emoji.Image.ResizeImage(16, 16);
                var pixel = image.GetPixel(0, 0);
                var colorRef = pixel.ToCOLORREF();
                REOBJECT reObject;

                // make one pixel unique

                colorRef = colorRef - embeddedEmojis.Count;
                pixel = DrawingExtensions.FromCOLORREF((uint) colorRef);
                image.SetPixel(0, 0, pixel);

                reObject = this.InsertImage(image, (uint) colorRef + 4276);

                richTextBox.SelectionStart -= 1;
                richTextBox.SelectionLength = 1;

                var selectedRtf = richTextBox.SelectedRtf;
                string pattern;

                pattern = @"(?s)\{(?<image>\\pict\{.*?\r\n.*?\})";

                if (Regex.IsMatch(selectedRtf, pattern))
                {
                    var match = Regex.Match(selectedRtf, pattern);
                    var identifier = match.GetGroupValue("image");
                    var embeddedEmoji = new EmbeddedEmoji(reObject, emoji);

                    embeddedEmojis.Add(identifier, embeddedEmoji);
                }
            };

            frmEmojisPalette.Load += (s, e2) =>
            {
                frmEmojisPalette.Top = rect.Top;
                frmEmojisPalette.Left = rect.Left;
            };

            frmEmojisPalette.Show();
        }
    }

    public class LinkData
    {
        public int StartPosition { get; set; }
        public int EndPosition { get; set; }
        public string Word { get; set; }

        public LinkData(string word, int startPosition, int endPosition)
        {
            this.StartPosition = startPosition;
            this.EndPosition = endPosition;
            this.Word = word;
        }

        public bool Intersects(LinkData linkData)
        {
            if (linkData.StartPosition.IsBetween(this.StartPosition, this.EndPosition))
            {
                return true;
            }
            else if (linkData.EndPosition.IsBetween(this.StartPosition, this.EndPosition))
            {
                return true;
            }
            else if (this.StartPosition.IsBetween(linkData.StartPosition, linkData.EndPosition))
            {
                return true;
            }
            else if (this.EndPosition.IsBetween(linkData.StartPosition, linkData.EndPosition))
            {
                return true;
            }

            return false;
        }
    }

    public class OnImageSelectedEventArgs : EventArgs
    {
        public string Identifier { get; }
        public string Rtf { get; }

        public OnImageSelectedEventArgs(string identifier, string rtf)
        {
            this.Identifier = identifier;
            this.Rtf = rtf;
        }
    }

    public class OnTextSelectedEventArgs : EventArgs
    {
        public string Rtf { get; }
        public int SelectionStart { get; }
        public int SelectionLength { get; }

        public OnTextSelectedEventArgs(string rtf, int selectionStart, int selectionLength)
        {
            this.Rtf = rtf;
            this.SelectionStart = selectionStart;
            this.SelectionLength = selectionLength;
        }
    }

    public class OnLinkSelectedEventArgs : EventArgs
    {
        public string Identifier { get; }
        public string Rtf { get; }
        public string Link { get; }

        public OnLinkSelectedEventArgs(string identifier, string rtf, string link)
        {
            this.Identifier = identifier;
            this.Rtf = rtf;
            this.Link = link;
        }
    }

    public class OnLinkChangedEventArgs : EventArgs
    {
        public string NewLink { get; }
        public string OldLink { get; }
        public bool DeleteOld { get; set; }

        public OnLinkChangedEventArgs(string newLink, string oldLink)
        {
            this.NewLink = newLink;
            this.OldLink = oldLink;
        }
    }

    public delegate void OnImageSelectedHandler(object obj, OnImageSelectedEventArgs e);
    public delegate void OnLinkSelectedHandler(object obj, OnLinkSelectedEventArgs e);
    public delegate void OnLinkChangedHandler(object obj, OnLinkChangedEventArgs e);
    public delegate void OnTextSelectedHandler(object obj, OnTextSelectedEventArgs e);
}
