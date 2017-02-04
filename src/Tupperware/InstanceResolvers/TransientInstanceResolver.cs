using System;

namespace Tupperware.InstanceResolvers
{
    public class TransientInstanceResolver<T> : IInstanceResolver<T>
    {
        private readonly Func<T> _resolveInstance;

        public TransientInstanceResolver()
            : this(new GreedyConstructorProvider())
        {
        }

        public TransientInstanceResolver(IConstructorProvider constructorProvider)
        {
            var objectBuilder = new ObjectBuilder<T>(constructorProvider);
            _resolveInstance = () => objectBuilder.CreateInstance();
        }

        public TransientInstanceResolver(object[] arguments)
            : this(new GreedyConstructorProvider(), arguments)
        {
        }

        public TransientInstanceResolver(IConstructorProvider constructorProvider, object[] arguments)
        {
            var objectBuilder = new ObjectBuilder<T>(constructorProvider);
            _resolveInstance = () => objectBuilder.CreateInstance(arguments);
        }

        public T Resolve()
        {
            return _resolveInstance();
        }
    }
}