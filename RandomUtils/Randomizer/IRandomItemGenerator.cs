using RandomUtils.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Randomizer
{
    internal interface IRandomItemGenerator : IRandomGenerator
    {
        dynamic Generate(Aspect aspect);
    }
}
