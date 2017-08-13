using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Attributes.Collections
{
    /// <summary>
    /// Used to specify the minimum number of items that can belong in the collection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MinimumLengthAttribute : Attribute
    {
        internal int MinimumLength { get; set; }

        /// <summary>
        /// Specify the minimum number of items that can belong in the collection.
        /// </summary>
        /// <param name="min">The minimum number of items that can belong in the collection.</param>
        public MinimumLengthAttribute(int min)
        {
            MinimumLength = min;
        }
    }
}
