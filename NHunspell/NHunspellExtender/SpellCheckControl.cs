using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic; // Install-Package Microsoft.VisualBasic
using Microsoft.VisualBasic.CompilerServices; // Install-Package Microsoft.VisualBasic
using NHunspell;
using Utils;

/// <summary>
/// This class holds the text in the textbox, along with which words are spelling errors.
/// This class will also return the requested number of suggestions for misspelled words.
/// </summary>
/// <remarks></remarks>
public partial class SpellCheckControl
{

    #region Variables
    private string FullText;
    private string[,] _Text;
    public Hunspell myNHunspell = default;

    private string[] _spellingErrors;
    private CharacterRange[] _spellingErrorRanges;
    private bool _setTextCalledFirst;
    private CharacterRange[] _ignoreRange;
    private bool _dontResetIgnoreRanges;

#if USE_API_SERVICES
    public List<GrammarEdit> GrammarEdits { get; set; } = new List<GrammarEdit>();
#endif

    internal IManagedLockObject LockObject { get; set; }

    #endregion
    #region New
    public SpellCheckControl(ref Hunspell NHunspellObject)
    {
        _Text = new string[2, 0];
        _spellingErrors = new string[0];
        FullText = "";
        myNHunspell = NHunspellObject;
        _setTextCalledFirst = false;
#if USE_API_SERVICES
        GrammarEdits = new List<GrammarEdit>();
#endif
        _ignoreRange = new CharacterRange[0];
        _dontResetIgnoreRanges = false;
        LockObject = LockManager.CreateObject();
    }
#endregion
    #region Adding or Removing Text
    /// <summary>
    /// Adds a character directly after the selection start and checks the new word
    /// for spelling errors
    /// </summary>
    /// <param name="Input">The character to be added</param>
    /// <param name="SelectionStart">The position to add the character after</param>
    /// <remarks></remarks>
    public void AddText(string Input, long SelectionStart)
    {
        string originalWord;
        string newWord;
        var endOfWord = default(int);
        var beginning = default(int);
        bool resetSpellingRanges = false;

        // Sometimes, the setText gets called first.
        if (_setTextCalledFirst)
        {
            _setTextCalledFirst = false;
            return;
        }

        // If we are allowed to reset the ignore ranges, reset them
        if (!_dontResetIgnoreRanges)
        {
            _ignoreRange = new CharacterRange[0];
        }

        // Check if the input is a letter or digit and if not, see if it is splitting up
        // a previous word.  If not, we don't need to do anything further
        if (!char.IsLetterOrDigit(Conversions.ToChar(Input)))
        {
            // We're going to see if it's being added at the beginning or at the end.
            // If it is, then we don't need to do anything about it.
            // Then we're going to see if the character preceding it or following after
            // it is a non letter or digit.  If it is, then we don't need to go any further

            if (SelectionStart == 0L)
                goto SaveFullText;
            if (SelectionStart >= FullText.Length)
                goto SaveFullText;
            if (!char.IsLetterOrDigit(FullText[(int)(SelectionStart - 1L)]) | !char.IsLetterOrDigit(FullText[(int)SelectionStart]))
                goto SaveFullText;
        }

        // Now we need to figure out what the original word was and what the new word will be

        // Start with the case that we're at the beginning of the text
        if (SelectionStart == 0L)
        {
            // Now make sure there is a letter or digit currently at the beginning of the original text
            if (!char.IsLetterOrDigit(Conversions.ToChar(Strings.Left(FullText, 1))))
            {
                originalWord = "";
                newWord = Input;
            }
            else
            {
                // Find the end of the word that begins the FullText
                endOfWord = (int)FindLastLetterOrDigitFromPosition(SelectionStart);
                originalWord = Strings.Left(FullText, (int)(endOfWord - SelectionStart + 1L));
                newWord = Input + originalWord;
            }
        }

        else if (SelectionStart == FullText.Length)
        {
            // Now check if we're at the end
            // Make sure there is a letter or digit at the end of the original text
            if (!char.IsLetterOrDigit(Conversions.ToChar(Strings.Right(FullText, 1))))
            {
                originalWord = "";
                newWord = Input;
            }
            else
            {
                beginning = (int)FindFirstLetterOrDigitFromPosition(SelectionStart);
                originalWord = Strings.Right(FullText, (int)(SelectionStart - beginning));
                newWord = originalWord + Input;
            }
        }

        else // We're somewhere in the middle of the text
        {
            beginning = (int)FindFirstLetterOrDigitFromPosition(SelectionStart);
            endOfWord = (int)FindLastLetterOrDigitFromPosition(SelectionStart);

            // Example: "This" inserting after 'h'
            // SelectionStart = 2
            // beginning = 0
            // endOfWord = 3
            originalWord = Strings.Mid(FullText, beginning + 1, endOfWord - beginning + 1);

            // Check if the original word is actually a word
            // If there are not non letters or digits (like two spaces in a row)
            // and someone is putting a letter or digit between them, original word will be
            // one of the non letters or digits
            if (originalWord.Length == 1)
            {
                if (!char.IsLetterOrDigit(originalWord[0]))
                {
                    originalWord = "";
                }
            }

            if (string.IsNullOrEmpty(originalWord))
            {
                newWord = Input;
            }
            else
            {
                newWord = Strings.Mid(FullText, beginning + 1, (int)(SelectionStart - beginning)) + Input + Strings.Mid(FullText, (int)(SelectionStart + 1L), (int)(endOfWord - SelectionStart + 1L));

            }

        }

        // Check if the original word was added already (original word could be a space which would not
        // have been added)
        if (Information.UBound(_Text, 2) > -1)
        {

            if (originalWord.Length > 0)
            {

                for (int i = 0, loopTo = Information.UBound(_Text, 2); i <= loopTo; i++)
                {

                    if ((_Text[0, i] ?? "") == (originalWord ?? ""))
                    {
                        // Yay, we found it!

                        // Check if the original word is being split up
                        // into two words.  If we get here, and the char is not a digit
                        // or letter, then it is because we've already checked a non-letter
                        // or digit being added at the beginning or end of a word
                        if (!char.IsLetterOrDigit(Conversions.ToChar(Input)))
                        {

                            // we have two words now
                            string word1, word2;
                            word1 = Strings.Left(newWord, (int)(SelectionStart - beginning));
                            word2 = Strings.Right(newWord, (int)(endOfWord - SelectionStart + 1L));

                            // Replace the original word with the first new word
                            // Check if the original word is in the sentence more than once
                            if (Conversions.ToDouble(_Text[1, i]) > 1d)
                            {


                                // If it is, then we're subtracting one instance of the original
                                // word and adding a newword
                                _Text[1, i] = (Conversions.ToDouble(_Text[1, i]) - 1d).ToString();

                                // See if word1 was has already been added
                                bool foundWord1 = false;

                                for (int j = 0, loopTo1 = Information.UBound(_Text, 2); j <= loopTo1; j++)
                                {
                                    if ((_Text[0, j] ?? "") == (word1 ?? ""))
                                    {
                                        foundWord1 = true;
                                        _Text[1, j] = (Conversions.ToDouble(_Text[1, j]) + 1d).ToString();
                                        break;
                                    }
                                }

                                if (!foundWord1)
                                {
                                    var old_Text = _Text;
                                    _Text = new string[2, Information.UBound(_Text, 2) + 1 + 1];
                                    if (old_Text is not null)
                                        for (var i1 = 0; i1 <= old_Text.Length / old_Text.GetLength(1) - 1; ++i1)
                                            Array.Copy(old_Text, i1 * old_Text.GetLength(1), _Text, i1 * _Text.GetLength(1), Math.Min(old_Text.GetLength(1), _Text.GetLength(1)));
                                    _Text[0, Information.UBound(_Text, 2)] = word1;
                                    _Text[1, Information.UBound(_Text, 2)] = 1.ToString();
                                }
                            }


                            else
                            {
                                // If the original is only in once, we can either replace it, or remove it
                                // First wee need to see if word1 is in already
                                // See if word1 was has already been added
                                bool foundWord1 = false;

                                for (int j = 0, loopTo2 = Information.UBound(_Text, 2); j <= loopTo2; j++)
                                {
                                    if ((_Text[0, j] ?? "") == (word1 ?? ""))
                                    {
                                        foundWord1 = true;
                                        _Text[1, j] = (Conversions.ToDouble(_Text[1, j]) + 1d).ToString();
                                        break;
                                    }
                                }

                                if (!foundWord1)
                                {
                                    // If we didn't find it already in the array, just replace the
                                    // original with the new word
                                    _Text[0, i] = word1;
                                }
                                else // We did find word1 in the array, so remove the original from it
                                {
                                    for (int j = i + 1, loopTo3 = Information.UBound(_Text, 2); j <= loopTo3; j++)
                                    {
                                        _Text[0, j - 1] = _Text[0, j];
                                        _Text[1, j - 1] = _Text[1, j];
                                    }

                                    var old_Text1 = _Text;
                                    _Text = new string[2, (Information.UBound(_Text, 2))];
                                    if (old_Text1 is not null)
                                        for (var i2 = 0; i2 <= old_Text1.Length / old_Text1.GetLength(1) - 1; ++i2)
                                            Array.Copy(old_Text1, i2 * old_Text1.GetLength(1), _Text, i2 * _Text.GetLength(1), Math.Min(old_Text1.GetLength(1), _Text.GetLength(1)));
                                }

                                // See if the original word was a spelling error and remove it
                                for (int j = 0, loopTo4 = Information.UBound(_spellingErrors); j <= loopTo4; j++)
                                {
                                    if ((_spellingErrors[j] ?? "") == (originalWord ?? ""))
                                    {
                                        if (Information.UBound(_spellingErrors) > 0)
                                        {
                                            // If there is more than one entry in _spellingErrors
                                            // then move the entries above this one down one
                                            for (int k = j + 1, loopTo5 = Information.UBound(_spellingErrors); k <= loopTo5; k++)
                                                _spellingErrors[k - 1] = _spellingErrors[k];
                                        }
                                        Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors));

                                        resetSpellingRanges = true;
                                        break;
                                    }
                                }
                            }

                            // see if word2 has already been added
                            bool foundWord2 = false;
                            for (int j = 0, loopTo6 = Information.UBound(_Text, 2); j <= loopTo6; j++)
                            {
                                if ((_Text[0, j] ?? "") == (word2 ?? ""))
                                {
                                    foundWord2 = true;
                                    _Text[1, j] = (Conversions.ToDouble(_Text[1, j]) + 1d).ToString();
                                    break;
                                }
                            }

                            if (!foundWord2)
                            {
                                var old_Text2 = _Text;
                                _Text = new string[2, Information.UBound(_Text, 2) + 1 + 1];
                                if (old_Text2 is not null)
                                    for (var i3 = 0; i3 <= old_Text2.Length / old_Text2.GetLength(1) - 1; ++i3)
                                        Array.Copy(old_Text2, i3 * old_Text2.GetLength(1), _Text, i3 * _Text.GetLength(1), Math.Min(old_Text2.GetLength(1), _Text.GetLength(1)));
                                _Text[0, Information.UBound(_Text, 2)] = word2;
                                _Text[1, Information.UBound(_Text, 2)] = 1.ToString();
                            }

                            // Spell check both words
                            bool foundSpellingWord1 = false;
                            for (int j = 0, loopTo7 = Information.UBound(_spellingErrors); j <= loopTo7; j++)
                            {
                                if ((_spellingErrors[j] ?? "") == (word1 ?? ""))
                                {
                                    foundSpellingWord1 = true;
                                    break;
                                }
                            }

                            if (!myNHunspell.Spell(word1) & !foundSpellingWord1)
                            {
                                Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors) + 1 + 1);
                                _spellingErrors[Information.UBound(_spellingErrors)] = word1;
                                resetSpellingRanges = true;
                            }
                            else if (!myNHunspell.Spell(word1))
                            {
                                resetSpellingRanges = true;
                            }

                            bool foundSpellingWord2 = false;
                            for (int j = 0, loopTo8 = Information.UBound(_spellingErrors); j <= loopTo8; j++)
                            {
                                if ((_spellingErrors[j] ?? "") == (word2 ?? ""))
                                {
                                    foundSpellingWord2 = true;
                                    break;
                                }
                            }

                            if (!myNHunspell.Spell(word2) & !foundSpellingWord2)
                            {
                                Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors) + 1 + 1);
                                _spellingErrors[Information.UBound(_spellingErrors)] = word2;
                                resetSpellingRanges = true;
                            }
                            else if (!myNHunspell.Spell(word2))
                            {
                                resetSpellingRanges = true;
                            }

