using System.Reflection;
using Shouldly;
using Tupperware.InstanceResolvers;
using Xunit;

namespace Tupperware.Tests.InstanceResolvers
{
    public class SingletonInstanceResolverTests
    {
        private readonly ConstructorInfo _nameConstructor;
        private readonly ConstructorInfo _complexObjectConstructor;


        public SingletonInstanceResolverTests()
        {
            _nameConstructor = typeof(Name).GetConstructors()[0];
            _complexObjectConstructor = typeof(ComplexObject).GetConstructors()[0];
        }

        [Fact]
        public void resolving_a_singleton_returns_same_object()
        {
            var emptyArgs = new object[] { };

            var singletonResolver = new SingletonInstanceResolver<Name>(_nameConstructor, emptyArgs);
            var instance1 = singletonResolver.Resolve(_nameConstructor, emptyArgs);
            var instance2 = singletonResolver.Resolve(_nameConstructor, emptyArgs);

            instance2.ShouldBeSameAs(instance1);
        }

        [Fact]
        public void resolving_a_singleton_with_known_arguments()
        {
            var foo = new Foo();
            var args = new object[] {foo};
            var singletonResolver = new SingletonInstanceResolver<ComplexObject>(_complexObjectConstructor, args);

            var instance1 = singletonResolver.Resolve(_complexObjectConstructor, args);
            var instance2 = singletonResolver.Resolve(_complexObjectConstructor, args);

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