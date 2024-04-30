using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Linq;
#if USE_API_SERVICES
using ApiServices.Library;
using ApiServices.Services;
#endif
using Microsoft.VisualBasic; // Install-Package Microsoft.VisualBasic
using Microsoft.VisualBasic.CompilerServices; // Install-Package Microsoft.VisualBasic
using Microsoft.Win32;
using NHunspell;
using Utils;
using System.Windows.Forms;
using NativeWindow = System.Windows.Forms.NativeWindow;
using Timer = System.Windows.Forms.Timer;
using Hunspell = NHunspell.Hunspell;

[ToolboxBitmap(typeof(NHunspellTextBoxExtender), "spellcheck.png")]
[ProvideProperty("SpellCheckEnabled", typeof(Control))]
public partial class NHunspellTextBoxExtender : Component, IExtenderProvider, ISupportInitialize
{

    public static event GetSentencesEventHandler GetSentencesEvent;
    internal static List<CustomThreadPool> customThreadPoolList;
    internal CustomThreadPool customThreadPool;
    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NHunspellTextBoxExtender));


    // Private Const MessageLogPath As String = "D:\Messagelog6.txt"

    #region Private Classes


    /// <summary>
    /// This is the class that handles painting the wavy red lines.
    /// 
    /// It utilizes the NativeWindow to find out when it needs to draw
    /// </summary>
    /// <remarks></remarks>
    private partial class CustomPaintTextBox : NativeWindow
    {

        private TextBoxBase parentTextBox;
        private Bitmap myBitmap;
        private Graphics textBoxGraphics;
        private Graphics bufferGraphics;
        private SpellCheckControl mySpellCheckControl;
        private NHunspellTextBoxExtender myParent;

        public event CustomPaintCompleteEventHandler CustomPaintComplete;

        public delegate void CustomPaintCompleteEventHandler(TextBoxBase sender, long Milliseconds);

        /// <summary>
        /// This is called when the textbox is being redrawn.
        /// When it is, for the textbox to get refreshed, call it's default
        /// paint method and then call our method
        /// </summary>
        /// <param name="m">The windows message</param>
        /// <remarks></remarks>
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case 15: // This is the WM_PAINT message
                    {
                        // Invalidate the textBoxBase so that it gets refreshed properly
                        parentTextBox.Invalidate();

                        // call the default win32 Paint method for the TextBoxBase first
                        base.WndProc(ref m);

                        // now use our code to draw the extra stuff
                        CustomPaint();
                        break;
                    }

                default:
                    {
                        base.WndProc(ref m);
                        break;
                    }
            }
        }

        public CustomPaintTextBox(ref TextBoxBase CallingTextBox, ref SpellCheckControl ThisSpellCheckControl, ref NHunspellTextBoxExtender Parent)
        {
            // Set up the CustomPaintTextBox
            parentTextBox = CallingTextBox;

            // Create the link to the parent
            myParent = Parent;

            // Create a bitmap with the same dimensions as the textbox
            myBitmap = new Bitmap(parentTextBox.Width, parentTextBox.Height);

            // Create the graphics object from this bitmpa...this is where we will draw the lines to start with
            bufferGraphics = Graphics.FromImage(myBitmap);
            bufferGraphics.Clip = new Region(parentTextBox.ClientRectangle);

            // Get the graphics object for the textbox.  We use this to draw the bufferGraphics
            textBoxGraphics = Graphics.FromHwnd(parentTextBox.Handle);

            // Assign a handle for this class and set it to the handle for the textbox
            this.AssignHandle(parentTextBox.Handle);

            // We also need to make sure we update the handle if the handle for the textbox changes
            // This occurs if wordWrap is turned off for a RichTextBox
            this.parentTextBox.HandleCreated += TextBoxBase_HandleCreated;

            // We need to add a handler to change the clip rectangle if the textBox is resized
            this.parentTextBox.ClientSizeChanged += TextBoxBase_ClientSizeChanged;

            // Now set our spellchecker
            mySpellCheckControl = ThisSpellCheckControl;
        }

        /// <summary>
        /// Gets the ranges of chars that represent the spelling errors and then draw a wavy red line underneath
        /// them.
        /// </summary>
        /// <remarks></remarks>
        private void CustomPaint() // ByVal sender As Object, ByVal e As DoWorkEventArgs)
        {
            // Determine if we need to draw anything

            // Benchmarking
            var startTime = DateTime.Now;
            bool draw = false;
            var lockObject = mySpellCheckControl.LockObject;

            using (lockObject.Lock())
            {

                // Clear the graphics buffer
                bufferGraphics.Clear(Color.Transparent);

                if (mySpellCheckControl.HasSpellingErrors())
                {

                    if (!myParent.IsEnabled(ref parentTextBox))
                        return;
                    if (!myParent.SpellAsYouType)
                        return;

                    draw = true;

                    RichTextBox tempRTB = default;

                    if (parentTextBox is RichTextBox)
                    {
                        tempRTB = new RichTextBox();
                        tempRTB.Rtf = ((RichTextBox)parentTextBox).Rtf;
                    }

                    // Now, find out if any of the spelling errors are within the visible part of the textbox
                    CharacterRange[] CharRanges = mySpellCheckControl.GetSpellingErrorRanges();
                    CharacterRange[] visibleRanges;
                    visibleRanges = new CharacterRange[0];

                    // First get the ranges of characters visible in the textbox
                    var startPoint = new Point(0, 0);
                    var endPoint = new Point(parentTextBox.ClientRectangle.Width, parentTextBox.ClientRectangle.Height);
                    long startIndex = parentTextBox.GetCharIndexFromPosition(startPoint);
                    long endIndex = parentTextBox.GetCharIndexFromPosition(endPoint);

                    // Benchmarking
                    // Dim visibleStartTime As DateTime = Now

                    // Go through each of the charRanges that were returned and see if they're visible
                    for (int i = 0, loopTo = Information.UBound(CharRanges); i <= loopTo; i++)
                    {
                        int rangeStart = -1;

                        // See if it's an ignore range
                        int rangeEnd = -1;
                        bool ignoreRange = false;

                        foreach (CharacterRange currentIgnoreRange in mySpellCheckControl.GetIgnoreRanges())
                        {
                            if (currentIgnoreRange.First == CharRanges[i].First & currentIgnoreRange.Length == CharRanges[i].Length)
                            {
                                ignoreRange = true;
                            }
                        }

                        // If it's not a range we're to ignore, then see if it's visible
                        if (!ignoreRange)
                        {
                            for (int x = 0; x < Information.UBound(CharRanges); i++)
                            {
                                {
                                    ref var withBlock = ref parentTextBox;
                                    if (Conversions.ToBoolean(Operators.AndObject(Operators.ConditionalCompareObjectGreaterEqual(x, startIndex, false), Operators.ConditionalCompareObjectLessEqual(x, endIndex, false))))
                                    {
                                        if (rangeStart == -1)
                                        {
                                            rangeStart = (int) x;
                                        }
                                        else
                                        {
                                            rangeEnd = (int) x;
                                        }
                                    }
                                }
                            }

                            // Now add a new visibleRange to the array
                            if (rangeStart != -1 & rangeEnd != -1)
                            {
                                var newRange = new CharacterRange(rangeStart, rangeEnd - rangeStart + 1);
                                Array.Resize(ref visibleRanges, Information.UBound(visibleRanges) + 1 + 1);
                                visibleRanges[Information.UBound(visibleRanges)] = newRange;
                            }
                        }

                        // Dim visibleDiff As TimeSpan = Now.Subtract(visibleStartTime)
                        // Debug.Print("VisibleRanges: " & visibleDiff.TotalMilliseconds & " Milliseconds")

                        // Now that we have the ranges that are visible, we're going to create the end points
                        // to call the drawWave
                    }


                    // Benchmarking
                    // Dim drawStartTime As DateTime = Now

                    foreach (var currentRange in visibleRanges)
                    {
                        // Get the X, Y of the start and end characters
                        startPoint = parentTextBox.GetPositionFromCharIndex(currentRange.First);
                        endPoint = parentTextBox.GetPositionFromCharIndex(currentRange.First + currentRange.Length - 1);

                        if (startPoint.Y != endPoint.Y)
                        {
                            // We have a word on multiple lines
                            int curIndex, startingIndex;
                            curIndex = currentRange.First;
                            startingIndex = curIndex;

                        GetNextLine:
                            ;

                            // Determine the first line of waves to draw
                            while (parentTextBox.GetPositionFromCharIndex(curIndex).Y == startPoint.Y & curIndex <= currentRange.First + currentRange.Length - 1)
                                curIndex += 1;

                            // Go back to the previous character
                            curIndex -= 1;

                            endPoint = parentTextBox.GetPositionFromCharIndex(curIndex);
                            var offsets = GetOffsets(ref parentTextBox, startingIndex, curIndex, tempRTB);

                            // Dim offsetsDiff As TimeSpan = Now.Subtract(startTime)
                            // Debug.Print("Get Offsets: " & offsetsDiff.TotalMilliseconds & " Milliseconds")

                            // If we're using a RichTextBox, we have to account for the zoom factor
                            if (parentTextBox is RichTextBox)
                            {
                                offsets.Y *= (int)Math.Round(offsets.Y * ((RichTextBox)parentTextBox).ZoomFactor);
                            }

                            // Reset the starting and ending points to make sure we're underneath the word
                            // (The measurestring adds some margin, so remove them)
                            startPoint.Y += offsets.Y - 2;
                            endPoint.Y += offsets.Y - 2;
                            endPoint.X += offsets.X - 0;

                            // Add a new wavy line using the starting and ending point

                            Pen newPen = Pens.Red;
                            DrawWave(newPen, startPoint, endPoint);

                            // Dim drawWaveDiff As TimeSpan = Now.Subtract(startTime)
                            // Debug.Print("DrawWave: " & drawWaveDiff.TotalMilliseconds & " Milliseconds")

                            startingIndex = curIndex + 1;
                            curIndex += 1;
                            startPoint = parentTextBox.GetPositionFromCharIndex(curIndex);

                            if (curIndex <= currentRange.First + currentRange.Length - 1)
                            {
                                goto GetNextLine;
                            }
                        }
                        else
                        {
                            var offsets = GetOffsets(ref parentTextBox, currentRange.First, currentRange.First + currentRange.Length - 1, tempRTB);

                            // Dim offsetsDiff As TimeSpan = Now.Subtract(startTime)
                            // Debug.Print("Get Offsets: " & offsetsDiff.TotalMilliseconds & " Milliseconds")

                            // If we're using a RichTextBox, we have to account for the zoom factor
                            if (parentTextBox is RichTextBox)
                            {
                                offsets.Y *= (int)Math.Round(offsets.Y * ((RichTextBox)parentTextBox).ZoomFactor);
                            }

                            // Reset the starting and ending points to make sure we're underneath the word
                            // (The measurestring adds some margin, so remove them)
                            startPoint.Y += offsets.Y - 2;
                            endPoint.Y += offsets.Y - 2;
                            endPoint.X += offsets.X - 4;

                            // Add a new wavy line using the starting and ending point
                            // If e.Cancel Then Return
                            Pen newPen = Pens.Red;
                            DrawWave(newPen, startPoint, endPoint);

                            // Dim drawWaveDiff As TimeSpan = Now.Subtract(startTime)
                            // Debug.Print("DrawWave: " & drawWaveDiff.TotalMilliseconds & " Milliseconds")
                        }
                    }
                }

#if USE_API_SERVICES
                if (mySpellCheckControl.GrammarEdits.Where((GrammarEdit g) => !g.HandledOrIgnored).Count() > 0)
                {

                    RichTextBox tempRTB = default;
                    var startPoint = new Point(0, 0);
                    var endPoint = new Point(parentTextBox.ClientRectangle.Width, parentTextBox.ClientRectangle.Height);
                    long startIndex = parentTextBox.GetCharIndexFromPosition(startPoint);
                    long endIndex = parentTextBox.GetCharIndexFromPosition(endPoint);

                    draw = true;

                    if (parentTextBox is RichTextBox)
                    {
                        tempRTB = new RichTextBox();
                        tempRTB.Rtf = ((RichTextBox)parentTextBox).Rtf;
                    }

                    foreach (var grammarEdit in mySpellCheckControl.GrammarEdits.Where((GrammarEdit g) => !g.HandledOrIgnored))
                    {

                        startPoint = parentTextBox.GetPositionFromCharIndex(((dynamic)grammarEdit).StartGrammar);
                        endPoint = parentTextBox.GetPositionFromCharIndex(((dynamic)grammarEdit).EndGrammar);

                        var offsets = GetOffsets(ref parentTextBox, ((dynamic)grammarEdit).StartGrammar, ((dynamic)grammarEdit).EndGrammar, tempRTB);

                        // If startPoint.Y <> endPoint.Y Then
                        // startPoint.Y = endPoint.Y
                        // End If

                        if (startPoint.X == endPoint.X)
                        {
                            startPoint.X -= 10;
                        }

                        startPoint.Y += offsets.Y - 2;
                        endPoint.Y += offsets.Y - 2;
                        endPoint.X += offsets.X - 4;

                        Pen newPen = Pens.Green;
                        DrawWave(newPen, startPoint, endPoint);

                    }

                }
#endif
            }

            if (draw)
            {
                // Dim drawDiff As TimeSpan = Now.Subtract(drawStartTime)
                // Debug.Print("Draw: " & drawDiff.TotalMilliseconds & " Milliseconds")

                // We've drawn all of the wavy lines, so draw that image over the textbox
                textBoxGraphics.DrawImageUnscaled(myBitmap, 0, 0);

                // Dim dateDiff As TimeSpan = Now.Subtract(startTime)
                // Debug.Print("----TotalTime: " & dateDiff.Seconds & " Seconds, " & dateDiff.Milliseconds & " Milliseconds------------")

                CustomPaintComplete?.Invoke(parentTextBox, (long) DateTime.Now.Subtract(startTime).TotalMilliseconds);
            }

        }

        /// <summary>
        /// Determines the X and Y offsets to use based on font height last letter width
        /// </summary>
        /// <param name="curTextBox"></param>
        /// <param name="startingIndex"></param>
        /// <param name="endingIndex"></param>
        /// <param name="tempRTB"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private Point GetOffsets(ref TextBoxBase curTextBox, int startingIndex, int endingIndex, RichTextBox tempRTB = default)
        {
            var startTime = DateTime.Now;

            // We now have the top left point of the characters, now we need to add the offsets
            int offsetY = 0;
            Font fontToUse = curTextBox.Font;
            var offsets = default(Point);

            fontToUse = new Font(fontToUse.FontFamily, (int) 0.1, fontToUse.Style, fontToUse.Unit, fontToUse.GdiCharSet, fontToUse.GdiVerticalFont);

            // If it's a RichTextBox, we have to do some extra things
            if (curTextBox is RichTextBox)
            {
                // We need to go through every character where we will draw the lines and get the tallest
                // font height

                // Benchmarking
                // Dim beforeCreateTextBoxDiff As TimeSpan = Now.Subtract(startTime)
                // Debug.Print("    Before Create TextBox: " & beforeCreateTextBoxDiff.TotalMilliseconds & " Milliseconds")

                // Create a temporary textbox for getting the RTF info so that we don't have to select and
                // de-select a lot of text and cause the screen to have to refresh
                if (tempRTB is null)
                {
                    tempRTB = new RichTextBox();
                    tempRTB.Rtf = ((RichTextBox)curTextBox).Rtf;
                }

                // Benchmarking
                // Dim createTextBoxDiff As TimeSpan = Now.Subtract(startTime)
                // Debug.Print("    Create TextBox: " & createTextBoxDiff.TotalMilliseconds & " Milliseconds")

                if (tempRTB.Text.Length > 0)
                {
                    // Have to find the first visible character on that line
                    long firstCharInLine, lastCharInLine, curCharLine;
                    curCharLine = tempRTB.GetLineFromCharIndex(startingIndex);
                    firstCharInLine = tempRTB.GetFirstCharIndexFromLine((int) curCharLine);
                    lastCharInLine = tempRTB.GetFirstCharIndexFromLine((int)(curCharLine + 1L));

                    if (lastCharInLine == -1)
                        lastCharInLine = curTextBox.TextLength;

                    var getFontHeightStart = DateTime.Now;

                    // Now go through every character that is visible and get the biggest font height
                    // Use the tempRTB for this
                    for (long i = firstCharInLine + 1L, loopTo = lastCharInLine + 1L; i <= loopTo; i++)
                    {
                        tempRTB.SelectionStart = (int) i;
                        tempRTB.SelectionLength = 1;
                        if (tempRTB.SelectionFont.Height > fontToUse.Height)
                        {
                            // fontHeight = .SelectionFont.Height
                            fontToUse = tempRTB.SelectionFont;
                        }
                    }

                    // Benchmarking
                    // Dim foundHeightdiff As TimeSpan = Now.Subtract(startTime)
                    // Debug.Print("    Get Font Height: " & foundHeightdiff.TotalMilliseconds & " Milliseconds")

                    offsetY = fontToUse.Height;

                }
            }
            else
            {
                // If we get here, it's just a standard textbox and we can just use the font height
                fontToUse = curTextBox.Font;

                offsetY = curTextBox.Font.Height;
            }

            // Now find out how wide the last character is
            int offsetX = 0;
            var index = Math.Min(curTextBox.Text.Length - 1, startingIndex + (endingIndex - startingIndex));

            if (index > 0)
            {
                offsetX = (int) textBoxGraphics.MeasureString(curTextBox.Text[index].ToString(), fontToUse).Width;

                offsets = new Point(offsetX, offsetY);
            }

            // Benchmarking
            // Dim timeDiff As TimeSpan = Now.Subtract(startTime)
            // Debug.Print("GetOffsets: " & timeDiff.TotalMilliseconds & " Milliseconds")

            return offsets;
        }

        /// <summary>
        /// The textbox is not redrawn much, so this will force the textbox to call the custom paint function.
        /// Otherwise, text can be entered and no wavy red lines will appear
        /// </summary>
        /// <remarks></remarks>
        public void ForcePaint()
        {
            parentTextBox.Invalidate();
        }

        /// <summary>
        /// Draws the wavy red line given a starting point and an ending point
        /// </summary>
        /// <param name="StartOfLine">A Point representing the starting point</param>
        /// <param name="EndOfLine">A Point representing the ending point</param>
        /// <remarks></remarks>
        private void DrawWave(Pen newPen, Point StartOfLine, Point EndOfLine)
        {

            if (EndOfLine.X - StartOfLine.X > 4)
            {
                var pl = new ArrayList();
                for (int i = StartOfLine.X, loopTo = EndOfLine.X - 2; i <= loopTo; i += 4)
                {
                    pl.Add(new Point(i, StartOfLine.Y));
                    pl.Add(new Point(i + 2, StartOfLine.Y + 2));
                }

                Point[] p = (Point[])pl.ToArray(typeof(Point));
                bufferGraphics.DrawLines(newPen, p);
            }
            else
            {
                bufferGraphics.DrawLine(newPen, StartOfLine, EndOfLine);
            }
        }

        /// <summary>
        /// Reassign this classes handle and the graphics object anytime the textbox's handle is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void TextBoxBase_HandleCreated(object sender, EventArgs e)
        {
            this.AssignHandle(parentTextBox.Handle);
            textBoxGraphics = Graphics.FromHwnd(parentTextBox.Handle);
        }

        /// <summary>
        /// When the TextBoxBase is resized, this will reset the objects that are used to draw
        /// the wavy, red line.  Without this, anything outside of the original bounds will not
        /// be drawn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks></remarks>
        private void TextBoxBase_ClientSizeChanged(object sender, EventArgs e)
        {
            TextBoxBase tempTextBox = (TextBoxBase) sender;

            // Create a bitmap with the same dimensions as the textbox
            myBitmap = new Bitmap(tempTextBox.Width, tempTextBox.Height);

            // Create the graphics object from this bitmpa...this is where we will draw the lines to start with
            bufferGraphics = Graphics.FromImage(myBitmap);
            bufferGraphics.Clip = new Region(tempTextBox.ClientRectangle);

            // Get the graphics object for the textbox.  We use this to draw the bufferGraphics
            textBoxGraphics = Graphics.FromHwnd(tempTextBox.Handle);
        }
    }


    #endregion
    #region Variables
    private NHunspell.Hunspell myNHunspell = default;

    // Hashtables
    private Hashtable controlEnabled;
    private Hashtable mySpellCheckers;
    private Hashtable myCustomPaintingTextBoxes;
    private Hashtable myContextMenus;
    // Private testHash As Hashtable

    // Other
    private bool controlPressed = false;
    private CustomPaintTextBox drawTest;
    private Control[] myControls;
    private bool boolDisableAddWordPrompt = false;
    private bool initializing = false;

    // Property values
    private bool _SpellAsYouType;
    private Shortcut _shortcutKey;
    private int myNumOfSuggestions;
    private string _Language;

    // Declared functions
    [DllImport("user32.dll")]
    private static extern int GetScrollPos(IntPtr hWnd, int nBar);
    [DllImport("user32.dll")]
    private static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);
    [DllImport("user32.dll")]
    private static extern bool PostMessageA(IntPtr hwnd, int wMsg, int wParam, int lParam);
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

    // Scrollbar direction
    private const int SBS_HORZ = 0;
    private const int SBS_VERT = 1;

    // Windows Messages
    private const int WM_VSCROLL = 0x115;
    private const int WM_HSCROLL = 0x114;
    private const int SB_THUMBPOSITION = 4;
    private Timer _timer;

    internal virtual Timer timer
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _timer;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_timer != null)
            {
#if USE_API_SERVICES
                _timer.Tick -= timer_Tick;
#endif
            }

            _timer = value;
            if (_timer != null)
            {
#if USE_API_SERVICES
                _timer.Tick += timer_Tick;
#endif
            }
        }
    }
    private IContainer components;

    // Redrawing
    private const int WM_SETREDRAW = 0xB;
