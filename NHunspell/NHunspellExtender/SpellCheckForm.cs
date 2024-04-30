using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualBasic; // Install-Package Microsoft.VisualBasic
using Microsoft.VisualBasic.CompilerServices; // Install-Package Microsoft.VisualBasic

public partial class SpellCheckForm
{


    #region Private variables
    private RichTextBox _originalRichTextBox;
    private TextBox _originalTextBox;
    // Used as a temporary textbox for manipulating the RTF info without causing screen updates

    private TextBoxBase _callingTextBox;
    // The textboxbase that called this form

    private SpellCheckControl _spellChecker;
    // The spellCheckControl associated with the calling textboxbase

    private long[,] sentenceBreaks;
    // sentenceBreaks stores the original position of the sentence break
    // along with the new position of the sentence breaks
    // Example:          0123456789012345678901234567890123456789012
    // _OriginalText  = "Thie ist a test.  To shiw the way it werks.
    // _NewText       = "This is a test. To show the way it works."
    // sentenceBreaks = { 15 , 14 }
    // { 42 , 40 }

    private long currentSentence;
    // Used as an index representing which sentence we are currently in

    private long currentWordStart;
    // An index representing the starting position of the currently misspelled word in relation
    // to the full text

    private bool DisableTextChanged;
    // Used to make sure that if we change the text programmatically, the buttons aren't changed

    private CharacterRange currentWordRange;
    // An index of the currently misspelled word in relation to the current sentence

    private long currentSentenceStartIndex;
    // An index representing the starting point for the current sentence in relation to the full text

    private double _zoomFactor;
    // The ZoomFactor of the calling textboxbase if it's a RichTextBox

    private bool _DisableConfirmationPrompt;
    #endregion


    #region New and Shown

    public SpellCheckForm(ref TextBoxBase CallingControl, SpellCheckControl SpellChecker, bool DisableConfirmationPrompt)
    {

        // This call is required by the designer.
        InitializeComponent();

        _zoomFactor = 1d;

        // Set up the form
        if (CallingControl is RichTextBox)
        {
            _originalRichTextBox = new RichTextBox();
            _originalTextBox = null;

            _originalRichTextBox.Rtf = ((RichTextBox)CallingControl).Rtf;
            _zoomFactor = 1d;
            _zoomFactor = ((RichTextBox)CallingControl).ZoomFactor;
        }
        else
        {
            _originalRichTextBox = null;
            _originalTextBox = new TextBox();

            _originalTextBox.Text = CallingControl.Text;
        }

        _DisableConfirmationPrompt = DisableConfirmationPrompt;

        _callingTextBox = CallingControl;

        _spellChecker = SpellChecker;

        lboSuggestions.Items.Clear();

        // Make sure that we're starting the spell checking fresh
        _spellChecker.ClearIgnoreRanges();

        SetSentenceBreaks();
        currentWordStart = -1;
        DisableTextChanged = false;

        SetNextWord();
        this.Shown += SpellCheckForm_Shown;
    }

    private void SpellCheckForm_Shown(object sender, EventArgs e)
    {
        if (_originalTextBox is not null)
        {
            if (_originalTextBox.Text == "" | !_spellChecker.HasSpellingErrors())
                this.Hide();
        }
        else if (_originalRichTextBox.Text == "" | !_spellChecker.HasSpellingErrors())
            this.Hide();
    }

    #endregion


    #region Add Word to Dictionary

