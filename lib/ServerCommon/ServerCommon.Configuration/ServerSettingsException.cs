using System;

namespace ServerCommon.Configuration
{
    public class ServerSettingsException : Exception
    {
        public ServerSettingsException(string message)
            : base(message)
        {
            // Left blank intentionally
        }
    }
}