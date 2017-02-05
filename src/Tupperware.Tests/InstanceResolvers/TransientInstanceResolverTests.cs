using System.Reflection;
using Shouldly;
using Tupperware.InstanceResolvers;
using Xunit;

namespace Tupperware.Tests.InstanceResolvers
{
    public class TransientInstanceResolverTests
    {
        private readonly ConstructorInfo _nameConstructor;
        private readonly ConstructorInfo _complexObjectConstructor;

        public TransientInstanceResolverTests()
        {
            _nameConstructor = typeof(Name).GetConstructors()[0];
            _complexObjectConstructor = typeof(ComplexObject).GetConstructors()[0];
        }

        [Fact]
        public void resolves_a_new_instance_every_time()
        {
            var transientResolver = new TransientInstanceResolver<Name>();

            var instance1 = transientResolver.Resolve(_nameConstructor, new object[] {});
            var instance2 = transientResolver.Resolve(_nameConstructor, new object[] {});

            instance2.ShouldNotBeSameAs(instance1);
        }

        [Fact]
        public void resolves_a_new_instance_with_known_arguments()
        {
            var foo = new Foo();
            var transientResolver = new TransientInstanceResolver<ComplexObject>();
            var args = new object[] {foo};

            var instance1 = transientResolver.Resolve(_complexObjectConstructor, args);
            var instance2 = transientResolver.Resolve(_complexObjectConstructor, args);

            instance2.ShouldNotBeSameAs(instance1);
        }
    }
}
