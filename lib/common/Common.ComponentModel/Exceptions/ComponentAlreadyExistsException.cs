using System;

namespace Common.ComponentModel.Exceptions
{
    public class ComponentAlreadyExistsException : Exception
    {
        public ComponentAlreadyExistsException(string componentName)
            : base($"The {componentName} component already exists!")
        {
            // Left blank intentionally
        }
    }
}