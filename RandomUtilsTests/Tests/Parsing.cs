using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomUtils.Parameterization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtilsTests.Tests
{
    [TestClass]
    public class Parsing
    {
        [TestMethod]
        [TestCategory("Parsing")]
        public void UnicodeCharacterFileParsing()
        {
            StringConstants constants = new StringConstants();
            Dictionary<int, char> characterDictionary = constants.GetMapping(RandomUtils.Enums.StringType.AlphaNumeric);
            //Assert.IsTrue(characterDictionary[032] == ' ');
            //Assert.IsTrue(characterDictionary[035] == '#');
            Assert.IsTrue(characterDictionary[048] == '0');
            Assert.IsTrue(characterDictionary[065] == 'A');
            Assert.IsTrue(characterDictionary[097] == 'a');
            //Assert.IsTrue(characterDictionary[414] == 'ƞ');
        }
    }
}
