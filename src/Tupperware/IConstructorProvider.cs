using System;
using System.Reflection;

namespace Tupperware
{
    public interface IConstructorProvider
    {
        ConstructorInfo GetConstructor(Type implementationType);
    }
}