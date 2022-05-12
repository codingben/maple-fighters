using System;
using Serilog;

namespace Game.Logger
{
    public class InterestManagementLogger : InterestManagement.ILogger
    {
        public static InterestMgmtLogLevel LogLevel;

        public static InterestManagement.ILogger GetLogger()
        {
            if (logger == null)
            {
                logger = new InterestManagementLogger();
            }

            return logger;
        }

        private static InterestManagementLogger logger;

        public void Info(string message)
        {
            if (LogLevel >= InterestMgmtLogLevel.Info)
            {
                Log.Logger.Information("{0} [Info] {1}", DateTime.Now, message);
            }
        }
    }
}