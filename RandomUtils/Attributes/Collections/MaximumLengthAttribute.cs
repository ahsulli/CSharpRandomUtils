using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Attributes.Collections
{
    /// <summary>
    /// Used to specify the maximum number of items that can belong in the collection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MaximumLengthAttribute : Attribute
    {
        internal int MaximumLength { get; set; }

        /// <summary>
        /// Specify the maximum number of items that can belong in the collection.
        /// </summary>
        /// <param name="max">The maximum number of items that can belong in the collection.</param>
        public MaximumLengthAttribute(int max)
        {
            MaximumLength = max;
        }
    }
}
