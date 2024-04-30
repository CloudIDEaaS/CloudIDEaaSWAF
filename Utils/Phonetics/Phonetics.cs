using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utils.PhoneticMatch
{
    public interface IPhoneticMatch
    {
        string CreateToken(string word);
    }

    public interface IStringDistance
    {
        int Distance(string source, string target);
    }

    public static class Extensions
    {
        public static bool SoundsLike(this string source, string compare)
        {
            var soundex = new Soundex();

            return soundex.CreateToken(source) == soundex.CreateToken(compare);
        }

        public static int GetSoundsLikeDistance(this string source, string compare)
        {
            var distance = new LevenshteinDistance();

            return distance.Distance(source, compare);
        }
    }

    public class LevenshteinDistance : IStringDistance
    {
        public int Distance(string source, string target)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrEmpty(target))
            {
                throw new ArgumentNullException("target");
            }

            source = source.ToUpper();
            target = target.ToUpper();

            var distance = new int[source.Length + 1, target.Length + 1];

            for (var i = 0; i <= source.Length; i++)
            {
                distance[i, 0] = i;
            }

            for (var j = 0; j <= target.Length; j++)
            {
                distance[0, j] = j;
            }

            for (var j = 1; j <= target.Length; j++)
            {
                for (var i = 1; i <= source.Length; i++)
                {
                    if (source[i - 1] == target[j - 1])
                    {
                        distance[i, j] = distance[i - 1, j - 1];
                    }
                    else
                    {
                        distance[i, j] = Math.Min(Math.Min(
                        distance[i - 1, j] + 1, //a deletion
                        distance[i, j - 1] + 1), //an insertion
                        distance[i - 1, j - 1] + 1 //a substitution
                        );
                    }
                }
            }

            return distance[source.Length, target.Length];
        }
    }

    public class Soundex : IPhoneticMatch
    {
        public string CreateToken(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentNullException("word");
            }

            const int maxSoundexCodeLength = 4;

            var soundexCode = new StringBuilder();

            word = Regex.Replace(word.ToUpper(), @"[^\w\s]", string.Empty);

            if (string.IsNullOrEmpty(word))
            {
                return string.Empty.PadRight(maxSoundexCodeLength, '0');
            }

            soundexCode.Append(word.First());
            ApplySoundexRules(word, soundexCode);

            return PadAndFormatReturnCode(soundexCode, maxSoundexCodeLength);
        }

        private static string PadAndFormatReturnCode(StringBuilder soundexCode, int maxSoundexCodeLength)
        {
            var returnValue = soundexCode.Replace("0", string.Empty).ToString();
            returnValue = returnValue.PadRight(maxSoundexCodeLength, '0');
            returnValue = returnValue.Substring(0, maxSoundexCodeLength);
            return returnValue;
        }

        private void ApplySoundexRules(string word, StringBuilder soundexCode)
        {
            var previousWasHorW = false;

            for (var i = 1; i < word.Length; i++)
            {
                var numberCharForCurrentLetter = GetCharNumberForLetter(word[i]);

                if (i == 1 && numberCharForCurrentLetter == GetCharNumberForLetter(soundexCode[0]))
                {
                    continue;
                }

                if (soundexCode.Length > 2 && previousWasHorW &&
                    numberCharForCurrentLetter == soundexCode[soundexCode.Length - 2])
                {
                    continue;
                }

                if (soundexCode.Length > 0 && numberCharForCurrentLetter == soundexCode[soundexCode.Length - 1])
                {
                    continue;
                }

                soundexCode.Append(numberCharForCurrentLetter);

                previousWasHorW = "HW".Contains(word[i]);
            }
        }

        private char GetCharNumberForLetter(char letter)
        {
            if ("BFPV".Contains(letter))
            {
                return '1';
            }

            if ("CGJKQSXZ".Contains(letter))
            {
                return '2';
            }

            if ("DT".Contains(letter))
            {
                return '3';
            }

            if ('L' == letter)
            {
                return '4';
            }

            if ("MN".Contains(letter))
            {
                return '5';
            }

            return 'R' == letter ? '6' : '0';
        }
    }
}
