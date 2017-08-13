using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Attributes
{
    /// <summary>
    /// Used to specify the concrete type of the property, including the top-level collection.
    /// This is helpful if the type calls for storing these items as an abstract type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ConcreteTypeAttribute : Attribute
    {
        internal Type ConcreteType { get; set; }

        /// <summary>
        /// Specify the concrete type of the property, including the top-level collection.
        /// </summary>
        /// <param name="concreteType">The concrete type of the property, including the top-level collection.</param>
        public ConcreteTypeAttribute(Type concreteType)
        {
            ConcreteType = concreteType;
        }
    }
}
