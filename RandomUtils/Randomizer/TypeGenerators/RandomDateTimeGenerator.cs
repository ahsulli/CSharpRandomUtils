using RandomUtils.Parameterization;
using RandomUtils.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtils.Randomizer.TypeGenerators
{
    internal class RandomDateTimeGenerator<TInput> : TypeGenerator, IRandomTypeGenerator<TInput, DateTime>
    {
        public RandomDateTimeGenerator(int seedValue) : base(seedValue)
        {

        }

        public DateTime Generate(Aspect aspect)
        {
            if (!aspect.Required)
            {
                int continueValue = RandomCore.Next(0, 1);
                if (continueValue == 1)
                {
                    return default(DateTime);
                }
            }

            DateTime randomValue = new DateTime();
            var randomDataModel = (TInput)Activator.CreateInstance(typeof(TInput));
            bool valid = false;
            while (!valid)
            {
                randomValue = GenerateRandomAddedTicks(aspect);
                if (aspect.Validator != null)
                {
                    valid = (bool)aspect.Validator.Invoke(randomDataModel, new object[] { randomValue });
                }
                else
                {
                    valid = true;
                }
            }
            return randomValue;
        }

        private DateTime GenerateRandomAddedTicks(Aspect aspect)
        {
            long tickRange = aspect.LatestDateTime.Ticks - aspect.EarliestDateTime.Ticks;
            int ranges = CalculateRanges(tickRange);
            int rangeIndex = RandomCore.Next(1, ranges);

            int intraRangeValue;
            if (rangeIndex == ranges)
            {
                long rangeMax = ranges * DefaultValues.MAXINTVALUE;
                int maxValue = Convert.ToInt32(rangeMax - aspect.LatestDateTime.Ticks);
                intraRangeValue = RandomCore.Next(1, maxValue);
            }
            else
            {
                intraRangeValue = RandomCore.Next(1, DefaultValues.MAXINTVALUE);
            }

            long randomAddedTickValue = (rangeIndex * DefaultValues.MAXINTVALUE) + intraRangeValue;
            return aspect.EarliestDateTime.AddTicks(randomAddedTickValue);
        }

        private int CalculateRanges(long tickRange)
        {
            decimal ranges = Convert.ToDecimal(tickRange / DefaultValues.MAXINTVALUE);
            if (tickRange % DefaultValues.MAXINTVALUE == 0)
            {
                return Convert.ToInt32(ranges);
            }
            return Convert.ToInt32(Math.Truncate(ranges) + 1);
        }
    }
}
