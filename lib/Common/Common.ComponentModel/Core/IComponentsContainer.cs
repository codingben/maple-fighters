using System;
using System.Collections.Generic;

namespace Common.ComponentModel.Core
{
    public interface IComponentsContainer : IDisposable
    {
        void Add<TComponent>(TComponent component)
            where TComponent : class;

        void AddExposedOnly<TComponent>(TComponent component)
            where TComponent : class;

        TComponent Remove<TComponent>()
            where TComponent : class;

        TComponent Find<TComponent>()
            where TComponent : class;

        TComponent FindExposedOnly<TComponent>()
            where TComponent : class;

        IEnumerable<object> GetAll();
    }
}