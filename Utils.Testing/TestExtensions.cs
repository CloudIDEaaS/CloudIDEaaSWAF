using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Utils
{
    public static class TestExtensions
    {
        public static IEnumerable<Type> GetTestTypes(this Assembly testAssembly)
        {
            return testAssembly.GetTypes().Where(t => t.GetMethods().Any(m => m.HasCustomAttribute<FactAttribute>()));
        }

        public static IEnumerable<Type> GetTestTypes(this Assembly testAssembly, TestKind testKind)
        {
            return testAssembly.GetTypes().Where(t => t.GetMethods().Any(m => m.HasCustomAttribute<FactAttribute>() && t.GetCustomAttribute<TestKindAttribute>().TestKind == testKind));
        }

        public static IEnumerable<MethodInfo> GetTests(this Assembly testAssembly, TestKind testKind)
        {
            var testTypes = testAssembly.GetTestTypes().Where(t => t.HasCustomAttribute<TestKindAttribute>() && t.GetCustomAttribute<TestKindAttribute>().TestKind == testKind);

            return testTypes.SelectMany(t => t.GetMethods().Where(m => m.HasCustomAttribute<FactAttribute>()));
        }
    }
}
