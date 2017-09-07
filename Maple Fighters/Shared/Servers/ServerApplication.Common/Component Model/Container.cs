using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;

namespace ServerApplication.Common.ComponentModel
{
    public sealed class Container<TComoponent> : IContainer
        where TComoponent : class, IComponent
    {
        private readonly List<TComoponent> components = new List<TComoponent>();

        public T AddComponent<T>(T component)
            where T : class , IComponent
        {
            if (GetComponent<T>() != null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Comoponent {typeof(T).Name} already exists!"), LogMessageType.Error);
                return null;
            }

            components.Add(component as TComoponent);
            return component;
        }

        public void RemoveComponent<T>()
            where T : IComponent
        {
            var component = GetComponent<T>();
            if (component == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not remove component {typeof(T).Name} - It doesn't exist."), LogMessageType.Warning);
                return;
            }

            var index = components.IndexOf(component as TComoponent);
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

        public void RemoveAllComponents()
        {
            components.ForEach(component => component.Dispose());

            var componentsListTemp = new List<TComoponent>();
            componentsListTemp.AddRange(components);

            foreach (var component in componentsListTemp)
            {
                components.Remove(component);
            }
        }
    }

    /*public sealed class Container<TComoponent, TOwner> : IContainer<TOwner>
        where TComoponent : class, IComponent
    {
        private readonly List<TComoponent> components = new List<TComoponent>();

        public T AddComponent<T>(T component)
            where T : Component<TOwner>, IComponent
        {
            if (GetComponent<T>() != null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Comoponent {typeof(T).Name} already exists!"), LogMessageType.Error);
                return null;
            }

            components.Add(component as TComoponent);
            return component;
        }

        public void RemoveComponent<T>()
            where T : IComponent
        {
            var component = GetComponent<T>();
            if (component == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not remove component {typeof(T).Name} - It doesn't exist."), LogMessageType.Warning);
                return;
            }

            var index = components.IndexOf(component as TComoponent);
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

        public void RemoveAllComponents()
        {
            components.ForEach(component => component.Dispose());

            var componentsListTemp = new List<TComoponent>();
            componentsListTemp.AddRange(components);

            foreach (var component in componentsListTemp)
            {
                components.Remove(component);
            }
        }
    }*/
}