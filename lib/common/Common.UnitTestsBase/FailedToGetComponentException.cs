using System;

namespace Common.UnitTestsBase
{
    public class FailedToGetComponentException : Exception
    {
        public FailedToGetComponentException(string componentName)
            : base($"Failed to get {componentName} component.")
        {
            // Left blank intentionally
        }
    }
}