using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Attributes
{
    /// <summary>
    /// Used to set the latest DateTime value for DateTime type properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class LatestDateAttribute : Attribute
    {
        internal DateTime LatestDateTime { get; set; }

        /// <summary>
        /// Set the latest DateTime value for the DateTime type property.
        /// </summary>
        /// <param name="latestDateTime">The latest DateTime value for the property.</param>
        public LatestDateAttribute(DateTime latestDateTime)
        {
            LatestDateTime = latestDateTime;
        }
    }
}
