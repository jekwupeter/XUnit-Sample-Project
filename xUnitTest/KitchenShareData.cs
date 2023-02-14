using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TimiTest.Models;

namespace xUnitTest
{
    public class KitchenShareData
    {
        public static IEnumerable<object[]> KitchenData
        {
            get
            {
                yield return new object[] { 1,
                    (new Kitchen() { Id = 1, Name = "Island", Code = "002" }) };

            }
        }

        public static IEnumerable<Object[]> KitchenDataTwo()
        {
            return new List<object[]>
            {
                new object[] { 1, new Kitchen {Id = 1, Name = "Island", Code = "002" } }
            };
        }

    }
}
