using System;

namespace ServerCommon.Application.Exceptions
{
    public class ServerComponentsNotInitializedException : Exception
    {
        public ServerComponentsNotInitializedException()
            : base("The server components provider is not initialized.")
        {
            // Left blank intentionally
        }
    }
}