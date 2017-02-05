using System;
using System.Reflection;
using Tupperware.InstanceResolvers;

namespace Tupperware
{
    internal interface IRegistration
    {
        Type ImplementationType { get; }
        object Resolve(ConstructorInfo constructor, object[] arguments);
    }

    internal class Registration<T> : IRegistration
    {
        private Lazy<IInstanceResolver<T>> _instanceResolver;
        private Lifecycle _lifecycle;
        public Type ImplementationType => typeof(T);

        public Registration(Lifecycle lifecycle)
        {
            _lifecycle = lifecycle;
            _instanceResolver = new Lazy<IInstanceResolver<T>>(isThreadSafe: true);
        }

        public object Resolve(ConstructorInfo constructor, object[] arguments)
        {
            if (_instanceResolver.IsValueCreated)
            {
                return resolve(_instanceResolver.Value, constructor, arguments);
            }

            _instanceResolver = new Lazy<IInstanceResolver<T>>(() =>
            {
                IInstanceResolver<T> resolver = null;
                switch (_lifecycle)
                {
                    case Lifecycle.Transient:
                        resolver = new TransientInstanceResolver<T>();
                        break;
                    case Lifecycle.Singleton:
                        resolver = new SingletonInstanceResolver<T>(constructor, arguments);
                        break;
                }

                return resolver;
            });

            return resolve(_instanceResolver.Value, constructor, arguments);
        }

        private object resolve(IInstanceResolver<T> resolver, ConstructorInfo constructor, object[] arguments)
        {
            return resolver.Resolve(constructor, arguments);
        }

    }
}