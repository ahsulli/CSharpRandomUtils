using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Attributes
{
    /// <summary>
    /// Used to set the minimum integer value or string length for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MinimumAttribute : Attribute
    {
        internal int MinimumSize { get; set; }

        /// <summary>
        /// Set the minimum integer value or string length for the property.
        /// </summary>
        /// <param name="min">The minimum integration value or string length for the property.</param>
        public MinimumAttribute(int min)
        {
            MinimumSize = min;
        }
    }
}
