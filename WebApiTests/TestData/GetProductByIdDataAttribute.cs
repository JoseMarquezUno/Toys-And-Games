using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace WebApiTests.TestData
{
    public class GetProductByIdDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] {1,false};
            yield return new object[] {2,false};
            yield return new object[] {3,false};
            yield return new object[] {4,false};
            yield return new object[] {-1,true};
            yield return new object[] {0,true};
            yield return new object[] {100,true};
            yield return new object[] {15,true};
        }
    }
}
