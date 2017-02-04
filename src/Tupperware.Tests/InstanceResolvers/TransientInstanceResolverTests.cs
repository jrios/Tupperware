using Shouldly;
using Tupperware.InstanceResolvers;
using Xunit;

namespace Tupperware.Tests.InstanceResolvers
{
    public class TransientInstanceResolverTests
    {
        [Fact]
        public void resolves_a_new_instance_every_time()
        {
            var transientResolver = new TransientInstanceResolver<Name>();

            var instance1 = transientResolver.Resolve();
            var instance2 = transientResolver.Resolve();

            instance2.ShouldNotBeSameAs(instance1);
        }

        [Fact]
        public void resolves_a_new_instance_with_known_arguments()
        {
            var foo = new Foo();
            var transientResolver = new TransientInstanceResolver<ComplexObject>(new object[] {foo});

            var instance1 = transientResolver.Resolve();
            var instance2 = transientResolver.Resolve();

            instance2.ShouldNotBeSameAs(instance1);
        }
    }
}
