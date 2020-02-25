using System;

namespace ServerCommon.Application.Exceptions
{
    public class ServerComponentsAlreadyInitializedException : Exception
    {
        public ServerComponentsAlreadyInitializedException()
            : base("The server components provider has already been initialized.")
        {
            // Left blank intentionally
        }
    }
}