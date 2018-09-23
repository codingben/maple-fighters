using System;

namespace ServerCommon.Logging
{
    public class LoggerNotFoundException : Exception
    {
        public LoggerNotFoundException()
            : base("Could not get the logger from the log manager.")
        {
            // Left blank intentionally
        }
    }
}