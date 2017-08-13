using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Attributes
{
    /// <summary>
    /// Used to specify the validation method for either a property or the entire instance of the class.
    /// This is helpful if a property or the entire instance of the class must meet additional criteria that can't be specified prior to random generation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ValidatorAttribute : Attribute
    {
        /// <summary>
        /// True if the method validates the entire instance of the class.
        /// False if the method validates the value of a property.
        /// </summary>
        public bool ClassScope { get; set; }
        internal string Validate { get; set; }

        /// <summary>
        /// Specifies that the method validates either a property or the entire instance of the class.
        /// </summary>
        /// <param name="propertyNameToValidate">The name of the property whose value is validated by this method.</param>
        /// <param name="classScope">True if the method validates the entire instance of the class. False if the method validates the value of a property.</param>
        public ValidatorAttribute(string propertyNameToValidate = "", bool classScope = false)
        {
            ClassScope = classScope;
            Validate = propertyNameToValidate;
        }
    }
}
