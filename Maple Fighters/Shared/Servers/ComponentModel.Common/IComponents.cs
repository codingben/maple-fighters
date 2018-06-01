using System;

namespace ComponentModel.Common
{
    public interface IComponents : IDisposable
    {
        T Add<T>(T component)
            where T : IDisposable;

        void Remove<T>()
            where T : IDisposable;

        T Get<T>();

        int Count();
    }
}