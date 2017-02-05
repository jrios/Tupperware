using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Tupperware
{
    public class Container
    {
        private readonly IConstructorProvider _constructorProvider;
        private readonly ConcurrentDictionary<Type, IRegistration> _registrations;

        public Container() : this(new GreedyConstructorProvider())
        {
        }

        public Container(IConstructorProvider constructorProvider)
        {
            _registrations = new ConcurrentDictionary<Type, IRegistration>();
            _constructorProvider = constructorProvider;
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
            var constructor = _constructorProvider.GetConstructor(registration.ImplementationType);
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