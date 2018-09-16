using System;

namespace Common.ComponentModel.Exceptions
{
    public class ComponentNotExposedException<TComponent> : Exception
        where TComponent : class
    {
        public ComponentNotExposedException()
            : base(
                $"The component {typeof(TComponent).Name} should be exposed.")
        {
            // Left blank intentionally
        }
    }
}