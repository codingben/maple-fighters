using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;

namespace ComponentModel.Common
{
    public sealed class Container<TOwner> : IContainer<TOwner>
        where TOwner : IEntity
    {
        private readonly List<Component<TOwner>> components = new List<Component<TOwner>>();
        private readonly TOwner owner;

        public Container(TOwner owner)
        {
            this.owner = owner;
        }

        public T AddComponent<T>(T component)
            where T : Component<TOwner>
        {
            if (components.OfType<T>().FirstOrDefault() != null)
            {
                var componentName = typeof(T).Name;
                LogUtils.Log(MessageBuilder.Trace($"Component {componentName} already exists!"));
                return null;
            }

            component.Awake(owner);
            components.Add(component);
            return component;
        }

        public void RemoveComponent<T>()
            where T : Component<TOwner>
        {
            var component = components.OfType<T>().FirstOrDefault();
            if (component == null)
            {
                var componentName = typeof(T).Name;
                LogUtils.Log(MessageBuilder.Trace($"Could not remove component {componentName} because it does not exist."));
                return;
            }

            var index = components.IndexOf(component);
            if (index != -1)
            {
                components.RemoveAt(index);
            }

            component.Dispose();
        }

        public T GetComponent<T>()
            where T : IExposableComponent
        {
            if (typeof(T).IsInterface)
            {
                return components.OfType<T>().FirstOrDefault();
            }

            var componentName = typeof(T).Name;
            LogUtils.Log(MessageBuilder.Trace($"Could not get a {componentName} via not interface type."));
            return default(T);
        }

        public void Dispose()
        {
            var componentsListTemp = new List<Component<TOwner>>();
            componentsListTemp.AddRange(components);

            foreach (var component in componentsListTemp)
            {
                components.Remove(component);
            }

            componentsListTemp.ForEach(component => component.Dispose());
            componentsListTemp.Clear();
        }
    }
}