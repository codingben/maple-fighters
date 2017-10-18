using System;

namespace ComponentModel.Common
{
    public interface IContainer<TOwner> : IDisposable
        where TOwner : IEntity
    {
        T AddComponent<T>(T component)
            where T : Component<TOwner>;

        void RemoveComponent<T>()
            where T : Component<TOwner>;

        T GetComponent<T>()
            where T : IExposableComponent;
    }
}