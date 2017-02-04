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
            bar.ShouldImplement<IBar>();
            bar.ShouldNotBeNull();
        }
    }

    public class Bar : IBar
    {
    }

    public interface IBar
    {
    }

    internal class Registration
    {
    }
}
