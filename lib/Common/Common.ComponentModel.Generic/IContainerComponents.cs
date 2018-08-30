using System;

namespace Common.ComponentModel.Generic
{
    public interface IContainerComponents<out TOwner> : IDisposable
        where TOwner : IEntity<TOwner>
    {
        T AddComponent<T>(T component)
            where T : IComponent<TOwner>;

        void RemoveComponent<T>()
            where T : IComponent<TOwner>;

        T GetComponent<T>()
            where T : class;

        int Count();
    }
}