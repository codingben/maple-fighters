using System;

namespace Common.ComponentModel.Exceptions
{
    public class ComponentInterfaceExpectedException : Exception
    {
        public ComponentInterfaceExpectedException(string componentName)
            : base($"The called component {componentName} is not via interface.")
        {
            // Left blank intentionally
        }
    }
}