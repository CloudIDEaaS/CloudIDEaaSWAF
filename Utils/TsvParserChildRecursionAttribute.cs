// file:	TsvParserRecursionAttribute.cs
//
// summary:	Implements the tsv parser recursion attribute class

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>   Attribute for tsv parser child recursion. </summary>
    ///
    /// <remarks>   Ken, 10/3/2020. </remarks>

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class TsvParserChildRecursionAttribute : Attribute
    {
        /// <summary>   Gets or sets options for controlling the item. </summary>
        ///
        /// <value> Options that control the item. </value>

        public object[] ChildRecursionParameters { get; set; }

        /// <summary>   Gets or sets the type of the item. </summary>
        ///
        /// <value> The type of the item. </value>

        public Type ItemType { get; set; }
    }
}