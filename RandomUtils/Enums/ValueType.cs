using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Enums
{
    /// <summary>
    /// Common types to be used by the Random Generator in place of randomly generating an object.
    /// </summary>
    public enum ValueType
    {
        /// <summary>
        /// This is the default value used by the compiler.
        /// </summary>
        DefaultValue,

        /// <summary>
        /// This tells the generator to leave the property null,
        /// so the property can be populated by your code after the random generation.
        /// </summary>
        LeaveNull,

        /// <summary>
        /// This tells the generator to reference another generated object, when a.b.a = a.
        /// This is common with objects tied to EntityFramework.
        /// </summary>
        NestedThis
    }
}
