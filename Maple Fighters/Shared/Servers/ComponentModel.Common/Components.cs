using System;
using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;

namespace ComponentModel.Common
{
    public class Components : IComponents
    {
        private readonly List<IDisposable> components = new List<IDisposable>();

        public T Add<T>(T component)
            where T : IDisposable
        {
            var isExists = (components.OfType<T>().FirstOrDefault()) != null;
            if (isExists)
            {
                var componentName = typeof(T).Name;
                LogUtils.Log(MessageBuilder.Trace($"Component {componentName} already exists!"));
                return default(T);
            }

            components.Add(component);
            return component;
        }

        public void Remove<T>() 
            where T : IDisposable
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

        public T Get<T>()
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
            var componentsListTemp = new List<IDisposable>();
            componentsListTemp.AddRange(components);
            componentsListTemp.ForEach(component => component.Dispose());

            foreach (var component in componentsListTemp)
            {
                components.Remove(component);
            }

            componentsListTemp.Clear();
        }

        public int Count()
        {
            return components.Count;
        }
    }
}