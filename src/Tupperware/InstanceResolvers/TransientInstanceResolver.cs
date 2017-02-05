using System;
using System.Reflection;

namespace Tupperware.InstanceResolvers
{
    public class TransientInstanceResolver<T> : IInstanceResolver<T>
    {
        public T Resolve(ConstructorInfo constructor, object[] arguments)
        {
            var objectBuilder = new ObjectBuilder<T>(constructor);
            return objectBuilder.BuildObjectInstance(arguments);
        }
    }
}