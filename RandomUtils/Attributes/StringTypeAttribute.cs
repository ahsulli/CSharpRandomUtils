using RandomUtils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Attributes
{
    /// <summary>
    /// Used to specify the parameters for the string to be randomly generated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class StringTypeAttribute : Attribute
    {
        internal StringType StringType { get; set; }
        internal string RegexPattern { get; set; }

        /// <summary>
        /// Specifies the parameters for the string to be randomly generated.
        /// </summary>
        /// <param name="stringType">Specify the constraint on the string generation based on some common values.</param>
        /// <param name="regexPattern">If <paramref name="stringType"/> is StringType.Regex, then provide the regex pattern.</param>
        public StringTypeAttribute(StringType stringType, string regexPattern = "")
        {
            StringType = stringType;
            RegexPattern = regexPattern;
        }
    }
}
