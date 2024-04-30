using System;
using System.Diagnostics;

namespace Utils
{
    [DebuggerDisplay(" { String } ")]
    public class CaselessString
    {
        public string String { get; private set; }

        public CaselessString(string str)
        {
            this.String = str;
        }

        public static implicit operator CaselessString(string str)
        {
            return new CaselessString(str);
        }

        public static implicit operator string(CaselessString str)
        {
            return str.String;
        }

        public static bool operator !=(CaselessString str1, CaselessString str2)
        {
            return string.Compare(str1, str2, StringComparison.InvariantCultureIgnoreCase) != 0;
        }

        public static bool operator ==(CaselessString str1, CaselessString str2)
        {
            return string.Compare(str1, str2, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is string)
            {
                return ((string) obj) == this.String;
            }

            return false;
        }
    }
}
