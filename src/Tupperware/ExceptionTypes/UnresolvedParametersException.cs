using System;

namespace Tupperware.ExceptionTypes
{
    public class UnresolvedParametersException : Exception
    {
        public UnresolvedParametersException(Type type) : 
            base($"The parameters for {type} could not be resolved." +
                 $" Make sure all parameters for {type} are registered with the container.")
        {
        }
    }
}
