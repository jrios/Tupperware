using System;
using System.Collections.Concurrent;
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
            var registration = _registrations[typeof(TRegisteredType)];
            var arguments = new object[] {};
            return registration.Resolve(_constructorProvider, arguments).As<TRegisteredType>();
        }
        
    }

    public class Registration
    {
        public Func<IConstructorProvider, object[], object> Resolver { get; set; }

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