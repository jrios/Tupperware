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
