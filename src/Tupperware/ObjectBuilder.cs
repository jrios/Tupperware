using System.Linq;
using System.Reflection;
using Tupperware.ExceptionTypes;

namespace Tupperware
{
    public class ObjectBuilder<T>
    {
        private readonly ConstructorInfo _objectConstructor;
        private readonly int _parameterCount;

        public ObjectBuilder(ConstructorInfo objectConstructor)
        {
            _objectConstructor = objectConstructor;
            _parameterCount = _objectConstructor.GetParameters().Length;
        }

        public T BuildObjectInstance()
        {
            return BuildObjectInstance(new object[] {});
        }

        public T BuildObjectInstance(object[] arguments)
        {
            if (_parameterCount != arguments.Length)
            {
                throw new UnresolvedParametersException(_objectConstructor.DeclaringType);
            }
            return _objectConstructor.Invoke(arguments).As<T>();
        }
    }
}