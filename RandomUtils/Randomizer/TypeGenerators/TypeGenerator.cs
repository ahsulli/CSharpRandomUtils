using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Randomizer.TypeGenerators
{
    internal abstract class TypeGenerator
    {
        internal Random RandomCore { get; set; }

        public TypeGenerator(int seedValue)
        {
            RandomCore = new Random(seedValue);
        }
    }
}
