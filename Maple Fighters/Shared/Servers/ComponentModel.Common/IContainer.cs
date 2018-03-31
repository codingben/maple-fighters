using System;

namespace ComponentModel.Common
{
    public interface IContainer : IDisposable
    {
        T AddComponent<T>(T component)
            where T : IComponent;

        void RemoveComponent<T>()
            where T : IComponent;

        T GetComponent<T>()
            where T : IExposableComponent;
    }

    public interface IContainer<out TOwner> : IDisposable
        where TOwner : IEntity<TOwner>
    {
        T AddComponent<T>(T component)
            where T : IComponent<TOwner>;

        void RemoveComponent<T>()
            where T : IComponent<TOwner>;

        T GetComponent<T>()
            where T : IExposableComponent;
    }
}