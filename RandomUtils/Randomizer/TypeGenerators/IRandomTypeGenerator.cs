using RandomUtils.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Randomizer.TypeGenerators
{
    internal interface IRandomTypeGenerator<TInput, TOutput>
    {
        TOutput Generate(Aspect aspect);
    }
}
