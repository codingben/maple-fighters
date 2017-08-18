using System.Collections.Generic;
using System.Linq;

namespace ServerApplication.Common.ComponentModel
{
    public sealed class ComponentsContainer : IComponentsContainer
    {
        private readonly List<object> components = new List<object>();

        public object AddComponent<T>(T component) 
            where T : class, IComponent
        {
            components.Add(component);

            return GetComponent<T>();
        }

        public void RemoveComponent<T>() 
            where T : class, IComponent
        {
            components.Remove(typeof(T));
        }

        public object GetComponent<T>()
            where T : class, IComponent
        {
            return components.OfType<T>().FirstOrDefault();
        }
    }
}