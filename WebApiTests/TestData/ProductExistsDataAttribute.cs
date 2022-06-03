using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace WebApiTests.TestData
{
    internal class ProductExistsDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { 1, true };
            yield return new object[] { 2, true };
            yield return new object[] { 3, true };
            yield return new object[] { 4, true };
            yield return new object[] { -1, false };
            yield return new object[] { 0, false };
            yield return new object[] { 11, false };
        }
    }
}
