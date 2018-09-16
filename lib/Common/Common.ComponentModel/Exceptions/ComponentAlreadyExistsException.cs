using System;

namespace Common.ComponentModel.Exceptions
{
    public class ComponentAlreadyExistsException<TComponent> : Exception
        where TComponent : class
    {
        public ComponentAlreadyExistsException()
            : base($"The {typeof(TComponent).Name} component already exists!")
        {
            // Left blank intentionally
        }
    }
}