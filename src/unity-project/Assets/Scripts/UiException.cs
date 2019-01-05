using System;

namespace UserInterface
{
    public class UiException : Exception
    {
        public UiException(string message)
            : base(message)
        {
            // Left blank intentionally
        }
    }
}