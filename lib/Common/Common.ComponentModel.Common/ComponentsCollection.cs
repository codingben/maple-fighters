using System.Collections.Generic;
using System.Linq;

namespace Common.ComponentModel.Common
{
    public sealed class ComponentsCollection : IComponentsCollection
    {
        private readonly List<object> components;

        public ComponentsCollection()
        {
            components = new List<object>();
        }

        public T Add<T>(T component)
        {
            if (components.Contains(component))
            {
                throw new ComponentModelException($"Component {typeof(T).Name} already exists!");
            }

            components.Add(component);
            return component;
        }

        public T Remove<T>()
        {
            var component = components.OfType<T>().FirstOrDefault();
            if (component == null)
            {
                throw new ComponentModelException($"Could not remove component {typeof(T).Name} because it does not exist.");
            }

            var index = components.IndexOf(component);
            if (index != -1)
            {
                components.RemoveAt(index);
            }

            return component;
        }

        public T Get<T>()
            where T : class
        {
            if (!(typeof(T).IsInterface))
            {
                throw new ComponentModelException($"Could not get a {typeof(T).Name} via not interface type.");
            }

            var component = components.OfType<T>().FirstOrDefault();
            return component;
        }

        public IEnumerable<object> GetAll()
        {
            return components.AsReadOnly();
        }

        public void Dispose()
        {
            components.Clear();
        }

        public int Count()
        {
            return components.Count;
        }
    }
}