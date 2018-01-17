using System;

namespace ComponentModel.Common
{
    public interface IContainer : IDisposable
    {
        T AddComponent<T>(T component)
            where T : Component;

        void RemoveComponent<T>()
            where T : Component;

        T GetComponent<T>()
            where T : IExposableComponent;
    }

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