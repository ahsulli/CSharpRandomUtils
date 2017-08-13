using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomUtils.Enums;
using RandomUtils.Parameterization;
using RandomUtils.Reflection;
using RandomUtilsTests.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtilsTests.Tests
{
    [TestClass]
    public class ReflectorTest
    {
        internal static List<Aspect> MockAspects { get; set; }

        [ClassInitialize]
        public static void CreateMockObjects(TestContext context)
        {
            Aspect aspect1 = new Aspect()
            {
                Property = typeof(TestObjectAlpha).GetProperty("testString1"),
                ConcreteType = typeof(string),
                Minimum = 1,
                MinimumLength = 0,
                Maximum = 5,
                MaximumLength = 0,
                Required = true,
                StringType = StringType.AlphaNumeric,
                ValueType = RandomUtils.Enums.ValueType.DefaultValue,
                Validator = null
            };

            Aspect aspect2 = new Aspect()
            {
                Property = typeof(TestObjectAlpha).GetProperty("testInt1"),
                ConcreteType = typeof(int),
                Minimum = DefaultValues.DEFAULTMINIMUM,
                MinimumLength = 0,
                Maximum = 10,
                MaximumLength = 0,
                Required = true,
                ValueType = RandomUtils.Enums.ValueType.DefaultValue,
                Validator = null
            };

            Aspect aspect3 = new Aspect()
            {
                Property = typeof(TestObjectAlpha).GetProperty("testString2"),
                ConcreteType = typeof(string),
                Minimum = 3,
                MinimumLength = 0,
                Maximum = 25,
                MaximumLength = 0,
                Required = false,
                StringType = StringType.All,
                ValueType = RandomUtils.Enums.ValueType.DefaultValue,
                Validator = null
            };

            Aspect aspect4 = new Aspect()
            {
                Property = typeof(TestObjectAlpha).GetProperty("testBeta1"),
                ConcreteType = typeof(TestObjectBeta),
                Minimum = DefaultValues.DEFAULTMINIMUM,
                MinimumLength = 0,
                Maximum = DefaultValues.DEFAULTMAXIMUM,
                MaximumLength = 0,
                Required = true,
                ValueType = RandomUtils.Enums.ValueType.DefaultValue,
                Validator = null
            };

            Aspect aspect5 = new Aspect()
            {
                Property = typeof(TestObjectBeta).GetProperty("testString1"),
                ConcreteType = typeof(string),
                Minimum = 6,
                MinimumLength = 0,
                Maximum = 40,
                MaximumLength = 0,
                Required = false,
                StringType = StringType.Regex,
                RegexPattern = "Gr8 [A-Za-z]{2,36}",
                ValueType = RandomUtils.Enums.ValueType.DefaultValue,
                Validator = null
            };

            Aspect aspect6 = new Aspect()
            {
                Property = typeof(TestObjectBeta).GetProperty("testInt1"),
                ConcreteType = typeof(int),
                Minimum = DefaultValues.DEFAULTMINIMUM,
                MinimumLength = 0,
                Maximum = 10,
                MaximumLength = 0,
                Required = true,
                ValueType = RandomUtils.Enums.ValueType.DefaultValue,
                Validator = null
            };

            Aspect aspect7 = new Aspect()
            {
                Property = typeof(TestObjectBeta).GetProperty("testBool1"),
                ConcreteType = typeof(bool),
                Minimum = DefaultValues.DEFAULTMINIMUM,
                MinimumLength = 0,
                Maximum = DefaultValues.DEFAULTMAXIMUM,
                MaximumLength = 0,
                Required = true,
                ValueType = RandomUtils.Enums.ValueType.DefaultValue,
                Validator = null
            };

            Aspect aspect8 = new Aspect()
            {
                Property = typeof(TestObjectBeta).GetProperty("testDateTime1"),
                ConcreteType = typeof(DateTime),
                Minimum = DefaultValues.DEFAULTMINIMUM,
                MinimumLength = 0,
                Maximum = DefaultValues.DEFAULTMAXIMUM,
                MaximumLength = 0,
                EarliestDateTime = new DateTime(2015, 1, 1, 0, 0, 0),
                LatestDateTime = new DateTime(2016, 1, 1, 0, 0, 0),
                Required = true,
                ValueType = RandomUtils.Enums.ValueType.DefaultValue,
                Validator = null
            };

            Aspect aspect9 = new Aspect()
            {
                Property = typeof(TestObjectBeta).GetProperty("testIList1"),
                ConcreteType = typeof(List<int>),
                ConcreteCollectionItemType = typeof(int),
                Minimum = 1,
                MinimumLength = DefaultValues.DEFAULTMINIMUMLENGTH,
                Maximum = DefaultValues.DEFAULTMAXIMUM,
                MaximumLength = DefaultValues.DEFAULTMAXIMUMLENGTH,
                Required = true,
                ValueType = RandomUtils.Enums.ValueType.DefaultValue,
                Validator = typeof(TestObjectBeta).GetMethod("ValidList"),
                CollectionItemValidator = typeof(TestObjectBeta).GetMethod("ValidListItem")
            };

            Aspect aspect10 = new Aspect()
            {
                Property = typeof(TestObjectBeta).GetProperty("deltas"),
                ConcreteType = typeof(List<TestObjectDelta>),
                ConcreteCollectionItemType = typeof(TestObjectDelta),
                Minimum = DefaultValues.DEFAULTMINIMUM,
                MinimumLength = 1,
                Maximum = DefaultValues.DEFAULTMAXIMUM,
                MaximumLength = 5,
                Required = true,
                ValueType = RandomUtils.Enums.ValueType.DefaultValue,
                Validator = null
            };

            Aspect aspect11 = new Aspect()
            {
                Property = typeof(TestObjectDelta).GetProperty("testString1"),
                ConcreteType = typeof(string),
                Minimum = 0,
                MinimumLength = 0,
                Maximum = 25,
                MaximumLength = 0,
                Required = false,
                StringType = StringType.AlphaNumeric,
                ValueType = RandomUtils.Enums.ValueType.DefaultValue,
                Validator = null
            };

            Aspect aspect12 = new Aspect()
            {
                Property = typeof(TestObjectDelta).GetProperty("testNullableIntLinkedList1"),
                ConcreteType = typeof(LinkedList<int?>),
                ConcreteCollectionItemType = typeof(int?),
                Minimum = DefaultValues.DEFAULTMINIMUM,
                Maximum = DefaultValues.DEFAULTMAXIMUM,
                MinimumLength = 2,
                MaximumLength = 5,
                Required = false,
                ValueType = RandomUtils.Enums.ValueType.DefaultValue,
                Validator = null
            };

            Aspect aspect13 = new Aspect()
            {
                Property = typeof(TestObjectDelta).GetProperty("testObjectBeta"),
                ConcreteType = typeof(TestObjectBeta),
                Minimum = DefaultValues.DEFAULTMINIMUM,
                Maximum = DefaultValues.DEFAULTMAXIMUM,
                Required = true,
                ValueType = RandomUtils.Enums.ValueType.NestedThis,
                Validator = null
            };

            MockAspects = new List<Aspect>()
            {
                aspect1,
                aspect2,
                aspect3,
                aspect4,
                aspect5,
                aspect6,
                aspect7,
                aspect8,
                aspect9,
                aspect10,
                aspect11,
                aspect12,
                aspect13
            };
        }

        [TestMethod]
        [TestCategory("Reflector")]
        public void CSVReflector()
        {
            ReflectAndAssert("RandomUtilsTests.InputFiles.TestDataConstraints.csv");
        }

        [TestMethod]
        [TestCategory("Reflector")]
        public void JSONReflector()
        {
            ReflectAndAssert("RandomUtilsTests.InputFiles.TestDataConstraints.json");
        }

        [TestMethod]
        [TestCategory("Reflector")]
        public void XMLReflector()
        {
            ReflectAndAssert("RandomUtilsTests.InputFiles.TestDataConstraints.xml");
        }

        [TestMethod]
        [TestCategory("Reflector")]
        public void YAMLReflector()
        {
            ReflectAndAssert("RandomUtilsTests.InputFiles.TestDataConstraints.yaml");
        }

        private void ReflectAndAssert(string resourceName)
        {
            Reflector<TestObjectAlpha> reflectorAlpha = new Reflector<TestObjectAlpha>(Assembly.GetExecutingAssembly());
            reflectorAlpha.Analyze(resourceName);
            List<Aspect> models = reflectorAlpha.Aspects;
            Reflector<TestObjectBeta> reflectorBeta = new Reflector<TestObjectBeta>(Assembly.GetExecutingAssembly());
            reflectorBeta.Analyze(resourceName);
            models.AddRange(reflectorBeta.Aspects);
            Reflector<TestObjectDelta> reflectorDelta = new Reflector<TestObjectDelta>(Assembly.GetExecutingAssembly());
            reflectorDelta.Analyze(resourceName);
            models.AddRange(reflectorDelta.Aspects);

            Assert.IsTrue(models.Count == MockAspects.Count);
            for (int i = 0; i < models.Count; i++)
            {
                Console.WriteLine($"Starting to validate {models[i].Property.Name}");
                Assert.IsTrue(models[i].Property == MockAspects[i].Property);
                Assert.IsTrue(models[i].Maximum == MockAspects[i].Maximum);
                Assert.IsTrue(models[i].Minimum == MockAspects[i].Minimum);
                Assert.IsTrue(models[i].Required == MockAspects[i].Required);
                Assert.IsTrue(models[i].ConcreteType == MockAspects[i].ConcreteType);
                Assert.IsTrue(models[i].MaximumLength == MockAspects[i].MaximumLength);
                Assert.IsTrue(models[i].MinimumLength == MockAspects[i].MinimumLength);
                Assert.IsTrue(models[i].Validator == MockAspects[i].Validator);
                Assert.IsTrue(models[i].ConcreteCollectionItemType == MockAspects[i].ConcreteCollectionItemType);
                Assert.IsTrue(models[i].CollectionItemValidator == MockAspects[i].CollectionItemValidator);
                Assert.IsTrue(models[i].EarliestDateTime == MockAspects[i].EarliestDateTime);
                Assert.IsTrue(models[i].LatestDateTime == MockAspects[i].LatestDateTime);
                Assert.IsTrue(models[i].StringType == MockAspects[i].StringType);
                Assert.IsTrue(models[i].RegexPattern == MockAspects[i].RegexPattern);
                Assert.IsTrue(models[i].ValueType == MockAspects[i].ValueType);
                Console.WriteLine($"Finished validating {models[i].Property.Name}");
            }
            Assert.IsTrue(reflectorAlpha.RandomizationValidator == typeof(TestObjectAlpha).GetMethod("RandomizationValidator"));
        }
    }
}