#endregion
    #region ISupportInitialize
    public void BeginInit()
    {
        initializing = true;
    }

    public void EndInit()
    {
        initializing = false;
    }
    #endregion
    #region New and CanExtend Methods

    /// <summary>
    /// Determines which items this extender can extend.  It is only objects that implement TextBoxBase
    /// </summary>
    /// <param name="extendee">The control being checked</param>
    /// <returns>A boolean value indicating whether it can be extended</returns>
    /// <remarks></remarks>
    public bool CanExtend(object extendee)
    {
        return extendee is TextBoxBase & myNHunspell is not null;
    }

    /// <summary>
    /// We need to make sure that the dic and aff files are on the disk.  Then, we try to create
    /// the Hunspell object.  After that, we set up the hashtables and tooltip
    /// </summary>
    /// <remarks></remarks>
    public NHunspellTextBoxExtender()
    {
        // Biggest problem is the requirement to have two dictionary files on the HDD along with
        // either the x64 or x86 Hunspell DLL which are not .NET dlls.
        // To get around this, we find the directory that the program is being called from and add
        // the dictionary files.
        // Then, we try to create the Hunspell and if a "DLL not found" error is thrown, we find out
        // where the dll's were supposed to be and then add them.

        LanguageChanged += MyLanguageChanged;
        MaintainUserChoice = true;

        string USdic, USaff;

        // Get the calling assembly's location
        string callingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        // Try and write to the directory to see if we can
        bool boolFailed = false;

        try
        {
            Directory.CreateDirectory(callingDir + @"\Test");
        }
        catch (Exception ex)
        {
            boolFailed = true;
        }
        finally
        {
            Directory.Delete(callingDir + @"\Test");
        }

        if (boolFailed)
        {
            callingDir = @"C:\Windows\Temp";
        }

        // First see if there is a registry value that tells us where to get the dictionary from.
        var regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\NHunspellTextBoxExtender");

        if (regKey is null)
        {
            regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("NHunspellTextBoxExtender");
            var regKeyLanguage = regKey.CreateSubKey("Languages");
            regKeyLanguage.SetValue("Default", "English");

            // Set the paths for the dic and aff files
            USdic = callingDir + @"\SpellCheck\en_US.dic";
            USaff = callingDir + @"\SpellCheck\en_US.aff";

            // Check if the spell check directory already exists.  If not, add it
            if (!Directory.Exists(callingDir + @"\SpellCheck"))
            {
                Directory.CreateDirectory(callingDir + @"\SpellCheck");
                var newDirInfo = new DirectoryInfo(callingDir + @"\SpellCheck");
                newDirInfo.Attributes = FileAttributes.Hidden;
            }

            // Check if the spell check files already exist.  If not, add it
            if (!File.Exists(USaff))
            {
                try
                {
                    File.WriteAllBytes(USaff, (byte[]) resources.GetObject("en_US"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing en_US.aff file!" + Constants.vbNewLine + ex.Message);
                }
            }

            if (!File.Exists(USdic))
            {
                try
                {
                    File.WriteAllBytes(USdic, My.Resources.en_US_dic);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing en_US.dic file!" + Constants.vbNewLine + ex.Message);
                }
            }

            string[] paths = new string[] { USaff, USdic };
            regKeyLanguage.SetValue("English", paths, RegistryValueKind.MultiString);

            string[] languages = new string[] { "English" };
            regKeyLanguage.SetValue("LanguageList", languages, RegistryValueKind.MultiString);

            regKeyLanguage.Close();
            regKeyLanguage.Dispose();

            _Language = "English";
        }
        else
        {
            // Get the default language
            var regKeyLanguage = regKey.OpenSubKey("Languages", true);

            string defaultLanguage = Conversions.ToString(regKeyLanguage.GetValue("Default"));

            string[] paths;

            paths = regKeyLanguage.GetValue(defaultLanguage) as string[];

            _Language = defaultLanguage;

            if (regKeyLanguage.GetValue(defaultLanguage) is null)
            {
                // Check if English is there and use it...otherwise, check if another language is available
                paths = regKeyLanguage.GetValue("English") as string[];

                if (regKeyLanguage.GetValue("English") is null)
                {
                    // Set the paths for the dic and aff files
                    USdic = callingDir + @"\SpellCheck\en_US.dic";
                    USaff = callingDir + @"\SpellCheck\en_US.aff";

                    // Check if the spell check directory already exists.  If not, add it
                    if (!Directory.Exists(callingDir + @"\SpellCheck"))
                    {
                        Directory.CreateDirectory(callingDir + @"\SpellCheck");
                        var newDirInfo = new DirectoryInfo(callingDir + @"\SpellCheck");
                        newDirInfo.Attributes = FileAttributes.Hidden;
                    }

                    // Check if the spell check files already exist.  If not, add it
                    if (!File.Exists(USaff))
                    {
                        try
                        {
                            File.WriteAllBytes(USaff, global::My.Resources.en_US);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error writing en_US.aff file!" + Constants.vbNewLine + ex.Message);
                        }
                    }

                    if (!File.Exists(USdic))
                    {
                        try
                        {
                            File.WriteAllBytes(USdic, global::My.Resources.en_US_dic);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error writing en_US.dic file!" + Constants.vbNewLine + ex.Message);
                        }
                    }

                    paths = new[] { USaff, USdic };
                    _Language = "English";
                }
                else
                {
                    if (!File.Exists(paths[0]))
                    {
                        USaff = callingDir + @"\SpellCheck\en_US.aff";

                        try
                        {
                            File.WriteAllBytes(USaff, global::My.Resources.en_US);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error writing en_US.aff file!" + Constants.vbNewLine + ex.Message);
                        }

                        paths[0] = USaff;
                    }

                    if (!File.Exists(paths[1]))
                    {
                        USdic = callingDir + @"\SpellCheck\en_US.dic";

                        try
                        {
                            File.WriteAllBytes(USdic, global::My.Resources.en_US_dic);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error writing en_US.dic file!" + Constants.vbNewLine + ex.Message);
                        }

                        paths[1] = USdic;
                    }
                }

                _Language = "English";
            }
            USaff = paths[0];
            USdic = paths[1];

            // check if these files exist
            if (!File.Exists(USaff))
            {

                DirectoryInfo dir;

                USaff = callingDir + @"\SpellCheck\en_US.aff";

                dir = new DirectoryInfo(Path.GetDirectoryName(USaff));

                dir.CreateIfNotExists();

                try
                {
                    File.WriteAllBytes(USaff, global::My.Resources.en_US);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing en_US.aff file!" + Constants.vbNewLine + ex.Message);
                }

                paths[0] = USaff;

                regKeyLanguage.SetValue(_Language, paths, RegistryValueKind.MultiString);
            }
            if (!File.Exists(USdic))
            {

                DirectoryInfo dir;

                USdic = callingDir + @"\SpellCheck\en_US.dic";

                dir = new DirectoryInfo(Path.GetDirectoryName(USdic));

                dir.CreateIfNotExists();

                try
                {
                    File.WriteAllBytes(USdic, global::My.Resources.en_US_dic);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing en_US.dic file!" + Constants.vbNewLine + ex.Message);
                }

                paths[1] = USdic;

                regKeyLanguage.SetValue(_Language, paths, RegistryValueKind.MultiString);
            }

            regKeyLanguage.Flush();
            regKeyLanguage.Close();
            regKeyLanguage.Dispose();
        }

        regKey.Close();
        regKey.Dispose();

        InitializeComponent();

    // Create the new hunspell
    CreateNewHunspell:
        ;

        try
        {
            // Hunspell.NativeDllPath = "D:\Temp"

            myNHunspell = new Hunspell(USaff, USdic);
        }
        catch (Exception ex)
        {
            if (ex is DllNotFoundException)
            {
                // Get where the dll is supposed to be
                string DLLpath = Strings.Trim(Strings.Mid(ex.Message, Strings.InStr(ex.Message, "DLL not found:") + 14));
                string DLLName = Path.GetFileName(DLLpath);

                // Find out which DLL is missing
                if (DLLName == "Hunspellx64.dll")
                {
                    // Copy the dll to the directory
                    try
                    {
                        File.WriteAllBytes(DLLpath, global::My.Resources.Hunspellx64);
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show("Error writing Hunspellx64.dll" + Constants.vbNewLine + ex2.Message);
                    }

                    // Try again
                    goto CreateNewHunspell;
                }
                else if (DLLName == "Hunspellx86.dll") // x86 dll
                {
                    // Copy the dll to the directory
                    try
                    {
                        File.WriteAllBytes(DLLpath, global::My.Resources.Hunspellx86);
                    }
                    catch (Exception ex3)
                    {
                        MessageBox.Show("Error writing Hunspellx86.dll" + Constants.vbNewLine + ex3.Message);
                    }

                    // Try again
                    goto CreateNewHunspell;
                }
                else if (DLLName == "NHunspell.dll")
                {
                    try
                    {
                        File.WriteAllBytes(DLLpath, global::My.Resources.NHunspell);
                    }
                    catch (Exception ex4)
                    {
                        MessageBox.Show("Error writing NHunspell.dll" + Constants.vbNewLine + ex4.Message);
                    }
                }
                else
                {
                    MessageBox.Show(ex.Message + ex.StackTrace);
                }
            }
            else
            {
                MessageBox.Show("SpellChecker cannot be created." + Constants.vbNewLine + "Spell checking will be disabled." + Constants.vbNewLine + Constants.vbNewLine + ex.Message + ex.StackTrace);
            }
        }

        // myNHunspell = FromAssembly()

        // See if there are any words to add
        if (File.Exists(callingDir + @"\SpellCheck\" + _Language + "AddedWords.dat"))
        {
            using (var r = new StreamReader(callingDir + @"\SpellCheck\" + _Language + "AddedWords.dat"))
            {
                while (!r.EndOfStream)
                    myNHunspell.Add(Strings.Trim(Strings.Replace(r.ReadLine(), Constants.vbNewLine, "")));
                r.Close();
            }
        }

        // Set up Hashtables
        controlEnabled = new Hashtable();
        mySpellCheckers = new Hashtable();
        myCustomPaintingTextBoxes = new Hashtable();
        myContextMenus = new Hashtable();

        // Set the initial properties
        myNumOfSuggestions = 5;
        _SpellAsYouType = true;
        _shortcutKey = Shortcut.F7;
        myControls = new Control[0];
    }

    public static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
    {
        for (int i = 0, loopTo = AppDomain.CurrentDomain.GetAssemblies().Count() - 1; i <= loopTo; i++)
        {
            MessageBox.Show(args.Name + Constants.vbNewLine + AppDomain.CurrentDomain.GetAssemblies()[i].GetName().Name);

            if ((AppDomain.CurrentDomain.GetAssemblies()[i].GetName().Name ?? "") == (args.Name ?? ""))
            {
                return AppDomain.CurrentDomain.GetAssemblies()[i];
            }
        }

        return null;
    }

    #endregion
    #region Enable/Disable

    /// <summary>
    /// Allows this class the be enabled programatically
    /// </summary>
    /// <param name="TextBoxesToEnable">
    /// Allows the programmer to add as many TextBoxBases as they want at once.
    /// </param>
    /// <remarks>
    /// Examples:
    /// EnableTextBoxBase(TextBox1)
    /// EnableTextBoxBase(RichTextBox1, RichTextBox2, TextBox1)
    /// </remarks>
    public void EnableTextBoxBase(params TextBoxBase[] TextBoxesToEnable) // ByRef TextBoxToEnable As TextBoxBase)
    {
        for (int c = 0, loopTo = Information.UBound(TextBoxesToEnable); c <= loopTo; c++)
        {
            if (TextBoxesToEnable[c] is TextBoxBase)
            {

                var TextBoxToEnable = TextBoxesToEnable[c];

                // Set the hashtables
                if (this.controlEnabled[TextBoxToEnable] is null)
                {
                    controlEnabled.Add(TextBoxToEnable, true);
                    mySpellCheckers.Add(TextBoxToEnable, new SpellCheckControl(ref myNHunspell));
                    var argCallingTextBox = TextBoxToEnable;
                    SpellCheckControl argThisSpellCheckControl = (SpellCheckControl)this.mySpellCheckers[TextBoxToEnable];
                    var argParent = this;
                    myCustomPaintingTextBoxes.Add(TextBoxToEnable, new CustomPaintTextBox(ref argCallingTextBox, ref argThisSpellCheckControl, ref argParent));
                    ((CustomPaintTextBox)this.myCustomPaintingTextBoxes[TextBoxToEnable]).CustomPaintComplete += OnCustomPaintComplete;

                    if (TextBoxToEnable.ContextMenuStrip is null)
                    {
                        TextBoxToEnable.ContextMenuStrip = new ContextMenuStrip();
                    }
                    TextBoxToEnable.ContextMenuStrip.Tag = TextBoxToEnable.Name;

                    myContextMenus.Add(TextBoxToEnable, TextBoxToEnable.ContextMenuStrip);

                    bool boolFound = false;
                    for (int i = 0, loopTo1 = Information.UBound(myControls); i <= loopTo1; i++)
                    {
                        if (myControls[i].Name == TextBoxToEnable.Name)
                        {
                            boolFound = true;
                            break;
                        }
                    }

                    if (!boolFound)
                    {
                        Array.Resize(ref myControls, Information.UBound(myControls) + 1 + 1);
                        myControls[Information.UBound(myControls)] = TextBoxToEnable;
                    }

                    // Set up all of the handlers
                    TextBoxToEnable.TextChanged += TextBox_TextChanged;
                    TextBoxToEnable.KeyDown += TextBox_KeyDown;
                    TextBoxToEnable.KeyPress += TextBox_KeyPress;
                    TextBoxToEnable.MouseMove += TextBox_MouseMove;
                    TextBoxToEnable.ContextMenuStrip.Opening += ContextMenu_Opening;
                    TextBoxToEnable.ContextMenuStrip.Closed += ContextMenu_Closed;

                    ((SpellCheckControl)this.mySpellCheckers[TextBoxToEnable]).SetText(TextBoxToEnable.Text);
                }
                else
                {
                    this.controlEnabled[TextBoxToEnable] = true;
                }

                TextBoxToEnable.Invalidate();
            }
        }
    }

    /// <summary>
    /// Allows this class to be disabled programatically
    /// </summary>
    /// <param name="TextBoxToDisable"></param>
    /// <remarks></remarks>
    public void DisableTextBoxBase(ref TextBoxBase TextBoxToDisable)
    {
        this.controlEnabled[TextBoxToDisable] = false;
        TextBoxToDisable.Invalidate();
    }

    public bool IsEnabled(ref TextBoxBase TextBoxBaseToCheck)
    {
        return Conversions.ToBoolean(this.controlEnabled[TextBoxBaseToCheck]);
    }
    #endregion
    #region Provided Properties
    #region Enabled

    /// <summary>
    /// This will return whether the spell checker is enabled for the requested textbox.
    /// The default value is false, otherwise, the SetSpellCheckEnabled will never be called
    /// and there will be no way to set up the event handlers
    /// </summary>
    /// <param name="extendee">The control being tested</param>
    /// <returns>A boolean representing whether spell check is enabled</returns>
    /// <remarks></remarks>
    [Category("SpellCheck")]
    [DefaultValue(false)]
    public bool GetSpellCheckEnabled(Control extendee)
    {
        if (this.controlEnabled[extendee] is null)
        {
            controlEnabled.Add(extendee, false);
        }

        return Conversions.ToBoolean(this.controlEnabled[extendee]);
    }

    public static void DisposeThreadPools()
    {

        foreach (var threadPool in customThreadPoolList)
            threadPool.Dispose();

    }

    /// <summary>
    /// Sets whether the spellcheck is enabled.  This is only called if the requested value
    /// is different from the default value (therefore if the spell check is enabled).
    /// Once we set the enabled property, we then set up the event handlers
    /// 
    /// In case the spellchecker is disabled programatically, we include the options for 
    /// removing the event handlers as well.
    /// </summary>
    /// <param name="extendee">The control associated with the enabled request</param>
    /// <param name="Input">A boolean representing whether spell check is enabled</param>
    /// <remarks></remarks>
    public void SetSpellCheckEnabled(Control extendee, bool Input)
    {
        if (myNHunspell is null)
        {
            controlEnabled.Add(extendee, false);
            return;
        }

        // Set the hashtables
        if (this.controlEnabled[extendee] is null)
        {

            customThreadPool = new CustomThreadPool();

            if (customThreadPoolList is null)
            {
                customThreadPoolList = new List<CustomThreadPool>();
            }

            customThreadPoolList.Add(customThreadPool);

            controlEnabled.Add(extendee, Input & myNHunspell is not null);

            mySpellCheckers.Add(extendee, new SpellCheckControl(ref myNHunspell));
            TextBoxBase argCallingTextBox = (TextBoxBase)extendee;
            SpellCheckControl argThisSpellCheckControl = (SpellCheckControl)this.mySpellCheckers[extendee];
            var argParent = this;
            myCustomPaintingTextBoxes.Add(extendee, new CustomPaintTextBox(ref argCallingTextBox, ref argThisSpellCheckControl, ref argParent));
            ((CustomPaintTextBox)this.myCustomPaintingTextBoxes[extendee]).CustomPaintComplete += OnCustomPaintComplete;

            if (((TextBoxBase)extendee).ContextMenuStrip is null)
            {
                ((TextBoxBase)extendee).ContextMenuStrip = new ContextMenuStrip();

            } ((TextBoxBase)extendee).ContextMenuStrip.Opening += ContextMenu_Opening;
            ((TextBoxBase)extendee).ContextMenuStrip.Closed += ContextMenu_Closed;

            myContextMenus.Add(extendee, ((TextBoxBase)extendee).ContextMenuStrip);

            Array.Resize(ref myControls, Information.UBound(myControls) + 1 + 1);
            myControls[Information.UBound(myControls)] = extendee;
        }
        else
        {
            this.controlEnabled[extendee] = Input & myNHunspell is not null;
        }

        // Get the handlers
        if (Input == true & myNHunspell is not null)
        {

            ((TextBoxBase)extendee).TextChanged += TextBox_TextChanged;
            ((TextBoxBase)extendee).KeyDown += TextBox_KeyDown;
            ((TextBoxBase)extendee).KeyPress += TextBox_KeyPress;
            ((TextBoxBase)extendee).MouseMove += TextBox_MouseMove;
        }
        else
        {
            ((TextBoxBase)extendee).TextChanged -= TextBox_TextChanged;
            ((TextBoxBase)extendee).KeyDown -= TextBox_KeyDown;
            ((TextBoxBase)extendee).KeyPress -= TextBox_KeyPress;
            ((TextBoxBase)extendee).MouseMove -= TextBox_MouseMove;
        }

    }

    #endregion


    #endregion
    #region Properties

    public enum SuggestionNumbers
    {
        One,
        Two,
        Three,
        Four,
        Five
    }

    [Description("Sets the key that will bring up the full spell check dialog")]
    [Browsable(true)]
    public Shortcut ShortcutKey
    {
        get
        {
            return _shortcutKey;
        }
        set
        {
            _shortcutKey = value;
        }
    }

    [Description("Sets the number of suggestions that will be returned on a right-click")]
    [Browsable(true)]
    [DefaultValue(SuggestionNumbers.Five)]
    public SuggestionNumbers NumberofSuggestions
    {
        get
        {
            switch (myNumOfSuggestions)
            {
                case 1:
                    {
                        return SuggestionNumbers.One;
                    }
                case 2:
                    {
                        return SuggestionNumbers.Two;
                    }
                case 3:
                    {
                        return SuggestionNumbers.Three;
                    }
                case 4:
                    {
                        return SuggestionNumbers.Four;
                    }

                default:
                    {
                        return SuggestionNumbers.Five;
                    }
            }
        }
        set
        {
            switch (value)
            {
                case SuggestionNumbers.One:
                    {
                        myNumOfSuggestions = 1;
                        break;
                    }
                case SuggestionNumbers.Two:
                    {
                        myNumOfSuggestions = 2;
                        break;
                    }
                case SuggestionNumbers.Three:
                    {
                        myNumOfSuggestions = 3;
                        break;
                    }
                case SuggestionNumbers.Four:
                    {
                        myNumOfSuggestions = 4;
                        break;
                    }
                case SuggestionNumbers.Five:
                    {
                        myNumOfSuggestions = 5;
                        break;
                    }
            }
        }
    }

    [Description("Enables or disables spell checking as the user types.")]
    public bool SpellAsYouType
    {
        get
        {
            return _SpellAsYouType;
        }
        set
        {
            _SpellAsYouType = value;

            foreach (TextBoxBase txtBox in myControls)
                txtBox.Invalidate();
        }
    }


    public event LanguageChangedEventHandler LanguageChanged;

    public delegate void LanguageChangedEventHandler(object sender, string NewLanguage);


    [Description("Selects the language for spell checking. (Will only change the language on the developer's computer)")]
    [Editor(typeof(LanguageEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public string Language
    {
        get
        {
            return _Language;
        }
        set
        {
            if (MaintainUserChoice & !DesignMode)
                return;

            bool boolFound = false;

            foreach (var lang in GetAvailableLanguages())
            {
                if ((lang ?? "") == (value ?? ""))
                    boolFound = true;
            }

            if (!boolFound)
                return;

            bool raiseChangeEvent = (value ?? "") != (_Language ?? "");

            _Language = value;

            if (raiseChangeEvent)
            {
                LanguageChanged?.Invoke(this, value);
            }
        }
    }


    [Description("If selected to false, whenever the program starts up, it will default to the designer selection" + Constants.vbNewLine + "If selected to true, will disable any direct calls to change the language." + "To change the language, use the SelectLanguage method")]
    public bool MaintainUserChoice { get; set; }

    #endregion
    #region Events



    #region CustomEvents
    public event CustomPaintCompleteEventHandler CustomPaintComplete;

    public delegate void CustomPaintCompleteEventHandler(TextBoxBase sender, long Milliseconds);

    public void OnCustomPaintComplete(TextBoxBase sender, long Milliseconds)
    {
        CustomPaintComplete?.Invoke(sender, Milliseconds);
    }
    #endregion
    #region TextBox Events

    /// <summary>
    /// When the text changes, check all of the words in the text box.  If there is a spelling error
    /// then inform the user of that error.
    /// </summary>
    /// <param name="sender">The textbox that is being typed in</param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    private void TextBox_TextChanged(object sender, EventArgs e)
    {

        TextBoxBase textBox = (TextBoxBase)sender;

        // If Not _SpellAsYouType Then Return

        {
            var withBlock = (SpellCheckControl)mySpellCheckers[sender];
            withBlock.SetText(((TextBoxBase)sender).Text);

        } ((TextBoxBase)sender).Invalidate();

        if (timer.Enabled)
        {
            timer.Stop();
        }

        lastSelectionStart = textBox.SelectionStart;
        lastTextBox = textBox;

        timer.Start();

    }

    /// <summary>
    /// Handles the shortcuts (have to use KeyDown because KeyPress doesn't capture the function keys or delete)
    /// </summary>
    /// <param name="sender">The TextBox being typed in</param>
    /// <param name="e">The key being pushed down</param>
    /// <remarks></remarks>
    private void TextBox_KeyDown(object sender, KeyEventArgs e)
    {

        if (ShortcutKey == (Shortcut) e.KeyCode)
        {
            // If Not _SpellAsYouType Then
            // CType(mySpellCheckers[sender), SpellCheckControl).SetText(CType(sender, TextBoxBase).Text]
            // End If

            if (!((SpellCheckControl)mySpellCheckers[sender]).HasSpellingErrors())
                return;

            TextBoxBase argcallingTextBox = (TextBoxBase)sender;
            RunFullSpellChecker(ref argcallingTextBox);
            return;
        }

        // If Not _SpellAsYouType Then Return

        if (e.Control | e.Alt)
        {
            controlPressed = true;
        }
        else
        {
            controlPressed = false;
        }

        if (e.KeyCode == Keys.Delete)
        {
            for (int i = 1, loopTo = ((TextBoxBase)sender).SelectionLength + 1; i <= loopTo; i++)
                ((SpellCheckControl)mySpellCheckers[sender]).RemoveText(((TextBoxBase)sender).SelectionStart);

            {
                var withBlock = (CustomPaintTextBox)myCustomPaintingTextBoxes[sender];
                withBlock.ForcePaint();
            }
        }
    }

    /// <summary>
    /// Handles the backspace and adding characters
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        // If Not _SpellAsYouType Then Return

        {
            var withBlock = (SpellCheckControl)mySpellCheckers[sender];
            if (controlPressed)
                return;

            if (((TextBoxBase)sender).SelectionLength > 0)
            {
                for (int i = 1, loopTo = ((TextBoxBase)sender).SelectionLength; i <= loopTo; i++)
                    withBlock.RemoveText(((TextBoxBase)sender).SelectionStart);
            }

            if (e.KeyChar == (char) Keys.Back)
            {
                withBlock.RemoveText(((TextBoxBase)sender).SelectionStart - 1);
            }
            else
            {
                withBlock.AddText(e.KeyChar.ToString(), ((TextBoxBase)sender).SelectionStart);
            }

            {
                var withBlock1 = (CustomPaintTextBox)myCustomPaintingTextBoxes[sender];
                withBlock1.ForcePaint();
            }
        }
    }

    #endregion
    #region Mouse Events

    private Point currentMouseLocation;
    private TextBoxBase currentTextBox;

    private void TextBox_MouseMove(object sender, MouseEventArgs e)
    {
        currentMouseLocation = e.Location;
        currentTextBox = (TextBoxBase)sender;
    }

    #endregion

    private void MyLanguageChanged(object sender, string NewLanguage)
    {
        ((NHunspellTextBoxExtender)sender).SetLanguage(NewLanguage);
    }


    #endregion

    #region ContextMenu

    private ToolStripMenuItem _Suggestion1, _Suggestion2, _Suggestion3, _Suggestion4, _Suggestion5;

    internal virtual ToolStripMenuItem Suggestion1
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _Suggestion1;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_Suggestion1 != null)
            {
                _Suggestion1.Click -= ContextMenuItem_Click;
            }

            _Suggestion1 = value;
            if (_Suggestion1 != null)
            {
                _Suggestion1.Click += ContextMenuItem_Click;
            }
        }
    }

    internal virtual ToolStripMenuItem Suggestion2
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _Suggestion2;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_Suggestion2 != null)
            {
                _Suggestion2.Click -= ContextMenuItem_Click;
            }

            _Suggestion2 = value;
            if (_Suggestion2 != null)
            {
                _Suggestion2.Click += ContextMenuItem_Click;
            }
        }
    }

    internal virtual ToolStripMenuItem Suggestion3
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _Suggestion3;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_Suggestion3 != null)
            {
                _Suggestion3.Click -= ContextMenuItem_Click;
            }

            _Suggestion3 = value;
            if (_Suggestion3 != null)
            {
                _Suggestion3.Click += ContextMenuItem_Click;
            }
        }
    }

    internal virtual ToolStripMenuItem Suggestion4
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _Suggestion4;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_Suggestion4 != null)
            {
                _Suggestion4.Click -= ContextMenuItem_Click;
            }

            _Suggestion4 = value;
            if (_Suggestion4 != null)
            {
                _Suggestion4.Click += ContextMenuItem_Click;
            }
        }
    }

    internal virtual ToolStripMenuItem Suggestion5
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _Suggestion5;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_Suggestion5 != null)
            {
                _Suggestion5.Click -= ContextMenuItem_Click;
            }

            _Suggestion5 = value;
            if (_Suggestion5 != null)
            {
                _Suggestion5.Click += ContextMenuItem_Click;
            }
        }
    }
    private ToolStripMenuItem _AddWord, _IgnoreWord, _IgnoreAll, _Spelling;

    internal virtual ToolStripMenuItem AddWord
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _AddWord;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_AddWord != null)
            {
                _AddWord.Click -= ContextMenuItem_Click;
            }

            _AddWord = value;
            if (_AddWord != null)
            {
                _AddWord.Click += ContextMenuItem_Click;
            }
        }
    }

    internal virtual ToolStripMenuItem IgnoreWord
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _IgnoreWord;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_IgnoreWord != null)
            {
                _IgnoreWord.Click -= ContextMenuItem_Click;
            }

            _IgnoreWord = value;
            if (_IgnoreWord != null)
            {
                _IgnoreWord.Click += ContextMenuItem_Click;
            }
        }
    }

    internal virtual ToolStripMenuItem IgnoreAll
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _IgnoreAll;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_IgnoreAll != null)
            {
                _IgnoreAll.Click -= ContextMenuItem_Click;
            }

            _IgnoreAll = value;
            if (_IgnoreAll != null)
            {
                _IgnoreAll.Click += ContextMenuItem_Click;
            }
        }
    }

    internal virtual ToolStripMenuItem Spelling
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            return _Spelling;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        set
        {
            if (_Spelling != null)
            {
                _Spelling.Click -= ContextMenuItem_Click;
            }

            _Spelling = value;
            if (_Spelling != null)
            {
                _Spelling.Click += ContextMenuItem_Click;
            }
        }
    }
    private ToolStripSeparator Separator1, Separator2;
    private Point originalMouseLocation;
    private TextBoxBase ownerTextBox;
    private int lastSelectionStart;
    private TextBoxBase lastTextBox;
    private System.Threading.ManualResetEvent resetEvent;

    /// <summary>
    /// Controls all of the contextmenuitem clicks
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    public void ContextMenuItem_Click(object sender, EventArgs e)
    {
        // Get which button was clicked then perform its action.  Afterwards, remove all buttons

        switch (((ToolStripMenuItem)sender).Name)
        {
            // If it's a Spell1 through Spell5, then it's a suggestion item
            case "qwr3Spell1":
                {
                    ReplaceWord((string) Suggestion1.Tag, Suggestion1.Text);
                    break;
                }
            case "qwr3Spell2":
                {
                    ReplaceWord((string)Suggestion2.Tag, Suggestion2.Text);
                    break;
                }
            case "qwr3Spell3":
                {
                    ReplaceWord((string)Suggestion3.Tag, Suggestion3.Text);
                    break;
                }
            case "qwr3Spell4":
                {
                    ReplaceWord((string)Suggestion4.Tag, Suggestion4.Text);
                    break;
                }
            case "qwr3Spell5":
                {
                    ReplaceWord((string)Suggestion5.Tag, Suggestion5.Text);
                    break;
                }
            case "qwr3Add":
                {
                    AddWordToDictionary((string)AddWord.Tag);
                    break;
                }
            case "qwr3Ignore":
                {
                    IgnoreSelectedWord((string)IgnoreWord.Tag, ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).Left, ((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).Top);
                    break;
                }
            case "qwr3IgnoreAll":
                {
                    IgnoreAllWord((string)IgnoreAll.Tag);
                    break;
                }
            case "qwr3Spelling":
                {
                    TextBoxBase ownerTextBoxBase = default;

                    for (int i = 0, loopTo = Information.UBound(myControls); i <= loopTo; i++)
                    {
                        if (myControls[i].Name == Spelling.Tag)
                        {
                            ownerTextBoxBase = (TextBoxBase) myControls[i];
                        }
                    }

                    RunFullSpellChecker(ref ownerTextBoxBase);
                    break;
                }
        }
    }


    /// <summary>
    /// If it was closed by not clicking on an item, then we remove the items and reset them
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    private void ContextMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
    {
        if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
            return;

        {
            var withBlock = (ContextMenuStrip)sender;
            if (Suggestion1 is not null)
            {
                withBlock.Items.Remove(Suggestion1);
                Suggestion1 = default;
            }
            if (Suggestion2 is not null)
            {
                withBlock.Items.Remove(Suggestion2);
                Suggestion2 = default;
            }
            if (Suggestion3 is not null)
            {
                withBlock.Items.Remove(Suggestion3);
                Suggestion3 = default;
            }
            if (Suggestion4 is not null)
            {
                withBlock.Items.Remove(Suggestion4);
                Suggestion4 = default;
            }
            if (Suggestion5 is not null)
            {
                withBlock.Items.Remove(Suggestion5);
                Suggestion5 = default;
            }
            if (AddWord is not null)
            {
                withBlock.Items.Remove(AddWord);
                AddWord = default;
            }
            if (IgnoreWord is not null)
            {
                withBlock.Items.Remove(IgnoreWord);
                IgnoreWord = default;
            }
            if (IgnoreAll is not null)
            {
                withBlock.Items.Remove(IgnoreAll);
                IgnoreAll = default;
            }
            if (Spelling is not null)
            {
                withBlock.Items.Remove(Spelling);
                Spelling = default;
            }
            if (Separator1 is not null)
            {
                withBlock.Items.Remove(Separator1);
                Separator1 = null;
            }
            if (Separator2 is not null)
            {
                withBlock.Items.Remove(Separator2);
                Separator2 = null;
            }
        }
    }


    /// <summary>
    /// If we are spell checking as the user types, add items to the textbox's context menu
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    private void ContextMenu_Opening(object sender, CancelEventArgs e)
    {
        if (!_SpellAsYouType)
            return;

        // MessageBox.Show(CType(sender, ContextMenuStrip).MouseButtons.ToString)

        e.Cancel = true;

        // Make sure that none of the items are still in the menu
        {
            var withBlock = (ContextMenuStrip)sender;
            if (Suggestion1 is not null)
            {
                withBlock.Items.Remove(Suggestion1);
                Suggestion1 = default;
            }
            if (Suggestion2 is not null)
            {
                withBlock.Items.Remove(Suggestion2);
                Suggestion2 = default;
            }
            if (Suggestion3 is not null)
            {
                withBlock.Items.Remove(Suggestion3);
                Suggestion3 = default;
            }
            if (Suggestion4 is not null)
            {
                withBlock.Items.Remove(Suggestion4);
                Suggestion4 = default;
            }
            if (Suggestion5 is not null)
            {
                withBlock.Items.Remove(Suggestion5);
                Suggestion5 = default;
            }

            if (AddWord is not null)
            {
                withBlock.Items.Remove(AddWord);
            }
            if (IgnoreWord is not null)
            {
                withBlock.Items.Remove(IgnoreWord);
            }
            if (IgnoreAll is not null)
            {
                withBlock.Items.Remove(IgnoreAll);
            }
            if (Spelling is not null)
            {
                withBlock.Items.Remove(Spelling);
            }

            if (Separator1 is not null)
            {
                withBlock.Items.Remove(Separator1);
                Separator1 = null;
            }
            if (Separator2 is not null)
            {
                withBlock.Items.Remove(Separator2);
                Separator2 = null;
            }
        }

        // See if we're over a spelling error

        // get the textbox
        ownerTextBox = currentTextBox;

        // See if spell-checking is enabled
        if (!(bool) this.controlEnabled[ownerTextBox])
        {
            if (((ContextMenuStrip)sender).Items.Count != 0)
                e.Cancel = false;

            return;
        }

        SpellCheckControl spellChecker = (SpellCheckControl)this.mySpellCheckers[ownerTextBox];
        var lockObject = spellChecker.LockObject;

        using (lockObject.Lock())
        {
            // first see if there are any spelling errors
            if (!spellChecker.HasSpellingErrors())
            {
                if (((ContextMenuStrip)sender).Items.Count != 0)
                    e.Cancel = false;

                return;
            }

#if USE_API_SERVICES
            if (!spellChecker.HasGrammarErrors())
            {
                if (((ContextMenuStrip)sender).Items.Count != 0)
                    e.Cancel = false;

                return;
            }
#endif
            int charIndex;

            // Get the location of the word based on the starting point for the context menu
            originalMouseLocation = currentMouseLocation;

            charIndex = ownerTextBox.GetCharIndexFromPosition(currentMouseLocation);

            // This will actually still return a character even if not directly over one.  We need to check the character
            // height against the mouseLocation.Y
            if (ownerTextBox is RichTextBox)
            {

                {
                    var withBlock1 = (RichTextBox)ownerTextBox;
                    // We're going to find the font that was the tallest used
                    var fontHeight = default(int);
                    Font selFont;
                    selFont = withBlock1.Font;

                    long firstCharInLine, lastCharInLine, curCharLine;
                    curCharLine = withBlock1.GetLineFromCharIndex(charIndex);
                    firstCharInLine = withBlock1.GetFirstCharIndexFromLine((int) curCharLine);
                    lastCharInLine = withBlock1.GetFirstCharIndexFromLine((int)(curCharLine + 1L));

                    if (lastCharInLine == -1)
                        lastCharInLine = ownerTextBox.TextLength;

                    var tempRTB = new RichTextBox();
                    tempRTB.Rtf = withBlock1.Rtf;

                    // Now find the tallest font
                    for (long i = firstCharInLine + 1L, loopTo = lastCharInLine + 1L; i <= loopTo; i++)
                    {
                        tempRTB.SelectionStart = (int) i;
                        tempRTB.SelectionLength = 1;
                        if (tempRTB.SelectionFont.Height > fontHeight)
                        {
                            fontHeight = tempRTB.SelectionFont.Height;
                            selFont = tempRTB.SelectionFont;
                        }
                    }

                    // Now find out if the mouse could be over the word or is in blank space
                    using (Graphics g = withBlock1.CreateGraphics())
                    {
                        int y;
                        y = (int) g.MeasureString(withBlock1.GetCharFromPosition(currentMouseLocation).ToString(), selFont).Height + withBlock1.GetPositionFromCharIndex(charIndex).Y;
                        if (currentMouseLocation.Y > y | currentMouseLocation.Y < 0)
                        {
                            if (((ContextMenuStrip)sender).Items.Count != 0)
                            {
                                e.Cancel = false;
                            }

                            return;
                        }
                    }
                }
            }
            else
            {
                // We get here if it's a regular textbox.  We can juse use it's font height and see if
                // we're over an item or blank space
                using (Graphics g = ownerTextBox.CreateGraphics())
                {
                    int y;

                    long currentIndex = ownerTextBox.GetCharIndexFromPosition(currentMouseLocation);
                    long currentLine = ownerTextBox.GetLineFromCharIndex((int) currentIndex);

                    y = (int) (ownerTextBox.Font.Height * (currentLine + 1L));
                    if (currentMouseLocation.Y > y | currentMouseLocation.Y < 0)
                    {
                        // We're not actually over an item

                        if (((ContextMenuStrip)sender).Items.Count != 0)
                        {
                            e.Cancel = false;
                        }

                        return;
                    }
                }
            }

            // If the current charIndex is not part of a misspelled word, just exit
            if (!spellChecker.IsPartOfSpellingError(charIndex))
            {
#if USE_API_SERVICES
                if (spellChecker.IsPartOfGrammarError(charIndex))
                {
                    SetupGrammarContextMenu((ContextMenuStrip)sender, charIndex);
                    e.Cancel = false;
                    return;
                }
#endif
                e.Cancel = true;
                return;
            }

            // Otherwise...

            // Set up the contextmenu
            if (((ContextMenuStrip)sender).Items.Count > 0)
            {
                // We're adding to the user created context menu, so add a line
                Separator1 = new ToolStripSeparator();
                ((ContextMenuStrip)sender).Items.Add(Separator1);
            }

            // Get the suggestions
            string[] suggestions;
            suggestions = new string[0];
            string misspelledWord;
            {
                var withBlock2 = (SpellCheckControl)this.mySpellCheckers[ownerTextBox];
                misspelledWord = withBlock2.GetMisspelledWordAtPosition(charIndex);
                suggestions = withBlock2.GetSuggestions(misspelledWord, myNumOfSuggestions);
            }

            // Add the suggestion buttons
            {
                var withBlock3 = (ContextMenuStrip)sender;
                if (Information.UBound(suggestions) == -1)
                {
                    Suggestion1 = new ToolStripMenuItem("No suggestions found");
                    Suggestion1.Name = "qwr3Spell1";
                    Suggestion1.ToolTipText = "No suggestions found";
                    Suggestion1.Font = new Font(Suggestion1.Font, FontStyle.Italic);
                    Suggestion1.Tag = misspelledWord;
                    Suggestion1.Enabled = false;
                    withBlock3.Items.Add(Suggestion1);
                }
                else
                {
                    for (int i = 0, loopTo1 = Information.UBound(suggestions); i <= loopTo1; i++)
                    {
                        var onClickHandler = new EventHandler(ContextMenuItem_Click);

                        // The tag on the suggestion items is the misspelled word
                        switch (i)
                        {
                            case 0:
                                {
                                    Suggestion1 = new ToolStripMenuItem(suggestions[i]);
                                    Suggestion1.Name = "qwr3Spell1";
                                    Suggestion1.ToolTipText = suggestions[i];
                                    Suggestion1.Font = new Font(Suggestion1.Font, FontStyle.Bold);
                                    Suggestion1.Tag = misspelledWord;
                                    withBlock3.Items.Add(Suggestion1);
                                    break;
                                }
                            case 1:
                                {
                                    Suggestion2 = new ToolStripMenuItem(suggestions[i]);
                                    Suggestion2.Name = "qwr3Spell2";
                                    Suggestion2.ToolTipText = suggestions[i];
                                    Suggestion2.Font = new Font(Suggestion2.Font, FontStyle.Bold);
                                    Suggestion2.Tag = misspelledWord;
                                    withBlock3.Items.Add(Suggestion2);
                                    break;
                                }
                            case 2:
                                {
                                    Suggestion3 = new ToolStripMenuItem(suggestions[i]);
                                    Suggestion3.Name = "qwr3Spell3";
                                    Suggestion3.ToolTipText = suggestions[i];
                                    Suggestion3.Font = new Font(Suggestion3.Font, FontStyle.Bold);
                                    Suggestion3.Tag = misspelledWord;
                                    withBlock3.Items.Add(Suggestion3);
                                    break;
                                }
                            case 3:
                                {
                                    Suggestion4 = new ToolStripMenuItem(suggestions[i]);
                                    Suggestion4.Name = "qwr3Spell4";
                                    Suggestion4.ToolTipText = suggestions[i];
                                    Suggestion4.Font = new Font(Suggestion4.Font, FontStyle.Bold);
                                    Suggestion4.Tag = misspelledWord;
                                    withBlock3.Items.Add(Suggestion4);
                                    break;
                                }
                            case 4:
                                {
                                    Suggestion5 = new ToolStripMenuItem(suggestions[i]);
                                    Suggestion5.Name = "qwr3Spell5";
                                    Suggestion5.ToolTipText = suggestions[i];
                                    Suggestion5.Font = new Font(Suggestion5.Font, FontStyle.Bold);
                                    Suggestion5.Tag = misspelledWord;
                                    withBlock3.Items.Add(Suggestion5);
                                    break;
                                }
                        }
                    }
                }

                Separator2 = new ToolStripSeparator();
                withBlock3.Items.Add(Separator2);

                // Now add the add and ignore buttons
                if (AddWord is null)
                {
                    AddWord = new ToolStripMenuItem("Add Word...");
                    AddWord.Name = "qwr3Add";
                    AddWord.ToolTipText = "Add the word to the dictionary";
                }
                // The addWord Tag is the misspelled word
                AddWord.Tag = misspelledWord;
                withBlock3.Items.Add(AddWord);

                if (IgnoreWord is null)
                {
                    IgnoreWord = new ToolStripMenuItem("Ignore Once...");
                    IgnoreWord.Name = "qwr3Ignore";
                    IgnoreWord.ToolTipText = "Ignore this instance of the currently selected word";
                }

                // The ignore once tag is the name of the textbox
                IgnoreWord.Tag = ownerTextBox.Name;
                withBlock3.Items.Add(IgnoreWord);

                if (IgnoreAll is null)
                {
                    IgnoreAll = new ToolStripMenuItem("Ignore All...");
                    IgnoreAll.Name = "qwr3IgnoreAll";
                    IgnoreAll.ToolTipText = "Ignore all instances of the currently selected word";
                }
                // The ignore all Tag is the misspelled word
                IgnoreAll.Tag = misspelledWord;
                withBlock3.Items.Add(IgnoreAll);

                // Now add the spelling button
                if (Spelling is null)
                {
                    Spelling = new ToolStripMenuItem("Run Spell Checker...");
                    Spelling.Name = "qwr3Spelling";
                    Spelling.ToolTipText = "Runs the full spell checker on this text.";
                }
                // The Spelling tag is the name of the textbox
                Spelling.Tag = ownerTextBox.Name;
                withBlock3.Items.Add(Spelling);
            }
        }

        e.Cancel = false;

    }

