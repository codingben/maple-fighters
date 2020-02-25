using System;

namespace Common.ComponentModel.Exceptions
{
    public class ComponentSettingsMissingException : Exception
    {
        public ComponentSettingsMissingException(string componentName)
            : base($"The component {componentName} has no settings!")
        {
            // Left blank intentionally
        }
    }
}