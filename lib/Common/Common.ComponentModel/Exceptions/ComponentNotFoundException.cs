using System;

namespace Common.ComponentModel.Exceptions
{
    public class ComponentNotFoundException<TComponent> : Exception
        where TComponent : class
    {
        public ComponentNotFoundException()
            : base($"The {typeof(TComponent).Name} component was not found.")
        {
            // Left blank intentionally
        }
    }
}