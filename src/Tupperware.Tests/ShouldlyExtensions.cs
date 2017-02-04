using System.Collections.Generic;
using System.Linq;
using Shouldly;

namespace Tupperware.Tests
{
    public static class ShouldlyExtensions
    {
        public static void ShouldHaveCount<T>(this IEnumerable<T> collection, int count)
        {
            collection.Count().ShouldBe(count);
        }

        public static void ShouldBeOfType<T>(this object actual)
        {
            actual.ShouldBeOfType(typeof(T));
        }
    }
}