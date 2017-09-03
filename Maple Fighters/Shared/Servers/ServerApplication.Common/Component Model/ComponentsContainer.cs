using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;

namespace ServerApplication.Common.ComponentModel
{
    public sealed class ComponentsContainer : IComponentsContainer
    {
        private readonly List<object> components = new List<object>();

        public T AddComponent<T>(T component)
            where T : IComponent
        {
            components.Add(component);

            return GetComponent<T>();
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

        public void RemoveAllComponents()
        {
            foreach (var component in components)
            {
                (component as IComponent).Dispose();

                components.Remove(component);
            }
        }
    }
}