    /// <summary>
    /// Given a misspelled word, this will add the word to the dictionary as a correctly spelled word.
    /// This added word will be perpetuated on future program starts
    /// </summary>
    /// <param name="sender">The System.Windows.Forms.Button that was clicked</param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    private void cmdAdd_Click(object sender, EventArgs e)
    {
        if (!_DisableConfirmationPrompt)
        {
            DialogResult dlgResult;
            dlgResult = MyDialog.Show("This will add the word to the dictionary." + Constants.vbNewLine + "Continue?", "Add Word to Dictionary");

            // Check if we're to disable future prompts
            if (dlgResult == (DialogResult.Ignore | DialogResult.No))
            {
                _DisableConfirmationPrompt = true;
                return;
            }
            else if (dlgResult == (DialogResult.Yes | DialogResult.Ignore))
            {
                _DisableConfirmationPrompt = true;
            }
            else if (dlgResult == DialogResult.No)
            {
                return;
            }
        }

        // Get the word to add
        string currentWord = Strings.Mid(txtCurrentSentence.Text, currentWordRange.First + 1, currentWordRange.Length);

        // Add it to the spell checker in memory
        _spellChecker.myNHunspell.Add(currentWord);

        // Add it to the added words file on disk
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
            if (Directory.Exists(callingDir + @"\Test"))
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

        var regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\NHunspellTextBoxExtender\Languages");

        string language = Conversions.ToString(regKey.GetValue("Default"));

        regKey.Close();
        regKey.Dispose();

        using (var w = new StreamWriter(callingDir + @"\SpellCheck\" + language + "AddedWords.dat", true))
        {
            w.WriteLine(currentWord);
            w.Flush();
            w.Close();
        }

        // Reset the callingTextBox
        // Make sure that the ignore ranges aren't reset (we can do this because we always start at the beginning
        // and move forwards so any previously set ignore ranges should still be ignored)
        _spellChecker.DontResetIgnoreRanges();

        // Clear the original text and overwrite it (just to refresh the spell checking)
        _callingTextBox.Clear();

        // If it's a RichTextBox, we also have to reset the RTF and ZoomFactor
        if (_callingTextBox is RichTextBox)
        {
            ((RichTextBox)_callingTextBox).Rtf = _originalRichTextBox.Rtf;
            ((RichTextBox)_callingTextBox).ZoomFactor = 1;
            ((RichTextBox)_callingTextBox).ZoomFactor = (int) _zoomFactor;
        }
        else
        {
            _callingTextBox.Text = _originalTextBox.Text;
        }

        _spellChecker.DontResetIgnoreRanges(false);

        // Go to the next misspelled word
        if (!SetNextWord())
        {
            this.Hide();
        }
    }

    #endregion


    #region Ignore

    /// <summary>
    /// This will ignore the currently selected word one time.  This only lasts as long as the text is not
    /// changed by the user.
    /// Also serves as the Undo Edit button if the user types in the textbox
    /// </summary>
    /// <param name="sender">System.Windows.Forms.Button</param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    private void cmdIgnoreOnce_Click(object sender, EventArgs e)
    {
        // Check if we've retitled the button
        if (cmdIgnoreOnce.Text == "Undo Edit")
        {
            // We're going to reset the text to its original state before any user manipulation
            DisableTextChanged = true;

            // We're just going to change the currentWordStart to a couple before (subtracting one would work as
            // well, but why not use 2?) and then just tell it to set the next word
            currentWordStart -= 2L;
            SetNextWord();

            DisableTextChanged = false;
        }
        else
        {
            // We get here if we're ignoring the misspelled word once.
            // We do this by adding an ignore range to the control's spellcheckcontrol

            // Get the range of the word in the newtext
            long rangeStart;

            if (Information.UBound(sentenceBreaks, 2) == 0)
            {
                rangeStart = currentWordRange.First;
            }
            else
            {
                rangeStart = currentSentenceStartIndex + currentWordRange.First;
            }

            // Add the range to ignore
            _spellChecker.AddRangeToIgnore(new CharacterRange((int) rangeStart, currentWordRange.Length));

            // Reset the calling textbox
            _spellChecker.DontResetIgnoreRanges();

            // Clear the original text and overwrite it (just to refresh the spell checking)
            _callingTextBox.Clear();

            // If it's a RichTextBox, we also have to reset the RTF and ZoomFactor
            if (_callingTextBox is RichTextBox)
            {
                ((RichTextBox)_callingTextBox).Rtf = _originalRichTextBox.Rtf;
                ((RichTextBox)_callingTextBox).ZoomFactor = 1;
                ((RichTextBox)_callingTextBox).ZoomFactor = (int)_zoomFactor;
            }
            else
            {
                _callingTextBox.Text = _originalTextBox.Text;
            }

            _spellChecker.DontResetIgnoreRanges(false);

            // Go to the next misspelled word
            if (!SetNextWord())
            {
                // we have gone through all of the spelling errors
                this.Hide();
            }
        }
    }

    /// <summary>
    /// This will ignore all future instances of the word for the remainder of the time the calling form is open.
    /// This is done by adding the word to the spellchecker dictionary in memory
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    private void cmdIgnoreAll_Click(object sender, EventArgs e)
    {
        // Make sure they want to do this because it can't be undone until this session finishes
        DialogResult dlgResult;
        dlgResult = MessageBox.Show("This will ignore all instances of the word." + Constants.vbNewLine + "Continue?", "Ignore All", MessageBoxButtons.YesNo);

        if (dlgResult == System.Windows.Forms.DialogResult.Yes)
        {
            // get the current word then add it to the dictionary in memory

            string currentWord = Strings.Mid(txtCurrentSentence.Text, currentWordRange.First + 1, currentWordRange.Length);
            _spellChecker.myNHunspell.Add(currentWord);

            // Reset the calling textbox
            _spellChecker.DontResetIgnoreRanges();

            // Clear the original text and overwrite it (just to refresh the spell checking)
            _callingTextBox.Clear();

            // If it's a RichTextBox, we also have to reset the RTF and ZoomFactor
            if (_callingTextBox is RichTextBox)
            {
                ((RichTextBox)_callingTextBox).Rtf = _originalRichTextBox.Rtf;
                ((RichTextBox)_callingTextBox).ZoomFactor = 1;
                ((RichTextBox)_callingTextBox).ZoomFactor = (int)_zoomFactor;
            }
            else
            {
                _callingTextBox.Text = _originalTextBox.Text;
            }

            _spellChecker.DontResetIgnoreRanges(false);

            // Go to the next word
            if (!SetNextWord())
            {
                // we have gone through all of the spelling errors
                this.Hide();
            }
        }
    }

    #endregion


    #region Change Word

    /// <summary>
    /// This will change one instance of the word.  It will either use the word selected from the listbox
    /// or what the user types.  The user can change the whole sentence through this function.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    private void cmdChange_Click(object sender, EventArgs e)
    {
        // If the listbox is enabled, then we're changing the word to the selected word from the list box
        // Otherwise, it's what was typed in the text box.

        // First, see if the new word is spelled correctly.  Otherwise, ask them if that's what they want to do
        // If it is, add any of the _NewText that occurred before this sentence, then add the sentence in the box
        // then add any sentences after this one.
        TextBoxBase textBoxToUse;

        if (_originalRichTextBox is not null)
        {
            textBoxToUse = _originalRichTextBox;
        }
        else
        {
            textBoxToUse = _originalTextBox;
        }

        long originalSentenceLength = sentenceBreaks[1, (int)currentSentence] - currentSentenceStartIndex + 1L;
        var newSentenceLength = txtCurrentSentence.TextLength;

        if (lboSuggestions.Enabled)
        {
            int rightAmount;

            string currentWord = Strings.Mid(txtCurrentSentence.Text, currentWordRange.First + 1, currentWordRange.Length);

            string newSentence;
            newSentence = Strings.Left(txtCurrentSentence.Text, currentWordRange.First) + lboSuggestions.SelectedItem.ToString;
            rightAmount = txtCurrentSentence.TextLength - currentWordRange.First - currentWordRange.Length;
            newSentence = newSentence + Strings.Right(txtCurrentSentence.Text, rightAmount);
            txtCurrentSentence.Text = newSentence;

            // Get the new word
            string newWord;

            newWord = lboSuggestions.SelectedItem.ToString();

            // Now change the original textbox
            // We're going to select the original word and then update the Selected Text.  This will preserve the
            // Selected RTF

            textBoxToUse.SelectionStart = (int) currentWordStart;
            textBoxToUse.SelectionLength = currentWordRange.Length;
            textBoxToUse.SelectedText = newWord;

            currentWordStart += newSentenceLength - originalSentenceLength;
        }
        else
        {
            // We're just going to assume that the whole sentence needs to be changed if the user didn't
            // select a word from the suggestions box. It's more of a headache to try to find out if the
            // user has changed more than the original word than it is to just change the whole sentence

            textBoxToUse.SelectionStart = (int)currentSentenceStartIndex;
            textBoxToUse.SelectionLength = (int)originalSentenceLength;
            textBoxToUse.SelectedText = txtCurrentSentence.Text;

            currentWordStart = currentSentenceStartIndex;
        }

        // We need to update the sentence breaks and currentWordStart
        // Update sentenceBreaks
        for (long i = currentSentence, loopTo = Information.UBound(sentenceBreaks, 2); i <= loopTo; i++)
            sentenceBreaks[1, (int)i] = sentenceBreaks[1, (int)i] + (newSentenceLength - originalSentenceLength);


        // Reset the calling textbox
        _spellChecker.DontResetIgnoreRanges();

        // Clear the original text and overwrite it (just to refresh the spell checking)
        _callingTextBox.Clear();

        // If it's a RichTextBox, we also have to reset the RTF and ZoomFactor
        if (_callingTextBox is RichTextBox)
        {
            ((RichTextBox)_callingTextBox).Rtf = _originalRichTextBox.Rtf;
            ((RichTextBox)_callingTextBox).ZoomFactor = 1;
            ((RichTextBox)_callingTextBox).ZoomFactor =(int)_zoomFactor;
        }
        else
        {
            _callingTextBox.Text = _originalTextBox.Text;
        }

        _spellChecker.DontResetIgnoreRanges(false);

        if (!SetNextWord())
        {
            this.Hide();
        }
    }

    /// <summary>
    /// This will change all instances of the misspelled word.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    private void cmdChangeAll_Click(object sender, EventArgs e)
    {
        // get the original word and the new word.  then go through all of the remaining misspelled words and see
        // if the original word occurs again.  Replace them as you go and update the _originalTextBox
        // along with the sentence breaks

        TextBoxBase textBoxToUse;

        if (_originalRichTextBox is not null)
        {
            textBoxToUse = _originalRichTextBox;
        }
        else
        {
            textBoxToUse = _originalTextBox;
        }

        string originalWord = Strings.Mid(textBoxToUse.Text, (int)currentSentenceStartIndex + currentWordRange.First + 1, currentWordRange.Length);
        string newWord;
        long originalSentenceLength = sentenceBreaks[1, (int)currentSentence] - currentSentenceStartIndex + 1L;
        var newSentenceLength = txtCurrentSentence.TextLength;

        if (lboSuggestions.Enabled)
        {
            newWord = lboSuggestions.SelectedItem.ToString();
        }
        else
        {
            newWord = Strings.Mid(txtCurrentSentence.Text, currentWordRange.First + 1, (int)(currentWordRange.Length + (newSentenceLength - originalSentenceLength)));
        }

        // Spell check the new word
        if (!_spellChecker.myNHunspell.Spell(newWord))
        {
            DialogResult dlgResult;
            dlgResult = MessageBox.Show(newWord + " is not recognized as a correctly spelled word." + Constants.vbNewLine + "Would you like to continue with the replacement?", "Misspelled Word", MessageBoxButtons.YesNo);

            if (dlgResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            // Now go through each character range starting with the current range and see if it matches
            // (We don't want to change any previously looked at words)
        }
        int offsetOfFirstChar = 0;

        foreach (CharacterRange currentRange in _spellChecker.GetSpellingErrorRanges())
        {
            if (currentRange.First >= currentWordRange.First + offsetOfFirstChar + currentSentenceStartIndex)
            {
                // Check if the current word matches
                if ((Strings.Mid(textBoxToUse.Text, currentRange.First + 1 + offsetOfFirstChar, currentRange.Length) ?? "") == (originalWord ?? ""))

                {

                    // Figure out where we are in relation to _NewText
                    // To do that, find the sentence break of this sentence and find the offset from the original
                    int offset = 0;
                    int sentenceIndex = 0;

                    for (int i = 0, loopTo = Information.UBound(sentenceBreaks, 2); i <= loopTo; i++)
                    {
                        if (sentenceBreaks[1, i] > currentRange.First)
                        {
                            sentenceIndex = i;
                            break;
                        }
                    }

                    // Update the _originalTextBox
                    textBoxToUse.SelectionStart = currentRange.First + offsetOfFirstChar;
                    textBoxToUse.SelectionLength = currentRange.Length;
                    textBoxToUse.SelectedText = newWord;

                    // Update sentence breaks
                    offset = newWord.Length - originalWord.Length;
                    offsetOfFirstChar += newWord.Length - originalWord.Length;
                    for (int i = sentenceIndex, loopTo1 = Information.UBound(sentenceBreaks, 2); i <= loopTo1; i++)
                        sentenceBreaks[1, i] = sentenceBreaks[1, i] + offset;
                }
            }
        }

        // Reset the calling textbox
        _spellChecker.DontResetIgnoreRanges();

        // Clear the original text and overwrite it (just to refresh the spell checking)
        _callingTextBox.Clear();

        // If it's a RichTextBox, we also have to reset the RTF and ZoomFactor
        if (_callingTextBox is RichTextBox)
        {
            ((RichTextBox)_callingTextBox).Rtf = _originalRichTextBox.Rtf;
            ((RichTextBox)_callingTextBox).ZoomFactor = 1;
            ((RichTextBox)_callingTextBox).ZoomFactor = (int)_zoomFactor;
        }
        else
        {
            _callingTextBox.Text = _originalTextBox.Text;
        }

        _spellChecker.DontResetIgnoreRanges(false);

        // Update the sentence breaks
        for (int i = 0, loopTo2 = Information.UBound(sentenceBreaks, 2); i <= loopTo2; i++)
            sentenceBreaks[0, i] = sentenceBreaks[1, i];

        // Go to the next word
        if (!SetNextWord())
        {
            this.Hide();
        }
    }

    #endregion


    #region Cancel

    private void cmdCancel_Click(object sender, EventArgs e)
    {
        this.Hide();
    }

    #endregion


    #region Events

    /// <summary>
    /// Handles if the user types in the textbox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    private void txtWord_TextChanged(object sender, EventArgs e)
    {
        TextBoxBase textBoxToUse;

        if (_originalRichTextBox is not null)
        {
            textBoxToUse = _originalRichTextBox;
        }
        else
        {
            textBoxToUse = _originalTextBox;
        }

        // Check if we were the one that changed the text and not the user
        if (!DisableTextChanged)
        {
            // see if something other than the current word was changed
            // This doesn't work the way it should...

            if ((Strings.Left(textBoxToUse.Text, currentWordRange.First) ?? "") != (Strings.Left(txtCurrentSentence.Text, currentWordRange.First) ?? ""))
            {
                // Changed something besides the selected word
                cmdChangeAll.Enabled = false;
            }
            else
            {
                int rightAmount = textBoxToUse.TextLength - currentWordRange.First - currentWordRange.Length;
                if ((Strings.Right(textBoxToUse.Text, rightAmount) ?? "") != (Strings.Right(txtCurrentSentence.Text, rightAmount) ?? ""))
                {
                    cmdChangeAll.Enabled = false;
                }
            }

            // Change the buttons and listbox
            lboSuggestions.Enabled = false;
            cmdIgnoreAll.Enabled = false;
            cmdAdd.Enabled = false;
            cmdIgnoreOnce.Text = "Undo Edit";

            // Change the word to the original formatting
            int selectionStart = txtCurrentSentence.SelectionStart;
            txtCurrentSentence.SelectionStart = 0;
            txtCurrentSentence.SelectionLength = txtCurrentSentence.TextLength;
            txtCurrentSentence.SelectionColor = Color.Black;
            txtCurrentSentence.SelectionFont = new Font(txtCurrentSentence.Font, FontStyle.Regular);
            txtCurrentSentence.SelectionStart = selectionStart;
            txtCurrentSentence.SelectionLength = 0;
        }
    }

    #endregion


    #region Other Subs and Functions
    public bool DisableConfirmationPrompt
    {
        get
        {
            return _DisableConfirmationPrompt;
        }
    }


    /// <summary>
    /// Returns the new text for the control
    /// </summary>
    /// <returns></returns>
    /// <remarks></remarks>
    public string NewText()
    {
        if (_originalRichTextBox is not null)
        {
            return _originalRichTextBox.Text;
        }
        else
        {
            return _originalTextBox.Text;
        }
    }

    /// <summary>
    /// Goes through the text and finds any sentence breaks.  We only care about '.', '!' and '?"
    /// </summary>
    /// <remarks></remarks>
    public void SetSentenceBreaks()
    {
        sentenceBreaks = new long[2, 0];

        TextBoxBase textBoxToUse;

        if (_originalRichTextBox is not null)
        {
            textBoxToUse = _originalRichTextBox;
        }
        else
        {
            textBoxToUse = _originalTextBox;
        }

        // Search through the text and find any sentence breaks
        // (the last char is always a sentence break, so we don't
        // need to include it now
        for (int i = 0, loopTo = textBoxToUse.TextLength - 2; i <= loopTo; i++)
        {
            if (Conversions.ToBoolean(IsSentenceEnding(textBoxToUse.Text[i])))
            {
                var oldSentenceBreaks = sentenceBreaks;
                sentenceBreaks = new long[2, Information.UBound(sentenceBreaks, 2) + 1 + 1];
                if (oldSentenceBreaks is not null)
                    for (var i1 = 0; i1 <= oldSentenceBreaks.Length / oldSentenceBreaks.GetLength(1) - 1; ++i1)
                        Array.Copy(oldSentenceBreaks, i1 * oldSentenceBreaks.GetLength(1), sentenceBreaks, i1 * sentenceBreaks.GetLength(1), Math.Min(oldSentenceBreaks.GetLength(1), sentenceBreaks.GetLength(1)));
                sentenceBreaks[0, Information.UBound(sentenceBreaks, 2)] = Conversions.ToLong(i);
                sentenceBreaks[1, Information.UBound(sentenceBreaks, 2)] = Conversions.ToLong(i);
            }
        }

        // now add the last char as its own break
        var oldSentenceBreaks1 = sentenceBreaks;
        sentenceBreaks = new long[2, Information.UBound(sentenceBreaks, 2) + 1 + 1];
        if (oldSentenceBreaks1 is not null)
            for (var i = 0; i <= oldSentenceBreaks1.Length / oldSentenceBreaks1.GetLength(1) - 1; ++i)
                Array.Copy(oldSentenceBreaks1, i * oldSentenceBreaks1.GetLength(1), sentenceBreaks, i * sentenceBreaks.GetLength(1), Math.Min(oldSentenceBreaks1.GetLength(1), sentenceBreaks.GetLength(1)));
        sentenceBreaks[0, Information.UBound(sentenceBreaks, 2)] = textBoxToUse.TextLength - 1;
        sentenceBreaks[1, Information.UBound(sentenceBreaks, 2)] = textBoxToUse.TextLength - 1;
    }

    /// <summary>
    /// See if the current character is a '.', '?' or '!'
    /// </summary>
    /// <param name="Input"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    public object IsSentenceEnding(char Input)
    {
        if (Conversions.ToString(Input) == "." | Conversions.ToString(Input) == "?" | Conversions.ToString(Input) == "!")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Refresh the textbox to show the next misspelled word
    /// </summary>
    /// <returns>A Boolean value indicating if there was another misspelled word.</returns>
    /// <remarks></remarks>
    public bool SetNextWord()
    {
        TextBoxBase textBoxToUse;

        if (_originalRichTextBox is not null)
        {
            textBoxToUse = _originalRichTextBox;
        }
        else
        {
            textBoxToUse = _originalTextBox;
        }

        // Get the character ranges and then check if there is a misspelled word after the current word
        CharacterRange[] ranges = _spellChecker.GetSpellingErrorRanges();
        DisableTextChanged = true;

        // Reset buttons
        cmdIgnoreAll.Enabled = true;
        cmdIgnoreOnce.Text = "Ignore Once";
        cmdAdd.Enabled = true;
        lboSuggestions.Enabled = true;
        cmdChangeAll.Enabled = true;

        // Go through all of the misspelled words and find the next one
        foreach (var currentRange in ranges)
        {
            if (currentRange.First > currentWordStart)
            {
                // We found the next word to look at.  Now load its sentence into the text box
                // and then highlight the selected word

                currentWordStart = currentRange.First;

                // Find the sentence break after the currentWordStart and then put that sentence
                // into the textbox
                for (int i = 0, loopTo = Information.UBound(sentenceBreaks, 2); i <= loopTo; i++)
                {
                    if (i == 0)
                    {
                        if (sentenceBreaks[1, i] > currentWordStart)
                        {
                            currentSentenceStartIndex = 0L;

                            // Set the textbox text
                            txtCurrentSentence.Text = Strings.Left(textBoxToUse.Text, (int) sentenceBreaks[0, i] + 1);

                            // Now find word to highlight
                            // Un-highlight everything first
                            txtCurrentSentence.SelectionStart = 0;
                            txtCurrentSentence.SelectionLength = txtCurrentSentence.TextLength;
                            txtCurrentSentence.SelectionColor = Color.Black;
                            txtCurrentSentence.SelectionFont = new Font(txtCurrentSentence.Font, FontStyle.Regular);

                            // if the calling textbox was a richtextbox, then set the original formatting
                            if (_callingTextBox is RichTextBox)
                            {
                                _originalRichTextBox.SelectionStart = (int)currentSentenceStartIndex;
                                _originalRichTextBox.SelectionLength = txtCurrentSentence.TextLength;

                                txtCurrentSentence.Rtf = _originalRichTextBox.SelectedRtf;
                            }

                            // Now highlight the new word and set the caret to the end of the word
                            txtCurrentSentence.SelectionStart = currentRange.First;
                            txtCurrentSentence.SelectionLength = currentRange.Length;
                            txtCurrentSentence.SelectionColor = Color.Red;
                            txtCurrentSentence.SelectionFont = new Font(txtCurrentSentence.Font, FontStyle.Bold);
                            txtCurrentSentence.SelectionLength = 0;
                            txtCurrentSentence.SelectionStart = currentRange.First + currentRange.Length + 1;

                            txtCurrentSentence.ZoomFactor = 1;

                            // Now set up the suggestions box
                            txtCurrentSentence.ZoomFactor = (int)_zoomFactor;
                            lboSuggestions.Items.Clear();

                            foreach (string st in _spellChecker.GetSuggestions(Strings.Mid(textBoxToUse.Text, currentRange.First + 1, currentRange.Length), 10))

                                lboSuggestions.Items.Add(st);

                            if (lboSuggestions.Items.Count > 0)
                            {
                                lboSuggestions.SelectedIndex = 0;
                            }
                            else
                            {
                                lboSuggestions.Items.Add("No suggestions found.");
                                lboSuggestions.Enabled = false;
                            }


                            currentWordRange = new CharacterRange(currentRange.First, currentRange.Length);
                            // originalTextBoxText = txtCurrentSentence.Text
                            DisableTextChanged = false;
                            return true;
                        }
                    }
                    else if (sentenceBreaks[1, i] > currentWordStart & sentenceBreaks[1, i - 1] < currentWordStart)
                    {
                        long startingPoint, sentenceLength;
                        startingPoint = sentenceBreaks[1, i - 1] + 1L;
                        // We add one to take us to the character directly after the previous sentence

                        // Now find the next letter or digit
                        for (long j = startingPoint, loopTo1 = textBoxToUse.TextLength; j <= loopTo1; j++)
                        {
                            if (char.IsLetterOrDigit(textBoxToUse.Text[(int) j]))
                            {
                                startingPoint = Conversions.ToLong(j);
                                break;
                            }
                        }

                        currentSentenceStartIndex = startingPoint;
                        sentenceLength = sentenceBreaks[1, i] - startingPoint + 1L;

                        // Now we add one to make it 1-based for the Mid function
                        startingPoint += 1L;

                        // Set the text of the textbox
                        txtCurrentSentence.Text = Strings.Mid(textBoxToUse.Text, (int) startingPoint, (int)sentenceLength);
                        currentSentence = i;

                        // Now we need to highlight the correct word
                        int wordStartInSent;
                        wordStartInSent =(int)(currentRange.First - (startingPoint - 1L)); // set starting point to 0-based

                        // Un-highlight everything first
                        txtCurrentSentence.SelectionStart = 0;
                        txtCurrentSentence.SelectionLength = txtCurrentSentence.TextLength;
                        txtCurrentSentence.SelectionColor = Color.Black;
                        txtCurrentSentence.SelectionFont = new Font(txtCurrentSentence.Font, FontStyle.Regular);

                        // if the calling textbox was a richtextbox, then set the original formatting
                        if (_callingTextBox is RichTextBox)
                        {
                            _originalRichTextBox.SelectionStart = (int)currentSentenceStartIndex;
                            _originalRichTextBox.SelectionLength = txtCurrentSentence.TextLength;

                            txtCurrentSentence.Rtf = _originalRichTextBox.SelectedRtf;
                        }

                        // Now highlight the new word
                        txtCurrentSentence.SelectionStart = wordStartInSent;
                        txtCurrentSentence.SelectionLength = currentRange.Length;
                        txtCurrentSentence.SelectionColor = Color.Red;
                        txtCurrentSentence.SelectionFont = new Font(txtCurrentSentence.Font, FontStyle.Bold);
                        txtCurrentSentence.SelectionLength = 0;
                        txtCurrentSentence.SelectionStart = currentRange.First + currentRange.Length + 1;

                        txtCurrentSentence.ZoomFactor = 1;

                        // Now set up the suggestions box
                        txtCurrentSentence.ZoomFactor = (int)_zoomFactor;
                        lboSuggestions.Items.Clear();

                        foreach (string st in _spellChecker.GetSuggestions(Strings.Mid(textBoxToUse.Text, currentRange.First + 1, currentRange.Length), 10))

                            lboSuggestions.Items.Add(st);

                        if (lboSuggestions.Items.Count > 0)
                        {
                            lboSuggestions.SelectedIndex = 0;
                        }
                        else
                        {
                            lboSuggestions.Items.Add("No suggestions found");
                            lboSuggestions.Enabled = false;
                        }

                        currentWordRange = new CharacterRange(wordStartInSent, currentRange.Length);
                        DisableTextChanged = false;
                        return true;
                    }
                }
            }
        }

        // If we get here, then there are no more misspelled words
        return false;
    }

    /// <summary>
    /// Returns the Rich Text Formatted text for RichTextBox controls.
    /// </summary>
    /// <returns></returns>
    /// <remarks></remarks>
    public string GetRTF()
    {
        if (_originalRichTextBox is not null)
        {
            return _originalRichTextBox.Rtf;
        }
        else
        {
            return "";
        }
    }

    #endregion

}