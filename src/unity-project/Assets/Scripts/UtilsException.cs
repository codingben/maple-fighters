using System;

namespace UI.Manager
{
    public class UtilsException : Exception
    {
        public UtilsException(string message)
            : base(message)
        {
            // Left blank intentionally
        }
    }
}