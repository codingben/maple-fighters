using System;
using System.Collections.Generic;

namespace Common.ComponentModel
{
    public interface IComponentCollections : IDisposable
    {
        TComponent Insert<TComponent>(TComponent component)
            where TComponent : class;

        TComponent InsertAndExpose<TComponent>(TComponent component)
            where TComponent : class;

        TComponent Remove<TComponent>()
            where TComponent : class;

        TComponent Find<TComponent>(ExposedState exposedState)
            where TComponent : class;

        IEnumerable<object> GetAll();
    }
}