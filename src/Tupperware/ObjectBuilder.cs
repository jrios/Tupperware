using System.Reflection;

namespace Tupperware
{
    public class ObjectBuilder<T>
    {
        private readonly ConstructorInfo _objectConstructor;

        public ObjectBuilder(IConstructorProvider ctorProvider)
        {
            _objectConstructor = ctorProvider.GetConstructor(typeof(T));
        }

        public T CreateInstance()
        {
            return CreateInstance(new object[] {});
        }

        public T CreateInstance(object[] arguments)
        {
            return _objectConstructor.Invoke(arguments).As<T>();
        }
    }
}