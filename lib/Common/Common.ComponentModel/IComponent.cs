using System;

namespace Common.ComponentModel
{
    public interface IComponent : IDisposable
    {
        void Awake(IComponentsProvider components);
    }
}