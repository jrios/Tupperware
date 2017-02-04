using System.Reflection;
using Shouldly;
using Xunit;

namespace Tupperware.Tests
{
    public class KnownConstructorProviderTests
    {
        [Fact]
        public void returns_the_known_constructor()
        {
            var greedyProvider = new GreedyConstructorProvider();
            var knownConstructor = greedyProvider.GetConstructor(typeof(SimpleObject));

            var knownConstructorProvider = new KnownConstructorProvider(knownConstructor);
            var returnedConstructor = knownConstructorProvider.GetConstructor(typeof(SimpleObject));
            returnedConstructor.ShouldBeSameAs(knownConstructor);
        }
    }

    public class GreedyConstructorProviderTests
    {
        [Fact]
        public void the_constructor_with_the_most_parameters_is_returned()
        {
            var greedyProvider = new GreedyConstructorProvider();
            var constructor = greedyProvider.GetConstructor(typeof(SimpleObject));
            constructor.GetParameters().ShouldHaveCount(3);
        }
    }

    internal class SimpleObject
    {
        public SimpleObject()
        {

        }

        public SimpleObject(string first, string second, string third)
        {
        }

        public SimpleObject(string first, string second)
        {
        }
    }

}
