using System;

namespace Common.ComponentModel
{
    public interface IComponentsProvider : IDisposable
    {
        TComponent Add<TComponent>(TComponent component)
            where TComponent : class;

        void Remove<TComponent>()
            where TComponent : class;

        TComponent Get<TComponent>()
            where TComponent : class;
    }
}