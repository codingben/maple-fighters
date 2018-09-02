using System;

namespace ServerCommon.Application
{
    public class ApplicationException : Exception
    {
        public ApplicationException(string message)
            : base(message)
        {
            // Left blank intentionally
        }
    }
}