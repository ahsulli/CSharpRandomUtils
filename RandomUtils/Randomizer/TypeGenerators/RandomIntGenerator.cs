using RandomUtils.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Randomizer.TypeGenerators
{
    internal class RandomIntGenerator<TInput> : TypeGenerator, IRandomTypeGenerator<TInput, int>
    {
        public RandomIntGenerator(int seedValue) : base(seedValue)
        {

        }

        public int Generate(Aspect aspect)
        {
            if (!aspect.Required)
            {
                int continueValue = RandomCore.Next(0, 1);
                if (continueValue == 1)
                {
                    return default(int);
                }
            }
            int randomValue = RandomCore.Next(aspect.Minimum, aspect.Maximum);
            var randomDataModel = (TInput)Activator.CreateInstance(typeof(TInput));
            if (aspect.Validator != null)
            {
                while (!(bool)aspect.Validator.Invoke(randomDataModel, new object[] { randomValue }))
                {
                    randomValue = RandomCore.Next(aspect.Minimum, aspect.Maximum);
                }
            }
            return randomValue;
        }
    }
}
