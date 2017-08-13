using RandomUtils.Enums;
using RandomUtils.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fare;
using RandomUtils.Parameterization;

namespace RandomUtils.Randomizer.TypeGenerators
{
    internal class RandomStringGenerator<TInput> : TypeGenerator, IRandomTypeGenerator<TInput, string>
    {
        private static Dictionary<StringType, Dictionary<int, char>> characterCodes = new Dictionary<StringType, Dictionary<int, char>>()
        {
            { StringType.All, new StringConstants().GetMapping(StringType.All) },
            { StringType.Alphabetic, new StringConstants().GetMapping(StringType.Alphabetic) },
            { StringType.AlphaNumeric, new StringConstants().GetMapping(StringType.AlphaNumeric) }
        };

        internal RandomValueConverter RandomConverter { get; set; }

        public RandomStringGenerator(int seedValue) : base(seedValue)
        {
            RandomConverter = new RandomValueConverter();
        }

        public string Generate(Aspect aspect)
        {
            if (!aspect.Required)
            {
                int continueValue = RandomCore.Next(0, 1);
                if (continueValue == 1)
                {
                    return string.Empty;
                }
            }
            if (aspect.Validator != null)
            {
                string randomValue = GenerateValidRandomString(aspect);
                return randomValue;
            }
            else
            {
                return RandomizeString(aspect);
            }
        }

        private string GenerateValidRandomString(Aspect aspect)
        {
            string randomValue = RandomizeString(aspect);
            var randomDataModel = (TInput)Activator.CreateInstance(typeof(TInput));
            while (!(bool)aspect.Validator.Invoke(randomDataModel, new object[] { randomValue }))
            {
                randomValue = RandomizeString(aspect);
            }
            return randomValue;
        }

        private string RandomizeString(Aspect aspect)
        {
            string randomValue = string.Empty;
            if (aspect.StringType != StringType.Regex)
            {
                int stringLength = RandomCore.Next(aspect.Minimum, aspect.Maximum);
                int characterCodeMax = characterCodes[aspect.StringType].Count - 1;
                for (int loopCounter = 0; loopCounter < stringLength; loopCounter++)
                {
                    randomValue += characterCodes[aspect.StringType]
                        .ElementAt(RandomCore.Next(0, characterCodeMax)).Value;
                }
            }
            else
            {
                Xeger regexStringGenerator = new Xeger(aspect.RegexPattern);
                randomValue = regexStringGenerator.Generate();
            }
            return randomValue;
        }
    }
}
