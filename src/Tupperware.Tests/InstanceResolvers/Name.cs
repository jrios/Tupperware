namespace Tupperware.Tests.InstanceResolvers
{
    public class Name
    {
        private readonly string _firstName;
        private readonly string _lastName;

        public Name()
        {
            _firstName = "Test";
            _firstName = "Test";
            _lastName = "Tester";
        }

        protected bool Equals(Name other)
        {
            return string.Equals(_firstName, other._firstName) && string.Equals(_lastName, other._lastName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Name)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_firstName != null ? _firstName.GetHashCode() : 0) * 397) ^ (_lastName != null ? _lastName.GetHashCode() : 0);
            }
        }
    }
}