                            // We've handled everything, we can GoTo SaveFullText
                            goto SaveFullText;
                        }

                        // We get here if the original word is not being split into two
                        // so just replace the original word in the array with the new one
                        if (Conversions.ToDouble(_Text[1, i]) > 1d)
                        {
                            // If the original word is in the text more than once, subtract one
                            // instance of it
                            _Text[1, i] = (Conversions.ToDouble(_Text[1, i]) - 1d).ToString();

                            // See if the new word is in the array already
                            bool foundNewWord = false;

                            for (int j = 0, loopTo9 = Information.UBound(_Text, 2); j <= loopTo9; j++)
                            {
                                if ((_Text[0, j] ?? "") == (newWord ?? ""))
                                {
                                    _Text[1, j] = (Conversions.ToDouble(_Text[1, j]) + 1d).ToString();
                                    foundNewWord = true;
                                }
                            }

                            if (!foundNewWord)
                            {
                                // Add a new word to the array
                                var old_Text3 = _Text;
                                _Text = new string[2, Information.UBound(_Text, 2) + 1 + 1];
                                if (old_Text3 is not null)
                                    for (var i4 = 0; i4 <= old_Text3.Length / old_Text3.GetLength(1) - 1; ++i4)
                                        Array.Copy(old_Text3, i4 * old_Text3.GetLength(1), _Text, i4 * _Text.GetLength(1), Math.Min(old_Text3.GetLength(1), _Text.GetLength(1)));
                                _Text[0, Information.UBound(_Text, 2)] = newWord;
                                _Text[1, Information.UBound(_Text, 2)] = 1.ToString();

                                // Spell check it
                                bool foundSpellNewWord = false;
                                for (int j = 0, loopTo10 = Information.UBound(_spellingErrors); j <= loopTo10; j++)
                                {
                                    if ((_spellingErrors[j] ?? "") == (newWord ?? ""))
                                    {
                                        foundSpellNewWord = true;
                                        break;
                                    }
                                }

                                // Check if the new word is a spelling error
                                if (!myNHunspell.Spell(newWord) & !foundSpellNewWord)
                                {
                                    Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors) + 1 + 1);
                                    _spellingErrors[Information.UBound(_spellingErrors)] = newWord;
                                    resetSpellingRanges = true;
                                }
                                else if (!myNHunspell.Spell(newWord))
                                {
                                    resetSpellingRanges = true;
                                }
                            }

                            // We've handled everything, we can GoTo SaveFullText
                            goto SaveFullText;
                        }

                        else
                        {
                            // We get here if the original word is only in the text once and it's not
                            // being split into two words

                            // Check if the new word is already added
                            bool foundNewWord = false;

                            for (int j = 0, loopTo11 = Information.UBound(_Text, 2); j <= loopTo11; j++)
                            {
                                if ((_Text[0, j] ?? "") == (newWord ?? ""))
                                {
                                    _Text[1, j] = (Conversions.ToDouble(_Text[1, j]) + 1d).ToString();
                                    foundNewWord = true;
                                    break;
                                }
                            }

                            if (!foundNewWord)
                            {
                                // If the new word is not in the array, then we can just replace
                                // the original word with it
                                _Text[0, i] = newWord;

                                // Spell check the new word (just a double check)
                                bool foundNewWordinSpell = false;

                                for (int j = 0, loopTo12 = Information.UBound(_spellingErrors); j <= loopTo12; j++)
                                {
                                    if ((_spellingErrors[j] ?? "") == (newWord ?? ""))
                                    {
                                        foundNewWordinSpell = true;
                                        break;
                                    }
                                }

                                // Check if the new word is a spelling error
                                if (!myNHunspell.Spell(newWord) & !foundNewWordinSpell)
                                {
                                    Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors) + 1 + 1);
                                    _spellingErrors[Information.UBound(_spellingErrors)] = newWord;
                                    resetSpellingRanges = true;
                                }
                                else if (!myNHunspell.Spell(newWord))
                                {
                                    resetSpellingRanges = true;
                                }
                            }
                            else
                            {
                                // We did find the new word and we've already added one instance
                                // The only thing left is to remove the original word
                                for (int j = i + 1, loopTo13 = Information.UBound(_Text, 2); j <= loopTo13; j++)
                                {
                                    _Text[0, j - 1] = _Text[0, j];
                                    _Text[1, j - 1] = _Text[1, j];
                                }
                                var old_Text4 = _Text;
                                _Text = new string[2, (Information.UBound(_Text, 2))];
                                if (old_Text4 is not null)
                                    for (var i5 = 0; i5 <= old_Text4.Length / old_Text4.GetLength(1) - 1; ++i5)
                                        Array.Copy(old_Text4, i5 * old_Text4.GetLength(1), _Text, i5 * _Text.GetLength(1), Math.Min(old_Text4.GetLength(1), _Text.GetLength(1)));
                            }

                            // See if the original word was a spelling error and remove it
                            for (int j = 0, loopTo14 = Information.UBound(_spellingErrors); j <= loopTo14; j++)
                            {
                                if ((_spellingErrors[j] ?? "") == (originalWord ?? ""))
                                {
                                    if (Information.UBound(_spellingErrors) > 0)
                                    {
                                        // If there is more than one entry in _spellingErrors
                                        // then move the entries above this one down one
                                        for (int k = j + 1, loopTo15 = Information.UBound(_spellingErrors); k <= loopTo15; k++)
                                            _spellingErrors[k - 1] = _spellingErrors[k];
                                    }
                                    Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors));
                                    resetSpellingRanges = true;
                                    break;
                                }
                            }

                            if (!myNHunspell.Spell(newWord))
                                resetSpellingRanges = true;

                            // We've handled everything so we can GoTo SaveFullText
                            goto SaveFullText;
                        }
                    }
                }
            }

            // If we get past the Next, then something went wrong
            else
            {
                // If we get here, then original word is blank
                // See if the new word is in the array
                bool foundNewWord = false;

                for (int i = 0, loopTo16 = Information.UBound(_Text, 2); i <= loopTo16; i++)
                {
                    if ((_Text[0, i] ?? "") == (newWord ?? ""))
                    {
                        foundNewWord = true;
                        _Text[1, i] = (Conversions.ToDouble(_Text[1, i]) + 1d).ToString();
                        break;
                    }
                }

                if (!foundNewWord)
                {
                    var old_Text5 = _Text;
                    _Text = new string[2, Information.UBound(_Text, 2) + 1 + 1];
                    if (old_Text5 is not null)
                        for (var i = 0; i <= old_Text5.Length / old_Text5.GetLength(1) - 1; ++i)
                            Array.Copy(old_Text5, i * old_Text5.GetLength(1), _Text, i * _Text.GetLength(1), Math.Min(old_Text5.GetLength(1), _Text.GetLength(1)));
                    _Text[0, Information.UBound(_Text, 2)] = newWord;
                    _Text[1, Information.UBound(_Text, 2)] = 1.ToString();

                    // Check if the new word is a spelling error
                    if (!myNHunspell.Spell(newWord))
                    {
                        Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors) + 1 + 1);
                        _spellingErrors[Information.UBound(_spellingErrors)] = newWord;
                        resetSpellingRanges = true;
                    }
                }
            }
        }
        else
        {
            // If we get here, then there is nothing in the Text array yet
            _Text = new string[2, 1];
            _Text[0, 0] = newWord;
            _Text[1, 0] = 1.ToString();

            // Check if the new word is a spelling error
            if (!myNHunspell.Spell(newWord))
            {
                _spellingErrors = new string[1];
                _spellingErrors[0] = newWord;
                resetSpellingRanges = true;
            }
        }

    SaveFullText:

        // Save FullText
        if (SelectionStart == 0L)
        {
            // We're at the beginning of the text
            FullText = Input + FullText;
        }
        else if (SelectionStart == FullText.Length)
        {
            // We're at the end of the text
            FullText = FullText + Input;
        }
        else
        {
            // We're somewhere in the middle
            FullText = Strings.Left(FullText, (int)SelectionStart) + Input + Strings.Right(FullText, (int)(FullText.Length - SelectionStart));

        }

        // Reset the spelling error ranges
        if (resetSpellingRanges)
        {
            SetSpellingErrorRanges();
        }
    }


    /// <summary>
    /// Removes the character after the selection start (which is 0-based)
    /// </summary>
    /// <param name="SelectionStart">The position directly before the character to be removed</param>
    /// <remarks></remarks>
    public void RemoveText(int SelectionStart)
    {
        // Remove Text is going to function as delete key
        // If the position given to us is at the end, this won't work
        // Also, if there is nothing in fulltext, then there's nothing to delete
        if (SelectionStart == FullText.Length | FullText.Length == 0 | SelectionStart == -1)
            return;

        // Sometimes the SetText is called first
        if (_setTextCalledFirst == true)
        {
            _setTextCalledFirst = false;
            return;
        }

        // If we can reset the ignoreRanges, then do that
        if (!_dontResetIgnoreRanges)
        {
            _ignoreRange = new CharacterRange[0];
        }


        // If there is only one char in FullText, we can just reset it
        if (FullText.Length == 1)
        {
            FullText = "";
            _Text = new string[2, 0];
            _spellingErrors = new string[0];
            return;
        }

        string originalWord;
        string newWord;
        int endOfWord;
        int beginning;
        bool resetSpellingRanges = false;

        // Check if we're deleting a non letter or digit
        if (!char.IsLetterOrDigit(FullText[SelectionStart]))
        {


            // see if there are no letters or digits around it...if so, we just
            // update fulltext and move on

            // Check if at the end
            if (SelectionStart == FullText.Length - 1 & !char.IsLetterOrDigit(FullText[SelectionStart]))
            {
                goto SaveFullText;
            }


            else if (SelectionStart == 0)
            {
                // We're at the beginning
                goto SaveFullText;
            }


            else
            {
                // We're in the middle

                // Check the char on either side...if one of them is a non letter or digit
                // then we can also just save the full text.  We're not combining two words
                // into one,
                // Example: "This is. A" deleting the period
                if (!char.IsLetterOrDigit(FullText[SelectionStart - 1]) | !char.IsLetterOrDigit(FullText[SelectionStart + 1]))
                {
                    goto SaveFullText;
                }


                else
                {
                    // If both of the char on either side are letters or digits, then
                    // we're merging two words into one...this is a special case where
                    // we will need to delete both of the original words and create a new one

                    // We need to get the two original words
                    // Example "this tle" deleting the space
                    // SelectionStart = 4
                    // firstWord beginning = 0
                    // firstWord end = 3
                    // secondWord beginning = 5
                    // secondWord end = 7

                    string firstWord, secondWord;

                    // Get the first word (to do this, use SelectionStart -1)
                    beginning = (int)FindFirstLetterOrDigitFromPosition(SelectionStart - 1);
                    firstWord = Strings.Mid(FullText, beginning + 1, SelectionStart - beginning);

                    // Get the second word (to do this, use Selection Start +1)
                    endOfWord = (int)FindLastLetterOrDigitFromPosition(SelectionStart + 1);
                    secondWord = Strings.Mid(FullText, SelectionStart + 2, endOfWord - SelectionStart);

                    // The new word is just firstWord & secondWord
                    newWord = firstWord + secondWord;

                    // Find the first word and remove one instance of it
                    for (int i = 0, loopTo = Information.UBound(_Text, 2); i <= loopTo; i++)
                    {


                        if ((_Text[0, i] ?? "") == (firstWord ?? "") & Conversions.ToDouble(_Text[1, i]) == 1d)
                        {
                            // Remove the word from Text and _spellingErrors
                            for (int j = i + 1, loopTo1 = Information.UBound(_Text, 2); j <= loopTo1; j++)
                            {
                                _Text[0, j - 1] = _Text[0, j];
                                _Text[1, j - 1] = _Text[1, j];
                            }

                            var old_Text = _Text;
                            _Text = new string[2, (Information.UBound(_Text, 2))];
                            if (old_Text is not null)
                                for (var i1 = 0; i1 <= old_Text.Length / old_Text.GetLength(1) - 1; ++i1)
                                    Array.Copy(old_Text, i1 * old_Text.GetLength(1), _Text, i1 * _Text.GetLength(1), Math.Min(old_Text.GetLength(1), _Text.GetLength(1)));

                            // now remove it from the spell check
                            for (int j = 0, loopTo2 = Information.UBound(_spellingErrors); j <= loopTo2; j++)
                            {
                                if (Information.UBound(_spellingErrors) > 0)
                                {
                                    for (int k = j + 1, loopTo3 = Information.UBound(_spellingErrors); k <= loopTo3; k++)
                                        _spellingErrors[k - 1] = _spellingErrors[k];
                                }
                                Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors));
                                resetSpellingRanges = true;
                                break;
                            }

                            break;
                        }


                        else if ((_Text[0, i] ?? "") == (firstWord ?? "") & Conversions.ToDouble(_Text[1, i]) == 1d)
                        {
                            // just remove an instance of the word
                            _Text[1, i] = (Conversions.ToDouble(_Text[1, i]) - 1d).ToString();
                            break;
                        }
                    }



                    // Find the second word and remove one instance of it
                    for (int i = 0, loopTo4 = Information.UBound(_Text, 2); i <= loopTo4; i++)
                    {


                        if ((_Text[0, i] ?? "") == (secondWord ?? "") & Conversions.ToDouble(_Text[1, i]) == 1d)
                        {
                            // Remove the word from Text and _spellingErrors
                            for (int j = i + 1, loopTo5 = Information.UBound(_Text, 2); j <= loopTo5; j++)
                            {
                                _Text[0, j - 1] = _Text[0, j];
                                _Text[1, j - 1] = _Text[1, j];
                            }

                            var old_Text1 = _Text;
                            _Text = new string[2, (Information.UBound(_Text, 2))];
                            if (old_Text1 is not null)
                                for (var i2 = 0; i2 <= old_Text1.Length / old_Text1.GetLength(1) - 1; ++i2)
                                    Array.Copy(old_Text1, i2 * old_Text1.GetLength(1), _Text, i2 * _Text.GetLength(1), Math.Min(old_Text1.GetLength(1), _Text.GetLength(1)));

                            // now remove it from the spell check
                            for (int j = 0, loopTo6 = Information.UBound(_spellingErrors); j <= loopTo6; j++)
                            {
                                if (Information.UBound(_spellingErrors) > 0)
                                {
                                    for (int k = j + 1, loopTo7 = Information.UBound(_spellingErrors); k <= loopTo7; k++)
                                        _spellingErrors[k - 1] = _spellingErrors[k];
                                }
                                Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors));
                                resetSpellingRanges = true;
                                break;
                            }

                            break;
                        }


                        else if ((_Text[0, i] ?? "") == (firstWord ?? "") & Conversions.ToDouble(_Text[1, i]) == 1d)
                        {
                            // just remove an instance of the word
                            _Text[1, i] = (Conversions.ToDouble(_Text[1, i]) - 1d).ToString();
                            break;
                        }


                    }

                    // Now add the new word
                    var old_Text2 = _Text;
                    _Text = new string[2, Information.UBound(_Text, 2) + 1 + 1];
                    if (old_Text2 is not null)
                        for (var i = 0; i <= old_Text2.Length / old_Text2.GetLength(1) - 1; ++i)
                            Array.Copy(old_Text2, i * old_Text2.GetLength(1), _Text, i * _Text.GetLength(1), Math.Min(old_Text2.GetLength(1), _Text.GetLength(1)));
                    _Text[0, Information.UBound(_Text, 2)] = newWord;
                    _Text[1, Information.UBound(_Text, 2)] = 1.ToString();

                    // Now spell check it
                    if (!myNHunspell.Spell(newWord))
                    {
                        Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors) + 1 + 1);
                        _spellingErrors[Information.UBound(_spellingErrors)] = newWord;
                        resetSpellingRanges = true;
                    }
                }

                goto SaveFullText;
            }

        }



        // Now we need to figure out what the original word was and what the new word will be

        // Start with the case that we're at the beginning of the text
        if (SelectionStart == 0)
        {


            // Now make sure there is a letter or digit currently at the beginning of the text
            if (!char.IsLetterOrDigit(Conversions.ToChar(Strings.Left(FullText, 1))))
            {
                // Example: " This" deleting the leading whitespace
                originalWord = "";
                newWord = "";
            }
            else
            {
                // Example: "This" deleting T
                // SelectionStart = 0
                // endOfWord = 3

                // Find the end of the word that begins the FullText
                endOfWord = (int)FindLastLetterOrDigitFromPosition(SelectionStart);
                originalWord = Strings.Left(FullText, endOfWord - SelectionStart + 1);
                newWord = Strings.Right(originalWord, originalWord.Length - 1);
            }
        }


        else if (SelectionStart == FullText.Length - 1)
        {


            // Now check if we're at the end
            // Make sure there is a letter or digit at the end of the text
            if (!char.IsLetterOrDigit(Conversions.ToChar(Strings.Right(FullText, 1))))
            {
                // Example: "This " deleting preceding white space
                originalWord = "";
                newWord = "";
            }
            else
            {
                // Example: "This" deleting s
                // SelectionStart = 3
                // beginning = 0

                beginning = (int)FindFirstLetterOrDigitFromPosition(SelectionStart);
                originalWord = Strings.Right(FullText, SelectionStart + 1 - beginning);
                newWord = Strings.Left(originalWord, originalWord.Length - 1);
            }
        }


        else // We're somewhere in the middle of the text
        {


            beginning = (int)FindFirstLetterOrDigitFromPosition(SelectionStart);
            endOfWord = (int)FindLastLetterOrDigitFromPosition(SelectionStart);

            // Example: "This" deleting i         "This will" deleting s
            // SelectionStart = 2                 SelectionStart = 3
            // beginning = 0                      beginning = 0
            // endOfWord = 3                      endOfWord = 3
            originalWord = Strings.Mid(FullText, beginning + 1, endOfWord - beginning + 1);

            newWord = Strings.Mid(FullText, beginning + 1, SelectionStart - beginning) + Strings.Mid(FullText, SelectionStart + 2, endOfWord - SelectionStart);
        }



        if (Information.UBound(_Text, 2) > -1)
        {
            for (int i = 0, loopTo8 = Information.UBound(_Text, 2); i <= loopTo8; i++)
            {
                if ((_Text[0, i] ?? "") == (originalWord ?? "") & Conversions.ToDouble(_Text[1, i]) == 1d)
                {
                    // Make sure there is a new word and we weren't deleting a single char word
                    if (newWord.Length > 0)
                    {
                        // Check if the word already exists
                        bool foundNewWord = false;

                        for (int j = 0, loopTo9 = Information.UBound(_Text, 2); j <= loopTo9; j++)
                        {
                            if ((_Text[0, j] ?? "") == (newWord ?? ""))
                            {
                                foundNewWord = true;
                                _Text[1, j] = (Conversions.ToDouble(_Text[1, j]) + 1d).ToString();

                                // we can also delete the original word
                                // See if the original word was a spelling error and remove it
                                for (int l = 0, loopTo10 = Information.UBound(_spellingErrors); l <= loopTo10; l++)
                                {
                                    if ((_spellingErrors[l] ?? "") == (originalWord ?? ""))
                                    {
                                        if (Information.UBound(_spellingErrors) > 0)
                                        {
                                            for (int k = l + 1, loopTo11 = Information.UBound(_spellingErrors); k <= loopTo11; k++)
                                                _spellingErrors[k - 1] = _spellingErrors[k];

                                            Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors));
                                            resetSpellingRanges = true;
                                        }
                                        else
                                        {
                                            _spellingErrors = new string[0];
                                            resetSpellingRanges = true;
                                        }
                                        break;
                                    }
                                }

                                // Move all entries in array after this down one
                                for (int l = i + 1, loopTo12 = Information.UBound(_Text, 2); l <= loopTo12; l++)
                                {
                                    _Text[0, l - 1] = _Text[0, l];
                                    _Text[1, l - 1] = _Text[1, l];
                                }

                                var old_Text3 = _Text;
                                _Text = new string[2, (Information.UBound(_Text, 2))];
                                if (old_Text3 is not null)
                                    for (var i3 = 0; i3 <= old_Text3.Length / old_Text3.GetLength(1) - 1; ++i3)
                                        Array.Copy(old_Text3, i3 * old_Text3.GetLength(1), _Text, i3 * _Text.GetLength(1), Math.Min(old_Text3.GetLength(1), _Text.GetLength(1)));

                                break;
                            }
                        }

                        if (!foundNewWord)
                        {
                            // replace the originalword with the newword
                            _Text[0, i] = newWord;

                            // See if the original word was a spelling error and remove it
                            for (int l = 0, loopTo13 = Information.UBound(_spellingErrors); l <= loopTo13; l++)
                            {
                                if ((_spellingErrors[l] ?? "") == (originalWord ?? ""))
                                {
                                    if (Information.UBound(_spellingErrors) > 0)
                                    {
                                        for (int k = l + 1, loopTo14 = Information.UBound(_spellingErrors); k <= loopTo14; k++)
                                            _spellingErrors[k - 1] = _spellingErrors[k];

                                        Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors));
                                        resetSpellingRanges = true;
                                    }
                                    else
                                    {
                                        _spellingErrors = new string[0];
                                        resetSpellingRanges = true;
                                    }
                                    break;
                                }
                            }

                            // Spell check the new word
                            bool foundSpellNewWord = false;

                            for (int j = 0, loopTo15 = Information.UBound(_spellingErrors); j <= loopTo15; j++)
                            {
                                if ((_spellingErrors[j] ?? "") == (newWord ?? ""))
                                {
                                    foundSpellNewWord = true;
                                    break;
                                }
                            }

                            if (!foundSpellNewWord & !myNHunspell.Spell(newWord))
                            {
                                Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors) + 1 + 1);
                                _spellingErrors[Information.UBound(_spellingErrors)] = newWord;
                                resetSpellingRanges = true;
                            }
                        }

                        break;
                    }


                    else
                    {
                        // There is no newWord...just delete the original word
                        for (int j = i + 1, loopTo16 = Information.UBound(_Text, 2); j <= loopTo16; j++)
                        {
                            _Text[0, j - 1] = _Text[0, j];
                            _Text[1, j - 1] = _Text[1, j];
                        }

                        var old_Text4 = _Text;
                        _Text = new string[2, (Information.UBound(_Text, 2))];
                        if (old_Text4 is not null)
                            for (var i4 = 0; i4 <= old_Text4.Length / old_Text4.GetLength(1) - 1; ++i4)
                                Array.Copy(old_Text4, i4 * old_Text4.GetLength(1), _Text, i4 * _Text.GetLength(1), Math.Min(old_Text4.GetLength(1), _Text.GetLength(1)));

                        // See if the original was a spelling error
                        for (int j = 0, loopTo17 = Information.UBound(_spellingErrors); j <= loopTo17; j++)
                        {
                            if ((_spellingErrors[j] ?? "") == (originalWord ?? ""))
                            {
                                if (Information.UBound(_spellingErrors) > 0)
                                {
                                    for (int k = j + 1, loopTo18 = Information.UBound(_spellingErrors); k <= loopTo18; k++)
                                        _spellingErrors[k - 1] = _spellingErrors[k];

                                    Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors));
                                    resetSpellingRanges = true;
                                }
                                else
                                {
                                    _spellingErrors = new string[0];
                                    resetSpellingRanges = true;
                                }
                                break;
                            }
                        }

                        break;


                    }
                }


                else if ((_Text[0, i] ?? "") == (originalWord ?? "") & Conversions.ToDouble(_Text[1, i]) > 1d)
                {
                    // Reduce the number of duplicate entries by one
                    _Text[1, i] = (Conversions.ToDouble(_Text[1, i]) - 1d).ToString();

                    // see if the new word is an already added word
                    bool FoundNewWord = false;

                    for (int j = 0, loopTo19 = Information.UBound(_Text, 2); j <= loopTo19; j++)
                    {
                        if ((_Text[0, j] ?? "") == (newWord ?? ""))
                        {
                            FoundNewWord = true;
                            _Text[1, j] = (Conversions.ToDouble(_Text[1, j]) + 1d).ToString();
                            break;
                        }
                    }

                    if (!FoundNewWord)
                    {
                        // Make sure there is a new word and we weren't deleting a single char word
                        if (newWord.Length > 0)
                        {
                            var old_Text5 = _Text;
                            _Text = new string[2, Information.UBound(_Text, 2) + 1 + 1];
                            if (old_Text5 is not null)
                                for (var i5 = 0; i5 <= old_Text5.Length / old_Text5.GetLength(1) - 1; ++i5)
                                    Array.Copy(old_Text5, i5 * old_Text5.GetLength(1), _Text, i5 * _Text.GetLength(1), Math.Min(old_Text5.GetLength(1), _Text.GetLength(1)));
                            _Text[0, Information.UBound(_Text, 2)] = newWord;
                            _Text[1, Information.UBound(_Text, 2)] = 1.ToString();

                            // Spell check newWord
                            FoundNewWord = false;

                            for (int j = 0, loopTo20 = Information.UBound(_spellingErrors); j <= loopTo20; j++)
                            {
                                if ((_spellingErrors[j] ?? "") == (newWord ?? ""))
                                {
                                    FoundNewWord = true;
                                    break;
                                }
                            }

                            if (!FoundNewWord)
                            {
                                if (!myNHunspell.Spell(newWord))
                                {
                                    _spellingErrors = new string[Information.UBound(_spellingErrors) + 1 + 1];
                                    _spellingErrors[Information.UBound(_spellingErrors)] = newWord;
                                    resetSpellingRanges = true;
                                }
                            }
                            break;
                        }


                        else
                        {
                            // If there is no new word, then move any word after it down the array and
                            // resize the array
                            for (int j = i + 1, loopTo21 = Information.UBound(_Text, 2); j >= loopTo21; j -= 1)
                            {
                                _Text[0, j - 1] = _Text[0, j];
                                _Text[1, j - 1] = _Text[1, j];
                            }
                            var old_Text6 = _Text;
                            _Text = new string[2, (Information.UBound(_Text, 2))];
                            if (old_Text6 is not null)
                                for (var i6 = 0; i6 <= old_Text6.Length / old_Text6.GetLength(1) - 1; ++i6)
                                    Array.Copy(old_Text6, i6 * old_Text6.GetLength(1), _Text, i6 * _Text.GetLength(1), Math.Min(old_Text6.GetLength(1), _Text.GetLength(1)));
                            break;
                        }


                    }


                }


            }


        }

    SaveFullText:
        ;

        // Save FullText
        if (SelectionStart == 0)
        {
            // we're at the beginning
            FullText = Strings.Right(FullText, FullText.Length - 1);
        }
        else if (SelectionStart == FullText.Length - 1)
        {
            // Deleting the last character
            FullText = Strings.Left(FullText, FullText.Length - 1);
        }
        else
        {
            // Deleting somewhere in the middle
            FullText = Strings.Left(FullText, SelectionStart) + Strings.Right(FullText, FullText.Length - SelectionStart - 1);
        }

        // Reset the spelling error ranges
        if (resetSpellingRanges)
        {
            SetSpellingErrorRanges();
        }
    }


    /// <summary>
    /// Parse the input string into its separate words
    /// </summary>
    /// <param name="Input"></param>
    /// <remarks></remarks>
    public void SetText(string Input)
    {
        // If we have already handled this with the keypress or keydown events
        // This will allow for the text to change based on non-user input
        if ((FullText ?? "") == (Input ?? ""))
            return;

        // If we can reset the ignore ranges, then do that
        if (!_dontResetIgnoreRanges)
        {
            _ignoreRange = new CharacterRange[0];
        }

        _setTextCalledFirst = true;

        // The idea here is that we need to know the start of a new word, and if the last letter
        // was part of another word.  wordStarted is used to determine if we have already had
        // a letter or digit preceding the current char.
        int wordStart = 1;
        bool wordStarted = false;
        _Text = new string[2, 0];
        _spellingErrors = new string[0];
        _spellingErrorRanges = new CharacterRange[0];

        // set FullText
        FullText = Input;
        bool resetSpellingRanges = false;


        // Go through every char in the textbox one by one
        for (int i = 1, loopTo = Input.Length; i <= loopTo; i++)
        {


            if (!char.IsLetterOrDigit(Conversions.ToChar(Strings.Mid(Input, i, 1))) & wordStarted == true)
            {
                // We know it's not a letter or digit so it could be the end of a word


                // Check if it's an apostrophe or hyphen, if it is, it's not the end of a word
                if ((Strings.Mid(Input, i, 1) == "'" | Strings.Mid(Input, i, 1) == "-") & i != Input.Length)
                {
                    if (char.IsLetterOrDigit(Conversions.ToChar(Strings.Mid(Input, i + 1, 1))))
                    {
                        // is an apostrophe or hyphen, then we just go to the next character
                        goto FoundApostrophe;
                    }
                }

                // Check if we think this is the beginning of a word.  If wordStart = i, then
                // we're possibly at the beginning of a word
                if (wordStart != i)
                {
                    wordStarted = false;

                    // Now see if the word has already been added (we're not adding words
                    // more than once.  This way we only have to spell check each word once)
                    bool boolFound = false;

                    for (int j = 0, loopTo1 = Information.UBound(_Text, 2); j <= loopTo1; j++)
                    {
                        if ((_Text[0, j] ?? "") == (Strings.Trim(Strings.Mid(Input, wordStart, i - wordStart)) ?? ""))
                        {
                            boolFound = true;
                            _Text[1, j] = (Conversions.ToDouble(_Text[1, j]) + 1d).ToString();
                            break;
                        }
                    }


                    // If the word hasn't been added, add it and then spell check it
                    if (!boolFound)
                    {
                        var old_Text = _Text;
                        _Text = new string[2, Information.UBound(_Text, 2) + 1 + 1];
                        if (old_Text is not null)
                            for (var i1 = 0; i1 <= old_Text.Length / old_Text.GetLength(1) - 1; ++i1)
                                Array.Copy(old_Text, i1 * old_Text.GetLength(1), _Text, i1 * _Text.GetLength(1), Math.Min(old_Text.GetLength(1), _Text.GetLength(1)));
                        _Text[0, Information.UBound(_Text, 2)] = Strings.Trim(Strings.Mid(Input, wordStart, i - wordStart));
                        _Text[1, Information.UBound(_Text, 2)] = 1.ToString();

                        // Spell check it
                        bool foundWord = false;

                        for (int j = 0, loopTo2 = Information.UBound(_spellingErrors); j <= loopTo2; j++)
                        {
                            if ((_spellingErrors[j] ?? "") == (Strings.Trim(Strings.Mid(Input, wordStart, i - wordStart)) ?? ""))
                            {
                                foundWord = true;
                                break;
                            }
                        }

                        if (!myNHunspell.Spell(_Text[0, Information.UBound(_Text, 2)]) & !foundWord)
                        {
                            Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors) + 1 + 1);
                            _spellingErrors[Information.UBound(_spellingErrors)] = _Text[0, Information.UBound(_Text, 2)];
                            resetSpellingRanges = true;
                        }
                    }
                    wordStart = i + 1;
                }
            }
            else if (char.IsLetterOrDigit(Conversions.ToChar(Strings.Mid(Input, i, 1))))
            {
                if (!wordStarted)
                    wordStart = i;
                wordStarted = true;
            }

        FoundApostrophe:
            ;

        }



        // We have to check the last character separately or the last word won't be added
        if (!char.IsLetterOrDigit(Conversions.ToChar(Strings.Right(Input, 1))))
        {
            goto ResetSpelling;
        }

        // Check the last word and see it is had been added
        bool boolFound2 = false;

        for (int j = 0, loopTo3 = Information.UBound(_Text, 2); j <= loopTo3; j++)
        {
            if ((_Text[0, j] ?? "") == (Strings.Trim(Strings.Mid(Input, wordStart, Input.Length - wordStart + 1)) ?? ""))
            {
                boolFound2 = true;
                _Text[1, j] = (Conversions.ToDouble(_Text[1, j]) + 1d).ToString();
                break;
            }
        }

        // If it hasn't been added, add it and spell check it
        if (!boolFound2)
        {
            var old_Text1 = _Text;
            _Text = new string[2, Information.UBound(_Text, 2) + 1 + 1];
            if (old_Text1 is not null)
                for (var i = 0; i <= old_Text1.Length / old_Text1.GetLength(1) - 1; ++i)
                    Array.Copy(old_Text1, i * old_Text1.GetLength(1), _Text, i * _Text.GetLength(1), Math.Min(old_Text1.GetLength(1), _Text.GetLength(1)));
            _Text[0, Information.UBound(_Text, 2)] = Strings.Trim(Strings.Mid(Input, wordStart, Input.Length - wordStart + 1));
            _Text[1, Information.UBound(_Text, 2)] = 1.ToString();

            // Spell check it
            bool foundWord = false;

            for (int j = 0, loopTo4 = Information.UBound(_spellingErrors); j <= loopTo4; j++)
            {
                if ((_spellingErrors[j] ?? "") == (_Text[0, Information.UBound(_Text, 2)] ?? ""))
                {
                    foundWord = true;
                    break;
                }
            }

            if (!myNHunspell.Spell(_Text[0, Information.UBound(_Text, 2)]) & !foundWord)
            {
                Array.Resize(ref _spellingErrors, Information.UBound(_spellingErrors) + 1 + 1);
                _spellingErrors[Information.UBound(_spellingErrors)] = _Text[0, Information.UBound(_Text, 2)];
                resetSpellingRanges = true;
            }
        }

    ResetSpelling:
        ;

        if (resetSpellingRanges)
        {
            SetSpellingErrorRanges();
        }
    }


    #endregion
    #region FindPositions
    /// <summary>
    /// Given a starting point, we're looking at the characters before it to find the
    /// position of the first character in the word containing the starting point
    /// </summary>
    /// <param name="SelectionStart">0-based starting point</param>
    /// <returns>0-based index of the first character in the word</returns>
    /// <remarks></remarks>
    private long FindFirstLetterOrDigitFromPosition(long SelectionStart)
    {
        for (long i = SelectionStart - 1L; i >= 0L; i += -1)
        {
            if (!char.IsLetterOrDigit(FullText[(int)i]))
            {
                // Need to check if it's an apostrophe or hyphen
                bool foundApOrHyph = false;

                if ((Conversions.ToString(FullText[(int)i]) == "'" | Conversions.ToString(FullText[(int)i]) == "-") & i != 0L)
                {
                    if (char.IsLetterOrDigit(FullText[(int)(i - 1L)]))
                    {
                        foundApOrHyph = true;
                    }
                }

                if (!foundApOrHyph)
                {
                    return i + 1L;
                }
            }
        }

        return 0L;
    }


    /// <summary>
    /// Given a starting position, this will return the 0-based index representing
    /// the end of a word containing the character at the starting position
    /// </summary>
    /// <param name="SelectionStart">0-based index</param>
    /// <returns>0-based index of the last character in the word</returns>
    /// <remarks></remarks>
    private long FindLastLetterOrDigitFromPosition(long SelectionStart)
    {
        // Character array is 0 based in this function
        for (long i = SelectionStart, loopTo = FullText.Length - 1; i <= loopTo; i++)
        {
            if (!char.IsLetterOrDigit(FullText[(int)i]))
            {
                // Need to check if it's an apostrophe or hyphen 
                bool foundApOrHyph = false;

                if ((Conversions.ToString(FullText[(int)i]) == "'" | Conversions.ToString(FullText[(int)i]) == "-") & i != FullText.Length)
                {
                    if (char.IsLetterOrDigit(FullText[(int)(i + 1L)]))
                    {
                        foundApOrHyph = true;
                    }
                }

                if (!foundApOrHyph)
                {
                    // We found the character after the end of the last word
                    return i - 1L;
                }
            }
        }

        return FullText.Length - 1;
    }


    #endregion


    #region Spelling Functions and Subs
    /// <summary>
    /// Add the range of a word to ignore.
    /// </summary>
    /// <param name="IgnoreRange"></param>
    /// <remarks></remarks>
    public void AddRangeToIgnore(CharacterRange IgnoreRange)
    {
        Array.Resize(ref _ignoreRange, Information.UBound(_ignoreRange) + 1 + 1);
        _ignoreRange[Information.UBound(_ignoreRange)] = IgnoreRange;
    }


    public void ClearIgnoreRanges()
    {
        _ignoreRange = new CharacterRange[0];
    }


    public void DontResetIgnoreRanges(bool DontReset = true)
    {
        _dontResetIgnoreRanges = DontReset;
    }


    public CharacterRange[] GetIgnoreRanges()
    {
        return _ignoreRange;
    }


    /// <summary>
    /// Returns the ranges of characters associated with misspelled words.
    /// This is used by the CustomPaint to know where to paint the squiggly lines
    /// </summary>
    /// <returns>CharacterRange</returns>
    /// <remarks></remarks>
    public CharacterRange[] GetSpellingErrorRanges()
    {
        return _spellingErrorRanges;
    }


    /// <summary>
    /// Return the words that are spelling errors
    /// </summary>
    /// <returns>Array of strings</returns>
    /// <remarks></remarks>
    public string[] GetSpellingErrors()
    {
        return _spellingErrors;
    }


    /// <summary>
    /// Returns the requested number of suggestions based on the inputted word
    /// </summary>
    /// <param name="Word">Word we need suggestions for</param>
    /// <param name="NumberOfSuggestions">How many suggestions to return</param>
    /// <returns>Array of strings with the suggestions</returns>
    /// <remarks></remarks>
    public string[] GetSuggestions(string Word, int NumberOfSuggestions)
    {
        string[] suggestions;
        var NHunspellSugg = new List<string>();
        suggestions = new string[0];
        NHunspellSugg = myNHunspell.Suggest(Word);

        for (int i = 0, loopTo = NumberOfSuggestions - 1; i <= loopTo; i++)
        {
            if (i < NHunspellSugg.Count)
            {
                Array.Resize(ref suggestions, Information.UBound(suggestions) + 1 + 1);
                suggestions[Information.UBound(suggestions)] = NHunspellSugg[i];
            }
        }

        return suggestions;
    }


    /// <summary>
    /// Given a 0-based char index, returns the misspelled word that that character is part of
    /// </summary>
    /// <param name="CharIndex">0-based Index</param>
    /// <returns>Strings.String Type</returns>
    /// <remarks></remarks>
    public string GetMisspelledWordAtPosition(int CharIndex)
    {

        foreach (var currentRange in _spellingErrorRanges)
        {
            if (CharIndex >= currentRange.First & CharIndex <= currentRange.First + currentRange.Length + 1)
            {
                return Strings.Mid(FullText, currentRange.First + 1, currentRange.Length);
            }
        }

        return "";
    }


    /// <summary>
    /// Returns whether or not the text has any spelling errors
    /// </summary>
    /// <returns>A boolean representing whether there are spelling errors</returns>
    /// <remarks></remarks>
    public bool HasSpellingErrors()
    {
        return Information.UBound(_spellingErrors) > -1;
    }

