using System;
using System.Linq;
using System.Reflection;

namespace Tupperware
{
    public interface IConstructorProvider
    {
        ConstructorInfo GetConstructor(Type implementationType);
    }

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

    public class KnownConstructorProvider : IConstructorProvider
    {
        private readonly ConstructorInfo _knownConstructor;

        public KnownConstructorProvider(ConstructorInfo knownConstructor)
        {
            _knownConstructor = knownConstructor;
        }

        public ConstructorInfo GetConstructor(Type implementationType)
        {
            return _knownConstructor;
        }
    }
}