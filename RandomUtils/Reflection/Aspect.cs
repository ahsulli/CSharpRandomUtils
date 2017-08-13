using RandomUtils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Reflection
{
    internal class Aspect
    {
        internal PropertyInfo Property { get; set; }
        internal bool Required { get; set; }
        internal int Minimum { get; set; }
        internal int Maximum { get; set; }
        internal MethodInfo Validator { get; set; }
        internal Type ConcreteType { get; set; }
        internal int MaximumLength { get; set; }
        internal int MinimumLength { get; set; }
        internal MethodInfo CollectionItemValidator { get; set; }
        internal Type ConcreteCollectionItemType { get; set; }
        internal DateTime EarliestDateTime { get; set; }
        internal DateTime LatestDateTime { get; set; }
        internal StringType StringType { get; set; }
        internal string RegexPattern { get; set; }
        internal Enums.ValueType ValueType { get; set; }
    }
}
