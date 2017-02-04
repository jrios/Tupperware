using System;
using System.Collections.Concurrent;
using System.Linq;
using Tupperware.InstanceResolvers;

namespace Tupperware
{
    public class Container
    {
        private readonly IConstructorProvider _constructorProvider;
        private readonly ConcurrentDictionary<Type, Registration> _registrations;

        public Container()
            : this(new GreedyConstructorProvider())
        {
        }

        public Container(IConstructorProvider constructorProvider)
        {
            _registrations = new ConcurrentDictionary<Type, Registration>();
            _constructorProvider = constructorProvider;
        }

        public void Register<TRegisteredType, TImplementation>()
        {
            _registrations.TryAdd(typeof(TRegisteredType), Registration.For<TImplementation>());
        }

        public TRegisteredType Resolve<TRegisteredType>()
        {
            return Resolve(typeof(TRegisteredType)).As<TRegisteredType>();
        }

        private object Resolve(Type resolutionType)
        {
            Registration registration;
            if (!_registrations.TryGetValue(resolutionType, out registration))
            {
                throw new MissingRegistrationException(resolutionType);
            }
            var constructor = _constructorProvider.GetConstructor(registration.ImplementationType);
            var arguments = constructor
                .GetParameters()
                .Select(param => Resolve(param.ParameterType))
                .ToArray();

            return registration.Resolve(_constructorProvider, arguments);
        }
    }

    public class Registration
    {
        public Func<IConstructorProvider, object[], object> Resolver { get; private set; }
        public Type ImplementationType { get; private set; }

        public static Registration For<T>(Lifecycle lifecycle = Lifecycle.Transient)
        {
            Func<IConstructorProvider, object[], object> resolver = null;

            switch (lifecycle)
            {
                case Lifecycle.Transient:
                    resolver = resolveTransient<T>();
                    break;
                case Lifecycle.Singleton:
                    resolver = resolveSingleton<T>();
                    break;
            }

            return new Registration
            {
                ImplementationType = typeof(T),
                Resolver = resolver
            };
        }

        private static Func<IConstructorProvider, object[], object> resolveSingleton<T>()
        {
            return (constructorProvider, arguments) =>
            {
                var resolver = new SingletonInstanceResolver<T>(constructorProvider, arguments);
                return resolver.Resolve();
            };
        }

        private static Func<IConstructorProvider, object[], object> resolveTransient<T>()
        {
            return (constructorProvider, arguments) =>
            {
                var resolver = new TransientInstanceResolver<T>(constructorProvider, arguments);
                return resolver.Resolve();
            };
        }

        public object Resolve(IConstructorProvider constructorProvider, object[] arguments)
        {
            return Resolver(constructorProvider, arguments);
        }
    }

    public enum Lifecycle
    {
        Transient,
        Singleton
    }
}