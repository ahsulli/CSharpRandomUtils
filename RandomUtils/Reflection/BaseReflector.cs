using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Reflection
{
    internal abstract class BaseReflector<TInput>
    {
        protected Assembly _callingAssembly { get; set; }
        protected string _resourceName { get; set; }
        internal List<Aspect> Aspects { get; set; }
        internal MethodInfo RandomizationValidator { get; set; }

        internal BaseReflector(Assembly assembly)
        {
            Aspects = new List<Aspect>();
            RandomizationValidator = null;
            _callingAssembly = assembly;
            _resourceName = string.Empty;
        }

        internal abstract void Analyze(string resourceName = "");
    }
}
