using RandomUtils.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RandomUtils.Randomizer.TypeGenerators;
using System.Diagnostics;
using System.Threading;
using RandomUtils.Parameterization;

namespace RandomUtils.Randomizer
{
    /// <summary>
    /// Generates a random object of the specified type.
    /// </summary>
    public class RandomGenerator : IRandomGenerator
    {
        private IRandomGenerator _randomGenerator { get; set; }

        /// <summary>
        /// Creates a RandomGenerator to generate the random object.
        /// </summary>
        /// <param name="randomValueType">The type of the random object.</param>
        /// <param name="resourceName">The resource file name that contains parameters of the random values.</param>
        public RandomGenerator(Type randomValueType, string resourceName = "")
        {
            Type randomGeneratorType = typeof(RandomGenerator<>).MakeGenericType(randomValueType);
            _randomGenerator = (IRandomGenerator)Activator.CreateInstance(randomGeneratorType, new object[] { randomValueType.Assembly, resourceName });
        }

        dynamic IRandomGenerator.Generate()
        {
            return _randomGenerator.Generate();
        }
    }

    /// <summary>
    /// Generates a random object of the specified type.
    /// </summary>
    /// <typeparam name="TInput">The type of the random object.</typeparam>
    public class RandomGenerator<TInput> : IRandomGenerator where TInput : new()
    {
        private static readonly object _stopwatchLock = new object();

        private Assembly _asm { get; set; }
        private List<int> _seeds { get; set; }
        private object _nestedThisValue { get; set; }

        internal static Stopwatch _stopwatch { get; set; }
        internal RandomValueConverter RandomConverter { get; set; }
        internal Random RandomCore { get; set; }
        internal BaseReflector<TInput> Reflector { get; set; }
        internal string ResourceName { get; set; }

        /// <summary>
        /// Creates a RandomGenerator to generate the random object.
        /// </summary>
        /// <param name="resourceName">The resource file name that contains parameters of the random values.</param>
        public RandomGenerator(string resourceName = "") : this(Assembly.GetCallingAssembly(), resourceName)
        {

        }

        /// <summary>
        /// Creates a RandomGenerator to generate the random object.
        /// </summary>
        /// <param name="asm">The assembly the contains the type, TInput.</param>
        /// <param name="resourceName">The resource file name that contains parameters of the random values.</param>
        public RandomGenerator(Assembly asm, string resourceName = "")
        {
            if (_stopwatch == null)
            {
                lock (_stopwatchLock)
                {
                    if (_stopwatch == null)
                    {
                        _stopwatch = Stopwatch.StartNew();
                    }
                }
            }
            Initialize(asm, resourceName);
        }

        /// <summary>
        /// Creates a RandomGenerator to generate the random object.
        /// </summary>
        /// <param name="asm">The assembly the contains the type, TInput.</param>
        /// <param name="stopwatch">The current stopwatch instance that is being used by the RandomGenerator to determine a seed value for the Random class.</param>
        /// <param name="nestedValue">The object that contains the object being randomized.</param>
        /// <param name="resourceName">The resource file name that contains parameters of the random values.</param>
        public RandomGenerator(Assembly asm, Stopwatch stopwatch, object nestedValue, string resourceName = "")
        {
            _stopwatch = stopwatch;
            _nestedThisValue = nestedValue;
            Initialize(asm, resourceName);
        }

        private void Initialize(Assembly asm, string resourceName)
        {
            _asm = asm;
            _seeds = new List<int>();
            Reflector = new Reflector<TInput>(_asm);
            ResourceName = resourceName;
            RandomCore = new Random(ConvertToSeed());
            RandomConverter = new RandomValueConverter();
        }

        dynamic IRandomGenerator.Generate()
        {
            return Generate();
        }

        internal TInput Generate()
        {
            Reflector.Analyze(ResourceName);
            if (Reflector.Aspects.Count == 1 && Reflector.Aspects.First().Property.PropertyType.IsValueType)
            {
                MethodInfo method = GetSimpleRandomObjectMethod("RandomizeSimpleObject", Reflector.Aspects.First().Property.PropertyType);
                return (TInput)method.Invoke(this, new object[] { Reflector.Aspects.First() }); ;
            }
            else
            {
                TInput randomValue = (TInput)Activator.CreateInstance(typeof(TInput));
                bool valid = false;
                while (!valid)
                {
                    foreach (Aspect item in Reflector.Aspects)
                    {
                        randomValue = GenerateValue(item, randomValue);
                    }
                    if (Reflector.RandomizationValidator != null)
                    {
                        valid = (bool)Reflector.RandomizationValidator.Invoke(randomValue, null);
                    }
                    else
                    {
                        valid = true;
                    }
                }
                return randomValue;
            }
        }

