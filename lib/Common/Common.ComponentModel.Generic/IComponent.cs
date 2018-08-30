using System;

namespace Common.ComponentModel.Generic
{
    public interface IComponent<in TOwner> : IDisposable
        where TOwner : IEntity<TOwner>
    {
        void Awake(TOwner entity);
    }
}