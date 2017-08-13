using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Enums
{
    /// <summary>
    /// Common constraints used when generating a random string value
    /// </summary>
    public enum StringType
    {
        /// <summary>
        /// No constraints. Use any UTF-8 character from alphanumeric and whitespace characters to symbols.
        /// </summary>
        All,

        /// <summary>
        /// Restrict the characters to alphabetic characters, both upper and lower case.
        /// </summary>
        Alphabetic,

        /// <summary>
        /// Restrict the characters to alphabetic, both upper and lower case, and numeric characters.
        /// This is the default value.
        /// </summary>
        AlphaNumeric,

        /// <summary>
        /// Restrict the characters to a specified regex pattern.
        /// </summary>
        Regex
    }
}
