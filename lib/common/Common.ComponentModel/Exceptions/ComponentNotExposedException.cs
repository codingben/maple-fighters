using System;

namespace Common.ComponentModel.Exceptions
{
    public class ComponentNotExposedException : Exception
    {
        public ComponentNotExposedException(string componentName)
            : base(
                $"The component {componentName} should be exposed.")
        {
            // Left blank intentionally
        }
    }
}