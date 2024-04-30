using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSecurity.Models
{
    public interface ICrsValidator
    {
        bool DetectSQLI(string stringValue);
        bool DetectSQLI(Dictionary<string, string> dictionaryValue);
        bool DetectSQLI(IEnumerable<string> enumerableValue);
        bool DetectXSS(Dictionary<string, string> dictionaryValue);
        bool DetectXSS(IEnumerable<string> enumerableValue);
        bool DetectXSS(string stringValue);
        bool ValidateByteRange(string stringValue, string byteRangeList);
        bool ValidateByteRange(IEnumerable<string> enumerableValue, int byteRangeStart, int byteRangeEnd);
        bool ValidateByteRange(Dictionary<string, string> dictionaryValue, string byteRangeList);
        bool ValidateByteRange(Dictionary<string, string> dictionaryValue, int byteRangeStart, int byteRangeEnd);
        bool ValidateByteRange(IEnumerable<string> enumerableValue, string byteRangeList);
        bool ValidateByteRange(string stringValue, int byteRangeStart, int byteRangeEnd);
        bool ValidateUrlEncoding(string stringValue);
        bool ValidateUtf8Encoding(string stringValue);
        bool ValidateUtf8Encoding(Dictionary<string, string> dictionaryValue);
        bool ValidateUtf8Encoding(IEnumerable<string> enumerableValue);
    }
}
