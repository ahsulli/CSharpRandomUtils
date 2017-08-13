using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Attributes
{
    /// <summary>
    /// Used to tell the random generator if the property requires a random value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RequiredAttribute : Attribute
    {
        internal bool IsRequired { get; set; }

        /// <summary>
        /// Tells the random generator if the property requires a random value. Default value is true. 
        /// If the property is optional, then the random generator randomly selects whether to generate a random value for the property.
        /// </summary>
        /// <param name="required">Whether the property requires a random value.</param>
        public RequiredAttribute(bool required = true)
        {
            IsRequired = required;
        }
    }
}