#if USE_API_SERVICES
    private void SetupGrammarContextMenu(ContextMenuStrip menu, int charIndex)
    {

        GrammarEdit grammarEdit;
        ToolStripMenuItem item;
        SpellCheckControl spellChecker = (SpellCheckControl)this.mySpellCheckers[ownerTextBox];
        var lockObject = spellChecker.LockObject;

        menu.Items.Clear();

        using (lockObject.Lock())
        {

            var list = spellChecker.GrammarEdits.Where((GrammarEdit g) => !g.HandledOrIgnored & charIndex.IsBetween(g.StartGrammar, g.EndGrammar)).ToList();

            foreach (GrammarEdit currentGrammarEdit in list)
            {
                grammarEdit = currentGrammarEdit;

                item = (ToolStripMenuItem) menu.Items.Add(grammarEdit.Result.Correction.Replace("..", "."));
                item.Tag = grammarEdit;

                item.Click += new EventHandler(GrammarItem_Click);

            }

            item = (ToolStripMenuItem) menu.Items.Add("Ignore");
            item.Tag = list;

        }

        item.Click += new EventHandler(GrammarItemIgnore_Click);

    }

    private void GrammarItemIgnore_Click(object sender, EventArgs e)
    {

        ToolStripMenuItem item = (ToolStripMenuItem) sender;
        List<GrammarEdit> list = (List<GrammarEdit>)item.Tag;
        SpellCheckControl spellChecker = (SpellCheckControl)this.mySpellCheckers[ownerTextBox];
        var lockObject = spellChecker.LockObject;

        using (lockObject.Lock())
            foreach (var grammarEdit in list)
                grammarEdit.Ingored = true;

        // Enable redraw
        SendMessage(ownerTextBox.Handle, WM_SETREDRAW, new IntPtr(Conversions.ToInteger(true)), IntPtr.Zero);

        ownerTextBox.Refresh();

    }

    private void GrammarItem_Click(object sender, EventArgs e)
    {
        ToolStripMenuItem item = (ToolStripMenuItem) sender;
        GrammarEdit grammarEdit = (GrammarEdit)item.Tag;
        var @add = default(int);
        int length;
        SpellCheckControl spellChecker = (SpellCheckControl)this.mySpellCheckers[ownerTextBox];
        var lockObject = spellChecker.LockObject;
        List<Edit> editsList;

        using (lockObject.Lock())
            editsList = grammarEdit.Result.Edits.ToList();

        foreach (var edit in editsList)
        {
            length = edit.End - edit.Start;
            ownerTextBox.Text = ownerTextBox.Text.Substitute(edit.Start + @add, length, edit.Replace);
            @add += edit.Replace.Length - length;
        }

        grammarEdit.Handled = true;

        // Enable redraw
        SendMessage(ownerTextBox.Handle, WM_SETREDRAW, new IntPtr(Conversions.ToInteger(true)), IntPtr.Zero);

        ownerTextBox.Refresh();
    }
