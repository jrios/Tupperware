using System.Reflection;
using Shouldly;
using Tupperware.ExceptionTypes;
using Xunit;

namespace Tupperware.Tests
{
    public class ObjectBuilderTests
    {
        private ConstructorInfo _basicObjectConstructor;
        private ConstructorInfo _complexObjectConstructor;

        public ObjectBuilderTests()
        {
            _basicObjectConstructor = typeof(BasicObject).GetConstructors()[0];
            _complexObjectConstructor = typeof(ComplexObject).GetConstructors()[0];
        }

        [Fact]
        public void a_basic_object_with_no_parameters_can_be_created()
        {

            var objectResolver = new ObjectBuilder<BasicObject>(_basicObjectConstructor);

            var basic = objectResolver.BuildObjectInstance();
            basic.ShouldNotBeNull();
            basic.Name.ShouldBe("Test");
        }

        [Fact]
        public void a_complex_object_with_dependencies_can_be_created()
        {
            var objectResolver = new ObjectBuilder<ComplexObject>(_complexObjectConstructor);
            var complexArg = new Foo();
            var arguments = new object[] { complexArg };

            var complexObject = objectResolver.BuildObjectInstance(arguments);
            complexObject.ComplexFoo.ShouldNotBeNull();
            complexObject.ComplexFoo.ShouldBeSameAs(complexArg);
        }

        [Fact]
        public void a_complex_object_with_no_arguments_passed_will_throw()
        {
            var objectResolver = new ObjectBuilder<ComplexObject>(_complexObjectConstructor);

            Should.Throw<UnresolvedParametersException>(() => objectResolver.BuildObjectInstance());
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