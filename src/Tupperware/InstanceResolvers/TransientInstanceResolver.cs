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
            var ctor = constructorProvider.GetConstructor(typeof(T));
            var objectBuilder = new ObjectBuilder<T>(ctor);
            _resolveInstance = () => objectBuilder.BuildObjectInstance();
        }

        public TransientInstanceResolver(object[] arguments)
            : this(new GreedyConstructorProvider(), arguments)
        {
        }

        public TransientInstanceResolver(IConstructorProvider constructorProvider, object[] arguments)
        {
            var ctor = constructorProvider.GetConstructor(typeof(T));
            var objectBuilder = new ObjectBuilder<T>(ctor);
            _resolveInstance = () => objectBuilder.BuildObjectInstance(arguments);
        }

        public T Resolve()
        {
            return _resolveInstance();
        }
    }
}