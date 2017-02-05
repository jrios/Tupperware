using System.Reflection;

namespace Tupperware.InstanceResolvers
{
    public interface IInstanceResolver<out T>
    {
        T Resolve(ConstructorInfo constructor, object[] arguments);
    }
}