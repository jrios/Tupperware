using Shouldly;
using Tupperware.InstanceResolvers;
using Xunit;

namespace Tupperware.Tests.InstanceResolvers
{
    public class SingletonInstanceResolverTests
    {
        [Fact]
        public void resolving_a_singleton_returns_same_object()
        {
            var singletonResolver = new SingletonInstanceResolver<Name>();
            var instance1 = singletonResolver.Resolve();
            var instance2 = singletonResolver.Resolve();

            instance2.ShouldBeSameAs(instance1);
        }

        [Fact]
        public void resolving_a_singleton_with_known_arguments()
        {
            var foo = new Foo();
            var args = new object[] {foo};
            var singletonResolver = new SingletonInstanceResolver<ComplexObject>(args);

            var instance1 = singletonResolver.Resolve();
            var instance2 = singletonResolver.Resolve();

            instance2.ShouldBeSameAs(instance1);
        }
    }

    public interface IFoo
    {
    }

    public class Foo : IFoo
    {
        private readonly string _bar;

        public Foo()
        {
            _bar = "bar";
        }

        protected bool Equals(Foo other)
        {
            return string.Equals(_bar, other._bar);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Foo) obj);
        }

        public override int GetHashCode()
        {
            return (_bar != null ? _bar.GetHashCode() : 0);
        }
    }
}