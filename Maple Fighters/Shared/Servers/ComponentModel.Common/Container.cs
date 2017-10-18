using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;

namespace ComponentModel.Common
{
    public sealed class Container<TOwner> : IContainer<TOwner>
       where TOwner : IEntity
    {
        private readonly List<IComponent> components = new List<IComponent>();
        private readonly TOwner owner;

        public Container(TOwner owner)
        {
            this.owner = owner;
        }

        public T AddComponent<T>(T component)
            where T : Component<TOwner>, IComponent
        {
            if (GetComponent<T>() != null)
            {
                var componentName = typeof(T).Name;
                LogUtils.Log(MessageBuilder.Trace($"Comoponent {componentName} already exists!"), LogMessageType.Error);
                return null;
            }

            component.Awake(owner);
            components.Add(component);
            return component;
        }

        public void RemoveComponent<T>()
            where T : IComponent
        {
            var component = GetComponent<T>();
            if (component == null)
            {
                var componentName = typeof(T).Name;
                LogUtils.Log(MessageBuilder.Trace($"Could not remove component {componentName} - It doesn't exist."), LogMessageType.Warning);
                return;
            }

            var index = components.IndexOf(component);
            if (index != -1)
            {
                components.RemoveAt(index);
            }
        }

        public T GetComponent<T>()
            where T : IComponent
        {
            return components.OfType<T>().FirstOrDefault();
        }

        public void Dispose()
        {
            components.ForEach(component => component.Dispose());

            var componentsListTemp = new List<IComponent>();
            componentsListTemp.AddRange(components);

            foreach (var component in componentsListTemp)
            {
                components.Remove(component);
            }
        }
    }
}