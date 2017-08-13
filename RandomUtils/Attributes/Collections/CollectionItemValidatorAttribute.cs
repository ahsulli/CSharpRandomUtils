using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Attributes.Collections
{
    /// <summary>
    /// Used to specify the validation method for each item in the collection.
    /// This is helpful if items in the collection must meet additional criteria that can't be specified prior to random generation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CollectionItemValidatorAttribute : Attribute
    {
        internal string Validate { get; set; }

        /// <summary>
        /// Indicates that the method validates each item in the collection of a property.
        /// </summary>
        /// <param name="validate">The name of the Collection-typed property that this method validates each item in.</param>
        public CollectionItemValidatorAttribute(string validate)
        {
            Validate = validate;
        }
    }
}