        private TItem GenerateItem<TItem>(Aspect aspect)
        {
            MethodInfo method = GetSimpleRandomObjectMethod("GenerateRandomSimpleObjectItem", aspect.ConcreteType);
            return (TItem)method.Invoke(this, new object[] { aspect });
        }

        private TInput GenerateValue(Aspect item, TInput randomValue)
        {
            bool isCollection = (item.Property.PropertyType.GetInterfaces()
                            .Where(type => type.Namespace.Contains("Collection")).Count() > 0 || item.Property.PropertyType.IsArray)
                            && !(item.Property.PropertyType == typeof(string));
            if (isCollection)
            {
                MethodInfo method = GetSimpleRandomObjectMethod("GenerateRandomCollection", item.ConcreteCollectionItemType, item.ConcreteType);
                item.Property.SetValue(randomValue, method.Invoke(this, new object[] { item, randomValue }));
            }
            else if (item.Property.PropertyType.IsValueType)
            {
                MethodInfo method = GetSimpleRandomObjectMethod("GenerateRandomSimpleObject", item.ConcreteType);
                item.Property.SetValue(randomValue, method.Invoke(this, new object[] { item }));
            }
            else if (item.Property.PropertyType == typeof(string))
            {
                RandomStringGenerator<TInput> stringGenerator = new RandomStringGenerator<TInput>(ConvertToSeed());
                item.Property.SetValue(randomValue, stringGenerator.Generate(item));
            }
            else
            {
                switch (item.ValueType)
                {
                    case Enums.ValueType.DefaultValue:
                        Type itemRandomizerType = typeof(RandomGenerator<>).MakeGenericType(item.ConcreteType);
                        IRandomGenerator randomizer = (IRandomGenerator)Activator.CreateInstance(itemRandomizerType, _asm, _stopwatch, randomValue, ResourceName);
                        dynamic tempRandomValue = randomizer.Generate();
                        item.Property.SetValue(randomValue, tempRandomValue);
                        break;
                    case Enums.ValueType.LeaveNull:
                        item.Property.SetValue(randomValue, null);
                        break;
                    case Enums.ValueType.NestedThis:
                        item.Property.SetValue(randomValue, _nestedThisValue);
                        break;
                }
            }
            return randomValue;
        }

        private ICollection<TOutput> GenerateRandomCollection<TOutput, TItem>(Aspect item, TInput baseObject)
        {
            ICollection<TOutput> randomPropertyValue = (ICollection<TOutput>)Activator.CreateInstance(item.ConcreteType);
            int loopCounter = 0;
            if (!item.Required)
            {
                if (Convert.ToBoolean(RandomCore.Next(0, 1)))
                {
                    return randomPropertyValue;
                }
            }
            loopCounter = RandomCore.Next(item.MinimumLength, item.MaximumLength);
            bool validCollection = false;
            while (!validCollection)
            {
                for (int i = 0; i < loopCounter; i++)
                {
                    bool validItem = false;
                    while (!validItem)
                    {
                        TOutput tempRandomValue;
                        if (item.ConcreteCollectionItemType.IsValueType)
                        {
                            MethodInfo method = GetSimpleRandomObjectMethod("GenerateItem", item.ConcreteCollectionItemType);
                            Aspect collectionItemAspect = CreateCollectionItemAspect(item);
                            tempRandomValue = (TOutput)method.Invoke(this, new object[] { collectionItemAspect });
                        }
                        else
                        {
                            Type itemRandomizerType = typeof(RandomGenerator<>).MakeGenericType(item.ConcreteCollectionItemType);
                            IRandomGenerator randomizer = (IRandomGenerator)Activator.CreateInstance(itemRandomizerType, _asm, _stopwatch, baseObject, ResourceName);
                            tempRandomValue = (TOutput)randomizer.Generate();
                        }
                        if (item.CollectionItemValidator != null)
                        {
                            validItem = (bool)item.CollectionItemValidator.Invoke(baseObject, new object[] { tempRandomValue });
                        }
                        else
                        {
                            validItem = true;
                        }
                        if (validItem)
                        {
                            randomPropertyValue.Add(tempRandomValue);
                        }
                    }
                }
                if (item.Validator != null)
                {
                    validCollection = (bool)item.Validator.Invoke(baseObject, new object[] { randomPropertyValue });
                }
                else
                {
                    validCollection = true;
                }
            }
            return randomPropertyValue;
        }

        private Aspect CreateCollectionItemAspect(Aspect item)
        {
            return new Aspect()
            {
                ConcreteType = item.ConcreteCollectionItemType,
                EarliestDateTime = item.EarliestDateTime,
                LatestDateTime = item.LatestDateTime,
                Maximum = item.Maximum,
                Minimum = item.Minimum,
                RegexPattern = item.RegexPattern,
                Required = item.Required,
                StringType = item.StringType,
                Validator = item.CollectionItemValidator
            };
        }