#if USE_API_SERVICES
    public bool HasGrammarErrors()
    {
        return GrammarEdits.Count > 0;
    }
#endif

    /// <summary>
    /// Given a 0-based character index, returns whether the item is part of a misspelled word
    /// </summary>
    /// <param name="CharIndex">0-based index</param>
    /// <returns>Boolean</returns>
    /// <remarks></remarks>
    public bool IsPartOfSpellingError(int CharIndex)
    {
        CharacterRange currentRange;
        bool result = false;

        if (_spellingErrorRanges is null)
        {
            return false;
        }

        foreach (var currentCurrentRange in _spellingErrorRanges)
        {
            currentRange = currentCurrentRange;
            if (CharIndex >= currentRange.First & CharIndex <= currentRange.First + currentRange.Length + 1)
            {
                result = true;
                break;
            }
        }

        if (result)
        {
            // we need to check if it's part of an ignore range
            foreach (var currentCurrentRange1 in _ignoreRange)
            {
                currentRange = currentCurrentRange1;
                if (CharIndex >= currentRange.First & CharIndex <= currentRange.First + currentRange.Length + 1)
                {
                    result = false;
                    break;
                }
            }
        }

        return result;
    }

#if USE_API_SERVICES
    public bool IsPartOfGrammarError(int CharIndex)
    {
        foreach (var grammarEdit in GrammarEdits)
        {
            if (CharIndex.IsBetween(grammarEdit.StartGrammar, grammarEdit.EndGrammar))
            {
                return true;
            }
        }

        return false;
    }
