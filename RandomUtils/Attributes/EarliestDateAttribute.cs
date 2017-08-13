using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Attributes
{
    /// <summary>
    /// Used to set the earliest DateTime value for DateTime type properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EarliestDateAttribute : Attribute
    {
        internal DateTime EarliestDate { get; set; }

        /// <summary>
        /// Set the earliest DateTime value for the DateTime type property.
        /// </summary>
        /// <param name="earliestDate">The earliest DateTime value for the property.</param>
        public EarliestDateAttribute(DateTime earliestDate)
        {
            EarliestDate = earliestDate;
        }
    }
}
