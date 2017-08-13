using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomUtils.Attributes;
using RandomUtils.Attributes.Collections;

namespace RandomUtilsTests.DataModels
{
    public class TestObjectBeta : ITestObjects
    {
        public string testString1 { get; set; }
        public int testInt1 { get; set; }
        public bool testBool1 { get; set; }
        public DateTime testDateTime1 { get; set; }

        [ConcreteType(typeof(List<int>))]
        [Required]
        public IList<int> testIList1 { get; set; }

        [ConcreteType(typeof(List<TestObjectDelta>))]
        [CollectionItemConcreteType(typeof(TestObjectDelta))]
        [Required]
        [MinimumLength(1)]
        public IEnumerable<ITestObjects> deltas { get; set; }

        [Validator("testIList1")]
        public bool ValidList(ICollection<int> list)
        {
            return list.Count > 0;
        }

        [CollectionItemValidator("testIList1")]
        public bool ValidListItem(int listItem)
        {
            return listItem > 0;
        }
    }
}
