using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Attributes
{
    /// <summary>
    /// Used to tell the random generator to not randomize the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RandomizerIgnoreAttribute : Attribute
    {
        /// <summary>
        /// Tells the random generator to not randomize the property.
        /// </summary>
        public RandomizerIgnoreAttribute()
        {

        }
    }
}
