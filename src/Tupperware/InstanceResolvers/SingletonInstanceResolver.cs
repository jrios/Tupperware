using System;
using System.Reflection;

namespace Tupperware.InstanceResolvers
{
    public class SingletonInstanceResolver<T> : IInstanceResolver<T>
    {
        private readonly Lazy<T> _instance;

        public SingletonInstanceResolver(ConstructorInfo constructorInfo, object[] arguments)
        {
            _instance = new Lazy<T>(() =>
            {
                var objectBuilder = new ObjectBuilder<T>(constructorInfo);
                return objectBuilder.BuildObjectInstance(arguments);
            });
        }

        public T Resolve(ConstructorInfo constructor, object[] arguments)
        {
            return _instance.Value;
        }
    }
}
