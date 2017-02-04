namespace Tupperware
{
    public static class TypeExtensions
    {
        public static T As<T>(this object castee)
        {
            return (T) castee;
        }
    }
}