#endif

    /// <summary>
    /// Replaces the word that was clicked on with a word from the suggestions
    /// </summary>
    /// <param name="OriginalWord"></param>
    /// <param name="NewWord"></param>
    /// <remarks></remarks>
    private void ReplaceWord(string OriginalWord, string NewWord)
    {
        // Get original scroll position
        int Position = GetScrollPos(ownerTextBox.Handle, SBS_VERT);
        SpellCheckControl spellChecker = (SpellCheckControl)this.mySpellCheckers[ownerTextBox];
        var lockObject = spellChecker.LockObject;

        // Disable redraw
        SendMessage(ownerTextBox.Handle, WM_SETREDRAW, new IntPtr(Conversions.ToInteger(false)), IntPtr.Zero);

        // Get the location of the original word to replace from the contextmenustrip
        int charIndex = ownerTextBox.GetCharIndexFromPosition(originalMouseLocation);

        CharacterRange[] charRanges;
        int wordStart;
        wordStart = -1;

        using (lockObject.Lock())
        {
            charRanges = spellChecker.GetSpellingErrorRanges();

            foreach (var currentRange in charRanges)
            {
                if (charIndex >= currentRange.First & charIndex <= currentRange.First + currentRange.Length - 1)
                {
                    wordStart = currentRange.First;
                }
            }
        }

        if (wordStart == -1)
            return;

        if (ownerTextBox is RichTextBox)
        {
            double _zoomFactor = ((RichTextBox)ownerTextBox).ZoomFactor;

            var tempRTB = new RichTextBox();
            tempRTB.Rtf = ((RichTextBox)ownerTextBox).Rtf;

            tempRTB.SelectionStart = wordStart;
            tempRTB.SelectionLength = OriginalWord.Length;
            tempRTB.SelectedText = NewWord;

            ((RichTextBox)ownerTextBox).Rtf = tempRTB.Rtf;
            ((RichTextBox)ownerTextBox).ZoomFactor = 1;
            ((RichTextBox)ownerTextBox).ZoomFactor = (int) _zoomFactor;
        }
        else if (wordStart == 0)
        {
            ownerTextBox.Text = Strings.Replace(ownerTextBox.Text, OriginalWord, NewWord, 1);
        }
        else
        {
            ownerTextBox.Text = Strings.Left(ownerTextBox.Text, wordStart - 1) + Strings.Replace(ownerTextBox.Text, OriginalWord, NewWord, wordStart, 1);
        }

        ownerTextBox.SelectionStart = wordStart + NewWord.Length;
        ownerTextBox.SelectionLength = 0;

        // Reset scroll position
        if (SetScrollPos(ownerTextBox.Handle, SBS_VERT, Position, true) != -1)
        {
            PostMessageA(ownerTextBox.Handle, WM_VSCROLL, SB_THUMBPOSITION + 0x10000 * Position, default);
        }

        // Enable redraw
        SendMessage(ownerTextBox.Handle, WM_SETREDRAW, new IntPtr(Conversions.ToInteger(true)), IntPtr.Zero);

        ownerTextBox.Refresh();
    }

    /// <summary>
    /// Adds the word to the dictionary in memory and to a file on disk
    /// </summary>
    /// <param name="WordToAdd"></param>
    /// <remarks></remarks>
    private void AddWordToDictionary(string WordToAdd)
    {
        if (!boolDisableAddWordPrompt)
        {
            DialogResult result;
            result = MyDialog.Show("This will add the word " + '"' + WordToAdd + '"' + "." + Constants.vbNewLine + Constants.vbNewLine + "Are you sure?", "Add Word to Dictionary");

            // result = MessageBox.Show("This will add the word " & Chr(34) & WordToAdd & Chr(34) & "." & _
            // vbNewLine & vbNewLine & "Are you sure?", "Add Word to Dictionary", _
            // MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)

            // Check if we're to disable future prompts
            if (result == (DialogResult.Ignore | DialogResult.No))
            {
                boolDisableAddWordPrompt = true;
                return;
            }
            else if (result == (DialogResult.Yes | DialogResult.Ignore))
            {
                boolDisableAddWordPrompt = true;
            }
            else if (result == DialogResult.No)
            {
                return;
            }
        }

        // Add it to the dictionary in memory
        myNHunspell.Add(WordToAdd);

        // Add it to the file on disk
        string callingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        // Try and write to the directory to see if we can
        bool boolFailed = false;

        try
        {
            Directory.CreateDirectory(callingDir + @"\Test");
        }
        catch (Exception ex)
        {
            boolFailed = true;
        }
        finally
        {
            Directory.Delete(callingDir + @"\Test");
        }

        if (boolFailed)
        {
            callingDir = @"C:\Windows\Temp";
        }

        if (!Directory.Exists(callingDir + @"\SpellCheck"))
        {
            Directory.CreateDirectory(callingDir + @"\SpellCheck");
        }

        using (var w = new StreamWriter(callingDir + @"\SpellCheck\" + Language + "AddedWords.dat", true))
        {
            w.WriteLine(WordToAdd);
            w.Flush();
            w.Close();
        }

        // Reset all of the textboxes to refresh the spelling
        for (int i = 0, loopTo = Information.UBound(myControls); i <= loopTo; i++)
        {
            if (myControls[i] is TextBoxBase)
            {
                {
                    var withBlock = (TextBoxBase)myControls[i];
                    // Get original scroll position
                    int Position = GetScrollPos(withBlock.Handle, SBS_VERT);

                    // Disable redraw
                    SendMessage(withBlock.Handle, WM_SETREDRAW, new IntPtr(Conversions.ToInteger(false)), IntPtr.Zero);

                    string controlText, controlRTF;
                    controlText = withBlock.Text;
                    controlRTF = "";

                    if (myControls[i] is RichTextBox)
                    {
                        controlRTF = ((RichTextBox)myControls[i]).Rtf;
                    }

                    int selectionStart, selectionLength;
                    selectionLength = withBlock.SelectionLength;
                    selectionStart = withBlock.SelectionStart;

                    withBlock.ResetText();
                    withBlock.Text = controlText;

                    if (!string.IsNullOrEmpty(controlRTF))
                    {
                        ((RichTextBox)myControls[i]).Rtf = controlRTF;
                    }

                    withBlock.SelectionStart = selectionStart;
                    withBlock.SelectionLength = selectionLength;

                    // Reset scroll position
                    if (SetScrollPos(withBlock.Handle, SBS_VERT, Position, true) != -1)
                    {
                        PostMessageA(withBlock.Handle, WM_VSCROLL, SB_THUMBPOSITION + 0x10000 * Position, default);
                    }

                    // Enable redraw
                    SendMessage(withBlock.Handle, WM_SETREDRAW, new IntPtr(Conversions.ToInteger(true)), IntPtr.Zero);

                    withBlock.Refresh();
                }
            }
        }


    }

    /// <summary>
    /// Ignores the selected word once
    /// </summary>
    /// <param name="callingTextBoxName"></param>
    /// <param name="LeftLocation"></param>
    /// <param name="TopLocation"></param>
    /// <remarks></remarks>
    private void IgnoreSelectedWord(string callingTextBoxName, int LeftLocation, int TopLocation)
    {
        // We're only ignoring the currently selected word, so we need to get the range to add it to the spell checker
        TextBoxBase callingTextBox = default;
        SpellCheckControl spellChecker = (SpellCheckControl)this.mySpellCheckers[ownerTextBox];
        var lockObject = spellChecker.LockObject;

        for (int i = 0, loopTo = Information.UBound(myControls); i <= loopTo; i++)
        {
            if (myControls[i].Name == callingTextBoxName)
            {
                callingTextBox = (TextBoxBase) myControls[i];
            }
        }

        if (callingTextBox is null)
            return;

        // Get the range of the original word
        int charIndex = callingTextBox.GetCharIndexFromPosition(originalMouseLocation);

        var misspelledRange = new CharacterRange(-1, -1);

        using (lockObject.Lock())
        {

            foreach (CharacterRange currentRange in spellChecker.GetSpellingErrorRanges())
            {
                if (currentRange.First <= charIndex & currentRange.First + currentRange.Length + 1 >= charIndex)
                {
                    misspelledRange = currentRange;
                    break;
                }
            }

            if (misspelledRange.First == -1)
                return;

            // Add the range to the ignore words list
            spellChecker.AddRangeToIgnore(misspelledRange);
        }

        // repaint the textbox
        callingTextBox.Invalidate();
    }

    private void InitializeComponent()
    {
        components = new Container();
        _timer = new System.Windows.Forms.Timer(components);
#if USE_API_SERVICES
        _timer.Tick += timer_Tick;
#endif
        // 
        // timer
        // 
        _timer.Interval = 1000;

    }

#if USE_API_SERVICES
    private void timer_Tick(object sender, EventArgs e)
    {
        var text = lastTextBox.Text;
        var args = new GetSentenceEventArgs(text);
        GrammarBotResult result;
        SpellCheckControl spellChecker = (SpellCheckControl)this.mySpellCheckers[lastTextBox];
        var lockObject = spellChecker.LockObject;

        if (resetEvent is null)
        {
            resetEvent = new System.Threading.ManualResetEvent(false);
        }
        else if (!resetEvent.WaitOne(1))
        {
            return;
        }

        timer.Stop();

        GetSentencesEvent(this, args);
                 
        if (args.Sentences is not null && args.Sentences.Length > 0)
        {
            customThreadPool.QueueUserTask(() =>
            {
                try
                {
                    foreach (var sentence in args.Sentences)
                    {
                        try
                        {
                            result = GrammarBotService.CheckGrammar(sentence);
                        }
                        catch (Exception ex)
                        {
                            return;
                        }

                        using (lockObject.Lock())
                        {
                            if (result.Edits.Count > 0 & !Operators.ConditionalCompareObjectEqual(result.Correction, sentence, false))
                            {

                                try
                                {
                                    var index = lastTextBox.Text.IndexOf(sentence);
                                    var startGrammar = index + result.Edits.Min((e2) => e2.Start);
                                    var endGrammar = Math.Min(index + result.Edits.Max((e2) => e2.End), lastTextBox.TextLength);

                                    GrammarEdit grammarEdit;

                                    foreach (var edit in result.Edits)
                                    {
                                        edit.Start += index;
                                        edit.End += index;
                                    }

                                    if (endGrammar == startGrammar)
                                    {
                                        startGrammar -= 1;
                                    }

                                    grammarEdit = spellChecker.GrammarEdits.SingleOrDefault((GrammarEdit g) => g.StartGrammar == startGrammar & g.EndGrammar == endGrammar & Operators.ConditionalCompareObjectEqual(g.Sentence, sentence, false));

                                    if (grammarEdit is null)
                                    {

                                        grammarEdit = new GrammarEdit(startGrammar, endGrammar, sentence, result);

                                        foreach (var oldEdit in spellChecker.GrammarEdits.Where((GrammarEdit g) => g.StartGrammar.IsBetween(grammarEdit.StartGrammar, grammarEdit.EndGrammar) | g.EndGrammar.IsBetween(grammarEdit.StartGrammar, grammarEdit.EndGrammar)).ToList())
                                            spellChecker.GrammarEdits.Remove(oldEdit);

                                        spellChecker.GrammarEdits.Add(grammarEdit);

                                        lastTextBox.DelayInvoke(1, () => lastTextBox.Invalidate());

                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }

                    using (lockObject.Lock())
                    {
                        foreach (var edit in spellChecker.GrammarEdits.ToList())
                        {

                            if (!text.Contains(edit.Sentence))
                            {
                                spellChecker.GrammarEdits.Remove(edit);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                }

                resetEvent.Set();

            }, (taskStatus) => { if (!taskStatus.Success) { DebugUtils.Break(); } });

            while (!resetEvent.WaitOne(1))
            {
                lastTextBox.DoEventsSleep(100);
            }

        }
    }
#endif

    /// <summary>
    /// Ignore all instances of the word.  This will add the word to the dictionary in memory, but not on disk
    /// </summary>
    /// <param name="WordToIgnore"></param>
    /// <remarks></remarks>
    private void IgnoreAllWord(string WordToIgnore)
    {

        // Add the word to the dictionary in memory
        myNHunspell.Add(WordToIgnore);

        // Reset all of the textboxes to refresh the spelling
        for (int i = 0, loopTo = Information.UBound(myControls); i <= loopTo; i++)
        {
            if (myControls[i] is TextBoxBase)
            {
                {
                    var withBlock = (TextBoxBase)myControls[i];
                    // Get original scroll position
                    int Position = GetScrollPos(withBlock.Handle, SBS_VERT);

                    // Disable redraw
                    SendMessage(withBlock.Handle, WM_SETREDRAW, new IntPtr(Conversions.ToInteger(false)), IntPtr.Zero);

                    string controlText, controlRTF;
                    controlText = withBlock.Text;
                    controlRTF = "";

                    if (myControls[i] is RichTextBox)
                    {
                        controlRTF = ((RichTextBox)myControls[i]).Rtf;
                    }

                    int selectionStart, selectionLength;
                    selectionLength = withBlock.SelectionLength;
                    selectionStart = withBlock.SelectionStart;

                    withBlock.ResetText();
                    withBlock.Text = controlText;

                    if (!string.IsNullOrEmpty(controlRTF))
                    {
                        ((RichTextBox)myControls[i]).Rtf = controlRTF;
                    }

                    withBlock.SelectionStart = selectionStart;
                    withBlock.SelectionLength = selectionLength;

                    // Reset scroll position
                    if (SetScrollPos(withBlock.Handle, SBS_VERT, Position, true) != -1)
                    {
                        PostMessageA(withBlock.Handle, WM_VSCROLL, SB_THUMBPOSITION + 0x10000 * Position, default);
                    }

                    // Enable redraw
                    SendMessage(withBlock.Handle, WM_SETREDRAW, new IntPtr(Conversions.ToInteger(true)), IntPtr.Zero);

                    withBlock.Refresh();
                }
            }
        }


    }

#endregion

    #region Change Language
    public string[] GetAvailableLanguages()
    {
        string[] languageList;

        var regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\NHunspellTextBoxExtender\Languages");
        languageList = regKey.GetValue("LanguageList") as string[];

        regKey.Close();
        regKey.Dispose();

        return languageList;
    }

    public void SetLanguage(string NewLanguage)
    {
        // Check if the language is in the registry
        bool boolFound = false;

        foreach (string st in GetAvailableLanguages())
        {
            if ((st ?? "") == (NewLanguage ?? ""))
                boolFound = true;
        }

        if (!boolFound)
        {
            throw new ArgumentException("LanguageToRemove does not exist!", "LanguageToRemove", new Exception("The language " + NewLanguage + " is not currently loaded."));
        }

        // Open the registry
        var regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\NHunspellTextBoxExtender\Languages", true);

        string[] paths = regKey.GetValue(NewLanguage) as string[];

        if (regKey.GetValue(NewLanguage) is null)
        {
            // If we get there, then the Aff and Dic files don't exist

            string[] languages = regKey.GetValue("LanguageList") as string[];
            var newLanguageList = new string[(Information.UBound(languages))];
            int count = 0;

            foreach (var Lang in languages)
            {
                if ((Lang ?? "") != (NewLanguage ?? ""))
                {
                    newLanguageList[count] = Lang;
                    count += 1;
                }
            }

            regKey.SetValue("LanguageList", newLanguageList, RegistryValueKind.MultiString);
            regKey.DeleteValue(NewLanguage);

            regKey.Close();
            regKey.Dispose();

            throw new FileNotFoundException("Aff and Dic files are missing");
        }
        else
        {
            foreach (var path in paths)
            {
                if (!File.Exists(path))
                {
                    // System.Windows.Forms.MessageBox.Show("Aff and Dic files are missing")

                    string[] languages = regKey.GetValue("LanguageList") as string[];
                    var newLanguageList = new string[(Information.UBound(languages))];
                    int count = 0;

                    foreach (var Lang in languages)
                    {
                        if ((Lang ?? "") != (NewLanguage ?? ""))
                        {
                            newLanguageList[count] = Lang;
                            count += 1;
                        }
                    }

                    regKey.SetValue("LanguageList", newLanguageList, RegistryValueKind.MultiString);
                    regKey.DeleteValue(NewLanguage);

                    regKey.Close();
                    regKey.Dispose();

                    throw new FileNotFoundException("File not found", path);
                }
            }
        }

        // If we get here, then the paths and language are valid
        // Now try to create the object
        try
        {
            myNHunspell = new Hunspell(paths[0], paths[1]);
        }
        catch (Exception ex)
        {
            // MessageBox.Show("Could not create the new Hunspell using the specified language")

            string[] languages = regKey.GetValue("LanguageList") as string[];
            var newLanguageList = new string[(Information.UBound(languages))];
            int count = 0;

            foreach (var Lang in languages)
            {
                if ((Lang ?? "") != (NewLanguage ?? ""))
                {
                    newLanguageList[count] = Lang;
                    count += 1;
                }
            }

            regKey.SetValue("LanguageList", newLanguageList, RegistryValueKind.MultiString);
            regKey.DeleteValue(NewLanguage);

            regKey.Close();
            regKey.Dispose();

            throw ex;
        }

        // See if there are any words to add
        if (File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\SpellCheck\" + _Language + "AddedWords.dat"))
        {
            using (var r = new StreamReader(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\SpellCheck\" + _Language + "AddedWords.dat"))
            {
                while (!r.EndOfStream)
                    myNHunspell.Add(Strings.Trim(Strings.Replace(r.ReadLine(), Constants.vbNewLine, "")));
                r.Close();
            }
        }

        _Language = NewLanguage;
        regKey.SetValue("Default", NewLanguage);
        regKey.Close();
        regKey.Dispose();

        // Reset all of the textboxes to refresh the spelling
        for (int i = 0, loopTo = Information.UBound(myControls); i <= loopTo; i++)
        {
            if (myControls[i] is TextBoxBase)
            {
                {
                    var withBlock = (TextBoxBase)myControls[i];
                    {
                        var withBlock1 = (SpellCheckControl)this.mySpellCheckers[myControls[i]];
                        withBlock1.UpdateHunspell(ref myNHunspell);
                    }


                    // Get original scroll position
                    int Position = GetScrollPos(withBlock.Handle, SBS_VERT);

                    // Disable redraw
                    SendMessage(withBlock.Handle, WM_SETREDRAW, new IntPtr(Conversions.ToInteger(false)), IntPtr.Zero);

                    string controlText, controlRTF;
                    controlText = withBlock.Text;
                    controlRTF = "";

                    if (myControls[i] is RichTextBox)
                    {
                        controlRTF = ((RichTextBox)myControls[i]).Rtf;
                    }

                    int selectionStart, selectionLength;
                    selectionLength = withBlock.SelectionLength;
                    selectionStart = withBlock.SelectionStart;

                    withBlock.ResetText();
                    withBlock.Text = controlText;

                    if (!string.IsNullOrEmpty(controlRTF))
                    {
                        ((RichTextBox)myControls[i]).Rtf = controlRTF;
                    }

                    withBlock.SelectionStart = selectionStart;
                    withBlock.SelectionLength = selectionLength;

                    // Reset scroll position
                    if (SetScrollPos(withBlock.Handle, SBS_VERT, Position, true) != -1)
                    {
                        PostMessageA(withBlock.Handle, WM_VSCROLL, SB_THUMBPOSITION + 0x10000 * Position, default);
                    }

                    // Enable redraw
                    SendMessage(withBlock.Handle, WM_SETREDRAW, new IntPtr(Conversions.ToInteger(true)), IntPtr.Zero);

                    withBlock.Refresh();
                }
            }
        }
    }

    public bool AddNewLanguage()
    {
        // Open an AddLanguage Form
        var newAddLanguage = new AddLanguage();
        newAddLanguage.ShowDialog();

        if (newAddLanguage.Result == DialogResult.Cancel)
            return false;

        // Add the Item to the registry
        RegistryKey regKey;
        regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\NHunspellTextBoxExtender\Languages", true);

        string[] languages = regKey.GetValue("LanguageList") as string[];

        bool boolFound = false;
        foreach (var lang in languages)
        {
            if (lang == newAddLanguage.txtName.Text)
            {
                boolFound = true;
                break;
            }
        }

        if (!boolFound)
        {
            Array.Resize(ref languages, Information.UBound(languages) + 1 + 1);
            languages[Information.UBound(languages)] = newAddLanguage.txtName.Text;

            regKey.SetValue("LanguageList", languages, RegistryValueKind.MultiString);
        }

        var paths = new string[2];

        paths[0] = newAddLanguage.txtAff.Text;
        paths[1] = newAddLanguage.txtDic.Text;

        regKey.SetValue(newAddLanguage.txtName.Text, paths, RegistryValueKind.MultiString);

        regKey.Close();
        regKey.Dispose();

        return true;
    }

    public void RemoveLanguage(string LanguageToRemove)
    {
        // Check if the language is in the registry
        bool boolFound = false;

        foreach (string st in GetAvailableLanguages())
        {
            if ((st ?? "") == (LanguageToRemove ?? ""))
                boolFound = true;
        }

        if (!boolFound)
        {
            throw new ArgumentException("LanguageToRemove does not exist!", "LanguageToRemove", new Exception("The language " + LanguageToRemove + " is not currently loaded."));
        }

        // Open the registry
        var regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\NHunspellTextBoxExtender\Languages", true);

        // Remove the language from the LanguageList
        string[] languages = regKey.GetValue("LanguageList") as string[];

        if (Information.UBound(languages) == -1)
        {
            throw new Exception("Unable to retrieve Language list from registry");
        }

        var newLanguageList = new string[(Information.UBound(languages))];
        int count = 0;

        foreach (var lang in languages)
        {
            if ((lang ?? "") != (LanguageToRemove ?? ""))
            {
                newLanguageList[count] = lang;
                count += 1;
            }
        }

        // Update the registry
        regKey.SetValue("LanguageList", newLanguageList, RegistryValueKind.MultiString);

        if (regKey.GetValue(LanguageToRemove) is not null)
        {
            regKey.DeleteValue(LanguageToRemove);
        }

        // Check if the default was the language removed
        if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(regKey.GetValue("Default"), LanguageToRemove, false)))
        {
            if (regKey.GetValue("English") is not null)
            {
                SetLanguage("English");
            }
            else if (GetAvailableLanguages().Count() == 0)
            {
                // Default to English
                ResetLanguages();
            }
            else
            {
                SetLanguage(GetAvailableLanguages()[0]);
            }
        }

        // Check if all of the languagues were removed
        if (newLanguageList.Count() == 0)
        {
            ResetLanguages();
        }

        regKey.Close();
        regKey.Dispose();
    }

    private void ResetLanguages()
    {
        string USdic, USaff;

        // Get the calling assembly's location
        string callingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        var regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\NHunspellTextBoxExtender\Languages", true);

        string[] paths = regKey.GetValue("English") as string[];

        if (regKey.GetValue("English") is null)
        {
            // Set the paths for the dic and aff files
            USdic = callingDir + @"\SpellCheck\en_US.dic";
            USaff = callingDir + @"\SpellCheck\en_US.aff";

            // Check if the spell check directory already exists.  If not, add it
            if (!Directory.Exists(callingDir + @"\SpellCheck"))
            {
                Directory.CreateDirectory(callingDir + @"\SpellCheck");
                var newDirInfo = new DirectoryInfo(callingDir + @"\SpellCheck");
                newDirInfo.Attributes = FileAttributes.Hidden;
            }

            // Check if the spell check files already exist.  If not, add it
            if (!File.Exists(USaff))
            {
                try
                {
                    File.WriteAllBytes(USaff, global::My.Resources.en_US);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing en_US.aff file!" + Constants.vbNewLine + ex.Message);
                }
            }

            if (!File.Exists(USdic))
            {
                try
                {
                    File.WriteAllBytes(USdic, global::My.Resources.en_US_dic);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing en_US.dic file!" + Constants.vbNewLine + ex.Message);
                }
            }

            paths = new[] { USaff, USdic };
        }
        else
        {
            if (!File.Exists(paths[0]))
            {
                USaff = callingDir + @"\SpellCheck\en_US.aff";

                try
                {
                    File.WriteAllBytes(USaff, global::My.Resources.en_US);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing en_US.aff file!" + Constants.vbNewLine + ex.Message);
                }

                paths[0] = USaff;
            }

            if (!File.Exists(paths[1]))
            {
                USdic = callingDir + @"\SpellCheck\en_US.dic";

                try
                {
                    File.WriteAllBytes(USdic, global::My.Resources.en_US_dic);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error writing en_US.dic file!" + Constants.vbNewLine + ex.Message);
                }

                paths[1] = USdic;
            }
        }

        _Language = "English";
        regKey.SetValue("Default", "English");
        regKey.SetValue("English", paths, RegistryValueKind.MultiString);
        regKey.SetValue("LanguageList", new[] { "English" }, RegistryValueKind.MultiString);
        regKey.Close();
        regKey.Dispose();

        SetLanguage("English");

    }

    public void UpdateLanguageFiles(string LanguageToUpdate, string NewAffFileLocation, string NewDicFileLocation, bool OverwriteExistingFiles = false, bool RemoveOlderFiles = false)
    {
        // Check if the language exists
        bool boolFound = false;

        foreach (string st in GetAvailableLanguages())
        {
            if ((st ?? "") == (LanguageToUpdate ?? ""))
                boolFound = true;
        }

        if (!boolFound)
        {
            throw new ArgumentException("LanguageToRemove does not exist!", "LanguageToRemove", new Exception("The language " + LanguageToUpdate + " is not currently loaded and cannot be updated." + Constants.vbNewLine + "If you are trying to add a new language, use teh AddLanguage() method"));
        }

        // Check if the new file paths are valid
        if (!File.Exists(NewAffFileLocation))
        {
            throw new FileNotFoundException("File could not be found", NewAffFileLocation);
        }

        if (!File.Exists(NewDicFileLocation))
        {
            throw new FileNotFoundException("File could not be found", NewDicFileLocation);
        }

        // Open the registry key
        var regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\NHunspellTextBoxExtender\Languages", true);

        string[] paths;

        if (OverwriteExistingFiles)
        {
            // Get the original file locations
            paths = regKey.GetValue(LanguageToUpdate) as string[];

            // If nothing was returned, we can just overwrite the registry
            if (regKey.GetValue(LanguageToUpdate) is null)
            {
                paths = new string[2];
                paths[0] = NewAffFileLocation;
                paths[1] = NewDicFileLocation;
            }
            else
            {
                // If we are overwriting, we need to check that the originals exist
                if (string.IsNullOrEmpty(paths[0]))
                {
                    paths[0] = NewAffFileLocation;
                }
                else
                {
                    File.Copy(NewAffFileLocation, paths[0]);
                }

                if (string.IsNullOrEmpty(paths[1]))
                {
                    paths[1] = NewDicFileLocation;
                }
                else
                {
                    File.Copy(NewDicFileLocation, paths[1]);
                }
            }

            // Save the new paths
            regKey.SetValue(LanguageToUpdate, paths, RegistryValueKind.MultiString);
            regKey.Close();
            regKey.Dispose();
        }
        else
        {
            // If we are removing the older files, check if they exist, then delete them
            if (RemoveOlderFiles)
            {
                paths = regKey.GetValue(LanguageToUpdate) as string[];

                if (!string.IsNullOrEmpty(paths[0]))
                {
                    if (File.Exists(paths[0]))
                        File.Delete(paths[0]);

                    if (File.Exists(paths[1]))
                        File.Delete(paths[1]);
                }
            }

            // Reset the registry
            paths = new string[2];
            paths[0] = NewAffFileLocation;
            paths[1] = NewDicFileLocation;

            regKey.SetValue(LanguageToUpdate, paths, RegistryValueKind.MultiString);
            regKey.Close();
            regKey.Dispose();
        }
    }
    #endregion

    #region SpellCheckForm

    public void RunFullSpellChecker(ref TextBoxBase callingTextBox)
    {
        // first see if there is anything misspelled
        if (!((SpellCheckControl)this.mySpellCheckers[callingTextBox]).HasSpellingErrors())
        {
            MessageBox.Show("No spelling errors were detected." + Constants.vbNewLine + Constants.vbNewLine + "Spell check is complete.");
            return;
        }

        // If the textbox is a rich text box, we have to get the selection fonts
        var fontHashTable = new Hashtable();
        string rtf = "";
        double zoomFactor = 1d;

        if (callingTextBox is RichTextBox)
        {
            rtf = ((RichTextBox)callingTextBox).Rtf;
            zoomFactor = ((RichTextBox)callingTextBox).ZoomFactor;
        }

        // Create the new spell checking form
        var newSpellCheckForm = new SpellCheckForm(ref callingTextBox, (SpellCheckControl)this.mySpellCheckers[callingTextBox], boolDisableAddWordPrompt);

        // Show the form
        newSpellCheckForm.ShowDialog();

        // We get here when the form is closed.
        // We're going to refresh the text in the textbox
        boolDisableAddWordPrompt = newSpellCheckForm.DisableConfirmationPrompt;

        // First make sure that the ignore ranges are not reset
        ((SpellCheckControl)this.mySpellCheckers[callingTextBox]).DontResetIgnoreRanges();

        // Clear the text in the textbox and reset it
        callingTextBox.Clear();

        callingTextBox.Text = newSpellCheckForm.NewText();

        if (callingTextBox is RichTextBox)
        {
            {
                var withBlock = (RichTextBox)callingTextBox;
                withBlock.Rtf = newSpellCheckForm.GetRTF();
                withBlock.ZoomFactor = 1;
                withBlock.ZoomFactor = (int) zoomFactor;
            }
        }

        callingTextBox.SelectionStart = callingTextBox.TextLength;

        ((SpellCheckControl)this.mySpellCheckers[callingTextBox]).DontResetIgnoreRanges(false);

        callingTextBox.Invalidate();

        // Reset all of the textboxes to refresh the spelling
        for (int i = 0, loopTo = Information.UBound(myControls); i <= loopTo; i++)
        {
            if (myControls[i].Name != callingTextBox.Name)
            {
                if (myControls[i] is TextBoxBase)
                {
                    {
                        var withBlock1 = (TextBoxBase)myControls[i];
                        string controlText, controlRTF;
                        controlText = withBlock1.Text;
                        controlRTF = "";

                        if (myControls[i] is RichTextBox)
                        {
                            controlRTF = ((RichTextBox)myControls[i]).Rtf;
                        }

                        int selectionStart, selectionLength;
                        selectionLength = withBlock1.SelectionLength;
                        selectionStart = withBlock1.SelectionStart;

                        withBlock1.ResetText();
                        withBlock1.Text = controlText;

                        if (!string.IsNullOrEmpty(controlRTF))
                        {
                            ((RichTextBox)myControls[i]).Rtf = controlRTF;
                        }

                        withBlock1.SelectionStart = selectionStart;
                        withBlock1.SelectionLength = selectionLength;
                    }
                }
            }
        }
    }

    ~NHunspellTextBoxExtender()
    {
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }


    #endregion

}