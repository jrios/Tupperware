namespace Tupperware.Tests.InstanceResolvers
{
    public class ComplexObject
    {
        public IFoo Foo { get; }

        public ComplexObject(IFoo foo)
        {
            Foo = foo;
        }

        protected bool Equals(ComplexObject other)
        {
            return Equals(Foo, other.Foo);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ComplexObject) obj);
        }

        public override int GetHashCode()
        {
            return (Foo != null ? Foo.GetHashCode() : 0);
        }
    }
}