using System;

namespace UserInterface
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