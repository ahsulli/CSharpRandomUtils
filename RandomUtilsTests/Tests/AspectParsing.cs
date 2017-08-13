using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RandomUtils.Enums;
using RandomUtils.Parameterization;
using RandomUtils.Randomizer;
using RandomUtilsTests.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtilsTests.Tests
{
    [TestClass]
    public class AspectParsing
    {
        public static AspectModelCollection MockAspectCollection { get; set; }

        [ClassInitialize]
        public static void CreateMockObjects(TestContext context)
        {
            AspectModel aspect1 = new AspectModel()
            {
                PropertyName = "testString1",
                ClassName = "TestObjectAlpha",
                Required = true,
                Minimum = 1,
                Maximum = 5,
                StringType = StringType.AlphaNumeric
            };

            AspectModel aspect2 = new AspectModel()
            {
                PropertyName = "testInt1",
                Required = true,
                Maximum = 10
            };

            AspectModel aspect3 = new AspectModel()
            {
                PropertyName = "testString2",
                Required = false,
                Minimum = 3,
                Maximum = 25,
                StringType = StringType.All
            };

            AspectModel aspect4 = new AspectModel()
            {
                PropertyName = "testBeta1",
                Required = true
            };

            AspectModel aspect5 = new AspectModel()
            {
                PropertyName = "testBool1",
                Required = true
            };

            AspectModel aspect6 = new AspectModel()
            {
                PropertyName = "testString1",
                ClassName = "TestObjectBeta",
                Required = false,
                Minimum = 6,
                Maximum = 40,
                StringType = StringType.Regex,
                RegexPattern = "Gr8 [A-Za-z]{2,36}"
            };

            AspectModel aspect7 = new AspectModel()
            {
                PropertyName = "testDateTime1",
                Minimum = DefaultValues.DEFAULTMINIMUM,
                MinimumLength = DefaultValues.DEFAULTMINIMUMLENGTH,
                Maximum = DefaultValues.DEFAULTMAXIMUM,
                MaximumLength = DefaultValues.DEFAULTMAXIMUMLENGTH,
                EarliestDateTime = new DateTime(2015, 1, 1, 0, 0, 0),
                LatestDateTime = new DateTime(2016, 1, 1, 0, 0, 0),
                Required = true
            };

            AspectModel aspect8 = new AspectModel()
            {
                PropertyName = "testIList1",
                Minimum = DefaultValues.DEFAULTMINIMUM,
                MinimumLength = DefaultValues.DEFAULTMINIMUMLENGTH,
                Maximum = DefaultValues.DEFAULTMAXIMUM,
                MaximumLength = DefaultValues.DEFAULTMAXIMUMLENGTH,
                Required = false
            };

            AspectModel aspect9 = new AspectModel()
            {
                PropertyName = "deltas",
                Minimum = DefaultValues.DEFAULTMINIMUM,
                MinimumLength = 1,
                Maximum = DefaultValues.DEFAULTMAXIMUM,
                MaximumLength = 5,
                Required = true
            };

            AspectModel aspect10 = new AspectModel()
            {
                PropertyName = "testString1",
                ClassName = "TestObjectDelta",
                Minimum = 0,
                Maximum = 25,
                Required = false
            };

            AspectModel aspect11 = new AspectModel()
            {
                PropertyName = "testStringArray1",
                Minimum = 0,
                Maximum = 25,
                MinimumLength = 1,
                MaximumLength = 10,
                Required = false,
                StringType = StringType.Alphabetic
            };

            MockAspectCollection = new AspectModelCollection();
            MockAspectCollection.Models.Add(aspect1);
            MockAspectCollection.Models.Add(aspect2);
            MockAspectCollection.Models.Add(aspect3);
            MockAspectCollection.Models.Add(aspect4);
            MockAspectCollection.Models.Add(aspect5);
            MockAspectCollection.Models.Add(aspect6);
            MockAspectCollection.Models.Add(aspect7);
            MockAspectCollection.Models.Add(aspect8);
            MockAspectCollection.Models.Add(aspect9);
            MockAspectCollection.Models.Add(aspect10);
            MockAspectCollection.Models.Add(aspect11);
        }

        [TestMethod]
        [TestCategory("Parsing")]
        public void CSVParsing()
        {
            ParseAndValidate("RandomUtilsTests.InputFiles.TestDataConstraints.csv");
        }

        [TestMethod]
        [TestCategory("Parsing")]
        public void JSONParsing()
        {
            ParseAndValidate("RandomUtilsTests.InputFiles.TestDataConstraints.json");
        }

        [TestMethod]
        [TestCategory("Parsing")]
        public void XMLParsing()
        {
            ParseAndValidate("RandomUtilsTests.InputFiles.TestDataConstraints.xml");
        }

        [TestMethod]
        [TestCategory("Parsing")]
        public void YAMLParsing()
        {
            ParseAndValidate("RandomUtilsTests.InputFiles.TestDataConstraints.yaml");
        }

        private void ParseAndValidate(string fileName)
        {
            Parser parser = new Parser(Assembly.GetExecutingAssembly(), fileName);
            List<AspectModel> models = parser.ParseAspects();

            Assert.IsTrue(models.Count == MockAspectCollection.Models.Count);
            for (int i = 0; i < models.Count; i++)
            {
                Console.WriteLine($"Starting to validate {models[i].PropertyName}");
                Assert.IsTrue(models[i].PropertyName == MockAspectCollection.Models[i].PropertyName);
                Assert.IsTrue(models[i].Maximum == MockAspectCollection.Models[i].Maximum);
                Assert.IsTrue(models[i].Minimum == MockAspectCollection.Models[i].Minimum);
                Assert.IsTrue(models[i].Required == MockAspectCollection.Models[i].Required);
                Assert.IsTrue(models[i].ClassName == MockAspectCollection.Models[i].ClassName);
                Assert.IsTrue(models[i].MaximumLength == MockAspectCollection.Models[i].MaximumLength);
                Assert.IsTrue(models[i].MinimumLength == MockAspectCollection.Models[i].MinimumLength);
                Assert.IsTrue(models[i].EarliestDateTime == MockAspectCollection.Models[i].EarliestDateTime);
                Assert.IsTrue(models[i].LatestDateTime == MockAspectCollection.Models[i].LatestDateTime);
                Assert.IsTrue(models[i].StringType == MockAspectCollection.Models[i].StringType);
                Assert.IsTrue(models[i].RegexPattern == MockAspectCollection.Models[i].RegexPattern);
                Console.WriteLine($"Finished validating {MockAspectCollection.Models[i].PropertyName}");
            }
        }
    }
}
