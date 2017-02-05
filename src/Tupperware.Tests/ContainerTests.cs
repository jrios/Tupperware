using Shouldly;
using Xunit;

namespace Tupperware.Tests
{
    public class ContainerTests
    {
        [Fact]
        public void container_can_register_types_without_throwing()
        {
            var container = new Container();
            Should.NotThrow(() => container.Register<IBar, Bar>());
        }

        [Fact]
        public void container_will_resolve_type_that_is_registered()
        {
            var container = new Container();
            container.Register<IBar, Bar>();

            var bar = container.Resolve<IBar>();
            bar.ShouldBeOfType<Bar>();
            bar.ShouldBeAssignableTo<IBar>();
            bar.ShouldNotBeNull();
        }

        [Fact]
        public void container_can_resolve_types_that_have_arguments()
        {
            var container = new Container();
            container.Register<IBaz, Baz>();
            container.Register<IArgument, Argument>();

            var baz = container.Resolve<IBaz>();
            baz.ShouldNotBeNull();
            baz.Argument.ShouldNotBeNull();
            baz.Argument.ShouldBeAssignableTo<IArgument>();
        }

        [Fact]
        public void container_will_throw_MissingRegistrationException_when_attempting_to_resolve_non_registered_types()
        {
            var container = new Container();
            Should.Throw<MissingRegistrationException>(() => container.Resolve<IBar>());
        }

        [Fact]
        public void container_can_register_type_with_singleton_lifecycle()
        {
            var container = new Container();
            container.Register<IBar, Bar>(Lifecycle.Singleton);
        }

        [Fact]
        public void container_resolves_transients_with_different_instances_each_time()
        {
            var container = new Container();
            container.Register<IBar, Bar>();

            var bar1 = container.Resolve<IBar>();
            var bar2 = container.Resolve<IBar>();

            bar2.ShouldNotBeSameAs(bar1);
        }

        [Fact]
        public void container_can_resolve_with_nongeneric_resolve_method()
        {
            var container = new Container();
            container.Register<IBar, Bar>();

            var bar = container.Resolve(typeof(IBar));

            bar.ShouldNotBeNull();
        }

        [Fact]
        public void container_resolves_singletons_with_same_instances_each_time()
        {
            var container = new Container();
            container.Register<IBar, Bar>(Lifecycle.Singleton);

            var bar1 = container.Resolve<IBar>();
            var bar2 = container.Resolve<IBar>();

            bar2.ShouldBeSameAs(bar1);
        }

        [Fact]
        public void can_register_a_concrete_type()
        {
            var container = new Container();
            container.Register<Bar>();

            var bar = container.Resolve<Bar>();
            bar.ShouldNotBeNull();
        }
    }

    public interface IBar { }
    public class Bar : IBar { }

    public interface IBaz
    {
        IArgument Argument { get; }
    }

    public class Baz : IBaz
    {
        public IArgument Argument { get; }

        public Baz(IArgument argument)
        {
            Argument = argument;
        }
    }

    public interface IArgument { }

    public class Argument : IArgument { }
}