#endif

    /// <summary>
    /// Sets the character ranges of the spelling errors
    /// </summary>
    /// <remarks></remarks>
    public void SetSpellingErrorRanges()
    {
        _spellingErrorRanges = new CharacterRange[0];

        if (!HasSpellingErrors())
            return;

        // The idea here is that we need to know the start of a new word, and if the last letter
        // was part of another word
        int wordStart = 1;
        bool wordStarted = false;

        // Go through every char in FullText one by one
        for (int i = 1, loopTo = FullText.Length; i <= loopTo; i++)
        {
            if (!char.IsLetterOrDigit(Conversions.ToChar(Strings.Mid(FullText, i, 1))))
            {
                // We know it's not a letter or digit so it could be the end of a word

                // Check if it's an apostrophe or hyphen, if it is, it's not the end of a word
                if ((Strings.Mid(FullText, i, 1) == "'" | Strings.Mid(FullText, i, 1) == "-") & i != FullText.Length)
                {
                    if (char.IsLetterOrDigit(Conversions.ToChar(Strings.Mid(FullText, i + 1, 1))))
                    {
                        // is an apostrophe or hyphen
                        goto FoundApostrophe;
                    }
                }

                // Check if we think this is the beginning of a word
                if (wordStart != i)
                {
                    wordStarted = false;

                    string currentWord = Strings.Trim(Strings.Mid(FullText, wordStart, i - wordStart));

                    // Spell check it
                    for (int j = 0, loopTo1 = Information.UBound(_spellingErrors); j <= loopTo1; j++)
                    {
                        if ((_spellingErrors[j] ?? "") == (currentWord ?? ""))
                        {
                            // Add it to the array
                            Array.Resize(ref _spellingErrorRanges, Information.UBound(_spellingErrorRanges) + 1 + 1);
                            _spellingErrorRanges[Information.UBound(_spellingErrorRanges)] = new CharacterRange(wordStart - 1, currentWord.Length);
                        }
                    }
                }
            }
            else
            {
                if (!wordStarted)
                    wordStart = i;
                wordStarted = true;
            }

        FoundApostrophe:
            ;

        }

        // We have to check the last character separately or the last word won't be added
        if (!char.IsLetterOrDigit(Conversions.ToChar(Strings.Right(FullText, 1))))
        {
            return;
        }

        string lastWord = Strings.Trim(Strings.Mid(FullText, wordStart, FullText.Length - wordStart + 1));

        // Spell check it
        for (int j = 0, loopTo2 = Information.UBound(_spellingErrors); j <= loopTo2; j++)
        {
            if ((_spellingErrors[j] ?? "") == (lastWord ?? ""))
            {
                // Add it to the array
                Array.Resize(ref _spellingErrorRanges, Information.UBound(_spellingErrorRanges) + 1 + 1);
                _spellingErrorRanges[Information.UBound(_spellingErrorRanges)] = new CharacterRange(wordStart - 1, lastWord.Length);
            }
        }
    }

#endregion

    public void UpdateHunspell(ref Hunspell newHunspell)
    {
        myNHunspell = newHunspell;
    }

}