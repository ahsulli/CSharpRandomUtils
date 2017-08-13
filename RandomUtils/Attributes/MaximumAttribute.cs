using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Attributes
{
    /// <summary>
    /// Used to set the maximum integration value or string length for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MaximumAttribute : Attribute
    {
        internal int MaximumSize { get; set; }

        /// <summary>
        /// Set the maximum integration value or string length for the property.
        /// </summary>
        /// <param name="max">The maximum integration value or string length for the property.</param>
        public MaximumAttribute(int max)
        {
            MaximumSize = max;
        }
    }
}
