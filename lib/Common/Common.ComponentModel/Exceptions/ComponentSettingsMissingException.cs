using System;

namespace Common.ComponentModel.Exceptions
{
    public class ComponentSettingsMissingException<TComponent> : Exception
        where TComponent : class
    {
        public ComponentSettingsMissingException()
            : base(
                $"A component {typeof(TComponent).Name} does not have component settings!")
        {
            // Left blank intentionally
        }
    }
}