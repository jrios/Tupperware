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
            var objectBuilder = new ObjectBuilder<T>(constructorProvider);
            _instance = new Lazy<T>(() => objectBuilder.CreateInstance(), isThreadSafe: true);
        }

        public SingletonInstanceResolver(IConstructorProvider constructorProvider, object[] arguments)
        {
            var objectBuilder = new ObjectBuilder<T>(constructorProvider);
            _instance = new Lazy<T>(() => objectBuilder.CreateInstance(arguments), isThreadSafe: true);
        }

        public T Resolve()
        {
            return _instance.Value;
        }
    }
}
