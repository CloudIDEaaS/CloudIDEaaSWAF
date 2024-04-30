using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils.Parsing
{
    public static class ExtensionMethods
    {
        public static CharacterCodes CharCodeAt(this string text, int pos)
        {
            return (CharacterCodes)text[pos];
        }
    }
}
