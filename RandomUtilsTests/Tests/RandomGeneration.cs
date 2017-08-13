using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomUtils.Randomizer;
using System.Reflection;
using RandomUtilsTests.DataModels;
using RandomUtils.Attributes;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.Linq;

namespace RandomUtilsTests.Tests
{
    [TestClass]
    public class RandomGeneration
    {
        [TestMethod]
        [TestCategory("Random Generation")]
        public void RandomGeneratorComplexTypes()
        {
            RandomGenerator<TestObjectAlpha> randomizer = new RandomGenerator<TestObjectAlpha>();
            TestObjectAlpha randomValue = randomizer.Generate();
            Assert.IsNotNull(randomValue);
            foreach (var item in randomValue.testBeta1.deltas)
            {
                TestObjectDelta castedItem = (TestObjectDelta)item;
                Assert.IsTrue(randomValue.testBeta1 == castedItem.testObjectBeta);
            }
            PrintObjectToConsole(randomValue);
        }

        [TestMethod]
        [TestCategory("Random Generation")]
        public void RandomGeneratorWrapperComplexTypes()
        {
            IRandomGenerator randomizer = new RandomGenerator(typeof(TestObjectAlpha));
            TestObjectAlpha randomValue = (TestObjectAlpha)randomizer.Generate();
            Assert.IsNotNull(randomValue);
            foreach (var item in randomValue.testBeta1.deltas)
            {
                TestObjectDelta castedItem = (TestObjectDelta)item;
                Assert.IsTrue(randomValue.testBeta1 == castedItem.testObjectBeta);
            }
            PrintObjectToConsole(randomValue);
        }

        [TestMethod]
        [TestCategory("Random Generation")]
        public void RandomGeneratorComplexTypesWithExternalFile()
        {
            RandomGenerator<TestObjectAlpha> randomizer = new RandomGenerator<TestObjectAlpha>("RandomUtilsTests.InputFiles.TestDataConstraints.csv");
            TestObjectAlpha randomValue = randomizer.Generate();
            Assert.IsNotNull(randomValue);
            foreach (var item in randomValue.testBeta1.deltas)
            {
                TestObjectDelta castedItem = (TestObjectDelta)item;
                Assert.IsTrue(randomValue.testBeta1 == castedItem.testObjectBeta);
            }
            PrintObjectToConsole(randomValue);
        }

        [TestMethod]
        [TestCategory("Random Generation")]
        public void RandomGeneratorWrapperComplexTypesWithExternalFile()
        {
            IRandomGenerator randomizer = new RandomGenerator(typeof(TestObjectAlpha), "RandomUtilsTests.InputFiles.TestDataConstraints.json");
            TestObjectAlpha randomValue = (TestObjectAlpha)randomizer.Generate();
            Assert.IsNotNull(randomValue);
            foreach (var item in randomValue.testBeta1.deltas)
            {
                TestObjectDelta castedItem = (TestObjectDelta)item;
                Assert.IsTrue(randomValue.testBeta1 == castedItem.testObjectBeta);
            }
            PrintObjectToConsole(randomValue);
        }

        private void PrintObjectToConsole<TInput>(TInput randomValue)
        {
            List<PropertyInfo> properties = randomValue.GetType().GetProperties()
                .Where(property => property.GetCustomAttribute<ValueAttribute>() == null ||
                property.GetCustomAttribute<ValueAttribute>().ValueType != RandomUtils.Enums.ValueType.NestedThis)
                .Where(property => property.CanWrite && property.GetCustomAttribute<RandomizerIgnoreAttribute>() == null)
                .ToList();
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(IEnumerable<ITestObjects>))
                {
                    Console.WriteLine("List of TestObjects");
                    int index = 0;
                    foreach (var item in (IEnumerable<ITestObjects>)property.GetValue(randomValue))
                    {
                        Console.WriteLine($"[{index}]: " + '{');
                        PrintObjectToConsole(item);
                        Console.WriteLine('}');
                        index++;
                    }
                }
                else if (property.PropertyType == typeof(TestObjectBeta))
                {
                    PrintObjectToConsole((TestObjectBeta)property.GetValue(randomValue));
                }
                else if (property.PropertyType == typeof(TestObjectDelta))
                {
                    PrintObjectToConsole((TestObjectDelta)property.GetValue(randomValue));
                }
                else if (property.PropertyType == typeof(IList<int>))
                {
                    List<int> items = (List<int>)property.GetValue(randomValue);
                    Console.WriteLine("List of int");
                    int index = 0;
                    foreach (var item in items)
                    {
                        Console.WriteLine($"{index}: {item}");
                        index++;
                    }
                }
                else if (property.PropertyType == typeof(LinkedList<int?>))
                {
                    LinkedList<int?> items = (LinkedList<int?>)property.GetValue(randomValue);
                    int index = 0;
                    foreach (int item in items)
                    {
                        Console.WriteLine($"{index}: {item}");
                        index++;
                    }
                }
                else if (property.PropertyType.GetInterface("IEnumerable") != null && property.PropertyType != typeof(string))
                {
                    IEnumerator<object> items = ((IEnumerable<object>)property.GetValue(randomValue)).GetEnumerator();
                    bool continueLoop = true;
                    while (continueLoop)
                    {
                        try
                        {
                            Console.WriteLine(items.Current);
                            items.MoveNext();
                        }
                        catch (InvalidOperationException)
                        {
                            continueLoop = false;
                        }
                    }
                }
                else
                {
                    object propValue = property.GetValue(randomValue);
                    if (propValue != null)
                    {
                        Console.WriteLine($"{property.Name}: {propValue}");
                    }
                    else
                    {
                        Console.WriteLine($"{property.Name}");
                    }
                }
            }
        }
    }
}
