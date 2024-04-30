// file:	TsvParser.cs
//
// summary:	Implements the tsv parser class

using System;
using System.Collections.Generic;

namespace Utils
{
    /// <summary>   Interface for tsv parser. </summary>
    ///
    /// <remarks>   Ken, 10/1/2020. </remarks>

    public interface ITsvParser
    {
    }

    /// <summary>   Interface for tsv parser host. </summary>
    ///
    /// <remarks>   Ken, 10/1/2020. </remarks>
    ///
    /// <typeparam name="TRecord">  Type of the record. </typeparam>

    public interface ITsvParserHost<TRecord>
    {
        /// <summary>   Parse file. </summary>
        ///
        /// <param name="file">             The file. </param>
        /// <param name="appName">          Name of the application. </param>
        /// <param name="organizationName"> Name of the organization. </param>
        ///
        /// <returns>   A List&lt;TRecord&gt; </returns>

        List<TRecord> ParseFile(string file, Dictionary<string, string> textReplacements, Func<string> getFirstColumnValue = null);
    }

    /// <summary>   A tsv parser. </summary>
    ///
    /// <remarks>   Ken, 10/1/2020. </remarks>

    public class TsvParser : ITsvParser
    {
    }
}