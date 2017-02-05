using System;

namespace Tupperware.InstanceResolvers
{
    public class SingletonInstanceResolver<T> : IInstanceResolver<T>
    {
        private readonly Lazy<T> _instance;

        public SingletonInstanceResolver()
            : this(new GreedyConstructorProvider())
        {
        }

        public SingletonInstanceResolver(object[] arguments)
            : this(new GreedyConstructorProvider(), arguments)
        {

        }

        public SingletonInstanceResolver(IConstructorProvider constructorProvider)
        {
            var ctor = constructorProvider.GetConstructor(typeof(T));
            var objectBuilder = new ObjectBuilder<T>(ctor);
            _instance = new Lazy<T>(() => objectBuilder.BuildObjectInstance(), isThreadSafe: true);
        }

        public SingletonInstanceResolver(IConstructorProvider constructorProvider, object[] arguments)
        {
            var ctor = constructorProvider.GetConstructor(typeof(T));
            var objectBuilder = new ObjectBuilder<T>(ctor);
            _instance = new Lazy<T>(() => objectBuilder.BuildObjectInstance(arguments), isThreadSafe: true);
        }

        public T Resolve()
        {
            return _instance.Value;
        }
    }
}
