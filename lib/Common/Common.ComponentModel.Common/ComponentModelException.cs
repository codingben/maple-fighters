using System;

namespace Common.ComponentModel.Common
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