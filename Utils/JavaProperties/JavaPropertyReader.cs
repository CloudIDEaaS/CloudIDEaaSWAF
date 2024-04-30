/*
 * Copyright 2009-15 Williams Technologies Limtied.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Kajbity is a trademark of Williams Technologies Limited.
 *
 * http://www.kajabity.com
 */

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace Utils;

/// <summary>
/// This class reads Java style properties from an input stream.  
/// </summary>
public class JavaPropertyReader
{
    private const int MATCH_end_of_input = 1;
    private const int MATCH_terminator = 2;
    private const int MATCH_whitespace = 3;
    private const int MATCH_any = 4;

    private const int ACTION_add_to_key = 1;
    private const int ACTION_add_to_value = 2;
    private const int ACTION_store_property = 3;
    private const int ACTION_escape = 4;
    private const int ACTION_ignore = 5;

    private const int STATE_start = 0;
    private const int STATE_comment = 1;
    private const int STATE_key = 2;
    private const int STATE_key_escape = 3;
    private const int STATE_key_ws = 4;
    private const int STATE_before_separator = 5;
    private const int STATE_after_separator = 6;
    private const int STATE_value = 7;
    private const int STATE_value_escape = 8;
    private const int STATE_value_ws = 9;
    private const int STATE_finish = 10;

    private static string[] stateNames = new string[]
    { "STATE_start", "STATE_comment", "STATE_key", "STATE_key_escape", "STATE_key_ws",
        "STATE_before_separator", "STATE_after_separator", "STATE_value", "STATE_value_escape",
        "STATE_value_ws", "STATE_finish" };

    private static int[][] states = new int[][] {
        new int[]{//STATE_start
            MATCH_end_of_input, STATE_finish,           ACTION_ignore,
            MATCH_terminator,   STATE_start,            ACTION_ignore,
            '#',                STATE_comment,          ACTION_ignore,
            '!',                STATE_comment,          ACTION_ignore,
            MATCH_whitespace,   STATE_start,            ACTION_ignore,
            '\\',               STATE_key_escape,       ACTION_escape,
            ':',                STATE_after_separator,  ACTION_ignore,
            '=',                STATE_after_separator,  ACTION_ignore,
            MATCH_any,          STATE_key,              ACTION_add_to_key,
        },
        new int[]{//STATE_comment
            MATCH_end_of_input, STATE_finish,           ACTION_ignore,
            MATCH_terminator,   STATE_start,            ACTION_ignore,
            MATCH_any,          STATE_comment,          ACTION_ignore,
        },
        new int[]{//STATE_key
            MATCH_end_of_input, STATE_finish,           ACTION_store_property,
            MATCH_terminator,   STATE_start,            ACTION_store_property,
            MATCH_whitespace,   STATE_before_separator, ACTION_ignore,
            '\\',               STATE_key_escape,       ACTION_escape,
            ':',                STATE_after_separator,  ACTION_ignore,
            '=',                STATE_after_separator,  ACTION_ignore,
            MATCH_any,          STATE_key,              ACTION_add_to_key,
        },
        new int[]{//STATE_key_escape
            MATCH_terminator,   STATE_key_ws,           ACTION_ignore,
            MATCH_any,          STATE_key,              ACTION_add_to_key,
        },
        new int[]{//STATE_key_ws
            MATCH_end_of_input, STATE_finish,           ACTION_store_property,
            MATCH_terminator,   STATE_start,            ACTION_store_property,
            MATCH_whitespace,   STATE_key_ws,           ACTION_ignore,
            '\\',               STATE_key_escape,       ACTION_escape,
            ':',                STATE_after_separator,  ACTION_ignore,
            '=',                STATE_after_separator,  ACTION_ignore,
            MATCH_any,          STATE_key,              ACTION_add_to_key,
        },
        new int[]{//STATE_before_separator
            MATCH_end_of_input, STATE_finish,           ACTION_store_property,
            MATCH_terminator,   STATE_start,            ACTION_store_property,
            MATCH_whitespace,   STATE_before_separator, ACTION_ignore,
            '\\',               STATE_value_escape,     ACTION_escape,
            ':',                STATE_after_separator,  ACTION_ignore,
            '=',                STATE_after_separator,  ACTION_ignore,
            MATCH_any,          STATE_value,            ACTION_add_to_value,
        },
        new int[]{//STATE_after_separator
            MATCH_end_of_input, STATE_finish,           ACTION_store_property,
            MATCH_terminator,   STATE_start,            ACTION_store_property,
            MATCH_whitespace,   STATE_after_separator,  ACTION_ignore,
            '\\',               STATE_value_escape,     ACTION_escape,
            MATCH_any,          STATE_value,            ACTION_add_to_value,
        },
        new int[]{//STATE_value
            MATCH_end_of_input, STATE_finish,           ACTION_store_property,
            MATCH_terminator,   STATE_start,            ACTION_store_property,
            '\\',               STATE_value_escape,     ACTION_escape,
            MATCH_any,          STATE_value,            ACTION_add_to_value,
        },
        new int[]{//STATE_value_escape
            MATCH_terminator,   STATE_value_ws,         ACTION_ignore,
            MATCH_any,          STATE_value,                ACTION_add_to_value
        },
        new int[]{//STATE_value_ws
            MATCH_end_of_input, STATE_finish,           ACTION_store_property,
            MATCH_terminator,   STATE_start,            ACTION_store_property,
            MATCH_whitespace,   STATE_value_ws,         ACTION_ignore,
            '\\',               STATE_value_escape,     ACTION_escape,
            MATCH_any,          STATE_value,            ACTION_add_to_value,
        }
    };

