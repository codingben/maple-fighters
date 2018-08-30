using System;

namespace Common.ComponentModel
{
    public class ComponentModelException : Exception
    {
        public ComponentModelException(string message)
            : base(message)
        {
            // Left blank intentionally
        }
    }
}