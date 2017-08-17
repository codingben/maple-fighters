using System.Collections.Generic;
using System.Linq;

namespace ServerApplication.Common.ComponentModel
{
    public sealed class ComponentsContainer : IComponentsContainer
    {
        private readonly List<object> components = new List<object>();

        public void AddComponent<T>(T component) 
            where T : class, IComponent
        {
            components.Add(component);
        }

        public void RemoveComponent<T>(T component) 
            where T : class, IComponent
        {
            components.Remove(component);
        }

        public void GetComponent<T>(out T component)
            where T : class, IComponent
        {
            component = components.OfType<T>().FirstOrDefault();
        }
    }
}