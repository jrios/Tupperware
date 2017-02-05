using Xunit;

namespace Tupperware.Tests
{
    public class GreedyConstructorProviderTests
    {
        [Fact]
        public void the_constructor_with_the_most_parameters_is_returned()
        {
            var greedyProvider = new GreedyConstructorProvider();
            var constructor = greedyProvider.GetConstructor(typeof(SimpleObject));
            constructor.GetParameters().ShouldHaveCount(3);
        }

        private class SimpleObject
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
}