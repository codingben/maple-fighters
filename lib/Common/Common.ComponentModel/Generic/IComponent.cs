using System;

namespace Common.ComponentModel.Generic
{
    public interface IComponent<in TOwner> : IDisposable
        where TOwner : class
    {
        void Awake(TOwner owner, IComponentsProvider components);
    }
}