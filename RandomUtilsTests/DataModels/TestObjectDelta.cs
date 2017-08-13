using RandomUtils.Attributes;
using RandomUtils.Attributes.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomUtilsTests.DataModels
{
    public class TestObjectDelta : ITestObjects
    {
        public string testString1 { get; set; }

        [RandomizerIgnore]
        public uint unsignedInt1 { get; set; }

        [RandomizerIgnore]
        public decimal testFloat1 { get; set; }

        internal bool? testBool1 { get; set; }

        [MinimumLength(2)]
        [MaximumLength(5)]
        public LinkedList<int?> testNullableIntLinkedList1 { get; set; }

        [Value(RandomUtils.Enums.ValueType.NestedThis)]
        public TestObjectBeta testObjectBeta { get; set; }
    }
}
