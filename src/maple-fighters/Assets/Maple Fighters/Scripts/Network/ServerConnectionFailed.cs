using System;

namespace Scripts.Services
{
    public class ServerConnectionFailed : Exception
    {
        public ServerConnectionFailed(string reason)
            : base(reason)
        {
            // Left blank intentionally
        }
    }
}