using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Tupperware
{
    public class Container
    {
        private readonly IConstructorProvider _constructorProvider;
        private readonly ConcurrentDictionary<Type, IRegistration> _registrations;
        private readonly ConcurrentDictionary<Type, ConstructorInfo> _knownConstructors;

        public Container() : this(new GreedyConstructorProvider())
        {
        }

        public Container(IConstructorProvider constructorProvider)
        {
            _constructorProvider = constructorProvider;
            _registrations = new ConcurrentDictionary<Type, IRegistration>();
            _knownConstructors = new ConcurrentDictionary<Type, ConstructorInfo>();
        }

        public void Register<TRegisteredType, TImplementation>(Lifecycle lifecycle = Lifecycle.Transient)
        {
            _registrations.TryAdd(typeof(TRegisteredType), new Registration<TImplementation>(lifecycle));
        }

        public TRegisteredType Resolve<TRegisteredType>()
        {
            return Resolve(typeof(TRegisteredType)).As<TRegisteredType>();
        }

        private object Resolve(Type resolutionType)
        {
            IRegistration registration;
            if (!_registrations.TryGetValue(resolutionType, out registration))
            {
                throw new MissingRegistrationException(resolutionType);
            }

            var constructor = _knownConstructors.GetOrAdd(registration.ImplementationType,
                type => _constructorProvider.GetConstructor(type));
            var arguments = constructor
                .GetParameters()
                .Select(param => Resolve(param.ParameterType))
                .ToArray();

            return registration.Resolve(constructor, arguments);
        }
    }

    public enum Lifecycle
    {
        Transient,
        Singleton
    }
}