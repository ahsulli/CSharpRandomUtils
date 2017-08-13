using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Attributes
{
    /// <summary>
    /// Used to indicate the type of value to be used for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ValueAttribute : Attribute
    {
        internal Enums.ValueType ValueType { get; set; }

        /// <summary>
        /// Indicates the type of value to be used for the property.
        /// </summary>
        /// <param name="type">The type of value to be used for the property.</param>
        public ValueAttribute(Enums.ValueType type)
        {
            ValueType = type;
        }
    }
}
