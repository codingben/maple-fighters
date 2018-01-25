using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;

namespace ComponentModel.Common
{
    public sealed class Container : IContainer
    {
        private readonly IContainer owner;
        private readonly List<IComponent> components = new List<IComponent>();

        public Container()
        {
            owner = this;
        }

        public T AddComponent<T>(T component)
            where T : IComponent
        {
            var isExists = components.OfType<T>().FirstOrDefault();
            if (isExists != null)
            {
                var componentName = typeof(T).Name;
                LogUtils.Log(MessageBuilder.Trace($"Component {componentName} already exists!"));
                return default(T);
            }

            component.Awake(owner);
            components.Add(component);
            return component;
        }

        public void RemoveComponent<T>()
            where T : IComponent
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
                var component = components.OfType<T>().FirstOrDefault();
                return component;
            }

            var componentName = typeof(T).Name;
            LogUtils.Log(MessageBuilder.Trace($"Could not get a {componentName} via not interface type."));
            return default(T);
        }

        public void Dispose()
        {
            var componentsListTemp = new List<IComponent>();
            componentsListTemp.AddRange(components);

            foreach (var component in componentsListTemp)
            {
                components.Remove(component);
            }

            componentsListTemp.ForEach(component => component.Dispose());
            componentsListTemp.Clear();
        }
    }

    public sealed class Container<TOwner> : IContainer<TOwner>
        where TOwner : IEntity
    {
        private readonly TOwner owner;
        private readonly List<IComponent<TOwner>> components = new List<IComponent<TOwner>>();

        public Container(TOwner owner)
        {
            this.owner = owner;
        }

        public T AddComponent<T>(T component)
            where T : IComponent<TOwner>
        {
            var isExists = components.OfType<T>().FirstOrDefault();
            if (isExists != null)
            {
                var componentName = typeof(T).Name;
                LogUtils.Log(MessageBuilder.Trace($"Component {componentName} already exists!"));
                return default(T);
            }

            component.Awake(owner);
            components.Add(component);
            return component;
        }

        public void RemoveComponent<T>()
            where T : IComponent<TOwner>
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
                var component = components.OfType<T>().FirstOrDefault();
                return component;
            }

            var componentName = typeof(T).Name;
            LogUtils.Log(MessageBuilder.Trace($"Could not get a {componentName} via not interface type."));
            return default(T);
        }

        public void Dispose()
        {
            var componentsListTemp = new List<IComponent<TOwner>>();
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