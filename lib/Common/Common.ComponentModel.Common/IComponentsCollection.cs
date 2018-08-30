using System;
using System.Collections.Generic;

namespace Common.ComponentModel.Common
{
    public interface IComponentsCollection : IDisposable
    {
        T Add<T>(T component);

        T Remove<T>();

        T Get<T>()
            where T : class;

        IEnumerable<object> GetAll();

        int Count();
    }
}