using System;

namespace ServerCommon.Application
{
    public class ServerApplicationException : Exception
    {
        public ServerApplicationException(string message)
            : base(message)
        {
            // Left blank intentionally
        }
    }
}