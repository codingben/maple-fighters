using System;

namespace ServerCommon.PeerLogic
{
    public class PeerLogicException : Exception
    {
        public PeerLogicException(string message)
            : base(message)
        {
            // Left blank intentionally
        }
    }
}