    private Hashtable hashtable;

    private const int bufferSize = 1000;

    private bool escaped = false;
    private bool withinQuote = false;
    private StringBuilder keyBuilder = new StringBuilder();
    private StringBuilder valueBuilder = new StringBuilder();

    /// <summary>
    /// Construct a reader passing a reference to a Hashtable (or JavaProperties) instance
    /// where the keys are to be stored.
    /// </summary>
    /// <param name="hashtable">A reference to a hashtable where the key-value pairs can be stored.</param>
    public JavaPropertyReader(Hashtable hashtable)
    {
        this.hashtable = hashtable;
    }

    /// <summary>
    /// <para>Load key value pairs (properties) from an input Stream expected to have ISO-8859-1 encoding (code page 28592).  
    /// The input stream (usually reading from a ".properties" file) consists of a series of lines (terminated 
    /// by \r, \n or \r\n) each a key value pair, a comment or a blank line.</para>
    /// 
    /// <para>Leading whitespace (spaces, tabs, formfeeds) are ignored at the start of any line - and a line that is empty or 
    /// contains only whitespace is blank and ignored.</para>
    /// 
    /// <para>A line with the first non-whitespace character is a '#' or '!' is a comment line and the rest of the line is 
    /// ignored.</para>
    /// 
    /// <para>If the first non-whitespace character is not '#' or '!' then it is the start of a key.  A key is all the
    /// characters up to the first whitespace or a key/value separator - '=' or ':'.</para>
    /// 
    /// <para>The separator is optional.  Any whitespace after the key or after the separator (if present) is ignored.</para>
    /// 
    /// <para>The first non-whitespace character after the separator (or after the key if no separator) begins the value.  
    /// The value may include whitespace, separators, or comment characters.</para>
    /// 
    /// <para>Any unicode character may be included in either key or value by using escapes preceded by the escape 
    /// character '\'.</para>
    /// 
    /// <para>The following special cases are defined:</para>
    /// <code>
    /// 	'\t' - horizontal tab.
    /// 	'\f' - form feed.
    /// 	'\r' - return
    /// 	'\n' - new line
    /// 	'\\' - add escape character.
    /// 
    /// 	'\ ' - add space in a key or at the start of a value.
    /// 	'\!', '\#' - add comment markers at the start of a key.
    /// 	'\=', '\:' - add a separator in a key.
    /// </code>
    /// 
    /// <para>Any unicode character using the following escape:</para>
    /// <code>
    /// 	'\uXXXX' - where XXXX represents the unicode character code as 4 hexadecimal digits.
    /// </code>
    /// 
    /// <para>Finally, longer lines can be broken by putting an escape at the very end of the line.  Any leading space
    /// (unless escaped) is skipped at the beginning of the following line.</para>
    /// 
    /// Examples
    /// <code>
    /// 	a-key = a-value
    /// 	a-key : a-value
    /// 	a-key=a-value
    /// 	a-key a-value
    /// </code>
    /// 
    /// <para>All the above will result in the same key/value pair - key "a-key" and value "a-value".</para>
    /// <code>
    /// 	! comment...
    /// 	# another comment...
    /// </code>
    /// 
    /// <para>The above are two examples of comments.</para>
    /// <code>
    /// 	Honk\ Kong = Near China
    /// </code>
    /// 
    /// <para>The above shows how to embed a space in a key - key is "Hong Kong", value is "Near China".</para>
    /// <code>
    /// 	a-longer-key-example = a really long value that is \
    /// 			split over two lines.
    /// </code>
    /// 
    /// <para>An example of a long line split into two.</para>
    /// </summary>
    /// <param name="stream">The input stream that the properties are read from.</param>
    public void Parse(Stream stream, string fileName)
    {
        reader = new BufferedStream(stream, bufferSize);
        //TODO: Read using correct encoding ISO-8859-1 encoding (code page 28592).

        var state = STATE_start;
        var line = string.Empty;

        do
        {
            int ch = nextChar();

            bool matched = false;

            for (int s = 0; s < states[state].Length; s += 3)
            {
                if (matches(states[state][s], ch))
                {
                    var debugInfo = stateNames[state] + ", " + (s/3) + ", " + ch + (ch>20 ? " (" + (char)ch + ")" : "");
                    var stateName = stateNames[state];

                    matched = true;

                    doAction(states[state][s + 2], ch);

                    state = states[state][s + 1];

                    break;
                }
            }

            this.currentFileName = fileName;

            if (((char)ch) == '\r')
            {
                line = string.Empty;
                lineNumber++;
                columnNumber = 0;
            }
            else
            {
                line += ((char)ch).ToString();
                columnNumber++;
            }

            if (!matched)
            {
                throw new ParseException("Unexpected character at " + 1 + ": <<<" + ch + ">>>");
            }

            lastCh = ch;
        }
        while (state != STATE_finish);
    }

