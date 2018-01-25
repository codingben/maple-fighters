using System;

namespace ComponentModel.Common
{
    public interface IComponent : IDisposable
    {
        void Awake(IContainer entity);
    }

    public interface IComponent<in TOwner> : IDisposable
        where TOwner : IEntity
    {
        void Awake(TOwner entity);
    }
}