        private MethodInfo GetSimpleRandomObjectMethod(string methodName, params Type[] types)
        {
            return GetType().GetRuntimeMethods()
                    .Where(method => method.Name == methodName && method.GetGenericArguments().Count() == types.Count()).First()
                    .MakeGenericMethod(types);
        }

        private TInput RandomizeSimpleObject<TOutput>(Aspect aspect)
        {
            TInput randomValue = (TInput)Activator.CreateInstance(typeof(TInput));
            bool valid = false;
            while (!valid)
            {
                TOutput currentRandomValue = GenerateRandomSimpleObject<TOutput>(aspect);
                if (aspect.Validator != null)
                {
                    valid = (bool)aspect.Validator.Invoke(randomValue, new object[] { currentRandomValue });
                }
                else
                {
                    aspect.Property.SetValue(randomValue, currentRandomValue);
                    valid = true;
                }
            }
            return randomValue;
        }

        private TOutput GenerateRandomSimpleObject<TOutput>(Aspect aspect)
        {
            Type primitiveType = aspect.Property.PropertyType;
            if (primitiveType.Name == "Nullable`1")
            {
                if (Convert.ToBoolean(RandomCore.Next(0, 1)))
                {
                    return default(TOutput);
                }
                else
                {
                    MethodInfo method = GetSimpleRandomObjectMethod("GenerateRandomSimpleObject", primitiveType.GenericTypeArguments.First());
                    return (TOutput)method.Invoke(this, new object[] { aspect });
                }
            }
            MethodInfo methodInvoker = GetType().GetRuntimeMethods()
                    .Where(method => method.Name == "GetRandomizer").First()
                    .MakeGenericMethod(primitiveType);
            IRandomTypeGenerator<TInput, TOutput> randomizer = (IRandomTypeGenerator<TInput, TOutput>)methodInvoker.Invoke(this, null);
            return randomizer.Generate(aspect);
        }

        private TOutput GenerateRandomSimpleObjectItem<TOutput>(Aspect aspect)
        {
            Type primitiveType = typeof(TOutput);
            if (primitiveType.Name == "Nullable`1")
            {
                if (Convert.ToBoolean(RandomCore.Next(0, 1)))
                {
                    return default(TOutput);
                }
                else
                {
                    MethodInfo method = GetSimpleRandomObjectMethod("GenerateRandomSimpleObjectItem", primitiveType.GenericTypeArguments.First());
                    return (TOutput)method.Invoke(this, new object[] { aspect });
                }
            }
            MethodInfo methodInvoker = GetType().GetRuntimeMethods()
                    .Where(method => method.Name == "GetRandomizer").First()
                    .MakeGenericMethod(primitiveType);
            IRandomTypeGenerator<TInput, TOutput> randomizer = (IRandomTypeGenerator<TInput, TOutput>)methodInvoker.Invoke(this, null);
            return randomizer.Generate(aspect);
        }

        private IRandomTypeGenerator<TInput, TOutput> GetRandomizer<TOutput>()
        {
            string primitiveTypeName = typeof(TOutput).Name;
            primitiveTypeName = Regex.Replace(primitiveTypeName, @"[\d-]", string.Empty);
            string typeGeneratorName = $"Random{primitiveTypeName}Generator`1";
            Type typeGenerator = Assembly.GetExecutingAssembly().DefinedTypes
                .Where(type => type.Name == typeGeneratorName).First()
                .MakeGenericType(typeof(TInput));
            return (IRandomTypeGenerator<TInput, TOutput>)Activator.CreateInstance(typeGenerator, ConvertToSeed());
        }

        private int ConvertToSeed()
        {
            CleanseSeeds();
            int seedValue = 0;
            if (_stopwatch.ElapsedMilliseconds <= DefaultValues.MAXINTVALUE)
            {
                seedValue = (int)_stopwatch.ElapsedMilliseconds;
            }
            seedValue = (int)_stopwatch.ElapsedMilliseconds % DefaultValues.MAXINTVALUE;
            if (!_seeds.Contains(seedValue))
            {
                _seeds.Add(seedValue);
                return seedValue;
            }
            while (true)
            {
                seedValue++;
                if (!_seeds.Contains(seedValue))
                {
                    _seeds.Add(seedValue);
                    return seedValue;
                }
            }
        }

        private void CleanseSeeds()
        {
            if (_seeds.Count / DefaultValues.MAXINTVALUE >= DefaultValues.DEFAULTSEEDCLEARINGTHRESHOLD)
            {
                _seeds.Clear();
            }
        }
    }
}