    private bool matches(int match, int ch)
    {
        switch (match)
        {
            case MATCH_end_of_input:
                return ch == -1;

            case MATCH_terminator:
                if (ch == '\r')
                {
                    if (peekChar() == '\n')
                    {
                        saved = false;
                    }
                    return true;
                }
                else if (ch == '\n')
                {
                    return true;
                }
                return false;

            case MATCH_whitespace:
                return ch == ' ' || ch == '\t' || ch == '\f';

            case MATCH_any:
                return true;

            default:
                return ch == match;
        }
    }

    private void doAction(int action, int ch)
    {
        switch (action)
        {
            case ACTION_add_to_key:
                keyBuilder.Append(escapedChar(ch));
                escaped = false;
                break;

            case ACTION_add_to_value:
                valueBuilder.Append(escapedChar(ch));
                escaped = false;
                break;

            case ACTION_store_property:
                //Debug.WriteLine( keyBuilder.ToString() + "=" + valueBuilder.ToString() );
                // Corrected to avoid duplicate entry errors - thanks to David Tanner.
                hashtable[(lineNumber, keyBuilder.ToString())] = valueBuilder.ToString();
                
                keyBuilder.Length = 0;
                valueBuilder.Length = 0;
                escaped = false;
                withinQuote = false;

                break;

            case ACTION_escape:

                if (withinQuote)
                {
                    var nextChar = peekChar();

                    if (nextChar != '\r')
                    {
                        valueBuilder.Append(escapedChar(ch));
                    }
                }
                else
                {
                    escaped = true;
                }

                break;

            //case ACTION_ignore:
            default:
                escaped = false;
                break;
        }

        if (action != ACTION_ignore)
        {
            if (ch == '\'' || ch == '\"')
            {
                if (lastCh != '\\')
                {
                    if (!withinQuote)
                    {
                        withinQuote = true;
                        openQuote = ch;
                    }
                    else if (ch == openQuote)
                    {
                        withinQuote = false;
                    }
                }
            }
        }
    }

    private char escapedChar(int ch)
    {
        if (escaped)
        {
            switch (ch)
            {
                case 't':
                    return '\t';
                case 'r':
                    return '\r';
                case 'n':
                    return '\n';
                case 'f':
                    return '\f';
                case 'u':
                    int uch = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        ch = nextChar();

                        if (ch >= '0' && ch <='9')
                        {
                            uch = (uch << 4) + ch - '0';
                        }
                        else if (ch >= 'a' && ch <='z')
                        {
                            uch = (uch << 4) + ch - 'a' + 10;
                        }
                        else if (ch >= 'A' && ch <='Z')
                        {
                            uch = (uch << 4) + ch - 'A' + 10;
                        }
                        else
                        {
                            throw new ParseException("Invalid Unicode character.");
                        }
                    }
                    return (char)uch;
            }
        }

        return (char)ch;
    }

    private BufferedStream reader = null;
    private int savedChar;
    private bool saved = false;
    private int lastCh;
    private string currentFileName;
    private int openQuote;
    private int lineNumber;
    private int columnNumber;

    private int nextChar()
    {
        if (saved)
        {
            saved = false;
            return savedChar;
        }

        return reader.ReadByte();
    }

    private int peekChar()
    {
        if (saved)
        {
            return savedChar;
        }

        saved = true;
        return savedChar = reader.ReadByte();
    }
}
