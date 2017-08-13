using RandomUtils.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Randomizer.TypeGenerators
{
    internal class RandomBooleanGenerator<TInput> : TypeGenerator, IRandomTypeGenerator<TInput, bool>
    {
        public RandomBooleanGenerator(int seedValue) : base(seedValue)
        {

        }

        public bool Generate(Aspect aspect)
        {
            if (!aspect.Required)
            {
                int continueValue = RandomCore.Next(0, 1);
                if (continueValue == 1)
                {
                    return default(bool);
                }
            }
            int tempValue = RandomCore.Next(0, 1);
            bool randomValue = tempValue == 0;
            var randomDataModel = (TInput)Activator.CreateInstance(typeof(TInput));
            if (aspect.Validator != null)
            {
                if (!(bool)aspect.Validator.Invoke(randomDataModel, new object[] { randomValue }))
                {
                    randomValue = tempValue == 1;
                }
            }
            return randomValue;
        }
    }
}
