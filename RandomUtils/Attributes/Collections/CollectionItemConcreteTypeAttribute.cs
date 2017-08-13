using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Attributes.Collections
{
    /// <summary>
    /// Used to specify the concrete type of items that are part of a collection.
    /// This is helpful if the type calls for storing these items as an abstract type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CollectionItemConcreteTypeAttribute : Attribute
    {
        internal Type ConcreteType { get; set; }

        /// <summary>
        /// Specify the concrete type of items that are part of this collection.
        /// </summary>
        /// <param name="concreteType">The concrete type of items that are part of this collection.</param>
        public CollectionItemConcreteTypeAttribute(Type concreteType)
        {
            ConcreteType = concreteType;
        }
    }
}
