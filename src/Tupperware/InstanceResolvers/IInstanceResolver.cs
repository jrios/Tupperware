namespace Tupperware.InstanceResolvers
{
    public interface IInstanceResolver<out T>
    {
        T Resolve();
    }
}