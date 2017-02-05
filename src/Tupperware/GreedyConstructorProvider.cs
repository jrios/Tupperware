using System;
using System.Linq;
using System.Reflection;

namespace Tupperware
{
    public class GreedyConstructorProvider : IConstructorProvider
    {
        public ConstructorInfo GetConstructor(Type type)
        {
            return type
                .GetConstructors()
                .OrderByDescending(ctor => ctor.GetParameters().Length)
                .FirstOrDefault();
        }
    }
}