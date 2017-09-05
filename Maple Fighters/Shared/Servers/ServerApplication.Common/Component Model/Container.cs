using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;

namespace ServerApplication.Common.ComponentModel
{
    public sealed class Container<TComoponent> : IContainer<TComoponent> 
        where TComoponent : class, IComponent
    {
        private readonly List<TComoponent> components = new List<TComoponent>();

        public void AddComponent(TComoponent component)
        {
            if (components.Contains(component))
            {
                LogUtils.Log(MessageBuilder.Trace($"Comoponent type - {typeof(TComoponent).Name} already exists."));
                return;
            }

            components.Add(component);
        }

        public void RemoveComponent<T>()
            where T : IComponent
        {
            var component = GetComponent<T>();
            if (component == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not remove component {typeof(T).Name} - it does not exist."));
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
            foreach (var component in components)
            {
                component.Dispose();

                components.Remove(component);
            }
        }
    }
}