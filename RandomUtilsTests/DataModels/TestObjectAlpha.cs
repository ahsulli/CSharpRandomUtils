using RandomUtils.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtilsTests.DataModels
{
    public class TestObjectAlpha : ITestObjects
    {
        public string testString1 { get; set; }
        public int testInt1 { get; set; }
        public string testString2 { get; set; }
        public TestObjectBeta testBeta1 { get; set; }

        public TestObjectAlpha()
        {

        }

        [Validator(ClassScope = true)]
        public bool RandomizationValidator()
        {
            return this != null;
        }
    }
}
