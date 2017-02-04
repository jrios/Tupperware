using Shouldly;
using Xunit;

namespace Tupperware.Tests
{
    public class ObjectBuilderTests
    {
        [Fact]
        public void a_basic_object_with_no_parameters_can_be_created()
        {
            var objectResolver = new ObjectBuilder<BasicObject>(new GreedyConstructorProvider());

            var basic = objectResolver.CreateInstance();
            basic.ShouldNotBeNull();
            basic.Name.ShouldBe("Test");
        }

        [Fact]
        public void a_complex_object_with_dependencies_can_be_created()
        {
            var objectResolver = new ObjectBuilder<ComplexObject>(new GreedyConstructorProvider());
            var complexArg = new Foo();
            var arguments = new object[] { complexArg };

            var complexObject = objectResolver.CreateInstance(arguments);
            complexObject.ComplexFoo.ShouldNotBeNull();
            complexObject.ComplexFoo.ShouldBeSameAs(complexArg);
        }

        private class ComplexObject
        {
            public IFoo ComplexFoo { get; }

            public ComplexObject(IFoo complexFoo)
            {
                ComplexFoo = complexFoo;
            }
        }

        private class BasicObject
        {
            public BasicObject()
            {
                Name = "Test";
            }

            public string Name { get; set; }
        }

        private class Foo : IFoo
        {
        }

        private interface IFoo
        {
        }
    }
}