using System;

namespace Scripts.Network
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