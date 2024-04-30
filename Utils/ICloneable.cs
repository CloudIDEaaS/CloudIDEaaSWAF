using System;

namespace Utils
{
    public interface ICloneable<T> where T : class
    {
        T ShallowClone();
        T DeepClone();
        T DefaultClone();
    }
}
