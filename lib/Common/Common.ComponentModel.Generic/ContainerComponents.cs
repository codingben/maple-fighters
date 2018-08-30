using System;
using Common.ComponentModel.Common;

namespace Common.ComponentModel.Generic
{
    public interface IEntity<out TOwner>
        where TOwner : IEntity<TOwner>
    {
        IContainerComponents<TOwner> Owner { get; }
    }

    public sealed class ContainerComponents<TOwner> : IContainerComponents<TOwner>
        where TOwner : IEntity<TOwner>
    {
        private readonly TOwner owner;
        private readonly IComponentsCollection components = new ComponentsCollection();

        public ContainerComponents(TOwner owner)
        {
            this.owner = owner;
        }

        public T AddComponent<T>(T component)
            where T : IComponent<TOwner>
        {
            if (components.Add(component) != null)
            {
                if (component is IComponent<TOwner> baseComponent)
                {
                    baseComponent.Awake(owner);
                }

                return component;
            }

            throw new ComponentModelException($"Failed to add {component.GetType().Name} component.");
        }

        public void RemoveComponent<T>()
            where T : IComponent<TOwner>
        {
            var component = components.Remove<T>();
            if (component is IComponent<TOwner> baseComponent)
            {
                baseComponent.Dispose();
            }
        }

        public T GetComponent<T>()
            where T : class
        {
            return components.Get<T>();
        }

        public void Dispose()
        {
            foreach (var component in components.GetAll())
            {
                if (component is IDisposable disposableComponent)
                {
                    disposableComponent.Dispose();
                }
            }

            components.Dispose();
        }

        public int Count()
        {
            return components.Count();
        }

        public IComponentsCollection GetComponentsCollection() => components;
    }
}