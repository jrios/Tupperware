using System;

namespace Tupperware
{
    public class MissingRegistrationException : Exception
    {
        public MissingRegistrationException(Type type)
            : base($"{type} cannot be resolved because it is missing in the container.")
        {
            
        }
    }
}
