using System;

namespace Common.ComponentModel.Exceptions
{
    public class ComponentNotFoundException : Exception
    {
        public ComponentNotFoundException(string componentName)
            : base($"The {componentName} component was not found.")
        {
            // Left blank intentionally
        }
    }
}